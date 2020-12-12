


-- Remettre la valeur du groupe id des absences à NuLL
-- A ce niveau c'est nécessaire pour les traitements 
-- Bug 5499

UPDATE A 
SET A.GroupeId = NULL
	FROM [FRED_CODE_ABSENCE] A
INNER JOIN FRED_SOCIETE soc on A.SocieteId = soc.SocieteId
INNER JOIN FRED_GROUPE gr on soc.GroupeId = gr.GroupeId
WHERE gr.Code LIKE '%GFES%'