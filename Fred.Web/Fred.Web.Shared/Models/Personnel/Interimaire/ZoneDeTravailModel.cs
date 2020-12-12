using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Referential;

namespace Fred.Web.Shared.Models.Personnel.Interimaire
{
  public class ZoneDeTravailModel
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant de le contrat intérimaire
    /// </summary>
    public int ContratInterimaireId { get; set; }

    /// <summary>
    ///   Obtient ou définit le contrat intérimaire
    /// </summary>
    public ContratInterimaireEnt Contrat { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de l'établissement comptable
    /// </summary>
    public int EtablissementComptableId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'établissement comptable
    /// </summary>
    public EtablissementComptableEnt EtablissementComptable { get; set; }

  }
}
