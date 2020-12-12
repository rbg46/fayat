-- ===============================================================================================
		-- Mise à jour des libellés dans la table des tâches par ceux des familles d'ODs.
-- ===============================================================================================

DECLARE @Libelle NVARCHAR(250),
		@Code VARCHAR(6),
		@CiId INT,
		@Id  INT,
		@FamilleOperationDiverseId INT;

	DECLARE @tmpCiId  TABLE
	(
		[CiId] INT NOT NULL
	)
	-- Récupération de tous les CIs
	INSERT INTO @tmpCiId([CiId])
	SELECT  CiId FROM FRED_CI

	-- Pour chaque CIs
	WHILE EXISTS(SELECT TOP(1) 1 FROM @tmpCiId)
	BEGIN
		SELECT TOP 1 @CiId = CiId FROM @tmpCiId

			DECLARE @tmpFOD TABLE 
			(
				Id INT NOT NULL IDENTITY PRIMARY KEY,
				FamilleOperationDiverseId [INT] NOT NULL,
				Libelle [NVARCHAR](250) NULL,
				SocieteId [INT] NOT NULL,
				Code [VARCHAR](6) NULL
			)
			-- Recupération de toutes les familles d'OD pour un CI
			INSERT INTO @tmpFOD([FamilleOperationDiverseId],[SocieteId],[Libelle],[Code])
			SELECT fod.FamilleOperationDiverseId, fod.SocieteId, 'ECART ' + UPPER(fod.Libelle),
					CASE 
						WHEN UPPER(fod.Libelle) IN ('DÉBOURSÉ MAIN D’ŒUVRE (HORS INTÉRIM)','DEBOURSE MAIN D’ŒUVRE (HORS INTERIM)','DÉBOURSÉ MAIN D''OEUVRE (HORS INTÉRIM)' ,'DEBOURSE MAIN D''OEUVRE (HORS INTÉRIM)','DEBOURSE MAIN D''OEUVRE (HORS INTERIM)',UPPER('MO POINTEE (Hors Interim)'),UPPER('MO POINTEE (Hors Intérim)'))
							THEN '999991'
						WHEN UPPER(fod.Libelle) IN ('DÉBOURSÉ MATÉRIEL INTERNE','DEBOURSE MATERIEL INTERNE','MATERIEL INTERNE POINTE')  
							THEN '999993'
						WHEN UPPER(fod.Libelle) IN ('DÉBOURSÉ ACHATS RAPPROCHE (y compris Intérim)','DEBOURSE ACHATS RAPPROCHE (y compris Interim)','DÉBOURSÉ ACHATS AVEC COMMANDE (Y COMPRIS INTERIM)','ACHAT AVEC COMMANDE (Y compris interim)','ACHAT AVEC COMMANDE (Y compris intérim)')
							THEN '999994'
						WHEN UPPER(fod.Libelle) IN ('DÉBOURSÉ AUTRES DÉPENSES' ,'AUTRE DEPENSES SANS COMMANDE')
							THEN '999995'
						WHEN UPPER(fod.Libelle) IN ('AMORTISSEMENT','DÉBOURSÉ AMORTISSEMENT','DÉBOURSÉ AMORTISSEMENT','DEBOURSE AMMORTISSEMENT','MATERIEL IMMOBILISE')
							THEN '999998' 
						WHEN UPPER(fod.Libelle) IN ('RECETTES (Hors explo et CB)','RECETTES')
							THEN '999999'
					END
			FROM FRED_FAMILLE_OPERATION_DIVERSE fod
			INNER JOIN FRED_CI ci ON fod.SocieteId = ci.SocieteId
			WHERE ci.CiId = @CiId
			ORDER BY fod.Code

			WHILE EXISTS(SELECT TOP(1) 1 FROM @tmpFOD)
			BEGIN	
				SELECT TOP 1 @Id = Id,@FamilleOperationDiverseId = FamilleOperationDiverseId, @Libelle = Libelle, @Code = Code FROM @tmpFOD;

				IF @Code IS NOT NULL AND EXISTS(SELECT TOP(1) 1 FROM FRED_TACHE WHERE CiId = @CiId AND Niveau = 3 AND Code = @Code)
					BEGIN
						UPDATE FRED_TACHE
						SET LIBELLE = @Libelle
						WHERE CiId = @CiId AND Niveau = 3 AND Code = @Code
					END

				DELETE FROM @tmpFOD  WHERE Id = @Id;
			END
		DELETE FROM @tmpCiId  WHERE CiId = @CiId;
	END