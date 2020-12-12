-- --------------------------------------------------
-- FRED 2018 - R4 - OCTOBRE 2018 
-- INJECTION DES DONNES POUR FRED - GROUPE FAYAT TP
-- --------------------------------------------------

-- CORRECTION SUR LE PERSONNEL INTERNE 
UPDATE FRED_PERSONNEL SET IsInterimaire = 0 WHERE  SocieteId = (SELECT SocieteId FROM FRED_SOCIETE WHERE code = '0143' AND GroupeId= (SELECT GroupeId FROM FRED_GROUPE where Code = 'GFTP'))




-- AJOUT PERSONNEL
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300083',@IsInterimaire=0,@IsInterne=1,@Nom='PLAS',@Prenom='EDGAR',@CategoriePerso='M',@DateEntree='16/06/1992'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300084',@IsInterimaire=0,@IsInterne=1,@Nom='CAUCHOIS',@Prenom='MATTHIAS',@CategoriePerso='M',@DateEntree='16/06/1992'



-- MODIFICATION DES PERSONNELS
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300033',@IsInterimaire=0,@IsInterne=0,@Nom='LOZAC''H',@Prenom='MICHAEL'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300003',@IsInterimaire=0,@IsInterne=0,@Nom='CARVALHO',@Prenom='JACQUES'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300053',@IsInterimaire=0,@IsInterne=0,@Nom='DARMAILLACQ',@Prenom='SEBASTIEN'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300024',@IsInterimaire=0,@IsInterne=0,@Nom='FEUILLERAT',@Prenom='MARTINE'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300055',@IsInterimaire=0,@IsInterne=0,@Nom='CANTANTE',@Prenom='LUCIANO'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300075',@IsInterimaire=0,@IsInterne=0,@Nom='HUZAR',@Prenom='HECTOR'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300045',@IsInterimaire=0,@IsInterne=0,@Nom='TERNOI',@Prenom='AURORE'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300035',@IsInterimaire=0,@IsInterne=0,@Nom='BAUTRAIT',@Prenom='SAMUEL'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300002',@IsInterimaire=0,@IsInterne=0,@Nom='BLANC',@Prenom='JEAN MARIE'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300004',@IsInterimaire=0,@IsInterne=0,@Nom='SENEL',@Prenom='NIHAT'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300009',@IsInterimaire=0,@IsInterne=0,@Nom='BOYER',@Prenom='JOEL'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300011',@IsInterimaire=0,@IsInterne=0,@Nom='SALOUL',@Prenom='PIERRE'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300015',@IsInterimaire=0,@IsInterne=0,@Nom='PALLAS',@Prenom='FRANCK'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300018',@IsInterimaire=0,@IsInterne=0,@Nom='MONTASSIER',@Prenom='JEAN MARIE'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300019',@IsInterimaire=0,@IsInterne=0,@Nom='YALCIN',@Prenom='MEHMET'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300020',@IsInterimaire=0,@IsInterne=0,@Nom='PEREIRA RODRIGUES',@Prenom='JOSE'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300021',@IsInterimaire=0,@IsInterne=0,@Nom='SALMON',@Prenom='XAVIER'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300023',@IsInterimaire=0,@IsInterne=0,@Nom='LEDUC',@Prenom='ALAIN'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300025',@IsInterimaire=0,@IsInterne=0,@Nom='NORMAND',@Prenom='RICHARD'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300026',@IsInterimaire=0,@IsInterne=0,@Nom='LATOUR',@Prenom='FREDERIC'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300027',@IsInterimaire=0,@IsInterne=0,@Nom='DANTAS',@Prenom='MANUEL'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300028',@IsInterimaire=0,@IsInterne=0,@Nom='SUPERVIELLE',@Prenom='GUILLAUME'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300031',@IsInterimaire=0,@IsInterne=0,@Nom='FIGUEIRA MENDES',@Prenom='SIDONIO'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300034',@IsInterimaire=0,@IsInterne=0,@Nom='BOUGGUEDIMA',@Prenom='ALI'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300036',@IsInterimaire=0,@IsInterne=0,@Nom='POUILLET',@Prenom='CEDRIC'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300037',@IsInterimaire=0,@IsInterne=0,@Nom='HRISTOV',@Prenom='SEVDALIN'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300038',@IsInterimaire=0,@IsInterne=0,@Nom='COURBIN',@Prenom='MICHAEL'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300040',@IsInterimaire=0,@IsInterne=0,@Nom='GLENISSON',@Prenom='LAURENT'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300046',@IsInterimaire=0,@IsInterne=0,@Nom='DE BRITO',@Prenom='SAMUEL'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300047',@IsInterimaire=0,@IsInterne=0,@Nom='NORMANDIN',@Prenom='HERVE'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300051',@IsInterimaire=0,@IsInterne=0,@Nom='FERNANDES',@Prenom='MANUEL AUGUSTO'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300052',@IsInterimaire=0,@IsInterne=0,@Nom='DA CUNHA ARAUJO',@Prenom='CARLOS MIGUEL'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300054',@IsInterimaire=0,@IsInterne=0,@Nom='DIAS RAMOS',@Prenom='TIAGO'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300060',@IsInterimaire=0,@IsInterne=0,@Nom='DA SILVA CARVALHO',@Prenom='MANUEL'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300061',@IsInterimaire=0,@IsInterne=0,@Nom='DUBERTRAND',@Prenom='JEAN PIERRE'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300062',@IsInterimaire=0,@IsInterne=0,@Nom='GALLE',@Prenom='LAURENCE'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300063',@IsInterimaire=0,@IsInterne=0,@Nom='KERDRAON',@Prenom='PASCAL'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300064',@IsInterimaire=0,@IsInterne=0,@Nom='COUTEIRO',@Prenom='SILVIO'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300067',@IsInterimaire=0,@IsInterne=0,@Nom='DUCUING',@Prenom='THIERRY'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300070',@IsInterimaire=0,@IsInterne=0,@Nom='FERNANDES',@Prenom='EMMANUEL'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300071',@IsInterimaire=0,@IsInterne=0,@Nom='TAOMBE',@Prenom='JEAN PATRICK'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300072',@IsInterimaire=0,@IsInterne=0,@Nom='RUIZ',@Prenom='MICHAEL'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300073',@IsInterimaire=0,@IsInterne=0,@Nom='SAUVAGE',@Prenom='FREDERIC'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300076',@IsInterimaire=0,@IsInterne=0,@Nom='GARCIA',@Prenom='NICOLAS'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300077',@IsInterimaire=0,@IsInterne=0,@Nom='GUYARD',@Prenom='DOMINIQUE'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300078',@IsInterimaire=0,@IsInterne=0,@Nom='MAZI',@Prenom='LAURA'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300080',@IsInterimaire=0,@IsInterne=0,@Nom='ETIENNE',@Prenom='DIDIER'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300081',@IsInterimaire=0,@IsInterne=0,@Nom='ORENGO',@Prenom='LOUIS OLIVIER'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14300082',@IsInterimaire=0,@IsInterne=0,@Nom='CASTELO DA COSTA',@Prenom='VITOR'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390001',@IsInterimaire=0,@IsInterne=0,@Nom='VICAIRE',@Prenom='LAURE'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390002',@IsInterimaire=0,@IsInterne=0,@Nom='AAJJAN',@Prenom='Said'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390003',@IsInterimaire=0,@IsInterne=0,@Nom='AATHMANE',@Prenom='Driss'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390004',@IsInterimaire=0,@IsInterne=0,@Nom='ABASSE',@Prenom='Lookman'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390005',@IsInterimaire=0,@IsInterne=0,@Nom='AIBICH',@Prenom='Said'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390006',@IsInterimaire=0,@IsInterne=0,@Nom='AMARENGO',@Prenom='Jacques'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390007',@IsInterimaire=0,@IsInterne=0,@Nom='AMANI',@Prenom='Jean-Jacques'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390008',@IsInterimaire=0,@IsInterne=0,@Nom='BELHADI',@Prenom='Helmi'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390009',@IsInterimaire=0,@IsInterne=0,@Nom='BELKACEM',@Prenom='Kahlouli'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390010',@IsInterimaire=0,@IsInterne=0,@Nom='BELLAHA',@Prenom='Lahouari'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390011',@IsInterimaire=0,@IsInterne=0,@Nom='BENAMEUR',@Prenom='Yahya'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390012',@IsInterimaire=0,@IsInterne=0,@Nom='BENAUD',@Prenom='Sébastien'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390013',@IsInterimaire=0,@IsInterne=0,@Nom='BESSE',@Prenom='Michel'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390014',@IsInterimaire=0,@IsInterne=0,@Nom='BESTAVEN',@Prenom='Zakaria'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390015',@IsInterimaire=0,@IsInterne=0,@Nom='BETTERCHA',@Prenom='Abdelkhader'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390016',@IsInterimaire=0,@IsInterne=0,@Nom='BOUCHBOUT',@Prenom='Karim'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390017',@IsInterimaire=0,@IsInterne=0,@Nom='BRULU',@Prenom='Jean-Charles'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390018',@IsInterimaire=0,@IsInterne=0,@Nom='BRUNETIER',@Prenom='Mikael'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390019',@IsInterimaire=0,@IsInterne=0,@Nom='CAELEN',@Prenom='Vincent'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390020',@IsInterimaire=0,@IsInterne=0,@Nom='CAHUZAC',@Prenom='Gaetan'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390021',@IsInterimaire=0,@IsInterne=0,@Nom='CARDOSO',@Prenom='Carlos'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390022',@IsInterimaire=0,@IsInterne=0,@Nom='CASTELO',@Prenom='Vitor'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390023',@IsInterimaire=0,@IsInterne=0,@Nom='CHERIF',@Prenom='Ahmed'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390024',@IsInterimaire=0,@IsInterne=0,@Nom='CHICOS',@Prenom='Petrica'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390025',@IsInterimaire=0,@IsInterne=0,@Nom='CIOBANU',@Prenom='Christian'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390026',@IsInterimaire=0,@IsInterne=0,@Nom='CISTERNA ALVAREZ',@Prenom='Eric'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390027',@IsInterimaire=0,@IsInterne=0,@Nom='COMBAUD',@Prenom='Jimmy'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390028',@IsInterimaire=0,@IsInterne=0,@Nom='CORREIA',@Prenom='Antonio'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390029',@IsInterimaire=0,@IsInterne=0,@Nom='COURTOIS',@Prenom='Erman'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390030',@IsInterimaire=0,@IsInterne=0,@Nom='DA SILVA',@Prenom='Macelo'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390031',@IsInterimaire=0,@IsInterne=0,@Nom='DARDEL',@Prenom='Romain'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390032',@IsInterimaire=0,@IsInterne=0,@Nom='DEBOCK',@Prenom='Yoann'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390033',@IsInterimaire=0,@IsInterne=0,@Nom='DIAGANA',@Prenom='Tidiane'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390034',@IsInterimaire=0,@IsInterne=0,@Nom='DO ROSARIO',@Prenom='Rafael'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390035',@IsInterimaire=0,@IsInterne=0,@Nom='DOUCHET',@Prenom='Brian'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390036',@IsInterimaire=0,@IsInterne=0,@Nom='ELIE',@Prenom='Christophe'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390037',@IsInterimaire=0,@IsInterne=0,@Nom='ESTATI',@Prenom='Mohammed'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390038',@IsInterimaire=0,@IsInterne=0,@Nom='ETIENNE',@Prenom='Didier'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390039',@IsInterimaire=0,@IsInterne=0,@Nom='FARNETI',@Prenom='Gilles'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390040',@IsInterimaire=0,@IsInterne=0,@Nom='FAURE',@Prenom='Eddy'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390041',@IsInterimaire=0,@IsInterne=0,@Nom='FERNANDES',@Prenom='Antoine'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390042',@IsInterimaire=0,@IsInterne=0,@Nom='FERRY',@Prenom='Romain'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390043',@IsInterimaire=0,@IsInterne=0,@Nom='GARBUIO',@Prenom='Axel'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390044',@IsInterimaire=0,@IsInterne=0,@Nom='GMILI',@Prenom='Abdelkader'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390045',@IsInterimaire=0,@IsInterne=0,@Nom='GOMEZ',@Prenom='Stéphane'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390046',@IsInterimaire=0,@IsInterne=0,@Nom='GORGULU',@Prenom='Ayhan'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390047',@IsInterimaire=0,@IsInterne=0,@Nom='GRANDET',@Prenom='Alain'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390048',@IsInterimaire=0,@IsInterne=0,@Nom='HADDAD',@Prenom='Mourad'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390049',@IsInterimaire=0,@IsInterne=0,@Nom='HADDAOUI',@Prenom='Mustapha'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390050',@IsInterimaire=0,@IsInterne=0,@Nom='HAKIKI',@Prenom='Noureddine'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390051',@IsInterimaire=0,@IsInterne=0,@Nom='HOUADSI',@Prenom='Toufik'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390052',@IsInterimaire=0,@IsInterne=0,@Nom='HRISTOV',@Prenom='Dilyan'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390053',@IsInterimaire=0,@IsInterne=0,@Nom='IGLESIAS',@Prenom='Mickael'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390054',@IsInterimaire=0,@IsInterne=0,@Nom='IVANOV',@Prenom='Strahil'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390055',@IsInterimaire=0,@IsInterne=0,@Nom='JACQUE',@Prenom='Kenzo'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390056',@IsInterimaire=0,@IsInterne=0,@Nom='JOLLET',@Prenom='Manuel'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390057',@IsInterimaire=0,@IsInterne=0,@Nom='KOLODZIEJ',@Prenom='Olivier'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390058',@IsInterimaire=0,@IsInterne=0,@Nom='KONUK',@Prenom='Mevlut'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390059',@IsInterimaire=0,@IsInterne=0,@Nom='KUPPIG',@Prenom='Romain'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390060',@IsInterimaire=0,@IsInterne=0,@Nom='LACHOURI',@Prenom='Sami'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390061',@IsInterimaire=0,@IsInterne=0,@Nom='LAHBIB',@Prenom='Ahmed'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390062',@IsInterimaire=0,@IsInterne=0,@Nom='EL HANATI',@Prenom='Lahcen'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390063',@IsInterimaire=0,@IsInterne=0,@Nom='LE GAC',@Prenom='Emmanuel'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390064',@IsInterimaire=0,@IsInterne=0,@Nom='LE ROUIC',@Prenom='Morgan'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390065',@IsInterimaire=0,@IsInterne=0,@Nom='LEDUC',@Prenom='Raphael'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390066',@IsInterimaire=0,@IsInterne=0,@Nom='LELOUX',@Prenom='Julien'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390067',@IsInterimaire=0,@IsInterne=0,@Nom='MADUREIRA',@Prenom='Pedro'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390068',@IsInterimaire=0,@IsInterne=0,@Nom='MARTINS',@Prenom='Wilfryed'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390069',@IsInterimaire=0,@IsInterne=0,@Nom='MEBAREK',@Prenom='Dwen'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390070',@IsInterimaire=0,@IsInterne=0,@Nom='MITKOV',@Prenom='Soni'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390071',@IsInterimaire=0,@IsInterne=0,@Nom='MOREIRA',@Prenom='Fabio'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390072',@IsInterimaire=0,@IsInterne=0,@Nom='MOUELHI',@Prenom='Soufiane'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390073',@IsInterimaire=0,@IsInterne=0,@Nom='NTUNUKINA',@Prenom='Pululu'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390074',@IsInterimaire=0,@IsInterne=0,@Nom='OLIVEIRA',@Prenom='Brando'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390075',@IsInterimaire=0,@IsInterne=0,@Nom='OUEDRAOGO',@Prenom='Adama'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390076',@IsInterimaire=0,@IsInterne=0,@Nom='PEREIRA',@Prenom='José'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390077',@IsInterimaire=0,@IsInterne=0,@Nom='PERINET',@Prenom='Jean-Francois'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390078',@IsInterimaire=0,@IsInterne=0,@Nom='PAULO',@Prenom='Pedro'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390079',@IsInterimaire=0,@IsInterne=0,@Nom='PEYRICHOU',@Prenom='Michael'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390080',@IsInterimaire=0,@IsInterne=0,@Nom='PHILOGENE',@Prenom='Djibrill'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390081',@IsInterimaire=0,@IsInterne=0,@Nom='PUJOL',@Prenom='Yoann'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390082',@IsInterimaire=0,@IsInterne=0,@Nom='SKERRITT',@Prenom='Patrice'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390083',@IsInterimaire=0,@IsInterne=0,@Nom='SOUIDIKA',@Prenom='Said'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390084',@IsInterimaire=0,@IsInterne=0,@Nom='SOUMARE',@Prenom='Zakaria'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390085',@IsInterimaire=0,@IsInterne=0,@Nom='TALEB AHMED',@Prenom='Farid'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390086',@IsInterimaire=0,@IsInterne=0,@Nom='STRUGARIU',@Prenom='Gheorghe'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390087',@IsInterimaire=0,@IsInterne=0,@Nom='TIAGO',@Prenom='Paulo'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390088',@IsInterimaire=0,@IsInterne=0,@Nom='VALENTIN',@Prenom='Thierry'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390089',@IsInterimaire=0,@IsInterne=0,@Nom='VALLEJO',@Prenom='Frédéric'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390090',@IsInterimaire=0,@IsInterne=0,@Nom='VIMARE',@Prenom='Clément'
EXEC Fred_ToolBox_Personnel @GroupeCode='GFTP',@SocieteCode='0143',@EtablissementPayeCode='EP0143',@EtablissementRattachementCode='143A',@Matricule='14390091',@IsInterimaire=0,@IsInterne=0,@Nom='WALBECQ',@Prenom='Jean-Francois'



