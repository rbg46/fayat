-- --------------------------------------------------
-- FRED 2018 - R4 - OCTOBRE 2018 
-- INJECTION DES DONNES POUR FRED - GROUPE FAYAT TP
-- --------------------------------------------------




-- --------------------------------------------------
-- INJECTION DES CODES RH
-- --------------------------------------------------

-- PRIME
 EXEC Fred_ToolBox_Code_Prime  @GroupeCode='GFTP' , @Code='1303', @Libelle='Primes diverses', @NombreHeuresMax='0', @Actif='1', @PrimePartenaire='0', @Publique='1', @SocieteCode= '0143', @PrimeType='0'
 EXEC Fred_ToolBox_Code_Prime  @GroupeCode='GFTP' , @Code='1304', @Libelle='Primes chantier', @NombreHeuresMax='0', @Actif='1', @PrimePartenaire='0', @Publique='1', @SocieteCode= '0143', @PrimeType='0'
 EXEC Fred_ToolBox_Code_Prime  @GroupeCode='GFTP' , @Code='1305', @Libelle='Prime exceptionnelle', @NombreHeuresMax='0', @Actif='1', @PrimePartenaire='0', @Publique='1', @SocieteCode= '0143', @PrimeType='0'


-- CODE ZONE DEPLACEMENT
 EXEC Fred_ToolBox_Code_Zone_Deplacement  @GroupeCode='GFTP' , @Code='Z1A', @Libelle='Zone 1A', @IsActif='1', @SocieteCode= '0143'
 EXEC Fred_ToolBox_Code_Zone_Deplacement  @GroupeCode='GFTP' , @Code='Z1B', @Libelle='Zone 1B', @IsActif='1', @SocieteCode= '0143'
 EXEC Fred_ToolBox_Code_Zone_Deplacement  @GroupeCode='GFTP' , @Code='Z2', @Libelle='Zone 2', @IsActif='1', @SocieteCode= '0143'
 EXEC Fred_ToolBox_Code_Zone_Deplacement  @GroupeCode='GFTP' , @Code='Z3', @Libelle='Zone 3', @IsActif='1', @SocieteCode= '0143'
 EXEC Fred_ToolBox_Code_Zone_Deplacement  @GroupeCode='GFTP' , @Code='Z4', @Libelle='Zone 4', @IsActif='1', @SocieteCode= '0143'
 EXEC Fred_ToolBox_Code_Zone_Deplacement  @GroupeCode='GFTP' , @Code='Z5', @Libelle='Zone 5', @IsActif='1', @SocieteCode= '0143'
 EXEC Fred_ToolBox_Code_Zone_Deplacement  @GroupeCode='GFTP' , @Code='Zone1', @Libelle='Zone 1 de 0-50 KM0', @IsActif='1', @SocieteCode= '0143'
 EXEC Fred_ToolBox_Code_Zone_Deplacement  @GroupeCode='GFTP' , @Code='Zone2', @Libelle='Zone 2 de 51-70 KM', @IsActif='1', @SocieteCode= '0143'
 EXEC Fred_ToolBox_Code_Zone_Deplacement  @GroupeCode='GFTP' , @Code='Zone3', @Libelle='Zone 3 de 71-400KM', @IsActif='1', @SocieteCode= '0143'
 EXEC Fred_ToolBox_Code_Zone_Deplacement  @GroupeCode='GFTP' , @Code='Zone4', @Libelle='Zone 4 de 400 KM et +', @IsActif='1', @SocieteCode= '0143'



