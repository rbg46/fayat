-- Correction des statuts des personnels

DECLARE @SocId INT;
SET @SocId = (SELECT SocieteId FROM FRED_SOCIETE WHERE Code='E001')

UPDATE FRED_PERSONNEL SET Statut='1' WHERE Statut='O' and SocieteId=@SocId
UPDATE FRED_PERSONNEL SET Statut='2' WHERE Statut='E' and SocieteId=@SocId
UPDATE FRED_PERSONNEL SET Statut='3' WHERE Statut='C' and SocieteId=@SocId