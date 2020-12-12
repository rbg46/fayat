-- --------------------------------------------------
-- FRED 2017 - R3 - JUILLET 2018 
-- INJECTION DES DONNES POUR FRED - FAYAT FONDATIONS
-- CREATION DES CODES DEPLACEMENT POUR SEFI ET FRANKI
-- --------------------------------------------------



DECLARE @SOCIETE_ORGANISATION_ID_SEFI INT;
DECLARE @SOCIETE_ORGANISATION_ID_FRANKI INT;

-- SOCIETE  FRANKI
SET @SOCIETE_ORGANISATION_ID_FRANKI = (SELECT SocieteId FROM  FRED_SOCIETE  WHERE Code = '700')

-- SOCIETE  SEFI
SET @SOCIETE_ORGANISATION_ID_SEFI = (SELECT SocieteId FROM  FRED_SOCIETE  WHERE Code = '500')




-- **********************
-- INJECTION FRANKI
-- **********************
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (1408,'Heures voyage service',0,0,0,0,1,@SOCIETE_ORGANISATION_ID_FRANKI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (1410,'Forfait recherche pension',0,0,1,0,1,@SOCIETE_ORGANISATION_ID_FRANKI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (1431,'Indemnité temps de voyage WE',0,0,1,0,1,@SOCIETE_ORGANISATION_ID_FRANKI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (1705,'Repas IPD',0,0,0,0,1,@SOCIETE_ORGANISATION_ID_FRANKI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (1710,'Trajet IPD',0,0,0,0,1,@SOCIETE_ORGANISATION_ID_FRANKI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (1712,'Trajet +50 Kms',0,0,0,0,1,@SOCIETE_ORGANISATION_ID_FRANKI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (1716,'Amplitude trajet voyage inter/chantier',0,0,1,0,1,@SOCIETE_ORGANISATION_ID_FRANKI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (4220,'Carte Orange',0,0,0,0,1,@SOCIETE_ORGANISATION_ID_FRANKI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (4252,'IGD Vitrolles',0,0,1,0,1,@SOCIETE_ORGANISATION_ID_FRANKI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (4260,'Repas IGD',0,0,1,0,1,@SOCIETE_ORGANISATION_ID_FRANKI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (4262,'Nuitées IGD',0,0,1,0,1,@SOCIETE_ORGANISATION_ID_FRANKI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (4264,'Voyages week-end',0,0,1,0,1,@SOCIETE_ORGANISATION_ID_FRANKI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (4266,'Voyages Eloignement (présence)',0,0,1,0,1,@SOCIETE_ORGANISATION_ID_FRANKI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (4270,'Indemnité complémentaire nuitées IDF',0,0,1,0,1,@SOCIETE_ORGANISATION_ID_FRANKI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (4278,'Remboursement frais transport inter-chantiers',0,0,1,0,1,@SOCIETE_ORGANISATION_ID_FRANKI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (4279,'Voyages détente',0,0,1,0,1,@SOCIETE_ORGANISATION_ID_FRANKI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (4280,'IGD Vitrolles (-500 Kms)',0,0,1,0,1,@SOCIETE_ORGANISATION_ID_FRANKI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (4261,'Nuitées Région IDF (72, 92, 93, 94) sauf 91',0,0,0,0,1,@SOCIETE_ORGANISATION_ID_FRANKI)



-- **********************
-- INJECTION SEFI
-- **********************INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (1410,'Forfait recherche pension',0,0,NULL,NULL,1,@SOCIETE_ORGANISATION_ID_SEFI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (1705,'Repas ',0,0,0,0,1,@SOCIETE_ORGANISATION_ID_SEFI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (1710,'Trajet',0,0,0,0,1,@SOCIETE_ORGANISATION_ID_SEFI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (1714,'IPD Intermédiaires',0,0,0,0,1,@SOCIETE_ORGANISATION_ID_SEFI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (4220,'Carte Orange',0,0,0,0,1,@SOCIETE_ORGANISATION_ID_SEFI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (4260,'Repas',0,0,1,0,1,@SOCIETE_ORGANISATION_ID_SEFI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (4262,'Nuitées',0,0,1,0,1,@SOCIETE_ORGANISATION_ID_SEFI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (4264,'Voyages week-end',0,0,1,0,1,@SOCIETE_ORGANISATION_ID_SEFI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (4266,'Voyages Eloignement (présence)',0,0,1,0,1,@SOCIETE_ORGANISATION_ID_SEFI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (4267,'Indemnité Voyage Détente',0,0,1,0,1,@SOCIETE_ORGANISATION_ID_SEFI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (4270,'Indemnité complémentaire nuitées IDF',0,0,1,0,1,@SOCIETE_ORGANISATION_ID_SEFI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (4278,'Remboursement frais transport inter-chantiers',0,0,1,0,1,@SOCIETE_ORGANISATION_ID_SEFI)
INSERT INTO FRED_CODE_DEPLACEMENT (Code,Libelle,KmMini,KmMaxi,IGD,IndemniteForfaitaire,Actif,societeId) VALUES (4261,'Nuitées Région IDF (72, 92, 93, 94) sauf 91',0,0,0,0,1,@SOCIETE_ORGANISATION_ID_SEFI)
