-- =======================================================================================================================================
-- Author:		Yoann Collet  30/01/2019
--
-- Description:
--      - Mise à Zéro des Favoris pour mettre en place le refonctionnement des gestions de favoris 
--
-- =======================================================================================================================================


IF NOT EXISTS ( SELECT * FROM FRED_FAVORI_UTILISATEUR)
BEGIN
    DELETE FROM FRED_FAVORI_UTILISATEUR
END
