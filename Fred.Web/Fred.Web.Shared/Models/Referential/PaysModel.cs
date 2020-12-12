using Fred.Web.Models.Referential;

namespace Fred.Web.Models
{
  public class PaysModel : IReferentialModel
  {
    /// <summary>
    /// Obtient ou définit Identifiant de Pays.
    /// </summary>
    public int PaysId { get; set; }

    /// <summary>
    /// Obtient ou définit Code ISO de Pays.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit Libellé de Pays.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    ///   Obtient le Code - Libelle du Pays
    /// </summary>
    public string CodeLibelle => Code + " - " + Libelle;

    /// <summary>
    /// Id du référentiel matétriel
    /// </summary>

    public string IdRef => this.PaysId.ToString();

    /// <summary>
    /// Libelle du référentiel matétriel
    /// </summary>
    public string LibelleRef => this.Libelle;

    /// <summary>
    /// Libelle du référentiel matétriel
    /// </summary>
    public string CodeRef => this.Code;
  }
}