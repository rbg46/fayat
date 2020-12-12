IF NOT EXISTS(SELECT * FROM FRED_FEATURE_FLIPPING WHERE Name = 'PersonnelsNouveauxFiltres')
BEGIN
    Insert Into FRED_FEATURE_FLIPPING ([Name], [IsActived], [DateActivation], [UserActivation], [Code])
    Values (N'PersonnelsNouveauxFiltres',1, GetDate(), N'super_fred', 9)
    -- Code = 9 (cf. Fred.Framework\FeatureFlipping\EnumFeatureFlipping.cs)
END