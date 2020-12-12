-- =======================================================================================================================================
-- Author:		Yoann Collet    05/07/2019
--
-- Description:
--      - MAJ de la permission 'menu.show.journalcomptable.index' pour qu'il soit affiché dans les menus
--
-- =======================================================================================================================================

UPDATE [dbo].[FRED_PERMISSION] set PermissionContextuelle = 0 where PermissionKey = 'menu.show.journalcomptable.index' ;