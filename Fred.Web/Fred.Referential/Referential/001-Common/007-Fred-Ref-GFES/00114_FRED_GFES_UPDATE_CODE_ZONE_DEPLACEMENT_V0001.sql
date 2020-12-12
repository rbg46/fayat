-- 16-01-2019
-- Préparation de MEP pour FES 
-- Update pour la table FRED_CODE_ZONE__DEPLACEMENT .


SELECT DISTINCT Code, SocieteId
into #tmpTableSoc 
FROM FRED_SOCIETE WHERE GroupeId =(SELECT GroupeId FROM FREd_GROUPE where Code LIKE '%GFES%')


-- A ce stade @KmMini et @KmMaxi ne sont pas considérées par la ToolBox .

Declare @tmpCode nvarchar(20);
Declare @tmpSocId INT;

WHILE exists (select * from #tmpTableSoc)
BEGIN

	select top 1 @tmpCode = Code, @tmpSocId = SocieteId from #tmpTableSoc order by Code asc;

	EXEC Fred_ToolBox_Code_Zone_Deplacement @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='Z1', @Libelle='Zone 1', @IsActif='Oui';
	UPDATE FRED_CODE_ZONE_DEPLACEMENT SET KmMini = 0, KmMaxi = 10 WHERE Code = 'Z1' AND SocieteId = @tmpSocId;

	EXEC Fred_ToolBox_Code_Zone_Deplacement @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='Z2', @Libelle='Zone 2', @IsActif='Oui';
	UPDATE FRED_CODE_ZONE_DEPLACEMENT SET KmMini = 10, KmMaxi = 20 WHERE Code = 'Z2' AND SocieteId = @tmpSocId;

	EXEC Fred_ToolBox_Code_Zone_Deplacement @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='Z3', @Libelle='Zone 3', @IsActif='Oui';
	UPDATE FRED_CODE_ZONE_DEPLACEMENT SET KmMini = 20, KmMaxi = 30 WHERE Code = 'Z3' AND SocieteId = @tmpSocId;

	EXEC Fred_ToolBox_Code_Zone_Deplacement @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='Z4', @Libelle='Zone 4', @IsActif='Oui';
	UPDATE FRED_CODE_ZONE_DEPLACEMENT SET KmMini = 30, KmMaxi = 40 WHERE Code = 'Z4' AND SocieteId = @tmpSocId;

	EXEC Fred_ToolBox_Code_Zone_Deplacement @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='Z5', @Libelle='Zone 5', @IsActif='Oui';
	UPDATE FRED_CODE_ZONE_DEPLACEMENT SET KmMini = 40, KmMaxi = 50 WHERE Code = 'Z5' AND SocieteId = @tmpSocId;

	EXEC Fred_ToolBox_Code_Zone_Deplacement @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='Z6', @Libelle='Zone 6', @IsActif='Oui';
	UPDATE FRED_CODE_ZONE_DEPLACEMENT SET KmMini = 50, KmMaxi = 9999 WHERE Code = 'Z6' AND SocieteId = @tmpSocId;

	DELETE #tmpTableSoc WHERE Code = @tmpCode
END

DROP TABLE #tmpTableSoc;