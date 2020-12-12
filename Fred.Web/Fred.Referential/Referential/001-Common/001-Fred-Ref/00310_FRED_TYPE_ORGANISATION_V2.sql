
-- --------------------------------------------------
-- Ajout du nouveau type 'Sous CI' pour FES
-- --------------------------------------------------

SET IDENTITY_INSERT  FRED_TYPE_ORGANISATION ON;

INSERT INTO  FRED_TYPE_ORGANISATION(TypeOrganisationId, Code, Libelle) VALUES(9,'SCI','Sous centre d''imputation');

SET IDENTITY_INSERT  FRED_TYPE_ORGANISATION  OFF;