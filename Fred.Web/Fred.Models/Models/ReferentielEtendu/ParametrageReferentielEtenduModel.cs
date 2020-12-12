using Fred.Web.Models.Organisation;
using Fred.Web.Models.Referential;
using System;
using System.Collections.Generic;

namespace Fred.Web.Models.ReferentielEtendu
{
  /// <summary>
  /// Représente un paramétrage de référentiel étendu
  /// </summary>
  public class ParametrageReferentielEtenduModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique de l'entité.
    /// </summary>
    public int ParametrageReferentielEtenduId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une organisation.
    /// </summary>
    public int OrganisationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité organisation.
    /// </summary>
    public OrganisationModel Organisation { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une devise.
    /// </summary>
    public int DeviseId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité devise.
    /// </summary>
    public DeviseModel Devise { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un référentiel étendu.
    /// </summary>
    public int ReferentielEtenduId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité référentiel étendu.
    /// </summary>
    public ReferentielEtenduModel ReferentielEtendu { get; set; }

   
  
    public int UniteId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité Unité.
    /// </summary>   
    public UniteModel Unite { get; set; }

    /// <summary>
    /// Obtient ou définit le montant
    /// </summary>
    public decimal? Montant { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des Montants et types des organisations parent
    /// </summary>
    public List<ParametrageParentModel> Parents
    {
      get
      {
        List<ParametrageParentModel> parents = new List<ParametrageParentModel>();
        if (ParametragesParent != null)
        {
          foreach (ParametrageReferentielEtenduModel param in ParametragesParent)
          {
            if (param != null)
            {
              if (param.Organisation != null)
              {
                ParametrageParentModel parent = new ParametrageParentModel() { TypeOrganisationId = param.Organisation.TypeOrganisationId, TypeOrganisation = param.Organisation.TypeOrganisation, Montant = param.Montant };
                parents.Add(parent);
              }
            }
            else
              parents.Add(null);
          }
        }
        return parents;
      }
    }

    /// <summary>
    /// Obtient ou définit la liste parametrage parents
    /// </summary>
    public List<ParametrageReferentielEtenduModel> ParametragesParent { get; set; }
    public int Order { get; internal set; }

    /// <summary>
    ///   Obtient ou définit la date de création
    /// </summary>
      public DateTime? DateCreation { get; set; }

    /// <summary>
    ///   Obtient ou définit l'id de l'auteur de la création
    /// </summary>
    public int? AuteurCreationId { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de modification
    /// </summary>
    public DateTime? DateModification { get; set; }

    /// <summary>
    ///   Obtient ou définit l'id de l'auteur de la modification
    /// </summary>
    public int? AuteurModificationId { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de suppression
    /// </summary>
    public DateTime? DateSuppression { get; set; }

    /// <summary>
    ///   Obtient ou définit l'id de l'auteur de la suppression
    /// </summary>
    public int? AuteurSuppressionId { get; set; }
  }
}