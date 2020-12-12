-- Suppression de toutes les agences de rattachement
UPDATE FRED_ETABLISSEMENT_PAIE SET AgenceRattachementId=null
WHERE EtablissementPaieId in (
	SELECT ep.EtablissementPaieId
	FROM FRED_ETABLISSEMENT_PAIE ep inner join FRED_SOCIETE s on ep.SocieteId=s.SocieteId
		inner join FRED_GROUPE g on s.GroupeId=g.GroupeId
	WHERE g.Code='GFES')

-- Suppression des références au personnel
UPDATE FRED_PERSONNEL SET EtablissementPayeId=null
WHERE EtablissementPayeId in (
	SELECT ep.EtablissementPaieId
	FROM FRED_ETABLISSEMENT_PAIE ep inner join FRED_SOCIETE s on ep.SocieteId=s.SocieteId
		inner join FRED_GROUPE g on s.GroupeId=g.GroupeId
	WHERE g.Code='GFES'
		and (SELECT s.Code + ';' + ep.Code) in ('E001;12','E001;13','E001;26','E001;29','E001;41','E002;21','E003;10','E003;11','E006;10','E006;11','E007;10','E010;90','E015;40','E016;13'))

UPDATE FRED_PERSONNEL SET EtablissementRattachementId=null
WHERE EtablissementRattachementId in (
	SELECT ep.EtablissementPaieId
	FROM FRED_ETABLISSEMENT_PAIE ep inner join FRED_SOCIETE s on ep.SocieteId=s.SocieteId
		inner join FRED_GROUPE g on s.GroupeId=g.GroupeId
	WHERE g.Code='GFES'
		and (SELECT s.Code + ';' + ep.Code) in ('E001;12','E001;13','E001;26','E001;29','E001;41','E002;21','E003;10','E003;11','E006;10','E006;11','E007;10','E010;90','E015;40','E016;13'))

DELETE FROM FRED_ETABLISSEMENT_PAIE WHERE EtablissementPaieId in (
	SELECT ep.EtablissementPaieId
	FROM FRED_ETABLISSEMENT_PAIE ep inner join FRED_SOCIETE s on ep.SocieteId=s.SocieteId
		inner join FRED_GROUPE g on s.GroupeId=g.GroupeId
	WHERE g.Code='GFES'
		and (SELECT s.Code + ';' + ep.Code) in ('E001;12','E001;13','E001;26','E001;29','E001;41','E002;21','E003;10','E003;11','E006;10','E006;11','E007;10','E010;90','E015;40','E016;13'))

