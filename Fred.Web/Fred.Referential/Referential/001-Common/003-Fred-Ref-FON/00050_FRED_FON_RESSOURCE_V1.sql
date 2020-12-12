-- --------------------------------------------------
-- FRED 2017 - R3 - JUILLET 2018 
-- INJECTION DES DONNES POUR FRED - FAYAT FONDATIONS
-- --------------------------------------------------




		

IF OBJECT_ID ( 'Fred_ToolBox_InsertRessources', 'P' ) IS NOT NULL   
    DROP PROCEDURE Fred_ToolBox_InsertRessources;  
GO  
CREATE PROCEDURE Fred_ToolBox_InsertRessources   
    @GroupeCode nvarchar(50),
	@SousChapitreCode nvarchar(50),
	@Code nvarchar(50),   
    @Libelle nvarchar(50) 
	

AS   



	DECLARE @GroupeId Int;
	DECLARE @ControleExistanceRessourceCodeDansGroupe Int;
	

	SET @GroupeId = (SELECT groupeId FROM FRED_GROUPE where Code = @GroupeCode);
	PRINT @GroupeId
	SET @ControleExistanceRessourceCodeDansGroupe = 
		(SELECT COUNT(RessourceId) FROM FRED_RESSOURCE, FRED_SOUS_CHAPITRE, FRED_CHAPITRE 
		WHERE FRED_RESSOURCE.SousChapitreId = FRED_SOUS_CHAPITRE.SousChapitreId 
		AND FRED_SOUS_CHAPITRE.ChapitreId = FRED_CHAPITRE.ChapitreId 
		AND FRED_CHAPITRE.GroupeId = @GroupeId AND FRED_RESSOURCE.code = @Code);

		IF(@ControleExistanceRessourceCodeDansGroupe = 0 )
		BEGIN
		DECLARE @SousChapitreId int;
		SET @SousChapitreId = 
			(SELECT FRED_SOUS_CHAPITRE.SousChapitreId 
			FROM FRED_SOUS_CHAPITRE, FRED_CHAPITRE
			WHERE FRED_SOUS_CHAPITRE.ChapitreId =  FRED_CHAPITRE.ChapitreId
			AND FRED_CHAPITRE.GroupeId = @GroupeId
			AND FRED_SOUS_CHAPITRE.Code = @SousChapitreCode)
			


			
				-- Récupération du Chapitre ID
				INSERT INTO FRED_RESSOURCE ([Code], [Libelle], [SousChapitreId], [Active], [DateCreation],[AuteurCreationId])
				VALUES (@Code, @Libelle,@SousChapitreId,1,GETDATE(),1 )
		END

GO 



