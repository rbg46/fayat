using System;
using Fred.Web.Models.Referential;
using Fred.Web.Models.ReferentielFixe;
using Fred.Web.Shared.Models.Commande;

namespace Fred.Web.Models
{
    /// <summary>
    /// Représente une ligne de commande
    /// </summary>
    public class CommandeLigneLightModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une ligne de commande.
        /// </summary>
        public int CommandeLigneId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant temporaire unique d'une commande.
        /// Cette propriété est utilisé pour ajouter des lignes de commande dans l'interface.
        /// </summary>
        public int CommandeLigneIdTemp { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'une ligne de commande.
        /// </summary>
        public int CommandeId { get; set; }

        /// <summary>
        /// Obtient ou définit la commande dont dépend une ligne de commande.
        /// </summary>
        public CommandeLightModel Commande { get; set; }

        /// <summary>
        /// Obtient ou définit l'Id de la tâche d'une ligne de commande.
        /// </summary>
        public int? TacheId { get; set; }

        /// <summary>
        /// Obtient ou définit l'objet tâche d'une ligne de commande.
        /// </summary>
        public TacheModel Tache { get; set; }

        /// <summary>
        /// Obtient ou définit l'Id de la ressource d'une ligne de commande.
        /// </summary>
        public int? RessourceId { get; set; }

        /// <summary>
        /// Obtient ou définit l'objet ressource d'une ligne de commande.
        /// </summary>
        public RessourceModel Ressource { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des réceptions d'une ligne de commande
        /// </summary>
        public DepenseAchatLightModel[] DepensesReception { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'une ligne de commande.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit le montant d'une ligne de commande.
        /// </summary>
        public decimal Quantite { get; set; }

        /// <summary>
        /// Obtient ou définit le prix unitaire d'une ligne de commande.
        /// Formatage à 2 décimales fait dans le template :
        /// Basculer les codes de champs --> \m \# "# ##0,00"
        /// </summary>
        public decimal PUHT { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'unité
        /// </summary>
        public int? UniteId { get; set; }

        /// <summary>
        /// Obtient ou définit l'unité d'une ligne de commande.
        /// </summary>
        public UniteModel Unite { get; set; }

        /// <summary>
        ///   Obtient le code de l'unité
        /// </summary>
        /// <remarks>EXTRACT EXCEL</remarks>
        public string CodeUnite
        {
            get
            {
                return Unite != null ? Unite.Code : string.Empty;
            }
        }

        /// <summary>
        /// Obtient ou définit le montant d'une ligne de commande.
        /// Formatage à 2 décimales fait dans le template :
        /// Basculer les codes de champs --> \m \# "# ##0,00"
        /// </summary>
        public decimal MontantHT { get; set; }

        /// <summary>
        /// Obtient le montant total réceptionné d'une ligne de commande
        /// </summary>
        public decimal MontantHTReceptionne { get; set; }

        /// <summary>
        /// Obtient la quantité réceptionnée de la ligne de commande
        /// </summary>
        public decimal QuantiteReceptionnee { get; set; }

        /// <summary>
        /// Obtient le solde de la commande
        /// </summary>
        public decimal MontantHTSolde { get; set; }

        /// <summary>
        /// Obtient la devise de la ligne de commande (renvoie la devise de l'en-tête de la commande)
        /// </summary>
        public DeviseModel Devise { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si une ligne de commande a été créée.
        /// </summary>
        /// <value>
        /// <c>true</c> si une ligne de commande a été créé; sinon, <c>false</c>.
        /// </value>
        public bool IsCreated { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si une ligne de commande a été supprimée.
        /// </summary>
        /// <value>
        /// <c>true</c> si une ligne de commande a été supprimée; sinon, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si une ligne de commande a été modifiée.
        /// </summary>
        /// <value>
        ///   <c>true</c> si une ligne de commande a été modifiée; sinon, <c>false</c>.
        /// </value>    
        public bool IsUpdated { get; set; }

        /// <summary>
        ///   Obtient ou définit la somme des soldes FAR de toutes les réceptions 
        /// </summary>    
        public decimal SoldeFar { get; set; }

        /// <summary>
        /// Obtient ou définit le montant facturé
        /// </summary>    
        public decimal MontantHTFacture { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant facturé
        ///   ∑ Quantité x PU HT de toutes les Dépenses Achat de type ‘Facture’ + ’Facture Ecart’ + ‘Avoir’ + ‘Avoir Ecart’ associées (via Dépense Parent ID) à cette réception dont la Date Opération est antérieure ou égale à J        
        /// </summary>        
        public decimal MontantFacture { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ID du saisisseur de la ligne de commande.
        /// </summary>    
        public int? AuteurCreationId { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de saisie de la ligne de commande.
        /// </summary>    
        public DateTime? DateCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ID de la personne ayant modifier la ligne de commande.
        /// </summary>    
        public int? AuteurModificationId { get; set; }

        /// <summary>
        ///   Obtient ou définit la dernière date de modification de la ligne de commande.
        /// </summary>    
        public DateTime? DateModification { get; set; }

        /// <summary>
        /// Obtient ou définit les informations de l'avenant ou null si la ligne n'est pas un avenant.
        /// </summary>
        public CommandeAvenantLoad.LigneModel AvenantLigne { get; set; }

        /// <summary>
        /// Obtient ou définit le numéro de la ligne dans la commande
        /// </summary>
        public int? NumeroLigne { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique du matériel externe
        /// </summary>
        public int? MaterielId { get; set; }

        /// <summary>
        /// Obtient ou définit si la ligne est verouillée
        /// </summary>
        public bool IsVerrou { get; set; }
    }
}
