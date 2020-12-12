namespace Fred.Web.Shared.Models.OperationDiverse
{
  /// <summary>
  /// Model pour les OD de type ventilation
  /// </summary>
  public class VentilationModel
  {
    /// <summary>
    /// Identifiant de la ventilation
    /// </summary>
    public int VentilationId { get; set; }

    /// <summary>
    /// Identifiant de la ressource
    /// </summary>
    public int ResourceId { get; set; }

    /// <summary>
    /// Nom de la ressource
    /// </summary>
    public string ResourceName { get; set; }

    /// <summary>
    /// Identifiant de la tâche
    /// </summary>
    public int TaskId { get; set; }

    /// <summary>
    /// Nom de la tâche
    /// </summary>
    public string TaskName { get; set; }

    /// <summary>
    /// Montant de la ventilation
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Commentaire lié à la ventilation
    /// </summary>
    public string Commentaire { get; set; }
    
    /// <summary>
    /// Nom de l'unité
    /// </summary>
    public string UnitName { get; set; }
    
    /// <summary>
    /// Identifiant de l'unité
    /// </summary>
    public int UnitId { get; set; }
    
    /// <summary>
    /// Libellé de la ventilation
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Prix unitaire hors taxe
    /// </summary>
    public decimal PUHT { get; set; }

    /// <summary>
    /// Quantité 
    /// </summary>
    public decimal Quantity { get; set; }
    }
}
