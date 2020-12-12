IF NOT EXISTS(SELECT * FROM FRED_FEATURE_FLIPPING WHERE Name ='filtreGererReceptions')
BEGIN
    Insert Into FRED_FEATURE_FLIPPING ([Name],[IsActived],[DateActivation],[UserActivation],[Code])
    Values (N'filtreGererReceptions',1,GetDate(),N'super_fred', 2)
    -- Code = 2 (cf. Fred.Framework\FeatureFlipping\EnumFeatureFlipping.cs)
END