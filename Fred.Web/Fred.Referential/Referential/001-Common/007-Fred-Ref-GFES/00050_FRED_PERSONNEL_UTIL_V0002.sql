-- Création d'un compte adminSEMERU pour la société SEMERU.

DECLARE @LastPersonnelId INT;
DECLARE @LastExternalDir INT;

DECLARE @SocieteSatelecId INT;
SET @SocieteSatelecId = (SELECT SocieteId FROM FRED_SOCIETE WHERE Code='E002') --SEMERU

INSERT INTO FRED_PERSONNEL (Matricule, IsInterimaire, IsInterne, Nom, Prenom, Statut, DateEntree, DateCreation, SocieteId)
	VALUES('SEMERUADM', 0, 0, 'SEMERUADM', 'Admin', 'C', GETDATE()-1, GETDATE(), @SocieteSatelecId)
DECLARE @AdminFESId INT;
SET @LastPersonnelId = (SELECT PersonnelId FROM FRED_PERSONNEL WHERE Matricule = 'SEMERUADM')
SET @AdminFESId = @LastPersonnelId
INSERT INTO FRED_EXTERNALDIRECTORY (MotDePasse, IsActived) VALUES ('adminSEMERU', 1)
SET @LastExternalDir = (SELECT max(FayatAccessDirectoryId) FROM FRED_EXTERNALDIRECTORY)
INSERT INTO FRED_UTILISATEUR (Login, FayatAccessDirectoryId, IsDeleted, SuperAdmin, UtilisateurId)
	VALUES ('adminSEMERU', @LastExternalDir, 0, 0, @AdminFESId)