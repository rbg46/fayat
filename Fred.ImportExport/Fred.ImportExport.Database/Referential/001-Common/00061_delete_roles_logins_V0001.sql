-- =======================================================================================================================================
-- Author:		Naoufal BENNAI 18/02/2020
--
-- Description: ajout du flux qui permet de supprimer les rôles et le login d’un personnel après qu’il est quitté sa société
--      - 
-- =======================================================================================================================================


IF NOT EXISTS (SELECT Code FROM [importExport].[Flux] WHERE Code = 'CLEAN_OUTGOING_USERS')
BEGIN
    INSERT INTO [importExport].[Flux] ([Code], [Libelle], [Titre], [Description], [IsActif], [SocieteCode], [SocieteModeleCode], [DateDerniereExecution], [GroupCode]) 
    VALUES ('CLEAN_OUTGOING_USERS', 'Nettoyage des utilisateurs sortis', NULL, 'Permet de supprimer les rôles et le login d’un personnel après qu’il est quitté sa société', 1, NULL, NULL, NULL, 'DEFAULT')
END