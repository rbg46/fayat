﻿using System;
using System.Collections.Generic;
using System.Drawing;
using Fred.Web.Models.Commande;
using Fred.Web.Models.Referential;
using Fred.Web.Shared.Models;
using Fred.Web.Shared.Models.Avis;
using Fred.Web.Shared.Models.Commande;
using Fred.Web.Shared.Models.Import;
using Fred.Web.Shared.Models.Referential;

namespace Fred.Web.Models
{
    /// <summary>
    /// Représente un commande
    /// </summary>
    public class CommandeLightModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une commande.
        /// </summary>
        public int? CommandeId { get; set; }

        /// <summary>
        /// Préfixe calculé du numéro de commande manuelle
        /// </summary>
        public string PatternNumeroCommandeManuelle { get; set; }

        /// <summary>
        /// Obtient ou définit le numéro d'une commande.
        /// </summary>
        //[StringLength(20, ErrorMessage = "Numero cannot be longer than 20 characters.")]
        public string Numero { get; set; }

        /// <summary>
        /// Obtient ou définit le numéro d'une commande externe.
        /// </summary>
        //[StringLength(20, ErrorMessage = "Numero cannot be longer than 20 characters.")]
        public string NumeroCommandeExterne { get; set; }

        /// <summary>
        /// Obtient ou définit l'ID CI de la commande.
        /// </summary>
        public int? CiId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité CI reliée à la commande
        /// </summary>
        public CILightModel CI { get; set; } = null;

        /// <summary>
        /// Obtient ou définit le libellé de la commande.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit la date de la commande.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Obtient ou définit la date de la commande.
        /// </summary>    
        public string DateFormat
        {
            get
            {
                return this.Date.ToString("dd/MM/yyyy");
            }
        }

        /// <summary>
        /// Obtient ou définit le type de commande
        /// </summary>
        public int? TypeId { get; set; }

        /// <summary>
        /// Obtient ou définit le type de commande
        /// </summary>
        public CommandeTypeModel Type { get; set; } = null;

        /// <summary>
        /// Obtient ou définit l'ID fournisseur de la commande.
        /// </summary>
        public int? FournisseurId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité du fournisseur de la commande.
        /// </summary>
        public FournisseurLightModel Fournisseur { get; set; } = null;

        /// <summary>
        /// Fournisseur provisoire
        /// </summary>
        public string FournisseurProvisoire { get; set; }

        /// <summary>
        /// Obtient ou définit le devise de commande
        /// </summary>
        public int? DeviseId { get; set; }

        /// <summary>
        /// Obtient ou définit le devise de commande
        /// </summary>
        public DeviseModel Devise { get; set; } = null;

        /// <summary>
        /// Obtient ou définit la liste des lignes d'une commande
        /// </summary>s
        public CommandeLigneLightModel[] Lignes { get; set; }

        /// <summary>
        /// Obtient le nombre de réceptions relatives aux lignes de la commande courante.
        /// </summary>
        public int NbReceptions
        {
            get
            {
                int nbReceptions = 0;

                if (this.Lignes != null)
                {
                    foreach (CommandeLigneLightModel ligne in this.Lignes)
                    {
                        if (ligne.DepensesReception != null)
                        {
                            nbReceptions += ligne.DepensesReception.Length;
                        }
                    }
                }

                return nbReceptions;
            }
        }

        /// <summary>
        /// Obtient ou définit le délai de livraison de la commande.
        /// </summary>
        public string DelaiLivraison { get; set; }

        /// <summary>
        /// Obtient ou définit le date de mise à disposition de la commande.
        /// </summary>
        public DateTime? DateMiseADispo { get; set; }

        /// <summary>
        /// Obtient ou définit l'ID du statut de la commande.
        /// </summary>
        public int? StatutCommandeId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité du statut relié à la commande
        /// </summary>
        public StatutCommandeModel StatutCommande { get; set; } = null;

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la commande contient la conduite MO.
        /// </summary>
        public bool MOConduite { get; set; }

        /// <summary>
        /// Obtient ou définit le format spécifique pour la conduite MO pour le template Word
        /// </summary>
        public string MOConduiteOui
        {
            get
            {
                return (this.MOConduite) ? "X" : "";
            }
        }

        /// <summary>
        /// Obtient ou définit le format spécifique pour la conduite MO pour le template Word
        /// </summary>
        public string MOConduiteNon
        {
            get
            {
                return (this.MOConduite) ? "" : "X";
            }
        }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la commande contient l'entretien journalier.
        /// </summary>
        public bool EntretienMecanique { get; set; }

