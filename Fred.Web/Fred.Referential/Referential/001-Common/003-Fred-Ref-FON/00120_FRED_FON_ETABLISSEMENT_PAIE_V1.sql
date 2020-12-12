-- --------------------------------------------------
-- FRED 2017 - R3 - JUILLET 2018 
-- INJECTION DES DONNES POUR FRED - FAYAT FONDATIONS
-- CREATION DES ETABLISSEMENTS PAIES
-- --------------------------------------------------


DECLARE @SOCIETE_ORGANISATION_ID_SEFI INT;
DECLARE @SOCIETE_ORGANISATION_ID_FRANKI INT;

-- SOCIETE  FRANKI
SET @SOCIETE_ORGANISATION_ID_FRANKI = (SELECT SocieteId FROM  FRED_SOCIETE  WHERE Code = '700')

-- SOCIETE  SEFI
SET @SOCIETE_ORGANISATION_ID_SEFI = (SELECT SocieteId FROM  FRED_SOCIETE  WHERE Code = '500')



INSERT INTO FRED_ETABLISSEMENT_PAIE (Code, Libelle, Adresse, Ville, CodePostal, PaysId, IsAgenceRattachement, GestionIndemnites, Actif, SocieteId, HorsRegion) VALUES 
('F700', 'FRANKI FONDATION', '9/11 Rue Gustave Eiffel', 'Grigny', '91350', 1,1,1,1, @SOCIETE_ORGANISATION_ID_FRANKI,0)
INSERT INTO FRED_ETABLISSEMENT_PAIE (Code, Libelle, Adresse, Ville, CodePostal, PaysId, IsAgenceRattachement, GestionIndemnites, Actif, SocieteId, HorsRegion) VALUES 
('F500', 'SEFI-INTRAFOR', '9/11 Rue Gustave Eiffel', 'Grigny', '91350', 1,1,1,1, @SOCIETE_ORGANISATION_ID_SEFI,0)

