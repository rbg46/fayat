 Declare @IdHeuresSemaineOuvrierETAM INT ;
 Declare @IdHeuresJourOuvrierETAM INT ;
 Declare @IdHeuresSemaineIAC INT ;
 Declare @IdHeuresJourIAC INT ;

 DECLARE @OrganisationId INT ;
 DECLARE @CountRow INT ;

 if exists (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%HeuresSemaineIAC%')
 BEGIN
 set @IdHeuresSemaineIAC = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%HeuresSemaineIAC%')
 END

 ELSE 
 BEGIN
 INSERT INTO FRED_PARAM_KEY ([Libelle], [Description], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId]) VALUES ('HeuresSemaineIAC','la Clé pour identifier les heures travails par semaine pour statut IAC','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1);
 set @IdHeuresSemaineIAC = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%HeuresSemaineIAC%')
 END
 
 if exists (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%HeuresSemaineOuvrierETAM%')
 BEGIN
 set @IdHeuresSemaineOuvrierETAM = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%HeuresSemaineOuvrierETAM%')
 END

 ELSE 
 BEGIN
 INSERT INTO FRED_PARAM_KEY ([Libelle], [Description], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId]) VALUES ('HeuresSemaineOuvrierETAM','la Clé pour identifier les heures travails par semaine pour statuts Ouvrier & ETAM','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1);
 set @IdHeuresSemaineOuvrierETAM = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%HeuresSemaineOuvrierETAM%')
 END

 if exists (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%HeuresJourIAC%')
 BEGIN
 set @IdHeuresJourIAC = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%HeuresJourIAC%')
 END

 ELSE 
 BEGIN
 INSERT INTO FRED_PARAM_KEY ([Libelle], [Description], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId]) VALUES ('HeuresJourIAC','la Clé pour identifier les heures travails par jour pour statut IAC','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1);
 set @IdHeuresJourIAC = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%HeuresJourIAC%')
 END
 
 if exists (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%HeuresJourOuvrierETAM%')
 BEGIN
 set @IdHeuresJourOuvrierETAM = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%HeuresJourOuvrierETAM%')
 END

 ELSE 
 BEGIN
 INSERT INTO FRED_PARAM_KEY ([Libelle], [Description], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId]) VALUES ('HeuresJourOuvrierETAM','la Clé pour identifier les heures travails par jour pour statuts Ouvrier & ETAM','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1);
 set @IdHeuresJourOuvrierETAM = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%HeuresJourOuvrierETAM%')
 END

 set @CountRow = (select count(*) from FRED_PARAM_VALUE)
 if(@countRow > 0)
 begin
 delete from FRED_PARAM_VALUE WHERE ParamKeyId IN (select ParamKeyId from FRED_PARAM_KEY WHERE ParamKeyId IN (@IdHeuresJourOuvrierETAM,@IdHeuresJourIAC,@IdHeuresSemaineOuvrierETAM,@IdHeuresSemaineIAC) )
 end

 DECLARE MY_CURSOR CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
 FOR 
   SELECT DISTINCT org.OrganisationId FROM FRED_ORGANISATION org , FRED_GROUPE grp 
  WHERE (org.OrganisationId = grp.OrganisationId OR grp.OrganisationId=org.pereid) and grp.Code = 'GFES'

OPEN MY_CURSOR
FETCH NEXT FROM MY_CURSOR INTO @OrganisationId
WHILE @@FETCH_STATUS = 0
BEGIN 
    
  if not exists (select ParamValueId from FRED_PARAM_VALUE where OrganisationId = @OrganisationId AND ParamKeyId= @IdHeuresSemaineOuvrierETAM)
	begin
  INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('35','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId, @IdHeuresSemaineOuvrierETAM);
	end
	 if not exists (select ParamValueId from FRED_PARAM_VALUE where OrganisationId = @OrganisationId AND ParamKeyId= @IdHeuresJourIAC)
	begin
  INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('7','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId, @IdHeuresJourIAC);   
	end
     if not exists (select ParamValueId from FRED_PARAM_VALUE WHERE OrganisationId = @OrganisationId AND ParamKeyId= @IdHeuresJourOuvrierETAM)
	begin
  INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('7','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId, @IdHeuresJourOuvrierETAM);
	end
  FETCH NEXT FROM MY_CURSOR INTO @OrganisationId
END
CLOSE MY_CURSOR
DEALLOCATE MY_CURSOR

