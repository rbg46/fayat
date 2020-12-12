using System;
using System.Collections.Generic;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;

namespace Fred.DataAccess.Interfaces

{
  /// <summary>
  ///   Représente un référentiel de données pour les pointages anticipées.
  /// </summary>
  public interface IPointageAnticipeRepository : IPointageBaseRepository<PointageAnticipeEnt>
  {
    /// <summary>
    ///   Recherche la liste des pointages anticipés correspondants aux prédicats
    /// </summary>
    /// <param name="predicateWhere">Prédicat de filtrage des résultats</param>
    /// <param name="orderBy">Clause de tri "champ + ordre, champ + ordre"</param>
    /// <returns>IEnumerable contenant les pointages anticipés correspondants aux critères de recherche</returns>
    IEnumerable<PointageAnticipeEnt> SearchPointageAnticipeWithFilter(Func<PointageAnticipeEnt, bool> predicateWhere, string orderBy);

    /// <summary>
    ///   Retourne une liste de pointages anticipés en fonction du personnel et d'une date
    /// </summary>
    /// <param name="personnelId">L'identifiant du personnel</param>
    /// <param name="date">La date du pointage</param>
    /// <returns>Une liste de pointages anticipés</returns>
    IEnumerable<PointageAnticipeEnt> GetListPointagesAnticipesByPersonnelIdAndDatePointage(int personnelId, DateTime date);

    /// <summary>
    ///   Retourne une liste de pointages anticipés pour un personnel dans un exercice paie
    /// </summary>
    /// <param name="personnel">Le personnel</param>
    /// <returns>Le une liste de pointages</returns>
    IEnumerable<PointageAnticipeEnt> GetListPointagesAnticipesInExerciceByPersonnel(PersonnelEnt personnel);

    /// <summary>
    ///   Récupère la liste des Rapport Ligne Anticipe par User
    /// </summary>
    /// <param name="userid">identifiant de l'utilisateur (GSP)</param>
    /// <returns>Liste des RapportLigne anticipes</returns>
    IEnumerable<int> GetAllPersonnelAnticipebyUser(int userid);
  }
}