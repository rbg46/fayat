-- =======================================================================================================================================
-- Author:		Yoann Collet  15/02/2019
--
-- Description:
--      - Mise à jour des numéro de ligne de commande sur les commandes validés
--
-- =======================================================================================================================================


-- Mise à jour des numéro de ligne de commande sur les commandes validées 


BEGIN TRAN

	DECLARE @commande_id int;
	DECLARE commande_cursor CURSOR FOR
		SELECT CommandeId FROM FRED_COMMANDE WHERE StatutCommandeId in (SELECT StatutCommandeId FROM FRED_STATUT_COMMANDE WHERE Code = 'VA' OR Code = 'CL')
	OPEN commande_cursor;
	FETCH NEXT FROM commande_cursor INTO @commande_id;
	WHILE @@FETCH_STATUS = 0 BEGIN

			DECLARE @commande_ligne_id int;
			DECLARE @numero int = 1;
			DECLARE commande_ligne_cursor CURSOR FOR
				SELECT CommandeLigneId FROM FRED_COMMANDE_LIGNE WHERE Commandeid = @commande_id
			OPEN commande_ligne_cursor;
			FETCH NEXT FROM commande_ligne_cursor INTO @commande_ligne_id;
			WHILE @@FETCH_STATUS = 0 BEGIN

					Update FRED_COMMANDE_LIGNE SET NumeroLigne = @numero WHERE CommandeLigneId = @commande_ligne_id

					SET @numero = @numero + 1;


					FETCH NEXT FROM commande_ligne_cursor INTO @commande_ligne_id;
			END
			CLOSE commande_ligne_cursor;
			DEALLOCATE commande_ligne_cursor;
		

		FETCH NEXT FROM commande_cursor INTO @commande_id;
	END
	CLOSE commande_cursor;
	DEALLOCATE commande_cursor;

COMMIT TRAN