        /// <summary>
        /// Obtient ou définit le format spécifique pour l'entretien mécanique pour le template Word
        /// </summary>
        public string EntretienMecaniqueOui
        {
            get
            {
                return (this.EntretienMecanique) ? "X" : "";
            }
        }

        /// <summary>
        /// Obtient ou définit le format spécifique pour l'entretien mécanique pour le template Word
        /// </summary>
        public string EntretienMecaniqueNon
        {
            get
            {
                return (this.EntretienMecanique) ? "" : "X";
            }
        }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la commande contient l'entretien journalier.
        /// </summary>
        public bool EntretienJournalier { get; set; }

        /// <summary>
        /// Obtient ou définit le format spécifique pour l'entretien journalier pour le template Word
        /// </summary>
        public string EntretienJournalierOui
        {
            get
            {
                return (this.EntretienJournalier) ? "X" : "";
            }
        }

        /// <summary>
        /// Obtient ou définit le format spécifique pour l'entretien journalier pour le template Word
        /// </summary>
        public string EntretienJournalierNon
        {
            get
            {
                return (this.EntretienJournalier) ? "" : "X";
            }
        }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la commande contient le carburant.
        /// </summary>
        public bool Carburant { get; set; }

        /// <summary>
        /// Obtient ou définit le format spécifique pour le carburant pour le template Word
        /// </summary>
        public string CarburantOui
        {
            get
            {
                return (this.Carburant) ? "X" : "";
            }
        }

        /// <summary>
        /// Obtient ou définit le format spécifique pour le carburant pour le template Word
        /// </summary>
        public string CarburantNon
        {
            get
            {
                return (this.Carburant) ? "" : "X";
            }
        }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la commande contient le lubrifiant.
        /// </summary>
        public bool Lubrifiant { get; set; }

        /// <summary>
        /// Obtient ou définit le format spécifique pour le lubrifiant pour le template Word
        /// </summary>
        public string LubrifiantOui
        {
            get
            {
                return (this.Lubrifiant) ? "X" : "";
            }
        }

        /// <summary>
        /// Obtient ou définit le format spécifique pour le lubrifiant pour le template Word
        /// </summary>
        public string LubrifiantNon
        {
            get
            {
                return (this.Lubrifiant) ? "" : "X";
            }
        }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la commande contient les frais d'amortissement.
        /// </summary>
        public bool FraisAmortissement { get; set; }

        /// <summary>
        /// Obtient ou définit le format spécifique pour les frais d'amortissement pour le template Word
        /// </summary>
        public string FraisAmortissementOui
        {
            get
            {
                return (this.FraisAmortissement) ? "X" : "";
            }
        }

        /// <summary>
        /// Obtient ou définit le format spécifique pour les frais d'amortissement pour le template Word
        /// </summary>
        public string FraisAmortissementNon
        {
            get
            {
                return (this.FraisAmortissement) ? "" : "X";
            }
        }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la commande contient les frais d'assurance.
        /// </summary>
        public bool FraisAssurance { get; set; }

        /// <summary>
        /// Obtient ou définit le format spécifique pour les frais d'assurance pour le template Word
        /// </summary>
        public string FraisAssuranceOui
        {
            get
            {
                return (this.FraisAssurance) ? "X" : "";
            }
        }

        /// <summary>
        /// Obtient ou définit le format spécifique pour les frais d'assurance pour le template Word
        /// </summary>
        public string FraisAssuranceNon
        {
            get
            {
                return (this.FraisAssurance) ? "" : "X";
            }
        }

        /// <summary>
        /// Obtient ou définit les conditions sociétés de la commande.
        /// </summary>
        public string ConditionSociete { get; set; }

        /// <summary>
        /// Obtient ou définit les conditions de prestation de la commande.
        /// </summary>
        public string ConditionPrestation { get; set; }

        /// <summary>
        /// Obtient ou définit l'ID du contact pour la commande.
        /// </summary>
        public int? ContactId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité du membre du personnel servant de contact pour la commande
        /// </summary>
        public PersonnelLightModel2 Contact { get; set; } = null;

        /// <summary>
        /// Obtient ou définit le numéro de contact pour la commande
        /// </summary>
        public string ContactTel { get; set; }

        /// <summary>
        /// Obtient ou définit l'ID du responsable du suivi pour la commande.
        /// </summary>
        public int? SuiviId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité du membre du personnel servant au suivi de la commande
        /// </summary>
        public PersonnelLightModel2 Suivi { get; set; } = null;

        /// <summary>
        /// Obtient ou définit l'ID du saisisseur de la commande.
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité du membre de l'utilisateur ayant saisi la commande
        /// </summary>
        public UtilisateurLightModel2 AuteurCreation { get; set; }

