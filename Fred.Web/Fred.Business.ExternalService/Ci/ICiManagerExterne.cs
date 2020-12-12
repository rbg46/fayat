using System;
using System.Threading.Tasks;
using Fred.Entities.CI;

namespace Fred.Business.ExternalService.Ci
{
    /// <summary>
    /// Manager qui gere les interaction avec fred ie pour les Cis
    /// </summary>
    public interface ICiManagerExterne : IService
    {
        /// <summary>
        /// Action qui est executée lors d'une mise a jour d'un ci depuis l'interface utilisateur
        /// Ici, je passe en parametre une Func de CIEnt car je ne voulais pas modifié le code existant 
        /// et execute le code apres avoir recupere le ci avant l'action de mise a jour
        /// </summary>
        /// <param name="ciId">L'id du ci</param>
        /// <param name="updateAction">Action de mise a jour cote fred</param>
        /// <returns>Le Ci mis a jour</returns>
        Task<CIEnt> OnUpdateCiAsync(int ciId, Func<CIEnt> updateAction);
    }
}
