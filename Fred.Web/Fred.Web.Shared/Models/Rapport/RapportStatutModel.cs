namespace Fred.Web.Models.Rapport
{
  /// <summary>
  /// Modèle du statut du rapport.
  /// </summary>
  public class RapportStatutModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un statut de rapport.
    /// </summary>
    public int RapportStatutId { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'un statut de rapport.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'un statut de rapport.
    /// </summary>
    public string Libelle { get; set; }
  }
}