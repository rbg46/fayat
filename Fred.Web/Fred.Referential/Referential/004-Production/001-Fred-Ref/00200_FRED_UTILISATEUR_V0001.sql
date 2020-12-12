
-- CREATION D UN COMPTE DE SERVICE POUR FRED WEB - COMPTE FRED IE SERVICE
-- UTILISER POUR LA SUPPRESSION DU CACHE DE FRED WEB PAR FRED IE 
DECLARE @MATRICULE varchar(10) ='FREDIESERV'
DECLARE @LOGIN  varchar(30) ='fredieser'

IF NOT EXISTS ( SELECT 1 FROM FRED_PERSONNEL WHERE Matricule = @MATRICULE)
BEGIN
   
    -- ON CREER UN ACTIVEDIRECTORY
    DECLARE @EXTERNALDIRECTORY_ID INTEGER ;
    INSERT INTO FRED_EXTERNALDIRECTORY (MotDePasse,DateExpiration,IsActived) VALUES ('B8?alxe3y*SEx3','01/01/2040','1');
    SELECT @EXTERNALDIRECTORY_ID = SCOPE_IDENTITY();

    -- OE RECHERCHE LA SOCIETE FIT
    DECLARE @SOCIETE_ID INTEGER;
    SELECT @SOCIETE_ID = SocieteId FROM FRED_SOCIETE WHERE Code = 'FIT'

    -- On insert un PERSONNEL pour Fred IE
    INSERT INTO FRED_PERSONNEL (SocieteId,Matricule,IsInterimaire,IsInterne,Nom,Prenom,Statut,CategoriePerso,DateEntree,DateSortie,Adresse1,Adresse2,Adresse3,CodePostal,Ville,PaysId,LongitudeDomicile,LatitudeDomicile,Tel1,Tel2,Email,TypeRattachement,DateCreation,DateModification,DateSuppression,UtilisateurIdCreation,UtilisateurIdModification,UtilisateurIdSuppression,RessourceId,EtablissementPayeId,EtablissementRattachementId) 
    VALUES (@SOCIETE_ID,@MATRICULE,'0','0','USER SERVICE FRED IE',NULL,NULL,NULL,'01/01/2019',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'fred.ie.service@fci.fayat.com',NULL,'01/01/2019',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL); 

    DECLARE @PersonnelId INTEGER;
    SELECT @PersonnelId = PersonnelId FROM FRED_PERSONNEL WHERE Matricule = @MATRICULE

    -- On insert un utilisateur pour Fred IE
    INSERT INTO FRED_UTILISATEUR (UtilisateurId,Login,DateDerniereConnexion,FayatAccessDirectoryId,IsDeleted,DateCreation,DateModification,DateSupression,UtilisateurIdCreation,UtilisateurIdModification,UtilisateurIdSupression,SuperAdmin,Folio,CommandeManuelleAllowed) 
    VALUES (@PersonnelId,@LOGIN,NULL,@EXTERNALDIRECTORY_ID,'0',GETDATE(),NULL,NULL,NULL,NULL,NULL,'0',NULL,'0'); 

END
