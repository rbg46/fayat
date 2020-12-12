SELECT r.RessourceId INTO #tmpRessource
FROM FRED_RESSOURCE r inner join FRED_SOUS_CHAPITRE sc on r.SousChapitreId = sc.SousChapitreId
	inner join FRED_CHAPITRE c on sc.ChapitreId = c.ChapitreId
	inner join FRED_GROUPE g on c.GroupeId=g.GroupeId
WHERE g.Code='GFES'

SELECT MaterielId INTO #tmpMateriel 
FROM FRED_MATERIEL
WHERE RessourceId in (select * from #tmpRessource)

DELETE FROM FRED_AFFECTATION_MOYEN WHERE MaterielId in (select * from #tmpMateriel)

DELETE FROM FRED_BAREME_EXPLOITATION_CI_SURCHARGE WHERE MaterielId in (select * from #tmpMateriel)

UPDATE FRED_PERSONNEL set MaterielId=null WHERE MaterielId in (select * from #tmpMateriel)

UPDATE FRED_RAPPORT_LIGNE set MaterielId=null WHERE MaterielId in (select * from #tmpMateriel)

DELETE FROM FRED_MATERIEL WHERE MaterielId in (select * from #tmpMateriel)

-- Suppression des valorisations associées à FES
DELETE FROM FRED_VALORISATION
WHERE ValorisationId in (
	select v.ValorisationId
	from FRED_VALORISATION v inner join FRED_BAREME_EXPLOITATION_CI b on v.BaremeId=b.BaremeId
		inner join FRED_SOCIETE_RESSOURCE_NATURE srn on b.ReferentielEtenduId=srn.ReferentielEtenduId
		inner join #tmpRessource r on srn.RessourceId=r.RessourceId)

-- Suppression des barêmes d'exploitation associés à FES
DELETE FROM FRED_BAREME_EXPLOITATION_CI
WHERE ReferentielEtenduId in (
	select srn.ReferentielEtenduId
	from FRED_SOCIETE_RESSOURCE_NATURE srn inner join #tmpRessource r on srn.RessourceId=r.RessourceId)

-- Suppression des associations
DELETE FROM FRED_SOCIETE_RESSOURCE_NATURE
WHERE RessourceId in (select * from #tmpRessource)

-- On désalloue les ressources aux personnels
UPDATE FRED_PERSONNEL set RessourceId=null
WHERE RessourceId in (select * from #tmpRessource)

-- Suppression des natures analytiques
DELETE FROM FRED_NATURE
WHERE NatureId in (
	select n.NatureId
	from FRED_NATURE n inner join FRED_SOCIETE s on n.SocieteId=s.SocieteId
		inner join FRED_GROUPE g on s.GroupeId=g.GroupeId
	where g.Code='GFES')

-- Suppression des ressources
DELETE FROM FRED_RESSOURCE
WHERE RessourceId in (select * from #tmpRessource)

-- Suppression des sous-chapitres
DELETE FROM FRED_SOUS_CHAPITRE
WHERE SousChapitreId in (
	select sc.SousChapitreId
	from FRED_SOUS_CHAPITRE sc inner join FRED_CHAPITRE c on sc.ChapitreId = c.ChapitreId
		inner join FRED_GROUPE g on c.GroupeId=g.GroupeId
	where g.Code='GFES')

-- Suppression des chapitres
DELETE FROM FRED_CHAPITRE
WHERE ChapitreId in (
	select c.ChapitreId
	from FRED_CHAPITRE c inner join FRED_GROUPE g on c.GroupeId=g.GroupeId
	where g.Code='GFES')

DROP TABLE #tmpRessource
DROP TABLE #tmpMateriel