using Fred.Web.Models.Referential;
using Fred.Web.Models.Referential.Light;
using Fred.Web.Models.ReferentielFixe;
using Fred.Web.Models.ReferentielFixe.Light;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Budget
{
  /// <summary>
  ///   Représente une ressource insérée dans une tache de niveau 4.
  /// </summary>
  public class RessourceTacheModel
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'un budget.
    /// </summary>
    public int RessourceTacheId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la tâche (de niveau T4) à laquelle cette ressource appartient
    ///   La tâche est de niveau T4
    /// </summary>
    public int TacheId { get; set; }

    /// <summary>
    ///   Obtient ou définit la tâche (de niveau T4) à laquelle cette ressource appartient
    /// </summary>
    public TacheLightModel Tache { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la ressource de référence attachée à cette ressource
    ///   La tâche est de niveau T4
    /// </summary>
    public int RessourceId { get; set; }

    /// <summary>
    ///   Obtient ou définit la ressource de référence attachée à cette ressource
    /// </summary>
    public RessourceLightModel Ressource { get; set; }

    /// <summary>
    ///   Obtient ou définit la quantité de base
    /// </summary>
    public double? QuantiteBase { get; set; }

    /// <summary>
    ///   Obtient ou définit la quantité
    /// </summary>
    public double? Quantite { get; set; }

    /// <summary>
    /// Recopie du prix unitaire de la ressource si personnalisé
    /// </summary>
    public double? PrixUnitaire { get; set; }

    /// <summary>
    ///   Obtient ou définit la formule de calcul du montant
    /// </summary>
    public string Formule { get; set; }

    /// <summary>
    ///   Obtient ou définit la liste des ressourceTacheDevises
    /// </summary>
    public ICollection<RessourceTacheDeviseModel> RessourceTacheDevises { get; set; }
  }
}