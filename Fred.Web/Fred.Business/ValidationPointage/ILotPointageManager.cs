using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities.ValidationPointage;

namespace Fred.Business.ValidationPointage
{
    /// <summary>
    ///   Interface Gestionnaire des Lots de Pointage
    /// </summary>
    public interface ILotPointageManager : IManager<LotPointageEnt>
    {
        /// <summary>
        ///   Récupère un Lot de Pointage en fonction de son Identifiant
        /// </summary>
        /// <param name="lotPointageId">Identifiant du lot de pointage</param>
        /// <returns>Lot de pointage</returns>
        LotPointageEnt Get(int lotPointageId);

        Task<LotPointageEnt> FindByIdNotTrackedAsync(int id);

        /// <summary>
        ///   Récupère un Lot de Pointage en fonction de son Identifiant
        /// </summary>
        /// <param name="lotPointageId">Identifiant du lot de pointage</param>
        /// <param name="includes">Includes lors du get</param>
        /// <returns>Lot de pointage</returns>
        LotPointageEnt Get(int lotPointageId, List<Expression<Func<LotPointageEnt, object>>> includes);

        /// <summary>
        /// Obtient le lot de pointage selon les paramètres spécifiés, ou le crée s'il n'existe pas
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur créateur du lot de pointage</param>
        /// <param name="periode">Date de la période du lot de pointage</param>
        /// <returns>Lot de pointage</returns>
        LotPointageEnt GetorCreate(int utilisateurId, DateTime periode);

        /// <summary>
        ///   Récupération de l'id du lot de pointage d'un utilisateur pour un période donnée
        /// </summary>
        /// <param name="utilisateurId">Identifiant du lot de pointage</param>
        /// <param name="periode">Période choisie</param>
        /// <returns>Identifiant du Lot de pointage</returns>
        int? GetLotPointageId(int utilisateurId, DateTime periode);

        /// <summary>
        ///   Récupère la liste des lots de pointage de la période donnée en fonction de l'identifiant de l'utilisateur
        ///   Selon l'AuteurVerrourId et période donnée 
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        /// <param name="periode">Période choisie</param>
        /// <returns>Lot de pointage de l'utilisateur</returns>
        LotPointageEnt Get(int utilisateurId, DateTime periode);

        /// <summary>
        ///   Récupère tous les lots de pointage confondus
        /// </summary>
        /// <returns>Liste des lots de pointage</returns>
        IEnumerable<LotPointageEnt> GetAll();

        /// <summary>
        ///   Récupère la liste des lots de pointages par période
        /// </summary>
        /// <param name="periode">période</param>
        /// <returns>Liste des lots de pointage filtré</returns>
        IEnumerable<LotPointageEnt> GetList(DateTime periode);

        /// <summary>
        ///   Récupère la liste des lots de pointages par auteur création
        /// </summary>
        /// <param name="auteurCreationId">Identifiant de l'auteur de création</param>
        /// <returns>Liste des lots de pointage filtré</returns>
        IEnumerable<LotPointageEnt> GetList(int auteurCreationId);

        /// <summary>
        ///   Ajoute un nouveau lot de pointage
        /// </summary>
        /// <param name="lotPointage">Lot de pointage a ajouter</param>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        /// <returns>Lot de pointage ajouté</returns>
        LotPointageEnt AddLotPointage(LotPointageEnt lotPointage, int utilisateurId);

        /// <summary>
        ///   Mise à jout du lot de pointage
        /// </summary>
        /// <param name="lotPointage">Lot de pointage à mettre à jour</param>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        /// <returns>Lot de pointage mis à jour</returns>
        LotPointageEnt UpdateLotPointage(LotPointageEnt lotPointage, int utilisateurId);

        /// <summary>
        ///   Supprime un lot de pointage par son identifiant
        /// </summary>
        /// <param name="lotPointageId">Identifiant du lot de pointage</param>
        void DeleteLotPointage(int lotPointageId);

        /// <summary>
        ///   Apposer un visa au lot de pointage   
        /// </summary>
        /// <param name="lotPointageId">Identifiant du Lot de pointage</param>    
        /// <returns>Lot de pointage signé</returns>
        LotPointageEnt SignLotPointage(int lotPointageId);

        /// <summary>
        /// Récupération des lots de pointages d'un utilisateur sur plusieurs périodes
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        /// <param name="periodes">Liste de périodes (string yyyyMM)</param>
        /// <returns>Dictionnaire (string periode yyyyMM, lotPointageId)</returns>
        Dictionary<string, int?> GetLotPointageId(int utilisateurId, List<string> periodes);

        /// <summary>
        /// Get lot pointage by userId and periode
        /// </summary>
        /// <param name="utilisateursIds">List des utilisateurs ids</param>
        /// <param name="periode">Période</param>
        /// <returns>Lot de pointage</returns>
        IEnumerable<LotPointageEnt> GetLotPointageByListUserIdAndPeriode(List<int> utilisateursIds, DateTime periode);
    }
}
