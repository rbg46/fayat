using Fred.Entities.Referential;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities.Budget
{
  /// <summary>
  ///   Représente la liaison entre un budget et une tâche
  /// </summary>
  public class BudgetTacheEnt
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'une liaison entre un budget et une tâche.
    /// </summary>
    public int BudgetTacheId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du budget
    /// </summary>
    public int BudgetId { get; set; }

    /// <summary>
    ///   Obtient ou définit le budget
    /// </summary>
    public BudgetEnt Budget { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la tâche
    /// </summary>
    public int TacheId { get; set; }

    /// <summary>
    ///   Obtient ou définit la tâche
    /// </summary>
    public TacheEnt Tache { get; set; }

    /// <summary>
    ///   Obtient ou définit le commentaire pour les tâches de niveau 1 à 3
    /// </summary>
    public string Commentaire { get; set; }
  }
}
