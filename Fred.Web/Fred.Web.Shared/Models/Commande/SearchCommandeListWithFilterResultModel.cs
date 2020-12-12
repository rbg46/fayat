using Fred.Web.Models.Commande;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models
{
  public class GetListGroupByCIResultModel
  {
    /// <summary>
    /// Liste de commandes groupé par ci en fonction du paging
    /// </summary>
   
    public IEnumerable<CommandeGroupByCIModel> GroupedCommandes { get; set; }

    /// <summary>
    /// Nombre total d'element sans le paging
    /// </summary>
    public int TotalCount { get;  set; }
  }
}
