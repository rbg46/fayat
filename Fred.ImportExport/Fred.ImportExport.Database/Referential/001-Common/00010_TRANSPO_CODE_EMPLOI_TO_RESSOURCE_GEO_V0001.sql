-- =======================================================================================================================================
-- Author:		Yoann COLLET  13/12/2019
--
-- Description:
--      - Insertion ou modification des Code Emploi/ Code Ressource GEO dans la table TranspoCodeEmploiToRessource
--
-- =======================================================================================================================================

-----------------------------------------------------  ENCA-01 GEO ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CC180')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'GEO' , 'CC180', 'ENCA-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CC180')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-01' WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CC180'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CC240')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'GEO' , 'CC240', 'ENCA-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CC240')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-01' WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CC240'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CR210')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'GEO' , 'CR210', 'ENCA-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CR210')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-01' WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CR210'
END

-----------------------------------------------------  ENCA-05 GEO ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CC090')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'GEO' , 'CC090', 'ENCA-05')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CC090')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-05' WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CC090'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CC380')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'GEO' , 'CC380', 'ENCA-05')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CC380')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-05' WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CC380'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CI200')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'GEO' , 'CI200', 'ENCA-05')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CI200')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-05' WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CI200'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CR190')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'GEO' , 'CR190', 'ENCA-05')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CR190')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-05' WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CR190'
END

-----------------------------------------------------  ENCA-10 GEO ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'AM060')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'GEO' , 'AM060', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'AM060')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'AM060'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'EA120')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'GEO' , 'EA120', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'EA120')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'EA120'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'EC060')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'GEO' , 'EC060', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'EC060')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'EC060'
END

-----------------------------------------------------  ENCA-15 GEO ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CC120')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'GEO' , 'CC120', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CC120')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CC120'
END

-----------------------------------------------------  ENCA-20 GEO ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CI140')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'GEO' , 'CI140', 'ENCA-20')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CI140')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-20' WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CI140'
END

-----------------------------------------------------  ENCA-25 GEO ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'EG030')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'GEO' , 'EG030', 'ENCA-25')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'EG030')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-25' WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'EG030'
END

-----------------------------------------------------  ENCA-35 GEO ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'AM020')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'GEO' , 'AM020', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'AM020')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'AM020'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'AM230')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'GEO' , 'AM230', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'AM230')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'AM230'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CR030')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'GEO' , 'CR030', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CR030')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CR030'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CR140')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'GEO' , 'CR140', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CR140')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'CR140'
END

-----------------------------------------------------  MO-01 GEO ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'OC070')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'GEO' , 'OC070', 'MO-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'OC070')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-01' WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'OC070'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'OC120')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'GEO' , 'OC120', 'MO-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'OC120')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-01' WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'OC120'
END

-----------------------------------------------------  MO-15 GEO ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'OO080')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'GEO' , 'OO080', 'MO-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'OO080')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-15' WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'OO080'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'OP010')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'GEO' , 'OP010', 'MO-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'OP010')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-15' WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'OP010'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'OS010')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'GEO' , 'OS010', 'MO-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'OS010')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-15' WHERE CodeSocieteImport = 'GEO' AND CodeEmploi = 'OS010'
END