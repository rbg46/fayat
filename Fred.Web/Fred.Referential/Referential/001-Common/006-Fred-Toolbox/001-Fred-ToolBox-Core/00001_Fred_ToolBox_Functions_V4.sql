
-- --------------------------------------------------
-- FRED 2017 - R4 - SEPTEMBRE 2018 
-- TOOLBOX INJECTION COMPTE ADMINISTRATEUR
-- MODOP : 
--    EXEC Fred_ToolBox_InsertAdminUser '500', 'ADM_FSI', 'admin_fsi', 'FSI', 'FR20ed12!1970'
-- Historique :
-- JNE : Suppression de la colonne NumSecu et CleSecu sur la table personnel
-- --------------------------------------------------

/*PROCEDURE STOCKEES*/
IF OBJECT_ID ( 'Fred_ToolBox_InsertAdminUser', 'P' ) IS NOT NULL   
	DROP PROCEDURE Fred_ToolBox_InsertAdminUser;  
GO  
CREATE PROCEDURE Fred_ToolBox_InsertAdminUser   
	@SocieteCode nvarchar(50),   
	@Matricule nvarchar(50), 
	@Login  nvarchar(50),
	@TrigrammeSociete nvarchar(50),
	@MotDePasse  nvarchar(50)

AS   
BEGIN
	PRINT '---------------------------------------------'
	PRINT ' FAYAT IT - 2018 '
	PRINT '---------------------------------------------'
	PRINT 'Enregistrement d''un compte Super Admin'
	PRINT '---------------------------------------------'

	DECLARE @ErrorLoginMatriculeNormeNommage int; 
	SET @ErrorLoginMatriculeNormeNommage = 1;

	DECLARE @CheckSociete INT; 
	DECLARE @LeftLogin varchar(5);
	SET @LeftLogin = (SELECT UPPER(LEFT(@Login,3)));

	IF (SELECT UPPER(LEFT(@Login,4))) = 'ADM_'
		BEGIN
			SET @ErrorLoginMatriculeNormeNommage = 0;
		END
	ELSE
		BEGIN
			SET @ErrorLoginMatriculeNormeNommage = 1;
		END
	
		IF (SELECT UPPER(LEFT(@Matricule,4))) = 'ADM_'
		BEGIN
			SET @ErrorLoginMatriculeNormeNommage = 0;
		END
	ELSE
		BEGIN
			SET @ErrorLoginMatriculeNormeNommage = 1;
		END

	IF @ErrorLoginMatriculeNormeNommage = 1
			BEGIN
				PRINT ' DESOLE, le login / matricule doivent commencé par ADM_'
			END
		ELSE
			BEGIN
				-- VERIFICATION UNICITE MATRICULE et LOGIN
				DECLARE @ErrorLoginMatriculeUnicité int; 
				SET @ErrorLoginMatriculeUnicité = 1;

				-- VERIFICATION QUE LE MATRICULE EST UNIQUE DANS LE SYSTEME
				IF(SELECT COUNT(PersonnelID) FROM FRED_PERSONNEL WHERE Matricule = @Matricule) != 0
				BEGIN
					PRINT 'ERREUR > Ce matricule déjà utilisé dans le système';
					SET @ErrorLoginMatriculeUnicité = 0;
				END
				IF(SELECT COUNT(Login) FROM FRED_UTILISATEUR WHERE Login = @Login) != 0
				BEGIN
					PRINT 'ERREUR > Ce login déjà utilisé dans le système';
					SET @ErrorLoginMatriculeUnicité = 0;
				END

				IF @ErrorLoginMatriculeUnicité = 1 
				BEGIN
						BEGIN TRY
							BEGIN TRANSACTION
								-- RECUPERATION CODE SOCIETE
								DECLARE @SOCIETE_ID INT;
								SET @SOCIETE_ID = (SELECT SocieteId FROM  FRED_SOCIETE  WHERE Code = @SocieteCode)
								-- RECUPERATION LIBELLE SOCIETE
								DECLARE @SOCIETE_LIBELLE VARCHAR(50);
								SET @SOCIETE_LIBELLE = (SELECT Libelle FROM  FRED_SOCIETE  WHERE Code = @SocieteCode);

								-- ETAPE 1 : CREATION PERSONNEL
								DECLARE @PERSONNEL_ID int;
								INSERT INTO FRED_PERSONNEL (SocieteId,Matricule,IsInterimaire,IsInterne,Nom,Prenom,Statut,CategoriePerso,DateEntree,DateSortie,Adresse1,Adresse2,Adresse3,CodePostal,Ville,PaysId,LongitudeDomicile,LatitudeDomicile,Tel1,Tel2,Email,TypeRattachement,DateCreation,DateModification,DateSuppression,UtilisateurIdCreation,UtilisateurIdModification,UtilisateurIdSuppression,RessourceId,EtablissementPayeId,EtablissementRattachementId) 
								VALUES (@SOCIETE_ID,@Matricule,'0','0','SuperAdmin_' + @TrigrammeSociete ,@SOCIETE_LIBELLE,NULL,NULL,'01/01/2017',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'fred.admin_'+@TrigrammeSociete+'@fci.fayat.com',NULL,'01/01/2017',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL); 
								SET @PERSONNEL_ID = @@IDENTITY;

								PRINT 'Compte enregistré comme personnel avec l''identifiant suivant';
								PRINT @PERSONNEL_ID;

								-- ETAPE 2 : CREATION DANS DIRECTORY
								DECLARE @FRED_EXTERNALDIRECTORY_ID INT;
								INSERT INTO FRED_EXTERNALDIRECTORY (MotDePasse,DateExpiration,IsActived) VALUES (@MotDePasse,'01/01/2030','1'); 
								SET @FRED_EXTERNALDIRECTORY_ID = @@IDENTITY;
				
								PRINT 'Mot de passe enregistré avec l''identifiant suivant';
								PRINT @FRED_EXTERNALDIRECTORY_ID;

								-- ETAPE 3 : CREATION DANS UTILISATEUR
								DECLARE @UTILISATEUR_ID INT;
				
								INSERT INTO FRED_UTILISATEUR (UtilisateurId,Login,DateDerniereConnexion,FayatAccessDirectoryId,IsDeleted,DateCreation,DateModification,DateSupression,UtilisateurIdCreation,UtilisateurIdModification,UtilisateurIdSupression,SuperAdmin,Folio,CommandeManuelleAllowed) 
								VALUES (@PERSONNEL_ID,@Login,NULL,@FRED_EXTERNALDIRECTORY_ID,'0',GETDATE(),NULL,NULL,NULL,NULL,NULL,'1',NULL,'0'); 
								SET @UTILISATEUR_ID = @@IDENTITY;
								PRINT 'Compte enregistré comme Utilisateur avec l''identifiant suivant';
								PRINT @UTILISATEUR_ID;

								PRINT 'Compte utilisateur enregistré'
				COMMIT TRAN
					END TRY

				BEGIN CATCH
					DECLARE @ERROR INT, @MESSAGE VARCHAR(4000), @XSTATE INT;
					SELECT @ERROR = ERROR_NUMBER(), @MESSAGE = ERROR_MESSAGE(), @XSTATE = XACT_STATE();
					ROLLBACK TRANSACTION FRED_TOOLBOX_INSERTADMINUSER;

					RAISERROR('Fred_ToolBox_InsertAdminUser: %d: %s', 16, 1, @error, @message) ;
				END CATCH
			END
				END
			END
GO 



