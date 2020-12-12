using System.Threading.Tasks;
using Fred.Entities.Models;
using Fred.Web.Shared.Models.Commande;

namespace Fred.Business.ExternalService.Materiel
{
    /// <summary>
    /// Définit un gestionnaire externe des commandes.
    /// </summary>
    public interface ICommandeManagerExterne : IManagerExterne
    {
        /// <summary>
        /// Permet d'envoyer une commande à SAP.
        /// </summary>
        /// <param name="commandeId">L'identifiant de la commande.</param>
        /// <returns>L'identifiant du job Hangfire.</returns>
        Task<ResultModel<string>> ExportCommandeToSapAsync(int commandeId);

        /// <summary>
        /// Permet d'envoyer un avenant à SAP.
        /// </summary>
        /// <param name="commandeId">L'identifiant de la commande.</param>
        /// <param name="numeroAvenant">Le numéro d'avenant.</param>
        /// <returns>L'identifiant du job Hangfire.</returns>
        Task<ResultModel<string>> ExportCommandeAvenantToSapAsync(int commandeId, int numeroAvenant);

        /// <summary>
        /// Permet de renvoyer une commande et ses avenants à SAP.
        /// </summary>
        /// <param name="commandeId">L'identifiant de la commande.</param>
        /// <returns>Le résultat de la demande.</returns>
        Task<CommandeReturnToSap.ResultModel> ReturnCommandeToSapAsync(int commandeId);
    }
}
