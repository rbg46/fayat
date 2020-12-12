-- =======================================================================================================================================
-- Author:		Yoann Collet    05/07/2019
--
-- Description:
--      - Comme SearchListeRapportEnt n'existe plus on ne peux plus désérialiser ses favoris
--      - C'est pourquoi je fais un nettoyage dans la base de données
--
-- =======================================================================================================================================

DELETE FRED_FAVORI_UTILISATEUR where typeFavori = 'ListeRapport' ;


