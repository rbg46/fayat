-- =======================================================================================================================================
-- Author:		BENNAI Naoufal  27/11/2019
--
-- Description:
--          RG_3465_011 : Mise à jour de la colonne SystemeInterimaire du Groupe GRZB avec la valeur « PIXID » 
--
-- =======================================================================================================================================

UPDATE [FRED_GROUPE] SET [SystemeInterimaire] = 'PIXID' WHERE [Code] = 'GRZB'