-- On insert un personnel pour Fred IE
INSERT INTO FRED_PERSONNEL (SocieteId,Matricule,IsInterimaire,IsInterne,Nom,Prenom,Statut,CategoriePerso,DateEntree,DateSortie,Adresse1,Adresse2,Adresse3,CodePostal,Ville,PaysId,LongitudeDomicile,LatitudeDomicile,Tel1,Tel2,Email,TypeRattachement,DateCreation,DateModification,DateSuppression,UtilisateurIdCreation,UtilisateurIdModification,UtilisateurIdSuppression,RessourceId,EtablissementPayeId,EtablissementRattachementId) 
VALUES ('2','FRED_IE','0','0','Fred IE',NULL,NULL,NULL,'01/01/2018',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'fred.ie@fci.fayat.com',NULL,'01/01/2018',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL); 

-- On récupère l'identifant du sous chapitre STORM
DECLARE @PersonnelId INTEGER;
SELECT @PersonnelId = PersonnelId FROM FRED_PERSONNEL WHERE Matricule = 'FRED_IE'

-- On insert un utilisateur pour Fred IE
INSERT INTO FRED_UTILISATEUR (UtilisateurId,Login,DateDerniereConnexion,FayatAccessDirectoryId,IsDeleted,DateCreation,DateModification,DateSupression,UtilisateurIdCreation,UtilisateurIdModification,UtilisateurIdSupression,SuperAdmin,Folio,CommandeManuelleAllowed) 
VALUES (@PersonnelId,'fred_ie',NULL,'2','0',GETDATE(),NULL,NULL,NULL,NULL,NULL,'1',NULL,'0'); 


