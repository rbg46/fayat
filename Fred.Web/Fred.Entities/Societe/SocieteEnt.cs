using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fred.Entities.DatesCalendrierPaie;
using Fred.Entities.Facture;
using Fred.Entities.Groupe;
using Fred.Entities.Image;
using Fred.Entities.IndemniteDeplacement;
using Fred.Entities.Journal;
using Fred.Entities.Organisation;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.Role;
using Fred.Entities.Societe.Classification;

namespace Fred.Entities.Societe
{
    /// <summary>
    ///   Représente une société
    /// </summary>
    public class SocieteEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une société.
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'organisation de la societe
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'objet socite attaché à une organisation
        /// </summary>
        public OrganisationEnt Organisation { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la société de l'établissement
        /// </summary>
        public int GroupeId { get; set; }

        /// <summary>
        ///   Obtient ou définit la société de l'établissement
        /// </summary>
        public GroupeEnt Groupe { get; set; }

        /// <summary>
        ///   Obtient ou définit le code condensé de la société
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le code société paye
        /// </summary>
        public string CodeSocietePaye { get; set; }

        /// <summary>
        ///   Obtient ou définit le code société comptable
        /// </summary>
        public string CodeSocieteComptable { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé de la société
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient une concaténation du code et du libellé
        /// </summary>
        public string CodeLibelle => Code + " - " + Libelle;

        /// <summary>
        ///   Obtient ou définit l'Adresse d'une société
        /// </summary>
        public string Adresse { get; set; }

        /// <summary>
        ///   Obtient ou définit la ville d'une société
        /// </summary>
        public string Ville { get; set; }

        /// <summary>
        ///   Obtient ou définit le code postal d'une société
        /// </summary>
        public string CodePostal { get; set; }

        /// <summary>
        ///   Obtient ou définit le numéro SIRET d'une société
        /// </summary>
        public string SIRET { get; set; }

        /// <summary>
        ///   Obtient ou définit le numéro SIREN d'une société
        /// </summary>
        public string SIREN { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si une société est externe au groupe ou non.
        /// </summary>
        public bool Externe { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si une société est active ou non.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        ///   Obtient ou définit le mois du début de l'exercice comptable
        /// </summary>
        public int? MoisDebutExercice { get; set; }

        /// <summary>
        ///   Obtient ou définit le mois de la fin de l'exercice comptable
        /// </summary>
        public int? MoisFinExercice { get; set; }

        /// <summary>
        ///   Obtient ou déinit une valeur indiquant si pour cette societe, la procédure de génération des samedi en CP est activée
        /// </summary>
        public bool IsGenerationSamediCPActive { get; set; }

        /// <summary>
        ///   Obtient ou déinit une valeur indiquant si pour cette societe, on doit importer les factures
        /// </summary>
        public bool ImportFacture { get; set; }

        /// <summary>
        ///   Obtient ou définit si la société peut être transférée vers AS400
        /// </summary>
        public bool TransfertAS400 { get; set; }

        /// <summary>
        ///   Obtient ou définit le nom de l'image pour l'écran d'authentification d'une société
        /// </summary>
        public string ImageScreenLogin { get; set; }

        /// <summary>
        ///   Obtient ou définit le nom de l'image pour le logo d'une société en header d'une page
        /// </summary>
        public string ImageLogoHeader { get; set; }

        /// <summary>
        ///  Id de l'entité Image represantant le logo
        /// </summary>
        public int? ImageLogoId { get; set; }

        /// <summary>
        ///  Entité Image represantant le logo
        /// </summary>
        public ImageEnt ImageLogo { get; set; }

        /// <summary>
        ///  Id des CGA pour une commande de fournitures
        /// </summary>
        public int? CGAFournitureId { get; set; }

        /// <summary>
        ///  CGA pour une commande de fournitures
        /// </summary>
        public ImageEnt CGAFourniture { get; set; }

        /// <summary>
        ///  Id des CGA pour une commande de location
        /// </summary>
        public int? CGALocationId { get; set; }

        /// <summary>
        ///  CGA pour une commande de location
        /// </summary>
        public ImageEnt CGALocation { get; set; }

        /// <summary>
        ///  Id des CGA pour une commande de prestation
        /// </summary>
        public int? CGAPrestationId { get; set; }

        /// <summary>
        ///  CGA pour une commande de prestation
        /// </summary>
        public ImageEnt CGAPrestation { get; set; }

        /// <summary>
        ///   Obtient ou définit si la société est une société intérimaire
        /// </summary>
        public bool IsInterimaire { get; set; }

        /// <summary>
        ///  Id de l'entité Image represantant le login
        /// </summary>
        public int? ImageLoginId { get; set; }

        /// <summary>
        /// Entité Image represantant le login
        /// </summary>
        public ImageEnt ImageLogin { get; set; }

        /// <summary>
        ///   Obtient ou définit le pied de page de la société
        /// </summary>
        public string PiedDePage { get; set; }

        /// <summary>
        ///   Obtient ou définit le code société STORM (SAP)
        /// </summary>
        public string CodeSocieteStorm { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du type de calcul des indemnités de déplacement.
        /// </summary>
        public int? IndemniteDeplacementCalculTypeId { get; set; }

        /// <summary>
        /// Obtient ou définit le type de calcul des indemnités de déplacement.
        /// </summary>
        public IndemniteDeplacementCalculTypeEnt IndemniteDeplacementCalculType { get; set; }

        /// <summary>
        ///     Obtient ou définit l'identifiant du type de société
        /// </summary>
        public int? TypeSocieteId { get; set; }

        /// <summary>
        ///     Obtient ou définit le type de société (Interne, Sep, Partenaire, Intérimaire)
        /// </summary>
        public TypeSocieteEnt TypeSociete { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du fournisseur
        /// </summary>
        public int? FournisseurId { get; set; }

        /// <summary>
        /// Obtient ou définit le fournisseur
        /// </summary>
        public FournisseurEnt Fournisseur { get; set; }

        /// <summary>
        /// Obtient ou définit si la societe est rattache a un Etablissement fictif
        /// RG_5423_003 : Lien du CI avec un établissement fictif
        ///         Il faut gérer le cas où ANAEL ne fournit pas d’établissement pour un CI mais on doit en définir un dans FRED malgré tout
        ///         (NB – cela signifie qu’il y aura des écarts entre les établissements définis dans FRED et ceux définis dans ANAEL).
        /// 
        /// Rajouter un champ[EtablissementParDefaut] (O/N) au niveau de la table[FRED_SOCIETE] pour marquer les sociétés concernées.
        /// 
        /// Lors de l’import d’un CI d’une société marquée[EtablissementParDefaut]= Oui,
        /// ignorer les informations d’établissement éventuellement fournies par ANAEL, et à la place lier le CI à l’établissement de code ‘01’ de la société (cet établissement devra avoir été créé avant l’exécution de l’import).
        /// Si cet établissement de code ‘01’ n’est pas trouvé pour la société, ne pas importer le CI concerné et passer au CI suivant(ne pas interrompre le flux).
        /// 
        /// Lors de l’import d’un CI d’une société marquée[EtablissementParDefaut]=Non, exécuter l’import comme actuellement en liant le CI à l’établissement fourni par ANAEL(ou directement à la société si aucun établissement n’est fourni).
        /// </summary>
        public bool EtablissementParDefaut { get; set; }

        /// <summary>
        ///     Obtient ou définit l'identifiant de la société gérante si c'est une société SEP
        /// </summary>
        public int? SocieteGeranteId { get; set; }

        /// <summary>
        ///     Obtient ou définit la société gérante si c'est une société SEP
        /// </summary>
        public SocieteEnt SocieteGerante { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la clasiification
        /// </summary>
        public int? SocieteClassificationId { get; set; }

        /// <summary>
        /// Obtient ou définit la clasiification
        /// </summary>
        public SocieteClassificationEnt Classification { get; set; }

        /// <summary>
        /// Obtient ou définit le flag Budget avancement Ecart
        /// </summary>
        public bool IsBudgetAvancementEcart { get; set; }

        /// <summary>
        /// Obtient ou définit le flag Budget type avancement dynamique
        /// </summary>
        public bool IsBudgetTypeAvancementDynamique { get; set; }

        /// <summary>
        /// Obtient ou définit le flag Budget saisie recette
        /// </summary>
        public bool IsBudgetSaisieRecette { get; set; }

        /// <summary>
        /// Obtient ou définti la liste des Associés SEP
        /// </summary>
        public ICollection<AssocieSepEnt> AssocieSeps { get; set; }

        /// <summary>
        /// Child AffectationInterimaires where [FRED_PERSONNEL_FOURNISSEUR_SOCIETE].[SocieteId] point to this entity (FK_FRED_PERSONNEL_FOURNISSEUR_SOCIETE_SOCIETE)
        /// </summary>
        public virtual ICollection<ContratInterimaireEnt> ContratInterimaires { get; set; } // FRED_PERSONNEL_FOURNISSEUR_SOCIETE.FK_FRED_PERSONNEL_FOURNISSEUR_SOCIETE_SOCIETE

        /// <summary>
        /// Child CodeAbsences where [FRED_CODE_ABSENCE].[SocieteId] point to this entity (FK_FRED_CODE_ABSENCE_SOCIETE)
        /// </summary>
        public virtual ICollection<CodeAbsenceEnt> CodeAbsences { get; set; } // FRED_CODE_ABSENCE.FK_FRED_CODE_ABSENCE_SOCIETE

        /// <summary>
        /// Child CodeDeplacements where [FRED_CODE_DEPLACEMENT].[SocieteId] point to this entity (FK_FRED_CODE_DEPLACEMENT_SOCIETE)
        /// </summary>
        public virtual ICollection<CodeDeplacementEnt> CodeDeplacements { get; set; } // FRED_CODE_DEPLACEMENT.FK_FRED_CODE_DEPLACEMENT_SOCIETE

        /// <summary>
        /// Child CodeZoneDeplacements where [FRED_CODE_ZONE_DEPLACEMENT].[SocieteId] point to this entity (Fk_CdZoneDepSocieteId)
        /// </summary>
        public virtual ICollection<CodeZoneDeplacementEnt> CodeZoneDeplacements { get; set; } // FRED_CODE_ZONE_DEPLACEMENT.Fk_CdZoneDepSocieteId

        /// <summary>
        /// Child DatesCalendrierPaies where [FRED_DATES_CALENDRIER_PAIE].[SocieteId] point to this entity (FK_SOCIETE_ID)
        /// </summary>
        public virtual ICollection<DatesCalendrierPaieEnt> DatesCalendrierPaies { get; set; } // FRED_DATES_CALENDRIER_PAIE.FK_SOCIETE_ID

        /// <summary>
        /// Child EtablissementComptables where [FRED_ETABLISSEMENT_COMPTABLE].[SocieteId] point to this entity (FK_ETABLISSEMENT_COMPTABLE_SOCIETE)
        /// </summary>
        public virtual ICollection<EtablissementComptableEnt> EtablissementComptables { get; set; } // FRED_ETABLISSEMENT_COMPTABLE.FK_ETABLISSEMENT_COMPTABLE_SOCIETE

        /// <summary>
        /// Child EtablissementPaies where [FRED_ETABLISSEMENT_PAIE].[SocieteId] point to this entity (FK_ETABLISSEMENT_PAIE_SOCIETE)
        /// </summary>
        public virtual ICollection<EtablissementPaieEnt> EtablissementPaies { get; set; } // FRED_ETABLISSEMENT_PAIE.FK_ETABLISSEMENT_PAIE_SOCIETE

        /// <summary>
        /// Child Factures where [FRED_FACTURE].[SocieteId] point to this entity (FK_FACTURE_AR_SOCIETE)
        /// </summary>
        public virtual ICollection<FactureEnt> Factures { get; set; } // FRED_FACTURE.FK_FACTURE_AR_SOCIETE

        /// <summary>
        /// Child Journals where [FRED_JOURNAL].[SocieteId] point to this entity (FK_JournalSociete)
        /// </summary>
        public virtual ICollection<JournalEnt> Journals { get; set; } // FRED_JOURNAL.FK_JournalSociete

        /// <summary>
        /// Child Materiels where [FRED_MATERIEL].[SocieteId] point to this entity (FK_FRED_MATERIEL_SOCIETE)
        /// </summary>
        public virtual ICollection<MaterielEnt> Materiels { get; set; } // FRED_MATERIEL.FK_FRED_MATERIEL_SOCIETE

        /// <summary>
        /// Child Natures where [FRED_NATURE].[SocieteId] point to this entity (FK_FRED_NATURE_SOCIETE)
        /// </summary>
        public virtual ICollection<NatureEnt> Natures { get; set; } // FRED_NATURE.FK_FRED_NATURE_SOCIETE

        /// <summary>
        /// Child OrganisationLiens where [FRED_ORGA_LIENS].[SocieteId] point to this entity (FK_FRED_ORGA_LIENS_SOCIETE)
        /// </summary>
        public virtual ICollection<OrganisationLienEnt> OrganisationLiens { get; set; } // FRED_ORGA_LIENS.FK_FRED_ORGA_LIENS_SOCIETE

        /// <summary>
        /// Child Personnels where [FRED_PERSONNEL].[SocieteId] point to this entity (FK_PERSONNEL_SOCIETE)
        /// </summary>
        public virtual ICollection<PersonnelEnt> Personnels { get; set; } // FRED_PERSONNEL.FK_PERSONNEL_SOCIETE

        /// <summary>
        /// Child Primes where [FRED_PRIME].[SocieteId] point to this entity (FK_FRED_PRIME_SOCIETE)
        /// </summary>
        public virtual ICollection<PrimeEnt> Primes { get; set; } // FRED_PRIME.FK_FRED_PRIME_SOCIETE

        /// <summary>
        /// Child ReferentielEtendus where [FRED_SOCIETE_RESSOURCE_NATURE].[SocieteId] point to this entity (FK_FRED_SOCIETE_RESSOURCE_NATURE_SOCIETE)
        /// </summary>
        public virtual ICollection<ReferentielEtenduEnt> ReferentielEtendus { get; set; } // FRED_SOCIETE_RESSOURCE_NATURE.FK_FRED_SOCIETE_RESSOURCE_NATURE_SOCIETE

        /// <summary>
        /// Child SocieteDevises where [FRED_SOCIETE_DEVISE].[SocieteId] point to this entity (FK_FRED_SOCIETE_DEVISE_SOCIETE)
        /// </summary>
        public virtual ICollection<SocieteDeviseEnt> SocieteDevises { get; set; } // FRED_SOCIETE_DEVISE.FK_FRED_SOCIETE_DEVISE_SOCIETE  

        /// <summary>
        ///   Obtient ou définit la liste des rôles que possèdent cette organisation
        /// </summary>  
        public virtual ICollection<RoleEnt> Roles { get; set; }
    }
}
