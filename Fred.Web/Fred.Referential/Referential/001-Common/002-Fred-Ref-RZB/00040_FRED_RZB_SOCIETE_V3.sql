-- --------------------------------------------------
-- FRED 2017 - R3 - AOUT 2018 
-- INJECTION DES DONNES POUR FRED - GROUPE RZB
-- CREATION DES SOCIETES DANS LE CADRE DE l'UTILSATION DE FRED POUR STAIR
-- --------------------------------------------------

-- Champs génériques pour la gestion des organisations
DECLARE @ORGA_GENE_TYPE int;
DECLARE @ORGA_GENE_PERE int;


-- ***************************************
-- GROUPE
-- ***************************************

-- Clé organisation GROUPE 
DECLARE @GROUPE_ID_GENE int;
--------------------
-- RECUPERATION DU GROUPE RAZEL-BEC
--------------------
DECLARE @GROUPE_RZB_CODE VARCHAR(20)='GRZB'
SELECT @GROUPE_ID_GENE=(SELECT GROUPEID FROM FRED_GROUPE WHERE CODE=@GROUPE_RZB_CODE)

-- ***************************************
-- SOCIETE
-- ***************************************
--------------------
-- Definitions génériques
--------------------

DECLARE @SOCIETE_ORGANISATION_ID int;
DECLARE @SOCIETE_ID int;

DECLARE 
	 @SOC_Code varchar(200)
	,@SOC_CodeSocietePaye varchar(200)
	,@SOC_CodeSocieteComptable varchar(200)
	,@SOC_Libelle varchar(200)
	,@SOC_Adresse varchar(200)
	,@SOC_Ville varchar(200)
	,@SOC_CodePostal varchar(200)
	,@SOC_SIRET varchar(200)
	,@SOC_Externe bit
	,@SOC_Active bit
	,@SOC_MoisDebutExercice int
	,@SOC_MoisFinExercice int
	,@SOC_IsGenerationSamediCPActive bit
	,@SOC_ImportFacture bit
	,@SOC_OrganisationId int
	,@SOC_TransfertAS400 bit
	,@SOC_ImageScreenLogin varchar(200)
	,@SOC_ImageLogoHeader varchar(200)
	,@SOC_ImageLogoId int
	,@SOC_ImageLoginId int
	,@SOC_IsInterimaire bit
	,@SOC_CodeSocieteStorm varchar(200)

-- DEFINITION DU GROUPE PERE POUR TOUTES LES SOCIETES A CREER
SELECT @ORGA_GENE_TYPE='4'						-- TYPE D'ORGANISATION = SOCIETE
SELECT @ORGA_GENE_PERE=(SELECT ORGANISATIONID FROM FRED_GROUPE WHERE CODE=@GROUPE_RZB_CODE)

--------------------
-- Clé Société LHERMTP
--------------------
-- Création de la société LHERMTP dans la table FRED_ORGANISATION
INSERT INTO FRED_ORGANISATION (TypeOrganisationId,PereId) VALUES (@ORGA_GENE_TYPE,@ORGA_GENE_PERE); 
SET @SOCIETE_ORGANISATION_ID = @@IDENTITY;
	
SELECT
	 @SOC_Code ='LHTP'
	,@SOC_CodeSocietePaye ='LHE'
	,@SOC_CodeSocieteComptable ='200'
	,@SOC_Libelle ='LHERM TP'
	,@SOC_Adresse ='Chemin DUBAC - BP 10060'
	,@SOC_Ville ='Cugnaux'
	,@SOC_CodePostal ='31270'
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
	,@SOC_CodeSocieteStorm ='0271'

	INSERT INTO  FRED_SOCIETE (	GroupeId,Code,CodeSocietePaye,CodeSocieteComptable,Libelle,Adresse,Ville,CodePostal,SIRET,Externe,Active,MoisDebutExercice,MoisFinExercice,IsGenerationSamediCPActive,ImportFacture,OrganisationId,TransfertAS400,ImageScreenLogin,ImageLogoHeader,ImageLogoId,ImageLoginId,IsInterimaire,CodeSocieteStorm) 
