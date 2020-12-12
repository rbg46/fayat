using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Dtos.Mobile
{
  /// <summary>
  /// Dto d'une tâche.
  /// </summary>
  public class TacheDto : DtoBase
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une tache.
    /// </summary>
    public int TacheId { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'une tache.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'une tache.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si une tâche est la tâche par défaut.
    /// </summary>
    public bool TacheParDefaut { get; set; }
  }
}