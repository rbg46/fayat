-- Initialisation des Types Sociétés si nécéssaire
IF NOT EXISTS (SELECT * FROM FRED_TYPE_SOCIETE WHERE CODE LIKE 'INT')
BEGIN
	INSERT INTO FRED_TYPE_SOCIETE (Code, Libelle) VALUES ('INT', 'Interne')
END

IF NOT EXISTS (SELECT * FROM FRED_TYPE_SOCIETE WHERE CODE LIKE 'PAR')
BEGIN
	INSERT INTO FRED_TYPE_SOCIETE (Code, Libelle) VALUES ('PAR', 'Partenaire')
END

IF NOT EXISTS (SELECT * FROM FRED_TYPE_SOCIETE WHERE CODE like 'SEP')
BEGIN
	INSERT INTO FRED_TYPE_SOCIETE (Code, Libelle) VALUES ('SEP','SEP')
END

-- UPDATE du type societe pour toutes les sociétés existantes
-- Si OLD [TypeSocieteId]=NULL ET [Externe]=0 ET [IsInterimaire]=0 ALORS NEW [TypeSocieteId]= "1-INT"
-- Si OLD [TypeSocieteId]=NULL ET ([Externe]<>0 OU [IsInterimaire]<>0) ALORS NEW [TypeSocieteId]= "2-PAR"
-- Si OLD [TypeSocieteId]<>NULL ALORS on laisse la valeur actuelle sans l'écraser 
UPDATE FRED_SOCIETE 
SET TypeSocieteId = (SELECT TypeSocieteId FROM FRED_TYPE_SOCIETE WHERE Code = 'INT')
WHERE IsInterimaire = 0 
		AND Externe = 0 
		AND TypeSocieteId IS NULL;

UPDATE FRED_SOCIETE 
SET TypeSocieteId = (SELECT TypeSocieteId FROM FRED_TYPE_SOCIETE WHERE Code = 'PAR')
WHERE (IsInterimaire <> 0 OR Externe <> 0)
		AND TypeSocieteId IS NULL;