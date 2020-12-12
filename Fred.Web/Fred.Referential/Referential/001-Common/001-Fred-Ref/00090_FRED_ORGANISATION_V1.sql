-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------


--SET IDENTITY_INSERT FRED_ORGANISATION ON;
---- HOLDING
--INSERT INTO FRED_ORGANISATION (OrganisationId,TypeOrganisationId,PereId) VALUES ('1','1',NULL); 
--INSERT INTO FRED_ORGANISATION (OrganisationId,TypeOrganisationId,PereId) VALUES ('2','1',NULL); 
---- POLE
--INSERT INTO FRED_ORGANISATION (OrganisationId,TypeOrganisationId,PereId) VALUES ('3','2','1'); 
--INSERT INTO FRED_ORGANISATION (OrganisationId,TypeOrganisationId,PereId) VALUES ('4','2','1'); 
---- GROUPE
--INSERT INTO FRED_ORGANISATION (OrganisationId,TypeOrganisationId,PereId) VALUES ('6','3','4'); 
---- SOCIETE
--INSERT INTO FRED_ORGANISATION (OrganisationId,TypeOrganisationId,PereId) VALUES ('9','4','6'); 

--SET IDENTITY_INSERT FRED_ORGANISATION OFF;




-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- S24 - Organisation_Structures
-- --------------------------------------------------
SET IDENTITY_INSERT  FRED_ORGANISATION ON;
INSERT INTO FRED_ORGANISATION (OrganisationId, TypeOrganisationId, PereId) VALUES (1,1,NULL);
INSERT INTO FRED_ORGANISATION (OrganisationId, TypeOrganisationId, PereId) VALUES (2,1,NULL);
INSERT INTO FRED_ORGANISATION (OrganisationId, TypeOrganisationId, PereId) VALUES (3,2,1);
INSERT INTO FRED_ORGANISATION (OrganisationId, TypeOrganisationId, PereId) VALUES (4,2,1);
INSERT INTO FRED_ORGANISATION (OrganisationId, TypeOrganisationId, PereId) VALUES (5,3,3);
INSERT INTO FRED_ORGANISATION (OrganisationId, TypeOrganisationId, PereId) VALUES (6,3,4);
INSERT INTO FRED_ORGANISATION (OrganisationId, TypeOrganisationId, PereId) VALUES (7,4,5);
INSERT INTO FRED_ORGANISATION (OrganisationId, TypeOrganisationId, PereId) VALUES (8,4,6);
INSERT INTO FRED_ORGANISATION (OrganisationId, TypeOrganisationId, PereId) VALUES (21,5,7);
INSERT INTO FRED_ORGANISATION (OrganisationId, TypeOrganisationId, PereId) VALUES (22,5,7);
INSERT INTO FRED_ORGANISATION (OrganisationId, TypeOrganisationId, PereId) VALUES (23,5,7);
INSERT INTO FRED_ORGANISATION (OrganisationId, TypeOrganisationId, PereId) VALUES (50,6,21);
INSERT INTO FRED_ORGANISATION (OrganisationId, TypeOrganisationId, PereId) VALUES (51,6,22);
INSERT INTO FRED_ORGANISATION (OrganisationId, TypeOrganisationId, PereId) VALUES (52,6,23);
INSERT INTO FRED_ORGANISATION (OrganisationId, TypeOrganisationId, PereId) VALUES (100,7,50);
INSERT INTO FRED_ORGANISATION (OrganisationId, TypeOrganisationId, PereId) VALUES (101,7,51);
INSERT INTO FRED_ORGANISATION (OrganisationId, TypeOrganisationId, PereId) VALUES (102,7,52);
SET IDENTITY_INSERT  FRED_ORGANISATION  OFF;
