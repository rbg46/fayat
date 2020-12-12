using System.ComponentModel;

namespace Fred.Entities
{
    /// <summary>
    /// Identifiant des paramètres de FRED.
    /// </summary>
    public enum ParametreId
    {
        /// <summary>
        /// API Google Map
        /// </summary>
        GoogleApiParams = 1,

        /// <summary>
        /// Url scan facture.
        /// </summary>
        URLScanFacture = 2,

        /// <summary>
        /// Prix par défaut pour les barèmes exploitation.
        /// </summary>
        BaremeExploitationPrixParDefaut = 3,

        /// <summary>
        /// Prix chauffezur par défaut pour les barèmes exploitation.
        /// </summary>
        BaremeExploitationPrixChauffeurParDefaut = 4,
    }

    /// <summary>
    /// Statut d'un budget
    /// </summary>
    public enum StatutBudget
    {
        /// <summary>
        /// Un budget en cours de construction
        /// </summary>
        Brouillon = 0,

        /// <summary>
        /// Un budget à valider par un manager
        /// </summary>
        AValider = 1,

        /// <summary>
        /// Un budget validé par un manager
        /// </summary>
        Valider = 2
    }

    /// <summary>
    /// Statut d'un avancement
    /// </summary>
    public enum StatutAvancementBudget
    {
        /// <summary>
        /// Un avancement en cours de construction
        /// </summary>
        Brouillon = 0,

        /// <summary>
        /// Un avancement à valider par un manager
        /// </summary>
        AValider = 1,

        /// <summary>
        /// Un avancement validé par un manager
        /// </summary>
        Valider = 2
    }

    /// <summary>
    /// Avancement d'un budget
    /// </summary>
    public enum TypeAvancementBudget
    {
        /// <summary>
        /// Type non défini
        /// </summary>
        Aucun = 0,

        /// <summary>
        /// Type quantité
        /// </summary>
        Quantite = 1,

        /// <summary>
        /// Type Pourcentage
        /// </summary>
        Pourcentage = 2
    }

    /// <summary>
    /// Avancement d'un budget
    /// </summary>
    public enum BudgetSousDetailViewMode
    {
        /// <summary>
        /// Vue T4.
        /// </summary>
        T4 = 0,

        /// <summary>
        /// Vue SD.
        /// </summary>
        SD = 1,

        /// <summary>
        /// Vue PU.
        /// </summary>
        PU = 2
    }

    /// <summary>
    ///   Statut d'un flux vers Fred.IE
    /// </summary>
    public enum FluxStatus
    {
        /// <summary>
        /// Flux non exécuté
        /// </summary>
        None = 0,

        /// <summary>
        /// Flux en cours d'exécution
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// Flux exécuté avec succés
        /// </summary>
        Done = 2,

        /// <summary>
        /// Flux refusé (droits insuffisants par exemple, ou autre ?)
        /// </summary>
        Refused = 3,

        /// <summary>
        /// Erreur lors du traitement du flux
        /// </summary>
        Failed = 4
    }

    /// <summary>
    ///   Énumération des type de base de données.
    /// </summary>
    public enum TypeBdd
    {
        /// <summary>
        ///   Type de base de données SQL Server
        /// </summary>
        SqlServer,

        /// <summary>
        ///   Type de base de données Db2
        /// </summary>
        Db2
    }

    /// <summary>
    ///   Énumération des type de Contrôles de pointage
    /// </summary>
    public enum TypeControlePointage
    {
        /// <summary>
        /// Pointage controlé par le chantier
        /// </summary>
        ControleChantier = 1,

        /// <summary>
        /// Pointage controlé par as400
        /// </summary>
        ControleVrac = 2
    }

    /// <summary>
    ///   Énumération des type d'image
    /// </summary>
    public enum TypeImage
    {
        /// <summary>
        /// ?
        /// </summary>
        Login = 1,
        /// <summary>
        /// ??
        /// </summary>
        Logo = 2,
        /// <summary>
        /// Type Conditions Générales de Ventes (docx)
        /// </summary>
        CGA = 3
    }

