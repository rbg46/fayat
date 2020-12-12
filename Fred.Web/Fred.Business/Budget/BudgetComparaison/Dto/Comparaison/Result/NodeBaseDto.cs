using System.Collections.Generic;

namespace Fred.Business.Budget.BudgetComparaison.Dto.Comparaison.Result
{
    /// <summary>
    /// Représente la base des noeuds.
    /// </summary>
    public abstract class NodeBaseDto
    {
        /// <summary>
        /// Les sous-noeuds.
        /// </summary>
        public List<NodeDto> Nodes { get; set; } = new List<NodeDto>();
    }
}
