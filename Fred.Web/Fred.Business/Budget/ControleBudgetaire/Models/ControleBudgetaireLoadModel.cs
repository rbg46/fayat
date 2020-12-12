using Fred.Business.Budget.Helpers;

namespace Fred.Business.Budget.ControleBudgetaire.Models
{
    /// <summary>
    /// Décrit le model attendu lors de la construction d'un controle budgétaire
    /// </summary>
    public class ControleBudgetaireLoadModel
    {
        /// <summary>
        /// Id du CI contenant le budget en application 
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// Id de la devise pour laquelle on veut ce controle budgétaire
        /// </summary>
        public int DeviseId { get; set; }

        /// <summary>
        /// Periode au format YYYYMM
        /// </summary>
        public int PeriodeComptable { get; set; }

        /// <summary>
        /// Axe principal choisi
        /// </summary>
        public AxePrincipal AxePrincipal { get; set; }

        /// <summary>
        /// Pour pouvoir supporter un ordre arbitraire dans les axes analytiques 
        /// il faut utiliser l'axe principal custom et renvoyer le tableau d'axes analytiques
        /// L'indice 0 du tableau étant le premier niveau de l'arbre, l'indice 1 le deuxième...
        /// </summary>
        public AxeTypes[] AxeAffichees { get; set; }

        /// <summary>
        /// True si les valeurs du controle budgétaire doivent être cumulées : c'est à dire de l'ouverture du CI jusqu'a la période choisie
        /// False si les valeurs du controle budgétaire ne sont que pour la periode choisie
        /// </summary>
        public bool Cumul { get; set; }

    }
}
