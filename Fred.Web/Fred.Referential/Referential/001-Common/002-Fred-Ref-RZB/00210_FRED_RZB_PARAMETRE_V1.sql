

-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- S27 - Parametre_Systeme
-- --------------------------------------------------

SET IDENTITY_INSERT  FRED_PARAMETRE ON;
INSERT INTO  FRED_PARAMETRE (ParametreId,Code,Libelle,Valeur,GroupeId) VALUES  (2,2,'URL Scan Facture','http://frsacgedfact01v.fr.rz.lan:8080/SHARE_RAZEL/ps_download_file.jsp?docidxged=',1)
SET IDENTITY_INSERT FRED_PARAMETRE  OFF;