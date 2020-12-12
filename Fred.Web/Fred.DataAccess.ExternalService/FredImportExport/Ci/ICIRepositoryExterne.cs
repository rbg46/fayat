using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fred.DataAccess.ExternalService.FredImportExport.Ci
{
    /// <summary>
    /// Repository externe pour les cis
    /// </summary>
    public interface ICIRepositoryExterne : IExternalRepository
    {
        /// <summary>
        /// Demande la mise a jour des cis
        /// </summary>
        /// <param name="utilisateurId">utilisateurId</param>
        /// <param name="ciIds">Liste de cis a mettre a jours</param>
        Task UpdateCisAsync(int utilisateurId, List<int> ciIds);
    }
}
