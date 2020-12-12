
DECLARE @Libelle NVARCHAR(250),
        @OldLibelle NVARCHAR(250),
		@CiId INT,
		@TacheId INT,
		@SocieteId INT,
		@OperationDiverseId INT;	

	IF OBJECT_ID('tempdb..#tmpOD') IS NOT NULL 
		BEGIN
			DROP TABLE #tmpOD
		END

	CREATE TABLE #tmpOD
	(
		CiId INT NOT NULL,
		OperationDiverseId [INT]  NULL,
		Libelle [NVARCHAR](500) NULL,
		SocieteId [INT] NOT NULL
	)

	-- Récupération de tous les CIs
	INSERT INTO #tmpOD(CiId,OperationDiverseId,Libelle,SocieteId)
	SELECT od.CiId,od.OperationDiverseId, UPPER(fod.Libelle),fod.SocieteId
    FROM FRED_FAMILLE_OPERATION_DIVERSE fod
	INNER JOIN FRED_OPERATION_DIVERSE od on fod.FamilleOperationDiverseId = od.FamilleOperationDiverseId
	WHERE fod.Libelle IS NOT NULL AND fod.Libelle <> '' AND od.OdEcart = 1

	-- Pour chaque CIs
	WHILE EXISTS(SELECT TOP(1) 1 FROM #tmpOD)
	BEGIN
			SELECT top 1 @CiId= od.CiId,@OperationDiverseId = od.OperationDiverseId,@Libelle = od.Libelle, @SocieteId = od.SocieteId FROM #tmpOD od

            SET @OldLibelle = CASE 
                                WHEN @Libelle IN ('DÉBOURSÉ ACHATS RAPPROCHE (y compris Intérim)','DEBOURSE ACHATS RAPPROCHE (y compris Interim)','DÉBOURSÉ ACHATS AVEC COMMANDE (Y COMPRIS INTERIM)','ACHAT AVEC COMMANDE (Y compris interim)','ACHAT AVEC COMMANDE (Y compris intérim)')
                                     THEN 'ECART ACHAT'
                                WHEN @Libelle = 'MATERIEL IMMOBILISE' AND @SocieteId = 60
                                    THEN 'ECART MATERIEL EXTERNE'
                                WHEN @Libelle IN ('DÉBOURSÉ AMORTISSEMENT','DEBOURSE AMMORTISSEMENT','MATERIEL IMMOBILISE') AND @SocieteId <> 60
                                    THEN 'ECART MATERIEL IMMOBILISE'                                
                                WHEN @Libelle IN ('DÉBOURSÉ AUTRES DÉPENSES' ,'AUTRE DEPENSES SANS COMMANDE')
                                    THEN 'ECART AUTRE FRAIS'
				                WHEN @Libelle IN ('DÉBOURSÉ MAIN D''OEUVRE (HORS INTÉRIM)' ,'DEBOURSE MAIN D''OEUVRE (HORS INTÉRIM)','DEBOURSE MAIN D''OEUVRE (HORS INTERIM)',UPPER('MO POINTEE (Hors Interim)'),UPPER('MO POINTEE (Hors Intérim)'))
                                    THEN 'ECART MO ENCADREMENT'
				                WHEN @Libelle IN ('DÉBOURSÉ MATÉRIEL INTERNE','DEBOURSE MATERIEL INTERNE','MATERIEL INTERNE POINTE')  
                                    THEN 'ECART MATERIEL'
                                WHEN @Libelle IN ('RECETTES (Hors explo et CB)','RECETTES')  
                                    THEN 'ECART RECETTES'
                                WHEN @Libelle IN ('DÉBOURSÉ ACHATS AVEC COMMANDE (Y COMPRIS INTERIM)','DEBOURSE ACHATS AVEC COMMANDE (Y COMPRIS INTERIM)')
                                    THEN UPPER('ECART ACHAT AVEC COMMANDE (y compris Intérim)')
				                ELSE 'ECART ' + UPPER(@Libelle) 
                            END

					
			-- Récuperation de l'id de la tâche correspondante de niveau 3 pour maj la tache par défaut d'une OD
			SELECT TOP 1 @TacheId = TacheId FROM FRED_TACHE WHERE CiId = @CiId AND Niveau = 3 AND UPPER(Libelle) = @OldLibelle;
					
			--PRINT 'UPDATE de : ciid = ' + CAST(@CiId as varchar(10)) + ' and libelle = ' + @Libelle + '  and  OperationDiverseId = ' + CAST(@OperationDiverseId as varchar(10)) + ' and FamilleOperationDiverseId = ' + CAST(@FamilleOperationDiverseId as varchar(10));
					
			-- Mise à jour de la la tâche par defaut de l'OD.
			IF(@TacheId IS NOT NULL)
			BEGIN
				--PRINT 'UPDATE de CiId('+ CAST(@CiId as varchar(10)) +') OperationDiverseId = ' + CAST(@OperationDiverseId as varchar(10)) + ' SET TacheId = ' + CAST(@TacheId as varchar(10));
				UPDATE FRED_OPERATION_DIVERSE
				SET TacheId = @TacheId
				WHERE OperationDiverseId = @OperationDiverseId;
			END

		DELETE FROM #tmpOD  WHERE CiId = @CiId AND OperationDiverseId = @OperationDiverseId;
	END
DROP TABLE #tmpOD;
		
	