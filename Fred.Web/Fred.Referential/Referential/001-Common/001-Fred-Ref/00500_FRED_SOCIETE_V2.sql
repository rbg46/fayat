-- =======================================================================================================================================
-- Author:		Yoann Collet
--
-- Description:
--      - Supprime les sociétés "société intérimaire SOMOPA" et " société intérimaire SEFI-INTRAFOR"
--
-- =======================================================================================================================================


BEGIN TRAN

-- Suppression de la "société intérimaire SOMOPA" 
IF EXISTS(SELECT * FROM FRED_SOCIETE WHERE Code = 'I0143' AND GroupeId in (SELECT GroupeId FROM FRED_GROUPE WHERE Code ='GFTP' AND PoleId in 
		 (SELECT PoleId FROM FRED_POLE WHERE Code = 'PTP' AND HoldingId in (SELECT HoldingId FROM FRED_HOLDING WHERE code = 'FSA'))))
					
BEGIN
		DELETE FROM FRED_SOCIETE WHERE Code = 'I0143' AND GroupeId in 
		(SELECT GroupeId FROM FRED_GROUPE WHERE Code ='GFTP' AND PoleId in 
		(SELECT PoleId FROM FRED_POLE WHERE Code = 'PTP' AND HoldingId in 
		(SELECT HoldingId FROM FRED_HOLDING WHERE code = 'FSA')))
END

-- Suppression de la "société intérimaire SEFI-INTRAFOR"
IF EXISTS(SELECT * FROM FRED_SOCIETE WHERE Code = 'IFS' AND GroupeId in (SELECT GroupeId FROM FRED_GROUPE WHERE Code ='FON' AND PoleId in 
		 (SELECT PoleId FROM FRED_POLE WHERE Code = 'SUPPORT' AND HoldingId in (SELECT HoldingId FROM FRED_HOLDING WHERE code = 'FSA'))))
					
BEGIN
		DELETE FROM FRED_SOCIETE WHERE Code = 'IFS' AND GroupeId in 
		(SELECT GroupeId FROM FRED_GROUPE WHERE Code ='FON' AND PoleId in 
		(SELECT PoleId FROM FRED_POLE WHERE Code = 'SUPPORT' AND HoldingId in 
		(SELECT HoldingId FROM FRED_HOLDING WHERE code = 'FSA')))
END

COMMIT TRAN
