namespace Fred.Web.Models.Personnel
{
  /// <summary>
  /// Représente un membre du personnel
  /// </summary>
  public class PersonnelLightModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique du membre du personnel
    /// </summary>
    public int PersonnelId { get; set; }
    
    /// <summary>
    /// Obtient ou définit le nom du membre du personnel
    /// </summary>
    public string Nom { get; set; }

    /// <summary>
    /// Obtient ou définit le prénom du membre du personnel
    /// </summary>
    public string Prenom { get; set; }

    /// <summary>
    ///   Obtient une concaténation du nom et du prénom du membre du personnel
    /// </summary>
    public string NomPrenom => Nom + " " + Prenom;
    }
}