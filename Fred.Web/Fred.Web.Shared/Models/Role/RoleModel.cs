using Fred.Web.Models.Referential;
using Fred.Web.Models.Utilisateur;
using System.Collections.Generic;

namespace Fred.Web.Models.Role
{
  /// <summary>
  /// Représente un rôle
  /// </summary>
  public class RoleModel : IReferentialModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un rôle.
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de la societe.
    /// </summary>
    public int SocieteId { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'un rôle
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    ///   Obtient ou définit le code d'un rôle
    /// </summary>   
    public string CodeNomFamilier { get; set; }
    /// <summary>
    /// Obtient ou définit le libellé d'un rôle
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    ///   Obtient ou définit la description d'un role
    /// </summary>   
    public string Description { get; set; }

    /// <summary>
    /// Obtient ou définit le seuil de commande pour le rôle
    /// </summary>
    public string CommandeSeuilDefaut { get; set; }

    /// <summary>
    /// Obtient ou définit le mode lecture ou écriture d'un rôle
    /// </summary>
    public bool ModeLecture { get; set; }

    /// <summary>
    /// Obtient ou définit si le rôle est actif ou non
    /// </summary>
    public bool Actif { get; set; }

    /// <summary>
    /// Obtient ou définit si le rôle est SuperAdmin ou non
    /// </summary>
    public bool SuperAdmin { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant le niveau paie du role
    /// </summary>
    public int NiveauPaie { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant le niveau compta du role
    /// </summary>
    public int NiveauCompta { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des noms modules associés au rôle
    /// </summary>
    public Dictionary<int, string> ModulesAssocies { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des seuils de validation associés
    /// </summary>
    public ICollection<SeuilValidationModel> SeuilsValidation { get; set; } = null;

    /// <summary>
    /// Obtient une concaténation du code et du libellé
    /// </summary>
    public string CodeLibelle => this.CodeNomFamilier + " - " + this.Libelle;

    public List<DeviseModel> SeuilDevises { get; set; }

    /// <summary>
    /// Liste des regroupement par organisation avec les affectations associées.
    /// </summary>
    /// <value>
    /// Les affectations regroupées par organisation.
    /// </value>
    public List<AffectationGroupByOrganisationModel> AffectationsByOrganisation { get; set; }

    /// <summary>
    /// Obtient le libellé référentiel d'un personnel.
    /// </summary>
    public string LibelleRef => this.Libelle;

    /// <summary>
    /// Obtient le code référentiel d'un personnel.
    /// </summary>
    public string CodeRef => this.CodeNomFamilier;

    /// <summary>
    /// Obtient l'id du référentiel
    /// </summary>
    public string IdRef => this.RoleId.ToString();
  }
}