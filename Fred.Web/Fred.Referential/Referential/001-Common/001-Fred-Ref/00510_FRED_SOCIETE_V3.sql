-- =======================================================================================================================================
-- Author:		Yoann Collet
--
-- Description:
--      - Mise à jour des société interimaire
--
-- =======================================================================================================================================


BEGIN TRAN

-- Mise à jour de la "société intérimaire RAZEL-BEC" 
IF EXISTS(SELECT * FROM FRED_SOCIETE WHERE Code = 'IRB' AND GroupeId in (SELECT GroupeId FROM FRED_GROUPE WHERE Code ='GRZB' AND PoleId in 
		 (SELECT PoleId FROM FRED_POLE WHERE Code = 'PTP' AND HoldingId in (SELECT HoldingId FROM FRED_HOLDING WHERE code = 'FSA'))))
					
BEGIN
		Update FRED_SOCIETE set IsInterimaire = 1, Libelle = 'SOCIETE INTERIMAIRE RAZEL-BEC'  WHERE Code = 'IRB' AND GroupeId in (SELECT GroupeId FROM FRED_GROUPE WHERE Code ='GRZB' AND PoleId in 
		 (SELECT PoleId FROM FRED_POLE WHERE Code = 'PTP' AND HoldingId in (SELECT HoldingId FROM FRED_HOLDING WHERE code = 'FSA')))
END

-- Mise à jour de la "société intérimaire FAYAT TP" 
IF EXISTS(SELECT * FROM FRED_SOCIETE WHERE Code = 'I0001' AND GroupeId in (SELECT GroupeId FROM FRED_GROUPE WHERE Code ='GFTP' AND PoleId in 
		 (SELECT PoleId FROM FRED_POLE WHERE Code = 'PTP' AND HoldingId in (SELECT HoldingId FROM FRED_HOLDING WHERE code = 'FSA'))))
					
BEGIN
		Update FRED_SOCIETE set IsInterimaire = 1 WHERE Code = 'I0001' AND GroupeId in (SELECT GroupeId FROM FRED_GROUPE WHERE Code ='GFTP' AND PoleId in 
		 (SELECT PoleId FROM FRED_POLE WHERE Code = 'PTP' AND HoldingId in (SELECT HoldingId FROM FRED_HOLDING WHERE code = 'FSA')))
END

-- Mise à jour de la "société intérimaire FONDATION" 
IF EXISTS(SELECT * FROM FRED_SOCIETE WHERE Code = 'IFF' AND GroupeId in (SELECT GroupeId FROM FRED_GROUPE WHERE Code ='FON' AND PoleId in 
		 (SELECT PoleId FROM FRED_POLE WHERE Code = 'SUPPORT' AND HoldingId in (SELECT HoldingId FROM FRED_HOLDING WHERE code = 'FSA'))))
					
BEGIN
		Update FRED_SOCIETE set IsInterimaire = 1, Libelle = 'SOCIETE INTERIMAIRE FONDATION' WHERE Code = 'IFF' AND GroupeId in (SELECT GroupeId FROM FRED_GROUPE WHERE Code ='FON' AND PoleId in 
		 (SELECT PoleId FROM FRED_POLE WHERE Code = 'SUPPORT' AND HoldingId in (SELECT HoldingId FROM FRED_HOLDING WHERE code = 'FSA')))
END


COMMIT TRAN