    /// <summary>
    /// Enumère les différents status de la connexion d'un utilisateur
    /// </summary>
    public enum ConnexionStatus
    {
        /// <summary>
        /// Le compte a expiré
        /// </summary>
        AccountExpired = 0,
        /// <summary>
        /// Le compte est inactif
        /// </summary>
        AccountInactive = 1,
        /// <summary>
        /// Le compte est supprimé
        /// </summary>
        AccountDeleted = 2,
        /// <summary>
        /// Le login est mal formaté
        /// </summary>
        BadlyFormated = 3,
        /// <summary>
        /// Le Login ou mot de passe est incorrect
        /// </summary>
        LoginAndPasswordNotFound = 4,
        /// <summary>
        /// tout est ok
        /// </summary>
        Ok = 5,
        /// <summary>
        /// le mot de passe est vide
        /// </summary>
        EmptyPassword = 6,
        /// <summary>
        /// le login est vide
        /// </summary>
        EmptyLogin = 7,
        /// <summary>
        /// Une erreur technique est survenue
        /// </summary>
        TechnicalError = 8,
        /// <summary>
        /// Le login et l'email sont vide
        /// </summary>
        EmptyEmailAndLogin = 9,
        /// <summary>
        /// Le compte est un compte interne
        /// </summary>
        AccountIsInterne = 10,
        /// <summary>
        /// Le compte est un compte super admin
        /// </summary>
        AccountIsSuperAdmin = 11,
        /// <summary>
        /// Le compte n'existe pas
        /// </summary>
        UserNotFound = 12,
        /// <summary>
        /// Le login est incorrect
        /// </summary>
        LoginNotFound = 13,
        /// <summary>
        /// L'email est incorrect
        /// </summary>
        EmailNotFound = 14,
        /// <summary>
        /// l'envoie du mail pour réinitialiser le mot de passer est un succès
        /// </summary>
        ResetPasswordSuccess = 15,
        /// <summary>
        /// La vérification du mot de passe est vide
        /// </summary>
        EmptyPasswordVerify = 16,
        /// <summary>
        /// La longueur du mot de passe n'est pas respecté
        /// </summary>
        PasswordRequiredLength = 17,
        /// <summary>
        /// Le mot de passe et la vérification de mot de passe ne correspondnt pas
        /// </summary>
        NotEqualsPasswords = 18,
        /// <summary>
        /// La modification du mot de passe est un succès
        /// </summary>
        UpdatePasswordSuccess = 19
    }


    /// <summary>
    /// Enumération indiquant la source d'une authentification
    /// </summary>
    public enum ConnexionErrorOrigin
    {
        /// <summary>
        /// L'erreur vient de l'autentification oauth
        /// </summary>
        Api = 0,

        /// <summary>
        ///  L'erreur vient du formulaire
        /// </summary>
        Forms = 1,

    }

    /// <summary>
    ///   Enumération indiquant le type d'export
    /// </summary>
    public enum TypeExport
    {
        /// <summary>
        ///   Export de type excel
        /// </summary>
        Excel = 0,

        /// <summary>
        ///   Export de type PDF
        /// </summary>
        Pdf = 1
    }

    /// <summary>
    /// Type d'organisation.
    /// Attention le type d'organisation est defini dans la base.
    /// Il faut donc remplacer toutes les valeur numerique par l'enumeration afin de pouvoir modifier
    /// les valeurs, si un jour on rajoute un nouveau type d'organisation.
    /// </summary>
    public enum OrganisationType
    {
        /// <summary>
        /// Holding
        /// </summary>
        Holding = 1,
        /// <summary>
        /// Pole
        /// </summary>
        Pole = 2,
        /// <summary>
        /// Groupe
        /// </summary>
        Groupe = 3,
        /// <summary>
        /// Societe
        /// </summary>
        Societe = 4,
        /// <summary>
        /// Périmètre UO
        /// </summary>
        Puo = 5,
        /// <summary>
        /// Unité Opérationnelle
        /// </summary>
        Uo = 6,
        /// <summary>
        /// Etablissement
        /// </summary>
        Etablissement = 7,
        /// <summary>
        /// Centre d'imputation
        /// </summary>
        Ci = 8,
        /// <summary>
        /// Sous Centre d'imputation
        /// </summary>
        SousCi = 9
    }

    /// <summary>
    /// type de mode affecté a une fonctionnalite
    /// </summary>
    public enum FonctionnaliteTypeMode
    {
        /// <summary>
        /// Fonctionnalite non affecté
        /// </summary>
        UnAffected = 0,
        /// <summary>
        /// Lecture seule
        /// </summary>
        Read = 1,
        /// <summary>
        /// Lecture ecriture
        /// </summary>
        Write = 2
    }

    /// <summary>
    /// Type d'unité
    /// </summary>
    public enum TypeUnite
    {
        /// <summary>
        /// Unité Achat
        /// </summary>
        HA = 1,

        /// <summary>
        /// Unité de Valorisation
        /// </summary>
        Valo = 2
    }

