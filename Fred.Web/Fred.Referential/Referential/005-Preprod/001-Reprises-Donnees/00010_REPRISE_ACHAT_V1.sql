
-------------------------------------- US 4302  REPRISE DE DONNEES FRED_DEPENSE_ACHAT et FRED_FACTURATION ----------------------------------


--------------------------------------------------------------------------------------------------------------------------------------------
--															RG_4302_005
--------------------------------------------------------------------------------------------------------------------------------------------
UPDATE FRED_FACTURATION SET TotalFarHt = NULL, MouvementFarHt = NULL, QuantiteFar = NULL;


--------------------------------------------------------------------------------------------------------------------------------------------
--															RG_4302_006
--------------------------------------------------------------------------------------------------------------------------------------------
UPDATE FRED_DEPENSE_ACHAT SET AfficherPuHt = 1, AfficherQuantite = 1;


--------------------------------------------------------------------------------------------------------------------------------------------
--															RG_4302_007
--------------------------------------------------------------------------------------------------------------------------------------------
PRINT 'DEBUT | Mise à niveau des lignes ‘2 – Facture’ vers le nouveau modèle'
UPDATE
    enfant
SET
    enfant.Quantite = enfant.QuantiteDepense, 
    enfant.DateOperation = fact.DateComptable, 
    enfant.AfficherPuHt = 1, 
    enfant.AfficherQuantite =1,
    enfant.MontantHtInitial = parent.PUHT * parent.Quantite, 
    enfant.CommandeLigneId = parent.CommandeLigneId, 
    enfant.AuteurModificationId = 2
    ,enfant.DateModification = GETDATE()  
FROM FRED_DEPENSE_ACHAT enfant
INNER JOIN FRED_DEPENSE_ACHAT parent ON enfant.DepenseParentId = parent.DepenseId
INNER JOIN FRED_FACTURATION fact ON fact.DepenseAchatFactureId = enfant.DepenseId
WHERE enfant.DepenseTypeId = 2 -- Type Facture
        AND enfant.DateOperation IS NULL
        AND enfant.MontantHtInitial IS NULL
        AND enfant.QuantiteDepense <> 0
        AND enfant.Quantite = 0;




        
--------------------------------------------------------------------------------------------------------------------------------------------
--															RG_4302_008
--------------------------------------------------------------------------------------------------------------------------------------------

PRINT 'DEBUT | Mise à niveau des lignes ‘3 – Facture Ecart’ vers le nouveau modèle'
UPDATE
    enfant
SET
    enfant.Quantite = enfant.QuantiteDepense, 
    enfant.DateOperation = fact.DateComptable, 
    enfant.AfficherPuHt = CASE WHEN fact.EcartPu <> 0 AND fact.EcartQuantite > 0 AND fact.Quantite > fact.EcartQuantite THEN 0 ELSE 1 END, 
    enfant.AfficherQuantite = CASE WHEN fact.EcartQuantite <= 0 AND fact.FacturationTypeId = 1 THEN 0 ELSE 1 END,
    enfant.MontantHtInitial = parent.PUHT * parent.Quantite, 
    enfant.CommandeLigneId = parent.CommandeLigneId, 
    enfant.AuteurModificationId = 2
    ,enfant.DateModification = GETDATE()  
FROM FRED_DEPENSE_ACHAT enfant
INNER JOIN FRED_DEPENSE_ACHAT parent ON enfant.DepenseParentId = parent.DepenseId
INNER JOIN FRED_FACTURATION fact ON fact.DepenseAchatFactureEcartId = enfant.DepenseId
WHERE enfant.DepenseTypeId = 3 -- Type Facture Ecart
        AND enfant.DateOperation IS NULL	    
        AND enfant.QuantiteDepense <> 0
        AND enfant.Quantite = 0;





--------------------------------------------------------------------------------------------------------------------------------------------
--															RG_4302_009
--------------------------------------------------------------------------------------------------------------------------------------------
GO
PRINT 'DEBUT | Création des lignes « 8 – Extourne FAR » pour toutes les réceptions facturées avec Ecart Quantité < Quantité facturée'
BEGIN  
  DECLARE @FacturationIdCur int
  DECLARE @ReceptionIdCur int
  DECLARE @QuantiteCur float
  DECLARE @EcartQuantiteCur float
  DECLARE @DateComptableCur datetime  
  DECLARE @Quantite float    
  
  DECLARE MY_CURSOR CURSOR LOCAL STATIC READ_ONLY FORWARD_ONLY
  FOR
  -- Récupération des facturations concernées par les Extournes FAR
  SELECT FacturationId, DepenseAchatReceptionId, Quantite, EcartQuantite, DateComptable
  FROM FRED_FACTURATION
  WHERE FacturationTypeId = 1 -- Type Facture
        AND Quantite > EcartQuantite
        AND DepenseAchatFarId IS NULL
        AND TotalFarHt IS NULL	

  OPEN MY_CURSOR
  FETCH NEXT FROM MY_CURSOR INTO @FacturationIdCur, @ReceptionIdCur, @QuantiteCur, @EcartQuantiteCur, @DateComptableCur
  WHILE @@FETCH_STATUS = 0
  BEGIN       			     	
    SET @Quantite = CASE WHEN @EcartQuantiteCur <= 0 THEN -@QuantiteCur
                         WHEN @QuantiteCur > @EcartQuantiteCur AND @EcartQuantiteCur > 0 THEN (@EcartQuantiteCur - @QuantiteCur) END;	

    -- a
    INSERT INTO FRED_DEPENSE_ACHAT (CiId, FournisseurId, Libelle, TacheId, RessourceId, Quantite, PUHT, Date, AuteurCreationId, DateCreation, Commentaire, DeviseId, NumeroBL, DepenseParentId, UniteId, DepenseTypeId, DateComptable, QuantiteDepense, AfficherPuHt, AfficherQuantite, DateOperation, MontantHtInitial, CommandeLigneId)	
    SELECT CiId, FournisseurId, Libelle, TacheId, RessourceId, @Quantite, PUHT, Date, 2, GETDATE(), Commentaire, DeviseId, NumeroBL, DepenseId, UniteId, 8, DateComptable, 0, 1, 1, @DateComptableCur, PUHT * Quantite, CommandeLigneId
    FROM FRED_DEPENSE_ACHAT 
    WHERE DepenseId = @ReceptionIdCur
    
    -- b
    UPDATE FRED_FACTURATION SET DepenseAchatFarId = @@IDENTITY WHERE FacturationId = @FacturationIdCur;

    FETCH NEXT FROM MY_CURSOR INTO @FacturationIdCur, @ReceptionIdCur, @QuantiteCur, @EcartQuantiteCur, @DateComptableCur
  END
  CLOSE MY_CURSOR
  DEALLOCATE MY_CURSOR
