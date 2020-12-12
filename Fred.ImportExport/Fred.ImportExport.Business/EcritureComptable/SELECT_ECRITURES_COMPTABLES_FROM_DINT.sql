SELECT 
	TRIM(DTCPTA) 	As DateComptable, 		
	TRIM(DLIB)		As Libelle,	
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
WHERE RSTE='{0}'  AND  DINT >=  '{1}'
ORDER BY DateComptable DESC