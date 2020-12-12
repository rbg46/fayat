-- Insertion des états pour un avancement


IF NOT EXISTS(
SELECT ae.Code
FROM	FRED_AVANCEMENT_ETAT ae
WHERE ae.Code = 'BR'
)
BEGIN	
INSERT INTO FRED_AVANCEMENT_ETAT (Code, Libelle) VALUES ('ER', 'Enregistré');
END

IF NOT EXISTS(
SELECT ae.Code
FROM	FRED_AVANCEMENT_ETAT ae
WHERE ae.Code = 'BR'
)
BEGIN	
INSERT INTO FRED_AVANCEMENT_ETAT (Code, Libelle) VALUES ('AV', 'A valider');
END

IF NOT EXISTS(
SELECT ae.Code
FROM	FRED_AVANCEMENT_ETAT ae
WHERE ae.Code = 'BR'
)
BEGIN	
INSERT INTO FRED_AVANCEMENT_ETAT (Code, Libelle) VALUES ('VA', 'Validé');
END





 
 --Fin d'insertion 