-- =======================================================================================================================================
-- Author:		Yoann Collet    18/03/2019
--
-- Description:
--      - Ajoute le statut de commande "préparée"
--
-- =======================================================================================================================================

SET IDENTITY_INSERT FRED_STATUT_COMMANDE ON;
INSERT INTO FRED_STATUT_COMMANDE (StatutCommandeId,Code,Libelle) VALUES ('6','PREP','Préparée'); 
SET IDENTITY_INSERT FRED_STATUT_COMMANDE OFF;
