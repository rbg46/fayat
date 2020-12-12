using Fred.Web.Models.CI;
using Fred.Web.Models.Groupe;
using Fred.Web.Models.Holding;
using Fred.Web.Models.OrganisationGenerique;
using Fred.Web.Models.Pole;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Role;
using Fred.Web.Models.Societe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Fred.Web.Models.Organisation
{
  public class OrganisationModel : IReferentialModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une organisation.
    /// </summary>
    public int OrganisationId { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'une organisation.
    /// </summary>
    public CIModel CI { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'une organisation.
    /// </summary>
    public EtablissementComptableModel Etablissement { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'une societe.
    /// </summary>
    public SocieteModel Societe { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'une organisation générique.
    /// </summary>
    public OrganisationGenriqueModel OrganisationGenerique { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'un groupe.
    /// </summary>
    public GroupeModel Groupe { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'un pole.
    /// </summary>
    public PoleModel Pole { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'une holding.
    /// </summary>
    public HoldingModel Holding { get; set; }

    /// <summary>
    /// Obtient ou définit la liaison entre organisation et d'un groupe.
    /// </summary>
    public int TypeOrganisationId { get; set; }

    /// <summary>
    /// Obtient ou définit le type de l'organisation.
    /// </summary>
    public virtual TypeOrganisationModel TypeOrganisation { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une organistion d'un niveau hiérarchique supérieure.
    /// </summary>
    public int? PereId { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'une organisation.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'une organisation.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient une concaténation du code et du libellé
    /// </summary>
    public string CodeLibelle { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des organisations enfants
    /// </summary>
    public virtual ICollection<OrganisationModel> OrganisationsEnfants { get; set; }

    ///// <summary>
    ///// Obtient ou définit l'orgaisation parent
    ///// </summary>
    //public virtual OrganisationModel OrganisationPere { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des associations rôle et organisations que possèdent l'utilisateur
    /// </summary>
    public virtual ICollection<AffectationSeuilOrgaModel> AffectationsSeuilRoleOrga { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des rôles et seuils que possèdent l'organisation
    /// </summary>
    public virtual IEnumerable<RoleModel> ListRoleOrganisation
    {
      get
      {
        List<RoleModel> roles = new List<RoleModel> { };
        if (this.AffectationsSeuilRoleOrga != null)
        {
          foreach (RoleModel role in this.AffectationsSeuilRoleOrga.Where(o => o.OrganisationId.Equals(this.OrganisationId)).Select(a => a.Role).Distinct().ToList())
          {
            role.SeuilDevises = new List<DeviseModel>();
            foreach (AffectationSeuilOrgaModel affectation in this.AffectationsSeuilRoleOrga.Where(a => a.RoleId.Equals(role.RoleId) && a.OrganisationId.Equals(this.OrganisationId)).ToList())
            {
              DeviseModel devise = CloneDevise(affectation.Devise);
              devise.Seuil = affectation.Seuil;
              role.SeuilDevises.Add(devise);
            }
            roles.Add(role);
          }
        }
        
        return roles;
      }
    }

    /// <summary>
    /// Obtient le libellé référentiel d'un personnel.
    /// </summary>
    public string LibelleRef => this.Libelle;

    /// <summary>
    /// Obtient le code référentiel d'un personnel.
    /// </summary>
    public string CodeRef
    {
      get
      {
        if (this.TypeOrganisation != null)
        {
          return this.TypeOrganisation.Libelle + " - " + this.Code;
        }
        else
        {
          return this.Code;
        }
      }

    }

    /// <summary>
    /// Obtient l'id du référentiel
    /// </summary>
    public string IdRef => this.OrganisationId.ToString();

    /// <summary>
    /// Clone un type organisation
    /// </summary>
    /// <param name="typeOrganisationModel">type organisation à cloner.</param>
    /// <returns>Le type organisation cloné, sinon nulle.</returns>
    private DeviseModel CloneDevise(DeviseModel deviseModel)
    {
      DeviseModel devise = new DeviseModel
      {
        AuteurCreation = deviseModel.AuteurCreation,
        AuteurModification = deviseModel.AuteurModification,
        AuteurSuppression = deviseModel.AuteurSuppression,
        CodeHtml = deviseModel.CodeHtml,
        CodePaysIso = deviseModel.CodePaysIso,
        DateCreation = deviseModel.DateCreation,
        DateModification = deviseModel.DateModification,
        DateSuppression = deviseModel.DateSuppression,
        DeviseId = deviseModel.DeviseId,
        IsDeleted = deviseModel.IsDeleted,
        IsoCode = deviseModel.IsoCode,
        IsoNombre = deviseModel.IsoNombre,
        Libelle = deviseModel.Libelle,
        Reference = deviseModel.Reference,
        Seuil = deviseModel.Seuil,
        Symbole = deviseModel.Symbole
      };
      return devise;
    }
  }
}