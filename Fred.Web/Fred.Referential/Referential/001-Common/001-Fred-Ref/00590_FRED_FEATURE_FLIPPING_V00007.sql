IF NOT EXISTS(SELECT * FROM FRED_FEATURE_FLIPPING WHERE Name ='editionsPaieAmeliorations')
BEGIN
    Insert Into FRED_FEATURE_FLIPPING ([Name],[IsActived],[DateActivation],[UserActivation],[Code])
    Values (N'editionsPaieAmeliorations', 1, GetDate(), N'super_fred', 7)    
END