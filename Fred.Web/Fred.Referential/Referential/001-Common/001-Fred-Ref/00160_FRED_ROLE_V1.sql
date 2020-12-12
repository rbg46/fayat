-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

SET IDENTITY_INSERT FRED_ROLE ON;
INSERT INTO FRED_ROLE (RoleId,SocieteId,Code,Libelle,CommandeSeuilDefaut,ModeLecture,Actif,NiveauPaie,NiveauCompta, CodeNomFamilier) VALUES ('1','2','ADM','Admin Appli','0','0','1','999','999','ADM'); 
SET IDENTITY_INSERT FRED_ROLE OFF;



-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- --------------------------------------------------

-- Voir Fichier des 00230_FRED_DROITS_HABILITATIONS