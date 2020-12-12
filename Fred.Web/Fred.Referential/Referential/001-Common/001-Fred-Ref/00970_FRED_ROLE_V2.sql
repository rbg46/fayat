-- =======================================================================================================================================
-- Author:		Yoann Collet  01/10/2019
--
-- Description:
--          Mise à jour des données pour les rôles Responsable Ci et Délégué Ci pour les socité appartenant au groupe FES
--
-- =======================================================================================================================================


BEGIN TRAN

	-- Mise à jour des données pour les rôles Responsable Ci pour les socité appartenant au groupe FES
	DECLARE @societe_id_rci int;
	DECLARE societe_cursor_rci CURSOR FOR
		Select distinct r.Societeid from FRED_ROLE r
		inner join FRED_SOCIETE s on r.SocieteId = s.SocieteId
		inner join FRED_GROUPE g ON g.GroupeId = s.GroupeId
		where g.code = 'GFES' and r.CodeNomFamilier in ('RCI')
	OPEN societe_cursor_rci;
	FETCH NEXT FROM societe_cursor_rci INTO @societe_id_rci;
	WHILE @@FETCH_STATUS = 0 BEGIN

		UPDATE FRED_ROLE set Specification = 1, NiveauPaie = 2, CommandeSeuilDefaut = NULL, Description = 'Responsable CI', NiveauCompta = 1 where CodeNomFamilier = 'RCI' and SocieteId = @societe_id_rci

		FETCH NEXT FROM societe_cursor_rci INTO @societe_id_rci;
	END
	CLOSE societe_cursor_rci;
	DEALLOCATE societe_cursor_rci;

COMMIT TRAN


BEGIN TRAN

	-- Mise à jour des données pour les rôles Délégué Ci pour les socité appartenant au groupe FES
	DECLARE @societe_id_dci int;
	DECLARE societe_cursor_dci CURSOR FOR
		Select distinct r.Societeid from FRED_ROLE r
		inner join FRED_SOCIETE s on r.SocieteId = s.SocieteId
		inner join FRED_GROUPE g ON g.GroupeId = s.GroupeId
		where g.code = 'GFES' and r.CodeNomFamilier in ('DCI')
	OPEN societe_cursor_dci;
	FETCH NEXT FROM societe_cursor_dci INTO @societe_id_dci;
	WHILE @@FETCH_STATUS = 0 BEGIN

		UPDATE FRED_ROLE set Specification = 2, NiveauPaie = 1, CommandeSeuilDefaut = NULL, Description = 'Délégué CI', NiveauCompta = 1  where CodeNomFamilier = 'DCI' and SocieteId = @societe_id_dci

		FETCH NEXT FROM societe_cursor_dci INTO @societe_id_dci;
	END
	CLOSE societe_cursor_dci;
	DEALLOCATE societe_cursor_dci;

COMMIT TRAN