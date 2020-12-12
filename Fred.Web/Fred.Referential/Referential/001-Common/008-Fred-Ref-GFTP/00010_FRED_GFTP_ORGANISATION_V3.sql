-- --------------------------------------------------
-- FRED 2018 - R4 - OCTOBRE 2018 
-- INJECTION DES DONNES POUR FRED - GROUPE FAYAT TP
-- --------------------------------------------------





-- --------------------------------------------------
-- CREATION DU GROUPE
-- --------------------------------------------------
EXEC Fred_ToolBox_Groupe @PoleCode='PTP',@Code='GFTP',@Libelle='FAYAT TP'




-- --------------------------------------------------
-- CREATION DES SOCIETES
-- --------------------------------------------------
 EXEC Fred_ToolBox_Societe   @GroupeCode= 'GFTP', @Code='0001', @CodeSocietePaye='FTP', @CodeSocieteComptable='NULL', @Libelle='FAYAT TP', @Adresse='Chemin Richelieu BP 20112', @Ville='Floirac Cedex', @CodePostal='33271', @SIRET='', @Externe='0', @Active='1', @MoisDebutExercice='10', @MoisFinExercice='9', @IsGenerationSamediCPActive='0', @ImportFacture='0', @CodeSocieteStorm='0001'
 EXEC Fred_ToolBox_Societe   @GroupeCode= 'GFTP', @Code='0143', @CodeSocietePaye='NULL', @CodeSocieteComptable='0143', @Libelle='SOMOPA', @Adresse='197, Avenue Clément Fayat BP 160 ', @Ville='Libourne Cedex', @CodePostal='33502', @SIRET='', @Externe='0', @Active='1', @MoisDebutExercice='10', @MoisFinExercice='9', @IsGenerationSamediCPActive='0', @ImportFacture='0',  @CodeSocieteStorm='0143'
 EXEC Fred_ToolBox_Societe   @GroupeCode= 'GFTP', @Code='I0001', @CodeSocietePaye='NULL', @CodeSocieteComptable='NULL', @Libelle='SOCIETE INTERIMAIRE FAYAT TP', @Adresse='Chemin Richelieu', @Ville='Floirac', @CodePostal='33271', @SIRET='', @Externe='0', @Active='1', @MoisDebutExercice='', @MoisFinExercice='', @IsGenerationSamediCPActive='0', @ImportFacture='0',  @CodeSocieteStorm='0001'
 EXEC Fred_ToolBox_Societe   @GroupeCode= 'GFTP', @Code='I0143', @CodeSocietePaye='NULL', @CodeSocieteComptable='NULL', @Libelle='SOCIETE INTERIMAIRE SOMOPA', @Adresse='197, Avenue Clément Fayat', @Ville='Libourne', @CodePostal='33502', @SIRET='', @Externe='0', @Active='1', @MoisDebutExercice='', @MoisFinExercice='', @IsGenerationSamediCPActive='0', @ImportFacture='0',  @CodeSocieteStorm='0169'



-- --------------------------------------------------
-- CREATION DES ETABLISSEMENTS COMPTABLES
-- --------------------------------------------------
EXEC Fred_ToolBox_Etablissement_Comptable  @GroupeCode= 'GFTP',   @SocieteCode= '0143', @Code='143A', @Libelle='SOMOPA', @Adresse='Chemin Richelieu', @Ville='Floirac CEDEX', @CodePostal='33271', @ModuleCommandeEnabled='0', @ModuleProductionEnabled='0', @IsDeleted='0'
EXEC Fred_ToolBox_Etablissement_Comptable @GroupeCode= 'GFTP',   @SocieteCode= '0001', @Code='001A', @Libelle='FAYAT TP', @Adresse='Avenue du Général de', @Ville='LIBOURNE Cedex', @CodePostal='33502', @ModuleCommandeEnabled='0', @ModuleProductionEnabled='0', @IsDeleted='0'
EXEC Fred_ToolBox_Etablissement_Comptable  @GroupeCode= 'GFTP',  @SocieteCode= '0001', @Code='001B', @Libelle='STAT DUGARCIN', @Adresse='Route de Gaugelin', @Ville='AUBIAC', @CodePostal='47310', @ModuleCommandeEnabled='0', @ModuleProductionEnabled='0', @IsDeleted='0'
EXEC Fred_ToolBox_Etablissement_Comptable @GroupeCode= 'GFTP',   @SocieteCode= '0001', @Code='001D', @Libelle='ADE TP', @Adresse='Route de Lesparre', @Ville='GAILLAN EN MEDOC', @CodePostal='33340', @ModuleCommandeEnabled='0', @ModuleProductionEnabled='0', @IsDeleted='0'


-- --------------------------------------------------
-- CREATION DES ETABLISSEMENTS DE PAIES
-- --------------------------------------------------

EXEC Fred_ToolBox_Etablissement_Paie  @GroupeCode='GFTP' , @Code='EP0143', @Libelle='Etablissement Paye SOMOPA 0143', @Adresse='Chemin Richelieu', @Ville='Floirac CEDEX', @CodePostal='33271', @Latitude='', @Longitude='',  @GestionIndemnites='1', @HorsRegion='1', @Actif='1', @SocieteCode= '0143' 
EXEC Fred_ToolBox_Etablissement_Paie  @GroupeCode='GFTP' , @Code='EP0001', @Libelle='Etablissement Paye FTP 0001', @Adresse='Avenue du Général de', @Ville='LIBOURNE Cedex', @CodePostal='33502', @Latitude='', @Longitude='',  @GestionIndemnites='1', @HorsRegion='1', @Actif='1', @SocieteCode= '0001'



