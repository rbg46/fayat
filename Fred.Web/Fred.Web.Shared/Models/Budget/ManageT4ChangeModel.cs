namespace Fred.Web.Shared.Models.Budget
{
  /// <summary>
  /// Modèles de changement d'une tâche de niveau 4.
  /// </summary>
  public class ManageT4Change
  {
    #region Model

    /// <summary>
    /// Modèle de changement d'une tâche de niveau 4.
    /// </summary>
    public class Model
    {
      /// <summary>
      /// Identifiant du CI.
      /// </summary>
      public int CiId { get; set; }

      /// <summary>
      /// Identifiant de la tâche.
      /// </summary>
      public int TacheId { get; set; }

      /// <summary>
      /// Code de la tâche.
      /// </summary>
      public string Code { get; set; }

      /// <summary>
      /// Libellé de la tâche.
      /// </summary>
      public string Libelle { get; set; }
    }

    #endregion
    #region ResultModel

    /// <summary>
    /// Représente le résultat du changement d'une tâche de niveau 4.
    /// </summary>
    public class ResultModel : ErreurResultModel
    {
      /// <summary>
      /// La tâche de niveau 4 créée ou null en cas d'erreur.
      /// </summary>
      public TacheResultModel Tache { get; set; }
    }

    #endregion
  }
}
