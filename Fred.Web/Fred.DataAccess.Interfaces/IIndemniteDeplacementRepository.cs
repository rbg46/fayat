
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Entities.IndemniteDeplacement;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les indemnites de deplacement
    /// </summary>
    public interface IIndemniteDeplacementRepository : IRepository<IndemniteDeplacementEnt>
    {
        /// <summary>
        ///   Retourne la liste de tous les indemnites de deplacement.
        /// </summary>
        /// <returns>List de toutes les sociétés</returns>
        IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementListAll();

        /// <summary>
        ///   La liste de tous les indemnites de deplacement par personnel.
        /// </summary>
        /// <param name="personnelId">Id unique d'un personnel</param>
        /// <returns>Renvoie la liste de des indemnites de deplacement active</returns>
        IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByPersonnelListAll(int personnelId);

        /// <summary>
        ///   La liste de tous les indemnites de deplacement.
        /// </summary>
        /// <param name="personnelId">Id unique d'un personnel</param>
        /// <returns>Renvoie la liste de des indemnites de deplacement active par personnel</returns>
        IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByPersonnelList(int personnelId);

        /// <summary>
        ///   La liste de tous les indemnites de deplacement par CI.
        /// </summary>
        /// <param name="idCi">id unique d'un CI</param>
        /// <returns>Renvoie la liste de des indemnites de deplacement active</returns>
        IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByCiListAll(int idCi);

        /// <summary>
        ///   La liste de tous les indemnites de deplacement active par CI.
        /// </summary>
        /// <param name="idCi">id unique d'un CI</param>
        /// <returns>Renvoie la liste de des indemnites de deplacement active</returns>
        IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByCiList(int idCi);

        /// <summary>
        ///   La liste de tous les indemnites de deplacement.
        /// </summary>
        /// <returns>Renvoie la liste des indemnites de deplacement active</returns>
        IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementList();

        /// <summary>
        ///   La liste de tous les indemnites de deplacement pour un personnel et une affaire.
        /// </summary>
        /// <param name="personnelId">Id unique d'un personnel</param>
        /// <param name="ciId">Id unique d'une affaire</param>
        /// <returns>Renvoie la liste de des indemnites de deplacement pour un personnel et une affaire</returns>
        IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByCi(int personnelId, int ciId);

        /// <summary>
        ///   Retourne l'indemnites de deplacement pour un personnel et une affaire.
        /// </summary>
        /// <param name="personnelId">Id unique d'un personnel</param>
        /// <param name="ciId">Id unique d'une affaire</param>
        /// <returns>Renvoie une indemnite de deplacement pour un personnel et une affaire</returns>
        IndemniteDeplacementEnt GetIndemniteDeplacementByPersonnelIdAndCiId(int personnelId, int ciId);

        /// <summary>
        ///   La liste de tous les indemnites de deplacement pour un personnel.
        /// </summary>
        /// <param name="personnelId">Id unique d'un personnel</param>
        /// <returns>Renvoie la liste de des indemnites de deplacement pour un personnel</returns>
        IQueryable<IndemniteDeplacementEnt> GetIndemniteDeplacementByPersonnel(int personnelId);

        /// <summary>
        ///   Sauvegarde les modifications d'un IndemniteDeplacement
        /// </summary>
        /// <param name="indemniteDeplacement">Indemnite de deplacement à modifier</param>
        /// <returns>L'identifiant de l'indemnite de deplacement modifiée</returns>
        int UpdateIndemniteDeplacement(IndemniteDeplacementEnt indemniteDeplacement);

        /// <summary>
        ///   Ajout d'une indemnite de deplacement
        /// </summary>
        /// <param name="indemniteDeplacement">Indemnite de deplacement à ajouter</param>
        /// <returns>L'identifiant de l'indemnite de deplacement ajouté</returns>
        int AddIndemniteDeplacement(IndemniteDeplacementEnt indemniteDeplacement);

        /// <summary>
        ///   Supprime un indemnite de deplacement
        /// </summary>
        /// <param name="id">L'identifiant de l'indemnite de deplacement à supprimer</param>
        /// <param name="idUtilisateur">Identifiant de l'utilisateur ayant fait l'action</param>
        void DeleteIndemniteDeplacementById(int id, int idUtilisateur);

        /// <summary>
        ///   Supprime PHYSIQUEMENT une indemnite de deplacement
        /// </summary>
        /// <param name="id">L'identifiant de l'indemnite de deplacement à supprimer</param>
        void RemoveIndemniteDeplacementById(int id);

        /// <summary>
        ///   Indemnite de deplacement via l'id
        /// </summary>
        /// <param name="id">Id de l'indemnite de deplacement</param>
        /// <returns>Renvoie un indemnite de deplacement</returns>
        IndemniteDeplacementEnt GetIndemniteDeplacementById(int id);

        /// <summary>
        ///   Indemnite de deplacement via personnelid
        /// </summary>
        /// <param name="personnelId">Id du personnel</param>
        /// <returns>Renvoie une indemnite de deplacement</returns>
        IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByPersonnelId(int personnelId);

        /// <summary>
        ///   Import des indemnites de deplacement automatiques depuis la Holding
        /// </summary>
        /// <param name="holdingId"> Id du Holding</param>
        /// <returns>Renvoie un int</returns>
        int ImportIndemniteDeplacementFromHolding(int holdingId);

        /// <summary>
        ///   Permet de récupérer la liste des indemnites de deplacement en fonction des critères de recherche.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur les indemnites de deplacement</param>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <returns>Retourne la liste filtrée des indemnites de deplacement</returns>
        IEnumerable<IndemniteDeplacementEnt> SearchIndemniteDeplacementWithFilters(Expression<Func<IndemniteDeplacementEnt, bool>> predicate, int personnelId);

        /// <summary>
        ///   Permet de récupérer la liste de tous les indemnites de deplacement en fonction des critères de recherche.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur tous les indemnites de deplacement</param>
        /// <returns>Retourne la liste filtrée de tous les indemnites de deplacement</returns>
        IEnumerable<IndemniteDeplacementEnt> SearchIndemniteDeplacementAllWithFilters(Func<IndemniteDeplacementEnt, bool> predicate);

        /// <summary>
        ///   Permet de récupérer la liste de tous les indemnites de deplacement en fonction des critères de recherche par
        ///   personnel.
        /// </summary>
        /// <param name="personnelId">Id personnel</param>
        /// <returns>Retourne la liste filtrée de tous les indemnites de deplacement</returns>
        IEnumerable<IndemniteDeplacementEnt> SearchIndemniteDeplacementAllByPersonnelIdWithFilters(int personnelId);

        /// <summary>
        /// Retourne les indemnités de déplacement à utiliser lors de l'export KLM.
        /// </summary>
        /// <param name="societeId">Identifiant de a société</param>
        /// <returns>Les indemnités de déplacement à utiliser lors de l'export KLM</returns>
        IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementForExportKlm(int societeId);
    }
}
