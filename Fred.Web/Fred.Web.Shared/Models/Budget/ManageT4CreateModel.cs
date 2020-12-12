namespace Fred.Web.Shared.Models.Budget
{
  /// <summary>
  /// Modèles de création d'une tâche de niveau 4.
  /// </summary>
  public class ManageT4Create
  {
    #region Model

    /// <summary>
    /// Modèle de création d'une tâche de niveau 4.
    /// </summary>
    public class Model
    {
      /// <summary>
      /// Identifiant du CI.
      /// </summary>
      public int CiId { get; set; }

      /// <summary>
      /// Identifiant de la tâche parente de niveau 3.
      /// </summary>
      public int Tache3Id { get; set; }

      /// <summary>
      /// Code de la nouvelle tâche.
      /// </summary>
      public string Code { get; set; }

      /// <summary>
      /// Libellé de la nouvelle tâche.
      /// </summary>
      public string Libelle { get; set; }
    }

    #endregion
    #region ResultModel

    /// <summary>
    /// Représente le résultat de la création d'une tâche de niveau 4.
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
