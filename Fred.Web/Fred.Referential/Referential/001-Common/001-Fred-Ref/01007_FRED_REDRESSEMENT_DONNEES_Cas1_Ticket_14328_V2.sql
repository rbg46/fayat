-- =======================================================================================================================================
-- Description :	Les rapportLigneId 1224825, 1227261, 1228392 et 1230650 sont pourtant liés à des rapports verrouillés 
--					(405402, 406394, 406978 et 408035) mais n'ont pas de réception d’intérim associée 
--					(ReceptionInterimaire = 0 dans FRED_RAPPORT_LIGNE)
-- =======================================================================================================================================

BEGIN TRANSACTION

BEGIN TRY

	-- Liste des 4 rapports ligne à mettre à jour
	DECLARE @RapportLigneIdsToUpdate TABLE (Id INT);
	INSERT INTO @RapportLigneIdsToUpdate VALUES (1224825), (1227261), (1228392) , (1230650);

	DECLARE @DateCommande DATETIME = '2020-03-02T00:00:00.000';

	SELECT TOP 1 
		rl.DatePointage, 
		rl.CiId, 
		c.DeviseId, 
		c.FournisseurId, 
		cl.CommandeLigneId, 
		ci.RessourceId, 
		ci.UniteId, 
		ci.Valorisation
	INTO #InfosRapportLigne
	FROM 
		@RapportLigneIdsToUpdate RLITU
		INNER JOIN FRED_RAPPORT_LIGNE rl ON rl.RapportLigneId = RLITU.Id
		INNER JOIN FRED_COMMANDE c ON 
			c.CiId = rl.CiId 
			AND Date = @DateCommande 
			AND Libelle LIKE '%Commande Automatique - 31180%'
		INNER JOIN FRED_COMMANDE_LIGNE cl ON cl.CommandeId = c.CommandeId
		INNER JOIN FRED_COMMANDE_CONTRAT_INTERIMAIRE cci ON cci.CommandeId = c.CommandeId
		INNER JOIN FRED_CONTRAT_INTERIMAIRE ci ON ci.PersonnelFournisseurSocieteId = cci.ContratId
	ORDER BY 
		rl.DatePointage

	DECLARE @DateNow DATETIME = GETDATE();
	DECLARE @DateComptableReception DATETIME = '2020-06-01T00:00:00.000';
	DECLARE @DatePointage DATETIME = '2020-08-03T00:00:00.000';
	DECLARE @CommentaireMajDepense VARCHAR(100) = 'Reprise Ticket 14586 - Depense de Mai pointee sur Aout car periode cloturee';

	-- Création de la réception intérimaire
	INSERT INTO FRED_DEPENSE_ACHAT (
		CommandeLigneId, 
		CiId, 
		FournisseurId, 
		Libelle, 
		TacheId, 
		RessourceId, 
		Commentaire, 
		Quantite, 
		PUHT, 
		UniteId, 
		Date, 
		DateCreation, 
		AuteurCreationId, 
		DeviseId, 
		NumeroBL, 
		FarAnnulee, 
		DepenseTypeId, 
		DateComptable, 
		DateVisaReception, 
		AuteurVisaReceptionId, 
		QuantiteDepense, 
		HangfireJobId, 
		AfficherPuHt, 
		AfficherQuantite, 
		IsReceptionInterimaire, 
		IsReceptionMaterielExterne
	) SELECT 
		I.CommandeLigneId, 
		I.CiId, 
		I.FournisseurId, 
		'S20 - Intérim Autilio LOPES TAVARES - 14390388 - 31180',
		18664, 
		I.RessourceId, 
		@CommentaireMajDepense,
		32.000, 
		I.Valorisation, 
		I.UniteId, 
		@DatePointage, 
		@DateNow, 
		2, 
		I.DeviseId, 
		'S202020-14390388', 
		0, 
		1, 
		@DateComptableReception, 
		@DateNow,
		2,
		32.00,
		29510, 
		1, 
		1,
		1,
		0
	FROM #InfosRapportLigne I

	-- Création de la valorisation négative
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
		RapportId,
		RapportLigneId,
		18664,
		ChapitreId,
		SousChapitreId,
		ReferentielEtenduId,
		UniteId,
		DeviseId,
		PersonnelId,
		Date,
		1,
		@DateNow,
		'Annulation Intérimaire',
		PUHT,
		-32.000,
		-992.000
	FROM FRED_VALORISATION
	WHERE 
		RapportId = 405402 
		AND RapportLigneId = 1224825

	-- Mise à jour du flag ReceptionInterimaire dans les rapports ligne concernés
	UPDATE rl SET rl.ReceptionInterimaire = 1
	FROM 
		@RapportLigneIdsToUpdate RLITU
		INNER JOIN FRED_RAPPORT_LIGNE rl ON rl.RapportLigneId = RLITU.Id

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

DROP TABLE #InfosRapportLigne