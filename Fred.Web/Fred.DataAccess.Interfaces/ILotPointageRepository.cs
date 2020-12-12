using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities.ValidationPointage;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    ///   Interface ILotPointageRepository
    /// </summary>
    public interface ILotPointageRepository : IRepository<LotPointageEnt>
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
        ///   Récupération du lot de pointage d'un utilisateur pour un période donnée
        /// </summary>
        /// <param name="utilisateurId">Identifiant du lot de pointage</param>
        /// <param name="periode">Période choisie</param>
        /// <returns>Lot de pointage</returns>
        LotPointageEnt Get(int utilisateurId, DateTime periode);

        /// <summary>
        ///   Récupération du lot de pointage d'un utilisateur pour un période donnée
        /// </summary>
        /// <param name="utilisateurId">Identifiant du lot de pointage</param>
        /// <param name="periode">Période choisie</param>
        /// <returns>Lot de pointage</returns>
        LotPointageEnt GetWithoutLines(int utilisateurId, DateTime periode);

        /// <summary>
        ///   Récupération de l'id du lot de pointage d'un utilisateur pour un période donnée
        /// </summary>
        /// <param name="utilisateurId">Identifiant du lot de pointage</param>
        /// <param name="periode">Période choisie</param>
        /// <returns>Identifiant du Lot de pointage</returns>
        int? GetLotPointageId(int utilisateurId, DateTime periode);

        /// <summary>
        ///   Récupère tous les lots de pointage confondus
        /// </summary>
        /// <returns>Liste des lots de pointage</returns>
        IEnumerable<LotPointageEnt> GetAll();

        /// <summary>
        ///   Récupère la liste des lots de pointages par période
        /// </summary>
        /// <param name="periode">période choisie</param>
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
        /// Get lot pointage by userId and periode
        /// </summary>
        /// <param name="utilisateursIds">List des utilisateurs ids</param>
        /// <param name="periode">Période</param>
        /// <returns>Lot de pointage</returns>
        IEnumerable<LotPointageEnt> GetLotPointageByListUserIdAndPeriode(List<int> utilisateursIds, DateTime periode);
    }
}
