using System;
using System.Collections.Generic;
using Fred.Entities.OperationDiverse;
using Fred.Web.Shared.Models.Enum;
using Fred.Web.Shared.Models.OperationDiverse;

namespace Fred.Business.OperationDiverse
{
    /// <summary>
    /// Manager des OperationDiverseAbonnement
    /// </summary>
    public interface IOperationDiverseAbonnementManager : IManager<OperationDiverseEnt>
    {
        /// <summary>
        /// Récupère toutes les opérations diverses d'un abonnement par ID du parent de l'abonnement
        /// </summary>
        /// <param name="parentODperationDiversesId">ID du parent de l'abonnement</param>
        /// <returns>Liste de <see cref="OperationDiverseEnt"></see></returns>
        IEnumerable<OperationDiverseEnt> GetODAbonnement(int parentODperationDiversesId);

        /// <summary>
        /// Recupère la liste des fréquences d'abonnement
        /// </summary>
        /// <returns>Liste de <see cref="EnumModel"/></returns>
        List<EnumModel> GetFrequenceAbonnement();

        /// <summary>
        /// Récupération de la dernière date (dernière échéance) de génération des opérations diverses abonnements
        /// </summary>
        /// <param name="datePremiereGeneration">Date de la première génération</param>
        /// <param name="frequenceAbonnement">Fréquence de l'abonnement</param>
        /// <param name="nombreOccurence">Nombre d'occurence d'opération diverse à générer</param>
        /// <returns>Date du dernier abonnement</returns>
        DateTime GetLastDayOfODAbonnement(DateTime datePremiereGeneration, int frequenceAbonnement, int nombreOccurence);

        /// <summary>
        /// Chargement des données abonnements
        /// </summary>
        /// <param name="operationDiverseModel">Opération diverse model à charger</param>
        /// <returns>Opération diverse model avec les données abonnements chargées</returns>
        OperationDiverseAbonnementModel LoadAbonnement(OperationDiverseAbonnementModel operationDiverseModel);

        /// <summary>
        /// Mise à jour d'un abonnement d'opération diverse
        /// </summary>
        /// <param name="operationDiverseModel">Opération diverse à mettre à jour</param>
        /// <returns>Liste de <see cref="OperationDiverseAbonnementModel"></see> mises à jour </returns>
        IEnumerable<OperationDiverseEnt> Update(OperationDiverseAbonnementModel operationDiverseModel);

        /// <summary>
        /// Ajout d'un abonnement d'opération diverse
        /// </summary>
        /// <param name="operationDiverseModel">Opération diverse à ajouter</param>
        /// <returns>Liste de <see cref="OperationDiverseEnt"></see> ajoutées </returns>
        IEnumerable<OperationDiverseEnt> Add(OperationDiverseAbonnementModel operationDiverseModel);

        /// <summary>
        /// Supprime tout ou partie d'un abonnement
        /// </summary>
        /// <param name="operationDiverseModel">Opération diverse à supprimer</param>
        void Delete(OperationDiverseAbonnementModel operationDiverseModel);
    }
}
