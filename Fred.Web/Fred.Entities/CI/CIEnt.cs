using System;
using System.Collections.Generic;
using System.Diagnostics;
using Fred.Entities.Affectation;
using Fred.Entities.Bareme;
using Fred.Entities.Budget;
using Fred.Entities.Commande;
using Fred.Entities.DatesClotureComptable;
using Fred.Entities.Depense;
using Fred.Entities.Facture;
using Fred.Entities.Organisation;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.Societe;

namespace Fred.Entities.CI
{
    /// <summary>
    ///   Représente une affaire
    /// </summary>
    [DebuggerDisplay("{Code} (Id: {CiId})")]
    public class CIEnt
    {
        private DateTime? dateOuverture;
        private DateTime? dateFermeture;
        private DateTime? horaireDebutM;
        private DateTime? horaireFinM;
        private DateTime? horaireDebutS;
        private DateTime? horaireFinS;
        private DateTime? dateImport;
        private DateTime? dateUpdate;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une affaire.
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'organisation de la CI
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'objet CI attaché à une organisation
        /// </summary>   
        public OrganisationEnt Organisation { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si une affaire est une SEP.
        /// </summary>
        public bool Sep { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'établissement comptable de l'affaire.
        /// </summary>
        public int? EtablissementComptableId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité établissement comptable de l'affaire.
        /// </summary>
        public EtablissementComptableEnt EtablissementComptable { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la société de l'affaire
        /// </summary>
        public int? SocieteId { get; set; }

        /// <summary>
        ///   Obtient ou définit la société de l'affaire
        /// </summary>
        public SocieteEnt Societe { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'une affaire.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'une affaire.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit la date d'ouverture du CI
        /// </summary>
        public DateTime? DateOuverture
        {
            get
            {
                return (dateOuverture.HasValue) ? DateTime.SpecifyKind(dateOuverture.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateOuverture = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit la date de fermeture du CI
        /// </summary>
        public DateTime? DateFermeture
        {
            get
            {
                return (dateFermeture.HasValue) ? DateTime.SpecifyKind(dateFermeture.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateFermeture = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'Adresse de l'affaire.
        /// </summary>
        public string Adresse { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Adresse2 de l'affaire.
        /// </summary>
        public string Adresse2 { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Adresse de l'affaire.
        /// </summary>
        public string Adresse3 { get; set; }

        /// <summary>
        ///   Obtient ou définit la ville de l'affaire.
        /// </summary>
        public string Ville { get; set; }

        /// <summary>
        ///   Obtient ou définit le code postal de l'affaire.
        /// </summary>
        public string CodePostal { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du pays du CI
        /// </summary>
        public int? PaysId { get; set; }

        /// <summary>
        ///   Obtient ou définit le Pays du CI
        /// </summary>
        public PaysEnt Pays { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entête sur l'Adresse de livraison de l'affaire
        /// </summary>
        public string EnteteLivraison { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Adresse de livraison de l'affaire.
        /// </summary>
        public string AdresseLivraison { get; set; }

        /// <summary>
        ///   Obtient ou définit le code postal de livraison de l'affaire.
        /// </summary>
        public string CodePostalLivraison { get; set; }

        /// <summary>
        ///   Obtient ou définit le code postal de livraison de l'affaire.
        /// </summary>
        public string VilleLivraison { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du pays de livraison du CI
        /// </summary>
        public int? PaysLivraisonId { get; set; }

        /// <summary>
        ///   Obtient ou définit le Pays  de livraison du CI
        /// </summary>
        public PaysEnt PaysLivraison { get; set; }

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
        ///   Obtient ou définit le Pays de facturation du CI
        /// </summary>
        public PaysEnt PaysFacturation { get; set; }

        /// <summary>
        ///   Obtient ou définit la longitude de la localisation de l'affaire.
        /// </summary>
        public double? LongitudeLocalisation { get; set; }

        /// <summary>
        ///   Obtient ou définit la latitude de la localisation de l'affaire.
        /// </summary>
        public double? LatitudeLocalisation { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si l'Adresse de facturation correspond à l'Adresse de
        ///   l'établissementcomptable
        /// </summary>
        public bool FacturationEtablissement { get; set; }

        /// <summary>
        ///   Obtient ou définit le responsable chantier
        /// </summary>
        public string ResponsableChantier { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id du responsable administratif
        /// </summary>
        public int? ResponsableChantierId { get; set; }

        /// <summary>
        ///   Obtient ou définit le responsable administratif
        /// </summary>
        public PersonnelEnt PersonnelResponsableChantier { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id du responsable administratif
        /// </summary>
        public int? ResponsableAdministratifId { get; set; }

        /// <summary>
        ///   Obtient ou définit le responsable administratif
        /// </summary>
        public PersonnelEnt ResponsableAdministratif { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entreprise partenaire
        /// </summary>
        public decimal? FraisGeneraux { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entreprise partenaire
        /// </summary>
        public decimal? TauxHoraire { get; set; }

        /// <summary>
        ///   Obtient ou définit l'horaire de début de matinée
        /// </summary>
        public DateTime? HoraireDebutM
        {
            get
            {
                return (horaireDebutM.HasValue) ? DateTime.SpecifyKind(horaireDebutM.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                horaireDebutM = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'horaire de fin de matinée
        /// </summary>
        public DateTime? HoraireFinM
        {
            get
            {
                return (horaireFinM.HasValue) ? DateTime.SpecifyKind(horaireFinM.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                horaireFinM = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'horaire de début de soirée
        /// </summary>
        public DateTime? HoraireDebutS
        {
            get
            {
                return (horaireDebutS.HasValue) ? DateTime.SpecifyKind(horaireDebutS.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                horaireDebutS = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'horaire de fin de soirée
        /// </summary>
        public DateTime? HoraireFinS
        {
            get
            {
                return (horaireFinS.HasValue) ? DateTime.SpecifyKind(horaireFinS.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                horaireFinS = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit le type du chantier
        /// </summary>
        public string TypeCI { get; set; }

        /// <summary>
        /// Obtient ou définit le type du chantier.
        /// </summary>
        public int? CITypeId { get; set; }

        /// <summary>
        /// Obtient ou définit le type du chantier.
        /// </summary>
        public CITypeEnt CIType { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant HT du chantier
        /// </summary>
        public decimal? MontantHT { get; set; }

        /// <summary>
        ///   Obtient ou définit la devise du montant HT du chantier
        /// </summary>
        public int? MontantDeviseId { get; set; }

        /// <summary>
        ///   Obtient ou définit la devise du montant HT du chantier
        /// </summary>
        public DeviseEnt MontantDevise { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la zone de deplacement est modifiable
        /// </summary>
        public bool ZoneModifiable { get; set; }

        /// <summary>
        ///   Obtient ou définit la prise en compte du carburant dans l'exploitation 
        /// </summary>
        public bool CarburantActif { get; set; }

        /// <summary>
        ///   Obtient ou définit la durée du chantier
        /// </summary>
        public int? DureeChantier { get; set; }

        /// <summary>
        /// Indique si le CI est géré par FRED ou non.
        /// </summary>
        public bool ChantierFRED { get; set; }

        /// <summary>
        /// Obtient ou définit la date de l'import.
        /// </summary>
        public DateTime? DateImport
        {
            get { return (dateImport.HasValue) ? DateTime.SpecifyKind(dateImport.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateImport = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        /// Obtient ou définit la date de la mise à jour.
        /// </summary>
        public DateTime? DateUpdate
        {
            get { return (dateUpdate.HasValue) ? DateTime.SpecifyKind(dateUpdate.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateUpdate = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        /// Obtient ou definit la valeur boolean pour gerer les astreintes
        /// </summary>
        public bool IsAstreinteActive { get; set; }

        /// <summary>
        /// Obtient la chaine de caractère du code interne unique
        /// </summary>
        public string CodeInterne { get; private set; }

        /// <summary>
        ///   Obtient ou définit la liste des taches du CI
        /// </summary>
        public ICollection<TacheEnt> Taches { get; set; }

        /// <summary>
        ///  Obtient ou définit la liste des devises du CI
        /// </summary>
        public ICollection<CIDeviseEnt> CIDevises { get; set; }

        /// <summary>
        ///   Obtient ou définit si le CI est clôturé
        /// </summary>
        public bool? IsClosed { get; set; }

        /// <summary>
        ///   Obtient ou définit si le CI possède plusieurs devises
        /// </summary>
        public bool IsCiHaveManyDevise { get; set; } = true;

        /// <summary>
        ///   Obtient une concaténation du code et du libellé
        /// </summary>
        public string CodeLibelle => (Code ?? string.Empty) + " - " + (Libelle ?? string.Empty);

        /// <summary>
        ///     Obtient ou définit l'identifiant le compte interne SEP
        /// </summary>
        public int? CompteInterneSepId { get; set; }

        /// <summary>
        ///     Obtient ou définit le compte interne SEP
        /// </summary>
        public CIEnt CompteInterneSep { get; set; }

        /// <summary>
        ///     Obtient ou définit l'objectif d'heures d'inserion du chantier
        /// </summary>
        public int? ObjectifHeuresInsertion { get; set; }

        /// <summary>
        ///     Obtient ou définit si le CI est ouvert ou non au pointage
        /// </summary>
        public bool IsDisableForPointage { get; set; }

        /// <summary>
        /// Permet d’indiquer si le CI est un CI générique pour les absences
        /// </summary>
        public bool IsAbsence { get; set; }

        ///////////////////////////////////////////////////////////////////////////
        // AJOUT LORS DE LE MIGRATION CODE FIRST 
        ///////////////////////////////////////////////////////////////////////////

        // Reverse navigation

        /// <summary>
        /// Child Budgets where [FRED_BUDGET].[CiId] point to this entity (FK_FRED_BUDGET_CI)
        /// </summary>
        public virtual ICollection<BudgetEnt> Budgets { get; set; } // FRED_BUDGET.FK_FRED_BUDGET_CI

        /// <summary>
        /// Child CICodeMajorations where [FRED_CI_CODE_MAJORATION].[CiId] point to this entity (FK_FRED_CI_CODE_MAJORATION_CI)
        /// </summary>
        public virtual ICollection<CICodeMajorationEnt> CICodeMajorations { get; set; } // FRED_CI_CODE_MAJORATION.FK_FRED_CI_CODE_MAJORATION_CI

        /// <summary>
        /// Child CIPrimes where [FRED_CI_PRIME].[CiId] point to this entity (FK_FRED_CI_PRIME_CI)
        /// </summary>
        public virtual ICollection<CIPrimeEnt> CIPrimes { get; set; } // FRED_CI_PRIME.FK_FRED_CI_PRIME_CI

        /// <summary>
        /// Child CIRessources where [FRED_CI_RESSOURCE].[CiId] point to this entity (FK_FRED_CI_RESSOURCE_CI)
        /// </summary>
        public virtual ICollection<CIRessourceEnt> CIRessources { get; set; } // FRED_CI_RESSOURCE.FK_FRED_CI_RESSOURCE_CI

        /// <summary>
        /// Child Commandes where [FRED_COMMANDE].[CiId] point to this entity (FK_COMMANDE_CI)
        /// </summary>
        public virtual ICollection<CommandeEnt> Commandes { get; set; } // FRED_COMMANDE.FK_COMMANDE_CI

        /// <summary>
        /// Child DatesClotureComptables where [FRED_DATES_CLOTURE_COMPTABLE].[CiId] point to this entity (FK_CI_ID)
        /// </summary>
        public virtual ICollection<DatesClotureComptableEnt> DatesClotureComptables { get; set; } // FRED_DATES_CLOTURE_COMPTABLE.FK_CI_ID

        /// <summary>
        /// Child Depenses where [FRED_DEPENSE].[CiId] point to this entity (FK_DEPENSE_CI)
        /// </summary>
        public virtual ICollection<DepenseAchatEnt> Depenses { get; set; } // FRED_DEPENSE.FK_DEPENSE_CI

        /// <summary>
        /// Child DepenseTemporaires where [FRED_DEPENSE_TEMPORAIRE].[CiId] point to this entity (FK_DEPENSE_TEMPORAIRE_CI)
        /// </summary>
        public virtual ICollection<DepenseTemporaireEnt> DepenseTemporaires { get; set; } // FRED_DEPENSE_TEMPORAIRE.FK_DEPENSE_TEMPORAIRE_CI

        /// <summary>
        /// Child FactureLignes where [FRED_FACTURE_LIGNE].[AffaireId] point to this entity (FK_LIGNE_FACTURE_CI)
        /// </summary>
        public virtual ICollection<FactureLigneEnt> FactureLignes { get; set; } // FRED_FACTURE_LIGNE.FK_LIGNE_FACTURE_CI

        /// <summary>
        /// Child PointageAnticipes where [FRED_POINTAGE_ANTICIPE].[CiId] point to this entity (FK_POINTAGE_ANTICIPE_CI)
        /// </summary>
        public virtual ICollection<PointageAnticipeEnt> PointageAnticipes { get; set; } // FRED_POINTAGE_ANTICIPE.FK_POINTAGE_ANTICIPE_CI

        /// <summary>
        /// Child Rapports where [FRED_RAPPORT].[CiId] point to this entity (FK_FRED_RAPPORT_FRED_CI)
        /// </summary>
        public virtual ICollection<RapportEnt> Rapports { get; set; } // FRED_RAPPORT.FK_FRED_RAPPORT_FRED_CI

        /// <summary>
        /// Child RapportLignes where [FRED_RAPPORT_LIGNE].[CiId] point to this entity (FK_RAPPORT_LIGNE_CI)
        /// </summary>
        public virtual ICollection<RapportLigneEnt> RapportLignes { get; set; } // FRED_RAPPORT_LIGNE.FK_RAPPORT_LIGNE_CI

        /// <summary>
        /// Child RapportLignes where [FRED_AFFECTATION].[CiId] point to this enttity (FK_AFFECTATION_CI)
        /// </summary>
        public virtual ICollection<AffectationEnt> Affectations { get; set; } //FRED_AFFECTATION.FK_AFFECTATION_CI

        /// <summary>
        /// Liste des surcharges barèmes exploitation CI
        /// </summary>
        public ICollection<SurchargeBaremeExploitationCIEnt> SurchargeBaremeExploitationCIs { get; set; }

        /// <summary>
        /// Obtient ou définit les barèmes exploitations CI
        /// </summary>
        public ICollection<BaremeExploitationCIEnt> BaremeExploitationCIs { get; set; }

        /// <summary>
        /// Obtient ou définit la description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Obtient ou définit le code affaire hors ANAEL (Hélios, etc...).
        /// </summary>
        public string CodeExterne { get; set; }

        /// <summary>
        ///   Obtient la liste des Organisations parent du CI
        /// </summary>
        public ICollection<OrganisationEnt> Parents { get; set; }
    }
}
