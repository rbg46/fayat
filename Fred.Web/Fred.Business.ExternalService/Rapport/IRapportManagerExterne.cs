using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Utilisateur;
using Fred.Web.Shared.Models.Rapport;

namespace Fred.Business.ExternalService.Rapport
{
    public interface IRapportManagerExterne : IManagerExterne
    {
        /// <summary>
        /// API PRIVE FRED - FRED IE
        /// Permet d'exporter les pointages des PERSONNEL vers SAP.
        /// </summary>
        /// <param name="rapportId"> Id du rapport validé</param>
        /// </summary>  
        Task ExportPointagePersonnelToSapAsync(int rapportId);

        /// <summary>
        /// API PRIVE FRED - FRED IE
        /// Permet d'exporter les pointages des PERSONNEL vers SAP async.
        /// </summary>
        /// <param name="rapportId"> Id du rapport validé</param>
        /// <param name="currentUser">utilisateur connecté</param>
        /// </summary>
        Task ExportPointagePersonnelToSapAsync(int rapportId, UtilisateurEnt currentUser);

        /// <summary>
        /// API PRIVE FRED - FRED IE
        /// Permet d'exporter les pointages des PERSONNEL de plusieurs rapports vers SAP.
        /// </summary>
        /// <param name="rapportIds">Liste d'Id de rapports validés</param>
        Task ExportPointagePersonnelToSapAsync(List<int> rapportIds);

        /// <summary>
        /// API PRIVE FRED - FRED IEx²
        /// Permet d'exporter les pointages des PERSONNEL de plusieurs rapports vers SAP.
        /// </summary>
        /// <param name="rapportIds">Liste d'Id de rapports validés</param>
        /// <param name="currentUser">utilisateur connecté</param>
        Task ExportPointagePersonnelToSapAsync(List<int> rapportIds, UtilisateurEnt currentUser);

        /// <summary>
        /// Permet d'exporter les pointages personnel de plusieurs rapports vers TIBCO.
        /// </summary>
        /// <param name="filter">model de filtre</param> 
        Task ExportPointagePersonnelToTibcoAsync(ExportPointagePersonnelFilterModel filter);
    }
}
