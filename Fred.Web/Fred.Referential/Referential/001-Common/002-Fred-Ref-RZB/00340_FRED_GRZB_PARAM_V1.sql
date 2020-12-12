-- =======================================================================================================================================
-- Author:		Yoann Collet  18/02/2019
--
-- Description:
--      - Ajout de paramètrage par société pour les sociétés du groupe Razel-Bec l'affichage de la case à cocher matériel à pointer dans les commandes
--
-- =======================================================================================================================================


if not exists (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%ShowenInputs%')
BEGIN
INSERT INTO FRED_PARAM_KEY ([Libelle],[Description],[DateCreation],[DateModification],[AuteurCreationId],[AuteurModificationId])
	VALUES ('ShowenInputs','la Clé pour identifier les composant à afficher pour une organisation donné','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1);
END

if not exists (select ParamKeyId from FRED_PARAM_KEY where Libelle like '%HiddenInputs%')
BEGIN
 INSERT INTO FRED_PARAM_KEY ([Libelle],[Description],[DateCreation],[DateModification],[AuteurCreationId],[AuteurModificationId])
 	VALUES ('HiddenInputs','la Clé pour identifier les composant à masquer pour une organisation donné','2018-09-27 00:00:00.000','2018-09-27 00:00:00.000',1,1);
END


BEGIN TRAN

	DECLARE @organisation_id int;
    DECLARE @param_key_id int = (select ParamKeyId from FRED_PARAM_KEY where Libelle = 'ShowenInputs');
	DECLARE organisation_cursor CURSOR FOR
		select OrganisationId from FRED_SOCIETE where GroupeId = (select groupeid from FRED_GROUPE where code = 'GRZB')
	OPEN organisation_cursor;
	FETCH NEXT FROM organisation_cursor INTO @organisation_id;
	WHILE @@FETCH_STATUS = 0 BEGIN

          INSERT INTO FRED_PARAM_VALUE (Value, DateCreation, DateModification, AuteurCreationId, AuteurModificationId, OrganisationId, ParamKeyId) VALUES ('materiel-a-pointer-commande-location',GETDATE(),null,1,null,@organisation_id, @param_key_id);   

		FETCH NEXT FROM organisation_cursor INTO @organisation_id;
	END
	CLOSE organisation_cursor;
	DEALLOCATE organisation_cursor;

COMMIT TRAN