-- =======================================================================================================================================
-- Description :	La commande pour le contrat 1436 n'a pas été crée (contrat d'un jour) 
--					pourtant il existe un pointage sur ce jour (rapportLigneId = 1240788)
-- =======================================================================================================================================

BEGIN TRANSACTION

BEGIN TRY

	DECLARE @DateNow DATETIME;
	SET @DateNow = GETDATE();

	DECLARE @DateRapport DATETIME;
	SET @DateRapport = '2020-05-15T00:00:00.000';

	DECLARE @RapportLigneToUpdate INT;
	SET @RapportLigneToUpdate = 1240788;

	SELECT 
		rl.AuteurCreationId,
		rl.RapportLigneId,
		rl.DatePointage,
		ci.PersonnelFournisseurSocieteId,
		ci.FournisseurId, 
		ci.DateDebut, 
		ci.NumContrat,
		ci.RessourceId,
		ci.Valorisation,
		ci.UniteId,
		ci.InterimaireId,
		c.CiId,
		c.AdresseLivraison,
		c.VilleLivraison,
		c.CodePostalLivraison,
		c.AdresseFacturation,
		c.VilleFacturation,
		c.CodePostalFacturation
	INTO #InfosRapportLigne1240788
	FROM 
		FRED_RAPPORT_LIGNE rl
		INNER JOIN FRED_CONTRAT_INTERIMAIRE ci ON ci.CiId = rl.CiId
			AND ci.InterimaireId = rl.PersonnelId
			AND ci.DateDebut >= @DateRapport 
			AND ci.DateFin <= @DateRapport
		INNER JOIN FRED_CI c ON c.CiId = ci.CiId
	WHERE 
		rl.RapportLigneId = @RapportLigneToUpdate

	-- Création de la commande
	INSERT INTO FRED_COMMANDE( 
		Numero, 
		CiId, 
		Libelle, 
		Date, 
		FournisseurId, 
		StatutCommandeId, 
		MOConduite, 
		EntretienMecanique, 
		EntretienJournalier, 
		Carburant, 
		Lubrifiant, 
		FraisAmortissement, 
		FraisAssurance, 
		ContactId, 
		SuiviId, 
		AuteurCreationId, 
		DateCreation, 
		LivraisonAdresse, 
		LivraisonVille, 
		LivraisonCPostale,
		FacturationAdresse,
		FacturationVille, 
		FacturationCPostale, 
		TypeId, 
		DeviseId, 
		AccordCadre, 
		CommandeManuelle, 
		NumeroContratExterne, 
		HangfireJobId, 
		IsAbonnement, 
		IsMaterielAPointer, 
		IsEnergie
	) SELECT
		'F000017800',
		I.CiId,
		'Commande Automatique - 31180-1 - 3514390',
		I.DateDebut,
		I.FournisseurId,
		3,
		0,
		0,
		0,
		0,
		0,
		0,
		0,
		I.AuteurCreationId,
		I.AuteurCreationId,
		9693,
		@DateNow,
		I.AdresseLivraison,
		I.VilleLivraison,
		I.CodePostalLivraison,
		I.AdresseFacturation,
		I.VilleFacturation,
		I.CodePostalFacturation,
		4,
		48,
		0,
		0,
		I.NumContrat,
		29509,
		0,
		0,
		0
	FROM #InfosRapportLigne1240788 I

	-- Création de la commande ligne
	DECLARE @CommandeId int = @@IDENTITY

	INSERT INTO FRED_COMMANDE_LIGNE ( 
		CommandeId, 
		Libelle, 
		TacheId, 
		RessourceId, 
		Quantite, 
		PUHT, 
		UniteId, 
		AuteurCreationId, 
		DateCreation, 
		IsVerrou
	) SELECT
		@CommandeId,
		'Intérim 14390388 LOPES TAVARES Autilio',
		18664,
		I.RessourceId,
		1,
		I.Valorisation,
		I.UniteId,
		I.AuteurCreationId,
		@DateNow,
		0
	FROM #InfosRapportLigne1240788 I

	-- Création de la commande contrat intérimaire
	DECLARE @CommandeLigneId int = @@IDENTITY

	INSERT INTO FRED_COMMANDE_CONTRAT_INTERIMAIRE ( 
		CommandeId, 
		ContratId,
		CiId, 
		InterimaireId, 
		RapportLigneId
	) SELECT
		@CommandeId,
		I.PersonnelFournisseurSocieteId,
		I.CiId,
		I.InterimaireId,
		I.RapportLigneId	
	FROM #InfosRapportLigne1240788 I

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
		@CommandeLigneId, 
		I.CiId, 
		I.FournisseurId, 
		'S20 - Intérim Autilio LOPES TAVARES - 14390388 - 31180-1',
		18664, 
		I.RessourceId, 
		'',
		7.000, 
		I.Valorisation, 
		I.UniteId, 
		I.DatePointage, 
		@DateNow, 
		2, 
		48, 
		'S202020-14390388', 
		0, 
		1, 
		'2020-06-01 00:00:00.000', 
		@DateNow,
		2,
		7.00,
		29510, 
		1, 
		1,
		1,
		0
	FROM #InfosRapportLigne1240788 I

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
		-Quantite,
		-Montant
	FROM FRED_VALORISATION
	WHERE 
		RapportLigneId = @RapportLigneToUpdate

	-- Mise à jour du flag ReceptionInterimaire et du contratId dans le rapport ligne concerné
	UPDATE rl SET rl.ReceptionInterimaire = 1, rl.ContratId = 1436
	FROM FRED_RAPPORT_LIGNE rl
	WHERE rl.RapportLigneId = @RapportLigneToUpdate

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

DROP TABLE #InfosRapportLigne1240788