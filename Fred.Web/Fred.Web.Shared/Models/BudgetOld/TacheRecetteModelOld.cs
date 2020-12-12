using Fred.Web.Models.Referential;
using Fred.Web.Models.Referential.Light;

namespace Fred.Web.Models.Budget
{
  /// <summary>
  ///   Représente une ressource insérée dans une tache de niveau 4.
  /// </summary>
  public class TacheRecetteModelOld
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'une liaison tache recette.
    /// </summary>
    public int TacheRecetteId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la tâche à laquelle cette recette appartient
    /// </summary>
    public int TacheId { get; set; }

    /// <summary>
    ///   Obtient ou définit la tâche à laquelle cette recette appartient
    /// </summary>
    public TacheLightModel Tache { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la Devise attachée à cette recette
    /// </summary>
    public int DeviseId { get; set; }

    /// <summary>
    ///   Obtient ou définit la devise attachée à cette recette
    /// </summary>
    public DeviseLightModel Devise { get; set; }

    /// <summary>
    ///   Obtient ou définit la recette
    /// </summary>
    public double? Recette { get; set; }
  }
}