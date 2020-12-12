using Fred.Web.Models.Organisation;

namespace Fred.Web.Models.OrganisationGenerique
{
  public class OrganisationGeneriqueModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une organisation générique.
    /// </summary>
    public int OrganisationGeneriqueId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de l'organisation de l'organisation générique
    /// </summary>
    public int OrganisationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'organisation de l'organisation générique
    /// </summary>
    public OrganisationModel Organisation { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'une organisation générique.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'une organisation générique.
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