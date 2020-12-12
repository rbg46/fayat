IF NOT EXISTS(SELECT * FROM FRED_FEATURE_FLIPPING WHERE Name ='budgetDetailCopierDeplacerT4')
BEGIN
    INSERT INTO FRED_FEATURE_FLIPPING ([Name],[IsActived],[DateActivation],[UserActivation],[Code])
    Values (N'budgetDetailCopierDeplacerT4', 0, GetDate(), N'super_fred', 11)
END
