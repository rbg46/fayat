
---------------------------------------------------------------------------------------------------------------------
-- Script de reprise de donnees de commandes avenant avec des donnees erronnees
---------------------------------------------------------------------------------------------------------------------
----- Pour les lignes d'avenants dont les donnees ne seraient pas bien renseignees...
----- On enleve la date de validation pour permettre a l'utilisateur de valider une seconde fois dans Fred Web.
----- Par la suite, il faudra relancer les jobs Hangfire qui ne sont pas passes.
---------------------------------------------------------------------------------------------------------------------

UPDATE fca
	SET fca.DateValidation = NULL
FROM 
	dbo.FRED_COMMANDE_AVENANT fca 
INNER JOIN 
	dbo.FRED_COMMANDE_LIGNE_AVENANT fcla ON fcla.AvenantId = fca.CommandeAvenantId
INNER JOIN 
	dbo.FRED_COMMANDE_LIGNE fcl ON fcla.CommandeLigneAvenantId = fcl.AvenantLigneId
INNER JOIN 
	dbo.FRED_COMMANDE fc ON fcl.CommandeId = fc.CommandeId
WHERE 
	fcl.UniteId IS NULL 
	OR fcl.RessourceId IS NULL 
	OR Quantite < 0
	OR PUHT < 0
	OR Quantite * PUHT < 0.01
	OR (fcl.TacheId IS NULL AND fc.IsAbonnement = 1)
