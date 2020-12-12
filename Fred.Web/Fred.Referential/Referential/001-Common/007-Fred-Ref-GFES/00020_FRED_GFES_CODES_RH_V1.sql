-- --------------------------------------------------
-- INJECTION MAJORATION
-- --------------------------------------------------
EXEC Fred_ToolBox_Code_Majoration @GroupeCode='GFES',@Code='THRA',@Libelle='Travail heures de route aller ',@EtatPublic='Oui',@IsActif='Oui'
EXEC Fred_ToolBox_Code_Majoration @GroupeCode='GFES',@Code='THRR',@Libelle='Travail heures de route retour ',@EtatPublic='Oui',@IsActif='Oui'
EXEC Fred_ToolBox_Code_Majoration @GroupeCode='GFES',@Code='TNH1',@Libelle='Travail de nuit (Horaire)',@EtatPublic='Oui',@IsActif='Oui'
EXEC Fred_ToolBox_Code_Majoration @GroupeCode='GFES',@Code='TNH2',@Libelle='Travail de nuit (Horaire)',@EtatPublic='Oui',@IsActif='Oui'
EXEC Fred_ToolBox_Code_Majoration @GroupeCode='GFES',@Code='HM050',@Libelle='Heures majorées à  50%',@EtatPublic='non',@IsActif='non'
EXEC Fred_ToolBox_Code_Majoration @GroupeCode='GFES',@Code='HM100',@Libelle='Heures majorées à 100%',@EtatPublic='non',@IsActif='non'

