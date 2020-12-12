namespace Fred.Web.Shared.Enum
{
    /// <summary>
    /// Cette classe représente le retour de TIBCO
    /// </summary>
    public enum ExportPointageErrorCode
    {
        /// <summary>
        /// Succés de l'export
        /// </summary>
        Success,

        /// <summary>
        /// Erreur lors de l'export
        /// </summary>
        Error
    }

    /// <summary>
    /// informations liée à la Recette
    /// </summary>
    public enum InfoAvancementRecette
    {
        facture,
        correctif,
        production,
        fraisPorp
    }

    /// <summary>
    /// Type d'un évènement sur une commande
    /// </summary>
    public enum TypeCommandeEvent
    {
        Creation = 1,
        ValidationCommande = 2,
        ValidationAvenant = 3,
        Avis = 4,
        Impression = 5
    }

    /// <summary>
    /// Détermine la fréquence d'un abonnement
    /// </summary>
    public enum FrequenceAbonnement
    {
        /// <summary>
        /// Aucun fréquence
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Journalier
        /// </summary>
        Jour = 1,

        /// <summary>
        /// Hebdomadaire
        /// </summary>
        Semaine = 2,

        /// <summary>
        /// Mensuel
        /// </summary>
        Mois = 3,

        /// <summary>
        /// Trimestriel
        /// </summary>
        Trimestre = 4,

        /// <summary>
        /// Annuel
        /// </summary>
        Annee = 5
    }

}
