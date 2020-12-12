DECLARE @HiddenInputsId INT ;
DECLARE @OrganisationId int;

 set @HiddenInputsId = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%HiddenInputs%');

 DECLARE MY_CURSOR CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
 FOR 
  SELECT DISTINCT org.OrganisationId FROM FRED_ORGANISATION org , FRED_GROUPE grp 
  WHERE  ( org.OrganisationId = grp.OrganisationId OR grp.OrganisationId=org.pereid) and grp.Code = 'GFES'

OPEN MY_CURSOR
FETCH NEXT FROM MY_CURSOR INTO @OrganisationId
WHILE @@FETCH_STATUS = 0
BEGIN

INSERT INTO FRED_PARAM_VALUE(Value, DateCreation, DateModification, AuteurCreationId, AuteurModificationId, OrganisationId, ParamKeyId) VALUES ('radioPrimesHeuresJour',getDate(),getDate(),1,1,@OrganisationId,@HiddenInputsId)
  FETCH NEXT FROM MY_CURSOR INTO @OrganisationId
END
CLOSE MY_CURSOR
DEALLOCATE MY_CURSOR