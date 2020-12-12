-- -------------------------------------------------- 
-- FRED 2017 - R4 - SEPTEMBRE 2018  
-- TOOLBOX MANAGEMENT TABLE  FRED_POLE 
-- MODOP : 
--    EXEC Fred_ToolBox_Pole @Code='PFES2',  @Libelle='Pôle FAYAT ENERGIES SERVICES',  @HoldingCode='FSA'
-- SELECT * FROM FRED_POLE
-- -------------------------------------------------- 

IF OBJECT_ID ( 'Fred_ToolBox_Pole', 'P' ) IS NOT NULL   
DROP PROCEDURE Fred_ToolBox_Pole; 
GO 
CREATE PROCEDURE Fred_ToolBox_Pole
 
 
		@verbose INT =NULL,
		@version INT =NULL,
		@Code  varchar(max), 
		@Libelle  varchar(max), 
		@HoldingCode  varchar(max)
 AS
DECLARE @ERROR INT;
		IF @verbose = 1 
		BEGIN
				PRINT '------------------------------'
				PRINT 'FAYAT IT - 2018 '
				PRINT 'INJECTION DES FRED_POLE (PS Fred_ToolBox_Pole)'
				PRINT '------------------------------'
		END
		IF @version = 1 
			BEGIN
				PRINT 'Version 0.1'
		END
 

 
DECLARE @PoleId INT
SET @PoleId= (SELECT PoleId FROM FRED_POLE WHERE Code = @Code)



	IF @PoleId IS NULL
		BEGIN
					-- ---------------------------- 
					-- ORDRE AJOUT 
					-- ---------------------------- 

					-- AJOUT POLE DANS LA TABLE ORGANISATION
					-- RECHERCHE DU POLE
					DECLARE @HoldingOrganisationId INT;
					SET @HoldingOrganisationId = (SELECT OrganisationId FROM FRED_HOLDING WHERE Code = @HoldingCode);
				
				
					-- AJOUT DANS LA TABLE ORGANISATION
					DECLARE @PoleOrganisationId INT;
					INSERT INTO FRED_ORGANISATION (TypeOrganisationId,PereId) VALUES ('2', @HoldingOrganisationId); 
					SET @PoleOrganisationId = @@IDENTITY;

			
					DECLARE @HoldingId INT;
					SET @HoldingId= (SELECT HoldingId FROM FRED_HOLDING WHERE Code = @HoldingCode)

					INSERT INTO FRED_POLE (
						Code, 
						Libelle, 
						HoldingId, 
						OrganisationId  
					)
					VALUES 
					(
						@Code, 
						@Libelle, 
						@HoldingId, 
						@PoleOrganisationId  
					);
		END
		ELSE
		BEGIN
			PRINT 'ERREUR : Pôle déjà existant'
		END

PRINT 'Fred_ToolBox_Pole v0.1 Ajoutée'


 -- ----------------------------------------------------------
 -- FIN PROCEDURE STOCKEE Fred_ToolBox_Pole  POUR TABLE  FRED_POLE
 -- ----------------------------------------------------------

GO