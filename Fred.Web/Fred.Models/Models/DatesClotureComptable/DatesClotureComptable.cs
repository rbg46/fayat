using Fred.Web.Models.Organisation;
using Fred.Web.Models.Pole;
using Fred.Web.Models.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.DatesClotureComptable
{
  public class DatesClotureComptableModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique de la cloture comptable.
    /// </summary>
    public int DatesClotureComptableId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de la CI.
    /// </summary>
    public int CiId { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de fin de saisie des clotures comptables.
    /// </summary>
    public DateTime? DateArretSaisie { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de transfert des clotures comptables.
    /// </summary>
    public DateTime? DateTransfertFAR { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de la cloture comptable.
    /// </summary>
    public DateTime? DateCloture { get; set; }

    ///// <summary>
    ///// Obtient ou définit l'année.
    ///// </summary>
    //public int Annee { get; set; }

    ///// <summary>
    ///// Obtient ou définit le mois.
    ///// </summary>
    //public int Mois { get; set; }

    ///// <summary>
    ///// Obtient ou définit le jour de fin des clotures comptables.
    ///// </summary>
    //public int? JourArretSaisie { get; set; }

    ///// <summary>
    ///// Obtient le jour de fin de saisie des clotures comptables.
    ///// </summary>
    //public int? OldJourArretSaisie { get; set; }

    ///// <summary>
    ///// Obtient ou définit le jour de transfert des clotures comptables.
    ///// </summary>
    //public int? JourTransfertFAR { get; set; }

    ///// <summary>
    ///// Obtient le jour de transfert des clotures comptables.
    ///// </summary>
    //public int? OldJourTransfertFAR { get; set; }

    ///// <summary>
    ///// Obtient ou définit le jour de la cloture.
    ///// </summary>
    //public int? JourCloture { get; set; }

    ///// <summary>
    ///// Obtient le jour de la cloture.
    ///// </summary>
    //public int? OldJourCloture { get; set; }

    ///// <summary>
    ///// Obtient ou définit la saisie
    ///// </summary>
    //public DateTime JourSaisie { get; set; }
  }
}