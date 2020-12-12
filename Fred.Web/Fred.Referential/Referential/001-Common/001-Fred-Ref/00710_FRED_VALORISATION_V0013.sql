DECLARE @DefaultDeviseId int, @DefaultUniteId int
Select @DefaultDeviseId = DeviseId FROM FRED_DEVISE WHERE IsoCode = 'EUR'
Select @DefaultUniteId = UniteId FROM FRED_UNITE WHERE  Code = 'H'


INSERT INTO FRED_BAREME_EXPLOITATION_CI(CIId,ReferentielEtenduId, UniteId, DeviseId, Statut, PeriodeDebut, PeriodeFin, Prix, PrixChauffeur, AuteurCreationId, DateCreation)
SELECT DISTINCT R.CiId, SRN.ReferentielEtenduId,@DefaultUniteId, @DefaultDeviseId,0, DATEFROMPARTS(YEAR(RL.DatePointage),MONTH(RL.DatePointage),1),NULL,0.01,30,1,GETDATE()
FROM FRED_RAPPORT r
INNER JOIN FRED_RAPPORT_LIGNE rl ON r.RapportId = rl.RapportId and r.CiId = rl.CiId
INNER JOIN FRED_RAPPORT_LIGNE_TACHE rlt  ON rlt.RapportLigneId = rl.RapportLigneId
INNER JOIN FRED_TACHE T ON T.TacheId = rlT.TacheId
INNER JOIN FRED_CI c ON c.CiId = r.CiId
INNER JOIN FRED_ETABLISSEMENT_COMPTABLE EC on C.EtablissementComptableId = EC.EtablissementComptableId
INNER JOIN FRED_SOCIETE s ON s.SocieteId = c.SocieteId
LEFT OUTER JOIN dbo.FRED_VALORISATION valoPersonnel on valoPersonnel.RapportId = r.RapportId and valoPersonnel.RapportLigneId = rl.RapportLigneId and rl.PersonnelId = valoPersonnel.PersonnelId AND valoPersonnel.montant > 0
LEFT JOIN FRED_PERSONNEL P ON P.PersonnelId = rl.PersonnelId 
LEFT JOIN FRED_RESSOURCE RES ON RES.RessourceId = P.RessourceId
INNER JOIN FRED_SOUS_CHAPITRE SC ON SC.SousChapitreId = RES.SousChapitreId
INNER JOIN FRED_SOCIETE_RESSOURCE_NATURE SRN ON SRN.RessourceId = P.RessourceId and SRN.SocieteId = s.SocieteId 

OUTER APPLY( 
			SELECT TOP 1 
				BEC.BaremeId,
				BEC.ReferentielEtenduId,
				CASE WHEN S.Prix IS NOT NULL THEN S.Prix ELSE BEC.Prix END AS Prix,
				CASE WHEN S.PrixChauffeur IS NOT NULL THEN S.PrixChauffeur ELSE BEC.PrixChauffeur END AS PrxChauffeur,
				CASE WHEN S.PrixConduite IS NOT NULL THEN S.PrixConduite ELSE BEC.PrixConduite END AS PrixConduite,
				CASE WHEN S.DeviseId IS NOT NULL THEN S.DeviseId ELSE BEC.DeviseId END As DeviseId,
				CASE WHEN S.UniteId IS NOT NULL THEN S.UniteId ELSE BEC.UniteId END As UniteId

				FROM FRED_BAREME_EXPLOITATION_CI BEC 
				LEFT join FRED_BAREME_EXPLOITATION_CI_SURCHARGE S on BEC.CIId = s.CIId and bec.ReferentielEtenduId = s.ReferentielEtenduId and bec.PeriodeFin = s.PeriodeDebut and bec.PeriodeFin = bec.PeriodeFin
				WHERE BEC.CIId = RL.CiId AND BEC.ReferentielEtenduId = SRN.ReferentielEtenduId  
				AND DATEFROMPARTS(YEAR(RL.DatePointage),MONTH(RL.DatePointage),1) >= BEC.PeriodeDebut  AND (BEC.PeriodeFin IS NULL OR (DATEFROMPARTS(YEAR(RL.DatePointage),MONTH(RL.DatePointage),1) < BEC.PeriodeFin))
			)BEC

WHERE 1=1
AND BEC.BaremeId is null
AND valoPersonnel.ValorisationId IS NULL 
AND (r.DateSuppression IS NULL AND rl.DateSuppression IS NULL)
AND c.SocieteId IN (72,1,60,61)
AND rlt.HeureTache> 0
AND rl.PersonnelId IS NOT NULL 
AND rl.PersonnelId <> 0
AND rl.PrenomNomTemporaire IS NULL 
AND P.IsInterimaire = 0
AND NOT (rl.HeureNormale = 0 AND rl.HeureMajoration = 0)

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
rlt.HeureTache as Quantite,
rlt.TacheId,

