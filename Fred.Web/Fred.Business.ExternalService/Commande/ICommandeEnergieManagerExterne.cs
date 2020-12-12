using System.Threading.Tasks;
using Fred.Business.CommandeEnergies;

namespace Fred.Business.ExternalService
{
    /// <summary>
    /// Définit un gestionnaire externe des commandes énergies.
    /// </summary>
    public interface ICommandeEnergieManagerExterne : IManagerExterne
    {
        /// <summary>
        /// Process de validation d'une commande énergie
        /// 1) Enregirement en BD
        /// 2) Envoi de la commande à SAP
        /// 3) Reception auto à 100% de la commande et envoi des réceptions à SAP
        /// 4) Clôturer la commande
        /// </summary>
        /// <param name="commandeEnergie">Commande énergie</param>
        /// <returns>Vrai si succès sinon faux</returns>
        Task<bool> ValidateAsync(CommandeEnergie commandeEnergie);
    }
}
