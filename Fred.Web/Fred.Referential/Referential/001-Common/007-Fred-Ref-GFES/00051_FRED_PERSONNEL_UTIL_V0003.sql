-- Création d'un compte adminSATELEC pour la société SATELEC.
DECLARE @LastPersonnelId INT;
DECLARE @LastExternalDir INT;

DECLARE @SocieteSatelecId INT;
SET @SocieteSatelecId = (SELECT SocieteId FROM FRED_SOCIETE WHERE Code='E001')

INSERT INTO FRED_PERSONNEL (Matricule, IsInterimaire, IsInterne, Nom, Prenom, Statut, DateEntree, DateCreation, SocieteId)
    VALUES('SATELECADM', 0, 0, 'SATELECADM', 'Admin', 'C', GETDATE()-1, GETDATE(), @SocieteSatelecId)
DECLARE @AdminFESId INT;
SET @LastPersonnelId = (SELECT PersonnelId FROM FRED_PERSONNEL WHERE Matricule = 'SATELECADM')
SET @AdminFESId = @LastPersonnelId
INSERT INTO FRED_EXTERNALDIRECTORY (MotDePasse, IsActived) VALUES ('adminSATELEC', 1)
SET @LastExternalDir = (SELECT max(FayatAccessDirectoryId) FROM FRED_EXTERNALDIRECTORY)
INSERT INTO FRED_UTILISATEUR (Login, FayatAccessDirectoryId, IsDeleted, SuperAdmin, UtilisateurId)
    VALUES ('adminSATELEC', @LastExternalDir, 0, 0, @AdminFESId)