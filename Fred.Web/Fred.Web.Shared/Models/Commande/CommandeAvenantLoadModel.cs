using System;

namespace Fred.Web.Shared.Models.Commande
{
  public class CommandeAvenantLoad
  {
    // Front -> back
    #region Model

    /// <summary>
    /// Modèle de chargement d'un avenant de commande.
    /// </summary>
    public class Model
    {
      /// <summary>
      /// Identifiant de la commande.
      /// </summary>
      public int CommandeId { get; set; }
    }

    #endregion

    // Back -> front
    #region LigneModel

    /// <summary>
    /// Modèle de chargement d'une ligne d'un avenant de commande.
    /// </summary>
    public class LigneModel
    {
      /// <summary>
      /// Indique s'il s'agit d'une diminution.
      /// </summary>
      public bool IsDiminution { get; set; }

      /// <summary>
      /// L'avenant concerné.
      /// </summary>
      public AvenantModel Avenant { get; set; }
    }

    #endregion
    #region AvenantModel

    /// <summary>
    /// Modèle de chargement d'un avenant.
    /// </summary>
    public class AvenantModel
    {
      /// <summary>
      /// Le numéro de l'avenant.
      /// </summary>
      public int NumeroAvenant { get; set; }

      /// <summary>
      /// La date de validation de l'avenant.
      /// </summary>
      public DateTime? DateValidation { get; set; }

      /// <summary>
      /// L'idenfiant du job hangfire.
      /// </summary>
      public string HangfireJobId { get; set; }
    }

    #endregion
  }
}
