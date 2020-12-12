using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Params;

namespace Fred.Business.Budget.Extensions
{
    /// <summary>
    /// Classe d'extension pour les ParamValues
    /// </summary>
    public static class ParamValuesExtension
    {
        /// <summary>
        /// Récupère la valeur de la clé et retourne un true si la valeur du paramValue est égale à 1
        /// </summary>
        /// <param name="paramValues">Liste de paramvalues</param>
        /// <param name="key">clef de parametre</param>
        /// <returns>true si valeur = "1"</returns>
        public static bool GetBooleanValueFromKey(this IEnumerable<ParamValueEnt> paramValues, string key)
        {
            var paramValue = paramValues.FirstOrDefault(x => x.ParamKey.Libelle == key);
            return paramValue != null && paramValue.Valeur == "1";
        }
    }
}

