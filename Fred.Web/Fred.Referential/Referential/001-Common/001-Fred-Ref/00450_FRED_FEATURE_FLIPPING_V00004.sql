IF NOT EXISTS(SELECT * FROM FRED_FEATURE_FLIPPING WHERE Name ='blocageFournisseursSansSIRET')
BEGIN
    Insert Into FRED_FEATURE_FLIPPING ([Name],[IsActived],[DateActivation],[UserActivation],[Code])
    Values (N'blocageFournisseursSansSIRET',1,GetDate(),N'super_fred', 4)
    -- Code = 4 (cf. Fred.Framework\FeatureFlipping\EnumFeatureFlipping.cs)
END