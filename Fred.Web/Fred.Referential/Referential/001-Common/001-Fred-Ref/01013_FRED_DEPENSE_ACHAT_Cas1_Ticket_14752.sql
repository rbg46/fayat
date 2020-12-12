-- =======================================================================================================================================
-- Description :	3/ Matricule I_14390496
--					La commande n'existe pas pour le contrat (PersonnelfournisseurSocieteId = 1791) 
--					alors qu'un pointage a été effectué (RapportLigneId = 1334867)
-- =======================================================================================================================================

BEGIN TRANSACTION

BEGIN TRY

	DECLARE @DateNow DATETIME;
	SET @DateNow = GETDATE();

	DECLARE @DateDebutContrat DATETIME;
	SET @DateDebutContrat = '2020-07-07T00:00:00.000';

	DECLARE @DateFinContrat DATETIME;
	SET @DateFinContrat = '2020-07-10T00:00:00.000';

	DECLARE @RapportLigneToUpdate INT;
	SET @RapportLigneToUpdate = 1334867;

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
	INTO #InfosRapportsLigne
	FROM 
		FRED_RAPPORT_LIGNE rl
		INNER JOIN FRED_CONTRAT_INTERIMAIRE ci ON ci.CiId = rl.CiId
			AND ci.InterimaireId = rl.PersonnelId
			AND ci.DateDebut >= @DateDebutContrat 
			AND ci.DateFin <= @DateFinContrat
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
		IsAbonnement, 
		IsMaterielAPointer, 
		IsEnergie
	) SELECT
		'Temp',
		I.CiId,
		'Commande Automatique - 31922 - 351439',
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
		I.AuteurCreationId,
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
		0,
		0,
		0
	FROM #InfosRapportsLigne I

	-- Récupération de l'identifiant de la commande
	DECLARE @CommandeId int = @@IDENTITY

	-- Mise à jour du numéro de la commande créé précedemment
	UPDATE c
	SET Numero = 'F' + RIGHT(REPLICATE('0', 9) + CAST(c.CommandeId AS VARCHAR(9)),9)
	FROM FRED_COMMANDE c
	WHERE CommandeId = @CommandeId

	-- Création de la commande ligne
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
		'Intérim 14390496 NERY Eduardo',
		18664,
		I.RessourceId,
		1,
		I.Valorisation,
		I.UniteId,
		I.AuteurCreationId,
		@DateNow,
		0
	FROM #InfosRapportsLigne I

	-- Création de la commande contrat intérimaire
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
	FROM #InfosRapportsLigne I

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

DROP TABLE #InfosRapportsLigne;