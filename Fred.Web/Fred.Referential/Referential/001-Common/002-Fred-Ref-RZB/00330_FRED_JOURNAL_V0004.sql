DECLARE  @Oth int = 0

SELECT @Oth = FamilleOperationDiverseId	FROM FRED_FAMILLE_OPERATION_DIVERSE WHERE CODE ='OTH' AND SocieteId = 1 

IF NOT EXISTS(SELECT * FROM FRED_JOURNAL WHERE CODE ='ZAB')
BEGIN
    INSERT INTO FRED_JOURNAL (SocieteId, Code, Libelle, DateCreation, TypeJournal,ParentFamilyODWithOrder,ParentFamilyODWithoutOrder) 
    VALUES (1,'HIS','Historique au statut Actif',GETDATE(),'Y', 0 , @Oth ) 	
END