
-- --------------------------------------------------
-- Ajout d'un type de commande pour les interimaires
-- --------------------------------------------------
IF NOT EXISTS(
SELECT cd.Code
FROM	FRED_COMMANDE_TYPE cd
WHERE cd.Code = 'I'
)
BEGIN	
INSERT INTO FRED_COMMANDE_TYPE (Code, Libelle) VALUES ('I', 'Interimaire')
END
