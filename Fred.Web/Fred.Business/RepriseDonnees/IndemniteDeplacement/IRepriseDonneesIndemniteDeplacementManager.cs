using System.IO;
using Fred.Entities.RepriseDonnees;

namespace Fred.Business.RepriseDonnees.IndemniteDeplacement
{
    /// <summary>
    /// Manager des reprise de données pour import des Indemnités de déplacement
    /// </summary>
    public interface IRepriseDonneesIndemniteDeplacementManager : IManager
    {
        /// <summary>
        /// Importation des Indemnité de Déplacement (avec suppression logique si déjà existante)
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>Le résultat de l'import</returns>
        ImportIndemniteDeplacementResult CreateIndemniteDeplacement(int groupeId, Stream stream);
    }
}
