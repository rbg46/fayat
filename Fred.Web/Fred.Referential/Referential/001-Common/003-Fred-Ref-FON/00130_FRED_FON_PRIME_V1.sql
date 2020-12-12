-- --------------------------------------------------
-- FRED 2017 - R3 - JUILLET 2018 
-- INJECTION DES DONNES POUR FRED - FAYAT FONDATIONS
-- CREATION DES PRIMES POUR SEFI ET FRANKI
-- --------------------------------------------------


DECLARE @SOCIETE_ORGANISATION_ID_SEFI INT;
DECLARE @SOCIETE_ORGANISATION_ID_FRANKI INT;

-- SOCIETE  FRANKI
SET @SOCIETE_ORGANISATION_ID_FRANKI = (SELECT SocieteId FROM  FRED_SOCIETE  WHERE Code = '700')

-- SOCIETE  SEFI
SET @SOCIETE_ORGANISATION_ID_SEFI = (SELECT SocieteId FROM  FRED_SOCIETE  WHERE Code = '500')

PRINT @SOCIETE_ORGANISATION_ID_FRANKI;
PRINT @SOCIETE_ORGANISATION_ID_SEFI;





-- **********************
-- INJECTION FRANKI
-- **********************

SELECT * FROM FRED_PRIME

INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1000','Prime exceptionnelle',0,0,1,0,@SOCIETE_ORGANISATION_ID_FRANKI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1008','Prime de galerie/tacot',0,0,1,0,@SOCIETE_ORGANISATION_ID_FRANKI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1010','Prime d''égout',0,0,1,0,@SOCIETE_ORGANISATION_ID_FRANKI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1014','Prime bétonnage exceptionnel ',0,0,1,0,@SOCIETE_ORGANISATION_ID_FRANKI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1016','Indemnité samedis travaillés',0,0,1,0,@SOCIETE_ORGANISATION_ID_FRANKI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1028','Prime de boue',0,0,1,0,@SOCIETE_ORGANISATION_ID_FRANKI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1030','Prime de salissure',0,0,1,0,@SOCIETE_ORGANISATION_ID_FRANKI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1034','Prime de marteau',0,0,1,0,@SOCIETE_ORGANISATION_ID_FRANKI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1036','Tacot Ciment',0,0,1,0,@SOCIETE_ORGANISATION_ID_FRANKI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1040','Prime de Samedi',0,0,1,0,@SOCIETE_ORGANISATION_ID_FRANKI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1044','Prime Chargement/Déchargement',0,0,1,0,@SOCIETE_ORGANISATION_ID_FRANKI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1054','Prime bétonnage ',0,0,1,0,@SOCIETE_ORGANISATION_ID_FRANKI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1066','Prime de forage',0,0,1,0,@SOCIETE_ORGANISATION_ID_FRANKI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1404','Prime de production',0,0,1,0,@SOCIETE_ORGANISATION_ID_FRANKI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1406','Prime de production',0,0,1,0,@SOCIETE_ORGANISATION_ID_FRANKI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1407','Prime travaux extérieurs',0,0,1,0,@SOCIETE_ORGANISATION_ID_FRANKI,1)


-- **********************
-- INJECTION SEFI
-- **********************
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1000','Prime exceptionnelle',0,0,1,0,@SOCIETE_ORGANISATION_ID_SEFI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1008','Prime de galerie/tacot',0,0,1,0,@SOCIETE_ORGANISATION_ID_SEFI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1010','Prime d''égout',0,0,1,0,@SOCIETE_ORGANISATION_ID_SEFI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1014','Prime bétonnage exceptionnel ',0,0,1,0,@SOCIETE_ORGANISATION_ID_SEFI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1016','Indemnité samedis travaillés',0,0,1,0,@SOCIETE_ORGANISATION_ID_SEFI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1028','Prime de boue',0,0,1,0,@SOCIETE_ORGANISATION_ID_SEFI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1030','Prime de salissure',0,0,1,0,@SOCIETE_ORGANISATION_ID_SEFI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1034','Prime de marteau',0,0,1,0,@SOCIETE_ORGANISATION_ID_SEFI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1036','Tacot Ciment',0,0,1,0,@SOCIETE_ORGANISATION_ID_SEFI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1040','Prime de Samedi',0,0,1,0,@SOCIETE_ORGANISATION_ID_SEFI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1044','Prime Chargement/Déchargement',0,0,1,0,@SOCIETE_ORGANISATION_ID_SEFI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1054','Prime bétonnage ',0,0,1,0,@SOCIETE_ORGANISATION_ID_SEFI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1066','Prime de forage',0,0,1,0,@SOCIETE_ORGANISATION_ID_SEFI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1404','Prime de production',0,0,1,0,@SOCIETE_ORGANISATION_ID_SEFI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1406','Prime de production',0,0,1,0,@SOCIETE_ORGANISATION_ID_SEFI,1)
INSERT INTO FRED_PRIME (Code,Libelle,PrimeType,NombreHeuresMax,Actif,PrimePartenaire,societeId, Publique) VALUES ('1407','Prime travaux extérieurs',0,0,1,0,@SOCIETE_ORGANISATION_ID_SEFI,1)


