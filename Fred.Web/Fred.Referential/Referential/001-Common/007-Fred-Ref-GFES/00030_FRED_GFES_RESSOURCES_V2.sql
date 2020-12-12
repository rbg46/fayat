-- Ajout d'une association entre Société, Ressource et Nature
-- On n'en met qu'un car il s'agit d'une donnée pour pouvoir modifier le personnel (suite à une abhération de base et de code)

/*
Note : joué dans 00030_FRED_GFES_RESSOURCES_V4.sql suite aux modifications des ressources dans 00030_FRED_GFES_RESSOURCES_V3.sql



DECLARE @GroupeFESId INT;
SET @GroupeFESId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code='GFES')

INSERT INTO FRED_SOCIETE_RESSOURCE_NATURE (SocieteId, RessourceId, NatureId, Achats)
    SELECT top 1 s.SocieteId, r.RessourceId, n.NatureId, 0
    FROM FRED_SOCIETE s inner join FRED_NATURE n on s.SocieteId = n.SocieteId
        inner join FRED_CHAPITRE c on c.GroupeId = s.GroupeId
        inner join FRED_SOUS_CHAPITRE sc on sc.ChapitreId = c.ChapitreId 
        inner join FRED_RESSOURCE r on r.SousChapitreId = sc.SousChapitreId
    WHERE s.GroupeId=@GroupeFESId

    */