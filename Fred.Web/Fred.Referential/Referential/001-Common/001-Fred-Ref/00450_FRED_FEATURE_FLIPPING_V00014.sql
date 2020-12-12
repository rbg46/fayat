IF NOT EXISTS(SELECT * FROM FRED_FEATURE_FLIPPING WHERE Name ='MultiplePeriodeOperationDiverses')
BEGIN
    INSERT INTO FRED_FEATURE_FLIPPING ([Name],[IsActived],[DateActivation],[UserActivation],[Code])
    Values (N'MultiplePeriodeOperationDiverses', 0, GetDate(), N'super_fred', 14)
END 
