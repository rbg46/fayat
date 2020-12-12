using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Web.Shared.Models.Budget.ControleBudgetaire
{
  public class ControleBudgetaireSaveModel
  {
    public int BudgetId { get; set; }

    /// <summary>
    /// Periode entière au format YYYYMM
    /// </summary>
    public int Periode { get; set; }

    public IEnumerable<ControleBudgetaireSaveModelValeurs> ControleBudgetaireValeurs { get; set; }
  }
}
