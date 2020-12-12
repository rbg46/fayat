--Association aux roles : Gestionnaire de moyens, Manager des personnels, Responsable Ci, Délégué CI

CREATE PROCEDURE #SET_FRED_ROLE_FONCTIONNALITE
    @RoleSpecification int,
    @CodeFonctionnalite nvarchar(max)
AS
BEGIN
    IF NOT EXISTS (
        SELECT rf.RoleFonctionnaliteId
        FROM FRED_ROLE_FONCTIONNALITE rf inner join FRED_ROLE r on rf.RoleId = r.RoleId
            inner join FRED_FONCTIONNALITE f on rf.FonctionnaliteId = f.FonctionnaliteId
            where r.Specification=@RoleSpecification and f.code=@CodeFonctionnalite)
    BEGIN
        INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
        SELECT r.RoleId, f.FonctionnaliteId, 2 as Mode
        FROM FRED_ROLE r, FRED_FONCTIONNALITE f
        where r.Specification=@RoleSpecification and f.code=@CodeFonctionnalite
    END
END
GO

-- Gestionnaire des moyens (RoleSepecification = 3)

    -- Affichage du filtre par Ci
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=3, @CodeFonctionnalite='1361';

    -- Affichage du champs responsable ou manager
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=3, @CodeFonctionnalite='1362';

-- Fin Gestionnaire des moyens

-- Responsable CI (RoleSepecification = 1)

    -- Affichage du filtre par Ci
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=1, @CodeFonctionnalite='1361';

    -- Affichage du champs responsable ou manager
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=1, @CodeFonctionnalite='1362';

-- Fin Responsable CI

-- Délégué CI (RoleSepecification = 2)

    -- Affichage du filtre par Ci
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=2, @CodeFonctionnalite='1361';

    -- Affichage du champs responsable ou manager
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=2, @CodeFonctionnalite='1362';

-- Fin Délégué CI

DROP PROCEDURE #SET_FRED_ROLE_FONCTIONNALITE