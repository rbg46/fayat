﻿-- --------------------------------------------------
-- FRED 2017 - R3 - JUILLET 2018 
-- INJECTION DES DONNES POUR FRED - FAYAT FONDATIONS
-- CREATION DES CODES ZONE DEPLACEMENT POUR SEFI ET FRANKI
-- --------------------------------------------------


-- SUPPRESSION DES CODE MAJORATION SUITE MAIL NCA 05/07/2018 17:31
--DELETE FROM FRED_CODE_DEPLACEMENT WHERE SocieteId =  (SELECT SocieteId FROM  FRED_SOCIETE  WHERE Code = '500')
--DELETE FROM FRED_CODE_DEPLACEMENT WHERE SocieteId =  (SELECT SocieteId FROM  FRED_SOCIETE  WHERE Code = '700')