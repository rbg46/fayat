
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
	,@Source nvarchar(max) = 'Redressement des données ticket #13816'
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
	
DECLARE @DefaultDeviseId INT = (SELECT TOP 1 DeviseId from FRED_DEVISE WHERE IsoCode = 'EUR');
DECLARE @DefaultUniteId INT = (SELECT TOP 1 UniteId from FRED_UNITE WHERE Code = 'H');
DECLARE @DefaultPUHT decimal(18,2) = 0.01;
	
	CREATE TABLE #RapportLignes(
		[ReferentielEtenduId] [int],
		[RessourceId] [int],
		[RapportLigneId] [int],
		[RapportId] [int],
		[CiId] [int],
		[PersonnelId] [int],
		[MaterielId] [int],
		[DatePointage] datetime,
		[SousChapitreId] [int],
		[TacheId] [INT], 
		[HeureTache] decimal(18,3),
		[VerrouPeriode] BIT);

--Récupération des rapports sans valo à redresser (cas materiel)

	INSERT INTO #RapportLignes([ReferentielEtenduId],[RessourceId],[RapportLigneId],[RapportId],[CiId],[PersonnelId],[MaterielId],[DatePointage],[SousChapitreId],[TacheId], [HeureTache],[VerrouPeriode])
	SELECT srn.ReferentielEtenduId,m.RessourceId,rl.RapportLigneId,rl.RapportId,rl.CiId,rl.PersonnelId,rl.MaterielId, rl.DatePointage,res.SousChapitreId,
	rlt.TacheId, rlt.HeureTache,CASE WHEN dcc.DateCloture IS NOT NULL THEN 1 ELSE 0 END as VerrouPeriode
	FROM dbo.FRED_RAPPORT_LIGNE  rl
		INNER JOIN dbo.FRED_RAPPORT r ON r.RapportId = rl.RapportId
		INNER JOIN dbo.FRED_RAPPORT_LIGNE_TACHE rlt  ON rlt.RapportLigneId = rl.RapportLigneId
		INNER JOIN dbo.FRED_CI c ON c.CiId = r.CiId
		INNER JOIN dbo.FRED_SOCIETE s ON s.SocieteId = c.SocieteId
		INNER JOIN FRED_DATES_CLOTURE_COMPTABLE dcc ON dcc.CiId = rl.CiId
			AND dcc.Annee = YEAR(rl.DatePointage)
			AND dcc.Mois = MONTH(rl.DatePointage)
		LEFT JOIN dbo.FRED_MATERIEL m ON m.MaterielId = rl.MaterielId
		INNER JOIN FRED_SOCIETE_RESSOURCE_NATURE srn WITH(NOLOCK) ON srn.RessourceId = m.ressourceid
		INNER JOIN FRED_RESSOURCE res WITH(NOLOCK) ON res.RessourceId = srn.RessourceId
		LEFT OUTER JOIN dbo.FRED_VALORISATION v ON v.RapportId = r.RapportId AND v.RapportLigneId = rl.RapportLigneid AND rl.materielid = v.materielid AND v.montant > 0
	WHERE 1 = 1
		AND v.ValorisationId IS NULL 
		AND (r.DateSuppression IS NULL AND rl.DateSuppression  IS NULL)
		AND s.Code in('0001','0143')
		AND rlt.HeureTache> 0
		AND rl.MaterielId IS NOT NULL
		AND rl.MaterielId <> 0
		AND rl.PrenomNomTemporaire IS NULL 
		AND rl.MaterielNomTemporaire IS NULL
		AND NOT (rl.HeureNormale = 0 AND rl.HeureMajoration = 0);
	

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
		SET @TacheId = NULL;
		SET @Quantite = NULL;
		SET @VerrouPeriode = 0;

		-- Recupération du rapport à traiter
		SELECT TOP 1 @RapportLigneId = RapportLigneId, @RapportId = RapportId, @PersonnelId = PersonnelId, @MaterielId = MaterielId, @CiId = CiId, @ReferentielEtenduId = ReferentielEtenduId, @DatePointage = DatePointage, @SousChapitreId = SousChapitreId,@TacheId = TacheId, @Quantite = HeureTache,@VerrouPeriode = VerrouPeriode
		FROM #RapportLignes

		--Recuperer le ChapitreId 
		SELECT TOP 1 @ChapitreId = ChapitreId FROM FRED_SOUS_CHAPITRE WITH(NOLOCK) WHERE SousChapitreId = @SousChapitreId;

		--Recupération du materiel
		SELECT TOP 1 @SocieteId = SocieteId, @MaterielLocation = MaterielLocation, @IsStorm = IsStorm, @RessourceId = RessourceId FROM FRED_MATERIEL WITH(NOLOCK) 
		WHERE MaterielId = @MaterielId;

		--Traitement du cas IsSep
		IF(EXISTS (SELECT TOP 1 1  FROM FRED_ASSOCIE_SEP WITH(NOLOCK) WHERE SocieteId = @SocieteId))
		BEGIN 
			SELECT TOP 1 @SocieteId = SocieteAssocieeId FROM FRED_ASSOCIE_SEP WITH(NOLOCK) WHERE SocieteId = @SocieteId AND TypeParticipationSepId = 1;
		
			SELECT TOP 1 @ReferentielEtenduId = ReferentielEtenduId FROM FRED_SOCIETE_RESSOURCE_NATURE WITH(NOLOCK)	WHERE SocieteId = @SocieteId AND RessourceId = @RessourceId;
		END
		ELSE IF(EXISTS (SELECT TOP 1 1  FROM FRED_MATERIEL WITH(NOLOCK) WHERE MaterielId = @MaterielId))
		BEGIN 
			SELECT TOP 1 @SocieteId = SocieteId FROM FRED_MATERIEL WITH(NOLOCK) WHERE MaterielId = @MaterielId;
		
			SELECT TOP 1 @ReferentielEtenduId = ReferentielEtenduId FROM FRED_SOCIETE_RESSOURCE_NATURE WITH(NOLOCK)	WHERE SocieteId = @SocieteId AND RessourceId = @RessourceId;
		END
		ELSE IF(@PersonnelId IS NOT NULL AND EXISTS (SELECT TOP 1 1  FROM FRED_PERSONNEL WITH(NOLOCK) WHERE PersonnelId = @PersonnelId))
		BEGIN 
			SELECT TOP 1 @SocieteId = SocieteId FROM FRED_PERSONNEL WITH(NOLOCK) WHERE PersonnelId = @PersonnelId;
		
			SELECT TOP 1 @ReferentielEtenduId = ReferentielEtenduId FROM FRED_SOCIETE_RESSOURCE_NATURE WITH(NOLOCK)	WHERE SocieteId = @SocieteId AND RessourceId = @RessourceId ;
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
				
				--Création d'un bareme si existe pas
				IF(@BaremeStormId IS NULL)
				BEGIN
				PRINT 'New Bareme storm'
					INSERT INTO [dbo].[FRED_BAREME_EXPLOITATION_ORGANISATION]
					   ([OrganisationId]
					   ,[RessourceId]
					   ,[UniteId]
					   ,[DeviseId]
					   ,[Statut]
					   ,[PeriodeDebut]
					   ,[Prix]
					   ,[DateCreation])
					VALUES
					   (@OrganisationId
					   ,@RessourceId
					   ,@DefaultUniteId
					   ,@DefaultDeviseId
					   ,1
					   ,getdate()
					   ,@DefaultPUHT
					   ,getdate())
					   
					   SET @BaremeStormId = SCOPE_IDENTITY();
					   SET @DeviseId = @DefaultDeviseId;
					   SET @PUHT = @DefaultPUHT;
					   SET @UniteId = @DefaultUniteId
			   END
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
					   SET @DeviseId = @DefaultDeviseId;
					IF(@UniteId IS NULL)
						SET @UniteId = @DefaultUniteId;
					IF(@PUHT IS NULL)
						SET @PUHT = @DefaultPUHT;
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
		END

		DELETE FROM #RapportLignes WHERE  RapportLigneId = @RapportLigneId;
	END

DROP TABLE #RapportLignes