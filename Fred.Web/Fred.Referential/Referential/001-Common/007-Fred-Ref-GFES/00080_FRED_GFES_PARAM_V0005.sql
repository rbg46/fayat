 DECLARE @HiddenInputsId INT ;
 DECLARE @OrganisationId INT ;
 DECLARE @CountRow INT ;

 
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
 delete from FRED_PARAM_VALUE where value like '%colonne_Materiel%'
 end

 DECLARE MY_CURSOR CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
 FOR 
 SELECT DISTINCT org.OrganisationId FROM FRED_ORGANISATION org , FRED_GROUPE grp 
   WHERE ( org.OrganisationId = grp.OrganisationId OR grp.OrganisationId=org.pereid) and grp.Code = 'GFES'

OPEN MY_CURSOR
FETCH NEXT FROM MY_CURSOR INTO @OrganisationId
WHILE @@FETCH_STATUS = 0
BEGIN 
   DECLARE @cnt INT = 1;

WHILE @cnt <= 26
BEGIN
  INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('colonne_Materiel'+CAST(@cnt AS VARCHAR(5)),'2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId, @HiddenInputsId);
  SET @cnt = @cnt + 1;

END;


  FETCH NEXT FROM MY_CURSOR INTO @OrganisationId
END
CLOSE MY_CURSOR
DEALLOCATE MY_CURSOR
