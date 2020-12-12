using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Commande
{
  public class CommandeReturnToSap
  {
    #region ResultModel

    /// <summary>
    /// Modèle le résultat de la demande.
    /// </summary>
    public class ResultModel
    {
      /// <summary>
      /// Indique si la commande a été traitée.
      /// </summary>
      public bool CommandeTraitee { get; set; }

      /// <summary>
      /// L'identifiant du job Hangfire pour la commande.
      /// </summary>
      public string CommandeHangfireJobId { get; set; }

      /// <summary>
      /// Les avenants.
      /// </summary>
      public List<AvenantModel> Avenants { get; private set; } = new List<AvenantModel>();
    }

    #endregion
    #region AvenantModel

    /// <summary>
    /// Modèle le résultat des avenants.
    /// </summary>
    public class AvenantModel
    {
      /// <summary>
      /// Le numéro d'avenant.
      /// </summary>
      public int NumeroAvenant { get; private set; }

      /// <summary>
      /// L'identifiant du job Hangfire.
      /// </summary>
      public string HangfireJobId { get; private set; }

      /// <summary>
      /// Constructeur.
      /// </summary>
      /// <param name="numeroAvenant">Le numéro d'avenant.</param>
      /// <param name="hangfireJobId">L'identifiant du job Hangfire.</param>
      public AvenantModel(int numeroAvenant, string hangfireJobId)
      {
        NumeroAvenant = numeroAvenant;
        HangfireJobId = hangfireJobId;
      }
    }

    #endregion
  }
}
