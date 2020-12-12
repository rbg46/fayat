-- Insert les libellés et codes des motifs de remplacement pour les intérimaires :

IF NOT EXISTS(
SELECT mr.Code
FROM	FRED_MOTIF_REMPLACEMENT mr
WHERE mr.Code = 'AA'
)
BEGIN	
INSERT INTO FRED_MOTIF_REMPLACEMENT (Code, Libelle) VALUES ('AA', 'Accroissement d''activité');
END


IF NOT EXISTS(
SELECT mr.Code
FROM	FRED_MOTIF_REMPLACEMENT mr
WHERE mr.Code = 'RS'
)
BEGIN	
INSERT INTO FRED_MOTIF_REMPLACEMENT (Code, Libelle) VALUES ('RS', 'Remplacement de salarié');
END

IF NOT EXISTS(
SELECT mr.Code
FROM	FRED_MOTIF_REMPLACEMENT mr
WHERE mr.Code = 'TU'
)
BEGIN	
INSERT INTO FRED_MOTIF_REMPLACEMENT (Code, Libelle) VALUES ('TU', 'Travaux urgents');
END

IF NOT EXISTS(
SELECT mr.Code
FROM	FRED_MOTIF_REMPLACEMENT mr
WHERE mr.Code = 'QR'
)
BEGIN	
INSERT INTO FRED_MOTIF_REMPLACEMENT (Code, Libelle) VALUES ('QR', 'Qualifications rares');
END



 --Fin d'insertion 