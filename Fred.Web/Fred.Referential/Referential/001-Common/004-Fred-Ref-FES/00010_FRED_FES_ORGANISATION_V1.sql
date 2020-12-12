-- --------------------------------------------------
-- FRED 2017 - R3 - AOUT 2018 
-- INJECTION DES DONNES POUR FRED - FAYAT ENERGIES SERVICES
-- CREATION DU POLE + GROUPE + SOCIETES + ETABLISSEMENT COMPTABLE POUR FES
-- --------------------------------------------------

-- Champs génériques pour la gestion des organisations
DECLARE @ORGA_GENE_TYPE int;
DECLARE @ORGA_GENE_PERE int;


-- ***************************************
-- POLE
-- ***************************************

-- Clé organisation POLE 
DECLARE @POLE_ORGANISATION_ID int;
DECLARE @POLE_FES_ID int;
DECLARE @POLE_FES_CODE VARCHAR(20)='PFES'
DECLARE @POLE_FES_LIB VARCHAR(200)='FAYAT ENERGIES SERVICES'

-- Création du POLE ENERGIES
SELECT @ORGA_GENE_TYPE='2'	-- TYPE D'ORGANISATION POLE
SELECT @ORGA_GENE_PERE='1'	-- PERE = HOLDING FAYAT

