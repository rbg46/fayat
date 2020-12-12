namespace Fred.Web.Shared.Models.Budget.Avancement.Excel
{
    /// <summary>
    /// Décrit le modèle attendu par l'API pour réaliser un export excel de l'Avancement
    /// </summary>
    public class AvancementExcelModelValeurs
    {
        /// <summary>
        /// Le code de la tache préfixé par son Type : T1, T2, T3, T4
        /// Le séparation entre les deux données n'est pas spécifiée
        /// </summary>
        public string CodeTachePrefixe
        {
            get
            {
                return NiveauTache + '-' + CodeTache;
            }
        }

        /// <summary>
        /// Le niveau de la tache : T1, T2, T3, T4
        /// </summary>
        public string NiveauTache { get; set; }

        /// <summary>
        /// Le code de la tache
        /// </summary>
        public string CodeTache { get; set; }

        /// <summary>
        /// Le libellé de la tache telle que spécifiée dans le plan de tache ou dans le détail du budget
        /// </summary>
        public string LibelleTache { get; set; }

        /// <summary>
        /// Le commentaire donné pour l'avancement à ce niveau de tache
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        /// Le code de l'unité associée à ce T4, si la donnée représentée est actuellment un T4, null sinon
        /// </summary>
        public string UniteCode { get; set; }

        /// <summary>
        /// La quantité budgétée pour ce T4 si la donnée représentée est actuellment un T4, null sinon
        /// </summary>
        public decimal? QuantiteBudgetee { get; set; }

        /// <summary>
        /// Le PU budgété pour ce T4 si la donnée représentée est actuellment un T4, null sinon
        /// </summary>
        public decimal? PuBudgete { get; set; }

        /// <summary>
        /// Le montant total budgété pour le T1, T2, T3 ou T4 représenté 
        /// </summary>
        public decimal MontantBudgete { get; set; }

        /// <summary>
        /// Unité de l'avancement qui est potentiellement différente de l'unité du T4
        /// selon que l'avancement est en pourcentage ou en quantité
        /// </summary>
        public string UniteAvancement { get; set; }

        /// <summary>
        /// La valeur de l'avancement qu'elle soit en quantité ou en pourcentage, suivi d'un espace et de l'unité
        /// </summary>
        public decimal? ValeursAvancementMoisCourant { get; set; }

        /// <summary>
        /// La valeur de l'avancement au mois précédent, qu'elle soit en quantité ou en pourcentage suivi d'un espace et de l'unité
        /// </summary>
        public decimal? ValeursAvancementMoisPrecedent { get; set; }
    }
}
