-- =======================================================================================================================================
-- Author:		Yoann COLLET  13/12/2019
--
-- Description:
--      - Insertion ou modification des Code Emploi/ Code Ressource LHE dans la table TranspoCodeEmploiToRessource
--
-- =======================================================================================================================================

-----------------------------------------------------  ENCA-01 LHE ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'CD060')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'LHE' , 'CD060', 'ENCA-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'CD060')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-01' WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'CD060'
END
-----------------------------------------------------  ENCA-15 LHE ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'CC040')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'LHE' , 'CC040', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'CC040')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'CC040'
END

-----------------------------------------------------  ENCA-25 LHE ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'EG020')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'LHE' , 'EG020', 'ENCA-25')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'EG020')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-25' WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'EG020'
END

-----------------------------------------------------  ENCA-35 LHE ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'CA040')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'LHE' , 'CA040', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'CA040')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'CA040'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'CA045')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'LHE' , 'CA045', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'CA045')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'CA045'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'CC220')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'LHE' , 'CC220', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'CC220')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'CC220'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'EA110')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'LHE' , 'EA110', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'EA110')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'EA110'
END

-----------------------------------------------------  MO-01 LHE ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OC068')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'LHE' , 'OC068', 'MO-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OC068')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-01' WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OC068'
END

-----------------------------------------------------  MO-05 LHE ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OC240')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'LHE' , 'OC240', 'MO-05')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OC240')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-05' WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OC240'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OC330')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'LHE' , 'OC330', 'MO-05')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OC330')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-05' WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OC330'
END

-----------------------------------------------------  MO-10 LHE ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OC060')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'LHE' , 'OC060', 'MO-10')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OC060')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-10' WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OC060'
END

-----------------------------------------------------  MO-15 LHE ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OM010')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'LHE' , 'OM010', 'MO-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OM010')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-15' WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OM010'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OO049')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'LHE' , 'OO049', 'MO-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OO049')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-15' WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OO049'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OR020')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'LHE' , 'OR020', 'MO-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OR020')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-15' WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OR020'
END

-----------------------------------------------------  MO-20 LHE ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OA015')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'LHE' , 'OA015', 'MO-20')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OA015')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-20' WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OA015'
END

-----------------------------------------------------  MO-30 LHE ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OM080')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'LHE' , 'OM080', 'MO-30')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OM080')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-30' WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OM080'
END

-----------------------------------------------------  MO-40 LHE ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OA090')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'LHE' , 'OA090', 'MO-40')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OA090')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-40' WHERE CodeSocieteImport = 'LHE' AND CodeEmploi = 'OA090'
END
