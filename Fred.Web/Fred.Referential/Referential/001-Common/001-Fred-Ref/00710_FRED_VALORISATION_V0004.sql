﻿

DELETE FROM FRED_VALORISATION where Source LIKE 'Script%';

--TRAITEMENT PERSONNEL INTERIMAIRE

DECLARE @CiId int
	,@RapportId int
	,@RapportLigneId int
	,@TacheId int
	,@ChapitreId int
	,@SousChapitreId int
	,@ReferentielEtenduId int
	,@SurchargeReferentielEtenduId int
	,@BaremeId int
	,@BaremeStormId int
	,@UniteId int
	,@DeviseId int
	,@PersonnelId int
	,@MaterielId int
	,@Date datetime = GETDATE()
	,@VerrouPeriode bit = 0
	,@DateCreation datetime = GETDATE()
	,@Source nvarchar(max) = 'Script Personnel Interimaire'
	,@PUHT decimal(18,3)
	,@Quantite decimal(18,3)
	,@Montant decimal(18,3)
	,@TauxHoraireConverti decimal(18,2)
	,@DatePointage datetime
	,@SocieteId int
	,@RessourceId int
	,@Prix decimal(18,2)
	,@PrixConduite decimal(18,2)

--Récupération des rapports sans valo à redresser (cas interimaire)
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
	INSERT INTO #RapportLignes([ReferentielEtenduId],[RessourceId] ,[RapportLigneId],[RapportId],[CiId],[PersonnelId],[MaterielId],[DatePointage],[SousChapitreId])
	SELECT srn.ReferentielEtenduId,p.RessourceId,rl.RapportLigneId,rl.RapportId,rl.CiId,rl.PersonnelId,rl.MaterielId, rl.DatePointage,res.SousChapitreId 
	FROM dbo.FRED_RAPPORT_LIGNE  rl WITH(NOLOCK)
	INNER JOIN FRED_PERSONNEL p WITH(NOLOCK) ON rl.PersonnelId = p.PersonnelId
	INNER JOIN FRED_SOCIETE_RESSOURCE_NATURE srn WITH(NOLOCK) ON srn.RessourceId = p.RessourceId AND srn.SocieteId = p.SocieteId
	INNER JOIN FRED_RESSOURCE res WITH(NOLOCK) ON res.RessourceId = srn.RessourceId
	WHERE rl.PersonnelId IS NOT NULL
	AND p.IsInterimaire = 1
	AND rl.CiId IN (SELECT ci.CiId FROM FRED_SOCIETE s WITH(NOLOCK)
					INNER JOIN FRED_CI ci WITH(NOLOCK) ON s.SocieteId = ci.SocieteId
					WHERE s.Code IN ('RB','MBTP'))
	AND rl.RapportLigneId NOT IN (SELECT RapportLigneId FROM FRED_VALORISATION WITH(NOLOCK) WHERE Quantite > 0)
	AND rl.CodeAbsenceId IS NULL
	AND rl.DATESuppression IS NULL
	AND (rl.HeureNormale > 0 OR rl.HeureMajoration > 0);

	CREATE TABLE #RAPPORT_LIGNE_TACHE(
		[TacheId] [int],
		[HeureTache] [int]);

	WHILE EXISTS(SELECT TOP(1) 1 FROM #RapportLignes)
	BEGIN
		SET @CiId = NULL;
		SET @RapportId  = NULL;
		SET @RapportLigneId  = NULL;
		SET @TacheId  = NULL;
		SET @ChapitreId  = NULL;
		SET @SousChapitreId  = NULL;
		SET @ReferentielEtenduId = 0;
		SET @SurchargeReferentielEtenduId = NULL;
		SET @BaremeId  = NULL;
		SET @BaremeStormId  = NULL;
		SET @UniteId  = NULL;
		SET @DeviseId  = NULL;
		SET @PersonnelId  = NULL;
		SET @MaterielId  = NULL;
		SET @PUHT= NULL;
		SET @Quantite= NULL;
		SET @Montant= NULL;
		SET @DatePointage= NULL;
		SET @SocieteId= NULL;
		SET @RessourceId= NULL;

		-- Recupération du rapport à traiter
		SELECT TOP 1 @RapportLigneId = RapportLigneId, @RapportId = RapportId, @PersonnelId = PersonnelId, @MaterielId = MaterielId, @CiId = CiId, @ReferentielEtenduId = ReferentielEtenduId, @DatePointage = DatePointage, @SousChapitreId = SousChapitreId FROM #RapportLignes

		--Recuperer le ChapitreId 
		SELECT TOP 1 @ChapitreId = ChapitreId FROM FRED_SOUS_CHAPITRE WITH(NOLOCK) WHERE SousChapitreId = @SousChapitreId;

		--Recuperer le TacheId
		INSERT INTO #RAPPORT_LIGNE_TACHE(TacheId, HeureTache)
		SELECT TacheId, HeureTache FROM FRED_RAPPORT_LIGNE_TACHE WITH(NOLOCK) WHERE RapportLigneId = @RapportLigneId AND HeureTache > 0;

		--Début traitement pour chaque tache, il faut créer une valo
		WHILE EXISTS(SELECT TOP(1) 1 FROM #RAPPORT_LIGNE_TACHE)
		BEGIN
		SELECT TOP 1 @TacheId = TacheId, @Quantite = HeureTache FROM #RAPPORT_LIGNE_TACHE;

		SELECT TOP 1 @DeviseId = DeviseId from FRED_DEVISE WHERE IsoCode = 'EUR';

		--Recupération du contrat interim
		IF(EXISTS(SELECT TOP 1 1 FROM FRED_CONTRAT_INTERIMAIRE interim WITH(NOLOCK) INNER JOIN FRED_RAPPORT_LIGNE p WITH(NOLOCK) 
			ON p.PersonnelId = interim.InterimaireId
			WHERE p.PersonnelId = @PersonnelId  
			AND interim.DateDebut <= p.DatePointage 
			AND (p.DatePointage <= interim.DateFin OR interim.DateFin IS NULL)))
		BEGIN
		SELECT TOP 1 @RessourceId = RessourceId, @SocieteId = SocieteId, @PUHT = Valorisation, @UniteId = UniteId
		FROM FRED_CONTRAT_INTERIMAIRE interim WITH(NOLOCK)
		INNER JOIN FRED_RAPPORT_LIGNE p WITH(NOLOCK) ON p.PersonnelId = interim.InterimaireId
		WHERE p.PersonnelId = @PersonnelId  
		AND interim.DateDebut <= p.DatePointage 
		AND (p.DatePointage <= interim.DateFin OR interim.DateFin IS NULL);

		--Traitement du cas IsSep
		IF(EXISTS (SELECT TOP 1 1  FROM FRED_ASSOCIE_SEP WITH(NOLOCK) WHERE SocieteId = @SocieteId))
		BEGIN 
			SELECT @SocieteId = SocieteAssocieeId FROM FRED_ASSOCIE_SEP WITH(NOLOCK) WHERE SocieteId = @SocieteId AND TypeParticipationSepId = 1;
			
			SELECT TOP 1 @ReferentielEtenduId = ReferentielEtenduId FROM FRED_SOCIETE_RESSOURCE_NATURE WITH(NOLOCK)
			WHERE SocieteId = @SocieteId AND RessourceId = @RessourceId;
		END

		IF(@MaterielId IS NOT NULL)
		BEGIN
			IF(EXISTS(SELECT TOP 1 1 FROM FRED_BAREME_EXPLOITATION_CI_SURCHARGE WITH(NOLOCK)
			WHERE PersonnelId = @PersonnelId AND CiId = @CiId AND PeriodeDebut <= @DatePointage AND PeriodeFin IS NULL))
			BEGIN
				SELECT TOP 1   @Prix = Prix, @PrixConduite = PrixConduite,@SurchargeReferentielEtenduId = ReferentielEtenduId
				FROM FRED_BAREME_EXPLOITATION_CI_SURCHARGE WITH(NOLOCK)
				WHERE MaterielId = @MaterielId 
				AND CiId = @CiId
				AND PeriodeDebut <= @DatePointage
				AND PeriodeFin IS NULL;
			END
			ELSE
			BEGIN
				IF(EXISTS(SELECT TOP 1 1 FROM FRED_BAREME_EXPLOITATION_CI WITH(NOLOCK)
					WHERE ReferentielEtenduId = @ReferentielEtenduId 
					AND CiId = @CiId
					AND PeriodeDebut <= @DatePointage
					AND (@DatePointage < PeriodeFin OR PeriodeFin IS NULL)))
				BEGIN
					SELECT TOP 1 @BaremeId = BaremeId, @Prix = Prix, @PrixConduite = PrixConduite 
					FROM FRED_BAREME_EXPLOITATION_CI WITH(NOLOCK)
					WHERE ReferentielEtenduId = @ReferentielEtenduId 
					AND CiId = @CiId
					AND PeriodeDebut <= @DatePointage
					AND (@DatePointage < PeriodeFin OR PeriodeFin IS NULL);
				END
			END
		END

		IF(@PrixConduite > 0)
		BEGIN
			SET @PUHT = @PrixConduite;
		END

		SET @Montant = ROUND(@PUHT * @Quantite, 2);

		IF(@Montant > 0 AND @UniteId IS NOT NULL)
		BEGIN
		--Insertion de la valo
			INSERT INTO [dbo].[FRED_VALORISATION]([CiId],[RapportId],[RapportLigneId],[TacheId],[ChapitreId],[SousChapitreId],[ReferentielEtenduId],[BaremeId],[BaremeStormId],
				[UniteId],[DeviseId],[PersonnelId],[Date],[VerrouPeriode],[DateCreation],[Source],[PUHT],[Quantite],[Montant])
			VALUES(@CiId,@RapportId,@RapportLigneId,@TacheId,@ChapitreId,@SousChapitreId,@ReferentielEtenduId,@BaremeId,@BaremeStormId,
				@UniteId,@DeviseId,@PersonnelId,@Date,@VerrouPeriode,@DateCreation,@Source,@PUHT,@Quantite,@Montant)
		END

		DELETE FROM #RAPPORT_LIGNE_TACHE WHERE TacheId = @TacheId;
		END
 END

		TRUNCATE TABLE #RAPPORT_LIGNE_TACHE
		DELETE FROM #RapportLignes WHERE  RapportLigneId = @RapportLigneId;
	END
DROP TABLE #RAPPORT_LIGNE_TACHE
DROP TABLE #RapportLignes