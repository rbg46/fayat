-- =======================================================================================================================================
-- Author:		Yoann COLLET  13/12/2019
--
-- Description:
--      - Insertion ou modification des Code Emploi/ Code Ressource RZB dans la table TranspoCodeEmploiToRessource
--
-- =======================================================================================================================================

-----------------------------------------------------  ENCA-01 RZB ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD042')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CD042', 'ENCA-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD042')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-01' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD042'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD130')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CD130', 'ENCA-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD130')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-01' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD130'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD135')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CD135', 'ENCA-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD135')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-01' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD135'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD245')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CD245', 'ENCA-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD245')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-01' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD245'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD275')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CD275', 'ENCA-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD275')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-01' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD275'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD344')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CD344', 'ENCA-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD344')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-01' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD344'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD422')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CD422', 'ENCA-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD422')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-01' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD422'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD423')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CD423', 'ENCA-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD423')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-01' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD423'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD435')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CD435', 'ENCA-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD435')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-01' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD435'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD441')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CD441', 'ENCA-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD441')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-01' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD441'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD462')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CD462', 'ENCA-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD462')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-01' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD462'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD463')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CD463', 'ENCA-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD463')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-01' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD463'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD485')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CD485', 'ENCA-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD485')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-01' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD485'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD530')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CD530', 'ENCA-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD530')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-01' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD530'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD531')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CD531', 'ENCA-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD531')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-01' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CD531'
END

-----------------------------------------------------  ENCA-05 RZB ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM380')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AM380', 'ENCA-05')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM380')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-05' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM380'
END


IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC092')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CC092', 'ENCA-05')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC092')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-05' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC092'
END

-----------------------------------------------------  ENCA-15 RZB ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM190')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AM190', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM190')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM190'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMC80')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMC80', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMC80')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMC80'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMD20')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMD20', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMD20')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMD20'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMM40')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMM40', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMM40')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMM40'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMM60')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMM60', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMM60')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMM60'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMP20')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMP20', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMP20')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMP20'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMP40')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMP40', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMP40')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMP40'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMP41')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMP41', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMP41')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMP41'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMT20')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMT20', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMT20')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMT20'
END
IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMT40')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMT40', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMT40')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMT40'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC041')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CC041', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC041')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC041'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC071')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CC071', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC071')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC071'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC344')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CC344', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC344')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC344'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC346')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CC346', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC346')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC346'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CE002')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CE002', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CE002')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CE002'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CI005')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CI005', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CI005')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CI005'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CI011')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CI011', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CI011')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CI011'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CI132')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CI132', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CI132')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CI132'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CI136')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CI136', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CI136')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CI136'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CI161')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CI161', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CI161')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CI161'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CI165')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CI165', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CI165')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CI165'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'EP042')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'EP042', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'EP042')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'EP042'
END

-----------------------------------------------------  ENCA-20 RZB ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM070')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AM070', 'ENCA-20')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM070')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-20' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM070'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CA021')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CA021', 'ENCA-20')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CA021')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-20' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CA021'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CI141')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CI141', 'ENCA-20')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CI141')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-20' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CI141'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CI142')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CI142', 'ENCA-20')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CI142')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-20' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CI142'
END

-----------------------------------------------------  ENCA-25 RZB ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMG20')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMG20', 'ENCA-25')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMG20')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-25' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMG20'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMT30')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMT30', 'ENCA-25')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMT30')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-25' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMT30'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMT70')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMT70', 'ENCA-25')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMT70')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-25' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMT70'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'ER001')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'ER001', 'ENCA-25')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'ER001')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-25' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'ER001'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC360')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CC360', 'ENCA-25')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC360')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-25' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC360'
END

