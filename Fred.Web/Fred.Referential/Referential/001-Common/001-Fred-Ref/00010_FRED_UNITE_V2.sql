-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

--UPDATE FRED_UNITE SET Code = 'KWH', Libelle = 'Kilowatt-heure' where UniteId = 2;
--UPDATE FRED_UNITE SET Code = 'LPH', Libelle = 'Litres par heure' where UniteId = 3;
--UPDATE FRED_UNITE SET Code = 'H', Libelle = 'Heure' where UniteId = 4;
--UPDATE FRED_UNITE SET Code = 'JR', Libelle = 'Jours' where UniteId = 5;

--SET IDENTITY_INSERT FRED_UNITE ON;
---- Unités issues de STORM SAP
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('6','%','Pourcentage')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('7','BT.','Bouteille')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('8','FUT','Fut')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('9','HA','Hectare')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('10','KG','Kilogramme')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('11','KM','Kilomètre')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('12','KM2','Kilomètre carré')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('13','KVA','Kilovolt ampère')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('14','L','Litre')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('15','LHK','Litres par 100 km')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('16','M','Mètre')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('17','M2','Mètre carré')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('18','M3','Mètre cube')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('19','M3H','Mètre cube/heure')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('20','M3J','Mètres cube/jour')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('21','MHR','Mètre/heure')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('22','MIN','Minute')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('23','MOS','Mois')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('24','PAL','Palette')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('25','PAQ','Paquet')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('26','PC','Pièce')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('27','PRS','Nombre de personnes')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('28','SAC','Sac')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('29','SM','Semaines')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('30','TO','Tonne')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('31','TOM','Tonne/mètre cube')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('32','UO','Unité d''oeuvre')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('33','VA','Voltampère')
--INSERT INTO FRED_UNITE (UniteId,Code,Libelle) VALUES ('34','FT','Forfait')

--SET IDENTITY_INSERT FRED_UNITE OFF;



