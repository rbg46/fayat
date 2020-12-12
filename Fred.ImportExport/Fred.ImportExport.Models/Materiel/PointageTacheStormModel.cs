namespace Fred.ImportExport.Models.Materiel
{
  /// <summary>
  /// Représente un model pour la tache d'un pointage STORM
  /// </summary>
  public class PointageTacheStormModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une tâche de pointage.
    /// </summary>
    public int RapportTachesId { get; set; }

    /// <summary>
    /// Obtient ou définit le nombre d'heures d'une tâche.
    /// </summary>
    public double HeureTache { get; set; }
  }
}
