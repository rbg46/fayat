
------ VALORISATIONS MATÉRIEL AYANT UNE QUANTITÉ À 0

SELECT 
	v.RapportLigneId, 
	v.CiId, 
	v.MaterielId
INTO 
	#LignesMaterielZero
FROM 
	FRED_VALORISATION v
	INNER JOIN FRED_RAPPORT_LIGNE rl on rl.RapportId = v.RapportId 
		and rl.RapportLigneId = v.RapportLigneId
	INNER JOIN FRED_CI c on c.CiId = v.CiId
	INNER JOIN FRED_SOCIETE s on s.SocieteId = c.SocieteId
WHERE 
	v.Quantite = 0 
	and rl.HeureNormale > 0 
	and VerrouPeriode = 0 
	and v.MaterielId is not null 
	and s.Code IN ('RB','MBTP')

-- On vérifie que les valorisations matériel à 0 ont bien au moins une valorisation matériel > à 0
SELECT 
	v.RapportLigneId, 
	v.MaterielId, 
	v.PUHT, 
	YEAR(v.Date) as Annee, 
	MONTH(v.Date) as Mois, 
	DAY(v.Date) as Jour
INTO 
	#LignesAvecValoMaterielSuperieurZero
FROM 
	#LignesMaterielZero lm
	INNER JOIN FRED_VALORISATION v on lm.RapportLigneId = v.RapportLigneId 
		and lm.CiId = v.CiId 
		and lm.MaterielId = v.MaterielId
GROUP BY 
	v.RapportLigneId, 
	v.MaterielId, 
	v.PUHT, 
	YEAR(v.Date), 
	MONTH(v.Date), 
	DAY(v.Date) 
HAVING count(v.Date) > 1

DELETE v
FROM 
	FRED_VALORISATION v
	INNER JOIN #LignesAvecValoMaterielSuperieurZero lms on lms.RapportLigneId = v.RapportLigneId 
		and lms.MaterielId = v.MaterielId 
		and lms.PUHT = v.PUHT 
		and lms.Annee = YEAR(v.Date) 
		and lms.Mois = MONTH(v.Date) 
		and lms.Jour = DAY(v.Date) 
WHERE 
	v.Quantite = 0

DROP TABLE #LignesMaterielZero, #LignesAvecValoMaterielSuperieurZero

------ VALORISATIONS PERSONNEL AYANT UNE QUANTITÉ À 0

SELECT 
	v.RapportLigneId, 
	v.CiId, 
	v.PersonnelId
INTO 
	#LignesPersonnelZero
FROM 
	FRED_VALORISATION v
	INNER JOIN FRED_RAPPORT_LIGNE rl on rl.RapportId = v.RapportId 
		and rl.RapportLigneId = v.RapportLigneId
	INNER JOIN FRED_CI c on c.CiId = v.CiId
	INNER JOIN FRED_SOCIETE s on s.SocieteId = c.SocieteId
WHERE 
	v.Quantite = 0 
	and rl.HeureNormale > 0 
	and VerrouPeriode = 0 
	and v.PersonnelId is not null 
	and s.Code IN ('RB','MBTP')

-- On vérifie que les valorisations personnel à 0 ont bien au moins une valorisation personnel > à 0
SELECT 
	v.RapportLigneId, 
	v.PersonnelId, 
	v.PUHT, 
	YEAR(v.Date) as Annee, 
	MONTH(v.Date) as Mois, 
	DAY(v.Date) as Jour
INTO 
	#LignesAvecValoPersonnelSuperieurZero
FROM 
	#LignesPersonnelZero lp 
	INNER JOIN FRED_VALORISATION v on lp.RapportLigneId = v.RapportLigneId 
		and lp.CiId = v.CiId 
		and lp.PersonnelId = v.PersonnelId
GROUP BY 
	v.RapportLigneId, 
	v.PersonnelId, 
	v.PUHT, 
	YEAR(v.Date), 
	MONTH(v.Date), 
	DAY(v.Date) 
HAVING count(v.Date) > 1

DELETE v
FROM 
	FRED_VALORISATION v
	INNER JOIN #LignesAvecValoPersonnelSuperieurZero lps on lps.RapportLigneId = v.RapportLigneId 
		and lps.PersonnelId = v.PersonnelId 
		and lps.PUHT = v.PUHT 
		and lps.Annee = YEAR(v.Date) 
		and lps.Mois = MONTH(v.Date) 
		and lps.Jour = DAY(v.Date) 
WHERE 
	v.Quantite = 0

DROP TABLE #LignesPersonnelZero, #LignesAvecValoPersonnelSuperieurZero