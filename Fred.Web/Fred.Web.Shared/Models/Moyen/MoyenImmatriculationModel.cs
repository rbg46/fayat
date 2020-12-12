namespace Fred.Web.Shared.Models.Moyen
{
  /// <summary>
  /// Représente une immatriculation d'un moyen
  /// </summary>
  public class MoyenImmatriculationModel
  {
    /// <summary>
    /// Obtient ou définit le numéro d'immatriculation
    /// </summary>
    public string Immatriculation { get; set; }

    /// <summary>
    /// Obtient ou définit le libelle du moyen
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit le code reférentiel d'immatriculation
    /// </summary>
    public string CodeRef
    {
      get
      {
        return Immatriculation;
      }
    }
  }
}
