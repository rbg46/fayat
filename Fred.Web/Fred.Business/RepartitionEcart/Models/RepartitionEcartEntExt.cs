using Fred.Entities.RepartitionEcart;
using System;
using System.Collections.Generic;

namespace Fred.Business.RepartitionEcart.Models
{
  /// <summary>
  /// Methodes d'extansion de RepartitionEcartEnt
  /// </summary>
  public static class RepartitionEcartEntExt
  {
    /// <summary>
    /// Calcule le total  d'une propriete d'une liste de RepartitionEcartEnt
    /// </summary>
    /// <param name="repartitionEcarts">repartitionEcarts</param>
    /// <param name="selector">selector</param>
    /// <returns>le total</returns>
    public static decimal CalculTotal(this List<RepartitionEcartEnt> repartitionEcarts, Func<RepartitionEcartEnt, decimal> selector)
    {
      decimal result = 0;
      foreach (var repartitionEcart in repartitionEcarts)
      {
        result += selector(repartitionEcart);
      }
      return result;
    }
  }
}