






/*PROCEDURE STOCKEES*/
IF OBJECT_ID ( 'Fred_ToolBox_CI', 'P' ) IS NOT NULL   
    DROP PROCEDURE Fred_ToolBox_CI;  
GO  
CREATE PROCEDURE Fred_ToolBox_CI
    @Code nvarchar(50),   
    @Libelle nvarchar(50), 
	@SocieteCode nvarchar(50), 
	@EtablissementCode nvarchar(50),
	@CITypeCode nvarchar(max)=NULL,
	@GroupeCode nvarchar(max),
	@CodePostal nvarchar(max)=NULL

AS   

	DECLARE @SocieteId int;
	DECLARE @EtablissementComptableId int;
	DECLARE @OrganisationPereId int;
	DECLARE @FacturationEtablissement int;
	DECLARE @CITypeId int;
	DECLARE @ERROR INT; 

	SET @ERROR = 0;

	-- RECHERCHE ID SOCIETE
	SET @SocieteId = (SELECT SocieteId FROM FRED_SOCIETE WHERE code = @SocieteCode AND GroupeId =(SELECT GroupeId FROM FREd_GROUPE where Code = @GroupeCode));
	IF(@SocieteId IS NULL)
	BEGIN
		SET @ERROR = 1;
		PRINT 'ERROR : Société non identifié. Ajout CI annulé'
	END 
	

	

	-- RECHERCHE ETABLISSEMENT En fonction du Code Etablissement et de la société
	SET @EtablissementComptableId = (SELECT EtablissementComptableId FROM FRED_ETABLISSEMENT_COMPTABLE WHERE Code = @EtablissementCode AND SocieteID = @SocieteId);
	IF(@EtablissementComptableId IS NULL)
	BEGIN
		SET @ERROR = 1;
		PRINT 'ERROR : Etablissement '+@EtablissementCode+' non identifié pour société '+@SocieteCode+'. Ajout CI annulé'
	END 


	SET @FacturationEtablissement = @EtablissementComptableId;


	-- RECHERCHE ID ORGANISATION DE L'ETABLISSEMENT
	SET @OrganisationPereId = (SELECT OrganisationId FROM FRED_ETABLISSEMENT_COMPTABLE WHERE Code = @EtablissementCode);
	IF(@OrganisationPereId IS NULL)
	BEGIN
		SET @ERROR = 1;
		PRINT 'ERROR : ID Organisation Etablissement non identifié. Ajout CI annulé'
	END 



	-- GESTION DU TYPE DE CI
	SET @CITypeId = (SELECT CITypeId  FROM FRED_CI_TYPE WHERE UPPER(Designation) = UPPER(@CITypeCode))


	--RECUPERATION DE LA CLE DU CI en FONCTION DE LA SOCIETE
	DECLARE @CiID INT;
	SET @CiID = (SELECT CiID FROM FRED_CI WHERE Code=@Code AND SocieteId = @SocieteId)



	IF @ERROR =0
	BEGIN
		IF(@CiID IS NULL)
			BEGIN
				-- ---------------
				-- CREATION DU CI
				-- Création du CI dans organisation
				DECLARE @CI_ORGANISATION_ID int;
				INSERT INTO FRED_ORGANISATION (TypeOrganisationId,PereId) VALUES ('8',@OrganisationPereId); 
				SET @CI_ORGANISATION_ID = @@IDENTITY;
	 

				-- INSERTION CENTRE IMPUTATION

				DECLARE @CI_ID int;
				INSERT INTO FRED_CI (
					Code, 
					Libelle, 
					Sep, 
					EtablissementComptableId, 
					FacturationEtablissement ,
					SocieteId, 
					OrganisationId, 
					ChantierFRED,
					CITypeId,
					CodePostal
					)
				VALUES (
					@Code,
					@Libelle,
					0,
					@EtablissementComptableId,
					@FacturationEtablissement ,
					@SocieteId, 
					@CI_ORGANISATION_ID, 
					0,
					@CITypeId,
					@CodePostal)
				SET @CI_ID = @@IDENTITY;


				-- INSERTION DES TACHES PAR DEFAUt
				DECLARE @TACHE_ID_N1 int; 
				DECLARE @TACHE_ID_N2 int; 
				DECLARE @TACHE_ID_N3 int; 


				-- CREATION NIVEAU 1
				INSERT INTO FRED_TACHE ([Code],		[Libelle],				[TacheParDefaut],	[Niveau], [Active],	[AuteurCreationId], [DateCreation], [CiId],		[ParentId],		[TacheType]) 
				VALUES (				'00',		'TACHE PAR DEFAUT',		0,					1,			1,			1,					GETDATE(),		@CI_ID,		NULL,			0)
				SET @TACHE_ID_N1 = @@IDENTITY;

				-- CREATION NIVEAU 2
				INSERT INTO FRED_TACHE ([Code],		[Libelle],				[TacheParDefaut],	[Niveau], [Active],	[AuteurCreationId], [DateCreation], [CiId],		[ParentId],			[TacheType]) 
				VALUES (				'0000',		'TACHE PAR DEFAUT',		0,					2,			1,			1,					GETDATE(),		@CI_ID,		@TACHE_ID_N1,		0)
				SET @TACHE_ID_N2 = @@IDENTITY;


				-- CREATION NIVEAU 3
				INSERT INTO FRED_TACHE ([Code],		[Libelle],				[TacheParDefaut],	[Niveau], [Active],	[AuteurCreationId], [DateCreation], [CiId],		[ParentId],			[TacheType]) 
				VALUES (				'000000',	'TACHE PAR DEFAUT',		1,					3,			1,			1,					GETDATE(),		@CI_ID,		@TACHE_ID_N2,		0)

				PRINT 'INFO : Ci Ajouté avec Plan de Tâche par Défaut'

				-- FIN CREATION DU CI
				-- ---------------
			END
		ELSE
			BEGIN
				-- MISE A JOUR DU CI
				PRINT 'INFO : MISE A JOUR'

				UPDATE FRED_CI
				SET
					Libelle = @Libelle, 
					CITypeId = @CITypeId,
					CodePostal =@CodePostal 

				WHERE
				CiId = @CiID

			END
	END




GO 

