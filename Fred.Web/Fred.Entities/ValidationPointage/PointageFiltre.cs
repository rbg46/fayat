using System.Collections.Generic;

namespace Fred.Entities.ValidationPointage
{
  /// <summary>
  ///   Classe filtre pour un lot de pointage
  /// </summary>
  public class PointageFiltre
  {
    #region CRITERES
    /// <summary>
    ///   Obtient ou définit l'identifiant de la société
    /// </summary>
    public int SocieteId { get; set; }

    /// <summary>
    ///   Obtient ou définit une liste d'identifiant d'établissement de paie
    /// </summary>
    public IEnumerable<int> EtablissementPaieIdList { get; set; }

    /// <summary>
    ///   Obtient ou définit de statut de personnel
    /// </summary>
    public IEnumerable<string> StatutPersonnelList { get; set; }

    /// <summary>
    ///   Obtient ou définit le matricule du personnel
    /// </summary>
    public string Matricule { get; set; }

    /// <summary>
    ///   Obtient ou définit s'il faut mettre à jour les absences
    /// </summary>
    public bool UpdateAbsence { get; set; }

    #endregion

    #region SCOPE DE RECHERCHE

    /// <summary>
    ///   Obtient ou définit une valeur indiquant la prise en compte de la société dans le filtrage
    /// </summary>
    public bool TakeSocieteId { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant la prise en compte du filtrage sur les établissements de paie
    /// </summary>
    public bool TakeEtablissementPaieIdList { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant la prise en compte du matricule dans le filtrage
    /// </summary>
    public bool TakeMatricule { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant la prise en compte de la mise à jour des absences dans le filtrage
    /// </summary>
    public bool TakeUpdateAbsence { get; set; }

    #endregion
  }
}
