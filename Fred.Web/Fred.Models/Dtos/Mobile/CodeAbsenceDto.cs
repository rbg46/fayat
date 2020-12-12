using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Dtos.Mobile
{
  /// <summary>
  /// DTO CodeAbsence
  /// </summary>
  /// <seealso cref="Fred.Web.Dtos.Mobile.DtoBase" />
  public class CodeAbsenceDto : DtoBase
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un Code d'absence.
    /// </summary>
    public int CodeAbsenceId { get; set; }

    /// <summary>
    /// Obtient ou définit l'Id de la société 
    /// </summary>
    public int SocieteId { get; set; }

    /// <summary>
    /// Obtient ou définit le code du CodeAbsence
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si Intemperie
    /// </summary>
    public bool Intemperie { get; set; }

    /// <summary>
    /// Obtient ou définit le TauxDecote
    /// </summary>
    public double TauxDecote { get; set; }

    /// <summary>
    /// Obtient ou définit le NBHeuresDefautETAM
    /// </summary>
    public double NBHeuresDefautETAM { get; set; }

    /// <summary>
    /// Obtient ou définit le NBHeuresMinETAM
    /// </summary>
    public double NBHeuresMinETAM { get; set; }

    /// <summary>
    /// Obtient ou définit le NBHeuresMaxETAM
    /// </summary>
    public double NBHeuresMaxETAM { get; set; }

    /// <summary>
    /// Obtient ou définit le NBHeuresDefautCO
    /// </summary>
    public double NBHeuresDefautCO { get; set; }

    /// <summary>
    /// Obtient ou définit le NBHeuresMinCO
    /// </summary>
    public double NBHeuresMinCO { get; set; }

    /// <summary>
    /// Obtient ou définit le NBHeuresMaxCO
    /// </summary>
    public double NBHeuresMaxCO { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si Actif
    /// </summary>
    public bool Actif { get; set; }
  }
}