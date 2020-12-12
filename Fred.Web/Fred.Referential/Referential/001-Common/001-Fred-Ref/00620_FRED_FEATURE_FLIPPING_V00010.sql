IF NOT EXISTS(SELECT * FROM FRED_FEATURE_FLIPPING WHERE Name ='verificationCoordonneesGPS')
BEGIN
    INSERT INTO FRED_FEATURE_FLIPPING ([Name],[IsActived],[DateActivation],[UserActivation],[Code])
    Values (N'verificationCoordonneesGPS', 1, GetDate(), N'super_fred', 10)    
END
