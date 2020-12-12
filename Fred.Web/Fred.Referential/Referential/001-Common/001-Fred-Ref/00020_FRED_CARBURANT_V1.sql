-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

--SET IDENTITY_INSERT FRED_CARBURANT ON;
--INSERT INTO FRED_CARBURANT (CarburantId,UniteId,Code,Libelle) VALUES ('1','2','EL','Electricité'); 
--INSERT INTO FRED_CARBURANT (CarburantId,UniteId,Code,Libelle) VALUES ('2','3','ES','Essence'); 
--INSERT INTO FRED_CARBURANT (CarburantId,UniteId,Code,Libelle) VALUES ('3','3','FU','Fuel'); 
--INSERT INTO FRED_CARBURANT (CarburantId,UniteId,Code,Libelle) VALUES ('4','3','GN','GNR'); 
--INSERT INTO FRED_CARBURANT (CarburantId,UniteId,Code,Libelle) VALUES ('5','3','GO','Gasoil'); 
--INSERT INTO FRED_CARBURANT (CarburantId,UniteId,Code,Libelle) VALUES ('6','1','SC','Sans Carburant'); 
--INSERT INTO FRED_CARBURANT (CarburantId,UniteId,Code,Libelle) VALUES ('7','1','AC','Autre Carburant'); 
--SET IDENTITY_INSERT FRED_CARBURANT OFF;



-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- S1 - Carburants
-- --------------------------------------------------
	SET IDENTITY_INSERT FRED_CARBURANT ON;

INSERT INTO FRED_CARBURANT (CarburantId, UniteId, Code, Libelle) VALUES(1,2,'EL','Electricité');	
INSERT INTO FRED_CARBURANT (CarburantId, UniteId, Code, Libelle) VALUES(2,3,'ES','Essence');	
INSERT INTO FRED_CARBURANT (CarburantId, UniteId, Code, Libelle) VALUES(3,3,'FU','Fuel');	
INSERT INTO FRED_CARBURANT (CarburantId, UniteId, Code, Libelle) VALUES(4,3,'GN','GNR');	
INSERT INTO FRED_CARBURANT (CarburantId, UniteId, Code, Libelle) VALUES(5,3,'GO','Gasoil');	
INSERT INTO FRED_CARBURANT (CarburantId, UniteId, Code, Libelle) VALUES(6,1,'SC','Sans Carburant');	
INSERT INTO FRED_CARBURANT (CarburantId, UniteId, Code, Libelle) VALUES(7,1,'AC','Autre Carburant');	
	SET IDENTITY_INSERT FRED_CARBURANT OFF;
