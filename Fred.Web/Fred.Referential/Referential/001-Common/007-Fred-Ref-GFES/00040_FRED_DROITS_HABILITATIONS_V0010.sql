
--Correction de des affectation du role délégué (Specification = 2)

-- Suppression de la fonctionnalite '1300' du role délégué
DECLARE @fonctionnaliteToDeleteId INT;
SET @fonctionnaliteToDeleteId = (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code='1300');

DELETE 
FROM [FRED_ROLE_FONCTIONNALITE] 
WHERE 
    FonctionnaliteId = @fonctionnaliteToDeleteId
AND RoleId IN ( SELECT RoleId FROM FRED_ROLE WHERE Specification = 2)


-- Affichage du menu de gestion des moyens par défaut pour le role délégué
IF NOT EXISTS (
            SELECT rf.RoleFonctionnaliteId
            FROM FRED_ROLE_FONCTIONNALITE rf inner join FRED_ROLE r on rf.RoleId = r.RoleId
                inner join FRED_FONCTIONNALITE f on rf.FonctionnaliteId = f.FonctionnaliteId
                where r.Specification=2 and f.code = '1350')
BEGIN
    INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
    SELECT r.RoleId, f.FonctionnaliteId, 2
    FROM FRED_ROLE r, FRED_FONCTIONNALITE f
    where r.Specification=2 and f.code = '1350'
END