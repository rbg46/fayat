using System.Linq;
using System.Threading.Tasks;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Fred.Framework.Exceptions;

namespace Fred.Business.Commande.Services
{
    /// <summary>
    /// Envoie de commande Flux ME21
    /// </summary>
    public class CommandeFluxService : ICommandeFluxService
    {
        private readonly ICommandeRepository commandeRepository;

        public CommandeFluxService(ICommandeRepository commandeRepository)
        {
            this.commandeRepository = commandeRepository;
        }

        /// <inheritdoc/>
        public async Task<int> GetOrganisationIdByCommandeIdAsync(int commandeId)
        {
            return await commandeRepository.GetOrganisationIdByCommandeIdAsync(commandeId);
        }

        /// <inheritdoc/>
        public async Task<CommandeEnt> GetCommandeSAPAsync(int commandeId, string commandeFluxMe21Error)
        {
            var commande = await commandeRepository.GetCommandeSAPAsync(commandeId);

            if (commande == null)
            {
                throw new FredBusinessException($"{commandeFluxMe21Error} {commandeId.ToString()}");
            }

            return commande.ComputeAll();
        }

        /// <inheritdoc/>
        public async Task<CommandeEnt> GetCommandeAvenantSAPAsync(int commandeId, int numeroAvenant, string commandeFluxMe22Error)
        {
            var commande = await commandeRepository.GetCommandeAvenantSAPAsync(commandeId, numeroAvenant);

            if (commande == null)
            {
                throw new FredBusinessException($"{commandeFluxMe22Error} {commandeId.ToString()}");
            }

            // Filtre sur l'avenant numéro 0 que nous ne devons pas renvoyer à SAP sinon erreur de doublon côté SAP
            commande.Lignes = commande.Lignes.Where(l => l.AvenantLigne?.Avenant != null && l.AvenantLigne.Avenant.NumeroAvenant == numeroAvenant).ToList();

            return commande.ComputeAll();
        }
    }
}
