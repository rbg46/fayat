 Declare @IdLockUnlockCommandeLigne INT ;

 DECLARE @OrganisationId INT ;
 DECLARE @CountRow INT ;

 --- Ajout des clés
 --- lock.unlock.commandeLigne
 if exists (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%lock.unlock.commandeLigne%')
 BEGIN
 set @IdLockUnlockCommandeLigne = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%lock.unlock.commandeLigne%')
 END

 ELSE 
 BEGIN
 INSERT INTO FRED_PARAM_KEY (Libelle, Description, DateCreation, DateModification, AuteurCreationId, AuteurModificationId) VALUES ('lock.unlock.commandeLigne','la Clé pour identifier si la fonctionnalité de verouillage de ligne de commande est active',GETDATE(),GETDATE(),1,1);
 set @IdLockUnlockCommandeLigne = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%lock.unlock.commandeLigne%')
 END

 --- Ajout des valeurs
 set @CountRow = (select count(*) from FRED_PARAM_VALUE)
 if(@countRow > 0)
 begin
 delete from FRED_PARAM_VALUE WHERE ParamKeyId IN (select ParamKeyId from FRED_PARAM_KEY WHERE ParamKeyId IN (@IdLockUnlockCommandeLigne) )
 end

 DECLARE MY_CURSOR CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
 FOR 
   SELECT DISTINCT org.OrganisationId FROM FRED_ORGANISATION org , FRED_GROUPE grp 
  WHERE (org.OrganisationId = grp.OrganisationId OR grp.OrganisationId=org.pereid) and grp.Code = 'GFTP'

OPEN MY_CURSOR
FETCH NEXT FROM MY_CURSOR INTO @OrganisationId
WHILE @@FETCH_STATUS = 0
BEGIN 
	 --- lock.unlock.commandeLigne
	if not exists (select ParamValueId from FRED_PARAM_VALUE where ParamKeyId = @IdLockUnlockCommandeLigne and OrganisationId = @OrganisationId)
	begin
		INSERT INTO FRED_PARAM_VALUE (Value, DateCreation, DateModification, AuteurCreationId, AuteurModificationId, OrganisationId, ParamKeyId) VALUES ('enabled',GETDATE(),GETDATE(),1,1,@OrganisationId, @IdLockUnlockCommandeLigne);
	end
  FETCH NEXT FROM MY_CURSOR INTO @OrganisationId
END
CLOSE MY_CURSOR
DEALLOCATE MY_CURSOR
