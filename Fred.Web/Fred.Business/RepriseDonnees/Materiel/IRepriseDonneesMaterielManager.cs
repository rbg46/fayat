using System.IO;
using Fred.Entities.RepriseDonnees;

namespace Fred.Business.RepriseDonnees.Materiel
{
    /// <summary>
    /// Manager des reprise de données pour import des Materiels
    /// </summary>
    public interface IRepriseDonneesMaterielManager : IManager
    {
        /// <summary>
        /// Importation des Materiels
        /// </summary>
        /// <param name="groupeId">groupe Id</param>
        /// <param name="stream">stream</param>
        /// <returns>le résultat de l'import</returns>
        ImportMaterielResult CreateMateriel(int groupeId, Stream stream);
    }
}
