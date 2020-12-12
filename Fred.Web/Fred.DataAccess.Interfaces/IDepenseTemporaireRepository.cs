using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.Entities.Depense;

namespace Fred.DataAccess.Interfaces

{
  /// <summary>
  ///   Représente un référentiel de données pour les dépenses temporaire.
  /// </summary>
  public interface IDepenseTemporaireRepository : IRepository<DepenseTemporaireEnt>
  {
    /// <summary>
    ///   Retourne la liste des dépenses temporaire.
    /// </summary>
    /// <returns>La liste des dépenses temporaire.</returns>
    IEnumerable<DepenseTemporaireEnt> GetDepenseList();

    /// <summary>
    ///   Retourne la liste des depenses filtrée selon les critères de recherche.
    /// </summary>
    /// <param name="filter">Objet de recherche et de tri des dépense temporaire</param>
    /// <param name="page">Page courante</param>
    /// <param name="pageSize">Taille de la page</param>
    /// <returns>La liste des depenses filtrées selon les critères de recherche et ordonnées selon les critères de tri</returns>
    IEnumerable<DepenseTemporaireEnt> SearchDepenseListWithFilter(
      SearchDepenseTemporaireEnt filter,
      int page,
      int pageSize);

    /// <summary>
    ///   Retourne le montant total de la liste des depenses filtrée selon les critères de recherche.
    /// </summary>
    /// <param name="predicateWhere">Prédicat de filtrage des depenses</param>
    /// <returns>
    ///   le montant total de La liste des depenses filtrées selon les critères de recherche et ordonnées selon les
    ///   critères de tri
    /// </returns>
    double GetMontantTotalDepenseWithFilter(Expression<Func<DepenseTemporaireEnt, bool>> predicateWhere);

    /// <summary>
    ///   Retourne une liste de dépenses temporaire en fonction d'un identifiant de CI et d'une date comptable
    ///   et qui n'ont pas été supprimées
    /// </summary>
    /// <param name="ciId">Identifiant du CI associé</param>
    /// <param name="dateComptable">Date comptable de la dépense</param>
    /// <returns>Une liste de dépenses temporaire</returns>
    IEnumerable<DepenseTemporaireEnt> GetListDepenseByCiIdAndDateComptable(int ciId, DateTime dateComptable);

    /// <summary>
    ///   Retourne la liste complète des dépenses temporaires en fonction d'un identifiant de dépense parente
    /// </summary>
    /// <param name="depenseParentId">Identifiant de la dépense parente associée</param>
    /// <returns>Une liste de dépenses temporaire</returns>
    IEnumerable<DepenseTemporaireEnt> GetDepenseListByDepenseParentId(int depenseParentId);

    /// <summary>
    ///   Retourne la liste complète des dépenses temporaire en fonction d'un identifiant de CI et d'une date comptable
    /// </summary>
    /// <param name="ciId">Identifiant du CI associé</param>
    /// <param name="dateComptable">Date comptable de la dépense</param>
    /// <returns>Une liste de dépenses temporaire</returns>
    IEnumerable<DepenseTemporaireEnt> GetTotalDepenseByCiIdAndDateComptable(int ciId, DateTime dateComptable);

    /// <summary>
    ///   Retourne la dépense portant l'identifiant unique indiqué.
    /// </summary>
    /// <param name="depenseId">Identifiant de la dépense à retrouver.</param>
    /// <returns>La dépense retrouvée, sinon nulle.</returns>
    DepenseTemporaireEnt GetDepenseById(int depenseId);

    /// <summary>
    ///   Ajout une nouvelle dépense.
    /// </summary>
    /// <param name="depense">dépense à ajouter</param>
    /// <returns> L'identifiant de la dépense ajoutée</returns>
    int AddDepense(DepenseTemporaireEnt depense);

    /// <summary>
    ///   Sauvegarde les modifications d'une dépense.
    /// </summary>
    /// <param name="depense">dépense à modifier</param>
    void UpdateDepense(DepenseTemporaireEnt depense);

    /// <summary>
    ///   Supprime une dépense
    /// </summary>
    /// <param name="id">L'identifiant du dépense à supprimer</param>
    void DeleteDepenseById(int id);
  }
}