    /// <summary>
    /// Type de surcharge ou exception pour les Baremes CI
    /// </summary>
    public enum TypeSurchargeBareme
    {
        /// <summary>
        /// Unité Achat
        /// </summary>
        Surcharge = 1,

        /// <summary>
        /// Unité de Valorisation
        /// </summary>
        Exception = 2
    }

    /// <summary>
    /// Statut pour les Baremes CI
    /// </summary>
    public enum StatutBareme
    {
        /// <summary>
        /// Unité Achat
        /// </summary>
        EnCours = 0,

        /// <summary>
        /// Unité de Valorisation
        /// </summary>
        Anterieur = 1,

        /// <summary>
        /// Unité de Valorisation
        /// </summary>
        Verrouille = 2,

        /// <summary>
        /// Unité de Valorisation
        /// </summary>
        Previsionnel = 1
    }

    /// <summary>
    ///   Type de dépense
    /// </summary>
    public enum DepenseType
    {
        /// <summary>
        /// Réception
        /// </summary>
        Reception = 1,

        /// <summary>
        /// Facture
        /// </summary>
        Facture = 2,

        /// <summary>
        /// Facture écart
        /// </summary>
        FactureEcart = 3,

        /// <summary>
        /// Facture non commandé
        /// </summary>
        FactureNonCommande = 4,

        /// <summary>
        /// Facture Avoir
        /// </summary>
        Avoir = 5,

        /// <summary>
        /// Avoir Ecart
        /// </summary>
        AvoirEcart = 6,

        /// <summary>
        /// Ajustement FAR SAP
        /// </summary>
        AjustementFar = 7,

        /// <summary>
        /// Extourne FAR
        /// </summary>
        ExtourneFar = 8
    }

    /// <summary>
    ///   Type de facturation
    /// </summary>
    public enum FacturationType
    {
        /// <summary>
        /// Facturation
        /// </summary>
        Facturation = 1,

        /// <summary>
        /// Coût additionnel
        /// </summary>
        CoutAdditionnel = 2,

        /// <summary>
        /// Article non commandé
        /// </summary>
        ArticleNonCommande = 3,

        /// <summary>
        /// Facturation en montant
        /// </summary>
        FacturationMontant = 4,

        /// <summary>
        /// Chargement provision far
        /// </summary>
        ChargementProvisionFar = 5,

        /// <summary>
        /// Déchargement provision far
        /// </summary>
        DechargementProvisionFar = 6,

        /// <summary>
        /// Avoir sur quantité
        /// </summary>
        AvoirQuantite = 7,

        /// <summary>
        /// Avoir sur montant
        /// </summary>
        AvoirMontant = 8,

        /// <summary>
        /// Annulation FAR
        /// </summary>
        AnnulationFar = 9,

        /// <summary>
        /// Avoir sans commande
        /// </summary>
        AvoirSansCommande = 10,

        /// <summary>
        /// Facture sans commande
        /// </summary>
        FactureSansCommande = 11
    }

    /// <summary>
    /// Type de tâche.
    /// </summary>
    public enum TacheType
    {
        /// <summary>
        /// Tâche utilisateur.
        /// </summary>
        Utilisateur = 999999,

        /// <summary>
        /// Tâche par défaut.
        /// </summary>
        Defaut = 0,

        /// <summary>
        /// Tâche d'écart niveau 1.
        /// </summary>
        EcartNiveau1 = 1,

        /// <summary>
        /// Tâche d'écart niveau 2.
        /// </summary>
        EcartNiveau2 = 2,

        /// <summary>
        /// Tâche d'écart MO encadrement.
        /// </summary>
        EcartMOEncadrement = 3,

        /// <summary>
        /// Tâche d'écart MO production.
        /// </summary>
        EcartMOProduction = 4,

        /// <summary>
        /// Tâche d'écart matériel.
        /// </summary>
        EcartMateriel = 5,

        /// <summary>
        /// Tâche d'écart achat.
        /// </summary>
        EcartAchat = 6,

        /// <summary>
        /// Tâche d'écart autre Frais
        /// </summary>
        EcartAutreFrais = 7,

        /// <summary>
        /// Tâche d'écart interim
        /// </summary>
        EcartInterim = 8,

        /// <summary>
        /// Tâche d'écart matériel Immobilisé.
        /// </summary>
        EcartMaterielImmobilise = 9,

        /// <summary>
        /// Tâche d'écart matériel externe.
        /// </summary>
        EcartMaterielExterne = 10,

        /// <summary>
        /// Tâche d'écart recette.
        /// </summary>
        EcartRecette = 11,

