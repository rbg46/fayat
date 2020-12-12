using Fred.Web.Models.Organisation;
using Fred.Web.Models.Personnel;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Societe;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fred.Web.Models.CI
{
  /// <summary>
  /// Représente une ci
  /// </summary>
  public class CIModel : IReferentialModel
  {

    private DateTime? dateOuverture;
    private DateTime? dateFermeture;
    private DateTime? horaireDebutM;
    private DateTime? horaireFinM;
    private DateTime? horaireDebutS;
    private DateTime? horaireFinS;

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une ci.
    /// </summary>
    public int CiId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de l'organisation du ci
    /// </summary>
    public int? OrganisationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'organisation du ci
    /// </summary>
    public OrganisationModel Organisation { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de l'établissement comptable du ci.
    /// </summary>
    public int? EtablissementComptableId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité établissement comptable de l'ci.
    /// </summary>
    public EtablissementComptableModel EtablissementComptable { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de la société de l'ci
    /// </summary>
    public int? SocieteId { get; set; }

    /// <summary>
    /// Obtient ou définit la société de l'ci
    /// </summary>
    public SocieteModel Societe { get; set; }

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
    ///   Obtient ou définit l'Adresse de Facturation de l'affaire.
    /// </summary>    
    public string AdresseFacturation { get; set; }

    /// <summary>
    ///   Obtient ou définit le code postal de Facturation de l'affaire.
    /// </summary>    
    public string CodePostalFacturation { get; set; }

    /// <summary>
    ///   Obtient ou définit le code postal de Facturation de l'affaire.
    /// </summary>    
    public string VilleFacturation { get; set; }

    /// <summary>
    ///   Obtient ou définit le pays de Facturation de l'affaire.
    /// </summary>    
    public string PaysFacturation { get; set; }

    /// <summary>
    /// Obtient ou définit la longitude de la localisation de l'affaire.
    /// </summary>
    public double? LongitudeLocalisation { get; set; }

    /// <summary>
    /// Obtient ou définit la latitude de la localisation de l'affaire.
    /// </summary>
    public double? LatitudeLocalisation { get; set; }

    /// <summary>
    /// Obtient ou définit si l'Adresse de facturation correspond à l'Adresse de l'établissementcomptable
    /// </summary>
    public bool FacturationEtablissement { get; set; }

    /// <summary>
    /// Obtient ou définit le responsable chantier
    /// </summary>
    public string ResponsableChantier { get; set; }

    /// <summary>
    /// Obtient ou définit l'id du responsable administratif
    /// </summary>
    public int? ResponsableAdministratifId { get; set; }

    /// <summary>
    /// Obtient ou définit le responsable administratif
    /// </summary>
    public PersonnelModel ResponsableAdministratif { get; set; }

    /// <summary>
    /// Obtient ou définit l'entreprise partenaire
    /// </summary>
    public decimal? FraisGeneraux { get; set; }

    /// <summary>
    /// Obtient ou définit l'entreprise partenaire
    /// </summary>
    public decimal? TauxHoraire { get; set; }

    /// <summary>
    /// Obtient ou définit l'horaire de début de matinée
    /// </summary>
    public DateTime? HoraireDebutM { get; set; }

    /// <summary>
    /// Obtient ou définit l'horaire de fin de matinée
    /// </summary>
    public DateTime? HoraireFinM { get; set; }

    /// <summary>
    /// Obtient ou définit l'horaire de début de soirée
    /// </summary>
    public DateTime? HoraireDebutS { get; set; }

    /// <summary>
    /// Obtient ou définit l'horaire de fin de soirée
    /// </summary>
    public DateTime? HoraireFinS { get; set; }

    /// <summary>
    /// Obtient ou définit le type du chantier
    /// </summary>
    public string TypeCI { get; set; }

    /// <summary>
    /// Obtient ou définit le montant HT du chantier
    /// </summary>
    public decimal? MontantHT { get; set; }

    /// <summary>
    /// Obtient ou définit la devise du montant HT du chantier
    /// </summary>
    public int? MontantDeviseId { get; set; }

    /// <summary>
    /// Obtient ou définit la devise du montant HT du chantier
    /// </summary>
    public DeviseModel MontantDevise { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si la zone de deplacement est modifiable
    /// </summary>
    public bool ZoneModifiable { get; set; }

    /// <summary>
    ///   Obtient ou définit la prise en compte du carburant dans l'exploitation 
    /// </summary>
    public bool CarburantActif { get; set; }

    /// <summary>
    /// Obtient ou définit la durée du chantier
    /// </summary>
    public int? DureeChantier { get; set; }

    /// <summary>
    /// Obtient une concaténation du code et du libellé
    /// </summary>
    public string CodeLibelle { get; set; }

    /// <summary>
    ///  Obtient ou définit la liste des devises du CI
    /// </summary>
    public ICollection<CIDeviseModel> CIDevises { get; set; }

    /// <summary>
    /// Obtient ou définit la concaténation du code et libellé société
    /// </summary>
    public string SocieteCodeDataGridColumn
    {
      get
      {
        if (this.Societe != null && this.Societe.Code != null)
        {
          return this.Societe.Code;
        }
        else return string.Empty;
      }
    }

    /// <summary>
    /// Obtient ou définit la concaténation du code et libellé société
    /// </summary>
    public string SocieteLibelleDataGridColumn
    {
      get
      {
        if (this.Societe != null && this.Societe.Libelle != null)
        {
          return this.Societe.Libelle;
        }
        else return string.Empty;
      }
    }

    /// <summary>
    /// Obtient ou définit la concaténation du code et libellé société
    /// </summary>
    public string SocieteDataGridColumn
    {
      get
      {
        if (this.Societe != null && this.Societe.Libelle != null && this.Societe.Code != null)
        {
          return this.Societe.Code + " - " + this.Societe.Libelle;
        }
        else return string.Empty;
      }
    }

    /// <summary>
    /// Obtient ou définit la concaténation du code et libellé société
    /// </summary>
    public string EtablissementComptableCodeDataGridColumn
    {
      get
      {
        if (this.EtablissementComptable != null && this.EtablissementComptable.Code != null)
        {
          return this.EtablissementComptable.Code;
        }
        else return string.Empty;
      }
    }

    /// <summary>
    /// Obtient ou définit la concaténation du code et libellé société
    /// </summary>
    public string EtablissementComptableLibelleDataGridColumn
    {
      get
      {
        if (this.EtablissementComptable != null && this.EtablissementComptable.Libelle != null)
        {
          return this.EtablissementComptable.Libelle;
        }
        else return string.Empty;
      }
    }

    /// <summary>
    /// Obtient ou définit la concaténation du code et libellé société
    /// </summary>
    public string EtablissementComptableDataGridColumn
    {
      get
      {
        if (this.EtablissementComptable != null && this.EtablissementComptable.Libelle != null && this.EtablissementComptable.Code != null)
        {
          return this.EtablissementComptable.Code + " - " + this.EtablissementComptable.Libelle;
        }
        else return string.Empty;
      }
    }

    /// <summary>
    /// Obtient l'identifiant du référentiel CI
    /// </summary>
    public string IdRef => this.CiId.ToString();

    /// <summary>
    /// Obtient ou définit le libelle du référentiel CI
    /// </summary>
    public string LibelleRef => this.Libelle;

    /// <summary>
    /// Obtient ou définit le code du référentiel CI
    /// </summary>
    public string CodeRef => this.Code;

    /// <summary>
    ///   Obtient ou définit si le CI est clôturé
    /// </summary>    
    public bool? IsClosed { get; set; }
  }
}