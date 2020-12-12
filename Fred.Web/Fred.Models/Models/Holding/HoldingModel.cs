using Fred.Web.Models.Organisation;

namespace Fred.Web.Models.Holding
{
  public class HoldingModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une holding.
    /// </summary>
    public int HoldingId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de l'organisation due la holding
    /// </summary>
    public int OrganisationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'organisation de la holding
    /// </summary>
    public OrganisationModel Organisation { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'une holding.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'une holding.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient une concaténation du code et du libellé
    /// </summary>
    public string CodeLibelle
    {
      get
      {
        return this.Code + " - " + this.Libelle;
      }
    }
  }
}