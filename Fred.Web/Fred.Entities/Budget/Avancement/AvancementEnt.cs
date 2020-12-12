using Fred.Entities.CI;
using Fred.Entities.Referential;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities.Budget.Avancement
{
  /// <summary>
  ///   Représente un avancement
  /// </summary>
  public class AvancementEnt
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'un avancement.
    /// </summary>
    public int AvancementId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du CI auquel se avancement appartient
    /// </summary>
    public int BudgetSousDetailId { get; set; }

    /// <summary>
    ///   Obtient ou définit le CI auquel ce avancement appartient
    /// </summary>
    public BudgetSousDetailEnt BudgetSousDetail { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du CI auquel se avancement appartient
    /// </summary>
    public int CiId { get; set; }

    /// <summary>
    ///   Obtient ou définit le CI auquel ce avancement appartient
    /// </summary>
    public CIEnt Ci { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la Devise attachée à cette recette
    /// </summary>
    public int DeviseId { get; set; }

    /// <summary>
    ///   Obtient ou définit la devise attachée à cette recette
    /// </summary>
    public DeviseEnt Devise { get; set; }

    /// <summary>
    /// Définit la période de début du avancement à laquel il prend effet. 
    /// Format YYYYMM
    /// </summary>
    public int Periode { get; set; }

    /// <summary>
    ///   Obtient ou définit l'état du avancement
    /// </summary>
    public AvancementEtatEnt AvancementEtat { get; set; }

    /// <summary>
    ///   Obtient ou définit l'état du avancement
    /// </summary>
    public int AvancementEtatId { get; set; }

    /// <summary>
    /// Représente toutes les étates de modification de ce avancement
    /// </summary>
    public ICollection<AvancementWorkflowEnt> Workflows { get; set; }

    /// <summary>
    ///   Obtient ou définit la quantité avancée
    /// </summary>
    public decimal? QuantiteSousDetailAvancee { get; set; }

    /// <summary>
    ///   Obtient ou définit le montant du sous-détail
    /// </summary>
    public decimal? PourcentageSousDetailAvance { get; set; }

    /// <summary>
    ///   Obtient ou définit le droit à dépenser
    /// </summary>
    public decimal DAD { get; set; }
  }
}