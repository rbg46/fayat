using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fred.Business.Utilisateur;
using Fred.Web.Models.Personnel;
using Fred.Web.Models.ExternalDirectory;
using Fred.Web.Models.Utilisateur;

namespace Fred.Web.Models.Utilisateur
{
  public class UtilisateurModel
  {

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un utilisateur
    /// </summary>
    public int UtilisateurId { get; set; }   

    /// <summary>
    /// Obtient ou définit la date de création
    /// </summary>
    public DateTime? DateCreation { get; set; }

    /// <summary>
    /// Obtient ou définit la date de modification
    /// </summary>
    /// 
    public DateTime? DateModification { get; set; }

    /// <summary>
    /// Obtient ou définit la date de suppression
    /// </summary>
    /// 
    public DateTime? DateSuppression { get; set; }

    /// <summary>
    /// Obtient ou définit l'auteur de la création
    /// </summary>
    /// 
    public UtilisateurModel AuteurCreation { get; set; } = null;

    /// <summary>
    /// Obtient ou définit l'auteur de la modification
    /// </summary>
    public UtilisateurModel AuteurModification { get; set; } = null;

    /// <summary>
    /// Obtient ou définit l'auteur de la suppression
    /// </summary>
    public UtilisateurModel AuteurSuppression { get; set; } = null;

    /// <summary>
    /// Obtient ou définit le login
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité du membre du utilisateur ayant valider la commande
    /// </summary>
    public PersonnelModel Personnel { get; set; } = null;

    /// <summary>
    /// Obtient ou définit l'identifiant personnel de l'utilisateur
    /// </summary>
    public int? PersonnelId { get; set; }

    /// <summary>
    /// Obtient ou définit la date de dernière connexion de l'utilisateur
    /// </summary>
    public DateTime? DateDerniereConnexion { get; set; }

    /// <summary>
    /// Obtient ou définit la relation avec la table stockant le mot de passe pour le personnel externe
    /// </summary>
    public int? FayatAccessDirectoryId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité du membre du utilisateur ayant valider la commande
    /// </summary>
    public ExternalDirectoryModel ExternalDirectory { get; set; } = null;

    /// <summary>
    /// Obtient ou définit une valeur indiquant si le statut du Personnel comme étant actif sur le profil Utilisateur
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Obtient ou définit le ...
    /// </summary>
    public bool IsActived { get { return !IsDeleted; } set { IsDeleted = !value; } }

    /// <summary>
    /// Obtient ou définit l'identifiant utilisateur de l'auteur de la Création du compte de l'utilisateur
    /// </summary>
    public int? UtilisateurIdCreation { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant utilisateur de l'auteur de la modification du compte de l'utilisateur
    /// </summary>
    public int? UtilisateurIdModification { get; set; }

    
    /// <summary>
    /// Obtient ou définit l'identifiant utilisateur de l'auteur de la suppression du compte de l'utilisateur
    /// </summary>
    public int? UtilisateurIdSupression { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des associations utilisateur, rôle et organisations que possèdent l'utilisateur
    /// </summary>
    public ICollection<AffectationSeuilUtilisateurModel> AffectationsRole { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si l'utilisateur est super admin
    /// </summary>
    public bool SuperAdmin { get; set; }

    /// <summary>
    /// Obtient ou définit le folio (trigramme) de l'utilisateur
    public string Folio { get; set; }

    /// <summary>
    /// Obtient une concaténation du nom et du prénom du membre du personnel
    /// </summary>    
    public string NomPrenom
    {
      get
      {
        return this.Personnel != null ? this.Personnel.NomPrenom : string.Empty;
      }
    }

    /// <summary>
    /// Obtient une concaténation du prénom et du nom du membre du personnel
    /// </summary>    
    public string PrenomNom
    {
      get
      {
        return this.Personnel != null ? this.Personnel.PrenomNom : string.Empty;
      }
    }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si le personnel peut saisir des commandes manuelles
    /// </summary>
    public bool CommandeManuelleAllowed { get; set; }

  }
}

