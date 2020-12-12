-- =======================================================================================================================================
-- Author:		Yoann COLLET  13/12/2019
--
-- Description:
--      - Insertion ou modification des Code Emploi/ Code Ressource MTP dans la table TranspoCodeEmploiToRessource
--
-- =======================================================================================================================================

-----------------------------------------------------  ENCA-01 MTP ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'CR498')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'MTP' , 'CR498', 'ENCA-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'CR498')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-01' WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'CR498'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'CR499')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'MTP' , 'CR499', 'ENCA-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'CR499')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-01' WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'CR499'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'CR500')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'MTP' , 'CR500', 'ENCA-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'CR500')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-01' WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'CR500'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'CR501')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'MTP' , 'CR501', 'ENCA-01')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'CR501')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-01' WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'CR501'
END

-----------------------------------------------------  ENCA-10 MTP ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'CC219')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'MTP' , 'CC219', 'ENCA-10')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'CC219')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-10' WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'CC219'
END


-----------------------------------------------------  ENCA-15 MTP ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'CD011')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'MTP' , 'CD011', 'ENCA-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'CD011')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-15' WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'CD011'
END

-----------------------------------------------------  ENCA-35 MTP ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'EA191')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'MTP' , 'EA191', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'EA191')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'EA191'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'EA245')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'MTP' , 'EA245', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'EA245')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'EA245'
END

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'EC232')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'MTP' , 'EC232', 'ENCA-35')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'EC232')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-35' WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'EC232'
END

-----------------------------------------------------  ENCA-40 MTP ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'CG025')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'MTP' , 'CG025', 'ENCA-40')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'CG025')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'ENCA-40' WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'CG025'
END

-----------------------------------------------------  MO-05 MTP ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'OC241')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'MTP' , 'OC241', 'MO-05')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'OC241')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-05' WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'OC241'
END

-----------------------------------------------------  MO-15 MTP ---------------------------------------------------------------------------

IF NOT EXISTS( SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'OO105')
BEGIN
    INSERT importExport.TranspoCodeEmploiToRessource (CodeSocieteImport, CodeEmploi, CodeRessource) VALUES ( 'MTP' , 'OO105', 'MO-15')
END
ELSE IF EXISTS (SELECT * FROM importExport.TranspoCodeEmploiToRessource WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'OO105')
BEGIN
    UPDATE importExport.TranspoCodeEmploiToRessource SET CodeRessource =  'MO-15' WHERE CodeSocieteImport = 'MTP' AND CodeEmploi = 'OO105'
END