EXEC Fred_ToolBox_InsertRessources  'FON', '6000','6001',' PERSONNEL STE '
EXEC Fred_ToolBox_InsertRessources  'FON', '6001','6002',' PERSONNEL GRP '
EXEC Fred_ToolBox_InsertRessources  'FON', '6001','6003',' PERSONNEL INTERIMAIRE '
EXEC Fred_ToolBox_InsertRessources  'FON', '6001','6004',' PERSONNEL EXTERIEUR '
EXEC Fred_ToolBox_InsertRessources  'FON', '6001','6010',' IK '
EXEC Fred_ToolBox_InsertRessources  'FON', '6001','6011',' PEAGES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6001','6012',' DIVERS DEPTS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6001','6013',' HOTEL '
EXEC Fred_ToolBox_InsertRessources  'FON', '6001','6014',' FRAIS DE RECEPTION '
EXEC Fred_ToolBox_InsertRessources  'FON', '6002','6020',' LOCATIONS MATERIEL GROUPE '
EXEC Fred_ToolBox_InsertRessources  'FON', '6002','6021',' LOCATIONS VEHICULES GRP '
EXEC Fred_ToolBox_InsertRessources  'FON', '6002','6022',' LOCATIONS DIVERSES GRP '
EXEC Fred_ToolBox_InsertRessources  'FON', '6003','6030',' CREDIT BAIL '
EXEC Fred_ToolBox_InsertRessources  'FON', '6003','6031',' LOCATIONS ET CHARGES IMMOBILIERES  '
EXEC Fred_ToolBox_InsertRessources  'FON', '6003','6032',' LOCATIONS VEHICULES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6003','6033',' LOCATIONS MATERIELS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6003','6034',' DROIT DE DECHARGES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6004','6040',' FOURNITURES NON STOCKABLE '
EXEC Fred_ToolBox_InsertRessources  'FON', '6004','6041',' CARBURANT GRP VEHICULES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6004','6042',' CARBURANT VEHICULES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6004','6043',' FOURNITURES DE BUREAU '
EXEC Fred_ToolBox_InsertRessources  'FON', '6004','6044',' FOURNITURES INFORMATIQUE '
EXEC Fred_ToolBox_InsertRessources  'FON', '6004','6045',' PETIT MOBILIER '
EXEC Fred_ToolBox_InsertRessources  'FON', '6004','6046',' PACKTAGE PERSONNEL '
EXEC Fred_ToolBox_InsertRessources  'FON', '6005','6050',' EAU '
EXEC Fred_ToolBox_InsertRessources  'FON', '6005','6051',' EDF '
EXEC Fred_ToolBox_InsertRessources  'FON', '6005','6052',' AIR LIQUIDE '
EXEC Fred_ToolBox_InsertRessources  'FON', '6005','6053',' OUTILS DE FORAGE '
EXEC Fred_ToolBox_InsertRessources  'FON', '6005','6054',' DIVERS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6005','6055',' CARBURANT GRP CHANTIERS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6005','6056',' CARBURANT CHANTIERS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6005','6057',' FOURNITURES POUR TIRANTS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6006','6060',' SOUS TRAITANCE INTERNE '
EXEC Fred_ToolBox_InsertRessources  'FON', '6006','6061',' SOUS TRAITANCE EXTERNE '
EXEC Fred_ToolBox_InsertRessources  'FON', '6006','6062',' SOUS TRAITANCE DIVERSES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6007','6070',' BETON '
EXEC Fred_ToolBox_InsertRessources  'FON', '6007','6071',' BENTONITE '
EXEC Fred_ToolBox_InsertRessources  'FON', '6007','6072',' CIMENT '
EXEC Fred_ToolBox_InsertRessources  'FON', '6007','6073',' ACIER '
EXEC Fred_ToolBox_InsertRessources  'FON', '6007','6074',' SABLONS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6007','6075',' DIVERS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6007','6076',' FOURNITURES POUR TIRANTS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6007','6077',' TUBES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6007','6078',' FTRES INCORPOROSE SOLS ET FOND '
EXEC Fred_ToolBox_InsertRessources  'FON', '6007','607A ',' PLATINES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6007','607B ',' EMBOUT TUBE '
EXEC Fred_ToolBox_InsertRessources  'FON', '6007','607c ',' SANGLES 35 MM '
EXEC Fred_ToolBox_InsertRessources  'FON', '6007','607D ',' SANGLE 50MM '
EXEC Fred_ToolBox_InsertRessources  'FON', '6007','607E ',' GOULOTTE '
EXEC Fred_ToolBox_InsertRessources  'FON', '6007','607F ',' PLATINE '
EXEC Fred_ToolBox_InsertRessources  'FON', '6007','607G ',' ANCRE '
EXEC Fred_ToolBox_InsertRessources  'FON', '6007','607H ',' ADHESIF '
EXEC Fred_ToolBox_InsertRessources  'FON', '6007','607I ',' SUPPORT CANAL D100 '
EXEC Fred_ToolBox_InsertRessources  'FON', '6008','6080',' TRANSPORT GRP '
EXEC Fred_ToolBox_InsertRessources  'FON', '6008','6081',' TRANSPORT CHANTIERS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6008','6082',' COURSIER '
EXEC Fred_ToolBox_InsertRessources  'FON', '6009','6090',' ETUDES ET PRESTATIONS DIVERSES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6009','6091',' DIVERS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6009','6092',' DIVERSES DOCUMENTATIONS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6009','6093',' HONORAIRES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6009','6094',' PUB.PUBLICAT. RELATIONS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6009','6095',' DEPLACEMENTS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6009','6096',' FRAIS POSTAUX ET TELECOMS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6009','6097',' BREVETS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6009','6098',' FOIRES ET EXPO '
EXEC Fred_ToolBox_InsertRessources  'FON', '6010','6100',' ENTRETIEN REP. GRP MAT. CHANTIER '
EXEC Fred_ToolBox_InsertRessources  'FON', '6010','6101',' ENTRETIEN REP. GRP SERVICES GENERAUX '
EXEC Fred_ToolBox_InsertRessources  'FON', '6010','6102',' ENTRETIEN REP. MAT. CHANTIER '
EXEC Fred_ToolBox_InsertRessources  'FON', '6010','6103',' ENTRETIEN REP. SERVICES GENERAUX '
EXEC Fred_ToolBox_InsertRessources  'FON', '6010','6104',' MAINTENANCE '
EXEC Fred_ToolBox_InsertRessources  'FON', '6011','6110',' DEPENSES PROVISIONNEES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6011','6111',' PROVISIONS P/RISQUES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6011','6112',' PROVISIONS P/DEPREC. '
EXEC Fred_ToolBox_InsertRessources  'FON', '6011','6113',' PROVISIONS AMORT. DEROGATOIRE '
EXEC Fred_ToolBox_InsertRessources  'FON', '6011','6114',' REP. PROVISIONS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6011','6115',' PROVISIONS FILIALES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6012','6120',' DEPENSES DIFFEREES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6013','6130',' ASSURANCES MULTIRISQUES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6013','6131',' ASSURANCE AUTO '
EXEC Fred_ToolBox_InsertRessources  'FON', '6013','6132',' ASSURANCE RC '
EXEC Fred_ToolBox_InsertRessources  'FON', '6013','6133',' ASSURANCE MATERIEL '
EXEC Fred_ToolBox_InsertRessources  'FON', '6013','6134',' ASSURANCE RC TVX '
EXEC Fred_ToolBox_InsertRessources  'FON', '6013','6135',' ASSURANCE PUC '
EXEC Fred_ToolBox_InsertRessources  'FON', '6013','6136',' ASSURANCE ASSISTANCE '
EXEC Fred_ToolBox_InsertRessources  'FON', '6013','6137',' ASSURANCE BRIS MACHINES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6013','6138',' ASSURANCE FRANCHISE/REMBT '
EXEC Fred_ToolBox_InsertRessources  'FON', '6014','6140',' AMORT. ET PROVISIONS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6015','6150',' TAXES ORG. SOCIAUX '
EXEC Fred_ToolBox_InsertRessources  'FON', '6015','6151',' TAXES ETAT '
EXEC Fred_ToolBox_InsertRessources  'FON', '6015','6152',' TAXES DIVERSES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6015','6153',' IMPOTS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6016','6160',' FRAIS ET SERVICES BANCAIRES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6016','6161',' CHARGES D''INTERETS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6016','6162',' DIVERSES CHARGES FINANCIERES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6016','6163',' DIVERS REVENUS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6016','6164',' DIVERS PRODUITS FINANCIERS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6017','6170',' CHARGES EXCP. S/OPERATIONS DE GESTION '
EXEC Fred_ToolBox_InsertRessources  'FON', '6017','6171',' CHARGES EXCP. DIVERSES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6017','6172',' VARIATION ELEMENTS CEDES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6017','6173',' DOTATIONS ET PROVISIONS EXCEPTIONNELLES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6017','6174',' PRODUITS EXCEPTIONNELS DIVERS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6017','6175',' PRODUITS EXCEPTIONNELS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6017','6176',' REPRISES S/ PROVISIONS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6017','6177',' TRANSFERT DE CHARGES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6017','6178',' RESTRUCTURATIONS '
EXEC Fred_ToolBox_InsertRessources  'FON', '6018','6180',' CREANCES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6018','6181',' QUOTE P/SEP '
EXEC Fred_ToolBox_InsertRessources  'FON', '6019','6190',' PRODUITS DE GESTION '
EXEC Fred_ToolBox_InsertRessources  'FON', '6019','6191',' LOCATIONS DIVERSES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6019','6192',' TRANSFERT DE CHARGES '
EXEC Fred_ToolBox_InsertRessources  'FON', '6800','6800',' RESULTAT ANALYTIQUE '
