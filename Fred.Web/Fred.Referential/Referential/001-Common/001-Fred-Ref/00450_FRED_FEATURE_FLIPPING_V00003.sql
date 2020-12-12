IF NOT EXISTS(SELECT * FROM FRED_FEATURE_FLIPPING WHERE Name ='activationClotureOperationDiverses')
BEGIN
    Insert Into FRED_FEATURE_FLIPPING ([Name],[IsActived],[DateActivation],[UserActivation],[Code])
    Values (N'activationClotureOperationDiverses',1,GetDate(),N'super_fred', 3)
    -- Code = 3 (cf. Fred.Framework\FeatureFlipping\EnumFeatureFlipping.cs)
END