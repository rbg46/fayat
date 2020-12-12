-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

----SET IDENTITY_INSERT FRED_ORGANISATION ON;
------ SOCIETE
----INSERT INTO FRED_ORGANISATION (OrganisationId,TypeOrganisationId,PereId) VALUES ('5','3','3'); 
----INSERT INTO FRED_ORGANISATION (OrganisationId,TypeOrganisationId,PereId) VALUES ('7','4','5'); 
------ FRED_ORGANISATION_GENERIQUE
----INSERT INTO FRED_ORGANISATION (OrganisationId,TypeOrganisationId,PereId) VALUES ('21','5','7'); 
----INSERT INTO FRED_ORGANISATION (OrganisationId,TypeOrganisationId,PereId) VALUES ('22','5','7'); 
----INSERT INTO FRED_ORGANISATION (OrganisationId,TypeOrganisationId,PereId) VALUES ('24','5','7'); 
----INSERT INTO FRED_ORGANISATION (OrganisationId,TypeOrganisationId,PereId) VALUES ('23','5','7'); 
----INSERT INTO FRED_ORGANISATION (OrganisationId,TypeOrganisationId,PereId) VALUES ('50','6','22'); 
----INSERT INTO FRED_ORGANISATION (OrganisationId,TypeOrganisationId,PereId) VALUES ('51','6','24'); 
------ FRED_ETABLISSEMENT_COMPTABLE
----INSERT INTO FRED_ORGANISATION (OrganisationId,TypeOrganisationId,PereId) VALUES ('60','7','50'); 
----INSERT INTO FRED_ORGANISATION (OrganisationId,TypeOrganisationId,PereId) VALUES ('61','7','51'); 
----SET IDENTITY_INSERT FRED_ORGANISATION OFF;




-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- --------------------------------------------------

-- Voir fichier 00090_FRED_ORGANISATION_V1.sql