-----------------------------------------------------  ENCA-35 RZB ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM150')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AM150', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM150')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM150'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM160')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AM160', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM160')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM160'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM240')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AM240', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM240')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM240'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMA30')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMA30', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMA30')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMA30'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMC10')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMC10', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMC10')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMC10'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMC15')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMC15', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMC15')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMC15'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMD40')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMD40', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMD40')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMD40'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMG00')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMG00', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMG00')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMG00'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMG05')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMG05', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMG05')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMG05'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'ER001')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'ER001', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'ER001')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'ER001'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMS10')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMS10', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMS10')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMS10'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMS30')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMS30', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMS30')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMS30'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMT00')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMT00', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMT00')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMT00'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMT80')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMT80', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMT80')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMT80'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CA013')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CA013', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CA013')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CA013'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC145')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CC145', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC145')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC145'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC168')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CC168', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC168')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC168'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC171')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CC171', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC171')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC171'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC337')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CC337', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC337')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC337'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CF100')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CF100', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CF100')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CF100'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CR025')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CR025', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CR025')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CR025'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CR326')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CR326', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CR326')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CR326'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CR417')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CR417', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CR417')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CR417'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'EA129')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'EA129', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'EA129')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'EA129'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'EA229')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'EA229', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'EA229')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'EA229'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'EC231')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'EC231', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'EC231')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'EC231'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'EG065')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'EG065', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'EG065')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'EG065'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'ES040')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'ES040', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'ES040')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'ES040'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'ET005')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'ET005', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'ET005')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'ET005'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC421')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CC421', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC421')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC421'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CR369')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CR369', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CR369')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CR369'
END
-----------------------------------------------------  ENCA-40 RZB ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AG090')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AG090', 'ENCA-40')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AG090')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-40' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AG090'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC275')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CC275', 'ENCA-40')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC275')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-40' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC275'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC285')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'CC285', 'ENCA-40')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC285')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-40' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'CC285'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'ER060')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'ER060', 'ENCA-40')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'ER060')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-40' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'ER060'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OC099')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'OC099', 'ENCA-40')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OC099')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-40' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OC099'
END

-----------------------------------------------------  ENCA-90 RZB ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMC60')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMC60', 'ENCA-90')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMC60')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-90' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMC60'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'ER025')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'ER025', 'ENCA-90')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'ER025')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-90' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'ER025'
END

-----------------------------------------------------  MO-01 RZB ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM035')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AM035', 'MO-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM035')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-01' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM035'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM050')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AM050', 'MO-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM050')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-01' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM050'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM120')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AM120', 'MO-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM120')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-01' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM120'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OC089')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'OC089', 'MO-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OC089')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-01' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OC089'
END

-----------------------------------------------------  MO-05 RZB ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMP10')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMP10', 'MO-05')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMP10')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-05' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMP10'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OC177')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'OC177', 'MO-05')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OC177')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-05' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OC177'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OC260')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'OC260', 'MO-05')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OC260')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-05' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OC260'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OG015')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'OG015', 'MO-05')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OG015')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-05' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OG015'
END

-----------------------------------------------------  MO-10 RZB ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OC055')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'OC055', 'MO-10')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OC055')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-10' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OC055'
END

-----------------------------------------------------  MO-15 RZB ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OC163')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'OC163', 'MO-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OC163')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OC163'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OM015')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'OM015', 'MO-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OM015')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OM015'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OO130')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'OO130', 'MO-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OO130')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OO130'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OR030')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'OR030', 'MO-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OR030')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-15' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OR030'
END

-----------------------------------------------------  MO-20 RZB ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OA032')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'OA032', 'MO-20')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OA032')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-20' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OA032'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OA065')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'OA065', 'MO-20')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OA065')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-20' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OA065'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OH001')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'OH001', 'MO-20')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OH001')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-20' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OH001'
END
-----------------------------------------------------  MO-30 RZB ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM210')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AM210', 'MO-30')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM210')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-30' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM210'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMC40')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMC40', 'MO-30')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMC40')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-30' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMC40'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMC50')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMC50', 'MO-30')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMC50')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-30' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMC50'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMD30')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMD30', 'MO-30')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMD30')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-30' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMD30'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'EA300')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'EA300', 'MO-30')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'EA300')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-30' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'EA300'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'EM018')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'EM018', 'MO-30')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'EM018')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-30' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'EM018'
END

-----------------------------------------------------  MO-35 RZB ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM047')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AM047', 'MO-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM047')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM047'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM110')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AM110', 'MO-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM110')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AM110'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AME40')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AME40', 'MO-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AME40')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-35' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AME40'
END

-----------------------------------------------------  MO-90 RZB ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AML10')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AML10', 'MO-90')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AML10')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-90' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AML10'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMM10')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'AMM10', 'MO-90')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMM10')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-90' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'AMM10'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'ET102')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'ET102', 'MO-90')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'ET102')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-90' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'ET102'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OC360')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'OC360', 'MO-90')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OC360')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-90' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OC360'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OE005')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'RZB' , 'OE005', 'MO-90')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OE005')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-90' WHERE CodeSocieteImport = 'RZB' AND CodeEmploi = 'OE005'
END

