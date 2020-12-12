-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

--SET IDENTITY_INSERT FRED_POLE ON;
--INSERT INTO FRED_POLE (PoleId,OrganisationId,HoldingId,Code,Libelle) VALUES ('1','3','1','PTP','POLE TP'); 
--INSERT INTO FRED_POLE (PoleId,OrganisationId,HoldingId,Code,Libelle) VALUES ('2','4','1','SUPPORT','FCI, FCAI, ...'); 
--SET IDENTITY_INSERT FRED_POLE OFF;




-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- S30 - Pole
-- --------------------------------------------------
SET IDENTITY_INSERT FRED_POLE ON;
INSERT FRED_POLE(PoleId,Code,Libelle,HoldingId,OrganisationId) VALUES(1,'PTP','POLE TP',1,3);
INSERT FRED_POLE(PoleId,Code,Libelle,HoldingId,OrganisationId) VALUES(2,'SUPPORT','FIT, FCAI, …',1,4);
SET IDENTITY_INSERT FRED_POLE OFF;
