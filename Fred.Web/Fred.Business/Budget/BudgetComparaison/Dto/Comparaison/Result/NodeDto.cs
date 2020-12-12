using System.Diagnostics;

namespace Fred.Business.Budget.BudgetComparaison.Dto.Comparaison.Result
{
    /// <summary>
    /// Représente un noeud.
    /// </summary>
    [DebuggerDisplay("{Code} - {Libelle}")]
    public class NodeDto : NodeBaseDto
    {
        /// <summary>
        /// Le code du noeud.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Le libellé du noeud.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Le budget 1.
        /// </summary>
        public GroupDto Budget1 { get; private set; } = new GroupDto();

        /// <summary>
        /// Le budget 2.
        /// </summary>
        public GroupDto Budget2 { get; private set; } = new GroupDto();

        /// <summary>
        /// L'écart.
        /// </summary>
        public GroupDto Ecart { get; private set; } = new GroupDto();
    }
}
