-- Activation des codes d'absence
DECLARE @GroupeFESId INT;
SET @GroupeFESId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code='GFES')

UPDATE FRED_CODE_ABSENCE SET Actif=1 WHERE SocieteId in (SELECT SocieteId FROM FRED_SOCIETE WHERE GroupeId=@GroupeFESId)