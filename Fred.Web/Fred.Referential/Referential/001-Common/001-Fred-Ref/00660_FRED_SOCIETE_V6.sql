-- Mise à jour des TypeSocieteId de chaque Société

IF EXISTS (SELECT TypeSocieteId FROM FRED_TYPE_SOCIETE WHERE Code like 'INT')
BEGIN
    UPDATE FRED_SOCIETE 
    SET TypeSocieteId = (SELECT TypeSocieteId FROM FRED_TYPE_SOCIETE WHERE Code like 'INT')
    WHERE IsInterimaire = 0 
          AND Externe = 0 
          AND TypeSocieteId IS NULL
END