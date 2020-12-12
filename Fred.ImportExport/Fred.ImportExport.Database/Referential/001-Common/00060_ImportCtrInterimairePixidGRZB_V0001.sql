-- =======================================================================================================================================
-- Author:		BENNAI Naoufal 16/10/2019
--
-- Description:
--      - RG_3465_017 : Ajout du flux CTR_RZB pour l’import des contrats intérimaires PIXID  
-- =======================================================================================================================================


IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'CTR_RZB')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution]) 
    VALUES ('CTR_RZB', 'Intérimaires Pixid GRZB', NULL, 'Import des contrats Intérimaires depuis Pixid pour le Groupe Razel-Bec', 1, 'GRZB', NULL, NULL)
END