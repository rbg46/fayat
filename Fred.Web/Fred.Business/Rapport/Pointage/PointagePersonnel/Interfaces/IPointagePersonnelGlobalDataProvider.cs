using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Rapport;

namespace Fred.Business.Rapport.Pointage.PointagePersonnel.Interfaces
{
    /// <summary>
    /// Permet de recuperer les pointages vace les infomations calculer et les message d'erreurs
    /// </summary>
    public interface IPointagePersonnelGlobalDataProvider
    {
        /// <summary>
        /// Permet de recuperer les pointages vace les infomations calculer et les message d'erreurs
        /// </summary>
        /// <param name="listPointages">Les lignes de rapports</param>
        /// <param name="personnelId">l'id du personnel</param>
        /// <param name="periode">le mois sur lequel la recheche est effectuée.</param>
        /// <returns>Les données necessaires pour calculé les champs caclulés et determiner les message d'erreurs</returns>
        Task<PointagePersonnelGlobalData> GetDataForFormatRapportLignesForViewAsync(IEnumerable<RapportLigneEnt> listPointages, int personnelId, DateTime periode);
    }
}
