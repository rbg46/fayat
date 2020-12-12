-- =======================================================================================================================================
-- Description :	Liste des cas en écart : voir détails dans le ticket 14586 dans l'onglet 05 - Qualification par MMI le 03/09/20
-- =======================================================================================================================================

BEGIN TRANSACTION

BEGIN TRY

	-- Liste des 6 valorisations négatives à supprimer
	DECLARE @ValoIdsToDelete TABLE (Id INT);
	INSERT INTO @ValoIdsToDelete VALUES (302972), (302973), (315488), (315489), (315490), (315491);

	DELETE v
	FROM 
		FRED_VALORISATION v
		INNER JOIN @ValoIdsToDelete valo ON valo.Id = v.ValorisationId

	-- Mise à jour du numéro de la commande
	UPDATE c
	SET Numero = 'F' + RIGHT(REPLICATE('0', 9) + CAST(c.CommandeId AS VARCHAR(9)),9)
	FROM FRED_COMMANDE c
	WHERE CommandeId = 19108

	-- Mise à jour de la quantité de la dépense
	UPDATE da
	SET 
		Quantite = 33,
		DateSuppression = NULL
	FROM FRED_DEPENSE_ACHAT da
	WHERE DepenseId = 168275 

	-- Mise à jour de la date de suppression de la dépense
	UPDATE da
	SET DateSuppression = NULL
	FROM FRED_DEPENSE_ACHAT da
	WHERE DepenseId = 164594 

	-- Création des valorisations négatives
	DECLARE @DateNow DATETIME = GETDATE();
	DECLARE @ValoIdsToCreate TABLE (RapportId INT, RapportLigneId INT, Qte DECIMAL (18, 2));
	INSERT INTO @ValoIdsToCreate VALUES (447331, 1322750, 33), (416256, 1253772, 39);

	INSERT INTO FRED_VALORISATION (
		CiId,
		RapportId,
		RapportLigneId,
		TacheId,
		ChapitreId,
		SousChapitreId,
		ReferentielEtenduId,
		UniteId,
		DeviseId,
		PersonnelId,
		Date,
		VerrouPeriode,
		DateCreation,
		Source,
		PUHT,
		Quantite,
		Montant
	) SELECT
		CiId,
		valoToCreate.RapportId,
		valoToCreate.RapportLigneId,
		18664,
		ChapitreId,
		SousChapitreId,
		ReferentielEtenduId,
		UniteId,
		DeviseId,
		PersonnelId,
		Date,
		VerrouPeriode,
		@DateNow,
		'Annulation Intérimaire',
		PUHT,
		-valoToCreate.Qte,
		ROUND(PUHT * -valoToCreate.Qte, 2)
	FROM 
		FRED_VALORISATION v
		INNER JOIN @ValoIdsToCreate valoToCreate ON 
			valoToCreate.RapportId = v.RapportId
			AND valoToCreate.RapportLigneId = v.RapportLigneId

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