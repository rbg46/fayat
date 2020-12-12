using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Fred.Web.Models.CI;
using Fred.Web.Models.Personnel;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Utilisateur;

namespace Fred.Web.Dtos.Mobile.Commande
{
    /// <summary>
    /// Représente une commande
    /// </summary>
    public class CommandeDto : DtoBase
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une commande.
        /// </summary>
        public int? CommandeId { get; set; }

        /// <summary>
        /// Obtient ou définit le numéro d'une commande.
        /// </summary>
        //[StringLength(20, ErrorMessage = "Numero cannot be longer than 20 characters.")]
        public string Numero { get; set; }

        /// <summary>
        /// Obtient ou définit l'ID CI de la commande.
        /// </summary>
        [Required]
        public int? CiId { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé de la commande.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit la date de la commande.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Obtient ou définit le type de commande
        /// </summary>
        [Required]
        public int? TypeId { get; set; }

        /// <summary>
        /// Obtient ou définit l'ID du type de la commande.
        /// </summary>
        public int? DeviseId { get; set; }

        /// <summary>
        /// Obtient ou définit l'ID fournisseur de la commande.
        /// </summary>
        public int? FournisseurId { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des lignes d'une commande
        /// </summary>s
        public CommandeLigneDto[] Lignes { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des visas d'une commande
        /// </summary>
        public VisaDto[] Visas { get; set; }

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
        /// Obtient ou définit une valeur indiquant si la commande contient la conduite MO.
        /// </summary>
        public bool MOConduite { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la commande contient l'entretien journalier.
        /// </summary>
        public bool EntretienMecanique { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la commande contient l'entretien journalier.
        /// </summary>
        public bool EntretienJournalier { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la commande contient le carburant.
        /// </summary>
        public bool Carburant { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la commande contient le lubrifiant.
        /// </summary>
        public bool Lubrifiant { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la commande contient les frais d'amortissement.
        /// </summary>
        public bool FraisAmortissement { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si la commande contient les frais d'assurance.
        /// </summary>
        public bool FraisAssurance { get; set; }

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
        public PersonnelModel Contact { get; set; } = null;

        /// <summary>
        /// Obtient ou définit le numéro de contact pour la commande
        /// </summary>
        public string ContactTel { get; set; }

        /// <summary>
        /// Obtient ou définit l'ID du responsable du suivi pour la commande.
        /// </summary>
        public int? SuiviId { get; set; }

        /// <summary>
        /// Obtient ou définit l'ID de la personne ayant validé la commande.
        /// </summary>
        public int? ValideurId { get; set; }

        /// <summary>
        /// Obtient ou définit la date de validation de la commande.
        /// </summary>
        public DateTime? DateValidation { get; set; }

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
        /// Obtient ou définit la date de suppression de la commande.
        /// </summary>
        public DateTime? DateSuppression { get; set; }

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
        /// Obtient le solde de la commande
        /// </summary>
        public decimal PourcentageReceptionne { get; set; }

        /// <summary>
        /// Obtient  une valeur indiquant si la commande est à l'état de brouillon
        /// </summary>
        public bool IsStatutBrouillon { get; set; }

        /// <summary>
        /// Obtient  une valeur indiquant si la commande est à valider
        /// </summary>
        public bool IsStatutAValider { get; set; }

        /// <summary>
        /// Obtient  une valeur indiquant si la commande est validée
        /// </summary>
        public bool IsStatutValidee { get; set; }

        /// <summary>
        /// Obtient  une valeur indiquant si la commande est clôturée
        /// </summary>
        public bool IsStatutCloturree { get; set; }

        /// <summary>
        /// Obtient  une valeur indiquant si la commande est complète
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Obtient  une valeur indiquant si la commande est validable par l'utilisateur courant
        /// </summary>
        public bool IsValidable { get; set; }

        /// <summary>
        /// Obtient la valeur qui définit si la commande peut être visée ou pas
        /// </summary>
        public bool IsVisable { get; set; }
    }

}