        /// <summary>
        /// Tâche d'écart frais generaux.
        /// </summary>
        EcartFraisGeneraux = 12,

        /// <summary>
        /// Tâche d'écart autres dépenses hors debours.
        /// </summary>
        EcartAutresDepensesHorsDebours = 13,

        /// <summary>
        ///   Tâche litige - article non commandé
        /// </summary>
        Litige = 20
    }

    /// <summary>
    /// Détermine le code de la tâche
    /// </summary>
    public enum TacheCode
    {
        /// <summary>
        /// Tâche écart
        /// </summary>
        TacheEcart = 99
    }

    /// <summary>
    /// Détermine la fréquence d'un abonnement
    /// </summary>
    public enum FrequenceAbonnement
    {
        /// <summary>
        ///   Journalier
        /// </summary>
        Jour = 0,

        /// <summary>
        ///   Hebdomadaire
        /// </summary>
        Semaine = 1,

        /// <summary>
        ///   Mensuel
        /// </summary>
        Mois = 2,

        /// <summary>
        ///   Trimestriel
        /// </summary>
        Trimestre = 3,

        /// <summary>
        ///   Annuel
        /// </summary>
        Annee = 4
    }

    /// <summary>
    /// Détermine une spécification à un role donnée
    /// </summary>
    public enum RoleSpecification
    {
        /// <summary>
        /// Responsable CI
        /// </summary>
        ResponsableCI = 1,

        /// <summary>
        /// Délégué
        /// </summary>
        Delegue = 2,

        /// <summary>
        /// Gestionnaire des moyens
        /// </summary>
        GestionnaireMoyen = 3
    }

    /// <summary>
    /// Détermine une le statut du visa d'une réception
    /// </summary>
    public enum StatutVisa
    {
        /// <summary>
        /// Succès
        /// </summary>
        Succes = 1,

        /// <summary>
        /// Echec
        /// </summary>
        Echec = 2
    }

    /// <summary>
    /// Détermine une le statut du visa d'une réception
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        ///     Opération 1 - FACTURE
        /// </summary>
        Facture = 1,

        /// <summary>
        ///     Opération 2 - AVOIR
        /// </summary>
        Avoir = 2,

        /// <summary>
        ///     Opération 3 - Chargement ultérieur
        /// </summary>
        Chargement = 3,

