using System.Collections.Generic;
using Fred.Business.Budget.Helpers;

namespace Fred.Business.Budget.ControleBudgetaire.Models
{
    /// <summary>
    /// Représente l'arbre des axes mais seulement avec les types d'axes et les sous axes
    /// </summary>
    public class AxeTreeLightModel
    {
        /// <summary>
        /// Le type d'axe
        /// </summary>
        public AxeTypes AxeType { get; set; }

        /// <summary>
        /// Les enfants
        /// </summary>
        public IEnumerable<AxeTreeLightModel> SousAxe { get; set; }
    }
}
