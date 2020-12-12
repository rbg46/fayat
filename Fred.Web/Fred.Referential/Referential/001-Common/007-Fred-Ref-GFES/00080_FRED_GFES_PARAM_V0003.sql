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

 set @CountRow = (select count(*) from FRED_PARAM_VALUE)
 if(@countRow > 0)
 begin
 delete from FRED_PARAM_VALUE where Value in ('controle', 'title-corespondant', 'title-gestionnaire', 'controle-chantier', 'controle-vrac',
            'controle-visaCorrespondant', 'controle-detail', 'footer-lot-pointage', 'controle-chantier-detail', 'controle-vrac-detail',
            'controle-corerspondant-detail', 'rd-choix-periode', 'btn-pointage-default', 'materiel-entete', 'astreinte-entete', 'ivd-entete', 'ivd-detail')
 end

 DECLARE MY_CURSOR CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
 FOR 
  SELECT DISTINCT org.OrganisationId FROM FRED_ORGANISATION org , FRED_GROUPE grp 
  WHERE  ( org.OrganisationId = grp.OrganisationId OR grp.OrganisationId=org.pereid) and grp.Code = 'GFES'

OPEN MY_CURSOR
FETCH NEXT FROM MY_CURSOR INTO @OrganisationId
WHILE @@FETCH_STATUS = 0
BEGIN 
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='controle' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('controle','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId, @ShowenInputsId);
	  END
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='title-corespondant' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('title-corespondant','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId,@HiddenInputsId);
	  END
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='title-gestionnaire' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('title-gestionnaire','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId,@ShowenInputsId);
	  END
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='controle-chantier' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('controle-chantier','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId,@HiddenInputsId)
	  END
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='controle-vrac' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('controle-vrac','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId,@HiddenInputsId)
	  END
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='controle-visaCorrespondant' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('controle-visaCorrespondant','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId,@HiddenInputsId)
	  END
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='controle-detai' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('controle-detail','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId,@ShowenInputsId)
	  END
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='footer-lot-pointage' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('footer-lot-pointage','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId,@HiddenInputsId)
	  END
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='controle-chantier-detail' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('controle-chantier-detail','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId,@HiddenInputsId)
	  END
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='controle-vrac-detail' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('controle-vrac-detail','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId,@HiddenInputsId)
	  END
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='controle-corerspondant-detail' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('controle-corerspondant-detail','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId,@HiddenInputsId)
	  END
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='rd-choix-periode' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('rd-choix-periode','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId,@ShowenInputsId)
	  END
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='btn-pointage-default' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('btn-pointage-default','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId,@HiddenInputsId)
	  END
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='materiel-entete' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('materiel-entete','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId,@HiddenInputsId)
	  END
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='astreinte-entete' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('astreinte-entete','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId,@ShowenInputsId)
	  END
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='ivd-entete' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('ivd-entete','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId,@HiddenInputsId)
	  END
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='ivd-detail' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId])  VALUES ('ivd-detail','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId,@HiddenInputsId)
	  END
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='date-astriente' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('date-astriente','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId,@ShowenInputsId)
	  END
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='materiel-detail' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('materiel-detail','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId,@HiddenInputsId)
	  END
  IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='astreinte-detail' )
	  BEGIN
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('astreinte-detail','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId,@ShowenInputsId)
	  END

  FETCH NEXT FROM MY_CURSOR INTO @OrganisationId
END
CLOSE MY_CURSOR
DEALLOCATE MY_CURSOR

