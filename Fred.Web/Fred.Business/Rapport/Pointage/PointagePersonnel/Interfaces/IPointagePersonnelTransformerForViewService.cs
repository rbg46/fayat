using System;
using System.Threading.Tasks;

namespace Fred.Business.Rapport.Pointage.PointagePersonnel.Interfaces
{
    /// <summary>
    /// Service qui retourne la liste des pointages pour un personnel et pour un mois donné.
    /// </summary>
    public interface IPointagePersonnelTransformerForViewService
    {
        /// <summary>
        /// Retourne la liste des pointages pour un personnel et pour un mois donné.
        /// </summary>
        /// <param name="personnelId">personnelId</param>
        /// <param name="periode">periode</param>
        /// <returns>Liste de pointage personnel formatté pour la vue et avec les messages d'erreurs</returns>
        Task<PointagePersonnelInfo> GetListPointagesForViewAsync(int personnelId, DateTime periode);
    }
}
