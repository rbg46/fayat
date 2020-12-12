-- Passage des personnels FES en utilisateurs
-- C EST DU TEST, ce script n'aurait pas du se trouver dans common
-- les données sont supprimées dans le script 00051_FRED_PERSONNEL_UTIL_V0002.sql

/*

CREATE PROCEDURE #ADD_USER
	@Matricule NVARCHAR(20),
	@Login VARCHAR(100),
	@SocieteId INT
AS
BEGIN
	DECLARE @UserId INT;
	DECLARE @LastExternalDir INT;
	
	SET @UserId = (SELECT PersonnelId FROM FRED_PERSONNEL WHERE Matricule = @Matricule and SocieteId=@SocieteId)
	INSERT INTO FRED_EXTERNALDIRECTORY (MotDePasse, isActived) VALUES (@Login, 1)
	SET @LastExternalDir = (SELECT max(FayatAccessDirectoryId) FROM FRED_EXTERNALDIRECTORY)
	INSERT INTO FRED_UTILISATEUR (Login, FayatAccessDirectoryId, IsDeleted, SuperAdmin, UtilisateurId)
		VALUES (@Login, @LastExternalDir, 0, 0, @UserId)
END
GO

DECLARE @SocieteSatelecId INT;
SET @SocieteSatelecId = (SELECT SocieteId FROM FRED_SOCIETE WHERE Code='E001')

EXEC #ADD_USER '1001', 'cNGuyen', @SocieteSatelecId;
EXEC #ADD_USER '1002', 'mVErkelens', @SocieteSatelecId;
EXEC #ADD_USER '1003', 'lRobert', @SocieteSatelecId;
EXEC #ADD_USER '1004', 'cBattisti', @SocieteSatelecId;
EXEC #ADD_USER '1005', 'sLabousse', @SocieteSatelecId;
EXEC #ADD_USER '1006', 'lCheriot', @SocieteSatelecId;
EXEC #ADD_USER '1007', 'aVerlaguet', @SocieteSatelecId;
EXEC #ADD_USER '1008', 'cGolenko', @SocieteSatelecId;
EXEC #ADD_USER '1009', 'bDeschamps', @SocieteSatelecId;
EXEC #ADD_USER '1010', 'dCocteau', @SocieteSatelecId;
EXEC #ADD_USER '1011', 'cLelouche', @SocieteSatelecId;
EXEC #ADD_USER '1012', 'rDeNiraux', @SocieteSatelecId;
EXEC #ADD_USER '1013', 'aRoussiere', @SocieteSatelecId;
EXEC #ADD_USER '1014', 'ePlatane', @SocieteSatelecId;
EXEC #ADD_USER '1015', 'mRodriguez', @SocieteSatelecId;
EXEC #ADD_USER '1016', 'yTriki', @SocieteSatelecId;
EXEC #ADD_USER '1017', 'nPoulizac', @SocieteSatelecId;
EXEC #ADD_USER '1018', 'jChatillon', @SocieteSatelecId;
EXEC #ADD_USER '1019', 'jPetit', @SocieteSatelecId;
EXEC #ADD_USER '1020', 'rGaillard', @SocieteSatelecId;
EXEC #ADD_USER '1021', 'oSamson', @SocieteSatelecId;
EXEC #ADD_USER '1022', 'sLefebvre', @SocieteSatelecId;
EXEC #ADD_USER '1023', 'aPereira', @SocieteSatelecId;
EXEC #ADD_USER '1024', 'mRoux', @SocieteSatelecId;
EXEC #ADD_USER '1025', 'cLemoine', @SocieteSatelecId;
EXEC #ADD_USER '1026', 'eLafitte', @SocieteSatelecId;
EXEC #ADD_USER '1027', 'kKhobzi', @SocieteSatelecId;
EXEC #ADD_USER '1028', 'tTrissac', @SocieteSatelecId;

DROP PROCEDURE #ADD_USER
*/