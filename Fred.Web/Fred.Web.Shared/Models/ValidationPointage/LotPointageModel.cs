using Fred.Web.Models.Utilisateur;
using System;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models
{
  /// <summary>
  ///   Classe LotPointageModel
  /// </summary>  
  public class LotPointageModel
  {
    private DateTime periode;

    /// <summary>
    ///   Obtient ou définit l'identifiant du lot de Pointage
    /// </summary>
    public int LotPointageId { get; set; }

    /// <summary>
    ///   Obtient ou définit la date comptable
    /// </summary>
    public DateTime Periode
    {
      get { return DateTime.SpecifyKind(periode, DateTimeKind.Utc); }
      set { periode = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
    }

    /// <summary>
    ///   Obtient ou définit la date du Visa
    /// </summary>
    public DateTime? DateVisa { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de l'utilisateur ayant visé le lot de pointage
    /// </summary>
    public int? AuteurVisaId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'auteur du Visa
    /// </summary>
    public UtilisateurModel AuteurVisa { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de l'utilisateur ayant créé le lot de pointage
    /// </summary>
    public int? AuteurCreationId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'auteur de la création du lot de pointage
    /// </summary>
    public UtilisateurModel AuteurCreation { get; set; }

    /// <summary>
    ///   Obtient ou définit le dernier contrôve chantier
    /// </summary>
    public ControlePointageModel ControleChantier { get; set; }

    /// <summary>
    ///   Obtient ou définit le dernier contrôle vrac
    /// </summary>
    public ControlePointageModel ControleVrac { get; set; }

    /// <summary>
    ///   Obtient ou définit le nombre de pointages total du lot
    /// </summary>
    public int NombrePointages { get; set; }

    /// <summary>
    ///   Obtient ou définit la liste des contrôles de pointages (Contrôle Chantier, Contrôle Vrac, Remontée Vrac)
    /// </summary>
    public virtual ICollection<ControlePointageModel> ControlePointages { get; set; }

  }
}