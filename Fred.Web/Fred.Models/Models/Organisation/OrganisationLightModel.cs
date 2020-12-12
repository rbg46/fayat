namespace Fred.Web.Models.Organisation
{
  /// <summary>
  /// Une représentation légère de l'entité Organisation
  /// </summary>
  public class OrganisationLightModel : Referential.IReferentialModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant de l'organisation
    /// </summary>
    public int OrganisationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de l'organisation parente
    /// </summary>
    public int? PereId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant du type de l'organisation
    /// </summary>
    public int TypeOrganisationId { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé type organisation.
    /// </summary>
    public string TypeOrganisation { get; set; }

    /// <summary>
    /// Obtient ou définit le code de l'organisation
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit les codes parent
    /// </summary>
    public string CodeParents { get; set; }

    /// <summary>
    /// Obtient ou définit le code parent direct
    /// </summary>
    public string CodeParent { get; set; }

    /// <summary>
    /// Obtient l'id du référentiel
    /// </summary>
    public string IdRef => this.OrganisationId.ToString();

    /// <summary>
    /// Définit le code du référentiel
    /// </summary>
    public string CodeRef => $"{this.TypeOrganisation} - {this.Code}";

    /// <summary>
    /// Obtient l'id du référentiel
    /// </summary>
    public string LibelleRef => this.Libelle;

    public string CodeLibelle => $"{this.Code} - {this.Libelle}";
  }
}
