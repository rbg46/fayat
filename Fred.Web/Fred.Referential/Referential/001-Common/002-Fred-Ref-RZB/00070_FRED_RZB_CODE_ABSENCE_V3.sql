-- --------------------------------------------------
-- MEP 2018 - Correction des données Code 
-- --------------------------------------------------

-- REQUETE AYANT GENEREE LES DONNEES
--SELECT 'UPDATE FRED_CODE_ABSENCE'+
--' SET NBHeuresDefautETAM = ' + CAST( NBHeuresDefautCO AS varchar) +
--',NBHeuresDefautCO = ' + CAST(NBHeuresDefautETAM AS varchar) +
--',NBHeuresMaxETAM = ' + CAST(NBHeuresMaxCO AS varchar) +
--', NBHeuresMaxCO = ' + CAST(NBHeuresMaxETAM AS varchar) +
--', NBHeuresMinETAM = ' + CAST(NBHeuresMinCO AS varchar) +
--', NBHeuresMinCO = ' + CAST(NBHeuresMinETAM AS varchar) +
--' WHERE CodeAbsenceId = ' + CAST(CodeAbsenceId AS varchar) + ';'
-- FROM FRED_CODE_ABSENCE

UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 1,NBHeuresDefautCO = 1,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 1, NBHeuresMinCO = 1 WHERE CodeAbsenceId = 1;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 10, NBHeuresMaxCO = 7, NBHeuresMinETAM = 1, NBHeuresMinCO = 1 WHERE CodeAbsenceId = 2;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 3.5, NBHeuresMinCO = 7 WHERE CodeAbsenceId = 3;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 10, NBHeuresMaxCO = 10, NBHeuresMinETAM = 1, NBHeuresMinCO = 1 WHERE CodeAbsenceId = 4;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 10, NBHeuresMaxCO = 10, NBHeuresMinETAM = 1, NBHeuresMinCO = 1 WHERE CodeAbsenceId = 5;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 3.7, NBHeuresMinCO = 3.5 WHERE CodeAbsenceId = 6;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 0.5,NBHeuresDefautCO = 0.5,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 0.1, NBHeuresMinCO = 0.1 WHERE CodeAbsenceId = 7;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 7.4, NBHeuresMinCO = 7 WHERE CodeAbsenceId = 8;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 7.4, NBHeuresMinCO = 7 WHERE CodeAbsenceId = 9;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 7.4, NBHeuresMinCO = 7 WHERE CodeAbsenceId = 10;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 7.4, NBHeuresMinCO = 7 WHERE CodeAbsenceId = 11;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 10, NBHeuresMinETAM = 1, NBHeuresMinCO = 1 WHERE CodeAbsenceId = 12;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 8, NBHeuresMaxCO = 8, NBHeuresMinETAM = 7, NBHeuresMinCO = 7 WHERE CodeAbsenceId = 14;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 7.4, NBHeuresMinCO = 7 WHERE CodeAbsenceId = 15;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 1, NBHeuresMinCO = 1 WHERE CodeAbsenceId = 16;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 7.4, NBHeuresMinCO = 7 WHERE CodeAbsenceId = 17;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 7.4, NBHeuresMinCO = 7 WHERE CodeAbsenceId = 18;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 10, NBHeuresMaxCO = 7, NBHeuresMinETAM = 1, NBHeuresMinCO = 1 WHERE CodeAbsenceId = 19;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 8, NBHeuresMaxCO = 8, NBHeuresMinETAM = 3.5, NBHeuresMinCO = 3.5 WHERE CodeAbsenceId = 20;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 7.4, NBHeuresMinCO = 7 WHERE CodeAbsenceId = 21;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 0, NBHeuresMinCO = 0 WHERE CodeAbsenceId = 22;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7.4,NBHeuresMaxETAM = 8, NBHeuresMaxCO = 8, NBHeuresMinETAM = 3.5, NBHeuresMinCO = 3.7 WHERE CodeAbsenceId = 23;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 7.4, NBHeuresMinCO = 7 WHERE CodeAbsenceId = 24;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 3.5, NBHeuresMinCO = 1 WHERE CodeAbsenceId = 25;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 3.5, NBHeuresMinCO = 1 WHERE CodeAbsenceId = 26;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 8, NBHeuresMinETAM = 3.7, NBHeuresMinCO = 0.5 WHERE CodeAbsenceId = 27;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 8, NBHeuresMinETAM = 0, NBHeuresMinCO = 0 WHERE CodeAbsenceId = 28;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 8, NBHeuresMaxCO = 7, NBHeuresMinETAM = 7, NBHeuresMinCO = 7 WHERE CodeAbsenceId = 29;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 7.4, NBHeuresMinCO = 7 WHERE CodeAbsenceId = 30;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 3.7, NBHeuresMinCO = 3.5 WHERE CodeAbsenceId = 31;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 7.4, NBHeuresMinCO = 7 WHERE CodeAbsenceId = 32;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 7.4, NBHeuresMinCO = 7 WHERE CodeAbsenceId = 33;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 3.7, NBHeuresMinCO = 1 WHERE CodeAbsenceId = 34;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 3.7, NBHeuresMinCO = 3.5 WHERE CodeAbsenceId = 35;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 8, NBHeuresMaxCO = 8, NBHeuresMinETAM = 3.7, NBHeuresMinCO = 1 WHERE CodeAbsenceId = 36;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 8, NBHeuresMaxCO = 8, NBHeuresMinETAM = 1, NBHeuresMinCO = 1 WHERE CodeAbsenceId = 37;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 7.4, NBHeuresMinCO = 7 WHERE CodeAbsenceId = 39;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 0, NBHeuresMinCO = 0 WHERE CodeAbsenceId = 40;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 8, NBHeuresMaxCO = 8, NBHeuresMinETAM = 0, NBHeuresMinCO = 0 WHERE CodeAbsenceId = 41;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 1,NBHeuresDefautCO = 1,NBHeuresMaxETAM = 3.7, NBHeuresMaxCO = 3.5, NBHeuresMinETAM = 0.5, NBHeuresMinCO = 0.5 WHERE CodeAbsenceId = 43;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 8, NBHeuresMaxCO = 8, NBHeuresMinETAM = 7, NBHeuresMinCO = 7 WHERE CodeAbsenceId = 44;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 8, NBHeuresMaxCO = 8, NBHeuresMinETAM = 3, NBHeuresMinCO = 3 WHERE CodeAbsenceId = 45;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 8, NBHeuresMaxCO = 8, NBHeuresMinETAM = 7, NBHeuresMinCO = 7 WHERE CodeAbsenceId = 46;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 8, NBHeuresMaxCO = 8, NBHeuresMinETAM = 7, NBHeuresMinCO = 7 WHERE CodeAbsenceId = 47;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 8, NBHeuresMaxCO = 8, NBHeuresMinETAM = 0, NBHeuresMinCO = 0 WHERE CodeAbsenceId = 48;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 7.4, NBHeuresMinCO = 7 WHERE CodeAbsenceId = 49;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 12, NBHeuresMaxCO = 12, NBHeuresMinETAM = 7, NBHeuresMinCO = 1 WHERE CodeAbsenceId = 50;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 8, NBHeuresMaxCO = 8, NBHeuresMinETAM = 7, NBHeuresMinCO = 7 WHERE CodeAbsenceId = 51;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 8, NBHeuresMaxCO = 8, NBHeuresMinETAM = 7, NBHeuresMinCO = 7 WHERE CodeAbsenceId = 52;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 8, NBHeuresMaxCO = 7, NBHeuresMinETAM = 7, NBHeuresMinCO = 1 WHERE CodeAbsenceId = 53;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 8, NBHeuresMaxCO = 7, NBHeuresMinETAM = 3.5, NBHeuresMinCO = 1 WHERE CodeAbsenceId = 54;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 10, NBHeuresMaxCO = 10, NBHeuresMinETAM = 1, NBHeuresMinCO = 1 WHERE CodeAbsenceId = 55;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 0, NBHeuresMinCO = 0 WHERE CodeAbsenceId = 57;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7.4,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 7.4, NBHeuresMaxCO = 7, NBHeuresMinETAM = 3.4, NBHeuresMinCO = 3.5 WHERE CodeAbsenceId = 58;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 7,NBHeuresDefautCO = 7,NBHeuresMaxETAM = 9, NBHeuresMaxCO = 9, NBHeuresMinETAM = 2, NBHeuresMinCO = 2 WHERE CodeAbsenceId = 59;
UPDATE FRED_CODE_ABSENCE SET NBHeuresDefautETAM = 0,NBHeuresDefautCO = 0,NBHeuresMaxETAM = 9, NBHeuresMaxCO = 9, NBHeuresMinETAM = 2, NBHeuresMinCO = 0.5 WHERE CodeAbsenceId = 60;