RES.SousChapitreId,
SC.ChapitreId,
SRN.ReferentielEtenduId,

P.PersonnelId,
P.RessourceId, 

BEC.Prix as PUHT,
BEC.UniteId,
BEC.Prix * rlt.HeureTache as Montant,
BEC.BaremeId AS BaremeId,
BEC.DeviseId

INTO #TEMP
FROM FRED_RAPPORT r
INNER JOIN FRED_RAPPORT_LIGNE rl ON r.RapportId = rl.RapportId and r.CiId = rl.CiId
INNER JOIN FRED_RAPPORT_LIGNE_TACHE rlt  ON rlt.RapportLigneId = rl.RapportLigneId
INNER JOIN FRED_TACHE T ON T.TacheId = rlT.TacheId
INNER JOIN FRED_CI c ON c.CiId = r.CiId
INNER JOIN FRED_ETABLISSEMENT_COMPTABLE EC on C.EtablissementComptableId = EC.EtablissementComptableId
INNER JOIN FRED_SOCIETE s ON s.SocieteId = c.SocieteId
LEFT OUTER JOIN dbo.FRED_VALORISATION valoPersonnel on valoPersonnel.RapportId = r.RapportId and valoPersonnel.RapportLigneId = rl.RapportLigneId and rl.PersonnelId = valoPersonnel.PersonnelId AND valoPersonnel.montant > 0
LEFT JOIN FRED_PERSONNEL P ON P.PersonnelId = rl.PersonnelId 
LEFT JOIN FRED_RESSOURCE RES ON RES.RessourceId = P.RessourceId
INNER JOIN FRED_SOUS_CHAPITRE SC ON SC.SousChapitreId = RES.SousChapitreId
LEFT JOIN FRED_SOCIETE_RESSOURCE_NATURE SRN ON SRN.RessourceId = P.RessourceId and SRN.SocieteId = s.SocieteId 

CROSS APPLY( 
			SELECT TOP 1 
				BEC.BaremeId,
				BEC.ReferentielEtenduId,
				CASE WHEN S.Prix IS NOT NULL THEN S.Prix ELSE BEC.Prix END AS Prix,
				CASE WHEN S.PrixChauffeur IS NOT NULL THEN S.PrixChauffeur ELSE BEC.PrixChauffeur END AS PrxChauffeur,
				CASE WHEN S.PrixConduite IS NOT NULL THEN S.PrixConduite ELSE BEC.PrixConduite END AS PrixConduite,
				CASE WHEN S.DeviseId IS NOT NULL THEN S.DeviseId ELSE BEC.DeviseId END As DeviseId,
				CASE WHEN S.UniteId IS NOT NULL THEN S.UniteId ELSE BEC.UniteId END As UniteId

				FROM FRED_BAREME_EXPLOITATION_CI BEC 
				LEFT join FRED_BAREME_EXPLOITATION_CI_SURCHARGE S on BEC.CIId = s.CIId and bec.ReferentielEtenduId = s.ReferentielEtenduId and bec.PeriodeFin = s.PeriodeDebut and bec.PeriodeFin = bec.PeriodeFin
				WHERE BEC.CIId = RL.CiId AND BEC.ReferentielEtenduId = SRN.ReferentielEtenduId  
				AND DATEFROMPARTS(YEAR(RL.DatePointage),MONTH(RL.DatePointage),1) >= BEC.PeriodeDebut  AND (BEC.PeriodeFin IS NULL OR (DATEFROMPARTS(YEAR(RL.DatePointage),MONTH(RL.DatePointage),1) < BEC.PeriodeFin))
			)BEC

WHERE 1=1
AND valoPersonnel.ValorisationId IS NULL 
AND (r.DateSuppression IS NULL AND rl.DateSuppression IS NULL)
AND S.CodeSocieteComptable IN ('RB','0001','0143','MBTP')
AND rlt.HeureTache> 0
AND rl.PersonnelId IS NOT NULL 
AND rl.PersonnelId <> 0
AND rl.PrenomNomTemporaire IS NULL 
AND P.IsInterimaire = 1
AND NOT (rl.HeureNormale = 0 AND rl.HeureMajoration = 0)


INSERT INTO FRED_VALORISATION (CiId, RapportId, RapportLigneId, TacheId, ChapitreId, SousChapitreId, ReferentielEtenduId, BaremeId, UniteId, DeviseId, PersonnelId, Date, VerrouPeriode, DateCreation, Source, PUHT, Quantite, Montant)
SELECT t.CiId, t.RapportId, T.RapportLigneId, T.TacheId,T.ChapitreId, T.SousChapitreId, T.ReferentielEtenduId, T.BaremeId, T.UniteId, T.DeviseId, T.PersonnelId as PersonnelId, T.[Date Pointage], 0 as VerrouPeriode,GETDATE() as DateCreation, 'Script Personnel' as Source, T.PUHT, T.Quantite, T.Montant
FROM #TEMP t
DROP TABLE #TEMP 