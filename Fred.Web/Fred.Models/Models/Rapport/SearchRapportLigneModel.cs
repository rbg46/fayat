using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Rapport
{
  public class SearchRapportLigneModel
  {
    /// <summary>
    /// Obtient ou définit une valeur indiquant si la date minimale pour laquelle récupérer les pointages
    /// </summary>
    public DateTime DatePointageMin { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si la date maximale pour laquelle récupérer les pointages
    /// </summary>
    public DateTime DatePointageMax { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si le personnel pour lequel récupérer les pointages
    /// </summary>
    public int PersonnelId { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si la récupération des pointages réels
    /// </summary>
    public bool IsReel { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si la récupération des pointages prévisionnels
    /// </summary>
    public bool IsPrevisionnel { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si l'ordre de tri sur les dates
    /// </summary>
    public bool? DatePointageAsc { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si l'ordre de tri sur le type de pointages
    /// </summary>
    public bool? ReelFirst { get; set; }
  }
}