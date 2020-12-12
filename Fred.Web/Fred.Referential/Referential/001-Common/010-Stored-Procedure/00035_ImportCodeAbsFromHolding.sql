IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ImportCodeAbsFromHolding]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ImportCodeAbsFromHolding]
GO

CREATE PROCEDURE [dbo].[ImportCodeAbsFromHolding] @holdingId    INT, 
                                                   @idNewSociete INT 
AS 
  /****************************************************************************** 
   
  NAME        :  [dbo].[ImportCodeAbsFromHolding] 
   
  DESCRIPTION :  Importe les codes absences depuis [dbo].[FRED_CODE_ABSENCE_HOLDING] vers [dbo].[FRED_CODE_ABSENCE] 
             
  PARAMETER   : @holdingId, @idNewSociete 
   
  *******************************************************************************/ 
  BEGIN 
      SET nocount ON 

      DECLARE @LogType NVARCHAR(128) 
      DECLARE @Message NVARCHAR(128) 
      DECLARE @Detail NVARCHAR(max) 
      DECLARE @RC INT 
      DECLARE @Code NVARCHAR(max) 
      DECLARE @Libelle NVARCHAR(max) 
      DECLARE @Intemperie BIT 
      DECLARE @TauxDecote FLOAT 
      DECLARE @NBHeuresDefautETAM FLOAT 
      DECLARE @NBHeuresMinETAM FLOAT 
      DECLARE @NBHeuresMaxETAM FLOAT 
      DECLARE @NBHeuresDefautCO FLOAT 
      DECLARE @NBHeuresMinCO FLOAT 
      DECLARE @NBHeuresMaxCO FLOAT 
      DECLARE @Actif BIT 

      BEGIN try 
          DECLARE cursorrow CURSOR FOR 
            SELECT [Code], 
                   [Libelle], 
                   [Intemperie], 
                   [TauxDecote], 
                   [NBHeuresDefautETAM],
                   [NBHeuresMinETAM], 
                   [NBHeuresMaxETAM], 
                   [NBHeuresDefautCO], 
                   [NBHeuresMinCO], 
                   [NBHeuresMaxCO], 
                   [Actif] 
            FROM   [dbo].[FRED_CODE_ABSENCE_HOLDING] AS U 
            WHERE  U.[HoldingId] = @holdingId 

          OPEN cursorrow 

          FETCH next FROM cursorrow INTO @Code, @Libelle, @Intemperie, 
          @TauxDecote 
          , 
          @NBHeuresDefautETAM, @NBHeuresMinETAM, @NBHeuresMaxETAM, 
          @NBHeuresDefautCO, 
          @NBHeuresMinCO, @NBHeuresMaxCO, @Actif 

          WHILE @@FETCH_STATUS = 0 
            BEGIN 
                -- INSERT 
                BEGIN 
                    BEGIN try 
                        BEGIN TRANSACTION 

                        INSERT INTO [dbo].[FRED_CODE_ABSENCE] 
                                    ([SocieteId], 
                                     [Code], 
                                     [Libelle], 
                                     [Intemperie], 
                                     [TauxDecote], 
                                     [NBHeuresDefautETAM], 
                                     [NBHeuresMinETAM], 
                                     [NBHeuresMaxETAM], 
                                     [NBHeuresDefautCO], 
                                     [NBHeuresMinCO], 
                                     [NBHeuresMaxCO], 
                                     [Actif]) 
                        SELECT @idNewSociete, 
                               @Code, 
                               @Libelle, 
                               @Intemperie, 
                               @TauxDecote, 
                               @NBHeuresDefautETAM, 
                               @NBHeuresMinETAM, 
                               @NBHeuresMaxETAM, 
                               @NBHeuresDefautCO, 
                               @NBHeuresMinCO, 
                               @NBHeuresMaxCO, 
                               @Actif 

                        COMMIT TRANSACTION 
                    END try 

                    BEGIN catch 
                        ROLLBACK TRANSACTION 
                    END catch 
                END 

                FETCH next FROM cursorrow INTO @Code, @Libelle, @Intemperie, 
                @TauxDecote, 
                @NBHeuresDefautETAM, @NBHeuresMinETAM, @NBHeuresMaxETAM, 
                @NBHeuresDefautCO 
                , 
                @NBHeuresMinCO, @NBHeuresMaxCO, @Actif 
            END 

          CLOSE cursorrow 

          DEALLOCATE cursorrow 
      END try 

      BEGIN catch 
          -- Désalloue le curseur si besoin 
          IF Cursor_status('global', 'CursorRow') >= -1 
            BEGIN 
                DEALLOCATE cursorrow 
            END 
      END catch 
  END 