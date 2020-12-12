using System;

namespace Fred.Business.DepenseGlobale
{
  /// <summary>
  ///   Classe filtre des dépenses globales
  /// </summary>
  public class DepenseGlobaleFiltre
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant du CI
    /// </summary>
    public int CiId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la tâche 
    /// </summary>
    public int? TacheId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la ressource
    /// </summary>
    public int? RessourceId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la devise
    /// </summary>
    public int? DeviseId { get; set; }

    /// <summary>
    ///   Obtient ou définit la période de début
    /// </summary>
    public DateTime? PeriodeDebut { get; set; }

    /// <summary>
    ///   Obtient ou définit la période de fin
    /// </summary>
    public DateTime? PeriodeFin { get; set; }

    /// <summary>
    ///   Obtient ou définit si on inclut les FAR ou non
    /// </summary>
    public bool IncludeFar { get; set; } = false;

    /// <summary>
    ///   Obtient ou définit si on remplace la tâche par la dernière remplacée ou non
    /// </summary>
    public bool LastReplacedTask { get; set; } = false;
  }
}
