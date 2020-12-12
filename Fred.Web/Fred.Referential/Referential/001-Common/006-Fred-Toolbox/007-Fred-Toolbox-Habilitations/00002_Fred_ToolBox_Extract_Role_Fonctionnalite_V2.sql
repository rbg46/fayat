-- -------------------------------------------------- 
-- FRED 2017 - R4 - SEPTEMBRE 2018  
-- TOOLBOX EXTRCATION DES ROLES ET FONCTIONNALITE POUR INJECTION DANS DANS SYSTEME CIBLE
-- MODOP : 
--    EXEC Fred_ToolBox_Extract_Role_Fonctionnalite @SocieteCode='0143', @GroupeCode='GFTP'
-- -------------------------------------------------- 
 
IF OBJECT_ID ( 'Fred_ToolBox_Extract_Role_Fonctionnalite', 'P' ) IS NOT NULL   
DROP PROCEDURE Fred_ToolBox_Extract_Role_Fonctionnalite; 
GO 
CREATE PROCEDURE Fred_ToolBox_Extract_Role_Fonctionnalite
		@SocieteCode nvarchar(max),
		@GroupeCode nvarchar(max) 
 AS

DECLARE @RoleFonctionnaliteId INT;
DECLARE @FonctionnaliteId INT;
DECLARE @RoleId INT;
DECLARE @Mode  nvarchar(max);

	DECLARE C CURSOR FOR
		SELECT RoleFonctionnaliteId, FonctionnaliteId , RoleId, Mode FROM FRED_ROLE_FONCTIONNALITE where RoleId IN 
	    (
		    SELECT RoleID FROM FRED_ROLE WHERE SocieteId = (SELECT SocieteId FROM FRED_SOCIETE Where code = @SocieteCode AND GroupeId = (Select groupeId from FRED_GROUPE where code = @GroupeCode))
	    )
 
	OPEN C
 
	FETCH C INTO @RoleFonctionnaliteId, @FonctionnaliteId, @RoleId, @Mode
 
	WHILE @@FETCH_STATUS = 0
	BEGIN
	

		DECLARE @RoleCodeNomFamilier nvarchar(max);
		DECLARE @ModuleCode  nvarchar(max);
        DECLARE @FonctionnaliteCode  nvarchar(max);
		DECLARE @RoleCode  nvarchar(max);

	

		SET @ModuleCode = (SELECT m.Code FROM FRED_FONCTIONNALITE f inner join FRED_MODULE m on f.ModuleId=m.ModuleId Where f.FonctionnaliteId = @FonctionnaliteId);
		SET @FonctionnaliteCode = (SELECT Code FROM FRED_FONCTIONNALITE Where FonctionnaliteId = @FonctionnaliteId);
		SET @RoleCodeNomFamilier = (SELECT CodeNomFamilier FROM FRED_ROLE Where Roleid = @RoleId);
		SET @RoleCode = (SELECT Code FROM FRED_ROLE Where Roleid = @RoleId);



		PRINT 'Exec Fred_ToolBox_Role_Fonctionnalite '
		+' @ModuleCode='''+ CONVERT(nvarchar,@ModuleCode) + ''','
        +' @FonctionnaliteCode='''+ CONVERT(nvarchar,@FonctionnaliteCode) + ''','
		+' @RoleCodeNomFamilier='''+ CONVERT(nvarchar,@RoleCodeNomFamilier) + ''','
		+' @RoleCode='''+ CONVERT(nvarchar,@RoleCode) + ''','
		+' @Mode='+ CONVERT(nvarchar,@Mode) + ','+
		+' @GroupeCode='''+ CONVERT(nvarchar,@GroupeCode) + ''','
		+' @SocieteCode='''+ CONVERT(nvarchar,@SocieteCode) + ''''
		FETCH C INTO @RoleFonctionnaliteId, @FonctionnaliteId, @RoleId,  @Mode
	END
 
	CLOSE C
	DEALLOCATE C

GO
