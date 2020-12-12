using Fred.Entities.Commande;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Rapport;

namespace Fred.Web.Models.Commande
{
  public class CommandeContratInterimaireModel
  {

    /// <summary>
    /// Obtient ou définit l'identifiant de la commande.
    /// </summary>
    public int CommandeId { get; set; }

    /// <summary>
    /// Obtient ou définit la commande.
    /// </summary>
    public CommandeEnt Commande { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant du contrat intérimaire.
    /// </summary>
    public int ContratId { get; set; }

    /// <summary>
    /// Obtient ou définit le contrat intérimaire
    /// </summary>
    public ContratInterimaireEnt Contrat { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant du CI
    /// </summary>
    public int CiId { get; set; }

    /// <summary>
    /// Obtient ou définit le CI
    /// </summary>
    public CommandeEnt Ci { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant du intérimaire
    /// </summary>
    public int InterimaireId { get; set; }

    /// <summary>
    /// Obtient ou définit le intérimaire
    /// </summary>
    public PersonnelEnt Interimaire { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de la ligne de rapport
    /// </summary>
    public int RapportLigneId { get; set; }

    /// <summary>
    /// Obtient ou définit la ligne de rapport
    /// </summary>
    public RapportLigneEnt RapportLigne { get; set; }


  }
}
