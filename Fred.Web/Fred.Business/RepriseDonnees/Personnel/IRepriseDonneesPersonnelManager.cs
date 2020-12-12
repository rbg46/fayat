using System.IO;
using Fred.Entities.RepriseDonnees;

namespace Fred.Business.RepriseDonnees.Personnel
{
    /// <summary>
    /// Manager des reprise de données pour import des Personnels
    /// </summary>
    public interface IRepriseDonneesPersonnelManager : IManager
    {
        /// <summary>
        /// Importation des Personnels
        /// </summary>
        /// <param name="groupeId">groupe Id</param>
        /// <param name="stream">stream</param>
        /// <returns>le résultat de l'import</returns>
        ImportPersonnelResult CreatePersonnel(int groupeId, Stream stream);
    }
}
