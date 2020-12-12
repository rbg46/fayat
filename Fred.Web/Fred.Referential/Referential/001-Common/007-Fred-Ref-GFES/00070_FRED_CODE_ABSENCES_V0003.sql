--Insertion du code d'absence RA
DECLARE @SocieteId INT;
DECLARE @GroupeId  INT;
Set @GroupeId = (SELECT GroupeId from FRED_GROUPE WHERE Code = 'GFES');

IF(NOT EXISTS (SELECT a.CodeAbsenceId FROM FRED_CODE_ABSENCE a WHERE a.Code='RA' AND a.GroupeId=@GroupeId))
BEGIN
INSERT INTO FRED_CODE_ABSENCE (SocieteId, HoldingId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) VALUES(null,null,'RA','Repos autorisé',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId, null, 0);
END

DECLARE MY_CURSOR CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
FOR 
SELECT DISTINCT SocieteId 
FROM FRED_SOCIETE where GroupeId=@GroupeId

OPEN MY_CURSOR
FETCH NEXT FROM MY_CURSOR INTO @SocieteId
WHILE @@FETCH_STATUS = 0
BEGIN 
   
  INSERT INTO FRED_CODE_ABSENCE (HoldingId, SocieteId, Code, Libelle, Intemperie, TauxDecote, NBHeuresDefautETAM, NBHeuresMinETAM, NBHeuresMaxETAM, NBHeuresDefautCO, NBHeuresMinCO, NBHeuresMaxCO, Actif, GroupeId, CodeAbsenceParentId, Niveau) 
  VALUES (null, @SocieteId, 'RA','Repos autorisé',0, 1,7.5,1,7.5,7.5,1,7.5,1,@GroupeId, null, 0);

  FETCH NEXT FROM MY_CURSOR INTO @SocieteId
END
CLOSE MY_CURSOR
DEALLOCATE MY_CURSOR