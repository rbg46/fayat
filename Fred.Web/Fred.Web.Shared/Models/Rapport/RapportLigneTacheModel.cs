using Fred.Web.Models.Referential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Rapport
{
  public class RapportLigneTacheModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique de la ligne prime du rapport
    /// </summary>
    public int RapportLigneTacheId { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de la ligne de rapport de rattachement
    /// </summary>
    public int RapportLigneId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité Rapport
    /// </summary>
    public RapportLigneModel RapportLigne { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de la tâche
    /// </summary>
    public int TacheId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité Tache
    /// </summary>
    public TacheModel Tache { get; set; }

    /// <summary>
    /// Obtient ou définit l'heure de la tâche
    /// </summary>
    public double? HeureTache { get; set; }

    /// <summary>
    /// Obtient ou définit le fait que la ligne soit en création
    /// </summary>
    public bool IsCreated { get; set; } = false;

    /// <summary>
    /// Obtient ou définit le fait que la ligne soit en modification
    /// </summary>
    public bool IsUpdated { get; set; } = false;

    /// <summary>
    /// Obtient ou définit le fait que la ligne soit à supprimer
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    ///   Obtient ou définit le commentaire de la tache
    /// </summary>
    public string Commentaire { get; set; } = string.Empty;
  }
}