VALUES (@GROUPE_ID_GENE,@SOC_Code,@SOC_CodeSocietePaye,@SOC_CodeSocieteComptable,@SOC_Libelle,@SOC_Adresse,@SOC_Ville,@SOC_CodePostal,@SOC_SIRET,@SOC_Externe,@SOC_Active,@SOC_MoisDebutExercice,@SOC_MoisFinExercice,@SOC_IsGenerationSamediCPActive,@SOC_ImportFacture,@SOC_OrganisationId,@SOC_TransfertAS400,@SOC_ImageScreenLogin,@SOC_ImageLogoHeader,@SOC_ImageLogoId,@SOC_ImageLoginId,@SOC_IsInterimaire,@SOC_CodeSocieteStorm )
SET @SOCIETE_ID = @@IDENTITY;

--------------------
-- Clé Société COTEG
--------------------
-- Création de la société LHERMTP dans la table FRED_ORGANISATION
INSERT INTO FRED_ORGANISATION (TypeOrganisationId,PereId) VALUES (@ORGA_GENE_TYPE,@ORGA_GENE_PERE); 
SET @SOCIETE_ORGANISATION_ID = @@IDENTITY;
	
SELECT
	 @SOC_Code ='COTG'
	,@SOC_CodeSocietePaye ='320'
	,@SOC_CodeSocieteComptable ='300'
	,@SOC_Libelle ='COTEG'
	,@SOC_Adresse ='219, rue des Marais'
	,@SOC_Ville ='Fontenay-sous-Bois'
	,@SOC_CodePostal ='94120'
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
	,@SOC_CodeSocieteStorm ='0010'

	INSERT INTO  FRED_SOCIETE (	GroupeId,Code,CodeSocietePaye,CodeSocieteComptable,Libelle,Adresse,Ville,CodePostal,SIRET,Externe,Active,MoisDebutExercice,MoisFinExercice,IsGenerationSamediCPActive,ImportFacture,OrganisationId,TransfertAS400,ImageScreenLogin,ImageLogoHeader,ImageLogoId,ImageLoginId,IsInterimaire,CodeSocieteStorm) 
VALUES (@GROUPE_ID_GENE,@SOC_Code,@SOC_CodeSocietePaye,@SOC_CodeSocieteComptable,@SOC_Libelle,@SOC_Adresse,@SOC_Ville,@SOC_CodePostal,@SOC_SIRET,@SOC_Externe,@SOC_Active,@SOC_MoisDebutExercice,@SOC_MoisFinExercice,@SOC_IsGenerationSamediCPActive,@SOC_ImportFacture,@SOC_OrganisationId,@SOC_TransfertAS400,@SOC_ImageScreenLogin,@SOC_ImageLogoHeader,@SOC_ImageLogoId,@SOC_ImageLoginId,@SOC_IsInterimaire,@SOC_CodeSocieteStorm )
SET @SOCIETE_ID = @@IDENTITY;

--------------------
-- Clé Société LACHAUX
--------------------
-- Création de la société LHERMTP dans la table FRED_ORGANISATION
INSERT INTO FRED_ORGANISATION (TypeOrganisationId,PereId) VALUES (@ORGA_GENE_TYPE,@ORGA_GENE_PERE); 
SET @SOCIETE_ORGANISATION_ID = @@IDENTITY;
	
SELECT
	 @SOC_Code ='LACH'
	,@SOC_CodeSocietePaye ='800'
	,@SOC_CodeSocieteComptable ='800'
	,@SOC_Libelle ='LACHAUX'
	,@SOC_Adresse ='Rue de l''Etang'
	,@SOC_Ville ='VILLEVAUDE'
	,@SOC_CodePostal ='77410'
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
	,@SOC_CodeSocieteStorm ='0034'

	INSERT INTO  FRED_SOCIETE (	GroupeId,Code,CodeSocietePaye,CodeSocieteComptable,Libelle,Adresse,Ville,CodePostal,SIRET,Externe,Active,MoisDebutExercice,MoisFinExercice,IsGenerationSamediCPActive,ImportFacture,OrganisationId,TransfertAS400,ImageScreenLogin,ImageLogoHeader,ImageLogoId,ImageLoginId,IsInterimaire,CodeSocieteStorm) 
VALUES (@GROUPE_ID_GENE,@SOC_Code,@SOC_CodeSocietePaye,@SOC_CodeSocieteComptable,@SOC_Libelle,@SOC_Adresse,@SOC_Ville,@SOC_CodePostal,@SOC_SIRET,@SOC_Externe,@SOC_Active,@SOC_MoisDebutExercice,@SOC_MoisFinExercice,@SOC_IsGenerationSamediCPActive,@SOC_ImportFacture,@SOC_OrganisationId,@SOC_TransfertAS400,@SOC_ImageScreenLogin,@SOC_ImageLogoHeader,@SOC_ImageLogoId,@SOC_ImageLoginId,@SOC_IsInterimaire,@SOC_CodeSocieteStorm )
SET @SOCIETE_ID = @@IDENTITY;

