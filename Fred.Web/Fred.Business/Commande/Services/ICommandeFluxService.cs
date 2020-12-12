using System.Threading.Tasks;
using Fred.Entities.Commande;

namespace Fred.Business.Commande.Services
{
    /// <summary>
    /// Envoie Commande Flux ME21
    /// </summary>
    public interface ICommandeFluxService : IService
    {
        /// <summary>
        /// Get organisation id for this commande
        /// </summary>
        /// <param name="commandeId">l'id de la commande</param>
        /// <returns>organisation id</returns>
        Task<int> GetOrganisationIdByCommandeIdAsync(int commandeId);

        /// <summary>
        /// Permet de récupérer une commande pour l'export SAP.
        /// </summary>
        /// <param name="commandeId">L'identifiant d'une commande.</param>
        /// <param name="commandeFluxMe21Error">error message</param>
        /// <returns>Une commande.</returns>
        Task<CommandeEnt> GetCommandeSAPAsync(int commandeId, string commandeFluxMe21Error);

        /// <summary>
        /// Permet de récupérer un avenant d'une commande pour l'export SAP.
        /// </summary>
        /// <param name="commandeId">L'identifiant d'une commande.</param>
        /// <param name="numeroAvenant">Avenant number</param>
        /// <param name="commandeFluxMe22Error">error message</param>
        /// <returns>Une commande.</returns>
        Task<CommandeEnt> GetCommandeAvenantSAPAsync(int commandeId, int numeroAvenant, string commandeFluxMe22Error);
    }
}
