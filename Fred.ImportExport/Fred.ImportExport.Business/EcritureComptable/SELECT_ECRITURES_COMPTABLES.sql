﻿SELECT 
	TRIM(DTCPTA) 	As DateComptable, 		
	TRIM(DLIB)		As Libelle,	
--	SUBSTRING (DTCPTA, 1,2) As JOUR,
--	SUBSTRING (DTCPTA, 4,2) As MOIS,
--	SUBSTRING (DTCPTA, 7,4) As ANNEE,
	TRIM(RNAT)		As AnaelCodeNature,
	TRIM(MONTANT)	As Montant,
	TRIM(DCDEV) 	As Devise,
	TRIM(MONTDEV) 	As MontantDevise,	
	TRIM(RSECT)		As AnaelCodeCi,
	TRIM(DWS)		  As Dws,
	TRIM(DINT)		As Dint,
	TRIM(DNOLIG)	As Dnolig,
	TRIM(LIGANA)	As Ligana,
	TRIM(DJAL) As AnaelCodeJournal,
	TRIM(COMMSAP) As AnaelCodeCommande
FROM AXSPE.SECAFRP
WHERE RSTE='{0}'  AND  CAST(SUBSTRING (DTCPTA, 7,4) || '-' || SUBSTRING (DTCPTA, 4,2) || '-' || SUBSTRING (DTCPTA, 1,2)  As DATE) >=  CAST('{1}' AS DATE) AND  CAST(SUBSTRING (DTCPTA, 7,4) || '-' ||SUBSTRING (DTCPTA, 4,2) || '-' || SUBSTRING (DTCPTA, 1,2)  As DATE)    <=  CAST('{2}' AS DATE) 
ORDER BY DateComptable DESC