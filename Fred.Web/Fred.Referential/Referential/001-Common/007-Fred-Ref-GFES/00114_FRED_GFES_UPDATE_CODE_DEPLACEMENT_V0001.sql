
-- 16-01-2019
-- Préparation de MEP pour FES 
-- Update pour la table FRED_CODE_DEPLACEMENT .

-- Suppression de tous les codes de déplacement pour un update des codes (Les codes ont changé , Ex avant : Zx aprés : IPDx)

-- Dépendances de la table : FRED_CODE_DEPLACEMENT

-- FRED_INDEMNITE_DEPLACEMENT
UPDATE I
SET 
	I.CodeDeplacementId = NULL
FROM FRED_INDEMNITE_DEPLACEMENT I
INNER JOIN FRED_CODE_DEPLACEMENT cd on I.CodeDeplacementId = cd.CodeDeplacementId
INNER JOIN FRED_SOCIETE soc on cd.SocieteId = soc.SocieteId
INNER JOIN FRED_GROUPE gr on soc.GroupeId = gr.GroupeId
WHERE gr.Code LIKE '%GFES%'


-- FRED_POINTAGE_ANTICIPE
UPDATE PA
	SET PA.CodeDeplacementId = NULL
FROM FRED_POINTAGE_ANTICIPE PA
INNER JOIN FRED_CODE_DEPLACEMENT cd on pa.CodeDeplacementId = cd.CodeDeplacementId
INNER JOIN FRED_SOCIETE soc on cd.SocieteId = soc.SocieteId
INNER JOIN FRED_GROUPE gr on soc.GroupeId = gr.GroupeId
WHERE gr.Code LIKE '%GFES%'


-- FRED_RAPPORT_LIGNE
UPDATE rl
SET 
	rl.CodeDeplacementId = NULL
FROM FRED_RAPPORT_LIGNE rl
INNER JOIN FRED_CODE_DEPLACEMENT cd on rl.CodeDeplacementId = cd.CodeDeplacementId
INNER JOIN FRED_SOCIETE soc on cd.SocieteId = soc.SocieteId
INNER JOIN FRED_GROUPE gr on soc.GroupeId = gr.GroupeId
WHERE gr.Code LIKE '%GFES%'

DELETE CD
FROM 
	[FRED_CODE_DEPLACEMENT] CD
INNER JOIN FRED_SOCIETE soc on CD.SocieteId = soc.SocieteId
INNER JOIN FRED_GROUPE gr on soc.GroupeId = gr.GroupeId
WHERE gr.Code LIKE '%GFES%'



-- Insertion des nouveaux codes

SELECT DISTINCT Code
into #tmpTableSoc 
FROM FRED_SOCIETE WHERE GroupeId = (SELECT GroupeId FROM FREd_GROUPE where Code = 'GFES')

Declare @tmpCode nvarchar(20);
WHILE exists (select * from #tmpTableSoc)
BEGIN

	select top 1 @tmpCode = Code from #tmpTableSoc order by Code asc;

	EXEC Fred_ToolBox_Code_Deplacement @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='IPD1', @Libelle='IPD - Zone 1', @KmMini=0, @KmMaxi=10, @IGD='Non', @IndemniteForfaitaire='Non', @Actif='Oui';
	EXEC Fred_ToolBox_Code_Deplacement @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='IPD2', @Libelle='IPD - Zone 2', @KmMini=10, @KmMaxi=20, @IGD='Non', @IndemniteForfaitaire='Non', @Actif='Oui';
	EXEC Fred_ToolBox_Code_Deplacement @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='IPD3', @Libelle='IPD - Zone 3', @KmMini=20, @KmMaxi=30, @IGD='Non', @IndemniteForfaitaire='Non', @Actif='Oui';
	EXEC Fred_ToolBox_Code_Deplacement @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='IPD4', @Libelle='IPD - Zone 4', @KmMini=30, @KmMaxi=40, @IGD='Non', @IndemniteForfaitaire='Non', @Actif='Oui';
	EXEC Fred_ToolBox_Code_Deplacement @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='IPD5', @Libelle='IPD - Zone 5', @KmMini=40, @KmMaxi=50, @IGD='Non', @IndemniteForfaitaire='Non', @Actif='Oui';
	EXEC Fred_ToolBox_Code_Deplacement @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='IPD6', @Libelle='IPD - Zone 6', @KmMini=50, @KmMaxi=9999, @IGD='Non', @IndemniteForfaitaire='Non', @Actif='Oui';

	DELETE #tmpTableSoc WHERE Code = @tmpCode
END

DROP TABLE #tmpTableSoc;