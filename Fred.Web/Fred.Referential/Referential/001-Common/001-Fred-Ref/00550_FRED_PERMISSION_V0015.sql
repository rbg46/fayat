-- =======================================================================================================================================
-- Author:		Yannick DEFAY
--
-- Description:
--      - Mise à jour des permissions
--      - Nouvelle permission pour la fonctionnalité 'Budget'
--
-- =======================================================================================================================================


BEGIN TRAN
    IF NOT EXISTS ( SELECT 1 FROM FRED_PERMISSION WHERE Code = '0127')
    BEGIN
	    INSERT INTO FRED_PERMISSION (PermissionKey, PermissionType, Code, Libelle, PermissionContextuelle)
	    VALUES ('menu.show.budget.avancement-recette.index', 1, '0127', 'Affiche l''écran Avancement Recette', 0)
    END

    ------------------------------------------------------
    -- Association des permissions aux fonctionnalités
    ------------------------------------------------------

    -- Affichage du menu
    IF NOT EXISTS (
	    SELECT pf.PermissionFonctionnaliteId
	    FROM FRED_PERMISSION_FONCTIONNALITE pf inner join FRED_PERMISSION p on pf.PermissionId = p.PermissionId
		    inner join FRED_FONCTIONNALITE f on pf.FonctionnaliteId = f.FonctionnaliteId
		    where p.Code='0127' and f.code='1202')
    BEGIN
	    INSERT INTO FRED_PERMISSION_FONCTIONNALITE 
		    SELECT P.PermissionId, f.FonctionnaliteId
		    FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f
		    where p.Code='0127' and f.code='1202'
    END
COMMIT TRAN

