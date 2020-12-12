-- Feature flipping pour activer l'us 7943 
-- Modification d'un Ci lors de l'initialisation d'un rapport journalier


IF NOT EXISTS(SELECT * FROM FRED_FEATURE_FLIPPING WHERE Name ='ModificationCiRapportJournalier')
BEGIN
    INSERT INTO FRED_FEATURE_FLIPPING ([Name],[IsActived],[DateActivation],[UserActivation],[Code])
    Values (N'ModificationCiRapportJournalier', 1, GetDate(), N'super_fred', 20)
END 