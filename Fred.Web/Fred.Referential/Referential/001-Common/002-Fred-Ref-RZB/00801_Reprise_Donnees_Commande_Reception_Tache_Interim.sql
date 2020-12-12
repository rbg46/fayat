
---------------------------------------------------------------------
-- Script de reprise de donnees pour les receptions 
--    -> lie les bonnes taches interimaires du ci du rapport
---------------------------------------------------------------------
-- Auteur : ycourtel
-- Date :   06/03/2020
---------------------------------------------------------------------

--Recupere la liste des taches et cis qui possede une tache interim
SELECT TacheId, CiId
INTO #newTachesCi
FROM dbo.FRED_TACHE ft
WHERE ft.TacheType = 8
AND ft.Niveau = 3
AND Active = 1;

--Change la tache par defaut par la tache litige du Ci
UPDATE fcl SET fcl.TacheId = newTaches.TacheId
FROM dbo.FRED_COMMANDE fc
INNER JOIN FRED_COMMANDE_LIGNE fcl ON fcl.CommandeId = fc.CommandeId
--Chercher les recetions liees
INNER JOIN dbo.FRED_DEPENSE_ACHAT fda ON fcl.CommandeLigneId = fda.CommandeLigneId AND fda.DepenseTypeId = 1
INNER JOIN [dbo].[FRED_COMMANDE_CONTRAT_INTERIMAIRE] fcci ON fc.CommandeId = fcci.CommandeId
INNER JOIN dbo.FRED_CI fc2 ON fcci.CiId = fc2.CiId
INNER JOIN dbo.FRED_SOCIETE fs ON fc2.SocieteId = fs.SocieteId
INNER JOIN dbo.FRED_TYPE_SOCIETE fts ON fs.TypeSocieteId = fts.TypeSocieteId
INNER JOIN dbo.FRED_RAPPORT_LIGNE frl ON fcci.RapportLigneId = frl.RapportLigneId
INNER JOIN #newTachesCi newTaches ON frl.CiId = newTaches.CiId
WHERE fc.Libelle LIKE 'Commande Automatique - %' 
AND (fda.TacheId <> newTaches.TacheId)
AND fts.Code <> 'SEP'

DROP TABLE #newTachesCi;
