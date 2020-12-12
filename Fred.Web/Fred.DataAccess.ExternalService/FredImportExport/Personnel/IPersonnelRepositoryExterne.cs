using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fred.DataAccess.ExternalService.FredImportExport.Personnel
{
    /// <summary>
    /// Repository externe pour les personnels
    /// </summary>
    public interface IPersonnelRepositoryExterne : IExternalRepository
    {
        /// <summary>
        /// Demande la mise a jour des personnels
        /// </summary>
        /// <param name="personnelIds">Liste de cis a mettre a jours</param>
        Task UpdatePersonnelsAsync(List<int> personnelIds);

        /// <summary>
        /// Permet de demander à hangfire un job sur l'export des réceptions intérimaires vers sap
        /// </summary>
        /// <param name="societesIds">liste des sociétés</param>
        /// <param name="userId">Identifiant utilisateur</param>
        Task ExportReceptionInterimairesAsync(List<int> societesIds, int userId);

        /// <summary>
        /// Permet de demander à hangfire un job sur l'export des réceptions intérimaires vers sap avec list cis
        /// </summary>
        /// <param name="ciIds">liste des identifiants des cis</param>
        /// <param name="userId">Identifiant utilisateur</param>
        Task ExportReceptionInterimairesByCisAsync(List<int> ciIds, int userId);
    }
}
