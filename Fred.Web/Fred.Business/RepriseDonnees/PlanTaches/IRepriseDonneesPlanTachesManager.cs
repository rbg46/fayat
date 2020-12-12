using System.IO;
using Fred.Entities.RepriseDonnees;

namespace Fred.Business.RepriseDonnees.PlanTaches
{
    /// <summary>
    /// Manager des reprise de données pour les plans de taches
    /// </summary>
    public interface IRepriseDonneesPlanTachesManager : IManager
    {
        /// <summary>
        /// Importation d'un plan de taches
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>le resultat de l'import</returns>
        ImportPlanTachesResult CreatePlanTaches(int groupeId, Stream stream);
    }
}
