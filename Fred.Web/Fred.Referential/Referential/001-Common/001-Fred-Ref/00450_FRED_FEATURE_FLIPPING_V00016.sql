IF NOT EXISTS(SELECT * FROM FRED_FEATURE_FLIPPING WHERE Name ='RapportsHorairesObligatoires')
BEGIN
    INSERT INTO FRED_FEATURE_FLIPPING ([Name],[IsActived],[DateActivation],[UserActivation],[Code])
    Values (N'RapportsHorairesObligatoires', 0, GetDate(), N'super_fred', 16)
END 