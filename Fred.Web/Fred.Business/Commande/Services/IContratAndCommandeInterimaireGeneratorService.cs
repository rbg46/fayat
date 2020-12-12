using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Commande;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Rapport;

namespace Fred.Business.Commande.Services
{
    /// <summary>
    /// Creer des commande pour les pointages interimaires
    /// </summary>
    public interface IContratAndCommandeInterimaireGeneratorService : IService
    {

        /// <summary>
        /// Creer des commandes pour des pointages interimaires
        /// </summary>
        /// <param name="listRapport"></param>
        /// <param name="callback">methode appelée apres la creation des commandes</param>
        Task CreateCommandesForPointagesInterimairesAsync(List<RapportEnt> listRapport, Func<int, Task> callback);

        /// <summary>
        /// Creer des commandes pour des pointages interimaires
        /// </summary>
        /// <param name="rapportComplet"></param>
        /// <returns>Liste de commandes</returns>
        List<CommandeEnt> CreateCommandesForPointagesInterimaires(RapportEnt rapportComplet);

        /// <summary>
        /// Creer des commandes pour des pointages interimaires
        /// </summary>
        /// <param name="contratInterimaire">contratInterimaire</param>
        /// <param name="callback">methode appelée apres la creation des commandes</param>
        Task CreateCommandesForPointagesInterimairesAsync(ContratInterimaireEnt contratInterimaire, Func<int, Task> callback);
    }
}
