namespace Fred.Web.Shared.Models.Moyen
{
  /// <summary>
  /// Représente un type d'affectation d'un moyen
  /// </summary>
  public class AffectationMoyenTypeModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant du type d'affectation d'un moyen
    /// </summary>
    public int AffectationMoyenTypeId { get; set; }

    /// <summary>
    /// Obtient ou définit le code du type d'affectation
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé du type d'affectation
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit le code du CI
    /// </summary>
    public string CiCode { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de la famille d'affectation
    /// </summary>
    public int AffectationMoyenFamilleId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité famille d'affectation
    /// </summary>
    public AffectationMoyenFamilleModel AffectationMoyenFamille { get; set; }

    /// <summary>
    /// Obtient le code du réferentiel AffectationMoyenType
    /// </summary>
    public string CodeRef
    {
      get
      {
        return Code;
      }
    }

    /// <summary>
    /// Obtient le libelle du réferentiel AffectationMoyenType
    /// </summary>
    public string LibelleRef
    {
      get
      {
        return Libelle;
      }
    }

    /// <summary>
    /// Propriété de séléction à utiliser au niveau du Front (Par AngularJs) pour identifier les séléctions
    /// </summary>
    public bool Selected { get; set; }
  }
}
