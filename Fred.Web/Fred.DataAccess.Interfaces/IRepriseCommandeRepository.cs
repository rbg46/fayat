using Fred.Entites.RepriseDonnees.Commande;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Entities.Referential;
using System;
using System.Collections.Generic;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// Repository des Reprises des données
    /// </summary>
    public interface IRepriseCommandeRepository : IMultipleRepository
    {
        /// <summary>
        /// UnitOfWork
        /// </summary>
        IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// Recuere les commandes eyant le numero ou le NumeroCommandeExterne contenu dans la liste.
        /// </summary>
        /// <param name="numeroAndNumeroCommandeExternes">Liste desnumero de commandes contenu </param>
        /// <returns>Les commandes</returns>
        List<CommandeEnt> GetCommandes(List<string> numeroAndNumeroCommandeExternes);

        /// <summary>
        /// Recupere les type de commandes
        /// </summary>
        /// <returns>les type de commandes</returns>
        List<CommandeTypeEnt> GetCommandesTypes();

        /// <summary>
        /// Recupere la liste des fournissuer par groupe et dont le Code estcontenu dans la liste 'codes'
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="codes">codes des fournisseurs rechercher</param>
        /// <returns>les de fournisseurs</returns>
        List<FournisseurEnt> GetFournisseurByGroupeAndCodes(int groupeId, List<string> codes);

        /// <summary>
        /// Retourne la listes de devises par codes
        /// </summary>
        /// <param name="codesDevises">codesDevises</param>
        /// <returns>liste de devises</returns>
        List<DeviseEnt> GetDeviseByCodes(List<string> codesDevises);

        /// <summary>
        /// Recherche dans la base les taches a partir de Requette :
        /// Si la requette n'a pas de code alors : 
        /// - rechercher la tâche par défaut du CI identifié
        /// Si la valeur "Code" de la requette est non vide :
        /// - rechercher cette valeur parmi les tâches de niveau 3 associées au CI identifié
        /// </summary>
        /// <param name="requests">Liste de requettes </param>
        /// <returns>Liste de reponse : si la "Tache" de la reponse est vide, c'est qu'aucune tache n'a été trouvée.</returns>
        List<GetT3ByCodesOrDefaultResponse> GetT3ByCodesOrDefault(List<GetT3ByCodesOrDefaultRequest> requests);

        /// <summary>
        /// Retourne une liste d'unité en fonction d'une liste de codes
        /// </summary>
        /// <param name="codesUnites">Liste de codes</param>
        /// <returns>Liste d'unités</returns>
        List<UniteEnt> GetUnitesByCodes(List<string> codesUnites);

        /// <summary>
        /// Retourne le DepenseTypeEnt de type reception
        /// </summary>
        /// <returns>DepenseTypeEnt de type reception</returns>
        DepenseTypeEnt GetDepenseTypeReception();

        /// <summary>
        /// Creation des commandes, commandesLignes et receptions(DepenseAchatEnt)
        /// </summary>
        /// <param name="commandes">Les commandes</param>
        /// <param name="commandeLignes">Les Commandes lignes</param>
        /// <param name="receptions">Les receptions</param>
        /// <param name="entitiesCreatedcallback">Action qui sera executée apres l'insertion des entitées</param>
        void SaveEntities(List<CommandeEnt> commandes,
                    List<CommandeLigneEnt> commandeLignes,
                    List<DepenseAchatEnt> receptions,
                    Action<List<CommandeEnt>> entitiesCreatedcallback);
    }
}
