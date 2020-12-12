-- --------------------------------------------------
-- US 8024 : verrou des commandes lignes
-- Historisation des actions - table action type
-- --------------------------------------------------

INSERT INTO FRED_ACTION_TYPE(Code, Libelle) VALUES ('VERROUILLAGE','Verrouillage d’une ligne de commande');
INSERT INTO FRED_ACTION_TYPE(Code, Libelle) VALUES ('DEVERROUILLAGE','Déverrouillage d’une ligne de commande');