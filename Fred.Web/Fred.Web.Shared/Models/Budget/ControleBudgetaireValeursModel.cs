using Fred.Entities.Budget;
using Fred.Entities.Referential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Web.Shared.Models.Budget
{
  public class ControleBudgetaireValeursModel
  {
    /// <summary>
    /// Un controle budgétaire estl ié à un budget
    /// </summary>
    public BudgetEnt Budget { get; set; }

    /// <summary>
    /// Id du budget auquel on est rattaché
    /// </summary>
    public int BudgetId { get; set; }

    /// <summary>
    /// Une valeur du controle budgétaire est nécessairement liée à une tache
    /// </summary>

    public TacheEnt Tache { get; set; }

    /// <summary>
    /// Id de la tache a laquelle on est rattachée
    /// </summary>
    public int TacheId { get; set; }


    /// <summary>
    /// Ce jeu de données n'est valable que pour un mois et une année, l'entier est au format YYYYMM
    /// </summary>
    public int PeriodeCreation { get; set; }


    /// <summary>
    /// Valeur de l'ajustement e.g Tel tache coutera finalement X€ plus cher 
    /// </summary>
    public decimal Ajustement { get; set; }

    /// <summary>
    /// Le commantaire liée à la valeur du montant de l'ajustement
    /// </summary>
    public string CommentaireAjustement { get; set; }

    /// <summary>
    /// Le montant de la prévision fin affaire calculée pour le mois précédent (YYYYMM -1 mois)
    /// </summary>
    public decimal Pfa { get; set; }
  }
}
