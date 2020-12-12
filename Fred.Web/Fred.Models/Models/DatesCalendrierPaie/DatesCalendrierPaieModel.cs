using Fred.Web.Models.Organisation;
using Fred.Web.Models.Pole;
using Fred.Web.Models.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

    ///// <summary>
    ///// Obtient ou définit l'année.
    ///// </summary>
    //public int Annee { get; set; }

    ///// <summary>
    ///// Obtient ou définit le mois.
    ///// </summary>
    //public int Mois { get; set; }

    ///// <summary>
    ///// Obtient ou définit le jour de fin de saisie des pointages.
    ///// </summary>
    //public int? JourFinPointages { get; set; }

    ///// <summary>
    ///// Obtient le jour de fin de saisie des pointages.
    ///// </summary>
    //public int? OldJourFinPointages { get; set; }

    ///// <summary>
    ///// Obtient ou définit le jour de transfert des pointages pour la paie.
    ///// </summary>
    //public int? JourTransfertPointages { get; set; }

    ///// <summary>
    ///// Obtient le jour de transfert des pointages pour la paie.
    ///// </summary>
    //public int? OldJourTransfertPointages { get; set; }

    ///// <summary>
    ///// Obtient ou définit le jour saisi dans le mois
    ///// </summary>
    //public DateTime JourSaisi { get; set; }
  }
}