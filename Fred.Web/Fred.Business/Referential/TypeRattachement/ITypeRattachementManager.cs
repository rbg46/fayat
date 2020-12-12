using System.Collections.Generic;

namespace Fred.Business.Referential
{
  /// <summary>
  ///   Interface des gestionnaires des pays
  /// </summary>
  public interface ITypeRattachementManager
  {
    /// <summary>
    ///   Retourne la liste des pays.
    /// </summary>
    /// <returns>Liste des types rattachement.</returns>
    IEnumerable<TypeRattachement.TypeRattachement> GetList();
  }
}