DECLARE @SocieteId int
SELECT TOP 1 @SocieteId	= SocieteId FROM FRED_SOCIETE WHERE CODE = 'MBTP'

IF(@SocieteId IS NOT NULL)
BEGIN
    --DELETE FROM FRED_JOURNAL WHERE SocieteId = @SocieteId
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
    
	   SELECT * 
	   INTO #Exist
	   FROM FRED_JOURNAL
	   WHERE societeId = @SocieteId

    INSERT INTO FRED_JOURNAL(SocieteId,Code,Libelle,ImportFacture,DateCreation,AuteurCreationId,DateModification,AuteurModificationId,DateCloture,AuteurClotureId,TypeJournal,ParentFamilyODWithOrder,ParentFamilyODWithoutOrder)
    SELECT @SocieteId, f.Code,f.Libelle,f.ImportFacture,getdate(),1,	null,f.AuteurModificationId,f.DateCloture,f.AuteurClotureId,f.TypeJournal,
    CASE WHEN r.Cible IS NULL THEN  0 ELSE r.Cible END as ParentFamilyODWithOrder,
    CASE WHEN rr.Cible IS NULL THEN  0 ELSE rr.Cible END as ParentFamilyODWithoutOrder
    FROM FRED_JOURNAL  f 
        LEFT JOIN  #Result r on r.IdSource = f.ParentFamilyODWithOrder
        LEFT JOIN  #Result rr on rr.IdSource = f.ParentFamilyODWithoutOrder 
		      LEFT JOIN #Exist e on e.SocieteId = @SocieteId and e.Code = f.Code
    WHERE f.SocieteId = 1 AND f.Code not in(SELECT Code from #Exist)
	
	  
    DROP TABLE #Src
    DROP TABLE #Cible
    DROP TABLE #Result
	
END