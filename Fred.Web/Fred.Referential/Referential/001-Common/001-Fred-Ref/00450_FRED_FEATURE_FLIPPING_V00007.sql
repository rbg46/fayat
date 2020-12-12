IF NOT EXISTS(SELECT * FROM FRED_FEATURE_FLIPPING WHERE Name ='correctionFournisseurSAP')
BEGIN
    Insert Into FRED_FEATURE_FLIPPING ([Name],[IsActived],[DateActivation],[UserActivation],[Code])
    Values (N'correctionFournisseurSAP', 1, GetDate(), N'super_fred', 8)    
END