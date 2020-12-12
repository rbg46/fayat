using Fred.Web.Models.Personnel;
using Newtonsoft.Json;

namespace Fred.Web.Shared.Models.Commande.List
{
  public class UtilisateurForCommandeListModel
  {


    /// <summary>
    /// Obtient ou définit l'entité du membre du utilisateur ayant valider la commande
    /// </summary>
    [JsonIgnore]
    public PersonnelModel Personnel { get; set; } = null;

    /// <summary>
    /// Obtient une concaténation du nom et du prénom du membre du personnel
    /// </summary>    
    public string NomPrenom
    {
      get
      {
        return this.Personnel != null ? this.Personnel.NomPrenom : string.Empty;
      }
    }

  }
}
