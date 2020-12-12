--Update Prime set multi pointage
DECLARE @GroupFESId INT;
SET @GroupFESId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code ='GFES')

UPDATE FRED_PRIME set MultiPerDay = 1 where GroupeId = @GroupFESId AND Code in ('GDI','GDP','AE','INS','ES');