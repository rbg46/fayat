using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Budget.Liste
{
    public class ListPeriodeRecalageModel
    {
        /// <summary>
        /// Indique l'erreur de chargement ou null si pas d'erreur.
        /// </summary>
        public string Erreur { get; set; }

        /// <summary>
        /// Id du budget
        /// </summary>
        public int BudgetId { get; set; }

        /// <summary>
        /// Liste des périodes en string 
        /// </summary>
        public List<string> Periodes { get; set; } = new List<string>();

    }
}
