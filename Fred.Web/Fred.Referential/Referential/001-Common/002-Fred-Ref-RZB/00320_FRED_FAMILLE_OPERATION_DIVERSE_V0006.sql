BEGIN

    UPDATE J SET ParentFamilyODWithoutOrder =  FOD.FamilleOperationDiverseId
    FROM FRED_JOURNAL J 
    INNER JOIN  FRED_FAMILLE_OPERATION_DIVERSE FOD  ON FOD.SocieteId = j.SocieteId and FOD.CODE ='MIT'
     WHERE J.CODE IN('AES','FMT','NDG')

    UPDATE J SET ParentFamilyODWithOrder =  FOD.FamilleOperationDiverseId
    FROM FRED_JOURNAL J 
    INNER JOIN  FRED_FAMILLE_OPERATION_DIVERSE FOD  ON FOD.SocieteId = j.SocieteId and FOD.CODE ='ACH'
    WHERE J.CODE IN('ALF')

    UPDATE J SET ParentFamilyODWithoutOrder =  FOD.FamilleOperationDiverseId
    FROM FRED_JOURNAL J
    INNER JOIN  FRED_FAMILLE_OPERATION_DIVERSE FOD  ON FOD.SocieteId = j.SocieteId and FOD.CODE ='OTH'
    WHERE J.CODE IN('ALF')

END