using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Web.Shared.Models.Budget.ControleBudgetaire
{
  public class ControleBudgetaireSaveModelValeurs
  {
    public decimal Ajustement { get; set; }

    public string CommentaireAjustement { get; set; }

    public decimal Pfa { get; set; }

    public int TacheId { get; set; }

    public int RessourceId { get; set; }
  }
}
