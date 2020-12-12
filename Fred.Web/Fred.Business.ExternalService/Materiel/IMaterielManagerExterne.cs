using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Utilisateur;

namespace Fred.Business.ExternalService.Materiel
{
    /// <summary>
    /// Définit un gestionnaire externe des matériel.
    /// </summary>
    public interface IMaterielManagerExterne : IManagerExterne
    {
        /// <summary>
        /// Permet d'exporter les pointages des materiels d'un rapport vers STORM.
        /// </summary>
        /// <param name="rapportId">L'identifiant d'un rapport.</param>
        Task ExportPointageMaterielToStormAsync(int rapportId);

        /// <summary>
        /// Permet d'exporter les pointages des materiels d'un rapport vers STORM en asynchrone.
        /// </summary>
        /// <param name="rapportId">L'identifiant d'un rapport.</param>
        /// <param name="currentUser">Utilisateur connecté</param>
        Task ExportPointageMaterielToStormAsync(int rapportId, UtilisateurEnt currentUser);

        /// <summary>
        /// Permet d'exporter les pointages des materiels d'une liste de rapports vers STORM
        /// </summary>
        /// <param name="rapportIds">Liste d'identifiants de rapports.</param>
        Task ExportPointageMaterielToStormAsync(List<int> rapportIds);

        /// <summary>
        /// Permet d'exporter les pointages des materiels d'une liste de rapports vers STORM en asynchrone.
        /// </summary>
        /// <param name="rapportIds">Liste d'identifiants de rapports.</param>
        /// <param name="currentUser">Utilisateur connecté</param>
        Task ExportPointageMaterielToStormAsync(List<int> rapportIds, UtilisateurEnt currentUser);
    }
}