IF NOT EXISTS (SELECT PoleId FROM FRED_POLE WHERE Code = @POLE_FES_CODE)
BEGIN
  INSERT INTO FRED_ORGANISATION (TypeOrganisationId,PereId) VALUES (@ORGA_GENE_TYPE,@ORGA_GENE_PERE); 
  SET @POLE_ORGANISATION_ID = @@IDENTITY;
  PRINT 'AJOUT POLE à HOLDING : ' + convert(varchar(20),@POLE_ORGANISATION_ID)

  -- Création de l'occurence dans la table FRED_POLE
  INSERT FRED_POLE(Code,Libelle,HoldingId,OrganisationId) VALUES(@POLE_FES_CODE,@POLE_FES_LIB,1,@POLE_ORGANISATION_ID);
  SET @POLE_FES_ID = @@IDENTITY;


  -- ***************************************
  -- GROUPE
  -- ***************************************

  -- Clé organisation GROUPE 
  DECLARE @GROUPE_ORGANISATION_ID int;
  DECLARE @GROUPE_FES_ID int;

  --------------------
  -- GROUPE SATELEC
  --------------------
  DECLARE @GROUPE_FES_CODE VARCHAR(20)='GSAT'
  DECLARE @GROUPE_FES_LIB VARCHAR(200)='GROUPE SATELEC'

  -- Création du GROUPE FES  dans la table FRED_ORGANISATION
  SELECT @ORGA_GENE_TYPE='3'						-- TYPE D'ORGANISATION GROUPE
  SELECT @ORGA_GENE_PERE=@POLE_ORGANISATION_ID	-- PERE = POLE ENERGIES

  INSERT INTO FRED_ORGANISATION (TypeOrganisationId,PereId) VALUES (@ORGA_GENE_TYPE,@ORGA_GENE_PERE); 
  SET @GROUPE_ORGANISATION_ID = @@IDENTITY;
  PRINT 'AJOUT GROUPE AU POLE: ' + convert(varchar(20),@GROUPE_ORGANISATION_ID)

  -- Création de l'occurence dans la table FRED_POLE
  INSERT INTO FRED_GROUPE(Code,Libelle,PoleId,OrganisationId) VALUES(@GROUPE_FES_CODE,@GROUPE_FES_LIB,@POLE_FES_ID,@GROUPE_ORGANISATION_ID);
  SET @GROUPE_FES_ID = @@IDENTITY;

  --------------------
  -- GROUPE SEMERU
  --------------------
  SELECT @GROUPE_FES_CODE='GSEM'
  SELECT @GROUPE_FES_LIB='GROUPE SEMERU'

  -- Création du GROUPE FES  dans la table FRED_ORGANISATION
  SELECT @ORGA_GENE_TYPE='3'						-- TYPE D'ORGANISATION GROUPE
  SELECT @ORGA_GENE_PERE=@POLE_ORGANISATION_ID	-- PERE = POLE ENERGIES

  INSERT INTO FRED_ORGANISATION (TypeOrganisationId,PereId) VALUES (@ORGA_GENE_TYPE,@ORGA_GENE_PERE); 
  SET @GROUPE_ORGANISATION_ID = @@IDENTITY;
  PRINT 'AJOUT GROUPE AU POLE: ' + convert(varchar(20),@GROUPE_ORGANISATION_ID)

  -- Création de l'occurence dans la table FRED_POLE
  INSERT INTO FRED_GROUPE(Code,Libelle,PoleId,OrganisationId) VALUES(@GROUPE_FES_CODE,@GROUPE_FES_LIB,@POLE_FES_ID,@GROUPE_ORGANISATION_ID);
  SET @GROUPE_FES_ID = @@IDENTITY;


  -- ***************************************
  -- SOCIETE
  -- ***************************************
  --------------------
  -- Clé Société SATELEC
  --------------------
  DECLARE @SOCIETE_ORGANISATION_ID int;
  DECLARE @SOCIETE_ID_SATELEC int;
  --------------------

  -- Création de la société SATELEC dans la table FRED_ORGANISATION
  SELECT @ORGA_GENE_TYPE='4'						-- TYPE D'ORGANISATION GROUPE
  SELECT @ORGA_GENE_PERE=@GROUPE_ORGANISATION_ID	-- PERE = GROUPE SATELEC

  INSERT INTO FRED_ORGANISATION (TypeOrganisationId,PereId) VALUES (@ORGA_GENE_TYPE,@ORGA_GENE_PERE); 
  SET @SOCIETE_ORGANISATION_ID = @@IDENTITY;

  DECLARE 
	   @SOC_Code varchar(200)='FES'
	  ,@SOC_CodeSocietePaye varchar(200)='001'
	  ,@SOC_CodeSocieteComptable varchar(200)='E001'
	  ,@SOC_Libelle varchar(200)='SATELEC'
	  ,@SOC_Adresse varchar(200)='24, avenue du Général-de-Gaulle '
	  ,@SOC_Ville varchar(200)='Viry-Châtillon'
	  ,@SOC_CodePostal varchar(200)='91178'
	  ,@SOC_SIRET varchar(200)=NULL
	  ,@SOC_Externe bit=0
	  ,@SOC_Active bit=1
	  ,@SOC_MoisDebutExercice int=5
	  ,@SOC_MoisFinExercice int=4
	  ,@SOC_IsGenerationSamediCPActive bit =1
	  ,@SOC_ImportFacture bit=0
	  ,@SOC_OrganisationId int=@SOCIETE_ORGANISATION_ID
	  ,@SOC_TransfertAS400 bit=0
	  ,@SOC_ImageScreenLogin varchar(200)=NULL
	  ,@SOC_ImageLogoHeader varchar(200)=NULL
	  ,@SOC_ImageLogoId int=NULL
	  ,@SOC_ImageLoginId int=NULL
	  ,@SOC_IsInterimaire bit=0
	  ,@SOC_CodeSocieteStorm varchar(200)='0084'

  INSERT INTO  FRED_SOCIETE (	GroupeId,Code,CodeSocietePaye,CodeSocieteComptable,Libelle,Adresse,Ville,CodePostal,SIRET,Externe,Active,MoisDebutExercice,MoisFinExercice,IsGenerationSamediCPActive,ImportFacture,OrganisationId,TransfertAS400,ImageScreenLogin,ImageLogoHeader,ImageLogoId,ImageLoginId,IsInterimaire,CodeSocieteStorm) 
  VALUES (@GROUPE_FES_ID,@SOC_Code,@SOC_CodeSocietePaye,@SOC_CodeSocieteComptable,@SOC_Libelle,@SOC_Adresse,@SOC_Ville,@SOC_CodePostal,@SOC_SIRET,@SOC_Externe,@SOC_Active,@SOC_MoisDebutExercice,@SOC_MoisFinExercice,@SOC_IsGenerationSamediCPActive,@SOC_ImportFacture,@SOC_OrganisationId,@SOC_TransfertAS400,@SOC_ImageScreenLogin,@SOC_ImageLogoHeader,@SOC_ImageLogoId,@SOC_ImageLoginId,@SOC_IsInterimaire,@SOC_CodeSocieteStorm )
  SET @SOCIETE_ID_SATELEC = @@IDENTITY;

  --------------------
  -- Clé Société SEMERU
  --------------------
  DECLARE @SOCIETE_ID_SEMERU int;

  -- Création de la société SATELEC dans la table FRED_ORGANISATION
  SELECT @ORGA_GENE_TYPE='4'						-- TYPE D'ORGANISATION GROUPE
  SELECT @ORGA_GENE_PERE=@GROUPE_ORGANISATION_ID	-- PERE = GROUPE SATELEC

  INSERT INTO FRED_ORGANISATION (TypeOrganisationId,PereId) VALUES (@ORGA_GENE_TYPE,@ORGA_GENE_PERE); 
  SET @SOCIETE_ORGANISATION_ID = @@IDENTITY;

  SELECT
	   @SOC_Code ='SEM'
	  ,@SOC_CodeSocietePaye ='002'
	  ,@SOC_CodeSocieteComptable ='E002'
	  ,@SOC_Libelle ='SEMERU'
	  ,@SOC_Adresse ='34 rue Charles Piketty'
	  ,@SOC_Ville ='Viry-Châtillon'
	  ,@SOC_CodePostal ='91170'
	  ,@SOC_SIRET =NULL
	  ,@SOC_Externe=0
	  ,@SOC_Active =1
	  ,@SOC_MoisDebutExercice =5
	  ,@SOC_MoisFinExercice =4
	  ,@SOC_IsGenerationSamediCPActive =1
	  ,@SOC_ImportFacture =0
	  ,@SOC_OrganisationId =@SOCIETE_ORGANISATION_ID
	  ,@SOC_TransfertAS400 =0
	  ,@SOC_ImageScreenLogin =NULL
	  ,@SOC_ImageLogoHeader =NULL
	  ,@SOC_ImageLogoId =NULL
	  ,@SOC_ImageLoginId =NULL
	  ,@SOC_IsInterimaire =0
	  ,@SOC_CodeSocieteStorm ='0069'

  INSERT INTO  FRED_SOCIETE (	GroupeId,Code,CodeSocietePaye,CodeSocieteComptable,Libelle,Adresse,Ville,CodePostal,SIRET,Externe,Active,MoisDebutExercice,MoisFinExercice,IsGenerationSamediCPActive,ImportFacture,OrganisationId,TransfertAS400,ImageScreenLogin,ImageLogoHeader,ImageLogoId,ImageLoginId,IsInterimaire,CodeSocieteStorm) 
  VALUES (@GROUPE_FES_ID,@SOC_Code,@SOC_CodeSocietePaye,@SOC_CodeSocieteComptable,@SOC_Libelle,@SOC_Adresse,@SOC_Ville,@SOC_CodePostal,@SOC_SIRET,@SOC_Externe,@SOC_Active,@SOC_MoisDebutExercice,@SOC_MoisFinExercice,@SOC_IsGenerationSamediCPActive,@SOC_ImportFacture,@SOC_OrganisationId,@SOC_TransfertAS400,@SOC_ImageScreenLogin,@SOC_ImageLogoHeader,@SOC_ImageLogoId,@SOC_ImageLoginId,@SOC_IsInterimaire,@SOC_CodeSocieteStorm )
  SET @SOCIETE_ID_SEMERU = @@IDENTITY;
END