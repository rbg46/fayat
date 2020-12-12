using Fred.Web.Models.CI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Web.Models.Commande
{
  /// <summary>
  /// Représente graphique d'une liste de commandes groupées par CI
  /// </summary>
  public class CommandeGroupByCIModel
  {
    /// <summary>
    /// Centre d'imputation de regroupement
    /// </summary>
    public CIModel CI { get; set; }

    /// <summary>
    /// Liste des commandes regroupées
    /// </summary>
    public CommandeModel[] Commandes { get; set; }
  }
}
