IF NOT EXISTS(SELECT * FROM FRED_FEATURE_FLIPPING WHERE Name ='ActivateDesactivateFiltrePointageSyntheseMensuelle')
BEGIN
    Insert Into FRED_FEATURE_FLIPPING ([Name],[IsActived],[DateActivation],[UserActivation],[Code])
    Values ('ActivateDesactivateFiltrePointageSyntheseMensuelle',0,GetDate(),N'super_fred', 19)
END
ELSE
BEGIN
Update FRED_FEATURE_FLIPPING SET Code = 19 WHERE Name = 'ActivateDesactivateFiltrePointageSyntheseMensuelle';
END
