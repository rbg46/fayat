IF NOT EXISTS(SELECT * FROM FRED_FEATURE_FLIPPING WHERE Name ='nouveauMenuDetailCommande')
BEGIN
    Insert Into FRED_FEATURE_FLIPPING ([Name],[IsActived],[DateActivation],[UserActivation],[Code])
    Values (N'nouveauMenuDetailCommande',1,GetDate(),N'super_fred', 1)
    -- Code = 1 (cf. Fred.Framework\FeatureFlipping\EnumFeatureFlipping.cs)
END