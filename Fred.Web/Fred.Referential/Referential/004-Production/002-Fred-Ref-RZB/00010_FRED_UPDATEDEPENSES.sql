-----------------------------------------------------------------------------------------------------------------------
---- Script de reprise de donnees erronnées. FRED_VALORISATION et FRED_DEPENSE_ACHAT
-----------------------------------------------------------------------------------------------------------------------


----Aout
--UPDATE FRED_VALORISATION
--SET Quantite = 0, Montant = 0, GroupeRemplacementTacheId = NULL
--WHERE valorisationid= '102915'

----Septembre
--UPDATE FRED_VALORISATION
--SET Quantite = 0, Montant = 0, GroupeRemplacementTacheId = NULL
--WHERE valorisationid IN ('122722', '122723','122724');

--UPDATE FRED_DEPENSE_ACHAT
--SET DateSuppression = '2020-02-20'
--WHERE DepenseId IN ('62499','62500','62501');


----Novembre
--UPDATE FRED_VALORISATION
--SET PUHT = 38, Montant = 38*Quantite
--  WHERE ciId = '2742' and  PersonnelId='15515'
--  AND quantite >0
--  AND [date] > '2019-11-01' and [date] < '2019-12-01'

--UPDATE FRED_VALORISATION
--SET PUHT = 35, Montant = 35*Quantite
--  WHERE ciId = '2742' and  PersonnelId='15519'
--  AND quantite >0
--  AND [date] > '2019-11-01' and [date] < '2019-12-01'

--UPDATE FRED_VALORISATION
--SET PUHT = 35, Montant = 35*Quantite
--  WHERE ciId = '2742' and  PersonnelId='15517'
--  AND quantite >0
--  AND [date] > '2019-11-01' and [date] < '2019-12-01'

--UPDATE FRED_VALORISATION
--SET PUHT = 35, Montant = 35*Quantite
--  WHERE ciId = '2742' and  PersonnelId='15516'
--  AND quantite >0
--  AND [date] > '2019-11-01' and [date] < '2019-12-01'