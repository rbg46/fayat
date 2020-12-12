-- =======================================================================================================================================
-- Description :	1/ Matricule I_14390181
--						On recrée la valorisation intérimaire du rapportLigneId = 813908 (valeur 224€ manquante)
--					2/ Matricule I_14390458
--						La valo positive n'existe pas pour le rapportLigneId = 1325661 (valeur 332,50€ manquante)
-- =======================================================================================================================================

DECLARE @EuroDeviseId int
SELECT @EuroDeviseId = DeviseId FROM FRED_DEVISE WHERE IsoCode = 'EUR'

-- Liste des rapports ligne concernés
DECLARE @RapportLigneIdsToUpdate TABLE (Id INT);
INSERT INTO @RapportLigneIdsToUpdate VALUES (1325661), (813908);

SELECT DISTINCT
    c.CiId,
    r.RapportId ,
    rl.RapportLigneId,
    rl.DatePointage,
    rlt.HeureTache as Quantite,
    rlt.TacheId,
    RES.SousChapitreId,
    SC.ChapitreId,
    SRN.ReferentielEtenduId,
    P.PersonnelId,
    CCI.Valorisation as PUHT,
    CCI.UniteId,
    @EuroDeviseId DeviseId,
    CCI.Valorisation * rlt.HeureTache as Montant
INTO #ValorisationToCreate
FROM
    @RapportLigneIdsToUpdate RLITU
    INNER JOIN FRED_RAPPORT_LIGNE rl ON rl.RapportLigneId = RLITU.Id
    INNER JOIN FRED_RAPPORT_LIGNE_TACHE rlt  ON rlt.RapportLigneId = rl.RapportLigneId
    INNER JOIN FRED_TACHE T ON T.TacheId = rlT.TacheId AND rlt.HeureTache > 0
    INNER JOIN FRED_RAPPORT r ON r.RapportId = rl.RapportId
    INNER JOIN FRED_CI c ON c.CiId = r.CiId
    INNER JOIN FRED_SOCIETE s ON s.SocieteId = c.SocieteId AND s.Code = 'RB'
    INNER JOIN FRED_PERSONNEL P ON P.PersonnelId = rl.PersonnelId AND P.IsInterimaire = 1
    INNER JOIN FRED_CONTRAT_INTERIMAIRE CCI ON
        CCI.InterimaireId = P.PersonnelId
        AND CAST(RL.DatePointage AS DATE) BETWEEN
            CCI.DateDebut
            AND DATEADD(DAY,cci.Souplesse,CCI.DateFin)
    INNER JOIN FRED_RESSOURCE RES ON RES.RessourceId = CCI.RessourceId
    INNER JOIN FRED_SOUS_CHAPITRE SC ON SC.SousChapitreId = RES.SousChapitreId
    INNER JOIN FRED_SOCIETE_RESSOURCE_NATURE SRN ON
        SRN.RessourceId = CCI.RessourceId
        AND SRN.SocieteId = s.SocieteId 
        AND SRN.ReferentielEtenduId IS NOT NULL
    LEFT JOIN dbo.FRED_VALORISATION valoPersonnel ON 
        valoPersonnel.RapportId = r.RapportId
        AND valoPersonnel.RapportLigneId = rl.RapportLigneId
        AND rl.PersonnelId = valoPersonnel.PersonnelId
        AND valoPersonnel.montant > 0
WHERE
    valoPersonnel.ValorisationId IS NULL 

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
    T.PersonnelId,
    T.DatePointage,
    1 as VerrouPeriode,
    GETDATE(),
    'Script Interim Ticket 14895',
    T.PUHT,
    T.Quantite,
    T.Montant
FROM #ValorisationToCreate t 
    
DROP TABLE #ValorisationToCreate