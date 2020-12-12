using Fred.Entities.Facturation;
using Fred.Framework.Extensions;
using System.Collections.Generic;

namespace Fred.Business.Facturation
{
  /// <summary>
  ///   Methodes d'extension pour FacturationEnt
  /// </summary>
  public static class FacturationEntExtension
  {
    #region FacturationEnt

    /// <summary>
    ///   Calcul le prix unitaire facturé
    /// </summary>
    /// <param name="facturation">FacturationEnt</param>
    /// <returns>FacturationEnt avec prix unitaire facturé</returns>
    public static FacturationEnt ComputePuFacture(this FacturationEnt facturation)
    {
      facturation.PuFacture = facturation.Quantite > 0 ? facturation.MontantHT / facturation.Quantite : 0;
      return facturation;
    }

    /// <summary>
    ///   Calcul de tous les champs calculés
    /// </summary>
    /// <param name="facturation">FacturationEnt</param>
    /// <returns>FacturationEnt avec Solde FAR</returns>
    public static FacturationEnt ComputeAll(this FacturationEnt facturation)
    {
      facturation.ComputePuFacture();
      return facturation;
    }

    #endregion

    #region Liste de FacturationEnt

    /// <summary>
    ///   Calcul le prix unitaire facturé
    /// </summary>
    /// <param name="facturations">Liste de FacturationEnt</param>
    /// <returns>Liste de FacturationEnt avec prix unitaire facturé</returns>
    public static IEnumerable<FacturationEnt> ComputePuFacture(this IEnumerable<FacturationEnt> facturations)
    {
      facturations.ForEach(facturation => facturation.ComputePuFacture());
      return facturations;
    }

    /// <summary>
    ///   Calcul de tous les champs calculés
    /// </summary>
    /// <param name="facturations">Liste de FacturationEnt</param>
    /// <returns>Liste des FacturationEnt avec tous ses champs calculés</returns>
    public static IEnumerable<FacturationEnt> ComputeAll(this IEnumerable<FacturationEnt> facturations)
    {
      if (facturations != null)
      {
        facturations.ForEach(depense => depense.ComputeAll());
      }
      return facturations;
    }

    #endregion
  }
}
