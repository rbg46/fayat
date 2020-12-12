using System.Collections.Generic;
using Fred.Entities.Referential;

namespace Fred.Business.Referential
{
  /// <summary>
  ///   Interface des gestionnaires des types de dépenses
  /// </summary>
  public interface ITypeDepenseManager : IManager<TypeDepenseEnt>
  {
    /// <summary>
    ///   Retourne la liste des Types de dépenses.
    /// </summary>
    /// <returns>Renvoie la liste des types de dépenses.</returns>
    IEnumerable<TypeDepenseEnt> GetTypeDepenseList();

    /// <summary>
    ///   Retourne le Type de dépenses portant l'identifiant unique indiqué.
    /// </summary>
    /// <param name="typeDepenseId">Identifiant du type de dépenses à retrouver.</param>
    /// <returns>Le Type de dépenses retrouvée, sinon nulle.</returns>
    TypeDepenseEnt GetTypeDepenseById(int typeDepenseId);

    /// <summary>
    ///   Ajout un nouveau Type de dépenses.
    /// </summary>
    /// <param name="typeDepenseEnt"> Type de dépenses à ajouter.</param>
    /// <returns> L'identifiant du type de dépenses ajoutée.</returns>
    int AddTypeDepense(TypeDepenseEnt typeDepenseEnt);

    /// <summary>
    ///   Sauvegarde les modifications d'un type de dépenses.
    /// </summary>
    /// <param name="typeDepenseEnt">Type de dépenses à modifier</param>
    void UpdateTypeDepense(TypeDepenseEnt typeDepenseEnt);

    /// <summary>
    ///   Supprime un Type de dépenses.
    /// </summary>
    /// <param name="id">L'identifiant du type de dépenses à supprimer.</param>
    void DeleteTypeDepenseById(int id);

    /// <summary>
    ///   Recherche une liste de type de dépenses.
    /// </summary>
    /// <param name="text">Le texte a chercher dans les propriétés des type de dépenses.</param>
    /// <returns>Une liste de type de dépenses.</returns>
    IEnumerable<TypeDepenseEnt> SearchTypeDepenses(string text);
  }
}