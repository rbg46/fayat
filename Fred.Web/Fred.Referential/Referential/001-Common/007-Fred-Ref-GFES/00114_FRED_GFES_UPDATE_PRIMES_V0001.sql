-- 16-01-2019
-- Préparation de MEP pour FES 
-- Update pour la table des PRIMES .


-- Suppression des primes IPD1 IPD2 IPD3 IPD4 IPD6

-- Dépendances de la table : [FRED_PRIME]

-- FRED_CI_PRIME
UPDATE cP
	SET cP.PrimeId = NULL
FROM FRED_CI_PRIME cP
INNER JOIN [FRED_PRIME] P on P.PrimeId = cP.PrimeId
INNER JOIN FRED_SOCIETE soc on P.SocieteId = soc.SocieteId
INNER JOIN FRED_GROUPE gr on soc.GroupeId = gr.GroupeId
WHERE gr.Code LIKE '%GFES%' 
	AND P.Code IN ('IPD1', 'IPD2', 'IPD3', 'IPD4', 'IPD6')

-- FRED_POINTAGE_ANTICIPE_PRIME
UPDATE pA
	SET pA.PrimeId = NULL
FROM FRED_POINTAGE_ANTICIPE_PRIME pA
INNER JOIN [FRED_PRIME] P on P.PrimeId = pA.PrimeId
INNER JOIN FRED_SOCIETE soc on P.SocieteId = soc.SocieteId
INNER JOIN FRED_GROUPE gr on soc.GroupeId = gr.GroupeId
WHERE gr.Code LIKE '%GFES%' 
	AND P.Code IN ('IPD1', 'IPD2', 'IPD3', 'IPD4', 'IPD6')

-- FRED_RAPPORT_LIGNE_PRIME
UPDATE rP
	SET rP.PrimeId = NULL
FROM FRED_RAPPORT_LIGNE_PRIME rP
INNER JOIN [FRED_PRIME] P on P.PrimeId = rP.PrimeId
INNER JOIN FRED_SOCIETE soc on P.SocieteId = soc.SocieteId
INNER JOIN FRED_GROUPE gr on soc.GroupeId = gr.GroupeId
WHERE gr.Code LIKE '%GFES%' 
	AND P.Code IN ('IPD1', 'IPD2', 'IPD3', 'IPD4', 'IPD6')

-- FRED_RAPPORTPRIME_LIGNE_PRIME
UPDATE rP
	SET rP.PrimeId = NULL
FROM FRED_RAPPORTPRIME_LIGNE_PRIME rP
INNER JOIN [FRED_PRIME] P on P.PrimeId = rP.PrimeId
INNER JOIN FRED_SOCIETE soc on P.SocieteId = soc.SocieteId
INNER JOIN FRED_GROUPE gr on soc.GroupeId = gr.GroupeId
WHERE gr.Code LIKE '%GFES%' 
	AND P.Code IN ('IPD1', 'IPD2', 'IPD3', 'IPD4', 'IPD6')


DELETE P
	FROM [FRED_PRIME] P
	INNER JOIN FRED_SOCIETE soc on P.SocieteId = soc.SocieteId
	INNER JOIN FRED_GROUPE gr on soc.GroupeId = gr.GroupeId
WHERE gr.Code LIKE '%GFES%' 
	AND P.Code IN ('IPD1', 'IPD2', 'IPD3', 'IPD4', 'IPD6')



-- -- Update de la liste des primes
SELECT DISTINCT Code
into #tmpTableSoc 
FROM FRED_SOCIETE WHERE GroupeId =(SELECT GroupeId FROM FRED_GROUPE where Code LIKE '%GFES%')

