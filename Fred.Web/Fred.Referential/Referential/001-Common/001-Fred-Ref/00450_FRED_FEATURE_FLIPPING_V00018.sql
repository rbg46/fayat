-- Feature flipping pour le contrôle de l'adresse du Ci pour choisir le bon IGD
-- NB : Normalement ça doit étre un code 17, Le code 18 a été choisi car le code 17 est utilisé 
--      dans 00830_FRED_FEATURE_FLIPPING_V00012

IF NOT EXISTS(SELECT * FROM FRED_FEATURE_FLIPPING WHERE Name ='VerificationCodePostalCiPrimeGDIGDP')
BEGIN
    INSERT INTO FRED_FEATURE_FLIPPING ([Name],[IsActived],[DateActivation],[UserActivation],[Code])
    Values (N'VerificationCodePostalCiPrimeGDIGDP', 0, GetDate(), N'super_fred', 18)
END 