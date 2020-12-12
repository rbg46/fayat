IF NOT EXISTS(SELECT * FROM FRED_FEATURE_FLIPPING WHERE Name ='ParametrageAvancementDroitADepense')
BEGIN
    INSERT INTO FRED_FEATURE_FLIPPING ([Name],[IsActived],[DateActivation],[UserActivation],[Code])
    Values (N'ParametrageAvancementDroitADepense', 0, GetDate(), N'super_fred', 15)
END 