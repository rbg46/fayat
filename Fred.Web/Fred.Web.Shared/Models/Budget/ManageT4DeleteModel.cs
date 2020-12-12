namespace Fred.Web.Shared.Models.Budget
{
  /// <summary>
  /// Modèles de suppression d'une tâche de niveau 4.
  /// </summary>
  public class ManageT4Delete
  {
    #region Model

    /// <summary>
    /// Modèle de suppression d'une tâche de niveau 4.
    /// </summary>
    public class Model
    {
      /// <summary>
      /// Identifiant de la tache.
      /// </summary>
      public int TacheId { get; set; }

      /// <summary>
      /// Identifiant du CI.
      /// </summary>
      public int CiId { get; set; }
    }

    #endregion
    #region ResultModel

    /// <summary>
    /// Représente le résultat de la suppression d'une tâche de niveau 4.
    /// </summary>
    public class ResultModel : ErreurResultModel
    { }

    #endregion
  }
}
