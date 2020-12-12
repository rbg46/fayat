-- Alimentation de la table FRED_RESSOURCE

-- NOTE JNE : Il existe déjà des PS pour faire ça : Fred_ToolBox, merci de les utiliser


CREATE PROCEDURE #SET_FRED_RESSOURCE
	@Code nvarchar(20), 
	@Libelle nvarchar(500), 
	@DateCreation datetime, 
	@CodeSousChapitre nvarchar(20), 
	@TypeRessourceCode nvarchar(20), 
	@Active bit

AS
BEGIN
	IF NOT EXISTS ( SELECT 1 FROM FRED_RESSOURCE WHERE Code = @Code)
	BEGIN
	    declare @sousChapitreId int;
		SET @sousChapitreId = (SELECT SousChapitreId FROM FRED_SOUS_CHAPITRE WHERE Code LIKE @CodeSousChapitre);

		declare @typeResourceId int;
		SET @typeResourceId = (SELECT TypeRessourceId FROM FRED_TYPE_RESSOURCE WHERE Code LIKE @TypeRessourceCode);

		INSERT INTO FRED_RESSOURCE (Code, Libelle, DateCreation, SousChapitreId, TypeRessourceId, Active)
		VALUES (@Code, @Libelle, @DateCreation, @sousChapitreId, @typeResourceId, @Active)
	END
END
GO

-- Insertion des données