-- CODE ABSENCES
 EXEC Fred_ToolBox_Code_Absence  @GroupeCode='GFTP' ,  @SocieteCode= '0143', @Code='300', @Libelle='Heures travaillées (heures normales)', @Intemperie='0', @TauxDecote='0', @NBHeuresDefautETAM='7.4', @NBHeuresMinETAM='1', @NBHeuresMaxETAM='7.4', @NBHeuresDefautCO='7.4', @NBHeuresMinCO='1', @NBHeuresMaxCO='7.4', @Actif='1'
 EXEC Fred_ToolBox_Code_Absence  @GroupeCode='GFTP' ,  @SocieteCode= '0143', @Code='1000', @Libelle='Maladie', @Intemperie='0', @TauxDecote='0', @NBHeuresDefautETAM='7.4', @NBHeuresMinETAM='1', @NBHeuresMaxETAM='7.4', @NBHeuresDefautCO='7.4', @NBHeuresMinCO='1', @NBHeuresMaxCO='7.4', @Actif='1'
 EXEC Fred_ToolBox_Code_Absence  @GroupeCode='GFTP' ,  @SocieteCode= '0143', @Code='1006', @Libelle='Accident travail', @Intemperie='0', @TauxDecote='0', @NBHeuresDefautETAM='7.4', @NBHeuresMinETAM='1', @NBHeuresMaxETAM='7.4', @NBHeuresDefautCO='7.4', @NBHeuresMinCO='1', @NBHeuresMaxCO='7.4', @Actif='1'
 EXEC Fred_ToolBox_Code_Absence  @GroupeCode='GFTP' ,  @SocieteCode= '0143', @Code='1020', @Libelle='Accident trajet', @Intemperie='0', @TauxDecote='0', @NBHeuresDefautETAM='7.4', @NBHeuresMinETAM='1', @NBHeuresMaxETAM='7.4', @NBHeuresDefautCO='7.4', @NBHeuresMinCO='1', @NBHeuresMaxCO='7.4', @Actif='1'
 EXEC Fred_ToolBox_Code_Absence  @GroupeCode='GFTP' ,  @SocieteCode= '0143', @Code='1025', @Libelle='Maternité', @Intemperie='0', @TauxDecote='0', @NBHeuresDefautETAM='7.4', @NBHeuresMinETAM='1', @NBHeuresMaxETAM='7.4', @NBHeuresDefautCO='7.4', @NBHeuresMinCO='1', @NBHeuresMaxCO='7.4', @Actif='1'
 EXEC Fred_ToolBox_Code_Absence  @GroupeCode='GFTP' ,  @SocieteCode= '0143', @Code='1030', @Libelle='Paternité', @Intemperie='0', @TauxDecote='0', @NBHeuresDefautETAM='7.4', @NBHeuresMinETAM='1', @NBHeuresMaxETAM='7.4', @NBHeuresDefautCO='7.4', @NBHeuresMinCO='1', @NBHeuresMaxCO='7.4', @Actif='1'
 EXEC Fred_ToolBox_Code_Absence  @GroupeCode='GFTP' ,  @SocieteCode= '0143', @Code='1035', @Libelle='Mi-temps thérapeutique', @Intemperie='0', @TauxDecote='0', @NBHeuresDefautETAM='7.4', @NBHeuresMinETAM='1', @NBHeuresMaxETAM='7.4', @NBHeuresDefautCO='7.4', @NBHeuresMinCO='1', @NBHeuresMaxCO='7.4', @Actif='1'
 EXEC Fred_ToolBox_Code_Absence  @GroupeCode='GFTP' ,  @SocieteCode= '0143', @Code='1040', @Libelle='Congés non rémunérés', @Intemperie='0', @TauxDecote='0', @NBHeuresDefautETAM='7.4', @NBHeuresMinETAM='1', @NBHeuresMaxETAM='7.4', @NBHeuresDefautCO='7.4', @NBHeuresMinCO='1', @NBHeuresMaxCO='7.4', @Actif='1'
 EXEC Fred_ToolBox_Code_Absence  @GroupeCode='GFTP' ,  @SocieteCode= '0143', @Code='1045', @Libelle='Chomage intempéries', @Intemperie='0', @TauxDecote='0', @NBHeuresDefautETAM='7.4', @NBHeuresMinETAM='1', @NBHeuresMaxETAM='7.4', @NBHeuresDefautCO='7.4', @NBHeuresMinCO='1', @NBHeuresMaxCO='7.4', @Actif='1'
 EXEC Fred_ToolBox_Code_Absence  @GroupeCode='GFTP' ,  @SocieteCode= '0143', @Code='1050', @Libelle='Absence exeptionnelle à justifier', @Intemperie='0', @TauxDecote='0', @NBHeuresDefautETAM='7.4', @NBHeuresMinETAM='1', @NBHeuresMaxETAM='7.4', @NBHeuresDefautCO='7.4', @NBHeuresMinCO='1', @NBHeuresMaxCO='7.4', @Actif='1'
 EXEC Fred_ToolBox_Code_Absence  @GroupeCode='GFTP' ,  @SocieteCode= '0143', @Code='1051', @Libelle='Absence entrée et sortie Société', @Intemperie='0', @TauxDecote='0', @NBHeuresDefautETAM='7.4', @NBHeuresMinETAM='1', @NBHeuresMaxETAM='7.4', @NBHeuresDefautCO='7.4', @NBHeuresMinCO='1', @NBHeuresMaxCO='7.4', @Actif='1'
 EXEC Fred_ToolBox_Code_Absence  @GroupeCode='GFTP' ,  @SocieteCode= '0143', @Code='1052', @Libelle='Absence évènement familial', @Intemperie='0', @TauxDecote='0', @NBHeuresDefautETAM='7.4', @NBHeuresMinETAM='1', @NBHeuresMaxETAM='7.4', @NBHeuresDefautCO='7.4', @NBHeuresMinCO='1', @NBHeuresMaxCO='7.4', @Actif='1'
 EXEC Fred_ToolBox_Code_Absence  @GroupeCode='GFTP' ,  @SocieteCode= '0143', @Code='1055', @Libelle='Congés sans solde', @Intemperie='0', @TauxDecote='0', @NBHeuresDefautETAM='7.4', @NBHeuresMinETAM='1', @NBHeuresMaxETAM='7.4', @NBHeuresDefautCO='7.4', @NBHeuresMinCO='1', @NBHeuresMaxCO='7.4', @Actif='1'
 EXEC Fred_ToolBox_Code_Absence  @GroupeCode='GFTP' ,  @SocieteCode= '0143', @Code='1060', @Libelle='Mise à pied', @Intemperie='0', @TauxDecote='0', @NBHeuresDefautETAM='7.4', @NBHeuresMinETAM='1', @NBHeuresMaxETAM='7.4', @NBHeuresDefautCO='7.4', @NBHeuresMinCO='1', @NBHeuresMaxCO='7.4', @Actif='1'
 EXEC Fred_ToolBox_Code_Absence  @GroupeCode='GFTP' ,  @SocieteCode= '0143', @Code='1065', @Libelle='Maladie Professionnelle non rémunérée', @Intemperie='0', @TauxDecote='0', @NBHeuresDefautETAM='7.4', @NBHeuresMinETAM='1', @NBHeuresMaxETAM='7.4', @NBHeuresDefautCO='7.4', @NBHeuresMinCO='1', @NBHeuresMaxCO='7.4', @Actif='1'
 EXEC Fred_ToolBox_Code_Absence  @GroupeCode='GFTP' ,  @SocieteCode= '0143', @Code='1075', @Libelle='Jour férié', @Intemperie='0', @TauxDecote='0', @NBHeuresDefautETAM='7.4', @NBHeuresMinETAM='1', @NBHeuresMaxETAM='7.4', @NBHeuresDefautCO='7.4', @NBHeuresMinCO='1', @NBHeuresMaxCO='7.4', @Actif='1'

