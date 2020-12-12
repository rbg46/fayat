using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.JobStatut;

namespace Fred.DataAccess.ExternalService.FredImportExport.EcritureComptable
{
    public interface IEcritureComptableRepositoryExterne : IExternalRepository
    {
        /// <summary>
        /// Verifie si on peux faire l'import
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="societeId">societeId</param>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">dateComptable</param>
        /// <returns>JobStatutModel</returns>
        Task<JobStatutModel> CheckImportEcritureComptablesAsync(int userId, int societeId, int ciId, DateTime dateComptable);

        /// <summary>
        /// importe les ecriture comptable
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="societeId">societeId</param>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">dateComptable</param>
        /// <returns>JobStatutModel</returns>
        Task<JobStatutModel> ImportEcritureComptablesAsync(int userId, int societeId, int ciId, DateTime dateComptable);

        Task<JobStatutModel> ImportEcritureComptablesAsync(int userId, string societeCode, int societeId, int ciId, DateTime dateComptableDebut, DateTime dateComptableFin);

        Task ImportEcrituresComptablesFromAnaelAsync(DateTime dateComptable, List<int> ciids, int? societeId, string societeCode);
    }
}