        /// <summary>
        ///     Opération 4 - Déchargement ultérieur
        /// </summary>
        Dechargement = 4
    }

    /// <summary>
    /// Type d'un système externe.
    /// </summary>
    public enum SystemeExterneType
    {
        /// <summary>
        /// Type facturation.
        /// </summary>
        Facturation = 1,

        /// <summary>
        /// Type commandes.
        /// </summary>
        Commandes = 2
    }

    /// <summary>
    /// Code des types d'affection d'un moyen
    /// </summary>
    public enum AffectationMoyenTypeCode
    {
        /// <summary>
        /// Aucune affectation
        /// </summary>
        NoAffectation = 1,

        /// <summary>
        /// Affecté à une personnel
        /// </summary>
        Personnel = 2,

        /// <summary>
        /// Affecté à une CI
        /// </summary>
        CI = 3,

        /// <summary>
        /// Affecté à un parking
        /// </summary>
        Parking = 4,

        /// <summary>
        /// Affecté à un dépot
        /// </summary>
        Depot = 5,

        /// <summary>
        /// Affecté à un stock
        /// </summary>
        Stock = 6,

        /// <summary>
        /// En réparation
        /// </summary>
        Reparation = 7,

        /// <summary>
        /// En entretien
        /// </summary>
        Entretien = 8,

        /// <summary>
        /// en contrôle
        /// </summary>
        Controle = 9,

        /// <summary>
        /// Location retourné au loeur
        /// </summary>
        RetourLoueur = 10,

        /// <summary>
        /// Location disponible
        /// </summary>
        ResteDisponible = 11
    }

    /// <summary>
    /// Les niveaux des codes absences
    /// </summary>
    public enum CodeAbsenceNiveau
    {
        /// <summary>
        /// Niveau 1 et 2
        /// </summary>
        N1N2 = 0,

        /// <summary>
        /// Niveau 1
        /// </summary>
        N1 = 1,
        /// <summary>
        /// Niveau 2
        /// </summary>
        N2 = 2
    }

    /// <summary>
    /// Target personnel statut
    /// </summary>
    public enum TargetPersonnel
    {
        /// <summary>
        /// Tous les personnels
        /// </summary>
        All = 0,

        /// <summary>
        /// Ouvrier
        /// </summary>
        Ouvrier = 1,

        /// <summary>
        /// Etam et Iac
        /// </summary>
        EtamIac = 2
    }

    /// <summary>
    /// Type de calcul pour les indemnités de déplacement.
    /// </summary>
    public enum IndemniteDeplacementCalculType
    {
        /// <summary>
        /// Calcul en kilomètre vol d'oiseau.
        /// </summary>
        KilometreVolOiseau = 1,

        /// <summary>
        /// Calcul en kilomètre routier.
        /// </summary>
        KilometreRoutier = 2,

        /// <summary>
        /// Calcul en kilomètre réel.
        /// </summary>
        KilometreReel = 3
    }

    /// <summary>
    /// Type pour définir le status de travail d'une journée donnée .
    /// </summary>
    public enum DayWorkingStatus
    {
        /// <summary>
        /// Journée travaillée
        /// </summary>
        Worked = 0,

        /// <summary>
        /// Journée d'absence
        /// </summary>
        Absence = 1,

        /// <summary>
        /// Journée non pointée
        /// </summary>
        NotRegistred = 2
    }

    /// <summary>
    /// Définie les différents niveaux de ressource
    /// </summary>
    public enum RessourceType
    {
        /// <summary>
        /// Chapitre, le plus haut niveau
        /// </summary>
        Chapitre,

        /// <summary>
        /// Sous Chapitre le niveau intermediaire
        /// </summary>
        SousChaptre,

        /// <summary>
        /// Ressource le niveau le plus bas
        /// </summary>
        Ressource
    }

    /// <summary>
    /// Les différents Type de Societes avec leurs  ID
    /// </summary>
    public enum EnumSocieteType
    {
        /// <summary>
        /// Societe Interne
        /// </summary>
        IsINT = 1,

        /// <summary>
        /// Societe Partenaire
        /// </summary>
        IsPAR = 2,

        /// <summary>
        /// Societe SEP
        /// </summary>
        ISSEP = 3
    }

    /// <summary>
    /// Le type de l'attachement d'une pièce jointe (ex : Commande / Dépense / ...)
    /// </summary>
    public enum PieceJointeTypeEntite
    {
        /// <summary>
        /// Attachement d'une pièce jointe à une commande
        /// </summary>
        Commande = 1,

        /// <summary>
        /// Attachement d'une pièce jointe à une réception
        /// </summary>
        Reception = 2
    }

    /// <summary>
    /// Enum représentant les types d'avis
    /// </summary>
    public enum TypeAvis
    {
        /// <summary>
        /// Accord de l'utilisateur
        /// </summary>
        [Description("Accord")]
        Accord = 1,

        /// <summary>
        /// Refus de l'utilisateur
        /// </summary>
        [Description("Refus")]
        Refus = 2,

        /// <summary>
        /// L'utilisateur ne donne pas son avis
        /// </summary>
        [Description("Sans avis")]
        SansAvis = 3
    }

    /// <summary>
    /// Le type d'action
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// Action de type verrouillage
        /// </summary>
        [Description("VERROUILLAGE")]
        Verrouillage = 1,

        /// <summary>
        /// Action de type deverouillage
        /// </summary>
        [Description("DEVERROUILLAGE")]
        Deverrouillage = 2
    }

    /// <summary>
    /// Le statut d'une action
    /// </summary>
    public enum ActionStatus
    {
        /// <summary>
        /// Action avec status init
        /// </summary>
        [Description("INITIATED")]
        Initiated = 1,

        /// <summary>
        /// Action avec status en cours
        /// </summary>
        [Description("PENDING")]
        Pending = 2,

        /// <summary>
        /// Action avec status success
        /// </summary>
        [Description("SUCCESS")]
        Success = 3,

        /// <summary>
        /// Action avec status warning
        /// </summary>
        [Description("WARNING")]
        Warning = 4,

        /// <summary>
        /// Action avec status suspendu
        /// </summary>
        [Description("SUSPENDED")]
        Suspended = 5,

        /// <summary>
        /// Action avec status a failed
        /// </summary>
        [Description("FAILED")]
        Failed = 6,
    }

    /// <summary>
    /// Enum représentant les statuts d'interface transfert données
    /// </summary>
    public enum StatutTransfertDonnee
    {
        /// <summary>
        /// A traiter
        /// </summary>
        ToTreat = 0,

        /// <summary>
        /// Traité
        /// </summary>
        Treated = 1,

        /// <summary>
        /// Echec du traitement
        /// </summary>
        TreatmentFailure = 2,

        /// <summary>
        /// Abandonné
        /// </summary>
        Abandoned = 3
    }
}
