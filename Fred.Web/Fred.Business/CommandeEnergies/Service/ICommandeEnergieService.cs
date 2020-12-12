using System.Collections.Generic;
using Fred.Entities.Depense;

namespace Fred.Business.CommandeEnergies
{
    /// <summary>
    /// Interface Service Commande Energie
    /// </summary>
    public interface ICommandeEnergieService : IService
    {
        /// <summary>
        /// Pré-chargement d'une commande énergie en fonction d'une commande 
        /// </summary>        
        /// <param name="commande">Commande issue du front</param>
        /// <returns>Commande énergie pré-chargée</returns>
        CommandeEnergie CommandeEnergiePreloading(CommandeEnergie commande);

        /// <summary>
        /// Récupération d'une commande énergie avec tous les champs calculés
        /// Barème, Unité Bareme, Quantité pointée, Quantité convertie, Montant Valorisé, Ecart Pu, Ecart Quantité, Ecart Montant
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande énergie</param>
        /// <returns>Commande énergie</returns>
        CommandeEnergie GetCommandeEnergie(int commandeId);

        /// <summary>
        /// Réception d'une commande énergie 
        /// - Génération des DepenseAchatEnt à partir des CommandeLigneEnt de la commande énergie
        /// - Ajout en BD des DepenseAchatEnt
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>       
        void ReceptionAuto(int commandeId);

        /// <summary>
        /// Récupération des réceptions éligibles à l'envoie vers SAP d'une commande énergie
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <returns>Liste des Réceptions</returns>
        List<DepenseAchatEnt> GetReceptionsForSap(int commandeId);
    }
}
