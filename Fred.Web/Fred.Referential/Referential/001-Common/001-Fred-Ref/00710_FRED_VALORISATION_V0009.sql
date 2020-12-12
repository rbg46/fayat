
--TRAITEMENT MATERIEL

DECLARE @CiId int
	,@RapportId int
	,@RapportLigneId int
	,@TacheId int
	,@ChapitreId int
	,@SousChapitreId int
	,@ReferentielEtenduId int
	,@BaremeId int
	,@BaremeStormId int
	,@UniteId int
	,@DeviseId int
	,@PersonnelId int
	,@MaterielId int
	,@VerrouPeriode bit = 0
	,@DateCreation datetime = GETDATE()
	,@Source nvarchar(max) = 'Script Materiel'
	,@PUHT decimal(18,3)
	,@Quantite decimal(18,3)
	,@Montant decimal(18,3)
	,@TauxHoraireConverti decimal(18,2)
	,@MaterielLocation bit
	,@IsStorm bit
	,@DatePointage datetime
	,@SocieteId int
	,@RessourceId int
	,@Valorisation decimal(18,2)
	,@Prix decimal(18,2)
	,@PrixConduite decimal(18,2)
	,@OrganisationId int
	,@UniteCode nvarchar(3)

--Récupération des rapports sans valo à redresser (cas materiel)
	CREATE TABLE #RapportLignes(
		[ReferentielEtenduId] [int],
		[RessourceId] [int],
		[RapportLigneId] [int],
		[RapportId] [int],
		[CiId] [int],
		[PersonnelId] [int],
		[MaterielId] [int],
		[DatePointage] datetime,
		[SousChapitreId] [int]);
	INSERT INTO #RapportLignes([ReferentielEtenduId],[RessourceId],[RapportLigneId],[RapportId],[CiId],[PersonnelId],[MaterielId],[DatePointage],[SousChapitreId])
	SELECT srn.ReferentielEtenduId,p.RessourceId,rl.RapportLigneId,rl.RapportId,rl.CiId,rl.PersonnelId,rl.MaterielId, rl.DatePointage,res.SousChapitreId 
	FROM dbo.FRED_RAPPORT_LIGNE  rl WITH(NOLOCK)
	INNER JOIN FRED_PERSONNEL p WITH(NOLOCK) ON rl.PersonnelId = p.PersonnelId
	INNER JOIN FRED_SOCIETE_RESSOURCE_NATURE srn WITH(NOLOCK) ON srn.RessourceId = p.RessourceId AND srn.SocieteId = p.SocieteId
	INNER JOIN FRED_RESSOURCE res WITH(NOLOCK) ON res.RessourceId = srn.RessourceId
	WHERE rl.MaterielId IS NOT NULL
	AND rl.CiId IN (SELECT ci.CiId FROM FRED_SOCIETE s WITH(NOLOCK)
					INNER JOIN FRED_CI ci WITH(NOLOCK) ON s.SocieteId = ci.SocieteId
					WHERE s.Code IN ('RB','MBTP'))
	AND (rl.RapportLigneId NOT IN (SELECT RapportLigneId FROM FRED_VALORISATION v WITH(NOLOCK) WHERE Quantite > 0 AND v.MaterielId=rl.MaterielId) OR
		 rl.RapportLigneId IN (SELECT RapportLigneId FROM FRED_VALORISATION WITH(NOLOCK) WHERE Source LIKE 'Script%'))
	AND rl.CodeAbsenceId IS NULL
	AND rl.DATESuppression IS NULL
	AND (rl.HeureNormale > 0 OR rl.HeureMajoration > 0);

	CREATE TABLE #RAPPORT_LIGNE_TACHE(
		[TacheId] [int],
		[HeureTache] [float]);

	WHILE EXISTS(SELECT TOP(1) 1 FROM #RapportLignes)
	BEGIN
		SET @CiId = NULL;
		SET @RapportId  = NULL;
		SET @RapportLigneId  = NULL;
		SET @ChapitreId  = NULL;
		SET @SousChapitreId  = NULL;
		SET @ReferentielEtenduId  = NULL;
		SET @BaremeId  = NULL;
		SET @BaremeStormId  = NULL;
		SET @UniteId  = NULL;
		SET @DeviseId  = NULL;
		SET @PersonnelId  = NULL;
		SET @MaterielId  = NULL;
		SET @PUHT= NULL;
		SET @Montant= NULL;
		SET @DatePointage= NULL;
		SET @SocieteId= NULL;
		SET @RessourceId= NULL;		
		SET @MaterielLocation= NULL;
		SET @IsStorm = NULL;
		SET @OrganisationId = NULL;
		SET @Quantite= NULL;
		SET @TacheId  = NULL;
		SET @UniteCode  = NULL;

		-- Recupération du rapport à traiter
		SELECT TOP 1 @RapportLigneId = RapportLigneId, @RapportId = RapportId, @PersonnelId = PersonnelId, @MaterielId = MaterielId, @CiId = CiId, @ReferentielEtenduId = ReferentielEtenduId, @DatePointage = DatePointage, @SousChapitreId = SousChapitreId
		FROM #RapportLignes

		--Recuperer le ChapitreId 
		SELECT TOP 1 @ChapitreId = ChapitreId FROM FRED_SOUS_CHAPITRE WITH(NOLOCK) WHERE SousChapitreId = @SousChapitreId;

		--Recuperer le TacheId
		INSERT INTO #RAPPORT_LIGNE_TACHE(TacheId, HeureTache)
		SELECT TacheId, HeureTache FROM FRED_RAPPORT_LIGNE_TACHE WITH(NOLOCK) WHERE RapportLigneId = @RapportLigneId;

		--Pour chaque tache, il faut créer une valo
		WHILE EXISTS(SELECT TOP(1) 1 FROM #RAPPORT_LIGNE_TACHE)
		BEGIN
			SELECT TOP 1 @TacheId = TacheId, @Quantite = HeureTache FROM #RAPPORT_LIGNE_TACHE;

			--Recupération du materiel
			SELECT TOP 1 @SocieteId = SocieteId, @MaterielLocation = MaterielLocation, @IsStorm = IsStorm, @RessourceId = RessourceId FROM FRED_MATERIEL WITH(NOLOCK) 
			WHERE MaterielId = @MaterielId;

			--Traitement du cas IsSep
			IF(EXISTS (SELECT TOP 1 1  FROM FRED_ASSOCIE_SEP WITH(NOLOCK) WHERE SocieteId = @SocieteId))
			BEGIN 
				SELECT TOP 1 @SocieteId = SocieteAssocieeId FROM FRED_ASSOCIE_SEP WITH(NOLOCK) WHERE SocieteId = @SocieteId AND TypeParticipationSepId = 1;
			
				SELECT TOP 1 @ReferentielEtenduId = ReferentielEtenduId FROM FRED_SOCIETE_RESSOURCE_NATURE WITH(NOLOCK)
				WHERE SocieteId = @SocieteId AND RessourceId = @RessourceId;
			END
			ELSE IF(EXISTS (SELECT TOP 1 1  FROM FRED_MATERIEL WITH(NOLOCK) WHERE MaterielId = @MaterielId))
			BEGIN 
				SELECT TOP 1 @SocieteId = SocieteId FROM FRED_MATERIEL WITH(NOLOCK) WHERE MaterielId = @MaterielId;
			
				SELECT TOP 1 @ReferentielEtenduId = ReferentielEtenduId FROM FRED_SOCIETE_RESSOURCE_NATURE WITH(NOLOCK)
				WHERE SocieteId = @SocieteId AND RessourceId = @RessourceId;
			END
			ELSE IF(@PersonnelId IS NOT NULL AND EXISTS (SELECT TOP 1 1  FROM FRED_PERSONNEL WITH(NOLOCK) WHERE PersonnelId = @PersonnelId))
			BEGIN 
				SELECT TOP 1 @SocieteId = SocieteId FROM FRED_PERSONNEL WITH(NOLOCK) WHERE PersonnelId = @PersonnelId;
			
				SELECT TOP 1 @ReferentielEtenduId = ReferentielEtenduId FROM FRED_SOCIETE_RESSOURCE_NATURE WITH(NOLOCK)
				WHERE SocieteId = @SocieteId AND RessourceId = @RessourceId ;
			END

			IF(@MaterielLocation = 1) -- Cas du materiel externe
			BEGIN
				SELECT TOP 1 @PUHT = PUHT, @UniteId = UniteId FROM FRED_COMMANDE_LIGNE WITH(NOLOCK) WHERE MaterielId = @MaterielId;
				SELECT TOP 1 @DeviseId = DeviseId from FRED_DEVISE WHERE IsoCode = 'EUR';
				SELECT TOP 1 @UniteCode = Code  FROM FRED_UNITE WITH(NOLOCK) WHERE @UniteId = UniteId;
				IF(@UniteCode = 'JR')
				SET @PUHT = ROUND(@PUHT / 8, 2);
				IF(@UniteCode = 'SM')
				SET @PUHT = ROUND(@PUHT / 40, 2);
			END
			ELSE -- Cas du materiel interne
			BEGIN
				SELECT TOP 1 @OrganisationId = ec.OrganisationId FROM FRED_ETABLISSEMENT_COMPTABLE  ec WITH(NOLOCK)
				INNER JOIN FRED_CI ci WITH(NOLOCK) ON ci.OrganisationId = ec.OrganisationId
				WHERE ec.SocieteId = @SocieteId
				
				IF(@OrganisationId IS NULL)
				BEGIN
					SELECT TOP 1 @OrganisationId = OrganisationId FROM FRED_CI WITH(NOLOCK)
					WHERE SocieteId = @SocieteId
				END
				-- Recuperation du bareme storm
				IF(@IsStorm <> 0)
				BEGIN
					SELECT TOP 1 @BaremeStormId = BaremeId, @DeviseId = DeviseId, @PUHT = Prix, @PrixConduite = PrixConduite, @UniteId = UniteId 
					FROM FRED_BAREME_EXPLOITATION_ORGANISATION WITH(NOLOCK)
					WHERE OrganisationId = @OrganisationId 
					AND RessourceId = @RessourceId
					AND (@DatePointage < PeriodeFin OR PeriodeFin IS NULL);
				END
				ELSE
				BEGIN
					SELECT TOP 1 @BaremeId = BaremeId, @DeviseId = DeviseId, @PUHT = Prix, @PrixConduite = PrixConduite, @UniteId = UniteId 
					FROM FRED_BAREME_EXPLOITATION_CI WITH(NOLOCK)
					WHERE ReferentielEtenduId = @ReferentielEtenduId
					AND (@DatePointage < PeriodeFin OR PeriodeFin IS NULL);

					IF(@BaremeId IS NULL)
					BEGIN
						SELECT TOP 1  @DeviseId = DeviseId, @PUHT = Prix, @PrixConduite = PrixConduite, @UniteId = UniteId
						FROM FRED_BAREME_EXPLOITATION_CI_SURCHARGE WITH(NOLOCK)
						WHERE CiId = @CiId 
						AND MaterielId = @MaterielId
						AND (@DatePointage < PeriodeFin OR PeriodeFin IS NULL);

						IF(@DeviseId IS NULL)
						SELECT TOP 1 @DeviseId = DeviseId from FRED_DEVISE WHERE IsoCode = 'EUR';
						IF(@UniteId IS NULL)
						SELECT TOP 1 @UniteId = UniteId from FRED_UNITE WHERE Code = 'H';
						IF(@PUHT IS NULL)
						SET @PUHT = 0.01;
					END
				END
			END

			SET @Montant = ROUND(@PUHT * @Quantite, 2);

			IF(@Montant > 0 AND @UniteId IS NOT NULL)
			BEGIN
				--Insertion de la valo
				INSERT INTO [dbo].[FRED_VALORISATION]([CiId],[RapportId],[RapportLigneId],[TacheId],[ChapitreId],[SousChapitreId],[ReferentielEtenduId],[BaremeId],[BaremeStormId],
						[UniteId],[DeviseId],[MaterielId],[Date],[VerrouPeriode],[DateCreation],[Source],[PUHT],[Quantite],[Montant])
				VALUES(@CiId,@RapportId,@RapportLigneId,@TacheId,@ChapitreId,@SousChapitreId,@ReferentielEtenduId,@BaremeId,@BaremeStormId,
						@UniteId,@DeviseId,@MaterielId,@DatePointage,@VerrouPeriode,@DateCreation,@Source,@PUHT,@Quantite,@Montant)
			END

			DELETE FROM #RAPPORT_LIGNE_TACHE WHERE TacheId = @TacheId;
		END

		TRUNCATE TABLE #RAPPORT_LIGNE_TACHE
		DELETE FROM #RapportLignes WHERE  RapportLigneId = @RapportLigneId;
	END
DROP TABLE #RAPPORT_LIGNE_TACHE
DROP TABLE #RapportLignes