Declare @tmpCode nvarchar(20);
WHILE exists (select * from #tmpTableSoc)
BEGIN

	select top 1 @tmpCode = Code from #tmpTableSoc order by Code asc;

	EXEC Fred_ToolBox_Code_Prime @GroupeCode='GFES',@NombreHeuresMax=7,@Code='GDI', @Libelle='IGD IDF', @Actif='Oui', @Publique='Oui', @SocieteCode=@tmpCode, @PrimeType='JOURNALIER';
	EXEC Fred_ToolBox_Code_Prime @GroupeCode='GFES',@NombreHeuresMax=7,@Code='GDP', @Libelle='IGD Province', @Actif='Oui', @Publique='Oui', @SocieteCode=@tmpCode, @PrimeType='JOURNALIER';
	EXEC Fred_ToolBox_Code_Prime @GroupeCode='GFES',@NombreHeuresMax=7,@Code='IR', @Libelle='Indemnité Repas', @Actif='Oui', @Publique='Oui', @SocieteCode=@tmpCode, @PrimeType='JOURNALIER';
	EXEC Fred_ToolBox_Code_Prime @GroupeCode='GFES',@NombreHeuresMax=7,@Code='TR', @Libelle='Titres restaurant', @Actif='Oui', @Publique='Oui', @SocieteCode=@tmpCode, @PrimeType='JOURNALIER';
	EXEC Fred_ToolBox_Code_Prime @GroupeCode='GFES',@NombreHeuresMax=7,@Code='AE', @Libelle='Prime égouts', @Actif='Oui', @Publique='Oui', @SocieteCode=@tmpCode, @PrimeType='JOURNALIER';
	EXEC Fred_ToolBox_Code_Prime @GroupeCode='GFES',@NombreHeuresMax=7,@Code='INS', @Libelle='Prime insalubrité ', @Actif='Oui', @Publique='Oui', @SocieteCode=@tmpCode, @PrimeType='JOURNALIER';
	EXEC Fred_ToolBox_Code_Prime @GroupeCode='GFES',@NombreHeuresMax=7,@Code='ES', @Libelle='Prime salissure', @Actif='Oui', @Publique='Oui', @SocieteCode=@tmpCode, @PrimeType='JOURNALIER';
	EXEC Fred_ToolBox_Code_Prime @GroupeCode='GFES',@NombreHeuresMax=7,@Code='ASTRS', @Libelle='Prime Astreinte du lundi au vendredi', @Actif='Oui', @Publique='Oui', @SocieteCode=@tmpCode, @PrimeType='JOURNALIER';
	EXEC Fred_ToolBox_Code_Prime @GroupeCode='GFES',@NombreHeuresMax=7,@Code='ASTRWE', @Libelle='Prime Astreinte weekend et jour férié', @Actif='Oui', @Publique='Oui', @SocieteCode=@tmpCode, @PrimeType='JOURNALIER';

	DELETE #tmpTableSoc WHERE Code = @tmpCode
END

DROP TABLE #tmpTableSoc;




-- IsPrimeAstreinte = 1 pour 'ASTRS', 'ASTRWE'

UPDATE P
SET 
	P.IsPrimeAstreinte = 1
FROM [FRED_PRIME] P
INNER JOIN FRED_SOCIETE soc on P.SocieteId = soc.SocieteId
INNER JOIN FRED_GROUPE gr on soc.GroupeId = gr.GroupeId
WHERE gr.Code LIKE '%GFES%' 
AND P.Code IN ('ASTRS', 'ASTRWE')

-- IsPrimeAstreinte = NULL pour les codes différents de : 'ASTRS', 'ASTRWE' (Par défault la ToolBox met les valeurs boolean à 0)

UPDATE P
SET 
	P.IsPrimeAstreinte = NULL
FROM [FRED_PRIME] P
INNER JOIN FRED_SOCIETE soc on P.SocieteId = soc.SocieteId
INNER JOIN FRED_GROUPE gr on soc.GroupeId = gr.GroupeId
WHERE gr.Code LIKE '%GFES%' 
AND P.Code NOT IN ('ASTRS', 'ASTRWE')

