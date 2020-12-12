IF  EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[FRED_saveCommandeLigne_fromBUYER]') 
AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FRED_saveCommandeLigne_fromBUYER]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Stephen MILLER
-- Create date: 10/08/2016
-- Description:	SP d'insertion des commandes dans FRED depuis BUYER
-- =============================================
CREATE PROCEDURE FRED_saveCommandeLigne_fromBUYER
@iCommandeId				INT,
@iTacheId					INT,	  						
@sLibelle					NVARCHAR(500),            			      			
@iTypeDepenseId				INT,
@dQuantite					NUMERIC(11,2),
@dPrixUnitaireHT			NUMERIC(11,2),
@sUnite						nvarchar(10)


AS
BEGIN
	INSERT INTO dbo.FRED_COMMANDE_LIGNE (CommandeId, TacheId, RessourceId, Libelle, Quantite, PUHT, UniteId)
	SELECT
	@iCommandeId, @iTacheId, @iTypeDepenseId, @sLibelle, @dQuantite, COALESCE(@dPrixUnitaireHT, 0), @sUnite -- FIXME : le coalesce est temporaire pour permettre aux requêtes de sélection de s'exécuter même en cas d'absence de PUHT
END
GO