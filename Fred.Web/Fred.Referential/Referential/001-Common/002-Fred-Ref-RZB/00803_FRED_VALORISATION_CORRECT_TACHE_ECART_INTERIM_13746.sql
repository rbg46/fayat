UPDATE V
SET V.TacheId = ExpectedTask.TacheId
FROM
    FRED_VALORISATION V
    JOIN FRED_TACHE ActualTask ON
        ActualTask.TacheId = V.TacheId
        AND ActualTask.CiId <> V.CiId
        AND ActualTask.TacheType = 8
        AND ActualTask.Niveau = 3
        AND ActualTask.Active = 1
    JOIN FRED_TACHE ExpectedTask ON
        ExpectedTask.CiId = V.CiId
        AND ExpectedTask.TacheType = 8
        AND ExpectedTask.Niveau = 3
        AND ExpectedTask.Active = 1