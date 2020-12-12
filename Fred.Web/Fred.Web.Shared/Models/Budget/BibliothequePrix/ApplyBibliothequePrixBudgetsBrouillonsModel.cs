namespace Fred.Web.Shared.Models.Budget.BibliothequePrix
{
    /// <summary>
    /// Model attendu par le point d'entrée de l'API permettant de mettre à jour des budgets à partir de la bibliotheque des prix
    /// </summary>
    public class ApplyBibliothequePrixBudgetsBrouillonsModel
    {
        /// <summary>
        /// Id de l'organisation liée au CI dont on veut récupérer la bibliotheque des prix
        /// </summary>
        public int CiOrganisationId { get; set; }

        /// <summary>
        /// La devise concernée.
        /// </summary>
        public int DeviseId { get; set; }

        /// <summary>
        /// Représente toutes les versions du budget à impacter
        /// </summary>
        public int[] BudgetIdAEnregistrer { get; set; }

        /// <summary>
        /// True si sur les budgets imapctés, les valeurs en exception (c'est à dire les lignes de sous détail dont les valeurs diffèrent de la version précédente 
        /// de la bibliotheque des prix) doivent être modifiées.
        /// </summary>
        public bool UpdateValeursSurLignesEnException { get; set; }
    }
}