--------------------
-- Clé Société GEOBIO
--------------------
-- Création de la société LHERMTP dans la table FRED_ORGANISATION
INSERT INTO FRED_ORGANISATION (TypeOrganisationId,PereId) VALUES (@ORGA_GENE_TYPE,@ORGA_GENE_PERE); 
SET @SOCIETE_ORGANISATION_ID = @@IDENTITY;
	
SELECT
	 @SOC_Code ='GEO'
	,@SOC_CodeSocietePaye ='GEO'
	,@SOC_CodeSocieteComptable ='1600'
	,@SOC_Libelle ='GEOBIO'
	,@SOC_Adresse ='137 rue claude balbastre'
	,@SOC_Ville ='Montpellier'
	,@SOC_CodePostal ='34070'
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
	,@SOC_CodeSocieteStorm ='NOT_DEFINED'

	INSERT INTO  FRED_SOCIETE (	GroupeId,Code,CodeSocietePaye,CodeSocieteComptable,Libelle,Adresse,Ville,CodePostal,SIRET,Externe,Active,MoisDebutExercice,MoisFinExercice,IsGenerationSamediCPActive,ImportFacture,OrganisationId,TransfertAS400,ImageScreenLogin,ImageLogoHeader,ImageLogoId,ImageLoginId,IsInterimaire,CodeSocieteStorm) 
VALUES (@GROUPE_ID_GENE,@SOC_Code,@SOC_CodeSocietePaye,@SOC_CodeSocieteComptable,@SOC_Libelle,@SOC_Adresse,@SOC_Ville,@SOC_CodePostal,@SOC_SIRET,@SOC_Externe,@SOC_Active,@SOC_MoisDebutExercice,@SOC_MoisFinExercice,@SOC_IsGenerationSamediCPActive,@SOC_ImportFacture,@SOC_OrganisationId,@SOC_TransfertAS400,@SOC_ImageScreenLogin,@SOC_ImageLogoHeader,@SOC_ImageLogoId,@SOC_ImageLoginId,@SOC_IsInterimaire,@SOC_CodeSocieteStorm )
SET @SOCIETE_ID = @@IDENTITY;

--------------------
-- Clé Société BIANCO
--------------------
-- Création de la société LHERMTP dans la table FRED_ORGANISATION
INSERT INTO FRED_ORGANISATION (TypeOrganisationId,PereId) VALUES (@ORGA_GENE_TYPE,@ORGA_GENE_PERE); 
SET @SOCIETE_ORGANISATION_ID = @@IDENTITY;
	
SELECT
	 @SOC_Code ='BIAN'
	,@SOC_CodeSocietePaye ='620'
	,@SOC_CodeSocieteComptable ='6200'
	,@SOC_Libelle ='BIANCO'
	,@SOC_Adresse ='69 Route du Chef Lieu'
	,@SOC_Ville ='MARTHOD'
	,@SOC_CodePostal ='73402'
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
	,@SOC_CodeSocieteStorm ='0024'

	INSERT INTO  FRED_SOCIETE (	GroupeId,Code,CodeSocietePaye,CodeSocieteComptable,Libelle,Adresse,Ville,CodePostal,SIRET,Externe,Active,MoisDebutExercice,MoisFinExercice,IsGenerationSamediCPActive,ImportFacture,OrganisationId,TransfertAS400,ImageScreenLogin,ImageLogoHeader,ImageLogoId,ImageLoginId,IsInterimaire,CodeSocieteStorm) 
VALUES (@GROUPE_ID_GENE,@SOC_Code,@SOC_CodeSocietePaye,@SOC_CodeSocieteComptable,@SOC_Libelle,@SOC_Adresse,@SOC_Ville,@SOC_CodePostal,@SOC_SIRET,@SOC_Externe,@SOC_Active,@SOC_MoisDebutExercice,@SOC_MoisFinExercice,@SOC_IsGenerationSamediCPActive,@SOC_ImportFacture,@SOC_OrganisationId,@SOC_TransfertAS400,@SOC_ImageScreenLogin,@SOC_ImageLogoHeader,@SOC_ImageLogoId,@SOC_ImageLoginId,@SOC_IsInterimaire,@SOC_CodeSocieteStorm )
SET @SOCIETE_ID = @@IDENTITY;

