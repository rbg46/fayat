using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using Fred.Entities.Avis;
using Fred.Entities.CI;
using Fred.Entities.Facturation;
using Fred.Entities.Import;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;

namespace Fred.Entities.Commande
{
    /// <summary>
    ///   Représente une commande.
    /// </summary>
    [DebuggerDisplay("{Numero}")]
    public class CommandeEnt
    {
        private DateTime date;
        private DateTime? dateMiseADispo;
        private DateTime? dateValidation;
        private DateTime? dateCreation;
        private DateTime? dateModification;
        private DateTime? dateSuppression;
        private DateTime? dateCloture;
        private DateTime? dateProchaineReception;
        private DateTime? dateDerniereReception;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une commande.
        /// </summary>
        public int CommandeId { get; set; }

        /// <summary>
        ///   Obtient ou définit le préfixe calculé du numéro de commande manuelle
        /// </summary>
        public string PatternNumeroCommandeManuelle { get; set; }

        /// <summary>
        ///   Obtient ou définit le numéro d'une commande.
        /// </summary>
        public string Numero { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'affaire de la commande.
        /// </summary>
        public int? CiId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité affaire reliée à la commande
        /// </summary>
        public CIEnt CI { get; set; } = null;

        /// <summary>
        ///   Obtient ou définit le libellé de la commande.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de la commande.
        /// </summary>
        public DateTime Date
        {
            get
            {
                return DateTime.SpecifyKind(date, DateTimeKind.Utc);
            }
            set
            {
                date = DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'ID fournisseur de la commande.
        /// </summary>
        public int? FournisseurId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du fournisseur de la commande.
        /// </summary>
        public FournisseurEnt Fournisseur { get; set; } = null;

        /// <summary>
        /// Fournisseur provisoire
        /// </summary>
        public string FournisseurProvisoire { get; set; }

        /// <summary>
        /// Date de la première impression de brouillon
        /// </summary>
        public DateTime? DatePremiereImpressionBrouillon { get; set; }

        /// <summary>
        /// Auteur de la première impression de brouillon
        /// </summary>
        public int? AuteurPremiereImpressionBrouillonId { get; set; }

        /// <summary>
        /// Auteur de la première impression de brouillon
        /// </summary>
        public UtilisateurEnt AuteurPremiereImpressionBrouillon { get; set; }

        /// <summary>
        /// Obtient ou définit l'id de l'agence fournisseur sélectionnée
        /// </summary>
        [Column("AgenceId")]
        public int? AgenceId { get; set; }

        /// <summary>
        /// Obtient ou définit l'agence fournisseur sélectionnée
        /// </summary>
        [ForeignKey("AgenceId")]
        public AgenceEnt Agence { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des lignes d'une commande
        /// </summary>
        public ICollection<CommandeLigneEnt> Lignes { get; set; }

        /// <summary>
        ///   Obtient ou définit le délai de livraison de la commande.
        /// </summary>
        public string DelaiLivraison { get; set; }

        /// <summary>
        ///   Obtient ou définit le date de mise à disposition de la commande.
        /// </summary>
        public DateTime? DateMiseADispo
        {
            get
            {
                return (dateMiseADispo.HasValue) ? DateTime.SpecifyKind(dateMiseADispo.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateMiseADispo = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'ID du statut de la commande.
        /// </summary>
        public int? StatutCommandeId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du statut relié à la commande
        /// </summary>
        public StatutCommandeEnt StatutCommande { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la commande contient la conduite MO.
        /// </summary>
        public bool MOConduite { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la commande contient l'entretien mécanique.
        /// </summary>
        public bool EntretienMecanique { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la commande contient l'entretien journalier.
        /// </summary>
        public bool EntretienJournalier { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la commande contient le carburant.
        /// </summary>
        public bool Carburant { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la commande contient le lubrifiant.
        /// </summary>
        public bool Lubrifiant { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la commande contient les frais d'amortissement.
        /// </summary>
        public bool FraisAmortissement { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la commande contient les frais d'amortissement.
        /// </summary>
        public bool FraisAssurance { get; set; }

        /// <summary>
        ///   Obtient ou définit les conditions sociétés de la commande.
        /// </summary>
        public string ConditionSociete { get; set; }

        /// <summary>
        ///   Obtient ou définit les conditions de prestation de la commande.
        /// </summary>
        public string ConditionPrestation { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ID du contact pour la commande.
        /// </summary>
        public int? ContactId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du personnel servant de contact pour la commande
        /// </summary>
        public PersonnelEnt Contact { get; set; }

        /// <summary>
        ///   Obtient ou définit le numéro de contact pour la commande
        /// </summary>
        public string ContactTel { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ID du responsable du suivi pour la commande.
        /// </summary>
        public int? SuiviId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du personnel servant au suivi de la commande
        /// </summary>
        public PersonnelEnt Suivi { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ID de la personne ayant valider la commande.
        /// </summary>
        public int? ValideurId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du utilisateur ayant valider la commande
        /// </summary>
        public UtilisateurEnt Valideur { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de validation de la commande.
        /// </summary>
        public DateTime? DateValidation
        {
            get
            {
                return (dateValidation.HasValue) ? DateTime.SpecifyKind(dateValidation.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateValidation = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'ID du saisisseur de la commande.
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du utilisateur ayant saisi la commande
        /// </summary>
        public UtilisateurEnt AuteurCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de saisie de la commande.
        /// </summary>
        public DateTime? DateCreation
        {
            get { return (dateCreation.HasValue) ? DateTime.SpecifyKind(dateCreation.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateCreation = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        ///   Obtient ou définit l'ID de la personne ayant modifier la commande.
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du utilisateur ayant modifier la commande
        /// </summary>
        public UtilisateurEnt AuteurModification { get; set; }

        /// <summary>
        ///  Obtient ou définit la dernière date de modification de la commande sans les millisecondes.
        ///  Important : Sans millisecondes afin d'éviter les differences lors des cast c# => js ou inversement
        /// </summary>
        public DateTime? DateModification
        {
            get { return (dateModification.HasValue) ? DateTime.SpecifyKind(dateModification.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateModification = (value.HasValue) ? new DateTime(value.Value.Ticks - (value.Value.Ticks % TimeSpan.TicksPerSecond), DateTimeKind.Utc) : default(DateTime?); }
        }

        #region Adresses

        /// <summary>
        ///   Obtient ou définit l'Entete de livraison de la commande.
        /// </summary>
        public string LivraisonEntete { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Adresse de livraison de la commande.
        /// </summary>
        public string LivraisonAdresse { get; set; }

        /// <summary>
        ///   Obtient ou définit la ville de livraison de la commande.
        /// </summary>
        public string LivraisonVille { get; set; }

        /// <summary>
        ///   Obtient ou définit le code postal de livraison de la commande.
        /// </summary>
        public string LivraisonCPostale { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du pays de livraison
        /// </summary>
        public int? LivraisonPaysId { get; set; }

        /// <summary>
        ///   Obtient ou définit le pays de livraison
        /// </summary>
        public PaysEnt LivraisonPays { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Adresse de facturation de la commande.
        /// </summary>
        public string FacturationAdresse { get; set; }

        /// <summary>
        ///   Obtient ou définit la ville de facturation de la commande.
        /// </summary>
        public string FacturationVille { get; set; }

        /// <summary>
        ///   Obtient ou définit le code postal de facturation de la commande.
        /// </summary>
        public string FacturationCPostale { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du pays de facturation
        /// </summary>
        public int? FacturationPaysId { get; set; }

        /// <summary>
        ///   Obtient ou définit le pays de facturation
        /// </summary>
        public PaysEnt FacturationPays { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Adresse de facturation de la commande.
        /// </summary>
        public string FournisseurAdresse { get; set; }

        /// <summary>
        ///   Obtient ou définit la ville de Fournisseur de la commande.
        /// </summary>
        public string FournisseurVille { get; set; }

        /// <summary>
        ///   Obtient ou définit le code postal de Fournisseur de la commande.
        /// </summary>
        public string FournisseurCPostal { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du pays du fournisseur
        /// </summary>
        public int? FournisseurPaysId { get; set; }

        /// <summary>
        ///   Obtient ou définit le pays du fournisseur
        /// </summary>
        public PaysEnt FournisseurPays { get; set; }

        #endregion

        /// <summary>
        ///   Obtient ou définit l'ID de la personne ayant supprimer la commande.
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du utilisateur ayant supprimer la commande
        /// </summary>
        public UtilisateurEnt AuteurSuppression { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de suppression de la commande.
        /// </summary>
        public DateTime? DateSuppression
        {
            get { return (dateSuppression.HasValue) ? DateTime.SpecifyKind(dateSuppression.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateSuppression = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        ///   Obtient ou définit la date de clôture de la commande.
        /// </summary>
        public DateTime? DateCloture
        {
            get { return (dateCloture.HasValue) ? DateTime.SpecifyKind(dateCloture.Value, DateTimeKind.Utc) : default(DateTime?); }
            set
            {
                dateCloture = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit le justificatif de la commande.
        /// </summary>
        public string Justificatif { get; set; }

        /// <summary>
        ///   Obtient ou définit les commentaires fournisseur de la commande.
        /// </summary>
        public string CommentaireFournisseur { get; set; }

        /// <summary>
        ///   Obtient ou définit les commentaires internes de la commande.
        /// </summary>
        public string CommentaireInterne { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ID du type de la commande.
        /// </summary>
        public int? TypeId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité type de commande
        /// </summary>
        public CommandeTypeEnt Type { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ID du type de la commande.
        /// </summary>
        public int? DeviseId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité type de commande si la commande peut être visée ou pas
        /// </summary>
        public DeviseEnt Devise { get; set; }

        /// <summary>
        ///   Obtient ou définit l'accord cadre si la commande peut être visée ou pas
        /// </summary>
        public bool AccordCadre { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la commande est manuelle.
        /// </summary>
        public bool CommandeManuelle { get; set; }

        /// <summary>
        ///   Obtient ou définit le numéro externe de la commande.
        /// </summary>
        public string NumeroCommandeExterne { get; set; }

        /// <summary>
        ///   Obtient ou définit le numéro de contrat pour les commandes intérimaires
        /// </summary>
        public string NumeroContratExterne { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la commande peut être visée ou pas
        /// </summary>
        public bool IsVisable { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la commande peut être visée ou pas
        /// </summary>
        public bool IsClotureComptable { get; set; }

        /// <summary>
        ///   Obtient le total de la commande
        /// </summary>
        public decimal MontantHT { get; set; }

        /// <summary>
        ///   Obtient le montant total réceptionné de la commande
        /// </summary>
        public decimal MontantHTReceptionne { get; set; }

        /// <summary>
        ///   Obtient le solde de la commande
        /// </summary>
        public decimal MontantHTSolde { get; set; }

        /// <summary>
        ///   Obtient le solde de la commande
        /// </summary>
        public decimal PourcentageReceptionne { get; set; }

        /// <summary>
        /// Obtient ou définit le montantHT facturé (Somme des montantHT des facturations)
        /// </summary>
        public decimal MontantHTFacture { get; set; }

        /// <summary>
        /// Obtient ou définit le montant facturé
        /// RG_3656_064 : Fonction de calcul du Montant Facturé d’une réception à une date J
        /// ∑ Quantité x PU HT de toutes les Dépenses Achat de type ‘Facture’ + ’Facture Ecart’ + ‘Avoir’ + ‘Avoir Ecart’ associées (via Dépense Parent ID) à cette réception dont la Date Opération est antérieure ou égale à J
        /// </summary>
        public decimal MontantFacture { get; set; }

        /// <summary>
        ///   Obtient une valeur indiquant si la commande est en cours de création
        /// </summary>
        public bool IsCreated => CommandeId == 0;

        /// <summary>
        ///   Obtient une valeur indiquant si la commande est à l'état de brouillon
        /// </summary>
        public bool IsStatutBrouillon => StatutCommande?.Code == StatutCommandeEnt.CommandeStatutBR;

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la commande est à valider
        /// </summary>
        public bool IsStatutAValider => StatutCommande?.Code == StatutCommandeEnt.CommandeStatutAV;

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la commande est validée
        /// </summary>
        public bool IsStatutValidee => StatutCommande?.Code == StatutCommandeEnt.CommandeStatutVA;

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la commande est clôturée
        /// </summary>
        public bool IsStatutCloturee => StatutCommande?.Code == StatutCommandeEnt.CommandeStatutCL;

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la commande manuelle est validée
        /// </summary>
        public bool IsStatutManuelleValidee => StatutCommande?.Code == StatutCommandeEnt.CommandeStatutMVA;

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la commande peut être validée par l'utilisateur courant
        /// </summary>
        public bool IsValidable { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si l'utilisateur connecté a le droit de saisir une commande manuelle
        /// </summary>
        public bool CommandeManuelleAllowed { get; set; }

        /// <summary>
        ///   Obtient ou définit le solde FAR 
        /// </summary>
        public decimal SoldeFar { get; set; }

        /// <summary>
        /// Obtient ou définit une collection de facturation.
        /// </summary>
        public virtual ICollection<FacturationEnt> Facturations { get; set; }

        /// <summary>
        /// Obtient ou définit l'idenfiant du job hangfire.
        /// </summary>
        public string HangfireJobId { get; set; }

        /// <summary>
        ///   Obtient ou définit si la commande est une commande Abonnement ou non
        /// </summary>
        public bool IsAbonnement { get; set; }

        /// <summary>
        ///   Obtient ou définit la durée de l'abonnement
        /// </summary>
        public int? DureeAbonnement { get; set; }

        /// <summary>
        ///   Obtient ou définit la fréquence de l'abonnement (journalier, hebdomadaire, trimestriel, annuel)
        /// </summary>
        public int? FrequenceAbonnement { get; set; }

        /// <summary>
        ///   Obtient ou définit la  date de la première génération d'une réception pour une commande Abonnement
        /// </summary>
        public DateTime? DatePremiereReception
        {
            get { return (dateDerniereReception.HasValue) ? DateTime.SpecifyKind(dateDerniereReception.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateDerniereReception = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        ///   Obtient ou définit la date de la prochaine génération d'une réception pour une commande Abonnement
        /// </summary>
        public DateTime? DateProchaineReception
        {
            get { return (dateProchaineReception.HasValue) ? DateTime.SpecifyKind(dateProchaineReception.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateProchaineReception = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        /// Obtient ou définit l'idenfiant du système externe.
        /// </summary>
        public int? SystemeExterneId { get; set; }

        /// <summary>
        /// Obtient ou définit le système externe.
        /// </summary>
        public SystemeExterneEnt SystemeExterne { get; set; }

        /// <summary>
        ///   Obtient ou définit la commande contrat interimaire
        /// </summary>
        public CommandeContratInterimaireEnt CommandeContratInterimaire { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ancien ID fournisseur de la commande.
        ///   cf. US 5163
        /// </summary>
        public int? OldFournisseurId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité de l'ancien fournisseur de la commande.
        ///   cf. US 5163
        /// </summary>
        public FournisseurEnt OldFournisseur { get; set; }

        /// <summary>
        /// Pièces jointes attachées à la commande
        /// </summary>
        public ICollection<PieceJointeCommandeEnt> PiecesJointesCommande { get; set; }

        /// <summary>
        /// Avis attachés à la commande
        /// </summary>
        public List<AvisCommandeEnt> AvisCommande { get; set; }

        /// <summary>
        ///   Obtient ou définit si la commande contient un matériel externe à pointer       
        /// </summary>
        public bool IsMaterielAPointer { get; set; }

        /// <summary>
        ///   Obtient ou définit si la commande est énergie   
        /// </summary>
        public bool IsEnergie { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique du type d'énergie de la commande 
        /// </summary>
        public int? TypeEnergieId { get; set; }

        /// <summary>
        ///   Obtient ou définit le type d'énergie de la commande
        /// </summary>
        public TypeEnergieEnt TypeEnergie { get; set; }

        /// <summary>
        ///   commande Etat Avalider Provisoire --c'est mauche,il faut le modifier avec la refacto
        /// </summary>
        [NotMapped]
        public bool CommandeAvaliderProvisoire { get; set; }

        /// <summary>
        ///   Supprimer les propriétés de l'objet
        /// </summary>
        public void CleanProperties()
        {
            CI = null;
            Contact = null;
            Suivi = null;
            Fournisseur = null;
            AuteurCreation = null;
            Type = null;
            Valideur = null;
            AuteurModification = null;
            AuteurSuppression = null;
            StatutCommande = null;
            Devise = null;
            LivraisonPays = null;
            FournisseurPays = null;
            FacturationPays = null;
            OldFournisseur = null;
            TypeEnergie = null;
            PiecesJointesCommande = null;

            if (Lignes != null)
            {
                foreach (CommandeLigneEnt commandeLigne in Lignes)
                {
                    commandeLigne.CleanProperties();
                }
            }
        }
    }
}
