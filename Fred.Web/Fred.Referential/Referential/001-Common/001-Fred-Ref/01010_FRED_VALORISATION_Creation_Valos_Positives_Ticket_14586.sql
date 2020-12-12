-- Personnel Intérimaire
DECLARE @DefaultDeviseId int
SELECT @DefaultDeviseId = DeviseId FROM FRED_DEVISE WHERE IsoCode = 'EUR'
SELECT DISTINCT
    s.libelle as Societe,
    s.SocieteId,
    c.Code  as [CI],
    c.CiId,
    r.RapportId ,
    rl.RapportLigneId,
    CAST(rl.DatePointage AS DATE) AS [Date Pointage],
    rl.HeureNormale,
    rl.HeureMajoration,
    rl.AvecChauffeur,
    rlt.HeureTache as Quantite,
    rlt.TacheId,
    RES.SousChapitreId,
    SC.ChapitreId,
    SRN.ReferentielEtenduId,
    P.PersonnelId,
    CCI.RessourceId,
    CCI.Valorisation as PUHT,
    CCI.DateCreation,
    CCI.UniteId,
    @DefaultDeviseId DeviseId,
    CCI.Valorisation * rlt.HeureTache as Montant
INTO #ValorisationV16
FROM
    FRED_RAPPORT r
    INNER JOIN FRED_RAPPORT_LIGNE rl ON
        r.RapportId = rl.RapportId
        AND r.CiId = rl.CiId
    INNER JOIN FRED_RAPPORT_LIGNE_TACHE rlt  ON rlt.RapportLigneId = rl.RapportLigneId
    INNER JOIN FRED_TACHE T ON T.TacheId = rlT.TacheId
    INNER JOIN FRED_CI c ON c.CiId = r.CiId
    INNER JOIN FRED_SOCIETE s ON s.SocieteId = c.SocieteId
    LEFT OUTER JOIN dbo.FRED_VALORISATION valoPersonnel ON 
        valoPersonnel.RapportId = r.RapportId
        AND valoPersonnel.RapportLigneId = rl.RapportLigneId
        AND rl.PersonnelId = valoPersonnel.PersonnelId
        AND valoPersonnel.montant > 0
    LEFT JOIN FRED_PERSONNEL P ON P.PersonnelId = rl.PersonnelId 
    INNER JOIN FRED_CONTRAT_INTERIMAIRE CCI ON
        CCI.InterimaireId = P.PersonnelId
        AND CAST(RL.DatePointage AS DATE) BETWEEN
            CCI.DateDebut
            AND DATEADD(DAY,cci.Souplesse,CCI.DateFin)
    LEFT JOIN FRED_RESSOURCE RES ON RES.RessourceId = CCI.RessourceId
    LEFT JOIN FRED_SOUS_CHAPITRE SC ON SC.SousChapitreId = RES.SousChapitreId
    LEFT JOIN FRED_SOCIETE_RESSOURCE_NATURE SRN ON
        SRN.RessourceId = CCI.RessourceId
        AND SRN.SocieteId = s.SocieteId 
WHERE
	rl.RapportLigneId IN (1253772, 1254499, 1255262 , 1256417 , 1257175)
	AND valoPersonnel.ValorisationId IS NULL 
    AND (r.DateSuppression IS NULL AND rl.DateSuppression IS NULL)
    AND S.Code IN ('RB')
    AND rlt.HeureTache> 0
    AND rl.PersonnelId IS NOT NULL 
    AND rl.PersonnelId <> 0
    AND rl.PrenomNomTemporaire IS NULL 
    AND P.IsInterimaire = 1
    AND NOT (rl.HeureNormale = 0 AND rl.HeureMajoration = 0)
INSERT INTO FRED_VALORISATION (
    CiId,
    RapportId,
    RapportLigneId,
    TacheId,
    ChapitreId,
    SousChapitreId,
    ReferentielEtenduId,
    UniteId,
    DeviseId,
    PersonnelId,
    Date,
    VerrouPeriode,
    DateCreation,
    Source,
    PUHT,
    Quantite,
    Montant
) SELECT
    t.CiId,
    t.RapportId,
    T.RapportLigneId,
    T.TacheId,
    T.ChapitreId,
    T.SousChapitreId,
    T.ReferentielEtenduId,
    T.UniteId,
    T.DeviseId,
    T.PersonnelId as PersonnelId,
    T.[Date Pointage],
    1 as VerrouPeriode,
    GETDATE() as DateCreation,
    'Script Interim Ticket 14586' as Source,
    T.PUHT,
    T.Quantite,
    T.Montant
FROM #ValorisationV16 t
WHERE T.ReferentielEtenduId IS NOT NULL;
DROP TABLE #ValorisationV16