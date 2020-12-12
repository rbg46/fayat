 -- =======================================================================================================================================
-- Author:		YDY
--
-- Description:
--      - Suppression des Barèmes CI STORM :
--				- TYPE RESSOURCE associé matériel : 'MAT'
--				- CODE SOCIETE Razel-Bec : 'RZB'
--      - Met à jour les lignes de valorisations possédant un barèmes STORM :
--				- Mise à NULL de la colonne BaremeId
--
-- =======================================================================================================================================


-- Liste des barèmes CI
SELECT B.BaremeId
FROM FRED_BAREME_EXPLOITATION_CI AS B 
WHERE B.ReferentielEtenduId IN (SELECT RE.ReferentielEtenduId
																FROM FRED_SOCIETE_RESSOURCE_NATURE AS RE 
																INNER JOIN FRED_RESSOURCE AS R 
																ON RE.RessourceId = R.RessourceId
																INNER JOIN FRED_TYPE_RESSOURCE AS TR 
																ON R.TypeRessourceId = TR.TypeRessourceId
																INNER JOIN FRED_SOCIETE AS S 
																ON RE.SocieteId = S.SocieteId
																WHERE S.CodeSocietePaye = 'RZB' 
																	AND TR.Code = 'MAT')

-- Liste des valorisation Storm
SELECT * 
FROM FRED_VALORISATION As V
WHERE V.BaremeId IN ( SELECT B.BaremeId
											FROM FRED_BAREME_EXPLOITATION_CI AS B 
											WHERE B.ReferentielEtenduId IN (SELECT RE.ReferentielEtenduId
																											FROM FRED_SOCIETE_RESSOURCE_NATURE AS RE 
																											INNER JOIN FRED_RESSOURCE AS R 
																											ON RE.RessourceId = R.RessourceId
																											INNER JOIN FRED_TYPE_RESSOURCE AS TR 
																											ON R.TypeRessourceId = TR.TypeRessourceId
																											INNER JOIN FRED_SOCIETE AS S 
																											ON RE.SocieteId = S.SocieteId
																											WHERE S.CodeSocietePaye = 'RZB' 
																												AND TR.Code = 'MAT'))

-- Mise à NULL des BaremeId STORM dans la table de valo
UPDATE FRED_VALORISATION
SET BaremeId = NULL
WHERE BaremeId IN ( SELECT B.BaremeId
										FROM FRED_BAREME_EXPLOITATION_CI AS B 
										WHERE B.ReferentielEtenduId IN (SELECT RE.ReferentielEtenduId
																										FROM FRED_SOCIETE_RESSOURCE_NATURE AS RE 
																										INNER JOIN FRED_RESSOURCE AS R 
																										ON RE.RessourceId = R.RessourceId
																										INNER JOIN FRED_TYPE_RESSOURCE AS TR 
																										ON R.TypeRessourceId = TR.TypeRessourceId
																										INNER JOIN FRED_SOCIETE AS S 
																										ON RE.SocieteId = S.SocieteId
																										WHERE S.CodeSocietePaye = 'RZB' 
																											AND TR.Code = 'MAT'))

-- Suppression des bareme CI pour e matériel STORM
DELETE FROM FRED_BAREME_EXPLOITATION_CI
WHERE BaremeId IN ( SELECT B.BaremeId
										FROM FRED_BAREME_EXPLOITATION_CI AS B 
										WHERE B.ReferentielEtenduId IN (SELECT RE.ReferentielEtenduId
																										FROM FRED_SOCIETE_RESSOURCE_NATURE AS RE 
																										INNER JOIN FRED_RESSOURCE AS R 
																										ON RE.RessourceId = R.RessourceId
																										INNER JOIN FRED_TYPE_RESSOURCE AS TR 
																										ON R.TypeRessourceId = TR.TypeRessourceId
																										INNER JOIN FRED_SOCIETE AS S 
																										ON RE.SocieteId = S.SocieteId
																										WHERE S.CodeSocietePaye = 'RZB' 
																											AND TR.Code = 'MAT'))

-- Liste des barèmes CI
SELECT B.BaremeId
FROM FRED_BAREME_EXPLOITATION_CI AS B 
WHERE B.ReferentielEtenduId IN (SELECT RE.ReferentielEtenduId
																FROM FRED_SOCIETE_RESSOURCE_NATURE AS RE 
																INNER JOIN FRED_RESSOURCE AS R 
																ON RE.RessourceId = R.RessourceId
																INNER JOIN FRED_TYPE_RESSOURCE AS TR 
																ON R.TypeRessourceId = TR.TypeRessourceId
																INNER JOIN FRED_SOCIETE AS S 
																ON RE.SocieteId = S.SocieteId
																WHERE S.CodeSocietePaye = 'RZB' 
																	AND TR.Code = 'MAT')

-- Liste des valorisation Storm
SELECT * 
FROM FRED_VALORISATION As V
WHERE V.BaremeId IN ( SELECT B.BaremeId
											FROM FRED_BAREME_EXPLOITATION_CI AS B 
											WHERE B.ReferentielEtenduId IN (SELECT RE.ReferentielEtenduId
																											FROM FRED_SOCIETE_RESSOURCE_NATURE AS RE 
																											INNER JOIN FRED_RESSOURCE AS R 
																											ON RE.RessourceId = R.RessourceId
																											INNER JOIN FRED_TYPE_RESSOURCE AS TR 
																											ON R.TypeRessourceId = TR.TypeRessourceId
																											INNER JOIN FRED_SOCIETE AS S 
																											ON RE.SocieteId = S.SocieteId
																											WHERE S.CodeSocietePaye = 'RZB' 
																												AND TR.Code = 'MAT'))