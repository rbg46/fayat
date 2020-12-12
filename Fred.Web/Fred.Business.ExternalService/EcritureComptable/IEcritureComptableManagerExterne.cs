using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.JobStatut;

namespace Fred.Business.ExternalService.EcritureComptable
{
    public interface IEcritureComptableManagerExterne : IManagerExterne
    {
        /// <summary>
        /// Demande si on peux faire un import
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">date</param>
        /// <returns>bool</returns>
        Task<bool> CanImportEcritureComptablesAsync(int societeId, int ciId, DateTime dateComptable);

        /// <summary>
        /// Demande l'execution de l'import  des ecritures comptable.
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">date</param>
        /// <returns>JobStatutModel</returns>
        Task<JobStatutModel> ImportEcritureComptablesAsync(int societeId, int ciId, DateTime dateComptable);

        /// <summary>
        /// Demande l'execution de l'import des ecritures comptable pour une période
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptableDebut">Date de début</param>
        /// <param name="dateComptableFin">Date de fin</param>
        /// <returns>JobStatutModel</returns>
        Task<JobStatutModel> ImportEcritureComptablesAsync(int societeId, int ciId, DateTime dateComptableDebut, DateTime dateComptableFin);

        /// <summary>
        /// Import une liste d'écriture comptable depuis l'écran de clôture
        /// </summary>
        /// <param name="dateComptable">Date comptable</param>
        /// <param name="ciids">Liste d'identifiant de CI</param>
        /// <param name="societeId">Societe ID</param>
        /// <param name="societeCode">Code de la societe</param>
        Task ImportEcrituresComptablesFromAnaelAsync(DateTime dateComptable, List<int> ciids, int? societeId, string societeCode);
    }
}
