IF NOT EXISTS(SELECT * FROM FRED_FEATURE_FLIPPING WHERE Name ='SiteWebEnMaintance')
BEGIN
    INSERT INTO FRED_FEATURE_FLIPPING ([Name],[IsActived],[DateActivation],[UserActivation],[Code])
    Values (N'SiteWebEnMaintance', 0, GetDate(), N'super_fred', 22)
END 
