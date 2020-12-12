namespace Fred.ImportExport.Business.ValidationPointage.Factory
{
  /// <summary>
  /// Classe qui regroupe les information necessaire a la selection d'un manager de validation pointage
  /// </summary>
  public class ValidationPointageFactorySetting
  {
    /// <summary>
    /// Le code du flux
    /// </summary>
    public string FluxCode { get; set; }

    /// <summary>
    /// Le code du groupe associé au flux
    /// </summary>
    public string GroupeCode { get; set; }

    /// <summary>
    /// La chaine de connexion associé au flux
    /// </summary>
    public string ConnexionChaineSource { get; set; }
  }
}
