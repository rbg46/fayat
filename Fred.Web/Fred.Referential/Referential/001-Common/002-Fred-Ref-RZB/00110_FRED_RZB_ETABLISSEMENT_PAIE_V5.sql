-- --------------------------------------------------
-- STAIR - Injection des etablissements comptables des autres sociétés du groupe RZB pour import PERSO
-- --------------------------------------------------
DECLARE @SOCIETE_ID int;

-- Etablissement de paie pour les sociétés du groupe RZB
-- GEOBIO
SELECT @SOCIETE_ID = (SELECT SOCIETEID FROM FRED_SOCIETE WHERE GROUPEID=1 AND CODE='GEO')

INSERT INTO FRED_ETABLISSEMENT_PAIE (Code, Libelle, Adresse, IsAgenceRattachement, GestionIndemnites,HorsRegion, SocieteId, Actif)
VALUES ('53', 'ETABLISSEMENT PAYE GEOBIO', NULL, 1,1,1, @SOCIETE_ID,1);

-- LACHAUX
SELECT @SOCIETE_ID = (SELECT SOCIETEID FROM FRED_SOCIETE WHERE GROUPEID=1 AND CODE='LACH')

INSERT INTO FRED_ETABLISSEMENT_PAIE (Code, Libelle, Adresse, IsAgenceRattachement, GestionIndemnites,HorsRegion, SocieteId, Actif)
VALUES ('01', 'ETABLISSEMENT PAYE LACHAUX', NULL, 1,1,1, @SOCIETE_ID,1);

-- LHERM TP
SELECT @SOCIETE_ID = (SELECT SOCIETEID FROM FRED_SOCIETE WHERE GROUPEID=1 AND CODE='LHTP')

INSERT INTO FRED_ETABLISSEMENT_PAIE (Code, Libelle, Adresse, IsAgenceRattachement, GestionIndemnites,HorsRegion, SocieteId, Actif)
VALUES ('01', 'ETABLISSEMENT PAYE LHERM TP', NULL, 1,1,1, @SOCIETE_ID,1);

-- BIANCO
SELECT @SOCIETE_ID = (SELECT SOCIETEID FROM FRED_SOCIETE WHERE GROUPEID=1 AND CODE='BIAN')

INSERT INTO FRED_ETABLISSEMENT_PAIE (Code, Libelle, Adresse, IsAgenceRattachement, GestionIndemnites,HorsRegion, SocieteId, Actif)
VALUES ('01', 'ETABLISSEMENT PAYE BIANCO', NULL, 1,1,1, @SOCIETE_ID,1);

-- COTEG
SELECT @SOCIETE_ID = (SELECT SOCIETEID FROM FRED_SOCIETE WHERE GROUPEID=1 AND CODE='COTG')

INSERT INTO FRED_ETABLISSEMENT_PAIE (Code, Libelle, Adresse, IsAgenceRattachement, GestionIndemnites,HorsRegion, SocieteId, Actif)
VALUES ('01', 'ETABLISSEMENT PAYE COTEG', NULL, 1,1,1, @SOCIETE_ID,1);

select * from FRED_ETABLISSEMENT_PAIE order by FRED_ETABLISSEMENT_PAIE.EtablissementPaieId desc