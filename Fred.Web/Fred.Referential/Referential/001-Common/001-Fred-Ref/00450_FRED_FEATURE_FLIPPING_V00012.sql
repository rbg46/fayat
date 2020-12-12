IF NOT EXISTS(SELECT * FROM FRED_FEATURE_FLIPPING WHERE Name ='suppresionEquipeCI')
BEGIN
    INSERT INTO FRED_FEATURE_FLIPPING ([Name],[IsActived],[DateActivation],[UserActivation],[Code])
    Values (N'suppresionEquipeCI', 0, GetDate(), N'super_fred', 12)
END 
