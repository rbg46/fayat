-- --------------------------------------------------
-- FRED 2018 - R4 - OCTOBRE 2018 
-- INJECTION DES DONNEES RAZEL BEC - TABLE DE TRANSCO
-- --------------------------------------------------

DECLARE @STORM_CA INT = (SELECT SystemeImportId FROM FRED_SYSTEME_IMPORT Where Code = 'STORM_CA');
DECLARE @SocieteId INTEGER = (SELECT [SocieteId]  FROM [dbo].[FRED_SOCIETE] WHERE [Code] = 'RB' AND GroupeId = (SELECT groupeId FROM FRED_GROUPE where Code = 'GRZB'));

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-ENCA' AND CodeExterne = '105100' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-ENCA', '105100', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-MO' AND CodeExterne = '105250' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-MO', '105250', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-PERSO' AND CodeExterne = '160000' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-PERSO', '160000', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-FORM' AND CodeExterne = '101202' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-FORM', '101202', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-INSTAL' AND CodeExterne = '212420' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-INSTAL', '212420', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-TERR' AND CodeExterne = '211540' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-TERR', '211540', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-CAMIO' AND CodeExterne = '211520' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-CAMIO', '211520', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-LEV' AND CodeExterne = '211600' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-LEV', '211600', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-CONCA' AND CodeExterne = '211640' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-CONCA', '211640', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-ENERG' AND CodeExterne = '211610' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-ENERG', '211610', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-POMPE' AND CodeExterne = '211620' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-POMPE', '211620', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-LOCMAT' AND CodeExterne = '213110' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-LOCMAT', '213110', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-TOPO' AND CodeExterne = '211650' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-TOPO', '211650', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-VUVL' AND CodeExterne = '211510' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-VUVL', '211510', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-CARBU' AND CodeExterne = '221140' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-CARBU', '221140', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-PNEU' AND CodeExterne = '221600' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-PNEU', '221600', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-USUR' AND CodeExterne = '221510' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-USUR', '221510', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-PCR' AND CodeExterne = '221720' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-PCR', '221720', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-TR' AND CodeExterne = '221810' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-TR', '221810', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-FOATE' AND CodeExterne = '221930' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-FOATE', '221930', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-GAZ' AND CodeExterne = '221932' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-GAZ', '221932', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-OUATE' AND CodeExterne = '221933' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-OUATE', '221933', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-FOMAT' AND CodeExterne = '221900' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-FOMAT', '221900', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-ENTMAT' AND CodeExterne = '221931' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-ENTMAT', '221931', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-EAU' AND CodeExterne = '611500' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-EAU', '611500', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-REDE' AND CodeExterne = '334100' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-REDE', '334100', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-TVE' AND CodeExterne = '311620' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-TVE', '311620', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-LIANT' AND CodeExterne = '319300' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-LIANT', '319300', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-EXPL' AND CodeExterne = '313000' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-EXPL', '313000', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-BETON' AND CodeExterne = '321000' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-BETON', '321000', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-CIMEN' AND CodeExterne = '321500' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-CIMEN', '321500', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-ARMA' AND CodeExterne = '326000' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-ARMA', '326000', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-COFFR' AND CodeExterne = '317100' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-COFFR', '317100', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-PRECO' AND CodeExterne = '324000' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-PRECO', '324000', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-BOIS' AND CodeExterne = '317200' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-BOIS', '317200', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-PALPL' AND CodeExterne = '327300' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-PALPL', '327300', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-PREFA' AND CodeExterne = '325000' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-PREFA', '325000', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-MOBU' AND CodeExterne = '311640' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-MOBU', '311640', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-PAVE' AND CodeExterne = '314000' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-PAVE', '314000', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-PVC' AND CodeExterne = '317000' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-PVC', '317000', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-METAL' AND CodeExterne = '328000' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-METAL', '328000', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-ASPH' AND CodeExterne = '320100' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-ASPH', '320100', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-GEO' AND CodeExterne = '315000' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-GEO', '315000', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-SERR' AND CodeExterne = '337000' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-SERR', '337000', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-CHIM' AND CodeExterne = '318000' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-CHIM', '318000', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-VEGE' AND CodeExterne = '311610' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-VEGE', '311610', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-PHYTO' AND CodeExterne = '311630' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-PHYTO', '311630', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-FOURNI' AND CodeExterne = '333000' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-FOURNI', '333000', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-FOURLABO' AND CodeExterne = '611600' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-FOURLABO', '611600', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-FOURTOPO' AND CodeExterne = '611620' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-FOURTOPO', '611620', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-SECUR' AND CodeExterne = '618710' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-SECUR', '618710', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-REDEV' AND CodeExterne = '334200' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-REDEV', '334200', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-TRSV' AND CodeExterne = '334400' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-TRSV', '334400', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-STPD' AND CodeExterne = '410000' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-STPD', '410000', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-STPE' AND CodeExterne = '450000' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-STPE', '450000', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-ETUD' AND CodeExterne = '592000' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-ETUD', '592000', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-CTRL' AND CodeExterne = '592100' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-CTRL', '592100', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-ESSAI' AND CodeExterne = '581400' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-ESSAI', '581400', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-GARDIEN' AND CodeExterne = '582400' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-GARDIEN', '582400', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-TRANS' AND CodeExterne = '619000' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-TRANS', '619000', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-PREST' AND CodeExterne = '590000' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-PREST', '590000', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-HON' AND CodeExterne = '591100' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-HON', '591100', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-NDD' AND CodeExterne = '582300' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-NDD', '582300', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-ACTE' AND CodeExterne = '593000' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-ACTE', '593000', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-PIL' AND CodeExterne = '594000' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-PIL', '594000', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-MISS' AND CodeExterne = '611160' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-MISS', '611160', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-VOY' AND CodeExterne = '611170' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-VOY', '611170', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-RECRT' AND CodeExterne = '611990' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-RECRT', '611990', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-TERRAIN' AND CodeExterne = '611230' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-TERRAIN', '611230', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-CBAIL' AND CodeExterne = '611300' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-CBAIL', '611300', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-ENTC' AND CodeExterne = '611400' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-ENTC', '611400', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-LOCBUR' AND CodeExterne = '611630' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-LOCBUR', '611630', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-ENTBUR' AND CodeExterne = '611700' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-ENTBUR', '611700', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-COURS' AND CodeExterne = '611220' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-COURS', '611220', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-POST' AND CodeExterne = '611200' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-POST', '611200', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-PHON' AND CodeExterne = '611210' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-PHON', '611210', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-FINFO' AND CodeExterne = '611610' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-FINFO', '611610', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-FOURBUR' AND CodeExterne = '611740' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-FOURBUR', '611740', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-DOCU' AND CodeExterne = '611750' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-DOCU', '611750', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-REPRO' AND CodeExterne = '611760' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-REPRO', '611760', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-KDO' AND CodeExterne = '611810' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-KDO', '611810', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-DON' AND CodeExterne = '611820' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-DON', '611820', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-SEMI' AND CodeExterne = '611951' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-SEMI', '611951', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-PUB' AND CodeExterne = '611770' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-PUB', '611770', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-COTIS' AND CodeExterne = '611780' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-COTIS', '611780', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-TRAD' AND CodeExterne = '611190' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-TRAD', '611190', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-ARCH' AND CodeExterne = '611195' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-ARCH', '611195', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-BREVET' AND CodeExterne = '611790' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-BREVET', '611790', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-DROIT' AND CodeExterne = '611850' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-DROIT', '611850', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-TAXE' AND CodeExterne = '611805' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-TAXE', '611805', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-PENAL' AND CodeExterne = '611961' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-PENAL', '611961', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-ASSU' AND CodeExterne = '649400' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-ASSU', '649400', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

IF((SELECT COUNT(*) FROM FRED_TRANSCO_IMPORT WHERE  CodeInterne = 'LIT-SUBV' AND CodeExterne = '630720' AND SocieteId = @SocieteID)=0) BEGIN
   INSERT INTO FRED_TRANSCO_IMPORT ([CodeInterne],[CodeExterne],[SocieteId],[SystemeImportId]) 
   SELECT 'LIT-SUBV', '630720', societeId  , @STORM_CA FROM FRED_SOCIETE  Where GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GRZB') AND FRED_SOCIETE.code =  'RB'
END ELSE BEGIN PRINT 'Ajout impossible, Données Existante' END