END







--------------------------------------------------------------------------------------------------------------------------------------------
--															RG_4302_010
--------------------------------------------------------------------------------------------------------------------------------------------
GO
PRINT 'DEBUT | Création des lignes « 7 – Ajustement FAR SAP » pour toutes les réceptions facturées avec Ecart Quantité > 0'
BEGIN  
  DECLARE @FacturationIdCur int
  DECLARE @ReceptionIdCur int
  DECLARE @QuantiteCur float
  DECLARE @EcartQuantiteCur float
  DECLARE @DateComptableCur datetime
  DECLARE @DatePieceSapCur datetime
  DECLARE @MontantHtCur float
  DECLARE @EcartPuCur float
  DECLARE @Quantite float
  DECLARE @ShowPUHT bit  
  DECLARE @PuHt float

  DECLARE MY_CURSOR CURSOR LOCAL STATIC READ_ONLY FORWARD_ONLY
  FOR
  -- Récupération des facturations concernées par les Ajustement FAR SAP
  SELECT FacturationId, DepenseAchatReceptionId, Quantite, EcartQuantite, DateComptable, DatePieceSap, MontantHT, EcartPU
  FROM FRED_FACTURATION
  WHERE FacturationTypeId = 1 -- Type Facture
        AND EcartQuantite > 0
        AND DepenseAchatAjustementId IS NULL
        AND TotalFarHt IS NULL

  OPEN MY_CURSOR
  FETCH NEXT FROM MY_CURSOR INTO @FacturationIdCur, @ReceptionIdCur, @QuantiteCur, @EcartQuantiteCur, @DateComptableCur, @DatePieceSapCur, @MontantHtCur, @EcartPuCur
  WHILE @@FETCH_STATUS = 0
  BEGIN       			      	  	  
    SET @Quantite = CASE WHEN @EcartQuantiteCur > 0 AND @QuantiteCur > @EcartQuantiteCur THEN -@EcartQuantiteCur 
                         WHEN @EcartQuantiteCur >= @QuantiteCur THEN -@QuantiteCur END;	
    SET @ShowPUHT = CASE WHEN @EcartPuCur = 0 THEN 1 ELSE 0 END;	
    SET @PuHt = CASE WHEN @QuantiteCur > 0 THEN @MontantHtCur / @QuantiteCur ELSE 0 END;

    -- a
    INSERT INTO FRED_DEPENSE_ACHAT (CiId, FournisseurId, Libelle, TacheId, RessourceId, Quantite, PUHT, Date, AuteurCreationId, DateCreation, Commentaire, DeviseId, NumeroBL, DepenseParentId, UniteId, DepenseTypeId, DateComptable, QuantiteDepense, AfficherPuHt, AfficherQuantite, DateOperation, MontantHtInitial, CommandeLigneId)	
    SELECT CiId, FournisseurId, Libelle, TacheId, RessourceId, @Quantite, @PuHt, @DatePieceSapCur, 2, GETDATE(), Commentaire, DeviseId, NumeroBL, DepenseId, UniteId, 7, @DateComptableCur, 0, @ShowPUHT, 1, @DateComptableCur, PUHT * Quantite, CommandeLigneId FROM FRED_DEPENSE_ACHAT WHERE DepenseId = @ReceptionIdCur
    
    -- b
    UPDATE FRED_FACTURATION SET DepenseAchatAjustementId = @@IDENTITY WHERE FacturationId = @FacturationIdCur;
    
    FETCH NEXT FROM MY_CURSOR INTO @FacturationIdCur, @ReceptionIdCur, @QuantiteCur, @EcartQuantiteCur, @DateComptableCur, @DatePieceSapCur, @MontantHtCur, @EcartPuCur
  END
  CLOSE MY_CURSOR
  DEALLOCATE MY_CURSOR
END