DECLARE @SocieteId INT; 
DECLARE @Code VARCHAR(MAX); 
DECLARE @Libelle VARCHAR(MAX); 
Declare A Cursor For SELECT  SocieteId, Code, Libelle FROM FRED_SOCIETE WHERE GroupeId = (SELECT  GroupeId FROM FRED_GROUPE WHERE Code = 'GFES')


	Open A
			Fetch next From A into @SocieteId, @Code, @Libelle
				While @@Fetch_Status=0 Begin
		
		

			-- --------------------------------------------------
			-- INJECTION PRIME (BOUCLE PAR SOCIETE)
			-- --------------------------------------------------
			EXEC Fred_ToolBox_Code_Prime @GroupeCode='GFES',@Code='GDI',@Libelle='IGD IDF',@NombreHeuresMax='7',@Actif='Oui',@PrimePartenaire='Non',@Publique='Oui',@PrimeType='Journalier',@SocieteCode=@code
			EXEC Fred_ToolBox_Code_Prime @GroupeCode='GFES',@Code='GDP',@Libelle='IGD Province',@NombreHeuresMax='7',@Actif='Oui',@PrimePartenaire='Non',@Publique='Oui',@PrimeType='Horaire',@SocieteCode=@code
			EXEC Fred_ToolBox_Code_Prime @GroupeCode='GFES',@Code='IR',@Libelle='Indemnité Repas',@NombreHeuresMax='7',@Actif='Oui',@PrimePartenaire='Non',@Publique='Oui',@PrimeType='Mensuelle',@SocieteCode=@code
			EXEC Fred_ToolBox_Code_Prime @GroupeCode='GFES',@Code='TR',@Libelle='Titres restaurant',@NombreHeuresMax='7',@Actif='Oui',@PrimePartenaire='Non',@Publique='Oui',@PrimeType='Journalier',@SocieteCode=@code
			EXEC Fred_ToolBox_Code_Prime @GroupeCode='GFES',@Code='AE',@Libelle='Prime égouts',@NombreHeuresMax='7',@Actif='Oui',@PrimePartenaire='Non',@Publique='Oui',@PrimeType='Horaire',@SocieteCode=@code
			EXEC Fred_ToolBox_Code_Prime @GroupeCode='GFES',@Code='INS',@Libelle='Prime insalubrité ',@NombreHeuresMax='7',@Actif='Oui',@PrimePartenaire='Non',@Publique='Oui',@PrimeType='Mensuelle',@SocieteCode=@code
			EXEC Fred_ToolBox_Code_Prime @GroupeCode='GFES',@Code='ES',@Libelle='Prime salissure',@NombreHeuresMax='7',@Actif='Oui',@PrimePartenaire='Non',@Publique='Oui',@PrimeType='Mensuelle',@SocieteCode=@code

			-- --------------------------------------------------
			-- INJECTION CODE ABSENCE (BOUCLE PAR SOCIETE)
			-- --------------------------------------------------
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='ANA',@Libelle='Absence non autorisée',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='ANP',@Libelle='Absence autorisée non payée',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='AP',@Libelle='Absence payée',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='I ',@Libelle='Absence intempérie',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='DEL',@Libelle='Absence délégation',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='PREAV',@Libelle='Préavis effectué payé',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='PNE',@Libelle='Préavis non effectué payé',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='VM',@Libelle='Visite médicale',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='ACC',@Libelle='Accident',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='AJ',@Libelle='Accident de trajet',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='AT',@Libelle='Accident du travail',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='MAL',@Libelle='Maladie',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='ML',@Libelle='Maladie non professionnelle',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='MP',@Libelle='Maladie professionnelle',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='MTHN',@Libelle='Mi-temps thérapeutique',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='CP ',@Libelle='Conge paye',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='RC',@Libelle='Repos compensateur',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='RT',@Libelle='RTT',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='CONG',@Libelle='Congé à justifier',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='CONV',@Libelle='Congé conventionnel (mariage, naissance, décès)',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='CPAR',@Libelle='Congé parental',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='CSAB',@Libelle='Congé sabbatique',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='CSS ',@Libelle='Congé sans solde',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='MT',@Libelle='Maternité',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'
			EXEC Fred_ToolBox_Code_Absence @GroupeCode='GFES',@SocieteCode=@code,@Code='PT',@Libelle='Paternité',@Intemperie='Non',@TauxDecote='1',@NBHeuresDefautETAM='7.5',@NBHeuresMinETAM='1',@NBHeuresMaxETAM='7.5',@NBHeuresDefautCO='7.5',@NBHeuresMinCO='1',@NBHeuresMaxCO='7.5'




			-- --------------------------------------------------
			-- INJECTION CODE DEPLACEMENT (BOUCLE PAR SOCIETE)
			-- --------------------------------------------------
			EXEC Fred_ToolBox_Code_Deplacement @GroupeCode='GFES',@SocieteCode=@code,@Code='Z1',@Libelle='Zone 1',@KmMini='0',@KmMaxi='10',@IGD='Oui'
			EXEC Fred_ToolBox_Code_Deplacement @GroupeCode='GFES',@SocieteCode=@code,@Code='Z2',@Libelle='Zone 2',@KmMini='10',@KmMaxi='20',@IGD='Oui'
			EXEC Fred_ToolBox_Code_Deplacement @GroupeCode='GFES',@SocieteCode=@code,@Code='Z3',@Libelle='Zone 3',@KmMini='20',@KmMaxi='30',@IGD='Oui'
			EXEC Fred_ToolBox_Code_Deplacement @GroupeCode='GFES',@SocieteCode=@code,@Code='Z4',@Libelle='Zone 4',@KmMini='30',@KmMaxi='40',@IGD='Oui'
			EXEC Fred_ToolBox_Code_Deplacement @GroupeCode='GFES',@SocieteCode=@code,@Code='Z5',@Libelle='Zone 5',@KmMini='40',@KmMaxi='50',@IGD='Oui'
			EXEC Fred_ToolBox_Code_Deplacement @GroupeCode='GFES',@SocieteCode=@code,@Code='Z6',@Libelle='Zone 6',@KmMini='50',@KmMaxi='999',@IGD='Oui'



			-- --------------------------------------------------
			-- INJECTION CODE ZONE DEPLACEMENT (BOUCLE PAR SOCIETE)
			-- --------------------------------------------------
			EXEC Fred_ToolBox_Code_Zone_Deplacement @GroupeCode='GFES',@SocieteCode=@code,@Code='Z1',@Libelle='Zone 1',@IsActif='Oui'
			EXEC Fred_ToolBox_Code_Zone_Deplacement @GroupeCode='GFES',@SocieteCode=@code,@Code='Z2',@Libelle='Zone 2',@IsActif='Oui'
			EXEC Fred_ToolBox_Code_Zone_Deplacement @GroupeCode='GFES',@SocieteCode=@code,@Code='Z3',@Libelle='Zone 3',@IsActif='Oui'
			EXEC Fred_ToolBox_Code_Zone_Deplacement @GroupeCode='GFES',@SocieteCode=@code,@Code='Z4',@Libelle='Zone 4',@IsActif='Oui'
			EXEC Fred_ToolBox_Code_Zone_Deplacement @GroupeCode='GFES',@SocieteCode=@code,@Code='Z5',@Libelle='Zone 5',@IsActif='Oui'
			EXEC Fred_ToolBox_Code_Zone_Deplacement @GroupeCode='GFES',@SocieteCode=@code,@Code='Z6',@Libelle='Zone 6',@IsActif='Oui'




			Fetch next From A into  @SocieteId, @Code, @Libelle
	End
	Close A

Deallocate A
	

