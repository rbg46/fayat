DECLARE @Code VARCHAR(6),
		@CiId INT,
		@DefaultTacheId INT,
		@FamilleOperationDiverseId INT;

	DECLARE @tmpCiId TABLE 
	(
		[CiId] INT NOT NULL
	)
	-- Récupération de tous les CIs
	INSERT INTO @tmpCiId([CiId])
	SELECT  CiId FROM FRED_CI

	-- Pour chaque CIs
	WHILE EXISTS(SELECT TOP(1) 1 FROM @tmpCiId)
	BEGIN
		SELECT @CiId = CiId FROM @tmpCiId
		SET  @DefaultTacheId = Null
				
			DECLARE @tmpFOD TABLE 
			(
				FamilleOperationDiverseId [INT] NOT NULL,
				Libelle [NVARCHAR](250) NULL,
				SocieteId [INT] NOT NULL,
				Code [VARCHAR](6) NULL
			)
			-- Recupération de toutes les familles d'OD pour un CI
			INSERT INTO @tmpFOD([FamilleOperationDiverseId],[SocieteId],[Libelle],[Code])
			SELECT  fod.FamilleOperationDiverseId, fod.SocieteId, 'ECART ' + UPPER(fod.Libelle),
				CASE 
					WHEN UPPER(fod.Libelle) IN ('DÉBOURSÉ MAIN D’ŒUVRE (HORS INTÉRIM)','DEBOURSE MAIN D’ŒUVRE (HORS INTERIM)','DÉBOURSÉ MAIN D''OEUVRE (HORS INTÉRIM)' ,'DEBOURSE MAIN D''OEUVRE (HORS INTÉRIM)','DEBOURSE MAIN D''OEUVRE (HORS INTERIM)',UPPER('MO POINTEE (Hors Interim)'),UPPER('MO POINTEE (Hors Intérim)'))
						THEN '999991'
					WHEN UPPER(fod.Libelle) IN ('DÉBOURSÉ MATÉRIEL INTERNE','DEBOURSE MATERIEL INTERNE','MATERIEL INTERNE POINTE')  
						THEN '999993'
					WHEN UPPER(fod.Libelle) IN ('DÉBOURSÉ ACHATS RAPPROCHE (Y COMPRIS INTERIM)','DEBOURSE ACHATS RAPPROCHE (Y COMPRIS INTÉRIM)','DÉBOURSÉ ACHATS AVEC COMMANDE (Y COMPRIS INTERIM)','ACHAT AVEC COMMANDE (Y COMPRIS INTERIM)','ACHAT AVEC COMMANDE (Y COMPRIS INTÉRIM)')
						THEN '999994'
					WHEN UPPER(fod.Libelle) IN ('DÉBOURSÉ AUTRES DÉPENSES' ,'AUTRE DEPENSES SANS COMMANDE','AUTRE FRAIS','AUTRES FRAIS','FRAIS GENERAUX (HORS CB)')
						THEN '999995'
					WHEN UPPER(fod.Libelle) IN ('ECART INTERIM')
						THEN '999996'
					WHEN UPPER(fod.Libelle) IN ('DÉBOURSÉ AMORTISSEMENT','DÉBOURSÉ AMORTISSEMENT','DEBOURSE AMMORTISSEMENT','MATERIEL IMMOBILISE')
						THEN '999998'
					WHEN UPPER(fod.Libelle) IN ('RECETTES (HORS EXPLO ET CB)','RECETTES')
						THEN '999999'
				END
			FROM FRED_FAMILLE_OPERATION_DIVERSE fod
			INNER JOIN FRED_CI ci ON fod.SocieteId = ci.SocieteId
			WHERE ci.CiId = @CiId
			ORDER BY fod.Code

			WHILE EXISTS(SELECT TOP(1) 1 FROM @tmpFOD)
			BEGIN	
				SELECT @FamilleOperationDiverseId = FamilleOperationDiverseId, @Code = Code FROM @tmpFOD;
									
				-- Récuperation de l'id de la tâche niveau 3 pour maj la tache par défaut d'une OD
				SELECT TOP(1) @DefaultTacheId = TacheId FROM FRED_TACHE WHERE CiId = @CiId AND Niveau = 3 AND Code = @Code

				-- Mise à jour de la la tâche par defaut de l'OD.
				IF(@DefaultTacheId IS NOT NULL)
				BEGIN
					UPDATE FRED_OPERATION_DIVERSE
					SET TacheId = @DefaultTacheId
					WHERE CiId = @CiId
					AND FamilleOperationDiverseId = @FamilleOperationDiverseId
					AND OdEcart = 1 ;
				END

				DELETE FROM @tmpFOD  WHERE FamilleOperationDiverseId = @FamilleOperationDiverseId;
			END
		DELETE FROM @tmpCiId  WHERE CiId = @CiId;
	END