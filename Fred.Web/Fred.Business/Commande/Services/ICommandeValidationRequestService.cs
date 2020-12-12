using Fred.Entities.Avis;

namespace Fred.Business.Commande.Services
{
    /// <summary>
    /// Service permettant la gestion des avis sur les commandes
    /// </summary>
    public interface ICommandeValidationRequestService : IService
    {
        /// <summary>
        /// Demander une validation sur une commande / commandeAvenant
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <param name="commandeAvenantId">Identifiant de l'avenant</param>
        /// <param name="avis">Avis à sauvegarder</param>
        /// <param name="baseUrl">Base url</param>
        void RequestValidation(int commandeId, int? commandeAvenantId, AvisEnt avis, string baseUrl);
    }
}
