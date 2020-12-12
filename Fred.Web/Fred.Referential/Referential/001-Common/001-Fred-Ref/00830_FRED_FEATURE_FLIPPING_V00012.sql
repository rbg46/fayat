IF NOT EXISTS(SELECT * FROM FRED_FEATURE_FLIPPING WHERE Name ='AfficherOngletMaterielsExternesRapports')
BEGIN
    INSERT INTO FRED_FEATURE_FLIPPING ([Name],[IsActived],[DateActivation],[UserActivation],[Code])
    Values (N'AfficherOngletMaterielsExternesRapports', 1, GetDate(), N'super_fred', 17)
END 
