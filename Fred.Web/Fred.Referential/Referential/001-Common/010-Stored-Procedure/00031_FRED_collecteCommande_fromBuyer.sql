IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FRED_collecteCommande_fromBuyer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FRED_collecteCommande_fromBuyer]
GO

-- =============================================
-- Author:		RBM
-- Create date: 30/01/2015
-- Description:	Collecte des commandes depuis Buyer
-- Modification: SAE 19/10/2015	Stephen Miller 11/08/2016
--								Collecte des commandes depuis Buyer et injection dans FRED
-- Paramètres : 
-- =============================================
CREATE PROCEDURE [dbo].[FRED_collecteCommande_fromBuyer]
  @ServerBuyer VARCHAR(MAX),
  @EtablID VARCHAR(MAX),
  @ForceEnregistrement BIT,
  @DateMinImport VARCHAR(MAX),
  @DateMaxImport VARCHAR(MAX) = NULL,
  @DebugMode BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @IdCommande     VARCHAR(MAX);
    DECLARE @NumeroCommande    VARCHAR(MAX);
    DECLARE @LibelleCmd     VARCHAR(MAX);
    DECLARE @CodeFournisseurBuyer  VARCHAR(MAX);
    DECLARE @CodeFournisseurRVG   VARCHAR(MAX);
    DECLARE @FournisseurID    VARCHAR(MAX);
    DECLARE @SiretFournisseur   VARCHAR(MAX);
    DECLARE @NomFournisseur    VARCHAR(MAX);
    DECLARE @NomFournisseurBuyer  VARCHAR(MAX);
    DECLARE @TypeCommande    VARCHAR(MAX);
    DECLARE @DelaiLivraison    VARCHAR(MAX);
    DECLARE @Brouillon     BIT    = 0;
    DECLARE @MOConduite     BIT    = 0;
    DECLARE @EntretienMecanique     BIT    = 0;
    DECLARE @EntretienJournalier    BIT    = 0;
    DECLARE @Carburant              BIT    = 0;
    DECLARE @Lubrifiant             BIT    = 0;
    DECLARE @FraisAmortissement     BIT    = 0;
    DECLARE @ImpotsAssurance        BIT    = 0;
    DECLARE @ConditionSociete       VARCHAR(MAX);
    DECLARE @ConditionPrestation    VARCHAR(MAX);
    --DECLARE @FournisseurTMPLibelle  VARCHAR(MAX);
    --DECLARE @FournisseurTMPAdresse  VARCHAR(MAX);
    --DECLARE @FournisseurTMPVille    VARCHAR(MAX);
    --DECLARE @FournisseurTMPCodePostal VARCHAR(MAX);
    --DECLARE @FournisseurTMPNumTel   VARCHAR(MAX);
    DECLARE @ContactEntreID         VARCHAR(MAX) = NULL;
    DECLARE @ContactPersoID         VARCHAR(MAX) = NULL;
    DECLARE @ContactNumTel         VARCHAR(MAX) = NULL;
    DECLARE @SuiviEntreID         VARCHAR(MAX) = NULL;
    DECLARE @SuiviPersoID           VARCHAR(MAX) = NULL;
    DECLARE @SaisieEntreID          VARCHAR(MAX) = NULL;
    DECLARE @SaisiePersoID          VARCHAR(MAX) = NULL;
    DECLARE @SaisieNom           VARCHAR(MAX) = NULL;
    DECLARE @SaisiePrenom          VARCHAR(MAX) = NULL;
    DECLARE @SaisieEmail    VARCHAR(MAX) = NULL;
    DECLARE @ValidEntreID           VARCHAR(MAX) = NULL;
    DECLARE @ValidPersoID           VARCHAR(MAX) = NULL;
    DECLARE @DateSaisie            DATETIME  = GETDATE();
    DECLARE @DateModification      DATETIME  = NULL;
    DECLARE @DateValidation        DATETIME  = NULL;
    DECLARE @DateTransfert         DATETIME  = GETDATE();
    DECLARE @LivraisonAdresse      VARCHAR(MAX);
    DECLARE @LivraisonVille    VARCHAR(MAX);
    DECLARE @LivraisonCodePostal    VARCHAR(MAX);
    DECLARE @FacturationAdresse   VARCHAR(MAX);
    DECLARE @FacturationVille      VARCHAR(MAX);
    DECLARE @FacturationCodePostal  VARCHAR(MAX);
    DECLARE @DateSuppression        DATETIME  = NULL;
    DECLARE @DateCloture          DATETIME  = NULL;
    DECLARE @Etabl                VARCHAR(MAX);
    DECLARE @Externe         VARCHAR(MAX) = 'buyer';
    DECLARE @Justificatif            VARCHAR(MAX);
    DECLARE @CommentaireFournisseur  VARCHAR(MAX);
    DECLARE @CommentaireInterne      VARCHAR(MAX) = 'Provient de BUYER';
    DECLARE @CodeSociete          VARCHAR(MAX) = '1000';
    DECLARE @DateCreation    DATETIME;
    DECLARE @DateMiseADispo    DATETIME  = NULL;

    DECLARE @iNumeroLigne              INTEGER;
    DECLARE @sLibelleLigne              VARCHAR(250);
    DECLARE @sCodeAffaire              VARCHAR(6);
    DECLARE @sCodeTache                VARCHAR(6)  = '000000';   -- Tâche par défaut
    DECLARE @sCodeTydep                VARCHAR(20);
    DECLARE @sCodeDetail               VARCHAR(20);
    DECLARE @dQtt                  DECIMAL(11, 2);
    DECLARE @dPrixUnitaireHT           DECIMAL(11, 2);
    DECLARE @sUnite                    VARCHAR(10);
    DECLARE @dQuantiteRecep      DECIMAL(11, 2);

    DECLARE @sCodeNature    VARCHAR(MAX);
    DECLARE @sTypeCommBuyer    VARCHAR(MAX);

    DECLARE @sqlStatementCmde   VARCHAR(MAX);
    DECLARE @sqlStatementCmdeLigne  VARCHAR(MAX);
    -- DECLARE @sqlStatementFournisseur VARCHAR(MAX);
    DECLARE @cmdErreur     BIT    = 0;
    DECLARE @CiId      INT;
    DECLARE @TypeID      INT;
    DECLARE @StatutCommandeId   INT;
    DECLARE @StatutCommandeCode   CHAR(2);
    DECLARE @iCommandeId    INT;
    DECLARE @TacheId     INT;
    DECLARE @TypeDepenseId    INT;
		DECLARE @DeviseId    INT;

    IF (@DebugMode = 1 ) SET NOCOUNT OFF;
    IF (@DebugMode = 1 ) PRINT 'Création de la table temporaire de commande';
    -- Création d'une table temporaire pour les commandes
    CREATE TABLE #CommandeTemp (
        IdCommande     VARCHAR(MAX) NULL,
        NumeroCommande    VARCHAR(MAX) NULL,
        LIBELLE      VARCHAR(MAX) NULL,
        DATE_COMMANDE    DATETIME NULL,
        FOURN_ID     VARCHAR(MAX) NULL,
        FOURN_SIRET     VARCHAR(MAX) NULL,
        FOURN_NOM     VARCHAR(MAX) NULL,
        USR_NOM      VARCHAR(MAX) NULL,
        USR_PRENOM     VARCHAR(MAX) NULL,
        USR_EMAIL     VARCHAR(MAX) NULL,
        DELAI_LIVRAISON    VARCHAR(MAX) NULL,
        DATE_MISE_A_DISPO   DATETIME NULL,
        FOURN_TMP_LIBELLE   VARCHAR(MAX) NULL,
        FOURN_TMP_ADRESSE   VARCHAR(MAX) NULL,
        FOURN_TMP_VILLE    VARCHAR(MAX) NULL,
        FOURN_TMP_CPOSTALE   VARCHAR(MAX) NULL,
        FOURN_TMP_NUMTEL   VARCHAR(MAX) NULL,
        /*ENTRE_ID_CONTACT varchar(2) NULL,
        PERSO_ID_CONTACT varchar(10) NULL,
        ENTRE_ID_SUIVI varchar(2) NULL,
        PERSO_ID_SUIVI varchar(10) NULL,
        ENTRE_ID_SAISIE varchar(2) NULL,
        PERSO_ID_SAISIE varchar(10) NULL,
        ENTRE_ID_VALIDEUR varchar(2) NULL,
        PERSO_ID_VALIDEUR varchar(10) NULL,*/
        DATE_SAISIE     DATETIME NULL,
        DATE_MODIF     DATETIME NULL,
        LIVRAISON_ADRESSE   VARCHAR(MAX) NULL,
        LIVRAISON_VILLE    VARCHAR(MAX) NULL,
        LIVRAISON_CPOSTALE   VARCHAR(MAX) NULL,
        FACTURATION_ADRESSE   VARCHAR(MAX) NULL,
        FACTURATION_VILLE   VARCHAR(MAX) NULL,
        FACTURATION_CPOSTALE  VARCHAR(MAX) NULL,
        COMMENTAIRE_FOURN   VARCHAR(MAX) NULL,
        TYPE_COMMANDE    VARCHAR(MAX) NULL,
        DATE_VALIDATION    DATETIME NULL
    )

    IF (@DebugMode = 1 ) PRINT 'Création de la table temporaire de lignes de commande';
    --creation d'une table temporaire pour les lignes
    CREATE TABLE #CommandeLigneTemp(
        IdCommande     VARCHAR(MAX) NULL,
        LIGNE_NUM     INTEGER  NULL,
        LIBELLE      VARCHAR(MAX) NULL,
        AFFAI_ID     VARCHAR(MAX) NULL,
        NATURE      VARCHAR(MAX) NULL,
        CMDE_LIGNE_QTE    NUMERIC(11, 2) NULL,
        CMDE_LIGNE_PUHT    NUMERIC(11, 2) NULL,
        CMDE_LIGNE_UNITE   VARCHAR(10) NULL
    )

    -- Récupération des commandes
    SET @sqlStatementCmde = '
        INSERT INTO #CommandeTemp
        SELECT   Commande.ord_id          As IdCommande
                , Commande.ord_ref          As NumeroCommande
                , Commande.ord_label_fr         As Libelle
                , Commande.created           As DateCreation
                , FournisseurLegacy.sup_auxiliary_account    As CodeFournisseur
                , Fournisseur.sup_siret         As SiretFournisseur
                , Fournisseur.sup_name_fr        As NomFournisseur
                , ContactCreateurCommande.contact_lastname    As NomSaisisseur
                , ContactCreateurCommande.contact_firstname    As PrenomSaisisseur
                , ContactCreateurCommande.contact_email     As EmailSaisisseur
                , Case when Commande.prev_delivery_date is null or Commande.ord_order_date is null or Commande.prev_delivery_date < Commande.ord_order_date then null else Cast(DATEDIFF(day, Commande.ord_order_date, Commande.prev_delivery_date) AS VARCHAR) end AS DelaiLivraison    -- date de livraison prévisionnelle - date commande
                , Commande.prev_delivery_date       AS DateMiseADispo
                , Fournisseur.sup_name_fr        AS FournisseurTMPLibelle
                , AdresseFourn.adr_num+'' ''+AdresseFourn.adr_voie  AS FournisseurTMPAdresse
                , AdresseFourn.zip_label_fr        AS FournisseurTMPVille
                , AdresseFourn.zip_code         AS FournisseurTMPCodePostal
                , Fournisseur.sup_phone         AS FournisseurTMPNumTel
                , Commande.ord_order_date        AS DateSaisie
                , Commande.modified          AS DateModification
                , AdresseLiv.adr_num+'' ''+AdresseLiv.adr_voie   AS LivraisonAdresse
                , AdresseLiv.zip_label_fr        AS LivraisonVille
                , AdresseLiv.zip_code         AS LivraisonCodePostal
                , AdresseFac.adr_num+'' ''+AdresseFac.adr_voie   AS FacturationAdresse
                , AdresseFac.zip_label_fr         AS FacturationVille
                , AdresseFac.zip_code         AS FacturationCodePostal
                , Commande.ord_comment_fr         AS CommentaireFournisseur
                , Panier.bsubj_code          As TypeCommande
                , Validation.dateMax         As DateValidation								

        FROM [SERVERBUYER].dbo.t_ord_order         Commande
                LEFT JOIN [SERVERBUYER].dbo.t_usr_contact      ContactCreateurCommande
                        ON ContactCreateurCommande.contact_id = Commande.contact_id
                LEFT JOIN [SERVERBUYER].dbo.t_sup_supplier      Fournisseur
                        ON Fournisseur.sup_id = Commande.sup_id
                LEFT JOIN [SERVERBUYER].dbo.t_sup_legacy      FournisseurLegacy
                        ON FournisseurLegacy.sup_id = Fournisseur.sup_id
                LEFT JOIN [SERVERBUYER].dbo.t_bas_address      AdresseFourn
                        ON AdresseFourn.adr_id = Fournisseur.adr_id_payment
                LEFT JOIN [SERVERBUYER].dbo.t_org_address      AdrLivInt
                        ON AdrLivInt.oadr_id = Commande.oadr_id_delivery
                LEFT JOIN [SERVERBUYER].dbo.t_bas_address      AdresseLiv
                        ON AdresseLiv.adr_id = AdrLivInt.adr_id
                LEFT JOIN [SERVERBUYER].dbo.t_org_address      AdrFacInt
                        ON AdrFacInt.oadr_id = Commande.oadr_id_billing
                LEFT JOIN [SERVERBUYER].dbo.t_bas_address      AdresseFac
                        ON AdresseFac.adr_id = AdrFacInt.adr_id
                LEFT JOIN [SERVERBUYER].dbo.T_ord_basket      Panier
                        ON Commande.basket_id = Panier.basket_id
                LEFT OUTER JOIN (Select max( Valid.wli_date_val) dateMax , Valid.x_id id from [SERVERBUYER].dbo.t_wfl_worklist Valid where Valid.tdesc_name=''t_ord_order''
                        group by Valid.x_id )         Validation
                        ON Validation.id= Commande.ord_id
        WHERE 1 = 1
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
    SELECT @sqlStatementCmde = REPLACE(@sqlStatementCmde, '[SERVERBUYER]', @ServerBuyer);
    SELECT @sqlStatementCmde = REPLACE(@sqlStatementCmde, '[DATEMINIMPORT]', @DateMinImport);
    SELECT @sqlStatementCmde = REPLACE(@sqlStatementCmde, '[DATEMAXIMPORT]', @DateMaxImport);
    IF (@DebugMode = 1 ) PRINT 'Récupération des commandes Buyer';
    IF (@DebugMode = 1 ) PRINT @sqlStatementCmde
    EXEC(@sqlStatementCmde)

    -- Récupération des lignes de commande
    SET @sqlStatementCmdeLigne = '
        INSERT INTO #CommandeLigneTemp
        SELECT Distinct LigneCommande.ord_id       As IdCommande
                , LigneCommande.oitem_seq        As IdLigne
                , LigneCommande.oitem_label        As Libelle
                , Alloc.cce_code          As Affaire
                , max(Nature.anat_code)         As Nature
                , LigneCommande.oitem_quantity       As Qte
                , LigneCommande.oitem_price_entry        As PU
                , LigneCommande.unit_code        As Unité

        FROM [SERVERBUYER].dbo.t_ord_item      LigneCommande
                LEFT JOIN [SERVERBUYER].dbo.t_ord_allocation Alloc ON Alloc.oitem_id = LigneCommande.oitem_id
                LEFT JOIN [SERVERBUYER].dbo.t_ord_account_analytical_nature nature on nature.anat_id = alloc.anat_id

        WHERE 1 = 1
                AND LigneCommande.ord_id IN (Select distinct IdCommande From #CommandeTemp)
                GROUP BY LigneCommande.ord_id, LigneCommande.oitem_seq, LigneCommande.oitem_label, Alloc.cce_code, LigneCommande.oitem_quantity, LigneCommande.oitem_price_entry, LigneCommande.unit_code';
    SELECT @sqlStatementCmdeLigne = REPLACE(@sqlStatementCmdeLigne, '[SERVERBUYER]', @ServerBuyer);

    IF (@DebugMode = 1 ) PRINT 'Récupération des lignes de commandes Buyer';
    IF (@DebugMode = 1 ) PRINT @sqlStatementCmdeLigne
    EXEC(@sqlStatementCmdeLigne)

    -- Curseur sur les commandes
    DECLARE curCommande SCROLL CURSOR FOR
            SELECT  IdCommande,
                    NumeroCommande,
                    LEFT(LIBELLE,250),
                    DATE_COMMANDE,
                    FOURN_ID,
                    FOURN_SIRET,
                    FOURN_NOM,
                    DELAI_LIVRAISON,
                    DATE_MISE_A_DISPO,
                    --FOURN_TMP_LIBELLE,
                    --FOURN_TMP_ADRESSE,
                    --FOURN_TMP_VILLE,
                    --FOURN_TMP_CPOSTALE,
                    --FOURN_TMP_NUMTEL,
                    DATE_SAISIE,
                    DATE_MODIF,
                    LIVRAISON_ADRESSE,
                    LIVRAISON_VILLE,
                    LIVRAISON_CPOSTALE,
                    FACTURATION_ADRESSE,
                    FACTURATION_VILLE,
                    FACTURATION_CPOSTALE,
                    COMMENTAIRE_FOURN,
                    TYPE_COMMANDE,
                    DATE_VALIDATION,
                    USR_NOM,
                    USR_PRENOM,
                    USR_EMAIL
            FROM #CommandeTemp



    -- Vérification et gestion d'erreurs
    -- Parcourt des commandes à vérifier
    OPEN curCommande
    FETCH First FROM curCommande INTO @IdCommande, @NumeroCommande, @LibelleCmd, @DateCreation, @CodeFournisseurBuyer, @SiretFournisseur, @NomFournisseurBuyer, @DelaiLivraison,@DateMiseADispo, --@FournisseurTMPLibelle,@FournisseurTMPAdresse,@FournisseurTMPVille,@FournisseurTMPCodePostal,@FournisseurTMPNumTel,
                                    @DateSaisie, @DateModification, @LivraisonAdresse, @LivraisonVille, @LivraisonCodePostal, @FacturationAdresse, @FacturationVille, @FacturationCodePostal, @CommentaireFournisseur,@sTypeCommBuyer,@DateValidation,@SaisieNom, @SaisiePrenom, @SaisieEmail
    WHILE (@@Fetch_status <> -1)
    BEGIN
        IF (@DebugMode = 1 ) PRINT '    - Vérification de la COMMANDE ' + CAST(@NumeroCommande AS VARCHAR(MAX));
        -- Vérification des commandes déjà existante
        IF (@ForceEnregistrement = 0) AND EXISTS (SELECT 1 FROM FRED_COMMANDE FC WHERE FC.Numero = @NumeroCommande )
        BEGIN
            -- on supprime la commande et ses commandes lignes
            DELETE FROM #CommandeLigneTemp WHERE IdCommande=@IdCommande
            DELETE FROM #CommandeTemp WHERE IdCommande=@IdCommande
            -- Pas enregistré cause doublon
            --INSERT INTO CMDE_ERREUR VALUES(@NumeroCommande,NULL,LEFT(@LibelleCmd + ' - '+ @sLibelleLigne,250),@sCodeAffaire,'COMMANDE EXISTANTE' ,GETDATE())
            -- SET @cmdErreur = 1;
            IF (@DebugMode = 1 ) PRINT '    - COMMANDE ' + CAST(@NumeroCommande AS VARCHAR(MAX)) +' DÉJÀ EXISTANTE';
        END
        ELSE
        BEGIN
            -- Parcourt des lignes de commande à vérifier
            -- Curseur sur les lignes de commandes
            DECLARE curLigneCommande SCROLL CURSOR FOR
                SELECT   LIGNE_NUM
                        , LEFT(LIBELLE,250)
                        , AFFAI_ID
                        , NATURE
                        , CMDE_LIGNE_QTE
                        , CMDE_LIGNE_PUHT
                        , CMDE_LIGNE_UNITE
                FROM #CommandeLigneTemp
                WHERE IdCommande=@IdCommande
            OPEN curLigneCommande
            FETCH First FROM curLigneCommande INTO @iNumeroLigne, @sLibelleLigne, @sCodeAffaire, @sCodeNature, @dQtt, @dPrixUnitaireHT, @sUnite
            WHILE (@@Fetch_status <> -1)
            BEGIN
                IF (@DebugMode = 1 ) PRINT '    - Vérification de la LIGNE DE COMMANDE ' +  CAST(@NumeroCommande AS VARCHAR(MAX)) +'-'+  CAST(@iNumeroLigne AS VARCHAR(MAX));

                IF (@DebugMode = 1 ) PRINT '  | Traitement des lignes en erreur';
                    -- On vérifie si l'affaire existe (sinon pb de FK lors de l'insertion), sinon on insert pas
                IF NOT EXISTS (select 1 from FRED_CI FC where CAST(FC.Code AS VARCHAR(MAX))=@sCodeAffaire)
                BEGIN
                 -- on supprime la commande et ses commandes lignes
                 DELETE FROM #CommandeLigneTemp WHERE IdCommande=@IdCommande
                 DELETE FROM #CommandeTemp WHERE IdCommande=@IdCommande
                 -- si on l'affaire n'existe pas, on ajoute la ligne dans la table d'erreur
                 -- INSERT INTO CMDE_ERREUR VALUES(@NumeroCommande, @iNumeroLigne, LEFT(@LibelleCmd + ' - '+ @sLibelleLigne,250), @sCodeAffaire, 'AFFAIRE INCONNU : '+ ISNULL(@sCodeAffaire, 'L''AFFAIRE N''EXISTE PAS DANS LA BASE RVG') ,GETDATE());
                 -- SET @cmdErreur = 1;
                 IF (@DebugMode = 1 ) PRINT '    - AFFAIRE INCONNU : '+ ISNULL(@sCodeAffaire, 'L''AFFAIRE N''EXISTE PAS DANS LA BASE FRED');
                END
                --ELSE IF (@EtablID IS NOT NULL AND NOT EXISTS (select 1 from FRED_CI FC where FC.Code=@sCodeAffaire AND FC.EtablissementComptableId=@EtablID) )
                --BEGIN

                -- -- on supprime la commande et ses commandes lignes
                -- DELETE FROM #CommandeLigneTemp WHERE IdCommande=@IdCommande
                -- DELETE FROM #CommandeTemp WHERE IdCommande=@IdCommande
                -- -- Pas enregistré cause affaire n'appartient pas à l'etabl
                -- --INSERT INTO CMDE_ERREUR VALUES(@NumeroCommande, @iNumeroLigne, LEFT(@LibelleCmd + ' - '+ @sLibelleLigne,250), @sCodeAffaire, 'COMMANDE HORS ETABLISSEMENT : '+ ISNULL(@EtablID, 'PAS D''ETABLISSEMENT PROVENANT DE BUYER') ,GETDATE());
                -- -- SET @cmdErreur = 1;
                -- IF (@DebugMode = 1 ) PRINT '    - COMMANDE HORS ETABLISSEMENT : '+ ISNULL(@EtablID, 'PAS D''ETABLISSEMENT PROVENANT DE BUYER');
                --END
                ELSE If @dQtt is null OR @dPrixUnitaireHT is null
                BEGIN

                    -- on supprime la commande et ses commandes lignes
                    DELETE FROM #CommandeLigneTemp WHERE IdCommande=@IdCommande
                    DELETE FROM #CommandeTemp WHERE IdCommande=@IdCommande
                    --Pas enregistré cause prix ou qtt null
                    --INSERT INTO CMDE_ERREUR VALUES(@NumeroCommande, @iNumeroLigne, LEFT(@LibelleCmd + ' - '+ @sLibelleLigne,250), @sCodeAffaire, 'QUANTITÉ OU PRIX UNITAIRE INEXISTANT' ,GETDATE());
                    -- SET @cmdErreur = 1;
                    IF (@DebugMode = 1 ) PRINT '    - QUANTITÉ OU PRIX UNITAIRE INEXISTANT';
                END

                FETCH NEXT FROM curLigneCommande INTO @iNumeroLigne, @sLibelleLigne, @sCodeAffaire, @sCodeNature, @dQtt, @dPrixUnitaireHT, @sUnite
            END
            CLOSE curLigneCommande
            DEALLOCATE curLigneCommande
        END

        FETCH NEXT FROM curCommande INTO @IdCommande, @NumeroCommande, @LibelleCmd, @DateCreation, @CodeFournisseurBuyer, @SiretFournisseur, @NomFournisseurBuyer, @DelaiLivraison,@DateMiseADispo, --@FournisseurTMPLibelle,@FournisseurTMPAdresse,@FournisseurTMPVille,@FournisseurTMPCodePostal,@FournisseurTMPNumTel
                                    @DateSaisie, @DateModification, @LivraisonAdresse, @LivraisonVille, @LivraisonCodePostal, @FacturationAdresse, @FacturationVille, @FacturationCodePostal, @CommentaireFournisseur,@sTypeCommBuyer,@DateValidation,@SaisieNom, @SaisiePrenom, @SaisieEmail
    END
    CLOSE curCommande

    -- Insertion des commande et des lignes de commande
    OPEN curCommande
    FETCH First FROM curCommande INTO @IdCommande, @NumeroCommande, @LibelleCmd, @DateCreation, @CodeFournisseurBuyer, @SiretFournisseur, @NomFournisseurBuyer, @DelaiLivraison,@DateMiseADispo, --@FournisseurTMPLibelle,@FournisseurTMPAdresse,@FournisseurTMPVille,@FournisseurTMPCodePostal,@FournisseurTMPNumTel
                                    @DateSaisie, @DateModification, @LivraisonAdresse, @LivraisonVille, @LivraisonCodePostal, @FacturationAdresse, @FacturationVille, @FacturationCodePostal, @CommentaireFournisseur,@sTypeCommBuyer,@DateValidation,@SaisieNom, @SaisiePrenom, @SaisieEmail
    WHILE (@@Fetch_status <> -1)
    BEGIN
        -- SET @cmdErreur = 0;
        -- BEGIN TRANSACTION CMD;

        IF (@ForceEnregistrement=1) OR NOT EXISTS (SELECT 1 FROM FRED_COMMANDE FC WHERE FC.Numero = @NumeroCommande)
        BEGIN
            -- Traitement de la commande
            IF (@DebugMode = 1 ) PRINT ' /********************* Traitement de la commande ' + CAST(@NumeroCommande AS VARCHAR(MAX)) + '*********************\ ';

            /* DEBUT INSTRUCTIONS SPECIFIQUES A FRED */

            /* Récupération du valideur*/
            /* Le saisisseur (AuteurCreation dans FRED) est l'appli BUYER par défaut;*/
            /* Pour le moment le valideur est l'utilisateur 'fayat' */
            /* Pour les autres infos, valideur, "suiveur", pour le moment NULL...*/
            --SET @SuiviPersoID = (SELECT FPG.PersonnelId FROM FRED_PERSONNEL_GLOBAL FPG WHERE (LTRIM(RTRIM(UPPER(FPG.Email)))) = (LTRIM(RTRIM(UPPER(@SaisieEmail)))) );
            --SET @ValidPersoID = (SELECT FPG.PersonnelId FROM FRED_PERSONNEL_GLOBAL FPG WHERE (LTRIM(RTRIM(UPPER(FPG.Email)))) = (LTRIM(RTRIM(UPPER(@SaisieEmail)))) );
            SET @SuiviPersoID = NULL; -- sur PERSONNEL_GLOBAL
            SET @ValidPersoID = (SELECT UtilisateurId FROM FRED_UTILISATEUR WHERE LOGIN like 'super_fred'); -- // FIXME [TSA:30/11/2017] par défaut, utilisateur Admin de FRED. Les utilisateurs Buyer ne sont pas forcément dans FRED.
																																																						-- Si l'utilisateur a bien été importé dans FRED, il a été importé en tant que FRED_PERSONNEL. Or, la commande demande des FRED_UTILISATEUR
            SET @SaisiePersoID = (SELECT UtilisateurId FROM FRED_UTILISATEUR WHERE LOGIN like 'super_fred');; -- // FIXME [TSA:30/11/2017]: idem qu'au dessus
            SET @ContactPersoID = NULL; -- sur PERSONNEL_GLOBAL

            /* Récupération de l'ID de CI */
            /* On récupère l'ID affaire de la première ligne de commande */
            
            SET @CiId = (SELECT CiId FROM FRED_CI WHERE Code Like (SELECT TOP 1 AFFAI_ID FROM #CommandeLigneTemp WHERE IdCommande=@IdCommande AND ISNUMERIC(AFFAI_ID) = 1));

            /* Initialisation DU STATUTCOMMANDEID */
            /* Valorisé en fonction d'un statutCommandeCode lui même valorisé en fonction de diverses informations... */
            SET @StatutCommandeCode = 'AV'
            IF @Brouillon = 1
                SET @StatutCommandeCode = 'BR'
            ELSE
            BEGIN
                IF @DateValidation IS NOT NULL
                    SET @StatutCommandeCode = 'VA'

                IF @DateCloture IS NOT NULL
                    SET @StatutCommandeCode = 'CL'
            END

            SET @StatutCommandeId = (SELECT FSC.StatutCommandeId FROM FRED_STATUT_COMMANDE FSC WHERE FSC.Code = @StatutCommandeCode)

            /* FIN INSTRUCTIONS SPECIFIQUES A FRED */

            /*  Récupération du fournisseur  */
            SET @CodeFournisseurRVG = NULL;
            SET @NomFournisseur = NULL;
            IF (@DebugMode = 1 ) PRINT '  | Code fournisseur Buyer à matcher : ' + CAST(ISNULL(@CodeFournisseurBuyer,'NULL') AS VARCHAR(MAX));

            -- Matching par code RAUX
            IF (@DebugMode = 1 ) PRINT '    - Tentative de matching du fournisseur par code RAUX : ' + CAST(ISNULL(@CodeFournisseurBuyer, 'PAS DE CODE RAUX BUYER') AS VARCHAR(MAX));
            --SELECT TOP 1 @CodeFournisseurRVG = FOURN_ID, @NomFournisseur = FOURN_LIBELLE FROM FOURN WHERE FOURN_ID = LTRIM(RTRIM(@CodeFournisseurBuyer));
            IF (@DebugMode = 1 ) PRINT '    - ' + CAST(@@ROWCOUNT AS VARCHAR(MAX)) + ' lignes trouvées';
                IF (@DebugMode = 1 ) PRINT '    - Code fournisseur RVG : ' + CAST(ISNULL(@CodeFournisseurRVG, 'NON') + ' TROUVÉ' AS VARCHAR(MAX));

             -- Matching par code SIRET
            IF (@CodeFournisseurRVG IS NULL)
             BEGIN
                IF (@DebugMode = 1 ) PRINT '    - Tentative de matching du fournisseur par code SIRET : ' + CAST(ISNULL(@SiretFournisseur, 'PAS DE CODE SIRET BUYER') AS VARCHAR(MAX));
                --SELECT TOP 1 @CodeFournisseurRVG = FOURN_ID, @NomFournisseur = FOURN_LIBELLE
                --FROM FOURN           FournisseurRVG
                --WHERE 1 = 1
                --  AND LTRIM(RTRIM(FournisseurRVG.FOURN_SIREN)) + RIGHT('00000' + LTRIM(RTRIM(FournisseurRVG.FOURN_SIRET)),5) = LTRIM(RTRIM(@SiretFournisseur));
                IF (@DebugMode = 1 ) PRINT '    - ' + CAST(@@ROWCOUNT AS VARCHAR(MAX)) + ' lignes trouvées';
                IF (@DebugMode = 1 ) PRINT '    - Code fournisseur RVG : ' + CAST(ISNULL(@CodeFournisseurRVG, 'NON') + ' TROUVÉ' AS VARCHAR(MAX));
             END

            IF (@CodeFournisseurRVG IS NULL)
             BEGIN
                SET @NomFournisseur = @NomFournisseurBuyer
                IF (@DebugMode = 1 ) PRINT '    - Code Fournisseur RVG introuvable !';
             END

            SET @FournisseurID = (SELECT TOP 1 FF.FournisseurId FROM dbo.FRED_FOURNISSEUR FF WHERE FF.Code = LTRIM(RTRIM(@CodeFournisseurBuyer)))

            /*  Récupération du type de commande  */
            IF @sTypeCommBuyer='LO'
            BEGIN
                Set @TypeCommande='L'
            END
            ELSE IF @sTypeCommBuyer='PR'
            BEGIN
                Set @TypeCommande='P'
            END
            ELSE
            BEGIN
                Set @TypeCommande='F'
            END

            /* Récupération de l'ID du type de commande */
            /* PB de conformité des codes, une seule lettre dans FRED, 2 lettres dans BUYER; on prend la première lettre du code de BUYER */
            SET @TypeID = (SELECT FCT.CommandeTypeId FROM FRED_COMMANDE_TYPE FCT WHERE FCT.Code = @TypeCommande)

            -- affichage des données récupérées en mode debug
            --IF (@DebugMode = 1 )PRINT 'Commande'+ CAST(ISNULL(@NumeroCommande,'NULL')AS VARCHAR(MAX)) + ';' + CAST(ISNULL(@Libelle,'NULL')AS VARCHAR(MAX)) + ';' + CAST(ISNULL(@DateCreation,'NULL') AS VARCHAR(MAX)) + ';'
            --IF (@DebugMode = 1 )PRINT CAST(ISNULL(@TypeCommande,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@CodeFournisseur,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@DelaiLivraison,'NULL')AS VARCHAR(MAX))+ ';'/*+CAST(ISNULL(@DateMiseADispo,'NULL')AS VARCHAR(MAX))*/
            --IF (@DebugMode = 1 )PRINT ';'+CAST(ISNULL(@Brouillon,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@MOConduite,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@EntretienMecanique,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@EntretienJournalier,'NULL')AS VARCHAR(MAX))
            --IF (@DebugMode = 1 )PRINT ';'+CAST(ISNULL(@Carburant,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@Lubrifiant,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@FraisAmortissement,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@ImpotsAssurance,'NULL')AS VARCHAR(MAX))
            --IF (@DebugMode = 1 )PRINT ';'+CAST(ISNULL(@ConditionSociete,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@ConditionPrestation,'NULL')AS VARCHAR(MAX)) + ';'+CAST(ISNULL(@FournisseurTMPLibelle,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@FournisseurTMPAdresse,'NULL')AS VARCHAR(MAX))
            --IF (@DebugMode = 1 )PRINT ';'+CAST(ISNULL(@FournisseurTMPVille,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@FournisseurTMPCodePostal,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@FournisseurTMPNumTel,'NULL')AS VARCHAR(MAX))
            --IF (@DebugMode = 1 )PRINT ';'+CAST(ISNULL(@ContactEntreID,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@ContactPersoID,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@SuiviEntreID,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@SuiviPersoID,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@ValidEntreID,'NULL')AS VARCHAR(MAX))
            --IF (@DebugMode = 1 )PRINT ';'+CAST(ISNULL(@ValidPersoID,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@DateSaisie,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@DateModification,'NULL')AS VARCHAR(MAX))+ ';'/*+CAST(ISNULL(@DateValidation,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@DateTransfert,'NULL')AS VARCHAR(MAX))*/
            --IF (@DebugMode = 1 )PRINT ';'+CAST(ISNULL(@LivraisonAdresse,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@LivraisonVille,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@LivraisonCodePostal,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@FacturationAdresse,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@FacturationVille,'NULL')AS VARCHAR(MAX))
            --IF (@DebugMode = 1 )PRINT ';'+CAST(ISNULL(@FacturationCodePostal,'NULL')AS VARCHAR(MAX))/*+';'+CAST(ISNULL(@DateSuppression,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@DateCloture,'NULL')AS VARCHAR(MAX))*/+ ';'+CAST(ISNULL(@Etabl,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@Externe,'NULL')AS VARCHAR(MAX))
            --IF (@DebugMode = 1 )PRINT ';'+CAST(ISNULL(@Justificatif,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@CommentaireFournisseur,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@CommentaireInterne,'NULL')AS VARCHAR(MAX))+ ';'+CAST(ISNULL(@CodeSociete,'NULL')AS VARCHAR(MAX))

						SET @DeviseId = (SELECT DeviseId FROM FRED_DEVISE WHERE IsoCode LIKE 'EUR');

            IF (@DebugMode = 1 )PRINT 'RVG_saveCommande pour la commande ' + CAST(ISNULL(@NumeroCommande,'NULL')AS VARCHAR(MAX));
            exec dbo.FRED_saveCommande_fromBUYER
                  @iCiId       = @CiId
                , @sNumero         = @NumeroCommande
                , @sLibelle                 = @LibelleCmd
                , @dtDate                   = @DateCreation
                , @sTypeID             = @TypeID
                , @iFournisseurId          = @FournisseurID
                , @sDelaiLivraison          = @DelaiLivraison
                , @dtDateMiseADispo         = @DateMiseADispo
                , @iStatutCommandeId             = @StatutCommandeId
                , @bMOConduite              = @MOConduite
                , @bEntretienMecanique      = @EntretienMecanique
                , @bEntretienJournalier     = @EntretienJournalier
                , @bCarburant               = @Carburant
                , @bLubrifiant              = @Lubrifiant
                , @bFraisAmortissement      = @FraisAmortissement
                , @bFraisAssurance         = @ImpotsAssurance
                , @sConditionSociete        = @ConditionSociete
                , @sConditionPrestation     = @ConditionPrestation
                , @sContactId           = @ContactPersoID
                , @sContactTel      = @ContactNumTel
                , @sSuiviId             = @SuiviPersoID
                , @sAuteurCreationId          = @SaisiePersoID
                , @sValideurId             = @ValidPersoID
                , @dtDateSaisie            = @DateSaisie
                , @dtDateModification       = @DateModification
                , @dtDateValidation         = @DateValidation
                , @sLivraisonAdresse       = @LivraisonAdresse
                , @sLivraisonVille     = @LivraisonVille
                , @sLivraisonCPostale      = @LivraisonCodePostal
                , @sFacturationAdresse    = @FacturationAdresse
                , @sFacturationVille       = @FacturationVille
                , @sFacturationCPostale       = @FacturationCodePostal
                , @dtDateSuppression         = @DateSuppression
                , @dtDateCloture           = @DateCloture
                , @sJustificatif             = @Justificatif
                , @sCommentaireFournisseur   = @CommentaireFournisseur
                , @sCommentaireInterne       = @CommentaireInterne
                , @bCommandeManuelle    = 'FALSE' -- [TSA:30/11/2017] False par défaut.
                , @iDeviseId      = @DeviseId -- [TSA:30/11/2017] Devise par défaut est l'EURO. On ne récupère que les commandes en EURO
                , @bAccordCadre      = 'FALSE' -- [TSA:30/11/2017] False par défaut.
                , @CommandeId        = @iCommandeId OUTPUT

            PRINT @iCommandeId

            -- Insertion des lignes de commandes
            -- Commandes Ligne
            -- Curseur sur les lignes de commandes
            DECLARE curLigneCommande SCROLL CURSOR FOR
                SELECT   LIGNE_NUM
                        , LIBELLE
                        , AFFAI_ID
                        , NATURE
                        , CMDE_LIGNE_QTE
                        , CMDE_LIGNE_PUHT
                        , CMDE_LIGNE_UNITE
                FROM #CommandeLigneTemp
                WHERE IdCommande=@IdCommande
            OPEN curLigneCommande
            FETCH First FROM curLigneCommande INTO @iNumeroLigne, @sLibelleLigne, @sCodeAffaire, @sCodeNature, @dQtt, @dPrixUnitaireHT, @sUnite
            WHILE (@@Fetch_status <> -1)
            BEGIN

                IF (@DebugMode = 1 ) PRINT ' /********************* Traitement de la ligne de commande ' + CAST(@NumeroCommande AS VARCHAR(MAX)) +'-'+  CAST(@iNumeroLigne AS VARCHAR(MAX)) + '*********************\ ';
                -- on récupère le tydep de + haut niveau pour cette nature
                --IF EXISTS (select 1 from TYDEP where NATUR_ID=@sCodeNature)
                --BEGIN
                -- SELECT top 1 @sCodeTydep = t2.tydep_id
                -- FROM TYDEP t
                -- inner join dbo.TYDEP t2 on t2.tydep_id = t.TYDEP_ID_FK
                -- where t2.TYDEP_ID_FK is null
                -- and t.NATUR_ID=@sCodeNature

                --END
                --ELSE
                --BEGIN
                -- Set @sCodeTydep=null
                --END

                /* DEBUT INSTRUCTIONS SPECIFIQUES A FRED */
                DECLARE @id INT = (SELECT TOP 1 FT.TacheId FROM FRED_TACHE FT WHERE FT.Code = @sCodeTache)
                SET @TacheId = CASE WHEN @id IS NOT NULL AND @id <> '' THEN @id ELSE NULL END

                SET @id = (SELECT TOP 1 FTD.TypeDepenseId FROM FRED_TYPE_DEPENSE FTD WHERE FTD.Code = @sCodeTydep)
                SET @TypeDepenseId = CASE WHEN @id IS NOT NULL AND @id <> '' THEN @id ELSE NULL END

                /* FIN INSTRUCTIONS SPECIFIQUES A FRED */

                -- affichage des données récupérées en mode debug
                IF (@DebugMode = 1 )PRINT 'Ligne '+ CAST(ISNULL(@NumeroCommande,'NULL')AS VARCHAR(MAX))+';'+CAST(ISNULL(@iNumeroLigne,'NULL')AS VARCHAR(MAX))+';'+CAST(ISNULL(@sLibelleLigne,'NULL')AS VARCHAR(MAX))+';'
                IF (@DebugMode = 1 )PRINT CAST(ISNULL(@sCodeAffaire,'NULL')AS VARCHAR(MAX))+';'+CAST(ISNULL(@sCodeTache,'NULL')AS VARCHAR(MAX))+';'+CAST(ISNULL(@sCodeTydep,'NULL')AS VARCHAR(MAX))+';'+CAST(ISNULL(@sCodeDetail,'NULL')AS VARCHAR(MAX))+';'
                IF (@DebugMode = 1 )PRINT CAST(@dQtt AS VARCHAR(MAX))+';'+CAST(@dPrixUnitaireHT AS VARCHAR(MAX))+';'+CAST(@sUnite AS VARCHAR(MAX))+';'+CAST(@dQuantiteRecep AS VARCHAR(MAX))


                IF (@DebugMode = 1 )PRINT 'Exécution de RVG_saveCommandeLigne';
                exec dbo.FRED_saveCommandeLigne_fromBUYER
                  @iCommandeId     = @iCommandeId
                , @iTacheId              = NULL --= @TacheId
                , @sLibelle               = @sLibelleLigne
                , @iTypeDepenseId   = NULL --= @TypeDepenseId
                , @dQuantite                = @dQtt
                , @dPrixUnitaireHT          = @dPrixUnitaireHT
                , @sUnite                  = @sUnite

                FETCH NEXT FROM curLigneCommande INTO @iNumeroLigne, @sLibelleLigne, @sCodeAffaire, @sCodeNature, @dQtt, @dPrixUnitaireHT, @sUnite
            END
            CLOSE curLigneCommande
            DEALLOCATE curLigneCommande
        END

        -- S'il y a eu une erreur fonctionnel, on annule l'enregistrement de la commande
        -- IF (@cmdErreur <> 0)
        -- BEGIN
            -- ROLLBACK TRANSACTION CMD;
        -- END
        -- ELSE
        -- BEGIN
            -- COMMIT TRANSACTION CMD;
        -- END

        FETCH NEXT FROM curCommande INTO @IdCommande, @NumeroCommande, @LibelleCmd, @DateCreation, @CodeFournisseurBuyer, @SiretFournisseur, @NomFournisseurBuyer,
                                    @DelaiLivraison,@DateMiseADispo, --@FournisseurTMPLibelle,@FournisseurTMPAdresse,@FournisseurTMPVille,@FournisseurTMPCodePostal,@FournisseurTMPNumTel,
                                    @DateSaisie, @DateModification, @LivraisonAdresse, @LivraisonVille, @LivraisonCodePostal, @FacturationAdresse,
                                    @FacturationVille, @FacturationCodePostal, @CommentaireFournisseur, @sTypeCommBuyer,@DateValidation,@SaisieNom, @SaisiePrenom, @SaisieEmail
    END
    CLOSE curCommande
    DEALLOCATE curCommande

IF (@DebugMode = 1 )
  PRINT 'Suppression de la table temporaire de commande';

DROP TABLE #CommandeTemp;

IF (@DebugMode = 1 )
  PRINT 'Suppression de la table temporaire de lignes de commande';

DROP TABLE #CommandeLigneTemp;

IF (@DebugMode = 1 )
  PRINT 'FIN DU TRAITEMENT';

END
GO
