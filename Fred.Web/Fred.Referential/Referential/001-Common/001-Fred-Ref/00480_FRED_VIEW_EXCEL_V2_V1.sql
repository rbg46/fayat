
/****** Object:  View [dbo].[vw_FRED_Get_Commandes_For_Excel_V2]    Script Date: 18/06/2018 11:30:14 ******/
IF OBJECT_ID('[dbo].[vw_FRED_Get_Commandes_For_Excel_V2]', 'V') IS NOT NULL
    DROP VIEW [dbo].[vw_FRED_Get_Commandes_For_Excel_V2]
GO


/****** Object:  View [dbo].[vw_FRED_Get_Commandes_For_Excel_V2]    Script Date: 18/06/2018 11:30:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Marlène MITAULT
-- Create date: 18/06/2018
-- Description:	Liste les commandes de la socité RB
-- =============================================

CREATE VIEW [dbo].[vw_FRED_Get_Commandes_For_Excel_V2]
   AS  

    SELECT s.CodeSocieteComptable As Société, ec.code as Etab, ci.code+' - '+ci.libelle As CI, 
        co.[Date] As [Date Commande], 
        co.Numero As [Numéro Commande Interne FRED],		
        co.NumeroCommandeExterne As [Numéro Commande Externe],
        co.NumeroContratExterne As [Numéro Contrat],
        co.Libelle As [Libellé Commande], f.code As [Code Fournisseur], 
        f.libelle As [Libellé Fournisseur], 
        (MIN(dp.[Date])) As [Dernière récep. non rapprochée], 
        sr.[Montant Commande] As [Montant Commande], --(SUM(dp.QuantiteDepense * dp.PUHT)) As [Montant Commande],  
        d.Libelle as Devise
    FROM FRED_COMMANDE co 
        LEFT JOIN FRED_CI ci on ci.CiId = co.CiId
        LEFT JOIN FRED_SOCIETE s ON ci.SocieteId=s.SocieteId 
        LEFT JOIN FRED_ETABLISSEMENT_COMPTABLE ec ON ci.EtablissementComptableId =ec.EtablissementComptableId
        LEFT JOIN FRED_FOURNISSEUR f ON f.FournisseurId=co.FournisseurId
        LEFT JOIN FRED_DEVISE d ON d.DeviseId = co.DeviseId
        LEFT JOIN FRED_COMMANDE_LIGNE cl ON cl.commandeId = co.commandeId
        LEFT JOIN FRED_DEPENSE_ACHAT dp ON dp.CommandeLigneId = cl.CommandeLigneId
        LEFT JOIN (SELECT c1.CommandeId, SUM(c2.Quantite * c2.PUHT) As [Montant Commande]
            FROM FRED_COMMANDE c1 
            LEFT JOIN FRED_COMMANDE_LIGNE c2 ON c2.commandeId = c1.commandeId
            group by  c1.CommandeId) sr ON sr.CommandeId = co.CommandeId
    where s.SocieteId=1
        AND   (dp.DepenseTypeId = '1' or dp.DepenseTypeId is null) -- dépense de type Reception ou pas de reception
        AND (dp.QuantiteDepense <> 0 or dp.QuantiteDepense is null) and  (dp.FarAnnulee =0 or dp.FarAnnulee is null)
    group by s.CodeSocieteComptable, ec.code, ci.code+' - '+ci.libelle, co.[Date], co.Numero, co.NumeroCommandeExterne, co.NumeroContratExterne,
        co.Libelle, f.code, f.libelle, sr.[Montant Commande], d.Libelle;

GO
