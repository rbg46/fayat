DECLARE @SocieteId int
SELECT TOP 1 @SocieteId	= SocieteId FROM FRED_SOCIETE WHERE CODE = 'MBTP'

IF(@SocieteId IS NOT NULL)
BEGIN
    DELETE FROM FRED_JOURNAL WHERE SocieteId = @SocieteId
    SELECT * 
    INTO #Src
    FROM FRED_FAMILLE_OPERATION_DIVERSE Src
    WHERE SocieteId = 1
    
    SELECT * 
    INTO #Cible
    FROM FRED_FAMILLE_OPERATION_DIVERSE 
    WHERE SocieteId = @SocieteId
    
    SELECT s.FamilleOperationDiverseId as IdSource, c.FamilleOperationDiverseId as Cible  
    INTO #Result
    FROM #Src s INNER JOIN  #Cible c on c.Code = s.Code
    
    INSERT INTO FRED_JOURNAL
    SELECT @SocieteId, Code,Libelle,ImportFacture,getdate(),1,	null,AuteurModificationId,DateCloture,AuteurClotureId,TypeJournal,
    r.Cible as ParentFamilyODWithOrder,
    rr.Cible  as ParentFamilyODWithoutOrder
    FROM FRED_JOURNAL  f 
        INNER JOIN  #Result r on r.IdSource = f.ParentFamilyODWithOrder 
        INNER JOIN  #Result rr on rr.IdSource = f.ParentFamilyODWithoutOrder
    WHERE f.SocieteId = 1
    
    DROP TABLE #Src
    DROP TABLE #Cible
    DROP TABLE #Result
END