﻿UPDATE FRED_SYSTEME_EXTERNE
SET Libelle = 'Commande STORM', Code = 'STORM_COMMANDE_RZB'
WHERE Code = 'STORM_COMMANDES'

UPDATE FRED_SYSTEME_EXTERNE
SET Code = 'STORM_FACTURATION_RZB'
WHERE Code = 'STORM_FACTURATION'