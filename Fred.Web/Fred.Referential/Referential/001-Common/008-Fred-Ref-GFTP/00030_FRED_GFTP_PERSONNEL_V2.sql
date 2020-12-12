-- =======================================================================================================================================
-- Author:		Yoann Collet  20/12/2018
--
-- Description:
--      - Met à jour le personnel intérimaire de somopa mal importer
--
-- =======================================================================================================================================


BEGIN TRAN

	-- Met à jour le personnel intérimaire de somopa mal importer
	DECLARE @personnel_id int;
	DECLARE personnel_cursor CURSOR FOR
		SELECT PersonnelId FROM FRED_PERSONNEL WHERE matricule like '1439%' AND societeid in (SELECT societeid FROM FRED_SOCIETE WHERE groupeid = ( SELECT groupeid FROM FRED_GROUPE WHERE code = 'GFTP'))
	OPEN personnel_cursor;
	FETCH NEXT FROM personnel_cursor INTO @personnel_id;
	WHILE @@FETCH_STATUS = 0 BEGIN

		declare @matricule nvarchar(20) = (SELECT matricule FROM FRED_PERSONNEL WHERE PersonnelId = @personnel_id);
		declare @societe_interimaire_id int = (SELECT societeid FROM FRED_SOCIETE WHERE IsInterimaire = 1 AND GroupeId = (SELECT groupeid FROM FRED_GROUPE WHERE code = 'GFTP'));

		UPDATE FRED_PERSONNEL 
		SET Matricule = 'I_' + @matricule, IsInterimaire = 1, RessourceId = null, EtablissementPayeId = null, EtablissementRattachementId = null, SocieteId = @societe_interimaire_id
		WHERE PersonnelId = @personnel_id;

		If NOT EXISTS (SELECT * FROM FRED_MATRICULE_EXTERNE WHERE PersonnelId = @personnel_id)
		BEGIN
			INSERT INTO FRED_MATRICULE_EXTERNE (PersonnelId, Matricule, Source) VALUES (@personnel_id, @matricule, 'SAP')
		END

		FETCH NEXT FROM personnel_cursor INTO @personnel_id;
	END
	CLOSE personnel_cursor;
	DEALLOCATE personnel_cursor;

COMMIT TRAN