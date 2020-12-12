using System;
using Fred.Web.Models.Personnel;
using Fred.Web.Models.ExternalDirectory;

namespace Fred.Web.Models.Utilisateur
{
  public class UtilisateurSansPersonnelModel
  {

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un utilisateur
    /// </summary>

    public int? UtilisateurId { get; set; }

    /// <summary>
    /// Obtient ou définit le login de l'utilisateur 
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// Obtient ou définit la date de dernière connexion de l'utilisateur
    /// </summary>

    public DateTime? DateDerniereConnexion { get; set; }

    /// <summary>
    /// Obtient ou définit le statut du personnel comme étant utilisateur dans le système (valeur 0 > Le personnel n'a pas les droits d'être utilisateur)
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Obtient ou définit le ...
    /// </summary>
    public bool IsActived { get { return !IsDeleted; } set { IsDeleted = !value; } }

    /// <summary>
    /// Obtient ou définit la date de création  du compte de l'utilisateur
    /// </summary>
    public DateTime? DateCreation { get; set; }

    /// <summary>
    /// Obtient ou définit la date de mise à jour  du compte de l'utilisateur
    /// </summary>
    public DateTime? DateModification { get; set; }

    /*
    /// <summary>
    /// Obtient ou définit l'auteur de la création ou modification du compte
    /// </summary>
    public int? UtilisateurIdAuteur { get; set; }
    */

    /// <summary>
    /// Obtient ou définit l'auteur de la création du compte
    /// </summary>
    public int? UtilisateurIdCreation { get; set; }

    /// <summary>
    /// Obtient ou définit l'auteur de la modification du compte
    /// </summary>
    public int? UtilisateurIdModification { get; set; }

    /// <summary>
    /// Obtient ou définit l'auteur de la suppression du compte
    /// </summary>
    public int? UtilisateurIdSuppression { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant personnel de l'utilisateur
    /// </summary>
    public int PersonnelId { get; set; }

    /// <summary>
    /// Obtient ou définit Objet Personnel pour navigation si nécessaire
    /// </summary>
    public PersonnelModel Personnel { get; set; } = null;

    public int? FayatAccessDirectoryId { get; set; }

    /// <summary>
    /// Obtient ou définit Segment Authentification pour un personnel Externe étant utilisateur
    /// </summary>
    public ExternalDirectoryModel ExternalDirectory { get; set; } = null;

    /// <summary>
    /// Obtient ou définit une valeur indiquant si l'utilisateur est super admin
    /// </summary>
    public bool SuperAdmin { get; set; }

    /// <summary>
    /// Obtient ou définit le folio (trigramme) de l'utilisateur
    public string Folio { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si le personnel peut saisir des commandes manuelles
    /// </summary>
    public bool CommandeManuelleAllowed { get; set; }
  }
}







