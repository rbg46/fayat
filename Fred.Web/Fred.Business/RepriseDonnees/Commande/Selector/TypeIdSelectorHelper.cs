using System.Linq;
using Fred.Business.RepriseDonnees.Commande.Models;
using Fred.Entities.Commande;

namespace Fred.Business.RepriseDonnees.Commande.Selector
{
    /// <summary>
    /// Selecteur du type de commande
    /// </summary>
    public class TypeIdSelectorHelper
    {
        /// <summary>
        /// Selection du type de la commande
        /// </summary>
        /// <param name="type">type en chaine de caractere</param>
        /// <param name="context">context</param>
        /// <returns>La valeur fred attendu</returns>
        public int? GetTypeId(string type, ContextForImportCommande context)
        {
            int? result = null;
            var typeUpper = type.ToUpper();
            switch (typeUpper)
            {
                case "FOURNITURE":
                    result = context.AllCommandesTypes.First(x => x.Code == CommandeTypeEnt.CommandeTypeF).CommandeTypeId;
                    break;
                case "LOCATION":
                    result = context.AllCommandesTypes.First(x => x.Code == CommandeTypeEnt.CommandeTypeL).CommandeTypeId;
                    break;
                case "PRESTATION":
                    result = context.AllCommandesTypes.First(x => x.Code == CommandeTypeEnt.CommandeTypeP).CommandeTypeId;
                    break;
                case "INTERIMAIRE":
                    result = context.AllCommandesTypes.First(x => x.Code == CommandeTypeEnt.CommandeTypeI).CommandeTypeId;
                    break;
                default:
                    result = null;
                    break;
            }
            return result;
        }
    }
}
