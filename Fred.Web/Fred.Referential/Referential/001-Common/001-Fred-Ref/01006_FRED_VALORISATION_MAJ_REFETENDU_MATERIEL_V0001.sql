/* Ticket 14147 : On met à jour le ReferentielEtenduId de la valorisation en fonction du ReferentielEtenduId de la ressource matériel de la valorisation */

SELECT
    v.ValorisationId,
    v.MaterielId
INTO #ValoAvecNouvelleRessourceMateriel
FROM 
    FRED_VALORISATION v
    INNER JOIN FRED_MATERIEL m ON m.MaterielId = v.MaterielId
    INNER JOIN FRED_SOCIETE_RESSOURCE_NATURE srn ON 
        srn.SocieteId = m.SocieteId 
        AND srn.RessourceId = m.RessourceId 
        AND srn.ReferentielEtenduId <> v.ReferentielEtenduId
WHERE v.VerrouPeriode = 0

UPDATE v
SET v.ReferentielEtenduId = srn.ReferentielEtenduId
FROM
    FRED_VALORISATION v
    INNER JOIN #ValoAvecNouvelleRessourceMateriel va ON va.ValorisationId = v.ValorisationId
    INNER JOIN FRED_MATERIEL m ON m.MaterielId = va.MaterielId
    INNER JOIN FRED_SOCIETE_RESSOURCE_NATURE srn ON 
        srn.SocieteId = m.SocieteId 
        AND srn.RessourceId = m.RessourceId 

DROP TABLE #ValoAvecNouvelleRessourceMateriel