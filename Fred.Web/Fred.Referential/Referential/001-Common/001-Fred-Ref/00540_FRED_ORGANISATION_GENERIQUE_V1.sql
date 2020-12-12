-- =======================================================================================================================================
-- Author:		Yoann Collet 18/12/2018
--
-- Description:
--      - Mise à jour des code et libelle organisation générique 
--
-- =======================================================================================================================================


BEGIN TRAN

	-- Mise à jour des code et libelle organisation générique afin qu'il n'y est plus d'espace inutile
	DECLARE @organisation_generique_id int;
	DECLARE organisation_generique_cursor CURSOR FOR
		SELECT OrganisationGeneriqueId from FRED_ORGANISATION_GENERIQUE
	OPEN organisation_generique_cursor;
	FETCH NEXT FROM organisation_generique_cursor INTO @organisation_generique_id;
	WHILE @@FETCH_STATUS = 0 BEGIN


		DECLARE @code_sans_espace nvarchar(150) = ( SELECT TRIM(code) from FRED_ORGANISATION_GENERIQUE WHERE  OrganisationGeneriqueId = @organisation_generique_id ) ;
		DECLARE @libelle_sans_espace nvarchar(150) = ( SELECT TRIM(libelle) from FRED_ORGANISATION_GENERIQUE WHERE  OrganisationGeneriqueId = @organisation_generique_id ) ;

		UPDATE FRED_ORGANISATION_GENERIQUE SET Code = @code_sans_espace, Libelle = @libelle_sans_espace where OrganisationGeneriqueId = @organisation_generique_id;

		FETCH NEXT FROM organisation_generique_cursor INTO @organisation_generique_id;
	END
	CLOSE organisation_generique_cursor;
	DEALLOCATE organisation_generique_cursor;

COMMIT TRAN

