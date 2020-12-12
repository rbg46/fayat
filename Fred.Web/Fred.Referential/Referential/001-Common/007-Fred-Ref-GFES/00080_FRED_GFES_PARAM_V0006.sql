 Declare @ShowenInputsId INT ;
 DECLARE @HiddenInputsId INT ;
 DECLARE @OrganisationId INT ;
 DECLARE @CountRow INT ;

 if exists (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%ShowenInputs%')
 BEGIN
 set @ShowenInputsId = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%ShowenInputs%')
 END

 ELSE 
 BEGIN
 INSERT INTO FRED_PARAM_KEY ([Libelle], [Description], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId]) VALUES ('ShowenInputs','la Clé pour identifier les composant à afficher pour une organisation donné','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1);
 set @ShowenInputsId = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%ShowenInputs%')
 END

 if exists (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%HiddenInputs%')
 BEGIN
 set @HiddenInputsId = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%HiddenInputs%')
 END

 ELSE 
 BEGIN
 INSERT INTO FRED_PARAM_KEY ([Libelle], [Description], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId]) VALUES ('HiddenInputs','la Clé pour identifier les composant à masquer pour une organisation donné','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1);
 set @HiddenInputsId = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%HiddenInputs%')
 END

 DECLARE MY_CURSOR CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
 FOR 
  SELECT DISTINCT org.OrganisationId FROM FRED_ORGANISATION org , FRED_GROUPE grp 
  WHERE  ( org.OrganisationId = grp.OrganisationId OR grp.OrganisationId=org.pereid) and grp.Code = 'GFES'

OPEN MY_CURSOR
FETCH NEXT FROM MY_CURSOR INTO @OrganisationId
WHILE @@FETCH_STATUS = 0
BEGIN 
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='personnel-statut-rzb' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('personnel-statut-rzb','2018-11-08 00:00:00.000','2018-11-08 00:00:00.000',1,1,@OrganisationId, @HiddenInputsId);
	  END
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='personnel-statut-fes' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('personnel-statut-fes','2018-11-08 00:00:00.000','2018-11-08 00:00:00.000',1,1,@OrganisationId, @ShowenInputsId);
	  END
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='personnel-coordonnes' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('personnel-coordonnes','2018-11-08 00:00:00.000','2018-11-08 00:00:00.000',1,1,@OrganisationId, @HiddenInputsId);
	  END
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='personnel-signature' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('personnel-signature','2018-11-08 00:00:00.000','2018-11-08 00:00:00.000',1,1,@OrganisationId, @HiddenInputsId);
	  END
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='personnel-droit-commande' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('personnel-droit-commande','2018-11-08 00:00:00.000','2018-11-08 00:00:00.000',1,1,@OrganisationId, @HiddenInputsId);
	  END 
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='personnel-manager' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('personnel-manager','2018-11-08 00:00:00.000','2018-11-08 00:00:00.000',1,1,@OrganisationId, @ShowenInputsId);
	  END 
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='default-ci' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('default-ci','2018-11-08 00:00:00.000','2018-11-08 00:00:00.000',1,1,@OrganisationId, @ShowenInputsId);
	  END
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='materiel-detail' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('materiel-detail','2018-11-08 00:00:00.000','2018-11-08 00:00:00.000',1,1,@OrganisationId, @HiddenInputsId);
	  END
  FETCH NEXT FROM MY_CURSOR INTO @OrganisationId 
END
CLOSE MY_CURSOR
DEALLOCATE MY_CURSOR

