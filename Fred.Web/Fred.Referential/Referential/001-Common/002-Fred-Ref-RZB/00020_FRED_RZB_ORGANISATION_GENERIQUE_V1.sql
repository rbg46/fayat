-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

--SET IDENTITY_INSERT FRED_ORGANISATION_GENERIQUE ON;
--INSERT INTO FRED_ORGANISATION_GENERIQUE (OrganisationGeneriqueId,OrganisationId,Code,Libelle) VALUES ('1','21','PUO_GDPRJ','Grands Projets /Export'); 
--INSERT INTO FRED_ORGANISATION_GENERIQUE (OrganisationGeneriqueId,OrganisationId,Code,Libelle) VALUES ('2','22','PUO_DRN','Direction Région Nord'); 
--INSERT INTO FRED_ORGANISATION_GENERIQUE (OrganisationGeneriqueId,OrganisationId,Code,Libelle) VALUES ('3','24','PUO_DRS','Direction Région Sud'); 
--INSERT INTO FRED_ORGANISATION_GENERIQUE (OrganisationGeneriqueId,OrganisationId,Code,Libelle) VALUES ('4','23','PUO_DIAF','Direction Internationale Afrique'); 
--INSERT INTO FRED_ORGANISATION_GENERIQUE (OrganisationGeneriqueId,OrganisationId,Code,Libelle) VALUES ('5','50','UO_LR','UO Languedoc-Roussillon'); 
--INSERT INTO FRED_ORGANISATION_GENERIQUE (OrganisationGeneriqueId,OrganisationId,Code,Libelle) VALUES ('6','51','UO_IDF_EST','UO IDF Est'); 
--SET IDENTITY_INSERT FRED_ORGANISATION_GENERIQUE OFF;




-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- S26 - Organisation_Generique
-- --------------------------------------------------
SET IDENTITY_INSERT  FRED_ORGANISATION_GENERIQUE ON;
INSERT INTO  FRED_ORGANISATION_GENERIQUE(OrganisationGeneriqueId, Code, Libelle,OrganisationId) VALUES(1,'PUO_GCNS','GCNS (Génie Civil Nucléaire & Services)',21);
INSERT INTO  FRED_ORGANISATION_GENERIQUE(OrganisationGeneriqueId, Code, Libelle,OrganisationId) VALUES(2,'PUO_DRN','Direction Région Nord',22);
INSERT INTO  FRED_ORGANISATION_GENERIQUE(OrganisationGeneriqueId, Code, Libelle,OrganisationId) VALUES(3,'PUO_DRS','Direction Région Sud',23);
INSERT INTO  FRED_ORGANISATION_GENERIQUE(OrganisationGeneriqueId, Code, Libelle,OrganisationId) VALUES(4,'UO_GCNS','GCNS (Génie Civil Nucléaire & Services)',50);
INSERT INTO  FRED_ORGANISATION_GENERIQUE(OrganisationGeneriqueId, Code, Libelle,OrganisationId) VALUES(5,'UO_IDF_OUEST','DRN - Agence IDF Ouest',51);
INSERT INTO  FRED_ORGANISATION_GENERIQUE(OrganisationGeneriqueId, Code, Libelle,OrganisationId) VALUES(6,'UO_LR','UO Languedoc',52);
SET IDENTITY_INSERT  FRED_ORGANISATION_GENERIQUE  OFF;
