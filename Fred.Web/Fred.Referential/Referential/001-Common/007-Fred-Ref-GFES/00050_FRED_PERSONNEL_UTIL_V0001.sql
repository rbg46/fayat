/*

 NON ! Pas de données de tests dans Common !


DECLARE @LastPersonnelId INT;
DECLARE @LastExternalDir INT;

DECLARE @SocieteSatelecId INT;
SET @SocieteSatelecId = (SELECT SocieteId FROM FRED_SOCIETE WHERE Code='E001')

INSERT INTO FRED_PERSONNEL (Matricule, IsInterimaire, IsInterne, Nom, Prenom, Statut, DateEntree, DateCreation, SocieteId)
    VALUES('TFESADM', 0, 0, 'TESTFES', 'Admin', 'C', GETDATE()-1, GETDATE(), @SocieteSatelecId)
DECLARE @AdminFESId INT;
SET @LastPersonnelId = (SELECT PersonnelId FROM FRED_PERSONNEL WHERE Matricule = 'TFESADM')
SET @AdminFESId = @LastPersonnelId
INSERT INTO FRED_EXTERNALDIRECTORY (MotDePasse, isActived) VALUES ('adminFES', 1)
SET @LastExternalDir = (SELECT max(FayatAccessDirectoryId) FROM FRED_EXTERNALDIRECTORY)
INSERT INTO FRED_UTILISATEUR (Login, FayatAccessDirectoryId, IsDeleted, SuperAdmin, UtilisateurId)
    VALUES ('adminFES', @LastExternalDir, 0, 0, @AdminFESId)

INSERT INTO FRED_PERSONNEL (Matricule, IsInterimaire, IsInterne, Nom, Prenom, Statut, DateEntree, DateCreation, SocieteId, ManagerId)
    VALUES('TFESGPA', 0, 0, 'TESTFES', 'GestionnairePaye', 'C', GETDATE()-1, GETDATE(), @SocieteSatelecId, @AdminFESId)
SET @LastPersonnelId = (SELECT PersonnelId FROM FRED_PERSONNEL WHERE Matricule = 'TFESGPA')
INSERT INTO FRED_EXTERNALDIRECTORY (MotDePasse, isActived) VALUES ('gPaye', 1)
SET @LastExternalDir = (SELECT max(FayatAccessDirectoryId) FROM FRED_EXTERNALDIRECTORY)
INSERT INTO FRED_UTILISATEUR (Login, FayatAccessDirectoryId, IsDeleted, SuperAdmin, UtilisateurId)
    VALUES ('gPaye', @LastExternalDir, 0, 0, @LastPersonnelId)

INSERT INTO FRED_PERSONNEL (Matricule, IsInterimaire, IsInterne, Nom, Prenom, Statut, DateEntree, DateCreation, SocieteId, ManagerId)
    VALUES('TFESRCI', 0, 0, 'TESTFES', 'ResponsableCI', 'C', GETDATE()-1, GETDATE(), @SocieteSatelecId, @AdminFESId)
DECLARE @GestionnaireId INT;
SET @LastPersonnelId = (SELECT PersonnelId FROM FRED_PERSONNEL WHERE Matricule = 'TFESRCI')
SET @GestionnaireId = @LastPersonnelId
INSERT INTO FRED_EXTERNALDIRECTORY (MotDePasse, isActived) VALUES ('respCI', 1)
SET @LastExternalDir = (SELECT max(FayatAccessDirectoryId) FROM FRED_EXTERNALDIRECTORY)
INSERT INTO FRED_UTILISATEUR (Login, FayatAccessDirectoryId, IsDeleted, SuperAdmin, UtilisateurId)
    VALUES ('respCI', @LastExternalDir, 0, 0, @LastPersonnelId)

INSERT INTO FRED_PERSONNEL (Matricule, IsInterimaire, IsInterne, Nom, Prenom, Statut, DateEntree, DateCreation, SocieteId, ManagerId)
    VALUES('TFESCCH', 0, 0, 'TESTFES', 'ChefChantier', 'C', GETDATE()-1, GETDATE(), @SocieteSatelecId, @GestionnaireId)
DECLARE @ChefChantierId INT;
SET @LastPersonnelId = (SELECT PersonnelId FROM FRED_PERSONNEL WHERE Matricule = 'TFESCCH')
SET @ChefChantierId = @LastPersonnelId
INSERT INTO FRED_EXTERNALDIRECTORY (MotDePasse, isActived) VALUES ('cChantier', 1)
SET @LastExternalDir = (SELECT max(FayatAccessDirectoryId) FROM FRED_EXTERNALDIRECTORY)
INSERT INTO FRED_UTILISATEUR (Login, FayatAccessDirectoryId, IsDeleted, SuperAdmin, UtilisateurId)
    VALUES ('cChantier', @LastExternalDir, 0, 0, @LastPersonnelId)

INSERT INTO FRED_PERSONNEL (Matricule, IsInterimaire, IsInterne, Nom, Prenom, Statut, DateEntree, DateCreation, SocieteId, ManagerId)
    VALUES('TFESCCT', 0, 0, 'TESTFES', 'ConductTravaux', 'C', GETDATE()-1, GETDATE(), @SocieteSatelecId, @ChefChantierId)
SET @LastPersonnelId = (SELECT PersonnelId FROM FRED_PERSONNEL WHERE Matricule = 'TFESCCT')
INSERT INTO FRED_EXTERNALDIRECTORY (MotDePasse, isActived) VALUES ('cTravaux', 1)
SET @LastExternalDir = (SELECT max(FayatAccessDirectoryId) FROM FRED_EXTERNALDIRECTORY)
INSERT INTO FRED_UTILISATEUR (Login, FayatAccessDirectoryId, IsDeleted, SuperAdmin, UtilisateurId)
    VALUES ('cTravaux', @LastExternalDir, 0, 0, @LastPersonnelId)

    */