namespace Fred.Entities.ActivitySummary
{
    /// <summary>
    /// Enum qui correspond aux requettes effectuer dans la base pour calculer les travaux en cours
    /// </summary>
    public enum TypeActivity
    {
        /// <summary>
        /// Commande à valider
        /// </summary>
        CommandeAvalider = 0,
        /// <summary>
        /// Rapports validé 1
        /// </summary>
        RapportsAvalide1 = 1,
        /// <summary>
        /// Receptions à viser
        /// </summary>
        ReceptionsAviser = 2,
        /// <summary>
        /// Budget à valider
        /// </summary>
        BudgetAvalider = 3,
        /// <summary>
        /// Avancement à valider
        /// </summary>
        AvancementAvalider = 4,
        /// <summary>
        /// Controle Budgetaire à valider
        /// </summary>
        ControleBudgetAvalider = 5
    }
}
