
-----------------------------------------------
-- Script de reprise de donnees pour les depense achats de type facture d'article non commande -> mise a jour de la tache vers la tache litige
-- Correction pour etre en phase avec la RG_3656_033
-----------------------------------------------
-- Auteur : ycourtel
-- Date :   05/03/2020
-----------------------------------------------

--Recupere la liste des taches et cis qui possede une tache litige
SELECT TacheId, CiId
INTO #newTachesCi
FROM dbo.FRED_TACHE ft
WHERE code = '999981';

--Change la tache par defaut par la tache litige du Ci
UPDATE fda SET fda.TacheId = newTaches.TacheId
FROM dbo.FRED_DEPENSE_ACHAT fda
INNER JOIN dbo.FRED_FACTURATION ff ON ff.DepenseAchatFactureEcartId = fda.DepenseId
INNER JOIN dbo.FRED_TACHE ft ON fda.TacheId = ft.TacheId
INNER JOIN #newTachesCi newTaches ON fda.CiId = newTaches.CiId
WHERE ff.FacturationTypeId = 3 
AND fda.DepenseTypeId = 4;

DROP TABLE #newTachesCi;
