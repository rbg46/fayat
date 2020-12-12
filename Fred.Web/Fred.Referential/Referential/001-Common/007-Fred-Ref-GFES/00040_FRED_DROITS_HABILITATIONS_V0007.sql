--Association aux roles : Gestionnaire de moyens, Manager des personnels, Responsable Ci, Délégué CI

CREATE PROCEDURE #SET_FRED_ROLE_FONCTIONNALITE
    @RoleSpecification int = null,
    @CodeFonctionnalite nvarchar(max),
    @RoleCodeFamilier nvarchar(max) = null
AS
BEGIN
    IF @RoleSpecification IS NOT NULL
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
    ELSE
    BEGIN
        IF NOT EXISTS (
        SELECT rf.RoleFonctionnaliteId
        FROM FRED_ROLE_FONCTIONNALITE rf inner join FRED_ROLE r on rf.RoleId = r.RoleId
            inner join FRED_FONCTIONNALITE f on rf.FonctionnaliteId = f.FonctionnaliteId
            where r.CodeNomFamilier=@RoleCodeFamilier and f.code=@CodeFonctionnalite)
        BEGIN
            INSERT INTO FRED_ROLE_FONCTIONNALITE (RoleId, FonctionnaliteId, Mode)
            SELECT r.RoleId, f.FonctionnaliteId, 2 as Mode
            FROM FRED_ROLE r, FRED_FONCTIONNALITE f
            where r.CodeNomFamilier=@RoleCodeFamilier and f.code=@CodeFonctionnalite
        END
    END
END
GO

-- Gestionnaire des moyens (RoleSepecification = 3)

    -- Affichage de toute la liste des affectations des moyens
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=3, @CodeFonctionnalite='1311';

    -- Affichage de toute la liste des CI 
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=3, @CodeFonctionnalite='1315';

    -- Affichage de toute la liste des personnels
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=3, @CodeFonctionnalite='1318';

-- Fin Gestionnaire des moyens


-- Manager des personnels (CodeFamilier : 'MP')
    
    -- Affichage de la liste des affectations moyens liées au personnels d'un manager
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleCodeFamilier='MP', @CodeFonctionnalite='1312';

    -- Affichage de la liste des personnels d'un manager 
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleCodeFamilier='MP', @CodeFonctionnalite='1319';

-- Fin manager des personnels


-- Responsable CI (RoleSepecification = 1)

    -- Affichage de la liste des affectations moyens liées au CI d'un responsable CI
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=1, @CodeFonctionnalite='1313';

    -- Affichage de la liste des CI d'un responsable CI 
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=1, @CodeFonctionnalite='1316';

    -- Affichage de la liste des personnels affectés aux CI d'un responsable CI
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=1, @CodeFonctionnalite='1320';

-- Fin Responsable CI

-- Délégué CI (RoleSepecification = 2)

    -- Affichage de la liste des affectations moyens liées au CI d'un délégué
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=2, @CodeFonctionnalite='1314';

    -- Affichage de la liste des CI d'un délégué
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=2, @CodeFonctionnalite='1317';

-- Fin Délégué CI

DROP PROCEDURE #SET_FRED_ROLE_FONCTIONNALITE