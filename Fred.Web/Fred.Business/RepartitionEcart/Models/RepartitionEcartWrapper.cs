using Fred.Entities.RepartitionEcart;
using System.Collections.Generic;

namespace Fred.Business.Models.RepartitionEcart
{
  /// <summary>
  /// RepartitionEcartWrapper
  /// </summary>
  public class RepartitionEcartWrapper
  {
    /// <summary>
    /// RepartitionEcartMateriel
    /// </summary>
    public List<RepartitionEcartEnt> RepartitionEcarts { get; set; } = new List<RepartitionEcartEnt>();
   
    /// <summary>
    ///   Obtient ou définit le total des Valorisations initiales.
    /// </summary>   
    public decimal TotalValorisationInitiale { get; set; }        

    /// <summary>
    ///   Obtient ou définit le total des montant Capitalise.
    /// </summary>  
    public decimal TotalMontantCapitalise { get; set; }

    /// <summary>
    ///   Obtient ou définit le total des ecarts.
    /// </summary>   
    public decimal TotalEcart { get; set; }

    /// <summary>
    /// Permet de savoir si les repartition sont cloturées.
    /// </summary>
    public bool IsClosed { get; set; }

  }
}
