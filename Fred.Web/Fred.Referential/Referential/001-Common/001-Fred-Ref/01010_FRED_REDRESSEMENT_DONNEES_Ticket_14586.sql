-- =======================================================================================================================================
-- Description :	Liste des cas en écart : voir détails dans le ticket 14586
--					"MMI 03/08/2020
--					Suite aux tickets 14328, 14168 et 13489 qui ont déjà été déployés en production, il reste des cas non traités:
--					(voir fichiers Excel  dans ATTACHMENTS)"
-- =======================================================================================================================================

BEGIN TRANSACTION

BEGIN TRY

	-- Liste des 5 rapports à supprimer
	DECLARE @RapportIdsToUpdate TABLE (Id INT, Montant DECIMAL(18,2), Matricule VARCHAR(10));
	INSERT INTO @RapportIdsToUpdate VALUES (279396, 256, 'I_14390233'), (279396, 240, 'I_14390330'), (279396, 256, 'I_14390288'), (416256, -1209, 'I_14390389'), (447331, -1240, 'I_14390388');

	DELETE v
	FROM 
		FRED_VALORISATION v
		INNER JOIN FRED_PERSONNEL p on v.PersonnelId = p.PersonnelId
		INNER JOIN @RapportIdsToUpdate rapportValo ON 
			rapportValo.Id = v.RapportId
			AND rapportValo.Montant = v.Montant
			AND rapportValo.Matricule = p.Matricule

	DECLARE @DateNow DATETIME = GETDATE();
	DECLARE @CommentaireMajDepense VARCHAR(30) = 'Reprise Ticket 14586';

	-- Liste des 2 dépenses à mettre à jour
	DECLARE @DepensesToUpdate TABLE (Libelle VARCHAR(100), Quantite DECIMAL(18,2), PUHT NUMERIC(11,2));
	INSERT INTO @DepensesToUpdate VALUES ('S22 - Intérim Osvel OLIVA IZNAGA - 14390389 - 96248-1', 39, 31), ('S27 - Intérim Autilio LOPES TAVARES - 14390388 - 31312-2', 40, 31);
	
	UPDATE da
	SET
			DA.DateSuppression = @DateNow, 
			DA.Commentaire = @CommentaireMajDepense
	FROM
		FRED_DEPENSE_ACHAT da
		INNER JOIN @DepensesToUpdate depensesToUpdate ON 
		depensesToUpdate.Libelle = da.Libelle
		AND depensesToUpdate.Quantite = da.Quantite
		AND depensesToUpdate.PUHT = da.PUHT

END TRY  
BEGIN CATCH  
	SELECT   
		ERROR_NUMBER() AS ErrorNumber,
		ERROR_SEVERITY() AS ErrorSeverity,
		ERROR_STATE() AS ErrorState,
		ERROR_PROCEDURE() AS ErrorProcedure,
		ERROR_LINE() AS ErrorLine,
		ERROR_MESSAGE() AS ErrorMessage

	IF @@TRANCOUNT > 0 
		ROLLBACK TRANSACTION;
END CATCH
  
IF @@TRANCOUNT > 0  
	COMMIT TRANSACTION
GO