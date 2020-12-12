-- --------------------------------------------------
-- FRED 2018 - R4 - OCTOBRE 2018 
-- INJECTION DES DONNES POUR FRED - GROUPE FAYAT TP
-- --------------------------------------------------

-- AJOUT EURO COMME DEVISE DE REFERENCE POUR SOMOPA
DECLARE @SocieteID INT = (SELECT SocieteId FROM FRED_SOCIETE WHERE code = '0143' AND GroupeId= (SELECT GroupeId FROM FRED_GROUPE where Code = 'GFTP'));
DECLARE @DeviseEuro INT =(SELECT DeviseId FROM FRED_DEVISE WHERE IsoCode = 'EUR' AND DeviseId = 48);
DECLARE @CheckDeviseReference INT = (SELECT COUNT(SocieteDeviseId) FROM FRED_SOCIETE_DEVISE Where SocieteID = @SocieteID AND DeviseId = @DeviseEuro);
PRINT @SocieteID;
PRINT @DeviseEuro;
PRINT @CheckDeviseReference;

IF(@CheckDeviseReference=0)
BEGIN
	INSERT INTO FRED_SOCIETE_DEVISE(SocieteId, DeviseId, DeviseDeReference) VALUES (@SocieteID, @DeviseEuro, 1 )
END



-- AJOUT EURO COMME DEVISE DE REFERENCE POUR SOMOPA
SET @SocieteID  = (SELECT SocieteId FROM FRED_SOCIETE WHERE code = '0001' AND GroupeId= (SELECT GroupeId FROM FRED_GROUPE where Code = 'GFTP'));
SET @DeviseEuro =(SELECT DeviseId FROM FRED_DEVISE WHERE IsoCode = 'EUR' AND DeviseId = 48);
SET @CheckDeviseReference  = (SELECT COUNT(SocieteDeviseId) FROM FRED_SOCIETE_DEVISE Where SocieteID = @SocieteID AND DeviseId = @DeviseEuro);
PRINT @SocieteID;
PRINT @DeviseEuro;
PRINT @CheckDeviseReference;

IF(@CheckDeviseReference=0)
BEGIN
	INSERT INTO FRED_SOCIETE_DEVISE(SocieteId, DeviseId, DeviseDeReference) VALUES (@SocieteID, @DeviseEuro, 1 )
END



-- CONSOLIDATION SUR iSAgenceRattachement
UPDATE FRED_ETABLISSEMENT_PAIE SET IsAgenceRattachement = 1 WHERE code ='EP0143'  AND SocieteId = (SELECT SocieteId FROM FRED_SOCIETE WHERE code = '0143' AND GroupeId= (SELECT GroupeId FROM FRED_GROUPE where Code = 'GFTP'))
UPDATE FRED_ETABLISSEMENT_PAIE SET IsAgenceRattachement = 1 WHERE code ='EP0001'  AND SocieteId = (SELECT SocieteId FROM FRED_SOCIETE WHERE code = '0001' AND GroupeId= (SELECT GroupeId FROM FRED_GROUPE where Code = 'GFTP'))