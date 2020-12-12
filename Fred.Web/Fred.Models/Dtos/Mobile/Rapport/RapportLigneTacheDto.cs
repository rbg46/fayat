using Fred.Web.Models.Referential;

namespace Fred.Web.Dtos.Mobile.Rapport
{
  /// <summary>
  /// Dto RapportLigneTache
  /// </summary>
  public class RapportLigneTacheDto
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique de la ligne prime du rapport
    /// </summary>
    public int RapportLigneTacheId { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de la ligne de rapport de rattachement
    /// </summary>
    public int RapportLigneId { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de la tâche
    /// </summary>
    public int TacheId { get; set; }

    /// <summary>
    /// Obtient ou définit l'heure de la tâche
    /// </summary>
    public double HeureTache { get; set; }
  }
}