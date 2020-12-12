-- Ajout de la société Moulin BTP (code 550) du groupe RZB
-- dans l'import des CI
-- dans l'import des Personnels


--WI 6165
UPDATE [importExport].[Flux] SET [SocieteCode] =  (N'GEO,800,LHE,620,320,MTP') WHERE Code = 'PERSONNEL_GRZB'
