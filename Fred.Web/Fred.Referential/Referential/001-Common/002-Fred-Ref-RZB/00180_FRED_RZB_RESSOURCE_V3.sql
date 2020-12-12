
-- --------------------------------------------------
-- MEP AVRIL 2018 - CORRECTION DE DONNEES
-- S2 - Chapitres_SousChapitres_Ressources
-- --------------------------------------------------

-- Correction pour ajouter le type PERSONNEL aux ressources MO

UPDATE FRED_RESSOURCE SET TypeRessourceId = 2 WHERE RessourceId  IN (
SELECT FRED_RESSOURCE.RessourceId FROM FRED_RESSOURCE, FRED_SOUS_CHAPITRE, FRED_CHAPITRE 
WHERE FRED_RESSOURCE.SousChapitreId = FRED_SOUS_CHAPITRE.SousChapitreId
AND FRED_SOUS_CHAPITRE.ChapitreId = FRED_CHAPITRE.ChapitreId
AND (FRED_CHAPITRE.Code = 10 OR FRED_CHAPITRE.Code = 11)
)


-- Correction pour ajouter le type MATERIEL aux Ressources Matériel
UPDATE FRED_RESSOURCE SET TypeRessourceId = 1 WHERE RessourceId  IN (
SELECT FRED_RESSOURCE.RessourceId FROM FRED_RESSOURCE, FRED_SOUS_CHAPITRE, FRED_CHAPITRE 
WHERE FRED_RESSOURCE.SousChapitreId = FRED_SOUS_CHAPITRE.SousChapitreId
AND FRED_SOUS_CHAPITRE.ChapitreId = FRED_CHAPITRE.ChapitreId
AND (FRED_CHAPITRE.Code = 20)
)
