SELECT DISTINCT
s.libelle as Societe,
c.Code  as [CI],
c.CiId,
EC.EtablissementComptableId,
r.RapportId ,
rl.RapportLigneId,
rl.DatePointage AS [Date Pointage],
rl.HeureNormale,
rl.HeureMajoration,
rl.AvecChauffeur,
rl.DateSuppression,
rlt.HeureTache,
rlt.TacheId,
m.MaterielId,
m.Code,
m.IsStorm,
m.MaterielLocation,
m.RessourceId AS RessourceMaterielId,
RES.SousChapitreId,
SC.ChapitreId,
SRN.ReferentielEtenduId,
BEO.OrganisationId,
CASE WHEN M.IsStorm = 0 THEN BEO.BaremeId ELSE NULL END AS BaremeId,
CASE WHEN M.IsStorm = 1 THEN BEO.BaremeId ELSE NULL END AS BaremeStormId,
BEO.UniteId,
BEO.DeviseId,
CASE WHEN  rl.AvecChauffeur = 1 THEN  BEO.Prix  + BEO.PrixChauffeur ELSE BEO.Prix END AS PUHT,
rlt.HeureTache as Quantite,
CASE WHEN  rl.AvecChauffeur = 1 THEN  (BEO.Prix  + BEO.PrixChauffeur) * rlt.HeureTache  ELSE BEO.Prix * rlt.HeureTache END AS Montant
INTO #TEMP
FROM FRED_RAPPORT r
INNER JOIN FRED_RAPPORT_LIGNE rl ON r.RapportId = rl.RapportId
INNER JOIN FRED_RAPPORT_LIGNE_TACHE rlt  ON rlt.RapportLigneId = rl.RapportLigneId
INNER JOIN FRED_TACHE T ON T.TacheId = rlT.TacheId
INNER JOIN FRED_CI c ON c.CiId = r.CiId
INNER JOIN FRED_ETABLISSEMENT_COMPTABLE EC on C.EtablissementComptableId = EC.EtablissementComptableId
INNER JOIN FRED_SOCIETE s ON s.SocieteId = c.SocieteId

LEFT OUTER JOIN FRED_VALORISATION valoMateriel ON valoMateriel.RapportId = r.RapportId AND valoMateriel.RapportLigneId = rl.RapportLigneid AND rl.MaterielId = valoMateriel.MaterielId AND valoMateriel.montant > 0
LEFT JOIN FRED_MATERIEL m ON m.MaterielId = rl.MaterielId
INNER JOIN FRED_SOCIETE_RESSOURCE_NATURE SRN ON SRN.RessourceId = m.RessourceId and SRN.SocieteId = s.SocieteId 
INNER JOIN FRED_RESSOURCE RES ON RES.RessourceId = M.RessourceId
INNER JOIN FRED_SOUS_CHAPITRE SC ON SC.SousChapitreId = RES.SousChapitreId
LEFT JOIN FRED_BAREME_EXPLOITATION_ORGANISATION BEO ON BEO.OrganisationId = EC.OrganisationId AND BEO.RessourceId = SRN.RessourceId  and DATEFROMPARTS(YEAR(RL.DatePointage),MONTH(RL.DatePointage),1) >= BEO.PeriodeDebut  AND (BEO.PeriodeFin IS NULL OR (DATEFROMPARTS(YEAR(RL.DatePointage),MONTH(RL.DatePointage),1) < BEO.PeriodeFin))

WHERE 1=1
AND valoMateriel.ValorisationId IS NULL
AND (r.DateSuppression IS NULL AND rl.DateSuppression IS NULL)
AND S.CodeSocieteComptable IN ('RB','0001','0143','MBTP')
AND rlt.HeureTache> 0
AND ( (rl.MaterielId IS NOT NULL AND rl.MaterielId > 0))
AND rl.MaterielNomTemporaire IS NULL
AND NOT (rl.HeureNormale = 0 AND rl.HeureMajoration = 0)

INSERT INTO FRED_VALORISATION (CiId, RapportId, RapportLigneId, TacheId, ChapitreId, SousChapitreId, ReferentielEtenduId, BaremeId, BaremeStormId, UniteId, DeviseId, PersonnelId, MaterielId, Date, VerrouPeriode, DateCreation, Source, PUHT, Quantite, Montant)
SELECT t.CiId, t.RapportId, T.RapportLigneId, T.TacheId,T.ChapitreId, T.SousChapitreId, T.ReferentielEtenduId, T.BaremeId,T.BaremeStormId, T.UniteId, T.DeviseId, null as PersonnelId,T.MaterielId, T.[Date Pointage], 0 as VerrouPeriode,GETDATE() as DateCreation, 'Script Rapport' as Source, T.PUHT, T.Quantite, T.Montant
FROM #TEMP t

DROP TABLE #TEMP