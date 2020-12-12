using System.IO;
using Fred.Entities.RepriseDonnees;

namespace Fred.Business.RepriseDonnees.ValidationCommande
{
    /// <summary>
    /// Manager des reprise de données pour la validation des commandes
    /// </summary>
    public interface IRepriseDonneesValidationCommandeManager : IManager
    {
        /// <summary>
        /// Valide une liste de commandes
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <param name="backgroundJobFunc">Fonction qui execute le jog d'envoie de la commande a SAP </param>
        /// <returns>le resultat de l'import</returns>
        ImportValidationCommandeResult ValidateCommandes(int groupeId, Stream stream, System.Func<int, string> backgroundJobFunc);

    }
}
