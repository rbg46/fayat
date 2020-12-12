
-- Suppression de tous les codes absence pour FES pour les actualiser

-- Dépendances de la table : FRED_CODE_ABSENCE

--FRED_POINTAGE_ANTICIPE
UPDATE pA
	SET pA.CodeAbsenceId = null
FROM 
	FRED_POINTAGE_ANTICIPE pA
inner join FRED_CODE_ABSENCE A on pA.CodeAbsenceId = A.CodeAbsenceId
INNER JOIN FRED_SOCIETE soc on A.SocieteId = soc.SocieteId
INNER JOIN FRED_GROUPE gr on soc.GroupeId = gr.GroupeId
WHERE gr.Code LIKE '%GFES%'

-- FRED_RAPPORT_LIGNE
Update rl
	SET rl.CodeAbsenceId = null
FROM 
	FRED_RAPPORT_LIGNE rl
inner join FRED_CODE_ABSENCE A on rl.CodeAbsenceId = A.CodeAbsenceId
INNER JOIN FRED_SOCIETE soc on A.SocieteId = soc.SocieteId
INNER JOIN FRED_GROUPE gr on soc.GroupeId = gr.GroupeId
WHERE gr.Code LIKE '%GFES%'


DELETE A 
	FROM [FRED_CODE_ABSENCE] A
INNER JOIN FRED_SOCIETE soc on A.SocieteId = soc.SocieteId
INNER JOIN FRED_GROUPE gr on soc.GroupeId = gr.GroupeId
WHERE gr.Code LIKE '%GFES%' 


SELECT DISTINCT Code
INTO #tmpTableSoc 
FROM FRED_SOCIETE WHERE GroupeId =(SELECT GroupeId FROM FRED_GROUPE where Code LIKE '%GFES%')


Declare @tmpCode nvarchar(20);
WHILE exists (select * from #tmpTableSoc)
BEGIN

	select top 1 @tmpCode = Code from #tmpTableSoc order by Code asc;

	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='ANA', @Libelle='Absence non autorisée', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='ANP', @Libelle='Absence autorisée non payée', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='AP', @Libelle='Absence payée', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='I', @Libelle='Absence intempérie', @Intemperie='Oui', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='RA', @Libelle='Repos autorisé', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='DEL', @Libelle='Absence délégation', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='PREAV', @Libelle='Préavis effectué payé', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='PNE', @Libelle='Préavis non effectué payé', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='VM', @Libelle='Visite médicale', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='AJ', @Libelle='Accident de trajet', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='AT', @Libelle='Accident du travail', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='ML', @Libelle='Maladie non professionnelle', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='MP', @Libelle='Maladie professionnelle', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='MTHN', @Libelle='Mi-temps thérapeutique', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='CP', @Libelle='Conge paye', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='RC', @Libelle='Repos compensateur', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='RT', @Libelle='RTT', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='CONV', @Libelle='Congé conventionnel (mariage, naissance, décès)', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='CPAR', @Libelle='Congé parental', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='CSAB', @Libelle='Congé sabbatique', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='CSS', @Libelle='Congé sans solde', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='MT', @Libelle='Maternité', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='PT', @Libelle='Paternité', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='FERIE', @Libelle='Jour férié', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='FORMA', @Libelle='Ecole / Apprentissage', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';
	EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES', @SocieteCode=@tmpCode, @Code='FORM', @Libelle='Formation', @TauxDecote=1, @NBHeuresDefautETAM=7.5, @NBHeuresMinETAM=1, @NBHeuresMaxETAM=7.5, @NBHeuresDefautCO=7.5, @NBHeuresMinCO=1, @NBHeuresMaxCO=7.5, @Actif='Oui';

	DELETE #tmpTableSoc WHERE Code = @tmpCode
END

DROP TABLE #tmpTableSoc;


-- La version actuelle de la Fred_ToolBox_Code_Absence ne met pas le champs GroupId à jour
UPDATE A 
SET A.GroupeId = gr.GroupeId
	FROM [FRED_CODE_ABSENCE] A
INNER JOIN FRED_SOCIETE soc on A.SocieteId = soc.SocieteId
INNER JOIN FRED_GROUPE gr on soc.GroupeId = gr.GroupeId
WHERE gr.Code LIKE '%GFES%'
