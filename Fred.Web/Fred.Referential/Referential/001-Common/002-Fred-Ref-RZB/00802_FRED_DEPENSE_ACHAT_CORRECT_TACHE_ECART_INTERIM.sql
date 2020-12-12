UPDATE DA
SET DA.TacheId = ExpectedTask.TacheId
FROM
    FRED_DEPENSE_ACHAT DA
    JOIN FRED_TACHE ActualTask ON
        ActualTask.TacheId = DA.TacheId
        AND ActualTask.CiId <> DA.CiId
        AND ActualTask.TacheType = 8
        AND ActualTask.Niveau = 3
        AND ActualTask.Active = 1
    JOIN FRED_TACHE ExpectedTask ON
        ExpectedTask.CiId = DA.CiId
        AND ExpectedTask.TacheType = 8
        AND ExpectedTask.Niveau = 3
        AND ExpectedTask.Active = 1