-- Insertion des états pour un budget

IF NOT EXISTS(
SELECT budget.Code
FROM	FRED_BUDGET_ETAT budget
WHERE budget.Code = 'BR'
)
BEGIN	
INSERT INTO FRED_BUDGET_ETAT (Code, Libelle) VALUES ('BR', 'Brouillon');
END

IF NOT EXISTS(
SELECT budget.Code
FROM	FRED_BUDGET_ETAT budget
WHERE budget.Code = 'AV'
)
BEGIN	
INSERT INTO FRED_BUDGET_ETAT (Code, Libelle) VALUES ('AV', 'A valider');
END

IF NOT EXISTS(
SELECT budget.Code
FROM	FRED_BUDGET_ETAT budget
WHERE budget.Code = 'EA'
)
BEGIN	
INSERT INTO FRED_BUDGET_ETAT (Code, Libelle) VALUES ('EA', 'En application');
END

IF NOT EXISTS(
SELECT budget.Code
FROM	FRED_BUDGET_ETAT budget
WHERE budget.Code = 'AR'
)
BEGIN	
INSERT INTO FRED_BUDGET_ETAT (Code, Libelle) VALUES ('AR', 'Archivé');
END





 
 --Fin d'insertion 