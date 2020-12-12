--Association aux roles : Gestionnaire de moyens , Responsable Ci , Délégué CI .

CREATE PROCEDURE #SET_FRED_ROLE_FONCTIONNALITE
    @RoleSpecification int = null,
    @CodeFonctionnalite nvarchar(max),
    @RoleCodeFamilier nvarchar(max) = null,
    @Mode int = 2
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
            SELECT r.RoleId, f.FonctionnaliteId, @Mode
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
            SELECT r.RoleId, f.FonctionnaliteId, @Mode
            FROM FRED_ROLE r, FRED_FONCTIONNALITE f
            where r.CodeNomFamilier=@RoleCodeFamilier and f.code=@CodeFonctionnalite
        END
    END
END
GO

-- Gestionnaire des moyens (RoleSepecification = 3)

    -- Affichage du menu
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=3, @CodeFonctionnalite='1350';

    -- Affichage du bouton d'affectation
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=3, @CodeFonctionnalite='1351';

    -- Affichage du bouton de restitution
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=3, @CodeFonctionnalite='1352';

    -- Affichage du bouton de location
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=3, @CodeFonctionnalite='1353';

    -- Affichage du bouton de maintenance
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=3, @CodeFonctionnalite='1354';

    -- Recherche : Affichage du de la lookup de filtre par personnel
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=3, @CodeFonctionnalite='1355';

    -- Recherche : Affichage par personnel et Ci (Tous)
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=3, @CodeFonctionnalite='1356';

    -- Affectation : séléction des moyens pour affectation
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=3, @CodeFonctionnalite='1357';

    -- Filtre : recherche avancée
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=3, @CodeFonctionnalite='1358';

    -- Validation et annulation
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=3, @CodeFonctionnalite='1359';

    -- Accès date fin d'affectation
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=3, @CodeFonctionnalite='1360';

-- Fin Gestionnaire des moyens

-- Responsable CI (RoleSepecification = 1)

    -- Affichage du menu
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=1, @CodeFonctionnalite='1350';

    -- Affichage du bouton d'affectation
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=1, @CodeFonctionnalite='1351';

    -- Recherche : Affichage du de la lookup de filtre par personnel
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=1, @CodeFonctionnalite='1355';

    -- Recherche : Affichage par personnel et Ci (Tous)
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=1, @CodeFonctionnalite='1356';

    -- Affectation : séléction des moyens pour affectation
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=1, @CodeFonctionnalite='1357';

    -- Validation et annulation
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=1, @CodeFonctionnalite='1359';

-- Fin Responsable CI

-- Délégué CI (RoleSepecification = 2)

    -- Affichage menu 
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=2, @CodeFonctionnalite='1300';

    -- Validation et annulation
    EXEC #SET_FRED_ROLE_FONCTIONNALITE @RoleSpecification=2, @CodeFonctionnalite='1359', @Mode=1;

-- Fin Délégué CI



DROP PROCEDURE #SET_FRED_ROLE_FONCTIONNALITE