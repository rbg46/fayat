using Fred.Web.Models.Referential;
using Fred.Web.Models.Utilisateur;
using System;
using System.Collections.Generic;

namespace Fred.Web.Models.Depense
{
  /// <summary>
  /// Représente le modèle d'une tâche de remplacement
  /// </summary>
  public class RemplacementTacheModel
  {
    /// <summary>
    /// Obtient ou définit l'entité du membre du personnel ayant modifié la dépense
    /// </summary>
    public UtilisateurModel AuteurCreation { get; set; } = null;

    /// <summary>
    /// Obtient ou définit l'ID de la personne ayant modifié la dépense.
    /// </summary>
    public int? AuteurCreationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité du membre du personnel ayant créé la dépense
    /// </summary>
    public UtilisateurModel AuteurRemplacement { get; set; } = null;

    /// <summary>
    ///  Obtient ou définit l'identifiant unique de l'auteur du rapprochement
    /// </summary>
    public int? AuteurRemplacementId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité du membre du personnel ayant supprimer la dépense
    /// </summary>
    public UtilisateurModel AuteurSuppression { get; set; } = null;

    /// <summary>
    /// Obtient ou définit l'ID de la personne ayant supprimer la dépense.
    /// </summary>
    public int? AuteurSuppressionId { get; set; }

    public DateTime? DateComptableRemplacement { get; set; }

    /// <summary>
    /// Obtient ou définit la date comptable.
    /// </summary>    
    public DateTime? DateComptable { get; set; }

    /// <summary>
    /// Obtient ou définit la date de facturation.
    /// </summary>    
    public DateTime? DateFacturation { get; set; }

    /// <summary>
    /// Obtient ou définit la date de modification de la dépense.
    /// </summary>
    public DateTime? DateModification { get; set; }

    /// <summary>
    /// Obtient ou définit la date comptable.
    /// </summary>
    public DateTime? DateRemplacement { get; set; }

    /// <summary>
    /// Obtient ou définit la date de suppression de la dépense.
    /// </summary>
    public DateTime? DateSuppression { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des Dépenses liées (Dep / ODs / Valos)
    /// </summary>
    public ICollection<ExplorateurDepenseModel> DepensesLiees { get; set; }

    /// <summary>
    /// Obtient ou définit l'ID du groupe de remplacement des tâches lié.
    /// </summary>
    public int GroupeRemplacementTacheId { get; set; }

    /// <summary>
    /// Obtient ou définit la période de clôture 
    /// </summary>
    public DateTime Periode { get; set; }

    /// <summary>
    ///  Obtient ou définit l'identifiant unique de l'auteur du rapprochement
    /// </summary>
    public int RangRemplacement { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une dépense.
    /// </summary>
    public int RemplacementTacheId { get; set; }

    /// <summary>
    /// Obtient ou définit l'objet tache
    /// </summary>
    public TacheModel Tache { get; set; }

    /// <summary>
    /// Obtient ou définit l'Id de la tâche d'une dépense.
    /// </summary>
    public int? TacheId { get; set; }

    /// <summary>
    ///   Obtient ou définit si le CI est clôturée pour la période (cf. DateComptableRemplacement)
    /// </summary>    
    public bool IsPeriodeCloturee { get; set; }
  }
}