        /// <summary>
        /// Obtient ou définit l'ID de la personne ayant validé la commande.
        /// </summary>
        public int? ValideurId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité du membre de l'utilisateur ayant validé la commande
        /// </summary>
        public UtilisateurLightModel2 Valideur { get; set; } = null;

        /// <summary>
        /// Obtient ou définit la date de saisie de la commande.
        /// </summary>
        public DateTime? DateCreation { get; set; }

        /// <summary>
        /// Obtient ou définit la dernière date de modification de la commande.
        /// </summary>
        public DateTime? DateModification { get; set; }

        /// <summary>
        /// Obtient ou définit l'ID de la personne ayant modifier la commande.
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité du membre de l'utilisateur ayant modifier la commande
        /// </summary>
        public UtilisateurLightModel2 AuteurModification { get; set; } = null;

        /// <summary>
        /// Obtient ou définit la date de validation de la commande.
        /// </summary>
        public DateTime? DateValidation { get; set; }

        /// <summary>
        /// Obtient ou définit l'Entete de livraison de la commande.
        /// </summary>
        public string LivraisonEntete { get; set; }

        /// <summary>
        /// Obtient ou définit l'Adresse de livraison de la commande.
        /// </summary>
        public string LivraisonAdresse { get; set; }

        /// <summary>
        /// Obtient ou définit la ville de livraison de la commande.
        /// </summary>
        public string LivraisonVille { get; set; }

