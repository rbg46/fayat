Declare @ShowenInputsId INT ;
DECLARE @HiddenInputsId INT ;
DECLARE @OrganisationId int;

 set @ShowenInputsId = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%ShowenInputs%');
 set @HiddenInputsId = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%HiddenInputs%');

 DECLARE MY_CURSOR CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
 FOR 
  SELECT DISTINCT org.OrganisationId FROM FRED_ORGANISATION org , FRED_GROUPE grp 
  WHERE  ( org.OrganisationId = grp.OrganisationId OR grp.OrganisationId=org.pereid) and grp.Code = 'GRZB'

OPEN MY_CURSOR
FETCH NEXT FROM MY_CURSOR INTO @OrganisationId
WHILE @@FETCH_STATUS = 0
BEGIN 

--showen
  INSERT INTO FRED_PARAM_VALUE(Value, DateCreation, DateModification, AuteurCreationId, AuteurModificationId, OrganisationId, ParamKeyId)
							   VALUES ('etab-paie-non-pointables-personnels-div',getDate(),getDate(),1,1,@OrganisationId,@ShowenInputsId)
  INSERT INTO FRED_PARAM_VALUE(Value, DateCreation, DateModification, AuteurCreationId, AuteurModificationId, OrganisationId, ParamKeyId) 
							   VALUES ('personnel-non-pointable-personnel-div',getDate(),getDate(),1,1,@OrganisationId,@ShowenInputsId)
  INSERT INTO FRED_PARAM_VALUE(Value, DateCreation, DateModification, AuteurCreationId, AuteurModificationId, OrganisationId, ParamKeyId) 
                               VALUES ('ci-team-handle-pointage-restrictions',getDate(),getDate(),1,1,@OrganisationId,@ShowenInputsId)
--Hidden
  INSERT INTO FRED_PARAM_VALUE(Value, DateCreation, DateModification, AuteurCreationId, AuteurModificationId, OrganisationId, ParamKeyId)
                               VALUES ('ci-team-statut-filter',getDate(),getDate(),1,1,@OrganisationId,@HiddenInputsId)
  INSERT INTO FRED_PARAM_VALUE(Value, DateCreation, DateModification, AuteurCreationId, AuteurModificationId, OrganisationId, ParamKeyId)
                               VALUES ('ci-team-handle-astreintes',getDate(),getDate(),1,1,@OrganisationId,@HiddenInputsId)
  INSERT INTO FRED_PARAM_VALUE(Value, DateCreation, DateModification, AuteurCreationId, AuteurModificationId, OrganisationId, ParamKeyId)
                               VALUES ('ci-team-import-team',getDate(),getDate(),1,1,@OrganisationId,@HiddenInputsId)
  INSERT INTO FRED_PARAM_VALUE(Value, DateCreation, DateModification, AuteurCreationId, AuteurModificationId, OrganisationId, ParamKeyId)
                               VALUES ('ci-team-statut-col',getDate(),getDate(),1,1,@OrganisationId,@HiddenInputsId)
  INSERT INTO FRED_PARAM_VALUE(Value, DateCreation, DateModification, AuteurCreationId, AuteurModificationId, OrganisationId, ParamKeyId)
                               VALUES ('ci-team-statut',getDate(),getDate(),1,1,@OrganisationId,@HiddenInputsId)
  INSERT INTO FRED_PARAM_VALUE(Value, DateCreation, DateModification, AuteurCreationId, AuteurModificationId, OrganisationId, ParamKeyId)
                               VALUES ('ci-team-favorite',getDate(),getDate(),1,1,@OrganisationId,@HiddenInputsId)
  INSERT INTO FRED_PARAM_VALUE(Value, DateCreation, DateModification, AuteurCreationId, AuteurModificationId, OrganisationId, ParamKeyId)
                               VALUES ('ci-team-delegation',getDate(),getDate(),1,1,@OrganisationId,@HiddenInputsId)
  
  FETCH NEXT FROM MY_CURSOR INTO @OrganisationId
END
CLOSE MY_CURSOR
DEALLOCATE MY_CURSOR