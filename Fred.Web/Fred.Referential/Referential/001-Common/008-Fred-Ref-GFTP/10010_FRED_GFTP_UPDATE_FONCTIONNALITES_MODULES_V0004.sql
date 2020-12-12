BEGIN TRAN ManualLockFonctionnality; 

	DECLARE @AchatModuleCode nvarchar(255) = '8';
	DECLARE @AchatModuleLibelle nvarchar(255) = 'Achats';
	DECLARE @FonctionnaliteCode nvarchar(255) = '2020';
	DECLARE @FonctionnaliteLibelle nvarchar(255) = 'Verrouillage et déverrouillage manuel de ligne de commande';
	DECLARE @PermissionKey nvarchar(max) ='functionality.lock.unlock.commandeLigne';
	DECLARE @SocieteCode nvarchar(20) =  '0001';
	DECLARE @RoleLibelle nvarchar(255) = '%admin appli%';
	DECLARE @FonctionnaliteMode int = 2;
	DECLARE @FonctionnaliteId int;
	DECLARE @RoleId int;
	DECLARE @SocieteId int;

	-- MAJ / Ajout du module Achats
	IF EXISTS (SELECT ModuleId FROM FRED_MODULE WHERE Code=@AchatModuleCode)   
		UPDATE FRED_MODULE SET Libelle=@AchatModuleLibelle, DateSuppression=NULL, Description=NULL WHERE Code=@AchatModuleCode  
	ELSE
		INSERT INTO FRED_MODULE (Code, Libelle, DateSuppression, [Description]) VALUES(@AchatModuleCode, @AchatModuleLibelle, NULL, NULL)

	-- MAJ / Ajout de la fonctionnalité
	IF EXISTS (SELECT FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code=@FonctionnaliteCode)   
		UPDATE FRED_FONCTIONNALITE SET ModuleId=(SELECT m.ModuleId FROM FRED_MODULE m WHERE m.Code=@AchatModuleCode), Code=@FonctionnaliteCode, Libelle=@FonctionnaliteLibelle, HorsOrga=0, DateSuppression=NULL, Description=NULL WHERE Code=@FonctionnaliteCode 
	ELSE 
		INSERT INTO FRED_FONCTIONNALITE (ModuleId, Code, Libelle, HorsOrga, DateSuppression, [Description]) VALUES ((SELECT m.ModuleId FROM FRED_MODULE m WHERE m.Code=@AchatModuleCode), @FonctionnaliteCode, @FonctionnaliteLibelle, 0, NULL, NULL)

	-- Ajout de la relation permission - fonctionnalité
	DELETE FROM FRED_PERMISSION_FONCTIONNALITE WHERE PermissionId = (SELECT  PermissionId FROM FRED_PERMISSION WHERE PermissionKey = @PermissionKey)
	INSERT INTO FRED_PERMISSION_FONCTIONNALITE SELECT p.PermissionId, f.FonctionnaliteId FROM FRED_PERMISSION p, FRED_FONCTIONNALITE f WHERE f.Code=@FonctionnaliteCode and p.PermissionKey = @PermissionKey


	-- Désactivation de la fonctionnalité pour toutes les société sauf FTP
	SELECT @SocieteId = SocieteId FROM FRED_SOCIETE WHERE Code = @SocieteCode
	SELECT @FonctionnaliteId = FonctionnaliteId FROM FRED_FONCTIONNALITE WHERE Code=@FonctionnaliteCode;
	DELETE FRED_FONCTIONNALITE_DESACTIVE WHERE FonctionnaliteId = @FonctionnaliteId
	INSERT INTO FRED_FONCTIONNALITE_DESACTIVE SELECT SocieteId, @FonctionnaliteId FROM FRED_SOCIETE WHERE SocieteId <> @SocieteId

	-- Attribution de la fonctionnalité au role AdminAppli
	SELECT @RoleId = RoleId FROM FRED_ROLE WHERE SocieteId = @SocieteId and Libelle like @RoleLibelle
	IF @RoleId IS NOT NULL
	BEGIN
		DELETE FRED_ROLE_FONCTIONNALITE WHERE FonctionnaliteId = @FonctionnaliteId
		INSERT INTO FRED_ROLE_FONCTIONNALITE(RoleId, FonctionnaliteId, Mode) VALUES(@RoleId, @FonctionnaliteId, @FonctionnaliteMode)
	END
	ELSE PRINT('Role Admin Appli non trouvé')

COMMIT TRAN ManualLockFonctionnality;
