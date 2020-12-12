using Fred.Web.Models.Personnel;
using System;

namespace Fred.Web.Shared.Models
{
  /// <summary>
  ///   Classe ControlePointageErreurModel
  /// </summary>
  public class ControlePointageErreurModel
  {
    private DateTime? dateRapport;

    /// <summary>
    ///   Obtient ou définit l'identifiant de l'erreur de contrôle
    /// </summary>
    public int ControlePointageErreurId { get; set; }

    /// <summary>
    ///   Obtient ou définit le message d'erreur
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du Controle Chantier
    /// </summary>
    public int ControlePointageId { get; set; }

    /// <summary>
    ///   Obtient ou définit le controle Chantier
    /// </summary>
    public ControlePointageModel ControlePointage { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du personnel concerné par l'erreur
    /// </summary>
    public int PersonnelId { get; set; }

    /// <summary>
    ///   Obtient ou définit le personnel concerné par l'erreur
    /// </summary>
    public PersonnelModel Personnel { get; set; }

    /// <summary>
    ///   Obtient ou définit le jour du rapport
    /// </summary>
    public DateTime? DateRapport
    {
      get { return (dateRapport.HasValue) ? DateTime.SpecifyKind(dateRapport.Value, DateTimeKind.Utc) : default(DateTime?); }
      set { dateRapport = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
    }

  }
}