IF NOT EXISTS(SELECT * FROM FRED_FEATURE_FLIPPING WHERE Name ='ExplorateurDepensesFiltresMOMateriels')
BEGIN
    INSERT INTO FRED_FEATURE_FLIPPING ([Name],[IsActived],[DateActivation],[UserActivation],[Code])
    Values (N'ExplorateurDepensesFiltresMOMateriels', 0, GetDate(), N'super_fred', 13)
END