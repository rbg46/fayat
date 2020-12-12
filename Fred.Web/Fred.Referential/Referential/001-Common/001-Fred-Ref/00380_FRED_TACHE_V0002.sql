DECLARE @Libelle NVARCHAR(250),
		@Code VARCHAR(6),
		@CiId INT,
		@TacheId INT,
		@Compteur INT = 3,
		@FamilleOperationDiverseId INT,
		@EcartCodeLevel1 VARCHAR(2) = '99', @EcartCodeLevel2 VARCHAR(4) = '9999',
		@DefaultCodeLevel1 VARCHAR(2) = '00', @DefaultCodeLevel2 VARCHAR(4) = '0000', @DefaultCodeLevel3 VARCHAR(6) = '000000';


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
		SET @Compteur = 3;
		SELECT @CiId = CiId FROM @tmpCiId

		-- Création de la tâche par defaut de niveau 1 si elle n'existe pas.
		IF NOT EXISTS(SELECT TacheId FROM FRED_TACHE WHERE CiId = @CiId AND Niveau = 1 AND Code = @DefaultCodeLevel1)
		BEGIN
			INSERT INTO FRED_TACHE([Code],[Libelle],[TacheParDefaut],[Niveau],[Active],[DateCreation],[CiId],[ParentId],[TacheType])
			VALUES(@DefaultCodeLevel1,'TACHE PAR DEFAUT',0,1,1,getDate(),@CiId,NULL,0);
		END

		-- Création de la tâche par defaut de niveau 2 si elle n'existe pas.
		IF NOT EXISTS(SELECT TacheId FROM FRED_TACHE WHERE CiId = @CiId AND Niveau = 2 AND Code = @DefaultCodeLevel2)
		BEGIN
			SELECT @TacheId = TacheId FROM FRED_TACHE WHERE CiId = @CiId AND Niveau = 1 AND Code = @DefaultCodeLevel1;
			IF @TacheId IS NOT NULL
			BEGIN
				INSERT INTO FRED_TACHE([Code],[Libelle],[TacheParDefaut],[Niveau],[Active],[DateCreation],[CiId],[ParentId],[TacheType])
				VALUES(@DefaultCodeLevel2,'TACHE PAR DEFAUT',0,2,1,getDate(),@CiId,@TacheId,0);
			END
		END

		-- Création de la tâche par defaut de niveau 3 si elle n'existe pas.
		IF NOT EXISTS(SELECT TacheId FROM FRED_TACHE WHERE CiId = @CiId AND Niveau = 3 AND Code = @DefaultCodeLevel3)
		BEGIN	
			SELECT @TacheId = TacheId FROM FRED_TACHE WHERE CiId = @CiId AND Niveau = 2 AND Code = @DefaultCodeLevel2;
			INSERT INTO FRED_TACHE([Code],[Libelle],[TacheParDefaut],[Niveau],[Active],[DateCreation],[CiId],[ParentId],[TacheType])
					VALUES(@DefaultCodeLevel3,'TACHE PAR DEFAUT',0,3,1,getDate(),@CiId,@TacheId,0);
		END

		-- Création de la tâche avec écart de niveau 1 si elle n'existe pas.
		IF NOT EXISTS(SELECT TacheId FROM FRED_TACHE WHERE CiId = @CiId AND Niveau = 1 AND Code = @EcartCodeLevel1)
		BEGIN
			INSERT INTO FRED_TACHE([Code],[Libelle],[TacheParDefaut],[Niveau],[Active],[DateCreation],[CiId],[ParentId],[TacheType])
			VALUES(@EcartCodeLevel1,'ECART',0,1,1,getDate(),@CiId,NULL,1);
		END

		-- Création de la tâche avec écart de niveau 2  si elle n'existe pas.
		IF NOT EXISTS(SELECT TacheId FROM FRED_TACHE WHERE CiId = @CiId AND Niveau = 2 AND Code = @EcartCodeLevel2)
		BEGIN	
			SELECT @TacheId = TacheId FROM FRED_TACHE WHERE CiId = @CiId AND Niveau = 1 AND Code = @EcartCodeLevel1;
			IF @TacheId IS NOT NULL
			BEGIN
				INSERT INTO FRED_TACHE([Code],[Libelle],[TacheParDefaut],[Niveau],[Active],[DateCreation],[CiId],[ParentId],[TacheType])
				VALUES(@EcartCodeLevel2,'ECART',0,2,1,getDate(),@CiId,@TacheId,2);
			END
		END

		-- Création des tâches avec écart de niveau 3  si elles n'existent pas.
		
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
					WHEN UPPER(fod.Libelle) IN ('DÉBOURSÉ ACHATS RAPPROCHE (y compris Intérim)','DEBOURSE ACHATS RAPPROCHE (y compris Interim)','DÉBOURSÉ ACHATS AVEC COMMANDE (Y COMPRIS INTERIM)','ACHAT AVEC COMMANDE (Y compris interim)','ACHAT AVEC COMMANDE (Y compris intérim)')
						THEN '999994'
					WHEN UPPER(fod.Libelle) IN ('DÉBOURSÉ AUTRES DÉPENSES' ,'AUTRE DEPENSES SANS COMMANDE')
						THEN '999995'
					WHEN UPPER(fod.Libelle) IN ('DÉBOURSÉ AMORTISSEMENT','DÉBOURSÉ AMORTISSEMENT','DEBOURSE AMMORTISSEMENT','MATERIEL IMMOBILISE')
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
				SELECT @FamilleOperationDiverseId = FamilleOperationDiverseId, @Libelle = Libelle, @Code = Code FROM @tmpFOD;
					
				SELECT @TacheId = TacheId FROM FRED_TACHE WHERE CiId = @CiId AND Niveau = 2 AND Code = @EcartCodeLevel2;
					
				IF @Code IS NOT NULL AND NOT EXISTS(SELECT TOP(1) 1 FROM FRED_TACHE WHERE CiId = @CiId AND Niveau = 3 AND Code = @Code)
					BEGIN
						INSERT INTO FRED_TACHE([Code],[Libelle],[TacheParDefaut],[Niveau],[Active],[DateCreation],[CiId],[ParentId],[TacheType])
						VALUES(@Code,@Libelle,0,3,1,getDate(),@CiId,@TacheId,@Compteur);
						SET @Compteur = @Compteur + 1;
					END

				--Insertion de la tache 999996  ECART INTERIM  si elle n'existe pas.
				IF NOT EXISTS(SELECT TOP(1) 1 FROM FRED_TACHE WHERE CiId = @CiId AND Niveau = 3 AND Code = '999996')
					BEGIN
						INSERT INTO FRED_TACHE([Code],[Libelle],[TacheParDefaut],[Niveau],[Active],[DateCreation],[CiId],[ParentId],[TacheType])
						VALUES('999996','ECART INTERIM',0,3,1,getDate(),@CiId,@TacheId,8);
					END

				IF NOT EXISTS(SELECT TOP(1) 1 FROM FRED_TACHE WHERE CiId = @CiId AND Niveau = 3 AND Code = '999999')
					BEGIN
						INSERT INTO FRED_TACHE([Code],[Libelle],[TacheParDefaut],[Niveau],[Active],[DateCreation],[CiId],[ParentId],[TacheType])
						VALUES('999999','ECART RECETTES',0,3,1,getDate(),@CiId,@TacheId,10);
					END

					-- Récuperation de l'id de la tâche niveau 3 pour maj la tache par défaut d'une OD
					SELECT TOP(1) @TacheId = TacheId FROM FRED_TACHE WHERE CiId = @CiId AND Niveau = 3 AND UPPER(Libelle) = @Libelle

					-- Mise à jour de la la tâche par defaut de l'OD.
					IF(@TacheId IS NOT NULL)
					BEGIN
						UPDATE FRED_OPERATION_DIVERSE
						SET TacheId = @TacheId
						WHERE CiId = @CiId
						AND FamilleOperationDiverseId = @FamilleOperationDiverseId
						AND OdEcart = 1 ;
					END

				DELETE FROM @tmpFOD  WHERE FamilleOperationDiverseId = @FamilleOperationDiverseId;
			END
		DELETE FROM @tmpCiId  WHERE CiId = @CiId;
	END