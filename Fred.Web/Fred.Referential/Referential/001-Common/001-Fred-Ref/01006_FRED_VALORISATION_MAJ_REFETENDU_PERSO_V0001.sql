/* Ticket 14147 : On met à jour le ReferentielEtenduId de la valorisation en fonction du barème enregistré dans la valorisation */

SELECT 
    v.ValorisationId, 
    b.ReferentielEtenduId
INTO #ValoPersonnel
FROM 
    FRED_VALORISATION v
    INNER JOIN FRED_PERSONNEL p ON p.PersonnelId = v.PersonnelId
    INNER JOIN FRED_BAREME_EXPLOITATION_CI b ON
        b.BaremeId = v.BaremeId
        AND b.ReferentielEtenduId <> v.ReferentielEtenduId
WHERE v.VerrouPeriode = 0

UPDATE v
SET v.ReferentielEtenduId = vp.ReferentielEtenduId
FROM 
    FRED_VALORISATION v
    INNER JOIN #ValoPersonnel vp ON vp.ValorisationId = v.ValorisationId

DROP TABLE #ValoPersonnel