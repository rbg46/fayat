namespace Fred.Web.Dtos.Mobile
{
  /// <summary>
  /// Représente un code déplacement
  /// </summary>
  public class CodeDeplacementDto
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un code déplacement.
    /// </summary>
    public int CodeDeplacementId { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'un code déplacement.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'un code déplacement.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit le kilométrage minimum d'un code déplacement.
    /// </summary>
    public int KmMini { get; set; }

    /// <summary>
    /// Obtient ou définit le kilométrage maximum d'un code déplacement.
    /// </summary>
    public int KmMaxi { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si IGD est le type d'un code déplacement.
    /// </summary>
    public bool IGD { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si le code déplacement est soumis à indemnité forfaitaire ou non.
    /// </summary>
    public bool IndemniteForfaitaire { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si l'instance est actif ou non
    /// </summary>
    public bool Actif { get; set; }
  }
}
