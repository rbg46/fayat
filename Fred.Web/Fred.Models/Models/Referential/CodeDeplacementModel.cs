namespace Fred.Web.Models.Referential
{
  /// <summary>
  /// Représente un code déplacement
  /// </summary>
  public class CodeDeplacementModel : IReferentialModel
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
    /// Obtient ou définit si IGD est le type d'un code déplacement.
    /// </summary>
    public bool IGD { get; set; }

    /// <summary>
    /// Obtient ou définit si le code déplacement est soumis à indemnité forfaitaire ou non.
    /// </summary>
    public bool IndemniteForfaitaire { get; set; }

    /// <summary>
    /// Obtient ou définit un code déplacement est actif ou non
    /// </summary>
    public bool Actif { get; set; }

    /// <summary>
    /// Obtient ou définit l'Id de la société 
    /// </summary>
    public int SocieteId { get; set; }

    /// <summary>
    /// Obtient l'identifiant du référentiel CodeDeplacement
    /// </summary>
    public string IdRef => this.CodeDeplacementId.ToString();

    /// <summary>
    /// Obtient le libelle du référentiel CodeDeplacement
    /// </summary>
    public string LibelleRef => this.Libelle;

    /// <summary>
    /// Obtient le code du référentiel CodeDeplacement
    /// </summary>
    public string CodeRef => this.Code;
  }
}
