-- Alimentation de la table FRED_SOUS_CHAPITRE

-- NOTE JNE : Il existe déjà des PS pour faire ça : Fred_ToolBox, merci de les utiliser


CREATE PROCEDURE #SET_FRED_SOUS_CHAPITRE
	@Code nvarchar(20), 
	@Libelle nvarchar(500), 
	@DateCreation datetime, 
	@CodeChapitre nvarchar(20)

AS
BEGIN
	IF NOT EXISTS ( SELECT 1 FROM FRED_SOUS_CHAPITRE WHERE Code = @Code)
	BEGIN
		declare @chapitreId int;
		set @chapitreId = (SELECT ChapitreId FROM FRED_CHAPITRE WHERE Code LIKE @CodeChapitre);

		INSERT INTO FRED_SOUS_CHAPITRE (Code, Libelle, DateCreation, ChapitreId)
		VALUES (@Code, @Libelle, @DateCreation, @chapitreId)
	END
END
GO

-- Insertion des données

EXEC #SET_FRED_SOUS_CHAPITRE @Code='10000', @Libelle='Famille Mesure', @DateCreation='2010-06-14', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='10100', @Libelle='Analyseur d énergie', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='10200', @Libelle='Analyseur enregistreur harmonique', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='10300', @Libelle='Caméra thermo graphique', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='10400', @Libelle='Contrôleur bipolaire HTA', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='10500', @Libelle='Tellurohmètre contrôleur de terre ', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='10600', @Libelle='Megohmetre', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='10600', @Libelle='Megohmetre', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='10700', @Libelle='Contrôleur VAT', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='10800', @Libelle='Appareil photo numérique ', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='10900', @Libelle='Détecteur canalisations ecchoM', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='11000', @Libelle='Détecteur de câbles', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='11100', @Libelle='détecteur de métaux', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='11200', @Libelle='Dynamomètre de traction', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='11300', @Libelle='Luxmètre', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='11400', @Libelle='Lasermetre', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='11500', @Libelle='Multimètre', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='11600', @Libelle='Multi testeur', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='11700', @Libelle='Ohmmètre de boucle ', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='11800', @Libelle='Oscilloscope', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='11900', @Libelle='Pince ampéremétrique', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='12000', @Libelle='Topomètre / odomètre', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='12100', @Libelle='Vérificateur de réseaux info wirescope', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='12200', @Libelle='Sonometre', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='12300', @Libelle='Cle dynamometrique', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='12400', @Libelle='Reflectometre', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='12500', @Libelle='Anemometre', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='12600', @Libelle='Spectrosonde', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='12700', @Libelle='Calibrateur 4/20', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='13000', @Libelle=' Mesureur de câble', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='13100', @Libelle='Thermohygrometre ', @DateCreation='2010-02-05', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='13200', @Libelle='Mesureur de vibration', @DateCreation='2010-02-05', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='13300', @Libelle='Dielectrometre', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='13400', @Libelle='Calibreur de pression', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='13500', @Libelle='PRELEVEUR D''ECHANTILLONS ASSAINISSEMENT', @DateCreation='2015-10-26', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='20000', @Libelle='Famille Perfo Sciage', @DateCreation='2010-06-14', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='20100', @Libelle='Visseuse', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='20200', @Libelle='Meuleuse', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='20300', @Libelle='Perceuse', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='20400', @Libelle='Perforateur', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='20500', @Libelle='Ponceuse', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='20600', @Libelle='Scie circulaire', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='20700', @Libelle='Scie sabre', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='20800', @Libelle='Scie sauteuse', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='20900', @Libelle='Tronçonneuse', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='21000', @Libelle='Burineur', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='21100', @Libelle='Carotteuse', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='21200', @Libelle='Brise béton ', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='21300', @Libelle='Marteau piqueur autonome', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='21400', @Libelle='Marteau piqueur ', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='21500', @Libelle='Rainureuse', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='21600', @Libelle='Scie à sol', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='30000', @Libelle='Famille Installation chantier', @DateCreation='2010-06-14', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='30100', @Libelle='Coffre à outillage ', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='30200', @Libelle='Coffret électrique de chantier', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='30300', @Libelle='Frigo', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='30400', @Libelle='Glacière', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='30500', @Libelle='Micro ondes', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='30600', @Libelle='Tente', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='30700', @Libelle='Etabli   eteau', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='30800', @Libelle='Vestiaire', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='30900', @Libelle='Compresseur', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='31000', @Libelle='Aerotherme', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='31100', @Libelle='Groupe électrogène', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='31200', @Libelle='Climatisation', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='31300', @Libelle='Sono/multimedia', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='31400', @Libelle='Bungalows containers', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='31500', @Libelle='Benne', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='31600', @Libelle='Compacteur', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='31700', @Libelle='Extracteur', @DateCreation='2010-08-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='31900', @Libelle='Cone panneau signalisation', @DateCreation='2012-10-09', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='40000', @Libelle='Famille Manut Transport', @DateCreation='2010-06-14', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='40100', @Libelle='Chariot diable', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='40200', @Libelle='hOME TRAINER', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='40300', @Libelle='Chariot dérouleur de touret', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='40400', @Libelle='Diables élévateurs jumelés', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='40500', @Libelle='vérin de déroulage', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='40600', @Libelle='Porte charge monte escaliers', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='40700', @Libelle='Rouleurs de fortes charges', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='40800', @Libelle='Dérouleur de gaine', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='40900', @Libelle='Galet', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='41000', @Libelle='Palans', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='41100', @Libelle='Tirefort', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='41200', @Libelle='Treuil', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='41300', @Libelle='Transpalette', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='41400', @Libelle='Aiguille fibre', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='41500', @Libelle='Leve tampon', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='41600', @Libelle='Rampe ', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='41700', @Libelle='Porteuse pneumatique', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='41800', @Libelle='Dérouleuse', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='41900', @Libelle='Chariot élévateur', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='42000', @Libelle='Remorque', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='43000', @Libelle='Potence', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='50000', @Libelle='Famille Outillage', @DateCreation='2010-06-14', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='50100', @Libelle='Cintreuse ', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='50200', @Libelle='Filliere', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='50300', @Libelle='Enrouleur rallonge', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='50400', @Libelle='Clé a choc', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='50500', @Libelle='Cloueuse', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='50600', @Libelle='Coffret à dénuder HT/BT', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='50700', @Libelle='Coffret à douilles', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='50800', @Libelle='Coffret à cliquet', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='50900', @Libelle='Coffret de mise à la terre " BT aérien " ', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='51000', @Libelle='Etiqueteuse', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='60000', @Libelle='Famille Soudure', @DateCreation='2010-06-14', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='60100', @Libelle='Poste à souder ', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='60200', @Libelle='Chariot chalumeau découpeur', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='60300', @Libelle='Chariot chalumeau ', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='60400', @Libelle='Poste TIG', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='60500', @Libelle='Soudeuse bobine fb opt. ', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='60600', @Libelle='Decap thermique', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='60700', @Libelle='Furet thermo', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='60800', @Libelle='Soudure ensemble Flasch GAZ', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='70000', @Libelle='Famille Sertissage', @DateCreation='2010-06-14', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='70100', @Libelle='Pince à sertir mamuelle', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='70200', @Libelle='Pince à sertir électrique ', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='70300', @Libelle='Pompe à sertir (20T)', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='70400', @Libelle='Coffret sertissage cuivre ', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='70500', @Libelle='Coffret sertissage alu', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='70600', @Libelle='Coupe câble ', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='70700', @Libelle='Emporte pièces', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='70800', @Libelle='Presse à sertir', @DateCreation='2016-11-22', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='70900', @Libelle='groupe hydraulique pour presse à sertir', @DateCreation='2016-11-22', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='80000', @Libelle='Famille Securité', @DateCreation='2010-06-14', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='80100', @Libelle='Détecteur gaz', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='80200', @Libelle='Entourage egout', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='80300', @Libelle='Emetteur récepteur', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='80400', @Libelle='Masque autosauveteur', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='80500', @Libelle='Gilet sauvetage', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='80600', @Libelle='Tripod   acces', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='80700', @Libelle='Feux tricolore', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='80800', @Libelle='Projecteur', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='80900', @Libelle='Lampe frontale', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='81000', @Libelle='Harnais', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='81100', @Libelle='Trousse TST', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='81200', @Libelle='Bouée', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='81300', @Libelle='Perche Malt 63/90-225/420K', @DateCreation='2014-01-14', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='90000', @Libelle='Famille Travail Hauteur', @DateCreation='2010-06-14', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='90100', @Libelle='Echafaudage', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='90200', @Libelle='Echelles', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='90300', @Libelle='Escabeau', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='90400', @Libelle='PIR ', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='90500', @Libelle='Nacelle', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='90600', @Libelle='Echafaudage emissaire', @DateCreation='2013-04-29', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='100000', @Libelle='Famille Nettoyage', @DateCreation='2010-06-14', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='100100', @Libelle='Aspirateur chantier', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='100200', @Libelle='Nettoyeur haute pression ', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='100300', @Libelle='Pompe à eau ', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='100400', @Libelle='Pompe de relevage ', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='100500', @Libelle='AUTOLAVEUSE', @DateCreation='2018-03-07', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='110000', @Libelle='Famille Gros Oeuvre', @DateCreation='2010-06-14', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='110100', @Libelle='Barrières', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='110200', @Libelle='Fonceur', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='110300', @Libelle='Moto pompe ', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='110400', @Libelle='Passerelles', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='110500', @Libelle='Pont lourd', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='110600', @Libelle='Betonniere', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='110700', @Libelle='Etais', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='110800', @Libelle='Compactage', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='110900', @Libelle='Aerateur d''egout', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='111000', @Libelle='Obturateur d egout', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='111100', @Libelle='Pelles ', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='111200', @Libelle='Leve poteau QUATTRO', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='111300', @Libelle='Tracteur', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='111600', @Libelle='BLINDAGES KVL', @DateCreation='2011-01-07', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='120000', @Libelle='Famille Divers', @DateCreation='2010-06-14', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='120100', @Libelle='Meche  ', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='120200', @Libelle='Couronne ', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='120300', @Libelle='Recharge gaz', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='120400', @Libelle='Disque diamant', @DateCreation='2010-02-04', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='130100', @Libelle='Atelier serrurerie peinture', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='140100', @Libelle='Atelier Garage', @DateCreation='2010-05-06', @CodeChapitre='EIMATERIEL';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='ABOMAT', @Libelle='Abonnements Matériels', @DateCreation='2017-11-09', @CodeChapitre='EIROULANT';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='BENNE', @Libelle='FOURGONS BENNE', @DateCreation='2010-02-04', @CodeChapitre='EIROULANT';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='BENNEGRUE', @Libelle='FOURGONS BENNE GRUE ', @DateCreation='2010-02-04', @CodeChapitre='EIROULANT';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='CONVOY', @Libelle='Convoyage de véhicules IDF', @DateCreation='2016-07-06', @CodeChapitre='EIROULANT';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='CTTE1', @Libelle='Camionettes léger1', @DateCreation='2010-02-04', @CodeChapitre='EIROULANT';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='CTTE2', @Libelle='Camionettes Utilitaires2', @DateCreation='2010-02-04', @CodeChapitre='EIROULANT';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='ENGINSPEC', @Libelle='ENGINS SPECIAUX PL', @DateCreation='2010-02-04', @CodeChapitre='EIROULANT';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='FOURGON', @Libelle='FOURGONS', @DateCreation='2010-02-04', @CodeChapitre='EIROULANT';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='GEOLOC', @Libelle='Abonnements géolocalisation DAM S', @DateCreation='2014-05-19', @CodeChapitre='EIROULANT';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='GY', @Libelle='Gyropode', @DateCreation='2018-04-11', @CodeChapitre='EIROULANT';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='LOCATEXTM', @Libelle='Location extérieure MO Chauffeurs', @DateCreation='2010-05-06', @CodeChapitre='EIROULANT';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='LOCEXTCTTE', @Libelle='Loc Ext - CTTE ', @DateCreation='2010-02-04', @CodeChapitre='EIROULANT';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='LOCEXTNAC', @Libelle='Loc Ext - Nacelles', @DateCreation='2010-02-04', @CodeChapitre='EIROULANT';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='LOCEXTPL', @Libelle='Loc Ext - PL ', @DateCreation='2010-02-04', @CodeChapitre='EIROULANT';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='LOCEXTVL', @Libelle='Loc Ext - VL', @DateCreation='2010-02-04', @CodeChapitre='EIROULANT';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='NACELLE', @Libelle='NACELLES VL', @DateCreation='2010-02-04', @CodeChapitre='EIROULANT';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='PL', @Libelle='Poids lourds', @DateCreation='2010-12-02', @CodeChapitre='EIROULANT';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='TM', @Libelle='Tricycle à Moteur', @DateCreation='2018-03-22', @CodeChapitre='EIROULANT';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='VL', @Libelle='Véhicule légers', @DateCreation='2010-02-04', @CodeChapitre='EIROULANT';
EXEC #SET_FRED_SOUS_CHAPITRE @Code='VP', @Libelle='Véhicules particuliers', @DateCreation='2010-02-04', @CodeChapitre='EIROULANT';

-- Fin insertion

-- Update de la colonne AuteurCreatinId

DECLARE @auteurCreationId int;
SET @auteurCreationId = (SELECT UtilisateurId FROM FRED_UTILISATEUR WHERE Login LIKE 'fred_ie');

UPDATE 
	FRED_SOUS_CHAPITRE
SET AuteurCreationId = @auteurCreationId
WHERE ChapitreId in (SELECT ChapitreId from FRED_CHAPITRE WHERE CODE IN ('EIMATERIEL', 'EIROULANT') )

-- Fin Update de la colonne AuteurCreatinId


DROP PROCEDURE #SET_FRED_SOUS_CHAPITRE

