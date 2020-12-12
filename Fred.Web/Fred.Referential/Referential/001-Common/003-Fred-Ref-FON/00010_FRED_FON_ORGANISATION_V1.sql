-- --------------------------------------------------
-- FRED 2017 - R3 - JUILLET 2018 
-- INJECTION DES DONNES POUR FRED - FAYAT FONDATIONS
-- CREATION DU POLE + GROUPE + SOCIETES + ETABLISSEMENT COMPTABLE POUR FONDATION
-- --------------------------------------------------



-- ***************************************
-- POLE
-- ***************************************

-- Clé organisation POLE 
DECLARE @POLE_ORGANISATION_ID int;

-- Création du POLE FONDATION
INSERT INTO FRED_ORGANISATION (TypeOrganisationId,PereId) VALUES ('2','1'); 
SET @POLE_ORGANISATION_ID = @@IDENTITY;
PRINT 'AJOUT POLE à HOLDING : ' 
PRINT @POLE_ORGANISATION_ID

-- Création de l'occurence dans la table FRED_POLE
SET IDENTITY_INSERT  FRED_POLE ON;
INSERT FRED_POLE(PoleId,Code,Libelle,HoldingId,OrganisationId) VALUES(4,'FON','FAYAT FONDATIONS',1,@POLE_ORGANISATION_ID);
SET IDENTITY_INSERT  FRED_POLE OFF;




-- ***************************************
-- GROUPE
-- ***************************************

-- Clé Organisation GROUPE
DECLARE @GROUPE_ORGANISATION_ID int;
DECLARE @GROUPE_ID int;
-- Création du GROUPE Pole Fondation dans la table FRED_ORGANISATION
INSERT INTO FRED_ORGANISATION (TypeOrganisationId,PereId) VALUES ('3',@POLE_ORGANISATION_ID); 
SET @GROUPE_ORGANISATION_ID = @@IDENTITY;

-- Création de l'occurence dans la table FRED_GROUPE
SET IDENTITY_INSERT  FRED_GROUPE ON;
INSERT INTO FRED_GROUPE(GroupeId,Code,Libelle,PoleId,OrganisationId) VALUES(4,'FON','FAYAT FONDATIONS',2,@GROUPE_ORGANISATION_ID);
SET @GROUPE_ID = @@IDENTITY;
SET IDENTITY_INSERT  FRED_GROUPE OFF;





-- ***************************************
-- SOCIETE
-- ***************************************

-- Clé Société Franki
DECLARE @SOCIETE_ORGANISATION_ID_FRANKI int;
DECLARE @SOCIETE_ID_FRANKI int;

-- Clé Société Sefi-Intrafor
DECLARE @SOCIETE_ORGANISATION_ID_SEFI int;
DECLARE @SOCIETE_ID_SEFI int;

-- Création de la société FRANKI dans la table FRED_ORGANISATION
INSERT INTO FRED_ORGANISATION (TypeOrganisationId,PereId) VALUES ('4',@GROUPE_ORGANISATION_ID); 
SET @SOCIETE_ORGANISATION_ID_FRANKI = @@IDENTITY;


-- Création de la société SEFI dans la table FRED_ORGANISATION
INSERT INTO FRED_ORGANISATION (TypeOrganisationId,PereId) VALUES ('4',@GROUPE_ORGANISATION_ID); 
SET @SOCIETE_ORGANISATION_ID_SEFI = @@IDENTITY;



-- ENREGISTREMENT FRANKI
INSERT INTO  FRED_SOCIETE (	GroupeId,		Code,				CodeSocietePaye,	CodeSocieteComptable,	Libelle,			Adresse,						Ville,				CodePostal,		SIRET,	Externe,	Active,		MoisDebutExercice,	MoisFinExercice,	IsGenerationSamediCPActive,		ImportFacture,		OrganisationId,						TransfertAS400,		ImageScreenLogin,		ImageLogoHeader,		ImageLogoId,		ImageLoginId,		IsInterimaire,		CodeSocieteStorm) 
VALUES (					@GROUPE_ID,		'700',				'NULL',				'NULL',					'FRANKI FONDATION',	'9/11 Rue Gustave Eiffel',		'Grigny',			'91350',		NULL,	0,			1,			10,					9,					0,								0,					@SOCIETE_ORGANISATION_ID_FRANKI,	0,					NULL,					NULL,					NULL,				NULL,				0,					'0169');
SET @SOCIETE_ID_FRANKI = @@IDENTITY;

-- ENREGISTREMENT SEFI
INSERT INTO  FRED_SOCIETE (	GroupeId,		Code,				CodeSocietePaye,	CodeSocieteComptable,	Libelle,			Adresse,						Ville,				CodePostal,		SIRET,	Externe,	Active,		MoisDebutExercice,	MoisFinExercice,	IsGenerationSamediCPActive,		ImportFacture,		OrganisationId,						TransfertAS400,		ImageScreenLogin,		ImageLogoHeader,		ImageLogoId,		ImageLoginId,		IsInterimaire,		CodeSocieteStorm) 
VALUES (					@GROUPE_ID,		'500',				'NULL',				'NULL',					'SEFI INTRAFOR',	'9/11 Rue Gustave Eiffel',		'Grigny',			'91350',		NULL,	0,			1,			10,					9,					0,								0,					@SOCIETE_ORGANISATION_ID_SEFI,		0,					NULL,					NULL,					NULL,				NULL,				0,					'0169');
SET @SOCIETE_ID_SEFI = @@IDENTITY;