-- CHANGEMENT DES RESSOURCES
-- POSITIONNEMENT TOUS PERSONNELS SUR 
-- RESSOURCES OUVRIER
DECLARE @RessourceID INT;
SET @RessourceID = 
							(
							SELECT FRED_RESSOURCE.RessourceId
							FROM FRED_RESSOURCE, FRED_SOUS_CHAPITRE
							WHERE FRED_SOUS_CHAPITRE.SousChapitreId IN 
								(
									SELECT FRED_SOUS_CHAPITRE.SousChapitreId 
									FROM FRED_SOUS_CHAPITRE, FRED_CHAPITRE 
									WHERE FRED_CHAPITRE.ChapitreId IN 
										(
										SELECT ChapitreId FROM FRED_CHAPITRE WHERE GroupeId = (SELECT GroupeId FROM FRED_GROUPE WHERE Code = 'GFTP'
										)
								AND FRED_SOUS_CHAPITRE.ChapitreId = FRED_CHAPITRE.ChapitreId)
								AND FRED_RESSOURCE.SousChapitreId = FRED_SOUS_CHAPITRE.SousChapitreId
								)
								
							AND FRED_RESSOURCE.Code = 'OUVR-01'
							)
UPDATE FRED_PERSONNEL SET RessourceId = @RessourceID WHERE  SocieteId = (SELECT SocieteId FROM FRED_SOCIETE WHERE code = '0143' AND GroupeId= (SELECT GroupeId FROM FRED_GROUPE where Code = 'GFTP'))





