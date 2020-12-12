using System.Collections.Generic;
using System.Linq;
using Fred.Web.Shared.Models.Budget.Depense;
using MoreLinq;

namespace Fred.Business.Budget.Extensions
{
    /// <summary>
    /// Classe d'extension de l'explorateur de dépenses
    /// </summary>
    public static class BudgetDepenseExtension
    {
        /// <summary>
        /// Retourne le code de l'unité associé à ces dépenses
        /// Si les dépenses dans la liste ont des unités différentes alors la fonction renvoie null
        /// </summary>
        /// <param name="depenses">la liste de dépenses </param>
        /// <returns>le code de l'unité si toutes les dépenses ont la même unité,</returns>
        public static string GetUniteAssocieeDepense(this IEnumerable<BudgetDepenseModel> depenses)
        {
            var uniteFirstDepense = depenses.FirstOrDefault()?.Unite;
            if (depenses.DistinctBy(d => d.UniteId).Count() == 1 && uniteFirstDepense != null)
            {
                //Si toutes les dépenses utilisent la même unité alors on peut utiliser cette unité sinon on ne met rien
                return uniteFirstDepense.Code;
            }
            return null;
        }
    }
}
