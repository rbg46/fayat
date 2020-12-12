-- --------------------------------------------------
-- FRED 2017 - R3 - JUILLET 2018 
-- INJECTION DES DONNES POUR FRED - FAYAT FONDATIONS
-- CREATION DES PRIMES POUR SEFI ET FRANKI
-- --------------------------------------------------

-- Correction sur les PRIMES


--Instruction A passer si nécessaire
--DELETE FROM FRED_RAPPORT_LIGNE_PRIME WHERE RapportLignePrimeId IN (
--SELECT RapportLignePrimeId FROM FRED_RAPPORT_LIGNE_PRIME WHERE PrimeID in (Select PrimeId FROM FRED_PRIME where  SocieteId = (Select SocieteID FROM FRED_SOCIETE where Code = '700'))
--)


-- SUPPRESSION DES REFERENCES référencées dans les PRIMES insérées dans 00130_FRED_FON_PRIME
DELETE FROM FRED_PRIME WHERE  SocieteId = (Select SocieteID FROM FRED_SOCIETE where Code = '700')

-- INJECTION DES NOUVELLES DONNEES
INSERT INTO FRED_PRIME (Code, Libelle,PrimeType,NombreHeuresMax, Actif, PrimePartenaire, SocieteID, Publique) SELECT '1000', 'Prime exceptionnelle','0','0', '1', '0', SocieteID, '1' FROM FRED_SOCIETE Where Code = '700'
INSERT INTO FRED_PRIME (Code, Libelle,PrimeType,NombreHeuresMax, Actif, PrimePartenaire, SocieteID, Publique) SELECT '1016', 'Indemnité samedis travaillés','0','0', '1', '0', SocieteID, '1' FROM FRED_SOCIETE Where Code = '700'
INSERT INTO FRED_PRIME (Code, Libelle,PrimeType,NombreHeuresMax, Actif, PrimePartenaire, SocieteID, Publique) SELECT '1040', 'Prime de Samedi','0','0', '1', '0', SocieteID, '1' FROM FRED_SOCIETE Where Code = '700'
INSERT INTO FRED_PRIME (Code, Libelle,PrimeType,NombreHeuresMax, Actif, PrimePartenaire, SocieteID, Publique) SELECT '1062', 'Prime Béton (FRANKI)','0','0', '1', '0', SocieteID, '1' FROM FRED_SOCIETE Where Code = '700'
INSERT INTO FRED_PRIME (Code, Libelle,PrimeType,NombreHeuresMax, Actif, PrimePartenaire, SocieteID, Publique) SELECT '1404', 'Prime de production','0','0', '1', '0', SocieteID, '1' FROM FRED_SOCIETE Where Code = '700'
INSERT INTO FRED_PRIME (Code, Libelle,PrimeType,NombreHeuresMax, Actif, PrimePartenaire, SocieteID, Publique) SELECT '1406', 'Prime de production','0','0', '1', '0', SocieteID, '1' FROM FRED_SOCIETE Where Code = '700'
INSERT INTO FRED_PRIME (Code, Libelle,PrimeType,NombreHeuresMax, Actif, PrimePartenaire, SocieteID, Publique) SELECT '1407', 'Prime travaux extérieurs','0','0', '1', '0', SocieteID, '1' FROM FRED_SOCIETE Where Code = '700'
INSERT INTO FRED_PRIME (Code, Libelle,PrimeType,NombreHeuresMax, Actif, PrimePartenaire, SocieteID, Publique) SELECT '1420', 'Prime chargement/déchargement','0','0', '1', '0', SocieteID, '1' FROM FRED_SOCIETE Where Code = '700'

