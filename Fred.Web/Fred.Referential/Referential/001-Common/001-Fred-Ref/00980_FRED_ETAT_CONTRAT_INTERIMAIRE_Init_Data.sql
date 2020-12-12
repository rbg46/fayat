-- =======================================================================================================================================
-- Author:		BENNAI Naoufal  27/11/2019
--
-- Description:
--          RG_3465_013 : Init de la table FRED_ETAT_CONTRAT_INTERIMAIRE
--
-- =======================================================================================================================================


IF NOT EXISTS(SELECT * FROM [FRED_ETAT_CONTRAT_INTERIMAIRE] WHERE CODE = 'VAL')
BEGIN 
	INSERT INTO [FRED_ETAT_CONTRAT_INTERIMAIRE] (Code,Libelle)
	VALUES ('VAL','Valide')
END

IF NOT EXISTS(SELECT * FROM [FRED_ETAT_CONTRAT_INTERIMAIRE] WHERE CODE = 'BLQ')
BEGIN 
	INSERT INTO [FRED_ETAT_CONTRAT_INTERIMAIRE] (Code,Libelle)
	VALUES ('BLQ','Bloqué')
END