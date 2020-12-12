using Fred.Web.Models.Organisation;
using Fred.Web.Models.Referential;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fred.Web.Models.Societe
{
  /// <summary>
  /// Représente une société
  /// </summary>
  public class SocieteModel : IReferentialModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une société.
    /// </summary>
    [Required]
    public int SocieteId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de l'organisation de la société
    /// </summary>
    public int OrganisationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'organisation de la société
    /// </summary>
    public OrganisationModel Organisation { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique du groupe de la société
    /// </summary>
    public int GroupeId { get; set; }

    /// <summary>
    /// Obtient ou définit le code condensé de la société
    /// </summary>
    [Required]
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le code société paye
    /// </summary>
    public string CodeSocietePaye { get; set; }

    /// <summary>
    /// Obtient ou définit le code société comptable
    /// </summary>
    public string CodeSocieteComptable { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé de la société
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient une concaténation du code et du libellé
    /// </summary>
    public string CodeLibelle { get; set; }

    /// <summary>
    /// Obtient ou définit l'Adresse d'une société
    /// </summary>
    public string Adresse { get; set; }

    /// <summary>
    /// Obtient ou définit la ville d'une société
    /// </summary>
    public string Ville { get; set; }

    /// <summary>
    /// Obtient ou définit le code postal d'une société
    /// </summary>
    public string CodePostal { get; set; }

    /// <summary>
    /// Obtient ou définit le numéro SIRET d'une société
    /// </summary>
    public string SIRET { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si une société est externe au groupe ou non.
    /// </summary>
    public bool Externe { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si une société est active ou non.
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Obtient ou définit le mois du début de l'exercice comptable
    /// </summary>    
    public int? MoisDebutExercice { get; set; }

    /// <summary>
    /// Obtient ou définit le mois de la fin de l'exercice comptable
    /// </summary>
    public int? MoisFinExercice { get; set; }

    /// <summary>
    /// Obtient ou déinit une valeur indiquant si pour cette societe, la procédure de génération des samedi en CP est activée
    /// </summary>
    public bool IsGenerationSamediCPActive { get; set; }

    /// <summary>
    /// Obtient ou déinit une valeur indiquant si pour cette societe, on doit importer les factures
    /// </summary>
    public bool ImportFacture { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si une société est partenaire.
    /// </summary>
    public bool Partenaire { get; set; }

    /// <summary>
    /// Obtient ou définit la quote-part de la société si c'est une partenaire.
    /// </summary>
    public short QuotePart { get; set; }

    /// <summary>
    /// Obtient ou définit le type de participation de la société si c'est une partenaire.
    /// </summary>
    public int TypeParticipationId { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé du type de participation de la société si c'est une partenaire.
    /// </summary>
    public string TypeParticipationLbl { get; set; }

    /// <summary>
    /// Obtient ou définit une liste de devise
    /// </summary>
    public List<DeviseModel> DeviseListe { get; set; }

    /// <summary>
    /// Obtient le libellé référentiel d'un personnel.
    /// </summary>
    public string LibelleRef => this.Libelle;

    /// <summary>
    /// Obtient le code référentiel d'un personnel.
    /// </summary>
    public string CodeRef => this.Code;

    /// <summary>
    /// Obtient l'id du référentiel
    /// </summary>
    public string IdRef => this.SocieteId.ToString();
  }
}
