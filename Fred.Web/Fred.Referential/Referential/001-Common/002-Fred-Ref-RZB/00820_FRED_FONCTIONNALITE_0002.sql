-- =======================================================================================================================================
--      - Ajout de la fonctionnalité 'Association Famille OD/Natures/Journaux' à la table FRED_FONCTIONNALITE lié au module Paramétrage
--      - Ajout de la permission 'Affichage du menu / Accès à la page 'Association Famille OD/Natures/Journaux''  à la table FRED_PERMISSION
--      - Ajout de la relation de la permission à la fonctionnalité dans la table FRED_PERMISSION_FONCTIONNALITE
--
-- =======================================================================================================================================


DECLARE @module_id int = (SELECT ModuleId FROM FRED_MODULE where Code = '7');

DECLARE @FonctionnaliteCode int = (SELECT MAX(CAST(Code AS int)) + 1 FROM [FRED_FONCTIONNALITE] WHERE ISNUMERIC(Code) = 1);
INSERT INTO FRED_FONCTIONNALITE 
				(ModuleId, 
				Code, 
				Libelle, 
				HorsOrga, 
				DateSuppression, 
				Description) 
VALUES 
				(@module_id, 
				@FonctionnaliteCode, 
				'Association Famille OD/Natures/Journaux', 
				0, 
				NULL, 
				NULL); 

DECLARE @fonctionnalite_id int = (SELECT SCOPE_IDENTITY());

IF NOT EXISTS(SELECT PermissionId FROM [FRED_PERMISSION] where [PermissionKey] = 'menu.show.famillesoperationsdiverses.association')
BEGIN
	DECLARE  @Code int = (SELECT MAX(Code) + 1  FROM [FRED_PERMISSION]);
	INSERT INTO[dbo].[FRED_PERMISSION] 
				([PermissionKey], 
				[PermissionType], 
				[Code], 
				[Libelle], 
				[PermissionContextuelle]) 
	VALUES('menu.show.famillesoperationsdiverses.association',
			1,
			@Code,
			'Affichage du menu / Accès à la page ''Association Famille OD/Natures/Journaux''.',
			0)
END

DECLARE @permission_id int = ( SELECT PermissionId FROM [FRED_PERMISSION] where [PermissionKey] = 'menu.show.famillesoperationsdiverses.association');

INSERT INTO FRED_PERMISSION_FONCTIONNALITE (PermissionId, FonctionnaliteId) VALUES (@permission_id, @fonctionnalite_id);

DECLARE @role_id int =(SELECT RoleId FROM FRED_ROLE r INNER JOIN FRED_SOCIETE s ON s.SocieteId = r.SocieteId WHERE r.Libelle = 'Admin Appli' AND s.Code = 'RB');

INSERT INTO [dbo].[FRED_ROLE_FONCTIONNALITE]
			([RoleId]
			,[FonctionnaliteId]
			,[Mode])
	VALUES
			(@role_id
			,@fonctionnalite_id
			,2)


