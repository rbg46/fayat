-- Insertions correctives suite à des scripts erronés

--AFFICHAGE DES COMPOSANTS
 Declare @ShowenInputsId INT ;
 DECLARE @HiddenInputsId INT ;
 DECLARE @OrganisationId INT ;
 DECLARE @CountRow INT ;

 set @ShowenInputsId = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%ShowenInputs%')
 set @HiddenInputsId = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%HiddenInputs%')

 DECLARE COLUMN_CURSOR CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
 FOR 
  SELECT DISTINCT org.OrganisationId FROM FRED_ORGANISATION org , FRED_GROUPE grp 
  WHERE  ( org.OrganisationId = grp.OrganisationId OR grp.OrganisationId=org.pereid) and grp.Code = 'GFES'

OPEN COLUMN_CURSOR
FETCH NEXT FROM COLUMN_CURSOR INTO @OrganisationId
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
		INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('ivd-detail','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId,@HiddenInputsId)
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

    --DECLARE @cnt INT = 1;
    --WHILE @cnt <= 26
    --BEGIN
    --    IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='colonne_Materiel'+CAST(@cnt AS VARCHAR(5)))
	   -- BEGIN
    --        INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('colonne_Materiel'+CAST(@cnt AS VARCHAR(5)),'2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId, @HiddenInputsId);
    --        SET @cnt = @cnt + 1;
    --    END
    --END

  FETCH NEXT FROM COLUMN_CURSOR INTO @OrganisationId
END
CLOSE COLUMN_CURSOR
DEALLOCATE COLUMN_CURSOR


-- COLONNES MATERIEL
DECLARE COLUMNMATERIEL_CURSOR CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
 FOR 
  SELECT DISTINCT org.OrganisationId FROM FRED_ORGANISATION org , FRED_GROUPE grp 
  WHERE  ( org.OrganisationId = grp.OrganisationId OR grp.OrganisationId=org.pereid) and grp.Code = 'GFES'

OPEN COLUMNMATERIEL_CURSOR
FETCH NEXT FROM COLUMNMATERIEL_CURSOR INTO @OrganisationId
WHILE @@FETCH_STATUS = 0
BEGIN
    IF NOT EXISTS (select pv.ParamValueId from FRED_PARAM_VALUE pv where pv.OrganisationId=@OrganisationId and pv.Value='colonne_Materiel1')
	BEGIN
        DECLARE @cnt INT = 1;
        WHILE @cnt <= 26
        BEGIN
            INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('colonne_Materiel'+CAST(@cnt AS VARCHAR(5)),'2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId, @HiddenInputsId);
            SET @cnt = @cnt + 1;
        END
    END

    FETCH NEXT FROM COLUMNMATERIEL_CURSOR INTO @OrganisationId
END
CLOSE COLUMNMATERIEL_CURSOR
DEALLOCATE COLUMNMATERIEL_CURSOR

--NOMBRE D'HEURES ET DE JOURS TRAVAILLES
Declare @IdHeuresSemaineOuvrierETAM INT;
Declare @IdHeuresJourOuvrierETAM INT;
Declare @IdHeuresSemaineIAC INT;
Declare @IdHeuresJourIAC INT;

set @IdHeuresSemaineIAC = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%HeuresSemaineIAC%')
set @IdHeuresSemaineOuvrierETAM = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%HeuresSemaineOuvrierETAM%')
set @IdHeuresJourIAC = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%HeuresJourIAC%')
set @IdHeuresJourOuvrierETAM = (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%HeuresJourOuvrierETAM%')

DECLARE WORKDAYS_CURSOR CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
 FOR 
   SELECT DISTINCT org.OrganisationId FROM FRED_ORGANISATION org , FRED_GROUPE grp 
  WHERE (org.OrganisationId = grp.OrganisationId OR grp.OrganisationId=org.pereid) and grp.Code = 'GFES'

OPEN WORKDAYS_CURSOR
FETCH NEXT FROM WORKDAYS_CURSOR INTO @OrganisationId
    WHILE @@FETCH_STATUS = 0
    BEGIN 
        if not exists (select ParamValueId from FRED_PARAM_VALUE where Value = '35' and OrganisationId = @OrganisationId)
        begin
            INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('35','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId, @IdHeuresSemaineOuvrierETAM);
        end
        if not exists (select ParamValueId from FRED_PARAM_VALUE where Value = '35' and OrganisationId = @OrganisationId)
        begin
            INSERT INTO FRED_PARAM_VALUE ([Value], [DateCreation], [DateModification], [AuteurCreationId], [AuteurModificationId], [OrganisationId], [ParamKeyId]) VALUES ('7','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1,@OrganisationId, @IdHeuresJourIAC);   
        end

        FETCH NEXT FROM WORKDAYS_CURSOR INTO @OrganisationId
    END
CLOSE WORKDAYS_CURSOR
DEALLOCATE WORKDAYS_CURSOR
