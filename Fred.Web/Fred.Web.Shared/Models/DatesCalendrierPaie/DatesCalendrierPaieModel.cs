using System;

namespace Fred.Web.Models.DatesCalendrierPaie
{
  public class DatesCalendrierPaieModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique du paramétrage d'un mois.
    /// </summary>
    public int DatesCalendrierPaieId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de la société.
    /// </summary>
    public int SocieteId { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de fin de saisie des pointages.
    /// </summary>
    public DateTime? DateFinPointages { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de transfert des pointages pour la paie.
    /// </summary>
    public DateTime? DateTransfertPointages { get; set; }
  }
}