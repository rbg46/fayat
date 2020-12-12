
/*Ajout d'un Chapitre*/
-- --------------------------------------------------
-- FRED 2017 - R4 - SEPTEMBRE 2018 
-- TOOLBOX INJECTION CHAPITRES
-- MODOP :  EXEC Fred_ToolBox_Chapitre @GroupeCode = 'GFTP', @Code = '10', @Libelle='MO ENCADREMENT', @Version=1, @verbose=1
-- --------------------------------------------------
IF OBJECT_ID ( 'Fred_ToolBox_Chapitre', 'P' ) IS NOT NULL   
    DROP PROCEDURE Fred_ToolBox_Chapitre;  
GO  
CREATE PROCEDURE Fred_ToolBox_Chapitre 
        @GroupeCode varchar(500) = NULL,
        @Code varchar(50) = NULL,
        @Libelle varchar(500) = NULL,
        @Version bit = NULL,
        @verbose bit = NULL
AS   



    IF @verbose = 1 
    BEGIN
        PRINT '------------------------------'
        PRINT ' FAYAT IT - 2018 '
        PRINT 'INJECTION DES DONNEES CHAPITRES (PS Fred_ToolBox_Chapitre)'
        PRINT '------------------------------'
    END
    IF @version = 1 
    BEGIN
        PRINT 'Version 0.1'
    END

    DECLARE @GroupeId int; 
    SET @GroupeId = (SELECT GroupeId FROM FRED_GROUPE Where Code = @GroupeCode);
    

    DECLARE @CHECK int;
    SET @CHECK = (SELECT COUNT(*) FROM FRED_CHAPITRE WHERE Code = @Code AND GroupeId = @GroupeId);


    IF @CHECK = 0 
    BEGIN
            INSERT INTO FRED_CHAPITRE
            (
                [GroupeId],
                [Code],
                [Libelle],
                [DateCreation],
                [AuteurCreationId]

            )
            VALUES
            (
                @GroupeID,
                @Code,
                @Libelle,
                GETDATE(),
                1
            )

            IF @verbose = 1 
            BEGIN
                PRINT 'Chapitre ' + @code + '/'+ @libelle + ' ajouté'
            END
    END
    ELSE
    BEGIN
            
            DECLARE @ChapitreId int; 
            SET @ChapitreId = (SELECT chapitreId FROM FRED_CHAPITRE where Code = @Code AND GroupeId = @GroupeId);

            UPDATE FRED_CHAPITRE
            SET 
                Libelle = @Libelle,
                Code = @Code,
                DateModification = GETDATE(),
                AuteurModificationId = 1
            WHERE ChapitreId = @ChapitreId

            
            IF @verbose = 1 
            BEGIN
            PRINT 'Chapitre ' + @code + '/'+ @libelle + ' mis à jour'
            END

    END

GO 
