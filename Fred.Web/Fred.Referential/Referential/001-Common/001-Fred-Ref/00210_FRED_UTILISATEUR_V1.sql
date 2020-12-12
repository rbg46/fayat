-- --------------------------------------------------
-- FRED 2017 - INJECTION DES DONNES
-- --------------------------------------------------

--SET IDENTITY_INSERT FRED_UTILISATEUR ON;

INSERT INTO FRED_UTILISATEUR (UtilisateurId,Login,DateDerniereConnexion,FayatAccessDirectoryId,IsDeleted,DateCreation,DateModification,DateSupression,UtilisateurIdCreation,UtilisateurIdModification,UtilisateurIdSupression,SuperAdmin,Folio,CommandeManuelleAllowed) VALUES ('1','super_fred',NULL,'1','0',GETDATE(),NULL,NULL,NULL,NULL,NULL,'1',NULL,'0'); 

--SET IDENTITY_INSERT FRED_UTILISATEUR OFF;



-- --------------------------------------------------
-- MEP AVRIL 2018 - INJECTION DES DONNES
-- N/A
-- --------------------------------------------------