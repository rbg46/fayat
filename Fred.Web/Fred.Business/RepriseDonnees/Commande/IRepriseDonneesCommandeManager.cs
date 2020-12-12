using Fred.Entities.RepriseDonnees;
using System.IO;

namespace Fred.Business.RepriseDonnees.Commande
{
    /// <summary>
    /// Manager des reprise de données pour les commandes et les receptions
    /// </summary>
    public interface IRepriseDonneesCommandeManager : IManager
    {
        /// <summary>
        /// Importation des commandes et des receptions
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>le resultat de l'import</returns>
        ImportCommandeResult CreateCommandeAndReceptions(int groupeId, Stream stream);
    }
}