        /// <summary>
        /// Obtient ou définit le code postale de livraison de la commande.
        /// </summary>
        public string LivraisonCPostale { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du pays de livraison
        /// </summary>    
        public int? LivraisonPaysId { get; set; }

        /// <summary>
        ///   Obtient ou définit le pays de livraison
        /// </summary>    
        public PaysModel LivraisonPays { get; set; }

        /// <summary>
        /// Obtient ou définit le nom de la société gérante du CI de la commande.
        /// </summary>
        public string FacturationDataGridColumn { get; set; }

        /// <summary>
        /// Obtient ou définit l'Adresse de facturation de la commande.
        /// </summary>
        public string FacturationAdresse { get; set; }

        /// <summary>
        /// Obtient ou définit la ville de facturation de la commande.
        /// </summary>
        public string FacturationVille { get; set; }

        /// <summary>
        /// Obtient ou définit le code postale de facturation de la commande.
        /// </summary>
        public string FacturationCPostale { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du pays de facturation
        /// </summary>    
        public int? FacturationPaysId { get; set; }

        /// <summary>
        ///   Obtient ou définit le pays de facturation
        /// </summary>    
        public PaysModel FacturationPays { get; set; }

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
        public PaysModel FournisseurPays { get; set; }

        /// <summary>
        /// Obtient ou définit la date de suppression de la commande.
        /// </summary>
        public DateTime? DateSuppression { get; set; }

        /// <summary>
        /// Obtient ou définit l'ID de la personne ayant supprimer la commande.
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité du membre de l'utilisateur ayant supprimer la commande
        /// </summary>
        public UtilisateurLightModel2 AuteurSuppression { get; set; } = null;

        /// <summary>
        /// Obtient ou définit la date de clôture de la commande.
        /// </summary>
        public DateTime? DateCloture { get; set; }

        /// <summary>
        /// Obtient ou définit le justificatif de la commande.
        /// </summary>
        public string Justificatif { get; set; }

        /// <summary>
        /// Obtient ou définit la commande manuelle.
        /// </summary>
        public bool CommandeManuelle { get; set; }

        /// <summary>
        /// Obtient ou définit les commentaires fournisseur de la commande.
        /// </summary>
        public string CommentaireFournisseur { get; set; }

        /// <summary>
        /// Obtient ou définit les commentaires internes de la commande.
        /// </summary>
        public string CommentaireInterne { get; set; }

        /// <summary>
        /// Obtient ou définit le montant total de la commande
        /// Formatage à 2 décimales fait dans le template :
        /// Basculer les codes de champs --> \m \# "# ##0,00"
        /// </summary>
        public decimal MontantHT { get; set; }

        /// <summary>
        /// Obtient ou définit le montant total réceptionné de la commande
        /// </summary>
        public decimal MontantHTReceptionne { get; set; }

        /// <summary>
        /// Obtient ou définit le solde de la commande
        /// </summary>
        public decimal MontantHTSolde { get; set; }

        /// <summary>
        /// Obtient ou définit le montant facturé
        /// </summary>
        public decimal MontantHTFacture { get; set; }

        /// <summary>
        ///   Obtient ou définit le solde FAR 
        /// </summary>
        public decimal SoldeFar { get; set; }

        /// <summary>
        /// Obtient ou définit l'accord cadre
        /// </summary>
        public bool AccordCadre { get; set; }

        /// <summary>
        /// Obtient le solde de la commande
        /// </summary>
        public decimal PourcentageReceptionne { get; set; }

        /// <summary>
        /// Obtient  une valeur indiquant si la commande est en cours de création
        /// </summary>
        public bool IsCreated { get; set; }

        /// <summary>
        /// Obtient  une valeur indiquant si la commande est à l'état de brouillon
        /// </summary>
        public bool IsStatutBrouillon { get; set; }

        /// <summary>
        /// Obtient  une valeur indiquant si la commande est à valider
        public bool IsStatutAValider { get; set; }

        /// <summary>
        /// Obtient  une valeur indiquant si la commande est validée
        /// </summary>
        public bool IsStatutValidee { get; set; }

        /// <summary>
        /// Obtient  une valeur indiquant si la commande est clôturée
        /// </summary>
        public bool IsStatutCloturee { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la commande manuelle est validée
        /// </summary>
        public bool IsStatutManuelleValidee { get; set; }

        /// <summary>
        /// Obtient  une valeur indiquant si la commande est validable par l'utilisateur courant
        /// </summary>
        public bool IsValidable { get; set; }

        /// <summary>
        /// Obtient la valeur qui définit si la commande peut être visée ou pas
        /// </summary>
        public bool IsVisable { get; set; }

        /// <summary>
        /// Erreurs métier détéctées suite à la vérification de la commande
        /// </summary>
        public string[] erreurs { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si l'utilisateur connecté a le droit de saisir une commande manuelle
        /// </summary>
        public bool CommandeManuelleAllowed { get; set; }

        /// <summary>
        /// Signature scannée du salarié
        /// </summary>
        public Image Signature { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du job hangfire lors de l'envoi de la commande vers Storm
        ///   Si renseigné, commande envoyée avec succès, sinon échec
        /// </summary>
        public string HangfireJobId { get; set; }

        /// <summary>
        ///   Obtient ou définit si la commande est une commande Abonnement ou non
        /// </summary>    
        public bool IsAbonnement { get; set; }

        /// <summary>
        ///   Obtient ou définit si la commande est une commande avec matériel à pointer ou non
        /// </summary>    
        public bool IsMaterielAPointer { get; set; }

        /// <summary>
        ///   Obtient ou définit si la commande est energie
        /// </summary>    
        public bool IsEnergie { get; set; }

        /// <summary>
        ///   Obtient ou définit la durée de l'abonnement
        /// </summary>    
        public int? DureeAbonnement { get; set; }

        /// <summary>
        ///   Obtient ou définit la fréquence de l'abonnement (journalier, hebdomadaire, trimestriel, annuel)
        /// </summary>    
        public int? FrequenceAbonnement { get; set; }

        /// <summary>
        ///    Obtient ou définit la  date de la première génération d'une réception pour une commande Abonnement
        /// </summary>
        public DateTime? DatePremiereReception { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de la prochaine génération d'un réception pour une commande Abonnement
        /// </summary>
        public DateTime? DateProchaineReception { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant facturé
        ///   ∑ Quantité x PU HT de toutes les Dépenses Achat de type ‘Facture’ + ’Facture Ecart’ + ‘Avoir’ + ‘Avoir Ecart’ associées (via Dépense Parent ID) à cette réception dont la Date Opération est antérieure ou égale à J        
        /// </summary>        
        public decimal MontantFacture { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des lignes des avenants de commande
        /// </summary>s
        public CommandeAvenantLoad.LigneModel[] AvenantLignes { get; set; }


        /// <summary>
        /// Le système externe utilisé si la commande provient d'un import.
        /// </summary>
        public SystemeExterneLightModel SystemeExterne { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ID fournisseur de la commande.
        /// </summary>        
        public int? OldFournisseurId { get; set; }

        /// <summary>
        /// Obtient ou définit le code et libellé de l'ancien fournisseur de la commande
        /// </summary>
        public string OldFournisseurCodeLibelle { get; set; }

        /// <summary>
        /// Obtient ou définit l'id de l'agence fournisseur sélectionnée
        /// </summary>
        public int? AgenceId { get; set; }

        /// <summary>
        /// Obtient ou définit l'agence fournisseur sélectionnée
        /// </summary>
        public AgenceModel Agence { get; set; }

        /// <summary>
        /// Avis attachés à la commande
        /// </summary>
        public List<AvisCommandeModel> AvisCommande { get; set; }

    }
}
