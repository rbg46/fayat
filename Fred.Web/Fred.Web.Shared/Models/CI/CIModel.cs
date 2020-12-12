using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Fred.Web.Models.Organisation;
using Fred.Web.Models.OrganisationGenerique;
using Fred.Web.Models.Personnel;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Societe;

namespace Fred.Web.Models.CI
{
    /// <summary>
    /// Représente une ci
    /// </summary>
    public class CIModel : IReferentialModel
    {
        /// <summary>
        /// Obtient Le code Societe + Code CI
        /// </summary>
        public virtual string CodeSocieteCI
        {
            get
            {
                if (this.Societe != null)
                {
                    return $"{Societe.Code} - {Code}";
                }
                else
                {
                    return Code;
                }
            }
        }

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
        /// Obtient ou définit l'établissement comptable du CI
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
        /// Obtient ou définit l'Adresse2.
        /// </summary>
        public string Adresse2 { get; set; }

        /// <summary>
        /// Obtient ou définit l'Adresse3.
        /// </summary>
        public string Adresse3 { get; set; }

        /// <summary>
        /// Obtient ou définit la ville de l'ci.
        /// </summary>
        public string Ville { get; set; }

        /// <summary>
        /// Obtient ou définit le code postal de l'ci.
        /// </summary>
        public string CodePostal { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du pays du CI
        /// </summary>
        public int? PaysId { get; set; }

        /// <summary>
        /// Obtient ou définit le pays de ci.
        /// </summary>
        public PaysModel Pays { get; set; }

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
        ///   Obtient ou définit l'identifiant du pays de Livraison du CI
        /// </summary>
        public int? PaysLivraisonId { get; set; }

        /// <summary>
        /// Obtient ou définit le pays de livraison de l'affaire.
        /// </summary>
        public PaysModel PaysLivraison { get; set; }

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
        ///   Obtient ou définit l'identifiant du pays de facturation du CI
        /// </summary>
        public int? PaysFacturationId { get; set; }

        /// <summary>
        ///   Obtient ou définit le pays de Facturation de l'affaire.
        /// </summary>    
        public PaysModel PaysFacturation { get; set; }

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
        /// Obtient ou définit l'id du responsable chantier
        /// </summary>
        public int? ResponsableChantierId { get; set; }

        /// <summary>
        /// Obtient ou définit le responsable chantier
        /// </summary>
        public PersonnelModel PersonnelResponsableChantier { get; set; }

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
        public virtual string CodeLibelle { get; set; }

        /// <summary>
        /// Obtient ou definit la valeur boolean pour gerer les astreintes
        /// </summary>
        public bool IsAstreinteActive { get; set; }

        /// <summary>
        ///  Obtient ou définit la liste des devises du CI
        /// </summary>
        public ICollection<CIDeviseModel> CIDevises { get; set; }

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

        /// <summary>
        ///   Obtient ou définit si le CI possède plusieurs devises
        /// </summary>
        public bool IsCiHaveManyDevise { get; set; }

        /// <summary>
        ///   Obtient le PUO du CI
        /// </summary>
        public OrganisationGeneriqueModel PUO { get; set; }

        /// <summary>
        ///   Obtient l'UO du CI
        /// </summary>
        public OrganisationGeneriqueModel UO { get; set; }

        /// <summary>
        ///   Obtient la liste des Organisations parent du CI
        /// </summary>
        public ICollection<OrganisationModel> Parents { get; set; }

        /// <summary>
        /// Obtient ou definit l'identifier de type Ci Groupe FES
        /// </summary>
        public int? CITypeId { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des taches du CI
        /// </summary>
        public ICollection<TacheModel> Taches { get; set; }

        /// <summary>
        ///     Obtient ou définit l'identifiant le compte interne SEP
        /// </summary>
        public int? CompteInterneSepId { get; set; }

        /// <summary>
        ///     Obtient ou définit le compte interne SEP
        /// </summary>        
        public CIModel CompteInterneSep { get; set; }

        /// <summary>
        ///     Obtient ou définit l'objectif d'heures d'inserion du chantier
        /// </summary>
        public int? ObjectifHeuresInsertion { get; set; }

        /// <summary>
        /// Obtient ou définit le code de la societe.
        /// </summary>
        public string SocieteCode { get; set; }

        /// <summary>
        /// Indique si le CI est géré par FRED ou non.
        /// </summary>
        public bool ChantierFRED { get; set; }

        /// <summary>
        /// Obtient ou définit si le CI est ouvert ou non au pointage
        /// </summary>
        public bool IsDisableForPointage { get; set; }
    }
}
