-- --------------------------------------------------
-- US 8024 : verrou des commandes lignes
-- Historisation des actions - table action status
-- --------------------------------------------------

INSERT INTO FRED_ACTION_STATUS(Name, Description) VALUES ('INITIATED','Action init');
INSERT INTO FRED_ACTION_STATUS(Name, Description) VALUES ('PENDING','Action pending');
INSERT INTO FRED_ACTION_STATUS(Name, Description) VALUES ('SUCCESS','Action success');
INSERT INTO FRED_ACTION_STATUS(Name, Description) VALUES ('WARNING','Action finished with warnings');
INSERT INTO FRED_ACTION_STATUS(Name, Description) VALUES ('SUSPENDED','Action suspended');
INSERT INTO FRED_ACTION_STATUS(Name, Description) VALUES ('FAILED','Action failed');