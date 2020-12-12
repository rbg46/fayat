﻿-- Regroupe la création de tous les chapitres, avec leurs mises à jour
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFES',@Code='MOEOP',@Libelle='Main d œuvre Opérationnelle'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFES',@Code='MOEADM',@Libelle='Main d œuvre Administrative'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFES',@Code='EIMATERIEL',@Libelle='EI Matériel et outillage'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFES',@Code='EIROULANT',@Libelle='Véhicule'
EXEC Fred_ToolBox_Chapitre @GroupeCode='GFES',@Code='EIOUTILLAGE',@Libelle='Outillage'

-- Regroupe la création de tous les sous-chapitres, avec leurs mises à jour
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='10000',@Libelle='Famille Mesure'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='10100',@Libelle='Analyseur d énergie'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='10200',@Libelle='Analyseur enregistreur harmonique'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='10300',@Libelle='Caméra thermo graphique'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='10400',@Libelle='Contrôleur bipolaire HTA'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='10500',@Libelle='Tellurohmètre contrôleur de terre '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='10600',@Libelle='Megohmetre'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='10600',@Libelle='Megohmetre'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='10700',@Libelle='Contrôleur VAT'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='10800',@Libelle='Appareil photo numérique '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='10900',@Libelle='Détecteur canalisations ecchoM'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='11000',@Libelle='Détecteur de câbles'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='11100',@Libelle='détecteur de métaux'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='11200',@Libelle='Dynamomètre de traction'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='11300',@Libelle='Luxmètre'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='11400',@Libelle='Lasermetre'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='11500',@Libelle='Multimètre'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='11600',@Libelle='Multi testeur'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='11700',@Libelle='Ohmmètre de boucle '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='11800',@Libelle='Oscilloscope'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='11900',@Libelle='Pince ampéremétrique'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='12000',@Libelle='Topomètre / odomètre'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='12100',@Libelle='Vérificateur de réseaux info wirescope'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='12200',@Libelle='Sonometre'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='12300',@Libelle='Cle dynamometrique'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='12400',@Libelle='Reflectometre'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='12500',@Libelle='Anemometre'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='12600',@Libelle='Spectrosonde'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='12700',@Libelle='Calibrateur 4/20'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='13000',@Libelle=' Mesureur de câble'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='13100',@Libelle='Thermohygrometre '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='13200',@Libelle='Mesureur de vibration'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='13300',@Libelle='Dielectrometre'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='13400',@Libelle='Calibreur de pression'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='13500',@Libelle='PRELEVEUR D ECHANTILLONS ASSAINISSEMENT'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='20000',@Libelle='Famille Perfo Sciage'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='20100',@Libelle='Visseuse'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='20200',@Libelle='Meuleuse'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='20300',@Libelle='Perceuse'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='20400',@Libelle='Perforateur'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='20500',@Libelle='Ponceuse'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='20600',@Libelle='Scie circulaire'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='20700',@Libelle='Scie sabre'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='20800',@Libelle='Scie sauteuse'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='20900',@Libelle='Tronçonneuse'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='21000',@Libelle='Burineur'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='21100',@Libelle='Carotteuse'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='21200',@Libelle='Brise béton '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='21300',@Libelle='Marteau piqueur autonome'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='21400',@Libelle='Marteau piqueur '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='21500',@Libelle='Rainureuse'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='21600',@Libelle='Scie à sol'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIMATERIEL',@Code='30000',@Libelle='Famille Installation chantier'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='30100',@Libelle='Coffre à outillage '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='30200',@Libelle='Coffret électrique de chantier'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='30300',@Libelle='Frigo'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='30400',@Libelle='Glacière'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='30500',@Libelle='Micro ondes'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='30600',@Libelle='Tente'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='30700',@Libelle='Etabli   eteau'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='30800',@Libelle='Vestiaire'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIMATERIEL',@Code='30900',@Libelle='Compresseur'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='31000',@Libelle='Aerotherme'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='31100',@Libelle='Groupe électrogène'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='31200',@Libelle='Climatisation'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='31300',@Libelle='Sono/multimedia'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIMATERIEL',@Code='31400',@Libelle='Bungalows containers'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='31500',@Libelle='Benne'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='31600',@Libelle='Compacteur'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='31700',@Libelle='Extracteur'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='31900',@Libelle='Cone panneau signalisation'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='40000',@Libelle='Famille Manut Transport'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='40100',@Libelle='Chariot diable'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='40200',@Libelle='hOME TRAINER'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='40300',@Libelle='Chariot dérouleur de touret'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='40400',@Libelle='Diables élévateurs jumelés'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='40500',@Libelle='vérin de déroulage'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='40600',@Libelle='Porte charge monte escaliers'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='40700',@Libelle='Rouleurs de fortes charges'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='40800',@Libelle='Dérouleur de gaine'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='40900',@Libelle='Galet'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='41000',@Libelle='Palans'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='41100',@Libelle='Tirefort'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIMATERIEL',@Code='41200',@Libelle='Treuil'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='41300',@Libelle='Transpalette'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='41400',@Libelle='Aiguille fibre'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='41500',@Libelle='Leve tampon'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='41600',@Libelle='Rampe '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='41700',@Libelle='Porteuse pneumatique'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIMATERIEL',@Code='41800',@Libelle='Dérouleuse'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIMATERIEL',@Code='41900',@Libelle='Chariot élévateur'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIMATERIEL',@Code='42000',@Libelle='Remorque'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='43000',@Libelle='Potence'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='50000',@Libelle='Famille Outillage'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='50100',@Libelle='Cintreuse '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='50200',@Libelle='Filliere'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='50300',@Libelle='Enrouleur rallonge'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='50400',@Libelle='Clé a choc'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='50500',@Libelle='Cloueuse'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='50600',@Libelle='Coffret à dénuder HT/BT'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='50700',@Libelle='Coffret à douilles'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='50800',@Libelle='Coffret à cliquet'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='50900',@Libelle='Coffret de mise à la terre " BT aérien " '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='51000',@Libelle='Etiqueteuse'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='60000',@Libelle='Famille Soudure'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='60100',@Libelle='Poste à souder '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='60200',@Libelle='Chariot chalumeau découpeur'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='60300',@Libelle='Chariot chalumeau '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='60400',@Libelle='Poste TIG'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='60500',@Libelle='Soudeuse bobine fb opt. '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='60600',@Libelle='Decap thermique'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='60700',@Libelle='Furet thermo'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='60800',@Libelle='Soudure ensemble Flasch GAZ'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='70000',@Libelle='Famille Sertissage'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='70100',@Libelle='Pince à sertir mamuelle'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='70200',@Libelle='Pince à sertir électrique '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='70300',@Libelle='Pompe à sertir (20T)'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='70400',@Libelle='Coffret sertissage cuivre '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='70500',@Libelle='Coffret sertissage alu'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='70600',@Libelle='Coupe câble '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='70700',@Libelle='Emporte pièces'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='70800',@Libelle='Presse à sertir'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='70900',@Libelle='groupe hydraulique pour presse à sertir'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='80000',@Libelle='Famille Securité'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='80100',@Libelle='Détecteur gaz'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='80200',@Libelle='Entourage egout'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='80300',@Libelle='Emetteur récepteur'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='80400',@Libelle='Masque autosauveteur'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='80500',@Libelle='Gilet sauvetage'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='80600',@Libelle='Tripod   acces'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='80700',@Libelle='Feux tricolore'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='80800',@Libelle='Projecteur'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='80900',@Libelle='Lampe frontale'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='81000',@Libelle='Harnais'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='81100',@Libelle='Trousse TST'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='81200',@Libelle='Bouée'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='81300',@Libelle='Perche Malt 63/90-225/420K'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='90000',@Libelle='Famille Travail Hauteur'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='90100',@Libelle='Echafaudage'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='90200',@Libelle='Echelles'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='90300',@Libelle='Escabeau'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='90400',@Libelle='PIR '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIMATERIEL',@Code='90500',@Libelle='Nacelle'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='90600',@Libelle='Echafaudage emissaire'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='100000',@Libelle='Famille Nettoyage'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='100100',@Libelle='Aspirateur chantier'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='100200',@Libelle='Nettoyeur haute pression '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='100300',@Libelle='Pompe à eau '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='100400',@Libelle='Pompe de relevage '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='100500',@Libelle='AUTOLAVEUSE'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='110000',@Libelle='Famille Gros Oeuvre'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='110100',@Libelle='Barrières'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='110200',@Libelle='Fonceur'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='110300',@Libelle='Moto pompe '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='110400',@Libelle='Passerelles'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='110500',@Libelle='Pont lourd'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='110600',@Libelle='Betonniere'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='110700',@Libelle='Etais'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='110800',@Libelle='Compactage'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='110900',@Libelle='Aerateur d egout'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='111000',@Libelle='Obturateur d egout'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIMATERIEL',@Code='111100',@Libelle='Pelles '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIMATERIEL',@Code='111200',@Libelle='Leve poteau QUATTRO'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIMATERIEL',@Code='111300',@Libelle='Tracteur'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='111600',@Libelle='BLINDAGES KVL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='120000',@Libelle='Famille Divers'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='120100',@Libelle='Meche  '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='120200',@Libelle='Couronne '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='120300',@Libelle='Recharge gaz'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='120400',@Libelle='Disque diamant'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='130100',@Libelle='Atelier serrurerie peinture'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIOUTILLAGE',@Code='140100',@Libelle='Atelier Garage'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIROULANT',@Code='ABOMAT',@Libelle='Abonnements Matériels'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIROULANT',@Code='BENNE',@Libelle='FOURGONS BENNE'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIROULANT',@Code='BENNEGRUE',@Libelle='FOURGONS BENNE GRUE '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIROULANT',@Code='CONVOY',@Libelle='Convoyage de véhicules IDF'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIROULANT',@Code='CTTE1',@Libelle='Camionettes léger1'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIROULANT',@Code='CTTE2',@Libelle='Camionettes Utilitaires2'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIROULANT',@Code='ENGINSPEC',@Libelle='ENGINS SPECIAUX PL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIROULANT',@Code='FOURGON',@Libelle='FOURGONS'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIROULANT',@Code='GEOLOC',@Libelle='Abonnements géolocalisation DAM S'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIROULANT',@Code='GY',@Libelle='Gyropode'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIROULANT',@Code='LOCATEXTM',@Libelle='Location extérieure MO Chauffeurs'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIROULANT',@Code='LOCEXTCTTE',@Libelle='Loc Ext - CTTE '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIROULANT',@Code='LOCEXTNAC',@Libelle='Loc Ext - Nacelles'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIROULANT',@Code='LOCEXTPL',@Libelle='Loc Ext - PL '
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIROULANT',@Code='LOCEXTVL',@Libelle='Loc Ext - VL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIROULANT',@Code='NACELLE',@Libelle='NACELLES VL'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIROULANT',@Code='PL',@Libelle='Poids lourds'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIROULANT',@Code='TM',@Libelle='Tricycle à Moteur'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIROULANT',@Code='VL',@Libelle='Véhicule légers'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='EIROULANT',@Code='VP',@Libelle='Véhicules particuliers'

EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='MOEADM',@Code='001',@Libelle='Moyen/Achat/Atelier'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='MOEADM',@Code='002',@Libelle='Compta. et Gestion'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='MOEOP',@Code='003',@Libelle='Chantier Travaux'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='MOEADM',@Code='004',@Libelle='RH/Paie/QSE/Juriste'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='MOEOP',@Code='005',@Libelle='Ch.affaire/sect./gpe'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='MOEOP',@Code='006',@Libelle='Commercial'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='MOEOP',@Code='007',@Libelle='Direction Opération.'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='MOEOP',@Code='008',@Libelle='Bureau d études'
EXEC Fred_ToolBox_SousChapitre @GroupeCode='GFES',@ChapitreCode='MOEADM',@Code='009',@Libelle='Direction Générale'