-- MAJORATIONS
 EXEC Fred_ToolBox_Code_Majoration   @Code='1200', @Libelle='HS 125%', @EtatPublic='1', @IsActif='1', @GroupeCode= 'GFTP', @IsHeureNuit='1'
 EXEC Fred_ToolBox_Code_Majoration   @Code='1200', @Libelle='HS 150%', @EtatPublic='1', @IsActif='1', @GroupeCode= 'GFTP', @IsHeureNuit='1'
 EXEC Fred_ToolBox_Code_Majoration   @Code='1200', @Libelle='Heures de nuit', @EtatPublic='1', @IsActif='1', @GroupeCode= 'GFTP', @IsHeureNuit='1'
 EXEC Fred_ToolBox_Code_Majoration   @Code='1200', @Libelle='HS 125%', @EtatPublic='1', @IsActif='1', @GroupeCode= 'GFTP', @IsHeureNuit='1'
 EXEC Fred_ToolBox_Code_Majoration   @Code='1200', @Libelle='HS 150%', @EtatPublic='1', @IsActif='1', @GroupeCode= 'GFTP', @IsHeureNuit='1'
 EXEC Fred_ToolBox_Code_Majoration   @Code='1200', @Libelle='Heures de nuit', @EtatPublic='1', @IsActif='1', @GroupeCode= 'GFTP', @IsHeureNuit='1'


--CODES DEPLACEMENTS

 EXEC Fred_ToolBox_Code_Deplacement  @GroupeCode='GFTP' , @Code='1300', @Libelle='Nombre de panier', @KmMini='0', @KmMaxi='100', @IGD='0', @IndemniteForfaitaire='1', @Actif='1', @SocieteCode= '0143'
 EXEC Fred_ToolBox_Code_Deplacement  @GroupeCode='GFTP' , @Code='1301', @Libelle='Nombre de repas', @KmMini='0', @KmMaxi='100', @IGD='0', @IndemniteForfaitaire='1', @Actif='1', @SocieteCode= '0143'
 EXEC Fred_ToolBox_Code_Deplacement  @GroupeCode='GFTP' , @Code='1302', @Libelle='Nombre de transport Z2', @KmMini='0', @KmMaxi='100', @IGD='0', @IndemniteForfaitaire='1', @Actif='1', @SocieteCode= '0143'
 EXEC Fred_ToolBox_Code_Deplacement  @GroupeCode='GFTP' , @Code='1306', @Libelle='Nombre de trajet Z2', @KmMini='0', @KmMaxi='100', @IGD='0', @IndemniteForfaitaire='1', @Actif='1', @SocieteCode= '0143'
 EXEC Fred_ToolBox_Code_Deplacement  @GroupeCode='GFTP' , @Code='1307', @Libelle='Nombre de transport Z5', @KmMini='0', @KmMaxi='100', @IGD='0', @IndemniteForfaitaire='1', @Actif='1', @SocieteCode= '0143'
 EXEC Fred_ToolBox_Code_Deplacement  @GroupeCode='GFTP' , @Code='1308', @Libelle='Nombre de trajet Z5', @KmMini='0', @KmMaxi='100', @IGD='0', @IndemniteForfaitaire='1', @Actif='1', @SocieteCode= '0143'

