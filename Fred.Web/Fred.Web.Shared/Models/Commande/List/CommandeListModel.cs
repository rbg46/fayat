using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Web.Models.Commande;
using Fred.Web.Models.Utilisateur;
using Fred.Web.Shared.Models.PieceJointe;
using Newtonsoft.Json;

namespace Fred.Web.Shared.Models.Commande.List
{
    public class CommandeListModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une commande.
        /// </summary>
        public int? CommandeId { get; set; }

        /// <summary>
        /// Obtient ou définit le numéro d'une commande.
        /// </summary>   
        public string Numero { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité CI reliée à la commande
        /// </summary>
        public CIForCommandeListModel CI { get; set; } = null;

        /// <summary>
        /// Obtient ou définit le libellé de la commande.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit la date de la commande.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité du fournisseur de la commande.
        /// </summary>
        public FournisseurForCommandeListModel Fournisseur { get; set; } = null;

        /// <summary>
        /// Obtient ou définit le devise de commande
        /// </summary>
        public DeviseForCommandeListModel Devise { get; set; } = null;

        /// <summary>
        /// Obtient ou définit la liste des lignes d'une commande
        /// </summary>s
        [JsonIgnore]
        public CommandeLigneModel[] Lignes { get; set; }

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
                    foreach (CommandeLigneModel ligne in this.Lignes)
                    {
                        if (ligne.DepensesReception != null)
                        {
                            nbReceptions += ligne.DepensesReception.Count();
                        }
                    }
                }
                return nbReceptions;
            }
        }

        /// <summary>
        /// Obtient ou définit l'entité du statut relié à la commande
        /// </summary>
        public StatutCommandeModel StatutCommande { get; set; } = null;

        /// <summary>
        /// Obtient ou définit l'entité du membre de l'utilisateur ayant saisi la commande
        /// </summary>
        [JsonIgnore]
        public UtilisateurModel AuteurCreation { get; set; }

        /// <summary>
        /// Obtient la concaténation du nom et du prénom du saisisseur
        /// </summary>
        public string SaisisseurDataGridColumn
        {
            get
            {
                return (this.AuteurCreation != null) ? this.AuteurCreation.PrenomNom : string.Empty;
            }
        }

        /// <summary>
        /// Obtient ou définit l'entité du membre de l'utilisateur ayant validé la commande
        /// </summary>
        [JsonIgnore]
        public UtilisateurModel Valideur { get; set; } = null;

        /// <summary>
        /// Obtient la concaténation du nom et du prénom de l'utilisateur ayant validé la commande
        /// </summary>
        public string ValideurDataGridColumn
        {
            get
            {
                return (this.Valideur != null) ? this.Valideur.PrenomNom : string.Empty;
            }
        }

        /// <summary>
        /// Obtient ou définit la date de saisie de la commande.
        /// </summary>
        public DateTime? DateCreation { get; set; }

        /// <summary>
        /// Obtient ou définit la dernière date de modification de la commande.
        /// </summary>
        public DateTime? DateModification { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité du membre de l'utilisateur ayant modifier la commande
        /// </summary>
        [JsonIgnore]
        public UtilisateurModel AuteurModification { get; set; } = null;

        /// <summary>
        /// Obtient la concaténation du nom et du prénom du saisisseur
        /// </summary>
        public string AuteurModificationDataGridColumn
        {
            get
            {
                return (this.AuteurModification != null) ? this.AuteurModification.PrenomNom : string.Empty;
            }
        }

        /// <summary>
        /// Obtient ou définit la date de validation de la commande.
        /// </summary>
        public DateTime? DateValidation { get; set; }

        /// <summary>
        /// Obtient ou définit la commande manuelle.
        /// </summary>
        public bool CommandeManuelle { get; set; }

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
        /// Obtient le solde de la commande
        /// </summary>
        public decimal PourcentageReceptionne { get; set; }

        /// <summary>
        /// Obtient ou définit le montant facturé
        /// </summary>
        public decimal MontantHTFacture { get; set; }

        /// <summary>
        ///   Obtient ou définit le solde FAR 
        /// </summary>
        public decimal SoldeFar { get; set; }

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
        ///   Obtient ou définit si la commande est une commande Abonnement ou non
        /// </summary>    
        public bool IsAbonnement { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant facturé
        ///   ∑ Quantité x PU HT de toutes les Dépenses Achat de type ‘Facture’ + ’Facture Ecart’ + ‘Avoir’ + ‘Avoir Ecart’ associées (via Dépense Parent ID) à cette réception dont la Date Opération est antérieure ou égale à J        
        /// </summary>        
        public decimal MontantFacture { get; set; }

        /// <summary>
        /// Obtient ou définit le numéro d'une commande externe.
        /// </summary>
        public string NumeroCommandeExterne { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ID de la pièce jointe de la commande.
        /// </summary> 
        public int? PieceJointeId { get; set; }

        /// <summary>
        /// Pièces jointes attachées à la commande
        /// </summary>
        public PieceJointeCommandeModel[] PiecesJointesCommande { get; set; }

        /// <summary>
        /// Obtient ou définit si la commande contient un materiel externe à pointer
        /// </summary>
        public bool IsMaterielAPointer { get; set; }

        /// <summary>
        /// Obtient ou définit si la commande est energie
        /// </summary>
        public bool IsEnergie { get; set; }
    }
}
