-- --------------------------------------------------
-- FRED 2017 - R3 - JUILLET 2018 
-- INJECTION DES DONNES POUR FRED - FAYAT FONDATIONS
-- CREATION DES CODES ABSENCES POUR SEFI ET FRANKI
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
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('AJ',@SOCIETE_ORGANISATION_ID_FRANKI,'Accident de trajet',0,0,7,7,7,7,0,0,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('ANP',@SOCIETE_ORGANISATION_ID_FRANKI,'Absence non payée',0,0,0,0,0,7,0,0,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('AP',@SOCIETE_ORGANISATION_ID_FRANKI,'Absence payée',0,0,7,7,7,7,0,0,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('AT',@SOCIETE_ORGANISATION_ID_FRANKI,'Accident de travail',0,0,7,7,7,7,0,0,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('CA',@SOCIETE_ORGANISATION_ID_FRANKI,'Congé ancienneté',0,0,7,7,7,7,0,0,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('CF',@SOCIETE_ORGANISATION_ID_FRANKI,'Congé fractionnement',0,0,7,7,7,7,0,0,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('CONV',@SOCIETE_ORGANISATION_ID_FRANKI,'Congé conventionnel',0,0,7,7,7,7,0,0,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('CP',@SOCIETE_ORGANISATION_ID_FRANKI,'Congé payé',0,0,7,7,7,7,0,0,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('CPAR',@SOCIETE_ORGANISATION_ID_FRANKI,'Congé parental',0,0,7,7,7,7,0,0,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('CPR',@SOCIETE_ORGANISATION_ID_FRANKI,'Congé payé Avril',0,0,7,7,7,7,0,0,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('CSS',@SOCIETE_ORGANISATION_ID_FRANKI,'Congé sans solde',0,0,7,7,7,7,0,0,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('JDS',@SOCIETE_ORGANISATION_ID_FRANKI,'Journée de Solidarité',0,0,0,0,0,7,0,0,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('ML',@SOCIETE_ORGANISATION_ID_FRANKI,'Maladie non professionnelle',0,0,7,7,7,7,0,0,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('MP',@SOCIETE_ORGANISATION_ID_FRANKI,'Maladie professionnelle',0,0,7,7,7,7,0,0,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('MT',@SOCIETE_ORGANISATION_ID_FRANKI,'Maternité',0,0,7,7,7,7,0,0,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('PT',@SOCIETE_ORGANISATION_ID_FRANKI,'Paternité',0,0,7,7,7,7,0,0,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('PTNP',@SOCIETE_ORGANISATION_ID_FRANKI,'Paternité non payée',0,0,0,0,0,7,0,0,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('PTP',@SOCIETE_ORGANISATION_ID_FRANKI,'Paternité',0,0,0,0,0,7,0,0,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('RT',@SOCIETE_ORGANISATION_ID_FRANKI,'Congé Artt',0,0,0,0,0,3.5,0,0,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('RTE',@SOCIETE_ORGANISATION_ID_FRANKI,'Congé Artt employeur',0,0,0,0,0,3.5,0,0,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('RTS',@SOCIETE_ORGANISATION_ID_FRANKI,'Congé Artt salarié',0,0,7,3.5,7,3.5,0,0,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('EXP',@SOCIETE_ORGANISATION_ID_FRANKI,'Expatriation',0,0,7,7,7,0,0,0,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('FORM',@SOCIETE_ORGANISATION_ID_FRANKI,'Congé formation',0,0,7,7,7,0,0,0,1)



-- **********************
-- INJECTION SEFI
-- **********************

INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('AJ',@SOCIETE_ORGANISATION_ID_SEFI,'Accident de trajet',0,0,7,7,7,8,8,8,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('ANP',@SOCIETE_ORGANISATION_ID_SEFI,'Absence non payée',0,0,0,0,0,8,8,8,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('AP',@SOCIETE_ORGANISATION_ID_SEFI,'Absence payée',0,0,7,7,7,8,8,8,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('AT',@SOCIETE_ORGANISATION_ID_SEFI,'Accident de travail',0,0,7,7,7,8,8,8,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('CA',@SOCIETE_ORGANISATION_ID_SEFI,'Congé ancienneté',0,0,7,7,7,8,8,8,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('CF',@SOCIETE_ORGANISATION_ID_SEFI,'Congé fractionnement',0,0,7,7,7,8,8,8,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('CONV',@SOCIETE_ORGANISATION_ID_SEFI,'Congé conventionnel',0,0,7,7,7,8,8,8,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('CP',@SOCIETE_ORGANISATION_ID_SEFI,'Congé payé',0,0,7,7,7,8,8,8,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('CPAR',@SOCIETE_ORGANISATION_ID_SEFI,'Congé parental',0,0,7,7,7,8,8,8,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('CPR',@SOCIETE_ORGANISATION_ID_SEFI,'Congé payé Avril',0,0,7,7,7,8,8,8,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('CSS',@SOCIETE_ORGANISATION_ID_SEFI,'Congé sans solde',0,0,7,7,7,8,8,8,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('JDS',@SOCIETE_ORGANISATION_ID_SEFI,'Journée de Solidarité',0,0,0,0,0,8,8,8,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('ML',@SOCIETE_ORGANISATION_ID_SEFI,'Maladie non professionnelle',0,0,7,7,7,8,8,8,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('MP',@SOCIETE_ORGANISATION_ID_SEFI,'Maladie professionnelle',0,0,7,7,7,8,8,8,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('MT',@SOCIETE_ORGANISATION_ID_SEFI,'Maternité',0,0,7,7,7,8,8,8,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('PT',@SOCIETE_ORGANISATION_ID_SEFI,'Paternité',0,0,7,7,7,8,8,8,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('PTNP',@SOCIETE_ORGANISATION_ID_SEFI,'Paternité non payée',0,0,0,0,0,8,8,8,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('PTP',@SOCIETE_ORGANISATION_ID_SEFI,'Paternité',0,0,0,0,0,8,8,8,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('RT',@SOCIETE_ORGANISATION_ID_SEFI,'Congé Artt',0,0,0,0,0,8,3,8,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('RTE',@SOCIETE_ORGANISATION_ID_SEFI,'Congé Artt employeur',0,0,0,0,0,8,3,8,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('RTS',@SOCIETE_ORGANISATION_ID_SEFI,'Congé Artt salarié',0,0,7,3.5,7,8,3,8,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('RTVE',@SOCIETE_ORGANISATION_ID_SEFI,'Congé Artt employeur',0,0,0,0,0,3,3,3,1)
INSERT INTO FRED_CODE_ABSENCE (Code,societeId,Libelle,Intemperie,TauxDecote,NBHeuresDefautETAM,NBHeuresMinETAM,NBHeuresMaxETAM,NBHeuresDefautCO,NBHeuresMinCO,NBHeuresMaxCO,Actif) VALUES ('RTVS',@SOCIETE_ORGANISATION_ID_SEFI,'Congé Artt salarié',0,0,0,0,0,3,3,3,1)