-- Mise à jour des établissements de paie
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E001',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='09',@Libelle='SATELEC DG',@Adresse='24 ave du Général de Gaulle',@CodePostal='91170',@Ville='VIRY-CHATILLON',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E001',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='10',@Libelle='SATELEC VIRY',@Adresse='24 ave du Général de Gaulle',@CodePostal='91170',@Ville='VIRY-CHATILLON',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E001',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='11',@Libelle='SATELEC ANTONY',@Adresse='3 rue Henri Poincaré',@CodePostal='92160',@Ville='ANTONY',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E001',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='14',@Libelle='SATELEC BAGNOLET',@Adresse='73 rue des Rigondes',@CodePostal='93170',@Ville='BAGNOLET',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E001',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='15',@Libelle='SATELEC BONNEUIL',@Adresse='4 ave des Marronniers',@CodePostal='94380',@Ville='BONNEUIL SUR MARNE',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E001',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='16',@Libelle='SATELEC CESSON',@Adresse='225 ave de l''Europe',@CodePostal='77240',@Ville='VERT SAINT DENIS',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E001',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='17',@Libelle='SATELEC NANTERRE',@Adresse='85 Rue des hautes Patures',@CodePostal='92000',@Ville='NANTERRE',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E001',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='18',@Libelle='SATELEC POWER NANT.',@Adresse='85 Rue des hautes Patures',@CodePostal='92000',@Ville='NANTERRE',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E001',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='20',@Libelle='SATELEC CUINCY',@Adresse='945 rue du Faubourg',@CodePostal='59553',@Ville='CUINCY',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E001',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='21',@Libelle='SATELEC TRITH',@Adresse='14  ZA Les Poutrelles',@CodePostal='59125',@Ville='TRITH SAINT LEGER',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E001',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='22',@Libelle='SATELEC TOURCOING',@Adresse='59  Chaussée Berthelot',@CodePostal='59200',@Ville='TOURCOING',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E001',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='23',@Libelle='SATELEC GDE SYNTHE',@Adresse='17 rue de l''Abbé Grégoire',@CodePostal='59760',@Ville='GRANDE-SYNTHE',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E001',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='24',@Libelle='SATELEC HENIN BT',@Adresse='141 bld Edouard Branly',@CodePostal='62110',@Ville='HENIN-BEAUMONT',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E001',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='25',@Libelle='SATELEC MONTIGNY',@Adresse='4 rue Saussaies-aux-Dames',@CodePostal='57950',@Ville='MONTIGNY-LES-METZ',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E001',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='27',@Libelle='SATELEC POULAINVILLE',@Adresse='0 rue Marius Morel',@CodePostal='80260',@Ville='POULAINVILLE',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E001',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='28',@Libelle='SATELEC ST ANDRE',@Adresse='10 RUE GUSTAVE EIFFEL',@CodePostal='10120',@Ville='SAINT ANDRE LES VERGERS',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E001',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='30',@Libelle='SATELEC PACA',@Adresse='68  Parc de l''Argile',@CodePostal='06370',@Ville='MOUANS-SARTOUX',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E001',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='40',@Libelle='SATELEC AQUITAINE',@Adresse='27 rue de Fleurenne',@CodePostal='33290',@Ville='BLANQUEFORT',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E001',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='42',@Libelle='SATELEC BISCARROSSE',@Adresse='175 rue Forestière',@CodePostal='40600',@Ville='BISCARROSSE',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E001',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='69',@Libelle='SATELEC LYON',@Adresse='2  Chemin du Genie',@CodePostal='69200',@Ville='VENISSIEUX',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E001',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='2A',@Libelle='SATELEC ALLONNE',@Adresse='115 rue des quarantes Mines',@CodePostal='60000',@Ville='ALLONNE',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E001',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='2B',@Libelle='SATELEC ST PAUL',@Adresse='3 Ave Piton Batard',@CodePostal='97460',@Ville='SAINT PAUL',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E002',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='10',@Libelle='SEMERU VIRY',@Adresse='34 RUE CHARLES PIKETTY',@CodePostal='91170',@Ville='VIRY-CHATILLON',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E002',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='11',@Libelle='SEMERU ANTONY',@Adresse='3 RUE HENRI POINCARE',@CodePostal='92160',@Ville='ANTONY',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E002',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='12',@Libelle='SEMERU BONNEUIL',@Adresse='4 AVE DES MARRONNIERS',@CodePostal='94380',@Ville='BONNEUIL SUR MARNE',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E002',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='13',@Libelle='SEMERU RUNGIS',@Adresse='54 RUE D ARCUEIL',@CodePostal='94150',@Ville='RUNGIS',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E002',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='20',@Libelle='SEMERU CUINCY',@Adresse='945 RUE DU FAUBOURG',@CodePostal='59553',@Ville='CUINCY',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E002',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='22',@Libelle='SEMERU TEMPLEMARS',@Adresse='0  PARC D''ACTIVITE B',@CodePostal='59175',@Ville='TEMPLEMARS',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E002',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='30',@Libelle='SEMERU PACA',@Adresse='68  PARC de L''ARGILE',@CodePostal='06370',@Ville='MOUANS-SARTOUX',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E002',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='40',@Libelle='SEMERU AQUITAINE',@Adresse='27 RUE DE FLEURENNE',@CodePostal='33290',@Ville='BLANQUEFORT',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E002',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='50',@Libelle='SEMERU LYON',@Adresse='2  CHEMIN DU GENIE',@CodePostal='69200',@Ville='VENISSIEUX',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E003',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='12',@Libelle='FARECO GENNEVILLIERS',@Adresse='223 RUE DES CABOEUFS',@CodePostal='92230',@Ville='GENNEVILLIERS',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E003',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='13',@Libelle='FARECO ASNIERES',@Adresse='250 Av DES GRESILLONS',@CodePostal='92600',@Ville='ASNIERES',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E003',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='20',@Libelle='FARECO MEYREUIL',@Adresse='8  PARC TECHNOLOGIQUE',@CodePostal='13590',@Ville='MEYREUIL',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E006',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='12',@Libelle='CITEPARK ANTONY',@Adresse='3 RUE HENRI POINCARE',@CodePostal='92160',@Ville='ANTONY',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E006',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='30',@Libelle='CITEPARK PACA',@Adresse='68  PARC DE l''ARGILE',@CodePostal='06370',@Ville='MOUANS-SARTOUX',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E010',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='10',@Libelle='ERS SAINT GREGOIRE',@Adresse='7  P.A. de Brocéliande',@CodePostal='35760',@Ville='SAINT GREGOIRE',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E010',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='11',@Libelle='ERS MELESSE',@Adresse='0 Rue de la Perrière',@CodePostal='35520',@Ville='MELESSE',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E010',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='20',@Libelle='ERS LAVAL',@Adresse='0  Allée de la Goberie',@CodePostal='53940',@Ville='SAINT-BERTHEVIN',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E010',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='30',@Libelle='ERS COUTANCES',@Adresse='0  ZI Chateau de la Mare',@CodePostal='50200',@Ville='COUTANCES',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E010',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='40',@Libelle='ERS ANGERS',@Adresse='15 rue Paul Langevin',@CodePostal='49240',@Ville='AVRILLE',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E010',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='50',@Libelle='ERS DINAN',@Adresse='2 rue de la Tramontane',@CodePostal='22100',@Ville='TADEN',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E010',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='60',@Libelle='ERS CARQUEFOU',@Adresse='25  Allée des Sapins',@CodePostal='44470',@Ville='CARQUEFOU',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E010',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='70',@Libelle='ERS SAINT AVE',@Adresse='30 rue Marcel Dassault',@CodePostal='56890',@Ville='SAINT AVE',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E010',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='80',@Libelle='ERS ELLIANT',@Adresse='0  Impasse Penalen',@CodePostal='29370',@Ville='ELLIANT',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E011',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='10',@Libelle='ERS MAINE',@Adresse='5  Allée du Perquoi',@CodePostal='72560',@Ville='CHANGE',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E011',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='20',@Libelle='ERS MAINE',@Adresse='3 rue de la Briaudière',@CodePostal='37510',@Ville='Ballan-Mire',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E012',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='10',@Libelle='SR DHENNIN',@Adresse='0  Zone Industrielle',@CodePostal='28140',@Ville='ORGERES EN BEAUCE',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E012',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='20',@Libelle='SR DHENNIN GELLAINVI',@Adresse='12 ave Gustave Eiffel',@CodePostal='28630',@Ville='GELLAINVILLE',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E013',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='11',@Libelle='SODECLIM',@Adresse='56 RUE LOUIS AUROUX',@CodePostal='94120',@Ville='FONTENAY SOUS BOIS',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E014',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='20',@Libelle='F.E.S.I Cuincy',@Adresse='945 RUE DU FBG D ESQUERCHIN',@CodePostal='59553',@Ville='CUINCY',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E015',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='41',@Libelle='GABRIELLE AUCH',@Adresse='2 IMP D ENGACHIES',@CodePostal='32000',@Ville='AUCH',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E015',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='42',@Libelle='GABRIELLE RIGNAC',@Adresse='0 ZA LES VIEILLES VIGNES',@CodePostal='46500',@Ville='RIGNAC',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E015',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='43',@Libelle='GABRIELLE  BEAUZELLE',@Adresse='160 RUE DE LA SUR',@CodePostal='31700',@Ville='BEAUZELLE',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E015',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='44',@Libelle='GABRIELLE CADOURS',@Adresse='0  Route de Toulouse',@CodePostal='31480',@Ville='CADOURS',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E016',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='14',@Libelle='AVEO NANTERRE',@Adresse='45 rue des hautes patures',@CodePostal='92000',@Ville='NANTERRE',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E017',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='97',@Libelle='SATELEC CENERGI',@Adresse='9 Rue Sully Prudhomme',@CodePostal='97420',@Ville='Le Port',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E017',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='98',@Libelle='SATELEC CENERGI',@Adresse='22 Rue Pierre Brossolette',@CodePostal='97420',@Ville='Le Port',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E018',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='14',@Libelle='SATCOMPTAGES SIEGE',@Adresse='24 Ave Gal de Gaulle',@CodePostal='91170',@Ville='VIRY-CHATILLON',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E018',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='27',@Libelle='SATCOMPTAGES VAUCH.',@Adresse='36 rue Rene Dingeon',@CodePostal='80132',@Ville='VAUCHELLES LES QUESNOY',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E018',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='56',@Libelle='SATCOMPTAGES ST AVE',@Adresse='30 rue Marcel Dassault',@CodePostal='56890',@Ville='SAINT AVE',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E019',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='27',@Libelle='VALIANCE CATTENOM',@Adresse='17 RUE DE L ABBE GREGOIRE',@CodePostal='59760',@Ville='GRANDE SYNTHE',@PaysCode='FR'
EXEC Fred_ToolBox_Etablissement_Paie @GroupeCode='GFES',@SocieteCode='E019',@IsAgenceRattachement=1,@Actif=1,@AgenceRattachementCode=null,@Code='23',@Libelle='VALIANCE GRDE SYNTHE',@Adresse='0 ZAC D HUSANGE',@CodePostal='57570',@Ville='CATTENOM',@PaysCode='FR'
