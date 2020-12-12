-- Insert les libellés et codes des motifs de remplacement pour les intérimaires :

IF EXISTS(
SELECT mr.Code
FROM	FRED_MOTIF_REMPLACEMENT mr
WHERE mr.Code = 'AA'
)
BEGIN	
UPDATE FRED_MOTIF_REMPLACEMENT SET Code = 'AA', Libelle = 'Accroissement d''activité' WHERE Code = 'AA'
END


IF EXISTS(
SELECT mr.Code
FROM	FRED_MOTIF_REMPLACEMENT mr
WHERE mr.Code = 'RS'
)
BEGIN	
UPDATE FRED_MOTIF_REMPLACEMENT SET Code = 'RS', Libelle = 'Remplacement de salarié' WHERE Code = 'RS'
END

IF EXISTS(
SELECT mr.Code
FROM	FRED_MOTIF_REMPLACEMENT mr
WHERE mr.Code = 'TU'
)
BEGIN	
UPDATE FRED_MOTIF_REMPLACEMENT SET Code = 'TU', Libelle = 'Travaux urgents' WHERE Code = 'TU'
END

IF EXISTS(
SELECT mr.Code
FROM	FRED_MOTIF_REMPLACEMENT mr
WHERE mr.Code = 'QR'
)
BEGIN	
UPDATE FRED_MOTIF_REMPLACEMENT SET Code = 'QR', Libelle = 'Qualifications rares' WHERE Code = 'QR'
END



 --Fin d'insertion 