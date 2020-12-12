namespace Fred.Framework.FeatureFlipping
{
    /// <summary>
    /// Liste des features flippings 
    /// Correspond aux éléments dans la table FRED_FEATURE_FLIPPING
    /// Pour 
    /// - Créer une feature :
    ///    - Ajouter la feature dans un script sql dans le référentiel
    ///    - Ajouter un enum avec le même code (colonne Code)
    ///    - Faire un TUA sur la feature activée et désactivée pour vérifier que le code s'exécute comme attendu dans les deux cas.
    /// - Supprimer une feature
    ///    - Dans un 1er temps elle peut être désactivé
    ///    - Supprimer en base la ligne correspondante via un script sql dans le référentiel
    ///    - Supprimer l'enum
    ///    
    /// Merci de lire la doc !
    /// </summary>
    public enum EnumFeatureFlipping
    {
        /// <summary>
        /// Feature d'exemple, merci de ne pas la supprimer, vous pouvez regarder comment elle est utilisée dans le code pour vous en inspirer.
        /// </summary>
        SampleFeature = 0,

        /// <summary>
        /// Feature de l'activation de la clôture des OD
        /// </summary>
        ActivationClotureOperationDiverses = 3,

        /// <summary>
        /// Feature blocage
        /// </summary>
        BlocageFournisseursSansSIRET = 4,

        /// <summary>
        /// Activation des colonnes triables sur l'explorateur de dépense
        /// </summary>
        TriTableauExplorateurDepenses = 5,

        /// <summary>
        /// Activation des formules sur le sous detail budget
        /// </summary>
        BudgetIntegrationFormules = 6,

        /// <summary>
        /// Activation des améliorations des éditions de la "vérification des temps" dans les rapports journaliers.
        /// </summary>
        EditionsPaieAmeliorations = 7,

        /// <summary>
        /// Correction fournisseur de la commande par celui reçu via flux MIRO
        /// </summary>
        CorrectionFournisseurSAP = 8,

        /// <summary>
        /// Feature des nouveaux filtres de recherche du personnel
        /// </summary>
        PersonnelsNouveauxFiltres = 9,

        /// <summary>
        /// Active la vérification des coordonnées GPS dans le cadre du calcul des indemnités de déplacement.
        /// </summary>
        VerificationCoordonneesGPS = 10,

        /// <summary>
        /// Active la copie / déplacement de T4 dans le détail budget.
        /// </summary>
        BudgetDetailCopierDeplacerT4 = 11,

        /// <summary>
        /// Active les filtres complémentaire MO/Matériel Interne ou Interim/Location
        /// </summary>
        ExplorateurDepensesFiltresMOMateriels = 13,

        /// <summary>
        /// Active la selection multiple des periodes sur les OD
        /// </summary>
        MultiplePeriodeOperationDiverses = 14,

        /// <summary>
        /// Active le paramétrage du droit à dépense sur les Avancements
        /// </summary>
        ParametrageAvancementDroitADepense = 15,

        /// <summary>
        /// Active l'obligation de saisir les plages horaires sur les CI
        /// </summary>
        RapportsHorairesObligatoires = 16,

        /// <summary>
        /// Active l'affichage des onglets matériels externe dans les picklists des pointages 
        /// </summary>
        AfficherOngletMaterielsExternesRapports = 17,

        /// <summary>
        /// La prime GDI / GDP dépend du code postal du chantier.
        /// </summary>
        VerificationCodePostalCiPrimeGDIGDP = 18,

        /// <summary>
        /// Activer ou désactiver le filtrage dans l'écran pointage synthése mensuelle
        /// </summary>
        ActivateDesactivateFiltrePointageSyntheseMensuelle = 19,

        /// <summary>
        /// Active la modification du ci lors de l'initialisation d'un rapport journalier cf: US_7943
        /// </summary>
        ModificationCiRapportJournalier = 20,

        /// <summary>
        /// Active le site comme etant en maintenance
        /// Cela reduit le nombre d'utilisateur qui peuvent acceder au site par exemple.
        /// </summary>
        WebSiteInMaintenance = 22,

        /// <summary>
        /// Active les Us 13085 et 6000
        /// </summary>
        ActivationUS13085_6000 = 23
    }
}
