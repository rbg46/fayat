-- =======================================================================================================================================
-- Author:		Yoann Collet  10/04/2019
--
-- Description:
--      - Correction d'orthographe des permissions
--
-- =======================================================================================================================================



IF EXISTS ( SELECT 1 FROM FRED_PERMISSION WHERE Code = '0129')
BEGIN
    UPDATE FRED_PERMISSION SET Libelle = 'Affichage du bouton de duplication sur la page ''Liste des pointages personnels''.' where Code = '0129'
END

IF EXISTS ( SELECT 1 FROM FRED_PERMISSION WHERE Code = '0130')
BEGIN
    UPDATE FRED_PERMISSION SET Libelle = 'Affichage du bouton de duplication sur la page ''Rapport journalier''.' where Code = '0130'
END