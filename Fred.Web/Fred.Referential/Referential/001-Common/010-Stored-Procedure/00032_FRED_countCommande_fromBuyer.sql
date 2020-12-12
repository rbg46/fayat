IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FRED_countCommande_fromBuyer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FRED_countCommande_fromBuyer]
GO

CREATE PROCEDURE [dbo].[FRED_countCommande_fromBuyer]
@ServerBuyer VARCHAR(MAX),
@EtablID VARCHAR(MAX),
@DateMinImport VARCHAR(MAX),
@DateMaxImport VARCHAR(MAX)	= NULL,
@NombreCommandes INT OUTPUT
AS
BEGIN
  
  DECLARE @query nvarchar(max);

  -- Récupération des commandes
  SET @query = N'
    SELECT	
        @NombreCommandes = count(Commande.ord_id)
    FROM	[SERVERBUYER].dbo.t_ord_order									Commande
        LEFT JOIN [SERVERBUYER].dbo.t_usr_contact						ContactCreateurCommande
            ON ContactCreateurCommande.contact_id = Commande.contact_id
        LEFT JOIN [SERVERBUYER].dbo.t_sup_supplier						Fournisseur
            ON Fournisseur.sup_id = Commande.sup_id
        LEFT JOIN [SERVERBUYER].dbo.t_sup_legacy						FournisseurLegacy
            ON FournisseurLegacy.sup_id = Fournisseur.sup_id
        LEFT JOIN [SERVERBUYER].dbo.t_bas_address						AdresseFourn
            ON AdresseFourn.adr_id = Fournisseur.adr_id_payment
        LEFT JOIN [SERVERBUYER].dbo.t_org_address						AdrLivInt
            ON AdrLivInt.oadr_id = Commande.oadr_id_delivery
        LEFT JOIN [SERVERBUYER].dbo.t_bas_address						AdresseLiv
            ON AdresseLiv.adr_id = AdrLivInt.adr_id
        LEFT JOIN [SERVERBUYER].dbo.t_org_address						AdrFacInt
            ON AdrFacInt.oadr_id = Commande.oadr_id_billing				
        LEFT JOIN [SERVERBUYER].dbo.t_bas_address						AdresseFac
            ON AdresseFac.adr_id = AdrFacInt.adr_id
        LEFT JOIN [SERVERBUYER].dbo.T_ord_basket						Panier
            ON Commande.basket_id = Panier.basket_id
        LEFT OUTER JOIN (Select max( Valid.wli_date_val) dateMax , Valid.x_id id from [SERVERBUYER].dbo.t_wfl_worklist Valid where Valid.tdesc_name=''t_ord_order''  
            group by Valid.x_id )									Validation
            ON Validation.id= Commande.ord_id 
    WHERE	1 = 1	
        AND (CONVERT(DATETIME, Commande.created, 103) >= CONVERT(DATETIME, ''[DATEMINIMPORT]'', 103) OR CONVERT(DATETIME, Commande.modified, 103) >= CONVERT(DATETIME, ''[DATEMINIMPORT]'', 103) )
        AND (CONVERT(DATETIME, Commande.created, 103) <= CONVERT(DATETIME, ''[DATEMAXIMPORT]'', 103) OR CONVERT(DATETIME, Commande.modified, 103) <= CONVERT(DATETIME, ''[DATEMAXIMPORT]'', 103) )
        AND Commande.status_code NOT IN (''can'',''del'')
        AND AdresseFac.adr_id is not null
        AND AdresseLiv.adr_id is not null
        AND Panier.bsubj_code is not null
        AND Validation.dateMax is not null
				AND Commande.unit_code_currency = ''EUR'' -- [TSA:30/11/2017] Par défaut, on ne prend que les commandes en EURO
        AND 
          ( 1 = 0
            OR Commande.status_code = ''ini''				
            OR Commande.status_code = ''ord''				
            OR Commande.status_code = ''rtrans''			
            )';

  SELECT @query = REPLACE(@query, '[SERVERBUYER]', @ServerBuyer);
  SELECT @query = REPLACE(@query, '[DATEMINIMPORT]', @DateMinImport);
  SELECT @query = REPLACE(@query, '[DATEMAXIMPORT]', @DateMaxImport);

  EXEC sp_executesql @query, N'@NombreCommandes int output', @NombreCommandes output

END
GO
