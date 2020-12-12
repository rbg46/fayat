


-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

--SET IDENTITY_INSERT  FRED_TYPE_ORGANISATION ON
--INSERT INTO  FRED_TYPE_ORGANISATION(TypeOrganisationId, Code, Libelle) VALUES(1,'HOLDING','Holding')
--INSERT INTO  FRED_TYPE_ORGANISATION(TypeOrganisationId, Code, Libelle) VALUES(2,'POLE','Pôle')
--INSERT INTO  FRED_TYPE_ORGANISATION(TypeOrganisationId, Code, Libelle) VALUES(3,'GROUPE','Groupe')
--INSERT INTO  FRED_TYPE_ORGANISATION(TypeOrganisationId, Code, Libelle) VALUES(4,'SOCIETE','Société')
--INSERT INTO  FRED_TYPE_ORGANISATION(TypeOrganisationId, Code, Libelle) VALUES(5,'PUO','Périmètre UO')
--INSERT INTO  FRED_TYPE_ORGANISATION(TypeOrganisationId, Code, Libelle) VALUES(6,'UO','Unité Opérationnelle')
--INSERT INTO  FRED_TYPE_ORGANISATION(TypeOrganisationId, Code, Libelle) VALUES(7,'ETABLISSEMENT','Etablissement')
--INSERT INTO  FRED_TYPE_ORGANISATION(TypeOrganisationId, Code, Libelle) VALUES(8,'CI','Centre d''imputation')
--SET IDENTITY_INSERT  FRED_TYPE_ORGANISATION  OFF



-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- S39 - Types_Organisation
-- --------------------------------------------------
SET IDENTITY_INSERT  FRED_TYPE_ORGANISATION ON;

INSERT INTO  FRED_TYPE_ORGANISATION(TypeOrganisationId, Code, Libelle) VALUES(1,'HOLDING','Holding');
INSERT INTO  FRED_TYPE_ORGANISATION(TypeOrganisationId, Code, Libelle) VALUES(2,'POLE','Pôle');
INSERT INTO  FRED_TYPE_ORGANISATION(TypeOrganisationId, Code, Libelle) VALUES(3,'GROUPE','Groupe');
INSERT INTO  FRED_TYPE_ORGANISATION(TypeOrganisationId, Code, Libelle) VALUES(4,'SOCIETE','Société');
INSERT INTO  FRED_TYPE_ORGANISATION(TypeOrganisationId, Code, Libelle) VALUES(5,'PUO','Périmètre UO');
INSERT INTO  FRED_TYPE_ORGANISATION(TypeOrganisationId, Code, Libelle) VALUES(6,'UO','Unité Opérationnelle');
INSERT INTO  FRED_TYPE_ORGANISATION(TypeOrganisationId, Code, Libelle) VALUES(7,'ETABLISSEMENT','Etablissement');
INSERT INTO  FRED_TYPE_ORGANISATION(TypeOrganisationId, Code, Libelle) VALUES(8,'CI','Centre d''imputation');
SET IDENTITY_INSERT  FRED_TYPE_ORGANISATION  OFF;