EXEC #SET_FRED_RESSOURCE @Code='106', @Libelle='106', @DateCreation='2010-02-04', @CodeSousChapitre='VL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='206', @Libelle='206', @DateCreation='2010-02-04', @CodeSousChapitre='VL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='207', @Libelle='207', @DateCreation='2016-02-12', @CodeSousChapitre='VL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='208', @Libelle='Peugeot 208', @DateCreation='2013-07-18', @CodeSousChapitre='VL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='307', @Libelle='307', @DateCreation='2010-02-04', @CodeSousChapitre='VL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='308', @Libelle='Peugeot 308', @DateCreation='2013-07-18', @CodeSousChapitre='VL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='1000', @Libelle='1000 Locatext MO Chauffeur', @DateCreation='2010-05-06', @CodeSousChapitre='LOCATEXTM', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='1500', @Libelle='1500 (Clio sté / C2 sté)', @DateCreation='2010-02-04', @CodeSousChapitre='LOCEXTVL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='1511', @Libelle='1511 (kangoo)', @DateCreation='2010-02-04', @CodeSousChapitre='LOCEXTCTTE', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='1512', @Libelle='1512 (Jumpy / Traffic)', @DateCreation='2010-02-04', @CodeSousChapitre='LOCEXTCTTE', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='1520', @Libelle='1520 (Jumper)', @DateCreation='2010-02-04', @CodeSousChapitre='LOCEXTCTTE', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='1521', @Libelle='1521 (Benne) ', @DateCreation='2010-02-04', @CodeSousChapitre='LOCEXTCTTE', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='1522', @Libelle='1522 (Benne grue)', @DateCreation='2010-02-04', @CodeSousChapitre='LOCEXTCTTE', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='1530', @Libelle='1530 (10T grue)', @DateCreation='2010-02-04', @CodeSousChapitre='LOCEXTPL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='1532', @Libelle='1532 (Nacelle 16m plateau)', @DateCreation='2010-02-04', @CodeSousChapitre='LOCEXTNAC', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='1533', @Libelle='1533 (Nacelle Topy 10m) ', @DateCreation='2010-02-04', @CodeSousChapitre='LOCEXTNAC', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='1534', @Libelle='1534 (Nacelle fourgon 11m)', @DateCreation='2010-02-04', @CodeSousChapitre='LOCEXTNAC', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='1535', @Libelle='1535 (15T grue)', @DateCreation='2010-02-04', @CodeSousChapitre='LOCEXTPL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='1536', @Libelle='CAMION PORTE POTEAUX ', @DateCreation='2011-11-03', @CodeSousChapitre='LOCEXTPL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='1537', @Libelle='PL 19T GRUE 8x4', @DateCreation='2011-11-03', @CodeSousChapitre='LOCEXTPL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='10000', @Libelle='Achat Mesure', @DateCreation='2010-06-14', @CodeSousChapitre='10000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='10110', @Libelle='Analyseur d énergie', @DateCreation='2010-05-06', @CodeSousChapitre='10100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='10210', @Libelle='Analyseur enregistreur harmonique', @DateCreation='2010-05-06', @CodeSousChapitre='10200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='10310', @Libelle='Caméra thermo graphique', @DateCreation='2010-05-06', @CodeSousChapitre='10300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='10320', @Libelle='Caméra de contrôle thermo graphique', @DateCreation='2010-05-06', @CodeSousChapitre='10300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='10410', @Libelle='Contrôleur bipolaire HTA', @DateCreation='2010-05-06', @CodeSousChapitre='10400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='10510', @Libelle='Tellurohmetre contrôleur de terre', @DateCreation='2010-05-06', @CodeSousChapitre='10500', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='10610', @Libelle='Megohmetre', @DateCreation='2010-05-06', @CodeSousChapitre='10600', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='10620', @Libelle='Controleur d''isolement', @DateCreation='2014-04-16', @CodeSousChapitre='10600', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='10710', @Libelle='Contrôleur VAT', @DateCreation='2010-05-06', @CodeSousChapitre='10700', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='10711', @Libelle='Contrôleur VAT HT', @DateCreation='2013-12-18', @CodeSousChapitre='10700', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='10810', @Libelle='Appareil photo numérique', @DateCreation='2010-05-06', @CodeSousChapitre='10800', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='10910', @Libelle='Détecteur canalisations ecchoM', @DateCreation='2010-05-06', @CodeSousChapitre='10900', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='11010', @Libelle='Détecteur de câbles', @DateCreation='2010-05-06', @CodeSousChapitre='11000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='11110', @Libelle='Détecteur de métaux', @DateCreation='2010-05-06', @CodeSousChapitre='11100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='11210', @Libelle='Dynamomètre de traction', @DateCreation='2010-05-06', @CodeSousChapitre='11200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='11310', @Libelle='Luxmètre', @DateCreation='2010-05-06', @CodeSousChapitre='11300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='11320', @Libelle='Luxmetre Cartographie', @DateCreation='2010-10-25', @CodeSousChapitre='11300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='11410', @Libelle='Lasermetre metrique', @DateCreation='2010-05-06', @CodeSousChapitre='11400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='11420', @Libelle='Lasermetre Niveau', @DateCreation='2010-05-06', @CodeSousChapitre='11400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='11430', @Libelle='Lasermetre Rotatif', @DateCreation='2010-05-06', @CodeSousChapitre='11400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='11510', @Libelle='Multimetre', @DateCreation='2010-05-06', @CodeSousChapitre='11500', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='11520', @Libelle='Multimetre   Pince ampermetrique', @DateCreation='2010-05-06', @CodeSousChapitre='11500', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='11530', @Libelle='Controleur differentiel', @DateCreation='2010-05-06', @CodeSousChapitre='11500', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='11610', @Libelle='Multi testeur', @DateCreation='2010-05-06', @CodeSousChapitre='11600', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='11710', @Libelle='Ohmmètre de boucle', @DateCreation='2010-05-06', @CodeSousChapitre='11700', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='11810', @Libelle='Oscilloscope', @DateCreation='2010-05-06', @CodeSousChapitre='11800', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='11910', @Libelle='Pince Amp. 200 A', @DateCreation='2010-05-06', @CodeSousChapitre='11900', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='11920', @Libelle='Pince Amp. 400A', @DateCreation='2010-05-06', @CodeSousChapitre='11900', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='12010', @Libelle='Topomètre / odomètre', @DateCreation='2010-05-06', @CodeSousChapitre='12000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='12110', @Libelle='Vérificateur de réseaux info wirescope', @DateCreation='2010-05-06', @CodeSousChapitre='12100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='12120', @Libelle='Testeur réseaux', @DateCreation='2014-12-31', @CodeSousChapitre='12100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='12210', @Libelle='Sonometre', @DateCreation='2010-05-06', @CodeSousChapitre='12200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='12310', @Libelle='Cle dynamometrique', @DateCreation='2010-05-06', @CodeSousChapitre='12300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='12410', @Libelle='Reflectometre', @DateCreation='2010-05-06', @CodeSousChapitre='12400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='12420', @Libelle='Accessoire reflect. Bobine amorce', @DateCreation='2010-05-06', @CodeSousChapitre='12400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='12510', @Libelle='Anemometre', @DateCreation='2010-05-06', @CodeSousChapitre='12500', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='12610', @Libelle='Spectrosonde', @DateCreation='2010-05-06', @CodeSousChapitre='12600', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='12710', @Libelle='Calibrateur 4/20', @DateCreation='2010-05-06', @CodeSousChapitre='12700', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='13010', @Libelle='Mesureur de cable', @DateCreation='2010-05-06', @CodeSousChapitre='13000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='13110', @Libelle='thermohygrometre', @DateCreation='2010-05-06', @CodeSousChapitre='13100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='13210', @Libelle='Mesureur de vibration', @DateCreation='2010-05-06', @CodeSousChapitre='13200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='13310', @Libelle='Dielectrometre', @DateCreation='2010-05-06', @CodeSousChapitre='13300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='13410', @Libelle='Calibreur de pression', @DateCreation='2010-05-06', @CodeSousChapitre='13400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='13510', @Libelle='PRELEVEUR AUTONOME MULTIFLACONS ISOTHERME', @DateCreation='2015-10-26', @CodeSousChapitre='13500', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='13520', @Libelle='DEBITMETRE D''ECHANTILLON AUTONOME MAINFLOW', @DateCreation='2016-08-29', @CodeSousChapitre='13500', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='13530', @Libelle='CAPTEUR ODEON', @DateCreation='2016-08-29', @CodeSousChapitre='13500', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='20000', @Libelle='Achat Perfo Sciage', @DateCreation='2010-06-14', @CodeSousChapitre='20000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='20110', @Libelle='Visseuse', @DateCreation='2010-05-06', @CodeSousChapitre='20100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='20210', @Libelle='Meuleuse diam 125', @DateCreation='2010-05-06', @CodeSousChapitre='20200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='20211', @Libelle='Meuleuse diam 125 accu', @DateCreation='2012-06-27', @CodeSousChapitre='20200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='20220', @Libelle='Meuleuse diam 230', @DateCreation='2010-05-06', @CodeSousChapitre='20200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='20230', @Libelle='Meuleuse diam 230 pneumatic', @DateCreation='2010-05-06', @CodeSousChapitre='20200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='20240', @Libelle='Meuleuse diam 300 pneumatic', @DateCreation='2010-05-06', @CodeSousChapitre='20200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='20310', @Libelle='Perceuse', @DateCreation='2010-05-06', @CodeSousChapitre='20300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='20410', @Libelle='Perfo autonome', @DateCreation='2010-05-06', @CodeSousChapitre='20400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='20420', @Libelle='Perfo filaire moyenne puissance', @DateCreation='2010-05-06', @CodeSousChapitre='20400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='20430', @Libelle='Perfo filaire forte puissance', @DateCreation='2010-06-11', @CodeSousChapitre='20400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='20440', @Libelle='Perfo Amiante', @DateCreation='2013-12-18', @CodeSousChapitre='20400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='20510', @Libelle='Ponceuse', @DateCreation='2010-05-06', @CodeSousChapitre='20500', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='20610', @Libelle='Scie circulaire', @DateCreation='2010-05-06', @CodeSousChapitre='20600', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='20710', @Libelle='Scie sabre', @DateCreation='2010-05-06', @CodeSousChapitre='20700', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='20810', @Libelle='Scie sauteuse', @DateCreation='2010-05-06', @CodeSousChapitre='20800', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='20910', @Libelle='Tronçonneuse', @DateCreation='2010-05-06', @CodeSousChapitre='20900', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='21010', @Libelle='Burineur', @DateCreation='2010-05-06', @CodeSousChapitre='21000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='21110', @Libelle='Carotteuse', @DateCreation='2010-05-06', @CodeSousChapitre='21100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='21210', @Libelle='Brise béton', @DateCreation='2010-05-06', @CodeSousChapitre='21200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='21310', @Libelle='Marteau piqueur autonome', @DateCreation='2010-05-06', @CodeSousChapitre='21300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='21410', @Libelle='Marteau piqueur', @DateCreation='2010-05-06', @CodeSousChapitre='21400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='21415', @Libelle='Marteau piqueur électrique', @DateCreation='2013-05-17', @CodeSousChapitre='21400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='21510', @Libelle='Rainureuse beton', @DateCreation='2010-05-06', @CodeSousChapitre='21500', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='21520', @Libelle='Rainureuse platre', @DateCreation='2010-05-06', @CodeSousChapitre='21500', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='21610', @Libelle='Scie de sol manuelle', @DateCreation='2010-05-06', @CodeSousChapitre='21600', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='21620', @Libelle='Scie de sol autoportée', @DateCreation='2010-05-06', @CodeSousChapitre='21600', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='21630', @Libelle='Scie de sol pneumatique', @DateCreation='2010-05-06', @CodeSousChapitre='21600', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='30000', @Libelle='Achat Installation Chantier', @DateCreation='2010-06-14', @CodeSousChapitre='30000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='30110', @Libelle='Coffre de chantier 1000x800x600', @DateCreation='2010-05-06', @CodeSousChapitre='30100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='30120', @Libelle='Coffre de chantier 2000x1000x1000', @DateCreation='2010-05-06', @CodeSousChapitre='30100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='30210', @Libelle='Coffret électrique de chantier', @DateCreation='2010-05-06', @CodeSousChapitre='30200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='30310', @Libelle='Frigo', @DateCreation='2010-05-06', @CodeSousChapitre='30300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='30410', @Libelle='Glacière', @DateCreation='2010-05-06', @CodeSousChapitre='30400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='30510', @Libelle='Micro ondes', @DateCreation='2010-05-06', @CodeSousChapitre='30500', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='30610', @Libelle='Tente', @DateCreation='2010-05-06', @CodeSousChapitre='30600', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='30710', @Libelle='Etabli   eteau', @DateCreation='2010-05-06', @CodeSousChapitre='30700', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='30810', @Libelle='Vestiaire', @DateCreation='2010-05-06', @CodeSousChapitre='30800', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='30910', @Libelle='Compress compact air', @DateCreation='2010-05-06', @CodeSousChapitre='30900', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='30920', @Libelle='Compresseur 8à 10bars', @DateCreation='2010-05-06', @CodeSousChapitre='30900', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31010', @Libelle='Aerotherme 3000 W', @DateCreation='2010-05-06', @CodeSousChapitre='31000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31020', @Libelle='Aerotherme 5000 W', @DateCreation='2010-05-06', @CodeSousChapitre='31000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31030', @Libelle='Aerotherme 12000 W', @DateCreation='2010-05-06', @CodeSousChapitre='31000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31110', @Libelle='Grp elec ess <2.4KVA', @DateCreation='2010-05-06', @CodeSousChapitre='31100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31120', @Libelle='Grp elec ess 4KVA', @DateCreation='2010-05-06', @CodeSousChapitre='31100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31130', @Libelle='Grp elec ess >7KVA', @DateCreation='2010-05-06', @CodeSousChapitre='31100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31140', @Libelle='Grp elec GO  7KVA', @DateCreation='2010-05-06', @CodeSousChapitre='31100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31150', @Libelle='Grp elec 100KVA', @DateCreation='2010-05-06', @CodeSousChapitre='31100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31160', @Libelle='Grp elec 15KVA', @DateCreation='2011-02-08', @CodeSousChapitre='31100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31210', @Libelle='Climatisation', @DateCreation='2010-05-06', @CodeSousChapitre='31200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31310', @Libelle='Sono', @DateCreation='2010-05-06', @CodeSousChapitre='31300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31320', @Libelle='video projecteur', @DateCreation='2012-11-23', @CodeSousChapitre='31300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31410', @Libelle='Bungalows vestiaire', @DateCreation='2010-05-06', @CodeSousChapitre='31400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31420', @Libelle='Bungalows bureau', @DateCreation='2010-05-06', @CodeSousChapitre='31400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31430', @Libelle='Bungalow sanitaire', @DateCreation='2010-05-06', @CodeSousChapitre='31400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31440', @Libelle='Bungalow réfectoire', @DateCreation='2010-05-06', @CodeSousChapitre='31400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31450', @Libelle='Bungalow mixte (vest / refect)', @DateCreation='2010-05-06', @CodeSousChapitre='31400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31455', @Libelle='WC chimique', @DateCreation='2012-10-30', @CodeSousChapitre='31400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31460', @Libelle='Container 10 pieds', @DateCreation='2010-05-06', @CodeSousChapitre='31400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31470', @Libelle='Container 20 pieds', @DateCreation='2010-05-06', @CodeSousChapitre='31400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31480', @Libelle='Container 40 pieds', @DateCreation='2010-05-06', @CodeSousChapitre='31400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31490', @Libelle='Roulotte', @DateCreation='2010-05-06', @CodeSousChapitre='31400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31500', @Libelle='Benne 10m3', @DateCreation='2010-05-06', @CodeSousChapitre='31500', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31600', @Libelle='Compacteur à déchets', @DateCreation='2010-05-06', @CodeSousChapitre='31600', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31710', @Libelle='Extracteur air', @DateCreation='2010-08-04', @CodeSousChapitre='31700', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='31910', @Libelle='Cone panneau signalisation', @DateCreation='2012-10-09', @CodeSousChapitre='31900', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='40000', @Libelle='Achat Manut Transport', @DateCreation='2010-06-15', @CodeSousChapitre='40000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='40110', @Libelle='Chariot diable', @DateCreation='2010-05-06', @CodeSousChapitre='40100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='40120', @Libelle='Gerbeur', @DateCreation='2010-05-06', @CodeSousChapitre='40100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='40210', @Libelle='Home trainer - 200 Kg', @DateCreation='2010-05-06', @CodeSousChapitre='40200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='40220', @Libelle='Home trainer  200 à 800 Kg', @DateCreation='2010-05-06', @CodeSousChapitre='40200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='40230', @Libelle='Home trainer  800 à 1.5 T', @DateCreation='2010-05-06', @CodeSousChapitre='40200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='40310', @Libelle='Chariot dérouleur de touret', @DateCreation='2010-05-06', @CodeSousChapitre='40300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='40410', @Libelle='Diables élévateurs jumelés', @DateCreation='2010-05-06', @CodeSousChapitre='40400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='40510', @Libelle='Vérin de déroulage >800 Kg', @DateCreation='2010-05-06', @CodeSousChapitre='40500', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='40520', @Libelle='Vérin de déroulage 800 Kg à 2 T', @DateCreation='2010-05-06', @CodeSousChapitre='40500', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='40610', @Libelle='Porte charge monte escaliers', @DateCreation='2010-05-06', @CodeSousChapitre='40600', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='40710', @Libelle='Rouleurs de fortes charges', @DateCreation='2010-05-06', @CodeSousChapitre='40700', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='40810', @Libelle='Dérouleur de gaine', @DateCreation='2010-05-06', @CodeSousChapitre='40800', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='40910', @Libelle='Galet', @DateCreation='2010-05-06', @CodeSousChapitre='40900', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41010', @Libelle='Palans 1 T', @DateCreation='2010-05-06', @CodeSousChapitre='41000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41020', @Libelle='Palans 3 T', @DateCreation='2010-05-06', @CodeSousChapitre='41000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41110', @Libelle='Tirefort 0.8T', @DateCreation='2010-05-06', @CodeSousChapitre='41100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41120', @Libelle='Tirefort 5T', @DateCreation='2010-05-06', @CodeSousChapitre='41100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41210', @Libelle='Treuil 0.5 T', @DateCreation='2010-05-06', @CodeSousChapitre='41200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41220', @Libelle='Treuil 1 T', @DateCreation='2010-05-06', @CodeSousChapitre='41200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41230', @Libelle='Treuil 2 T', @DateCreation='2010-05-06', @CodeSousChapitre='41200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41240', @Libelle='Treuil 3T', @DateCreation='2010-05-06', @CodeSousChapitre='41200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41250', @Libelle='Treuil 4T', @DateCreation='2010-05-06', @CodeSousChapitre='41200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41260', @Libelle='Treuil 0.3 T elec220v', @DateCreation='2010-11-30', @CodeSousChapitre='41200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41270', @Libelle='Treuil 0.5 T elec 220v', @DateCreation='2010-11-30', @CodeSousChapitre='41200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41280', @Libelle='Treuil 2 T elec 220v', @DateCreation='2010-11-30', @CodeSousChapitre='41200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41290', @Libelle='TREUIL 10T', @DateCreation='2014-04-29', @CodeSousChapitre='41200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41310', @Libelle='Transpalette', @DateCreation='2010-05-06', @CodeSousChapitre='41300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41410', @Libelle='Aiguille 80 M', @DateCreation='2010-05-06', @CodeSousChapitre='41400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41420', @Libelle='Aiguille 200 M', @DateCreation='2010-05-06', @CodeSousChapitre='41400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41430', @Libelle='Aiguille 300M', @DateCreation='2010-05-06', @CodeSousChapitre='41400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41440', @Libelle='Aiguille vibrante autonome', @DateCreation='2010-05-06', @CodeSousChapitre='41400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41450', @Libelle='Corde sur touret', @DateCreation='2010-05-06', @CodeSousChapitre='41400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41510', @Libelle='Leve tampon', @DateCreation='2010-05-06', @CodeSousChapitre='41500', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41610', @Libelle='Rampe', @DateCreation='2010-05-06', @CodeSousChapitre='41600', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41710', @Libelle='Porteuse pneumatique', @DateCreation='2010-05-06', @CodeSousChapitre='41700', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41810', @Libelle='Dérouleuse 1T', @DateCreation='2010-05-06', @CodeSousChapitre='41800', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41820', @Libelle='Dérouleuse 1T5', @DateCreation='2010-05-06', @CodeSousChapitre='41800', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41830', @Libelle='Dérouleuse 4T', @DateCreation='2010-05-06', @CodeSousChapitre='41800', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41840', @Libelle='Dérouleuse 5T5', @DateCreation='2010-05-06', @CodeSousChapitre='41800', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41850', @Libelle='Dérouleuse 20T', @DateCreation='2010-05-06', @CodeSousChapitre='41800', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='41910', @Libelle='Chariot élévateur', @DateCreation='2010-05-06', @CodeSousChapitre='41900', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='42010', @Libelle='Remorque', @DateCreation='2010-05-06', @CodeSousChapitre='42000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='42020', @Libelle='Remorque basculante', @DateCreation='2010-05-06', @CodeSousChapitre='42000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='42030', @Libelle='Remorque plateau', @DateCreation='2010-05-06', @CodeSousChapitre='42000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='43010', @Libelle='Potence', @DateCreation='2010-05-06', @CodeSousChapitre='43000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='43020', @Libelle='Potence materiel', @DateCreation='2010-05-06', @CodeSousChapitre='43000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='50000', @Libelle='Achat Outillage', @DateCreation='2010-06-15', @CodeSousChapitre='50000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='50110', @Libelle='Cintreuse', @DateCreation='2010-05-06', @CodeSousChapitre='50100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='50210', @Libelle='Filliere', @DateCreation='2010-05-06', @CodeSousChapitre='50200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='50310', @Libelle='Enrouleur rallonge', @DateCreation='2010-05-06', @CodeSousChapitre='50300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='50410', @Libelle='Clé a choc electrique', @DateCreation='2010-05-06', @CodeSousChapitre='50400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='50420', @Libelle='Clé a choc pneumatique', @DateCreation='2010-05-06', @CodeSousChapitre='50400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='50510', @Libelle='Cloueuse electrique', @DateCreation='2010-05-06', @CodeSousChapitre='50500', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='50520', @Libelle='Cloueuse à gaz', @DateCreation='2010-05-06', @CodeSousChapitre='50500', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='50610', @Libelle='Coffret à dénuder HT/BT', @DateCreation='2010-05-06', @CodeSousChapitre='50600', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='50710', @Libelle='Coffret à douilles', @DateCreation='2010-05-06', @CodeSousChapitre='50700', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='50810', @Libelle='Coffret à cliquet', @DateCreation='2010-05-06', @CodeSousChapitre='50800', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='50910', @Libelle='Coffret de mise à la terre «  BT aérien »', @DateCreation='2010-05-06', @CodeSousChapitre='50900', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='51010', @Libelle='Etiqueteuse', @DateCreation='2010-05-06', @CodeSousChapitre='51000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='60000', @Libelle='Achat Soudure', @DateCreation='2010-06-15', @CodeSousChapitre='60000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='60110', @Libelle='Poste à souder', @DateCreation='2010-05-06', @CodeSousChapitre='60100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='60210', @Libelle='Chariot chalumeau découpeur', @DateCreation='2010-05-06', @CodeSousChapitre='60200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='60310', @Libelle='Chariot chalumeau', @DateCreation='2010-05-06', @CodeSousChapitre='60300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='60410', @Libelle='Poste TIG', @DateCreation='2010-05-06', @CodeSousChapitre='60400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='60510', @Libelle='Soudeuse bobine fb opt.', @DateCreation='2010-05-06', @CodeSousChapitre='60500', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='60610', @Libelle='Decap thermique', @DateCreation='2010-05-06', @CodeSousChapitre='60600', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='60710', @Libelle='Furet thermo', @DateCreation='2010-05-06', @CodeSousChapitre='60700', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='60810', @Libelle='Soudure ensemble Flasch GAZ', @DateCreation='2010-05-06', @CodeSousChapitre='60800', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='70000', @Libelle='Achat Sertissage', @DateCreation='2010-06-15', @CodeSousChapitre='70000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='70110', @Libelle='Pince à sertir mamuelle', @DateCreation='2010-05-06', @CodeSousChapitre='70100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='70210', @Libelle='Pince à sertir electrique', @DateCreation='2010-05-06', @CodeSousChapitre='70200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='70310', @Libelle='Pompe à sertir (20T)', @DateCreation='2010-05-06', @CodeSousChapitre='70300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='70410', @Libelle='Coffret sertissage cuivre', @DateCreation='2010-05-06', @CodeSousChapitre='70400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='70510', @Libelle='Coffret sertissage alu', @DateCreation='2010-05-06', @CodeSousChapitre='70500', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='70610', @Libelle='Coupe câble a cliquet', @DateCreation='2010-05-06', @CodeSousChapitre='70600', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='70620', @Libelle='Coupe câble a chaine', @DateCreation='2010-05-06', @CodeSousChapitre='70600', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='70630', @Libelle='Coupe câble electrique', @DateCreation='2010-05-06', @CodeSousChapitre='70600', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='70640', @Libelle='Coupe câble hydraulique', @DateCreation='2010-05-06', @CodeSousChapitre='70600', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='70650', @Libelle='Outil de coupe', @DateCreation='2010-05-06', @CodeSousChapitre='70600', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='70710', @Libelle='Emporte pièces', @DateCreation='2010-05-06', @CodeSousChapitre='70700', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='70810', @Libelle='Presse à sertir 100 tonnes', @DateCreation='2016-11-22', @CodeSousChapitre='70800', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='70820', @Libelle='Presse à sertir 20 Tonnes', @DateCreation='2016-11-22', @CodeSousChapitre='70800', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='70910', @Libelle='Groupe hydraulique thermique pour presse à sertir', @DateCreation='2016-11-22', @CodeSousChapitre='70900', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='80000', @Libelle='Achat Securite', @DateCreation='2010-06-15', @CodeSousChapitre='80000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='80110', @Libelle='Xam 2000', @DateCreation='2010-05-06', @CodeSousChapitre='80100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='80120', @Libelle='Xam 2000 (ari)', @DateCreation='2010-05-06', @CodeSousChapitre='80100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='80130', @Libelle='Xam 7000', @DateCreation='2010-05-06', @CodeSousChapitre='80100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='80140', @Libelle='XAM 5000', @DateCreation='2010-05-18', @CodeSousChapitre='80100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='80210', @Libelle='Entourage egout', @DateCreation='2010-05-06', @CodeSousChapitre='80200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='80310', @Libelle='Talky Walky', @DateCreation='2010-05-06', @CodeSousChapitre='80300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='80320', @Libelle='Accessoire longue porté', @DateCreation='2010-05-06', @CodeSousChapitre='80300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='80330', @Libelle='Talky Walky ATEX', @DateCreation='2010-06-11', @CodeSousChapitre='80300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='80410', @Libelle='Masque autosauveteur', @DateCreation='2010-05-06', @CodeSousChapitre='80400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='80510', @Libelle='Gilet sauvetage', @DateCreation='2010-05-06', @CodeSousChapitre='80500', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='80610', @Libelle='Tripod  acces', @DateCreation='2010-05-06', @CodeSousChapitre='80600', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='80710', @Libelle='Feux tricolore', @DateCreation='2010-05-06', @CodeSousChapitre='80700', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='80810', @Libelle='Projecteur pied  500W', @DateCreation='2010-05-06', @CodeSousChapitre='80800', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='80820', @Libelle='Projecteur Siroco 2000W', @DateCreation='2010-05-06', @CodeSousChapitre='80800', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='80830', @Libelle='Projecteur LED', @DateCreation='2010-05-06', @CodeSousChapitre='80800', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='80910', @Libelle='Lampe frontale', @DateCreation='2010-05-06', @CodeSousChapitre='80900', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='81010', @Libelle='Harnais Nacelle', @DateCreation='2010-05-06', @CodeSousChapitre='81000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='81020', @Libelle='Harnais Assainissement', @DateCreation='2010-05-06', @CodeSousChapitre='81000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='81030', @Libelle='Harnais Pylone', @DateCreation='2010-05-06', @CodeSousChapitre='81000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='81110', @Libelle='Trousse TST', @DateCreation='2010-05-06', @CodeSousChapitre='81100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='81120', @Libelle='Caisse boite edf', @DateCreation='2010-05-06', @CodeSousChapitre='81100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='81210', @Libelle='Bouée', @DateCreation='2010-05-06', @CodeSousChapitre='81200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='81310', @Libelle='Perche Malt 63/90-225/420K', @DateCreation='2014-01-14', @CodeSousChapitre='81300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='90000', @Libelle='Achat Travail Hauteur', @DateCreation='2010-06-15', @CodeSousChapitre='90000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='90110', @Libelle='Echafaudage', @DateCreation='2010-05-06', @CodeSousChapitre='90100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='90210', @Libelle='Echelles', @DateCreation='2010-05-06', @CodeSousChapitre='90200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='90310', @Libelle='Escabeau 3 marches', @DateCreation='2010-05-06', @CodeSousChapitre='90300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='90320', @Libelle='Escabeau 5 marches', @DateCreation='2010-05-06', @CodeSousChapitre='90300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='90330', @Libelle='Escabeau 7 marches', @DateCreation='2010-05-06', @CodeSousChapitre='90300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='90340', @Libelle='Escabeau 9 marches', @DateCreation='2010-05-06', @CodeSousChapitre='90300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='90410', @Libelle='PIR 3/5 marches', @DateCreation='2010-05-06', @CodeSousChapitre='90400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='90420', @Libelle='PIR 4/7 marches', @DateCreation='2010-05-06', @CodeSousChapitre='90400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='90430', @Libelle='PIR 7/9 marches', @DateCreation='2010-05-06', @CodeSousChapitre='90400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='90510', @Libelle='Nacelle', @DateCreation='2010-05-06', @CodeSousChapitre='90500', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='90610', @Libelle='Echafaudage emissaire', @DateCreation='2013-04-29', @CodeSousChapitre='90600', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='100000', @Libelle='Achat Nettoyage', @DateCreation='2010-06-15', @CodeSousChapitre='100000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='100110', @Libelle='Aspirateur chantier', @DateCreation='2010-05-06', @CodeSousChapitre='100100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='100120', @Libelle='Aspirateur Amiante', @DateCreation='2013-12-18', @CodeSousChapitre='100100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='100210', @Libelle='Nettoyeur haute pression electrique', @DateCreation='2010-05-06', @CodeSousChapitre='100200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='100220', @Libelle='Nettoyeur haute pression thermique', @DateCreation='2010-05-06', @CodeSousChapitre='100200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='100310', @Libelle='Pompe a eau', @DateCreation='2010-05-06', @CodeSousChapitre='100300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='100410', @Libelle='Pompe de relevage', @DateCreation='2010-05-06', @CodeSousChapitre='100400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='100510', @Libelle='AUTOLAVEUSE', @DateCreation='2018-03-07', @CodeSousChapitre='100500', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='110000', @Libelle='Achat Gros Oeuvre', @DateCreation='2010-06-15', @CodeSousChapitre='110000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='110110', @Libelle='Barrières HERAS', @DateCreation='2010-05-06', @CodeSousChapitre='110100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='110120', @Libelle='Barrières protection fouille', @DateCreation='2010-05-06', @CodeSousChapitre='110100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='110210', @Libelle='Fonceur', @DateCreation='2010-05-06', @CodeSousChapitre='110200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='110220', @Libelle='Fusée 45', @DateCreation='2010-05-06', @CodeSousChapitre='110200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='110310', @Libelle='Moto pompe', @DateCreation='2010-05-06', @CodeSousChapitre='110300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='110410', @Libelle='Passerelles', @DateCreation='2010-05-06', @CodeSousChapitre='110400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='110510', @Libelle='Pont lourd', @DateCreation='2010-05-06', @CodeSousChapitre='110500', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='110610', @Libelle='Betonniere', @DateCreation='2010-05-06', @CodeSousChapitre='110600', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='110710', @Libelle='Etais', @DateCreation='2010-05-06', @CodeSousChapitre='110700', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='110810', @Libelle='Dameuse / pilonneuse 80kg', @DateCreation='2010-05-06', @CodeSousChapitre='110800', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='110820', @Libelle='Plaque vibrante 300 kg / Pied de mouton', @DateCreation='2010-05-06', @CodeSousChapitre='110800', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='110910', @Libelle='Aerateur d''egout', @DateCreation='2010-05-06', @CodeSousChapitre='110900', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='111010', @Libelle='Obturateur d egout', @DateCreation='2010-05-07', @CodeSousChapitre='111000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='111110', @Libelle='Pelle 2T', @DateCreation='2010-05-06', @CodeSousChapitre='111100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='111120', @Libelle='Pelle 2T5', @DateCreation='2010-05-06', @CodeSousChapitre='111100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='111130', @Libelle='Pelle 12T', @DateCreation='2010-05-06', @CodeSousChapitre='111100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='111140', @Libelle='BRH', @DateCreation='2010-05-06', @CodeSousChapitre='111100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='111210', @Libelle='Leve poteau QUATTRO', @DateCreation='2010-05-06', @CodeSousChapitre='111200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='111310', @Libelle='Tracteur', @DateCreation='2010-05-06', @CodeSousChapitre='111300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='111320', @Libelle='TRACTO PELLE', @DateCreation='2016-02-12', @CodeSousChapitre='111100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='111610', @Libelle='BLINDAGES KVL', @DateCreation='2011-01-07', @CodeSousChapitre='111600', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='120000', @Libelle='Achat Divers', @DateCreation='2010-06-15', @CodeSousChapitre='120000', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='120110', @Libelle='Meche', @DateCreation='2010-05-06', @CodeSousChapitre='120100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='120210', @Libelle='Couronne', @DateCreation='2010-05-06', @CodeSousChapitre='120200', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='120310', @Libelle='Recharge gaz', @DateCreation='2010-05-06', @CodeSousChapitre='120300', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='120410', @Libelle='Disque diamant', @DateCreation='2010-05-06', @CodeSousChapitre='120400', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='130110', @Libelle='Perceuse sur pieds', @DateCreation='2010-05-06', @CodeSousChapitre='130100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='130120', @Libelle='Plieuse', @DateCreation='2010-05-06', @CodeSousChapitre='130100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='130130', @Libelle='Presse plieuse HAWA', @DateCreation='2010-05-06', @CodeSousChapitre='130100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='130140', @Libelle='Nettoyeur peinture', @DateCreation='2010-05-06', @CodeSousChapitre='130100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='130150', @Libelle='Aspirateur de fumée', @DateCreation='2010-05-06', @CodeSousChapitre='130100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='130160', @Libelle='Cisaille', @DateCreation='2010-05-06', @CodeSousChapitre='130100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='130170', @Libelle='Secheur d air', @DateCreation='2010-05-06', @CodeSousChapitre='130100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='140110', @Libelle='Pince dymo', @DateCreation='2010-05-06', @CodeSousChapitre='140100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='140120', @Libelle='Reglophare', @DateCreation='2010-05-06', @CodeSousChapitre='140100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='140130', @Libelle='Resculteur pneu', @DateCreation='2010-05-06', @CodeSousChapitre='140100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='140140', @Libelle='Equilibreuse', @DateCreation='2010-05-06', @CodeSousChapitre='140100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='140150', @Libelle='Balayeuse', @DateCreation='2010-05-06', @CodeSousChapitre='140100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='140160', @Libelle='Concept auto diagnostic', @DateCreation='2010-05-06', @CodeSousChapitre='140100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='140170', @Libelle='Demonte pneu', @DateCreation='2010-05-06', @CodeSousChapitre='140100', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='ABOMAT', @Libelle='Abonnements Matériels', @DateCreation='2017-11-09', @CodeSousChapitre='ABOMAT', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='AMPLIROL15', @Libelle='Camion Amplirol 15 T', @DateCreation='2010-10-26', @CodeSousChapitre='PL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='B100BENNE', @Libelle='B110 Benne ', @DateCreation='2010-02-04', @CodeSousChapitre='BENNE', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='B110FOURG', @Libelle='B110 fourgon ', @DateCreation='2010-02-04', @CodeSousChapitre='FOURGON', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='BENNE9T', @Libelle='Camion benne 9T', @DateCreation='2010-05-06', @CodeSousChapitre='PL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='BERLINGO', @Libelle='Berlingo', @DateCreation='2010-02-04', @CodeSousChapitre='CTTE1', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='BOXER', @Libelle='Boxer', @DateCreation='2010-02-04', @CodeSousChapitre='FOURGON', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='BUS', @Libelle='Bus plateforme', @DateCreation='2010-02-04', @CodeSousChapitre='ENGINSPEC', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='C15', @Libelle='C15', @DateCreation='2010-02-04', @CodeSousChapitre='CTTE1', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='C2', @Libelle='C2', @DateCreation='2010-02-04', @CodeSousChapitre='VL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='C3', @Libelle='C3', @DateCreation='2010-02-04', @CodeSousChapitre='VL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='CAMAYONNPC', @Libelle='Ravitailleur NPC', @DateCreation='2010-02-04', @CodeSousChapitre='ENGINSPEC', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='CAMAYONPIF', @Libelle='Ravitailleur PIF ', @DateCreation='2010-02-04', @CodeSousChapitre='ENGINSPEC', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='CAMBOUCLE', @Libelle='Camion Boucle', @DateCreation='2011-02-23', @CodeSousChapitre='PL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='CAMDEP', @Libelle='Depanneuse', @DateCreation='2010-02-04', @CodeSousChapitre='ENGINSPEC', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='CAMPLATEF', @Libelle='Camion plateforme', @DateCreation='2010-02-04', @CodeSousChapitre='ENGINSPEC', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='CAMST', @Libelle='Camion TST', @DateCreation='2010-02-04', @CodeSousChapitre='ENGINSPEC', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='CAMTREUIL', @Libelle='Camion Treuil ', @DateCreation='2010-02-04', @CodeSousChapitre='ENGINSPEC', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='CANTER', @Libelle='Canter', @DateCreation='2010-02-04', @CodeSousChapitre='FOURGON', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='CLIO ', @Libelle='CLIO ', @DateCreation='2010-02-04', @CodeSousChapitre='VL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='CONVOY', @Libelle='Convoyage de véhicules IDF', @DateCreation='2016-07-06', @CodeSousChapitre='CONVOY', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='DAILYBENNE', @Libelle='Daily benne', @DateCreation='2010-02-04', @CodeSousChapitre='BENNE', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='DAILYFOURG', @Libelle='Daily Fourgon ', @DateCreation='2010-02-04', @CodeSousChapitre='FOURGON', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='DAILYGRUE', @Libelle='Daily benne grue', @DateCreation='2010-02-04', @CodeSousChapitre='BENNEGRUE', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='DUCATO ', @Libelle='Ducato ', @DateCreation='2010-02-04', @CodeSousChapitre='FOURGON', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='EXPERT', @Libelle='Expert ', @DateCreation='2010-02-04', @CodeSousChapitre='CTTE2', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='FOURGON11', @Libelle='Fourgon 11m', @DateCreation='2010-02-04', @CodeSousChapitre='NACELLE', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='GEOLOC', @Libelle='Abonnements géolocalisation DAM S', @DateCreation='2014-05-19', @CodeSousChapitre='GEOLOC', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='GRUE10T', @Libelle='10T Grue ', @DateCreation='2010-02-04', @CodeSousChapitre='PL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='GRUE15T', @Libelle='15T Grue', @DateCreation='2010-02-04', @CodeSousChapitre='PL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='GYROPODE', @Libelle='GYROPODE', @DateCreation='2018-04-11', @CodeSousChapitre='GY', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='JUMPER', @Libelle='Jumper', @DateCreation='2010-02-04', @CodeSousChapitre='FOURGON', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='JUMPY', @Libelle='Jumpy', @DateCreation='2010-02-04', @CodeSousChapitre='CTTE2', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='KANGOO ', @Libelle='Kangoo', @DateCreation='2010-02-04', @CodeSousChapitre='CTTE1', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='LEVEPOTEAU', @Libelle='CAMION TARRIERE LEVE POTEAUX', @DateCreation='2016-02-12', @CodeSousChapitre='ENGINSPEC', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='LOGAN', @Libelle='DACIA LOGAN', @DateCreation='2013-07-18', @CodeSousChapitre='VL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='MASCOTT', @Libelle='Mascott ', @DateCreation='2010-02-04', @CodeSousChapitre='FOURGON', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='MASTER', @Libelle='Master ', @DateCreation='2010-02-04', @CodeSousChapitre='FOURGON', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='MAXITYBENN', @Libelle='Maxity Benne', @DateCreation='2010-05-06', @CodeSousChapitre='BENNE', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='MEGANE', @Libelle='MEGANE société', @DateCreation='2016-02-12', @CodeSousChapitre='VL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='NACELLEPL', @Libelle='Nacelle PL', @DateCreation='2010-02-04', @CodeSousChapitre='PL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='NEMO', @Libelle='Nemo', @DateCreation='2011-06-16', @CodeSousChapitre='CTTE1', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='PARTNER', @Libelle='Partner', @DateCreation='2010-02-04', @CodeSousChapitre='CTTE1', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='PICASSO', @Libelle='Picasso', @DateCreation='2010-02-04', @CodeSousChapitre='VL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='PICK UP ', @Libelle='Pick up ', @DateCreation='2010-02-04', @CodeSousChapitre='VL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='PLATEAU16', @Libelle='Plateau 16m', @DateCreation='2010-02-04', @CodeSousChapitre='NACELLE', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='PORTPOTEAU', @Libelle='Porte poteau', @DateCreation='2010-02-04', @CodeSousChapitre='PL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='SAXO', @Libelle='Saxo', @DateCreation='2010-02-04', @CodeSousChapitre='VL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='SCUDO', @Libelle='Scudo ', @DateCreation='2010-02-04', @CodeSousChapitre='CTTE2', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='SPRINTBENN', @Libelle='Sprinter Benne ', @DateCreation='2010-02-04', @CodeSousChapitre='BENNE', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='SPRINTFOUR', @Libelle='Sprinter', @DateCreation='2010-02-04', @CodeSousChapitre='FOURGON', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='SPRINTGRUE', @Libelle='Sprinter benne grue', @DateCreation='2010-02-04', @CodeSousChapitre='BENNEGRUE', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='TOPY10', @Libelle='Topy 10', @DateCreation='2010-02-04', @CodeSousChapitre='NACELLE', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='TRACSEMI', @Libelle='TRACTEUR DE SEMI REMORQUE', @DateCreation='2016-02-12', @CodeSousChapitre='PL', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='TRAFIC', @Libelle='Traffic', @DateCreation='2010-02-04', @CodeSousChapitre='CTTE2', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='TRICYCLE', @Libelle='Tricycle à Moteur', @DateCreation='2018-03-22', @CodeSousChapitre='TM', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='VITO', @Libelle='VITO', @DateCreation='2016-10-05', @CodeSousChapitre='CTTE2', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='VPBUDGET', @Libelle='VP Budget', @DateCreation='2010-02-04', @CodeSousChapitre='VP', @TypeRessourceCode='MAT', @Active=1;
EXEC #SET_FRED_RESSOURCE @Code='VPREGULE', @Libelle='VP régulé ', @DateCreation='2010-02-04', @CodeSousChapitre='VP', @TypeRessourceCode='MAT', @Active=1;



-- Fin insertion


-- Update de la colonne AuteurCreationId

DECLARE @auteurCreationId int;
SET @auteurCreationId = (SELECT UtilisateurId FROM FRED_UTILISATEUR WHERE Login LIKE 'fred_ie');

UPDATE Fr
SET 
	Fr.AuteurCreationId = @auteurCreationId
FROM FRED_RESSOURCE Fr
inner join FRED_SOUS_CHAPITRE Frs on Fr.SousChapitreId = Frs.SousChapitreId
inner join FRED_CHAPITRE c on Frs.ChapitreId = c.ChapitreId
WHERE c.ChapitreId in (SELECT ChapitreId from FRED_CHAPITRE WHERE CODE IN ('EIMATERIEL', 'EIROULANT'))

-- Fin update de la colonne AuteurCreationId

DROP PROCEDURE #SET_FRED_RESSOURCE