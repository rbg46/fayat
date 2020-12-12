 Declare @IdSocieteCodeSocieteStorm INT ;
 Declare @IdSocieteFournisseur INT ;
 Declare @IdSocieteDeviseDeReferenceRequired INT ;

 DECLARE @OrganisationId INT ;
 DECLARE @CountRow INT ;

 --- Ajout des clés
 --- Societe.CodeSocieteStorm
 if exists (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%Societe.CodeSocieteStorm%')
 BEGIN
 set @IdSocieteCodeSocieteStorm = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%Societe.CodeSocieteStorm%')
 END

 ELSE 
 BEGIN
 INSERT INTO FRED_PARAM_KEY (Libelle, Description, DateCreation, DateModification, AuteurCreationId, AuteurModificationId) VALUES ('Societe.CodeSocieteStorm','la Clé pour identifier si le code société storm est affiché dans l écran des société',GETDATE(),GETDATE(),1,1);
 set @IdSocieteCodeSocieteStorm = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%Societe.CodeSocieteStorm%')
 END
 
 --- Societe.Fournisseur
 if exists (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%Societe.Fournisseur%')
 BEGIN
 set @IdSocieteFournisseur = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%Societe.Fournisseur')
 END

 ELSE 
 BEGIN
 INSERT INTO FRED_PARAM_KEY (Libelle, Description, DateCreation, DateModification, AuteurCreationId, AuteurModificationId) VALUES ('Societe.Fournisseur','la Clé pour identifier si le fournisseur est affiché dans l écran des société',GETDATE(),GETDATE(),1,1);
 set @IdSocieteFournisseur = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%Societe.Fournisseur%')
 END

 --- Societe.DeviseDeReferenceRequired
 if exists (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%Societe.DeviseDeReferenceRequired%')
 BEGIN
 set @IdSocieteDeviseDeReferenceRequired = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%Societe.DeviseDeReferenceRequired%')
 END

 ELSE 
 BEGIN
 INSERT INTO FRED_PARAM_KEY (Libelle, Description, DateCreation, DateModification, AuteurCreationId, AuteurModificationId) VALUES ('Societe.DeviseDeReferenceRequired','la Clé pour identifier si la devise de référence est obligatoire à la saisie dans l écran des société',GETDATE(),GETDATE(),1,1);
 set @IdSocieteDeviseDeReferenceRequired = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%Societe.DeviseDeReferenceRequired%')
 END

 --- Ajout des valeurs
 set @CountRow = (select count(*) from FRED_PARAM_VALUE)
 if(@countRow > 0)
 begin
 delete from FRED_PARAM_VALUE WHERE ParamKeyId IN (select ParamKeyId from FRED_PARAM_KEY WHERE ParamKeyId IN (@IdSocieteCodeSocieteStorm,@IdSocieteFournisseur,@IdSocieteDeviseDeReferenceRequired) )
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
	 --- Societe.CodeSocieteStorm
	if not exists (select ParamValueId from FRED_PARAM_VALUE where ParamKeyId = @IdSocieteCodeSocieteStorm and OrganisationId = @OrganisationId)
	begin
		INSERT INTO FRED_PARAM_VALUE (Value, DateCreation, DateModification, AuteurCreationId, AuteurModificationId, OrganisationId, ParamKeyId) VALUES ('disabled',GETDATE(),GETDATE(),1,1,@OrganisationId, @IdSocieteCodeSocieteStorm);
	end
	 --- Societe.Fournisseur
	if not exists (select ParamValueId from FRED_PARAM_VALUE where ParamKeyId = @IdSocieteFournisseur and OrganisationId = @OrganisationId)
	begin
		INSERT INTO FRED_PARAM_VALUE (Value, DateCreation, DateModification, AuteurCreationId, AuteurModificationId, OrganisationId, ParamKeyId) VALUES ('disabled',GETDATE(),GETDATE(),1,1,@OrganisationId, @IdSocieteFournisseur);   
	end
	 --- Societe.DeviseDeReferenceRequired
	if not exists (select ParamValueId from FRED_PARAM_VALUE where ParamKeyId = @IdSocieteDeviseDeReferenceRequired and OrganisationId = @OrganisationId)
	begin
		INSERT INTO FRED_PARAM_VALUE (Value, DateCreation, DateModification, AuteurCreationId, AuteurModificationId, OrganisationId, ParamKeyId) VALUES ('disabled',GETDATE(),GETDATE(),1,1,@OrganisationId, @IdSocieteDeviseDeReferenceRequired);   
	end
  FETCH NEXT FROM MY_CURSOR INTO @OrganisationId
END
CLOSE MY_CURSOR
DEALLOCATE MY_CURSOR

