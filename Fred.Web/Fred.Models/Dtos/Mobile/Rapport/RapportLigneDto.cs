using System;
using System.Collections.Generic;
using Fred.Web.Models.CodeAbsence;
using Fred.Web.Models.Personnel;
using Fred.Web.Models.Rapport;
using Fred.Web.Models.Referential;

namespace Fred.Web.Dtos.Mobile.Rapport
{
  /// <summary>
  /// Dto RapportLigne
  /// </summary>
  public class RapportLigneDto
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique de la ligne du rapport
    /// </summary>
    public int RapportLigneId { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si la ligne d'un rapport soit sélectionné de l'UI
    /// </summary>
    public bool IsChecked { get; set; }

    /// <summary>
    /// Obtient ou définit l'id du rapport auquel est rattachée la ligne de rapport
    /// </summary>
    public int RapportId { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de l'entité personnel
    /// </summary>
    public int? PersonnelId { get; set; }

    /// <summary>
    /// Obtient ou définit l'heure Totale
    /// </summary>
    public double HeureNormale { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de l'entité CodeMajoration
    /// </summary>
    public int? CodeMajorationId { get; set; }

    /// <summary>
    /// Obtient ou définit le l'heure majorée
    /// </summary>
    public double HeureMajoration { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de l'entité code absence
    /// </summary>
    public int? CodeAbsenceId { get; set; }

    /// <summary>
    /// Obtient ou définit le l'heure de l'absence
    /// </summary>
    public double HeureAbsence { get; set; }

    /// <summary>
    /// Obtient ou définit le n° absence intemperie
    /// </summary>
    public int? NumSemaineIntemperieAbsence { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de l'entité code déplacement
    /// </summary>
    public int? CodeDeplacementId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité DeplacementZone
    /// </summary>
    public string DeplacementZone { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si l'entité IVDeplacement de la ligne de rapport
    /// </summary>
    public bool DeplacementIV { get; set; }

    /// <summary>
    /// Obtient ou définit le temps de Marche matériel
    /// </summary>
    public double MaterielMarche { get; set; }

    /// <summary>
    /// Obtient ou définit le temps d'Arret matériel
    /// </summary>
    public double MaterielArret { get; set; }

    /// <summary>
    /// Obtient ou définit le temps de Panne matériel
    /// </summary>
    public double MaterielPanne { get; set; }
    
    /// <summary>
    /// Obtient ou définit le numéro de la semaine en intempérie
    /// </summary>
    public double MaterielIntemperie { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de l'entité matériel
    /// </summary>
    public int? MaterielId { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des primes
    /// </summary>
    public ICollection<RapportLignePrimeDto> ListPrimes { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des taches
    /// </summary>
    public ICollection<RapportLigneTacheDto> ListTaches { get; set; }
  }
}