using Fred.Web.Models.Societe;
using Fred.Web.Models.Utilisateur;
using System;

namespace Fred.Web.Models.Journal
{
  public class JournalModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un journal.
    /// </summary>
    public int JournalId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de la societe
    /// </summary>
    public int SocieteId { get; set; }

    /// <summary>
    /// Obtient ou définit la societe
    /// </summary>
    public SocieteModel Societe { get; set; }

    /// <summary>
    /// Obtient ou définit le code du journal
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libelle du journal
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit la date de création
    /// </summary>
    public DateTime DateCreation { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de l'utilisateur ayant fait la creation
    /// </summary>
    public int? AuteurCreationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'utilisateur ayant fait la création
    /// </summary>
    public UtilisateurModel AuteurCreation { get; set; }

    /// <summary>
    /// Obtient ou définit la date de modification
    /// </summary>
    public DateTime? DateModification { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de l'utilisateur ayant fait la modification
    /// </summary>
    public int? AuteurModificationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'utilisateur ayant fait la modification
    /// </summary>
    public UtilisateurModel AuteurModification { get; set; }

    /// <summary>
    /// Obtient ou définit la date de cloture
    /// </summary>
    public DateTime? DateCloture { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de l'utilisateur ayant fait la cloture
    /// </summary>
    public int? AuteurClotureId { get; set; }

    /// <summary>
    /// Obtient ou définit l'utilisateur ayant fait la cloture
    /// </summary>
    public UtilisateurModel AuteurCloture { get; set; }

    /// <summary>
    ///   Obtient ou définit le type de journal
    /// </summary>
    public string TypeJournal { get; set; }

    /// <summary>
    /// Indique si le journal est actif
    /// </summary>
    public bool IsActif
    {
      get { return !DateCloture.HasValue; }
      set
      {
        DateCloture = value ? null : DateCloture.HasValue ? DateCloture : DateTime.UtcNow;
      }
    }
  }
}