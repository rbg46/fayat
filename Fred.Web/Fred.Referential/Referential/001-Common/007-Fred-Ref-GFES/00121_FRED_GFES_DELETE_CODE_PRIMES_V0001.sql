SELECT p.PrimeId INTO #tmpPrime
FROM FRED_PRIME p inner join FRED_SOCIETE s on p.SocieteId=s.SocieteId
	inner join FRED_GROUPE g on s.GroupeId=g.GroupeId
WHERE p.Code in ('IPD1', 'IPD2', 'IPD3', 'IPD4', 'IPD5', 'IPD6') and g.Code='GFES'

DELETE FROM FRED_CI_PRIME
WHERE PrimeId in (select * from #tmpPrime)

DELETE FROM FRED_POINTAGE_ANTICIPE_PRIME
WHERE PrimeId in (select * from #tmpPrime)

DELETE FROM FRED_RAPPORT_LIGNE_PRIME
WHERE PrimeId in (select * from #tmpPrime)

DELETE FROM FRED_RAPPORTPRIME_LIGNE_PRIME
WHERE PrimeId in (select * from #tmpPrime)

DELETE FROM FRED_PRIME
WHERE PrimeId in (select * from #tmpPrime)

DROP TABLE #tmpPrime