-- ***************************************
-- CREATION DES SOCIETES INTERIMAIRES
-- ***************************************

-- Création des deux noeuds organisation 

DECLARE @ORGANISATION_ID_INTERIM_FRANKI INT; 
INSERT INTO FRED_ORGANISATION (TypeOrganisationId, PereId) VALUES (4,@GROUPE_ORGANISATION_ID)
SET @ORGANISATION_ID_INTERIM_FRANKI = (SELECT SCOPE_IDENTITY());

DECLARE @ORGANISATION_ID_INTERIM_SEFI INT; 
INSERT INTO FRED_ORGANISATION (TypeOrganisationId, PereId) VALUES (4,@GROUPE_ORGANISATION_ID)
SET @ORGANISATION_ID_INTERIM_SEFI = (SELECT SCOPE_IDENTITY());



INSERT INTO  FRED_SOCIETE (	GroupeId,		Code,				CodeSocietePaye,	CodeSocieteComptable,	Libelle,								Adresse,						Ville,				CodePostal,		SIRET,	Externe,	Active,		MoisDebutExercice,	MoisFinExercice,	IsGenerationSamediCPActive,		ImportFacture,		OrganisationId,							TransfertAS400,		ImageScreenLogin,		ImageLogoHeader,		ImageLogoId,		ImageLoginId,		IsInterimaire,		CodeSocieteStorm) 
VALUES (					@GROUPE_ID,		'IFF',				'NULL',				'NULL',					'SOCIETE INTERIMAIRE FRANKI FONDATION',	'2 avenue general de Gaulle',	'Viry-Chatillon',	'91170',		NULL,	0,			1,			NULL,				NULL,				0,								0,					@ORGANISATION_ID_INTERIM_FRANKI,		0,					NULL,					NULL,					NULL,				NULL,				1,					'0169');


INSERT INTO  FRED_SOCIETE (	GroupeId,		Code,				CodeSocietePaye,	CodeSocieteComptable,	Libelle,								Adresse,						Ville,				CodePostal,		SIRET,	Externe,	Active,		MoisDebutExercice,	MoisFinExercice,	IsGenerationSamediCPActive,		ImportFacture,		OrganisationId,							TransfertAS400,		ImageScreenLogin,		ImageLogoHeader,		ImageLogoId,		ImageLoginId,		IsInterimaire,		CodeSocieteStorm) 
VALUES (					@GROUPE_ID,		'IFS',				'NULL',				'NULL',					'SOCIETE INTERIMAIRE SEFI-INTRAFOR',	'2 avenue general de Gaulle',	'Viry-Chatillon',	'91170',		NULL,	0,			1,			NULL,				NULL,				0,								0,					@ORGANISATION_ID_INTERIM_SEFI,			0,					NULL,					NULL,					NULL,				NULL,				1,					'0169');






-- ***************************************
-- ETABLISSEMENT
-- ***************************************


-- Clé Société Franki
DECLARE @ETB_ORGANISATION_ID_FRANKI int;


-- Clé Société Sefi-Intrafor
DECLARE @ETB_ORGANISATION_ID_SEFI int;


-- Création de la société FRANKI dans la table FRED_ORGANISATION
INSERT INTO FRED_ORGANISATION (TypeOrganisationId,PereId) VALUES ('7',@SOCIETE_ORGANISATION_ID_FRANKI); 
SET @ETB_ORGANISATION_ID_FRANKI = @@IDENTITY;


-- Création de la société SEFI dans la table FRED_ORGANISATION
INSERT INTO FRED_ORGANISATION (TypeOrganisationId,PereId) VALUES ('7',@SOCIETE_ORGANISATION_ID_SEFI); 
SET @ETB_ORGANISATION_ID_SEFI = @@IDENTITY;



insert into FRED_ETABLISSEMENT_COMPTABLE (SocieteId,Code,Libelle,Adresse,Ville,CodePostal,ModuleCommandeEnabled,ModuleProductionEnabled,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId,IsDeleted,OrganisationId,PaysId) 
VALUES (@SOCIETE_ID_FRANKI, 'F700', 'FRANKI FONDATION', '9/11 Rue Gustave Eiffel', 'Grigny', '91350',0,0,GETDATE(),NULL,NULL,NULL,NULL,NULL,0,@ETB_ORGANISATION_ID_FRANKI,1);


insert into FRED_ETABLISSEMENT_COMPTABLE (SocieteId,Code,Libelle,Adresse,Ville,CodePostal,ModuleCommandeEnabled,ModuleProductionEnabled,DateCreation,DateModification,DateSuppression,AuteurCreationId,AuteurModificationId,AuteurSuppressionId,IsDeleted,OrganisationId,PaysId) 
VALUES (@SOCIETE_ID_SEFI, 'F500', 'SEFI-INTRAFOR', '9/11 Rue Gustave Eiffel', 'Grigny', '91350',0,0,GETDATE(),NULL,NULL,NULL,NULL,NULL,0,@ETB_ORGANISATION_ID_SEFI,1);

