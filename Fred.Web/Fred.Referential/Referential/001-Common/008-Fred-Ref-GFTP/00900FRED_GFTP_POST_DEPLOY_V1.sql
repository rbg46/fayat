-- --------------------------------------------------
-- FRED 2018 - R4 - OCTOBRE 2018 
-- INJECTION DES DONNES POUR FRED - GROUPE FAYAT TP
-- --------------------------------------------------
-- SCRIPTS EXECUTE UNIQUEMENT MANUELLEMENT

-- INJECTION DES RÖLES

--EXEC Fred_ToolBox_Role @GroupeCode='GFTP',  @Code='DRA', @Libelle='Admin Appli', @CommandeSeuilDefaut='', @ModeLecture='0', @Actif='1', @SuperAdmin='0', @NiveauPaie='6', @NiveauCompta='0', @Description='', @SocieteCode= '0143', @CodeNomFamilier='ADM', @Specification=''
--EXEC Fred_ToolBox_Role   @Code='ARH', @Libelle='Admin Paye', @CommandeSeuilDefaut='', @ModeLecture='0', @Actif='1', @SuperAdmin='0', @NiveauPaie='5', @NiveauCompta='0', @Description='', @SocieteCode= '0143', @CodeNomFamilier='ARH', @Specification=''
--EXEC Fred_ToolBox_Role   @Code='DRA', @Libelle='Chef de chantier', @CommandeSeuilDefaut='', @ModeLecture='0', @Actif='1', @SuperAdmin='0', @NiveauPaie='1', @NiveauCompta='0', @Description='Profil Chef de Chantier pour SOMOPA (0143)', @SocieteCode= '0143', @CodeNomFamilier='CDC', @Specification=''
--EXEC Fred_ToolBox_Role   @Code='DRA', @Libelle='Conducteur de travaux', @CommandeSeuilDefaut='', @ModeLecture='0', @Actif='1', @SuperAdmin='0', @NiveauPaie='3', @NiveauCompta='0', @Description='Profil Conducteur de Travau pour SOMOPA (0143)', @SocieteCode= '0143', @CodeNomFamilier='CDT', @Specification=''
--EXEC Fred_ToolBox_Role   @Code='DRA', @Libelle='Gestionnaire Paye', @CommandeSeuilDefaut='', @ModeLecture='0', @Actif='1', @SuperAdmin='0', @NiveauPaie='5', @NiveauCompta='0', @Description='Profil Gestionnaire de Paye pour SOMOPA (0143)', @SocieteCode= '0143', @CodeNomFamilier='GSP', @Specification=''
--EXEC Fred_ToolBox_Role   @Code='DRA', @Libelle='Directeur', @CommandeSeuilDefaut='', @ModeLecture='0', @Actif='1', @SuperAdmin='0', @NiveauPaie='3', @NiveauCompta='5', @Description='Profil Directeur pour SOMOPA (0143)', @SocieteCode= '0143', @CodeNomFamilier='DIR', @Specification=''
--EXEC Fred_ToolBox_Role   @Code='DRA', @Libelle='Gestionnaire de Chantier', @CommandeSeuilDefaut='', @ModeLecture='0', @Actif='1', @SuperAdmin='0', @NiveauPaie='1', @NiveauCompta='0', @Description='Profil Gestionnaire de chantier pour SOMOPA (0143)', @SocieteCode= '0143', @CodeNomFamilier='OPE', @Specification=''

-- RECUPERATION DES RÖLES SUR ENVIRONNEMENT SOURCES
--EXEC Fred_ToolBox_Extract_Role_Fonctionnalite @SocieteCode='0143', @GroupeCode='GFTP'

-- SUPPRESSION DES RESSOURCES


-- SUPPRESSION DES SOUS-CHAPITRES


-- SUPPRESSION DES CHAPITRES



-- INJECTION DU NOUVEAU PLAN DE RESSOURCES

