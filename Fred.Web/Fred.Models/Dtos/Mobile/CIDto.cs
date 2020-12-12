using System;
using System.ComponentModel.DataAnnotations;
using Fred.Web.Models.Organisation;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Societe;

namespace Fred.Web.Dtos.Mobile
{
  /// <summary>
  /// Représente une ci
  /// </summary>
  public class CIDto : DtoBase
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une ci.
    /// </summary>
    public int CiId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de l'organisation du ci
    /// </summary>
    public int? OrganisationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de l'établissement comptable du ci.
    /// </summary>
    public int? EtablissementComptableId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de la société de l'ci
    /// </summary>
    public int? SocieteId { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'une ci.
    /// </summary>
    [Required]
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'une ci.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si une ci est une SEP.
    /// </summary>
    [Required]
    public bool Sep { get; set; }

    /// <summary>
    /// Obtient ou définit la date d'ouverture du CI
    /// </summary>
    public DateTime? DateOuverture { get; set; }

    /// <summary>
    /// Obtient ou définit la date de fermeture du CI
    /// </summary>
    public DateTime? DateFermeture { get; set; }

    /// <summary>
    /// Obtient ou définit l'Adresse de l'ci.
    /// </summary>
    public string Adresse { get; set; }

    /// <summary>
    /// Obtient ou définit la ville de l'ci.
    /// </summary>
    public string Ville { get; set; }

    /// <summary>
    /// Obtient ou définit le code postal de l'ci.
    /// </summary>
    public string CodePostal { get; set; }

    /// <summary>
    /// Obtient ou définit le pays de l'ci.
    /// </summary>
    public string Pays { get; set; }

    /// <summary>
    /// Obtient ou définit l'entête sur l'Adresse de livraison de l'affaire
    /// </summary>
    public string EnteteLivraison { get; set; }

    /// <summary>
    /// Obtient ou définit l'Adresse de livraison de l'affaire.
    /// </summary>
    public string AdresseLivraison { get; set; }

    /// <summary>
    /// Obtient ou définit le code postal de livraison de l'affaire.
    /// </summary>
    public string CodePostalLivraison { get; set; }

    /// <summary>
    /// Obtient ou définit le code postal de livraison de l'affaire.
    /// </summary>
    public string VilleLivraison { get; set; }

    /// <summary>
    /// Obtient ou définit le pays de livraison de l'affaire.
    /// </summary>
    public string PaysLivraison { get; set; }

    /// <summary>
    /// Obtient ou définit la longitude de la localisation de l'affaire.
    /// </summary>
    public string LongitudeLocalisation { get; set; }

    /// <summary>
    /// Obtient ou définit la latitude de la localisation de l'affaire.
    /// </summary>
    public string LatitudeLocalisation { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si l'Adresse de facturation correspond à l'Adresse de l'établissementcomptable
    /// </summary>
    public bool FacturationEtablissement { get; set; }
  }
}