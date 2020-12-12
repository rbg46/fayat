﻿ DECLARE @SocieteIdRzb int, @SocieteIdMbtp int

 SELECT @SocieteIdRzb = SOCIETEID FROM FRED_SOCIETE WHERE CODE  = 'RB'
 SELECT @SocieteIdMbtp  = SOCIETEID FROM FRED_SOCIETE WHERE CODE  = 'MBTP'
  
 UPDATE V SET PUHT = BAREME.Prix, Montant = Quantite * BAREME.Prix
 FROM FRED_VALORISATION V 
 INNER JOIN FRED_CI C ON C.CiId  = V.CiId
 INNER JOIN FRED_BAREME_EXPLOITATION_CI BAREME on BAREME.BaremeId  = V.BaremeId AND BAREME.ReferentielEtenduId = V.ReferentielEtenduId
 WHERE C.SocieteId IN (@SocieteIdRzb, @SocieteIdMbtp)
 AND PUHT = 0