namespace Fred.Business.RepriseDonnees.ValidationCommande.Validators.Results
{
    /// <summary>
    /// type de regle à verifiée
    /// </summary>
    public enum ValidationCommandeRuleType
    {
        /// <summary>
        /// Pour chacun des numéros de commande présent dans le fichier d’entrée :
        /// -   Faire une recherche d’abord sur le champ [NumeroCommandeExterne] de la table [FRED_COMMANDE] pour retrouver la commande à valider.
        /// -   Si non trouvé, faire une recherche sur le champ [Numero] de la table [FRED_COMMANDE].
        /// -   Si toujours aucune correspondance trouvée => rejet de la ligne.
        /// </summary>
        ReconnaissanceDesNumerosDeCommande = 0,
        /// <summary>
        /// Pour chaque commande du fichier d’entrée : 
        /// vérifier que la commande n’a pas déjà été envoyée à SAP,
        /// c’est-à-dire que le champ [HangfireJobId] de la table [FRED_COMMANDE] est bien à NULL.
        /// Si le champ [HangfireJobId] de la table [FRED_COMMANDE] est non NULL => rejet de la ligne.
        /// </summary>
        VerificationDesCommandesDejaEnvoyeesASap = 1,
    }
}
