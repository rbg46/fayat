using System.Collections.Generic;
using Fred.Entities.Referential;

namespace Fred.DataAccess.Interfaces

{
  /// <summary>
  ///   Représente un référentiel de données pour les type de dépenses.
  /// </summary>
  public interface ITypeDepenseRepository : IRepository<TypeDepenseEnt>
  {
    /// <summary>
    ///   Retourne la liste des types de dépenses.
    /// </summary>
    /// <returns>La liste des types de dépenses.</returns>
    IEnumerable<TypeDepenseEnt> GetTypeDepenseList();

    /// <summary>
    ///   Retourne le type de dépenses dont portant l'identifiant unique indiqué.
    /// </summary>
    /// <param name="typeDepenseId">Identifiant du type de dépenses à retrouver.</param>
    /// <returns>Le type de dépenses retrouvé, sinon nulle.</returns>
    TypeDepenseEnt GetTypeDepenseById(int typeDepenseId);

    /// <summary>
    ///   Ajout un nouveau type de dépenses
    /// </summary>
    /// <param name="typeDepenseEnt"> type de dépenses à ajouter</param>
    /// <returns> L'identifiant du type de dépenses ajouté</returns>
    int AddTypeDepense(TypeDepenseEnt typeDepenseEnt);

    /// <summary>
    ///   Sauvegarde les modifications d'un type de dépenses
    /// </summary>
    /// <param name="typeDepenseEnt">type de dépenses à modifier</param>
    void UpdateTypeDepense(TypeDepenseEnt typeDepenseEnt);

    /// <summary>
    ///   Supprime un type de dépenses
    /// </summary>
    /// <param name="id">L'identifiant du type de dépenses à supprimer</param>
    void DeleteTypeDepenseById(int id);

    /// <summary>
    ///   Recherche une liste de type de dépenses.
    /// </summary>
    /// <param name="text">Le texte a chercher dans les propriétés des types de dépenses.</param>
    /// <returns>Une liste de type de dépenses.</returns>
    IEnumerable<TypeDepenseEnt> SearchTypeDepense(string text);
  }
}