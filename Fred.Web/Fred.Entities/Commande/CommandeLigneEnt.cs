using System;
using System.Collections.Generic;
using Fred.Entities.Depense;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Utilisateur;

namespace Fred.Entities.Commande
{
    /// <summary>
    ///   Représente une ligne de commande.
    /// </summary>
    public class CommandeLigneEnt
    {
        private DateTime? dateCreation;
        private DateTime? dateModification;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une ligne de commande.
        /// </summary>
        public int CommandeLigneId { get; set; }

        /// <summary>
        ///   Obtient ou définit la commande dont dépend une ligne de commande.
        /// </summary>
        public CommandeEnt Commande { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'une ligne de commande.
        /// </summary>
        public int CommandeId { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'une ligne de commande.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Id de la tâche d'une ligne de commande.
        /// </summary>
        public int? TacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'objet tache
        /// </summary>
        public TacheEnt Tache { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Id de la ressource d'une ligne de commande.
        /// </summary>
        public int? RessourceId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'objet ressource
        /// </summary>
        public RessourceEnt Ressource { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant d'une ligne de commande.
        /// </summary>
        public decimal Quantite { get; set; }

        /// <summary>
        ///   Obtient ou définit le prix unitaire d'une ligne de commande.
        /// </summary>
        public decimal PUHT { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'unité
        /// </summary>
        public int? UniteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'unité
        /// </summary>
        public UniteEnt Unite { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ID du saisisseur de la ligne de commande.
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du utilisateur ayant saisi la ligne de commande
        /// </summary>
        public UtilisateurEnt AuteurCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de saisie de la ligne de commande.
        /// </summary>
        public DateTime? DateCreation
        {
            get { return (dateCreation.HasValue) ? DateTime.SpecifyKind(dateCreation.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateCreation = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        ///   Obtient ou définit l'ID de la personne ayant modifier la ligne de commande.
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du utilisateur ayant modifier la commande
        /// </summary>
        public UtilisateurEnt AuteurModification { get; set; }

        /// <summary>
        ///   Obtient ou définit la dernière date de modification de la ligne de commande.
        /// </summary>
        public DateTime? DateModification
        {
            get { return (dateModification.HasValue) ? DateTime.SpecifyKind(dateModification.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateModification = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        ///   Obtient le montant hors taxe
        /// </summary>
        public decimal MontantHT { get; set; } //// => Quantite * PUHT;

        /// <summary>
        ///   Obtient ou définit la liste de toute (y compris les réceptions supprimées logiquement) les dépenses associées à
        ///   cette ligne de commande.
        /// </summary>
        public ICollection<DepenseAchatEnt> AllDepenses { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des dépenses de type Réception
        /// </summary>
        public IEnumerable<DepenseAchatEnt> DepensesReception { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des dépenses de type Facture
        /// </summary>
        public IEnumerable<DepenseAchatEnt> DepensesFacture { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des dépenses de type FactureEcart
        /// </summary>
        public IEnumerable<DepenseAchatEnt> DepensesFactureEcart { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des dépenses de type Far (Extourne ou Ajustement FAR)
        /// </summary>
        public IEnumerable<DepenseAchatEnt> DepensesFar { get; set; }

        /// <summary>
        ///   Obtient le montant total réceptionné d'une ligne de commande
        /// </summary>
        public decimal MontantHTReceptionne { get; set; }

        /// <summary>
        ///   Obtient la quantité réceptionnée de la ligne de commande
        /// </summary>
        public decimal QuantiteReceptionnee { get; set; }

        /// <summary>
        ///   Obtient le solde de la commande
        /// </summary>
        public decimal MontantHTSolde { get; set; }

        /// <summary>
        ///   Obtient la devise de la ligne commande (renvoie la devise de l'entête de la commande)
        /// </summary>
        public DeviseEnt Devise
        {
            get
            {
                if (Commande != null)
                {
                    return Commande.Devise;
                }

                return null;
            }
        }

        // CECI EST DU CODE METIER CELA N A RIEN A FAIRE ICI : FAIRE UNE METHODE D EXTENSION
        /// <summary>
        ///   Obtient ou définit une valeur indiquant si une ligne de commande a été créée.
        /// </summary>
        /// <value>
        ///   <c>true</c> si une ligne de commande a été créé; sinon, <c>false</c>.
        /// </value>
        public bool IsCreated => CommandeLigneId == 0;

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si une ligne de commande a été supprimée.
        /// </summary>
        /// <value>
        ///   <c>true</c> si une ligne de commande a été supprimée; sinon, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si une ligne de commande a été modifiée.
        /// </summary>
        /// <value>
        ///   <c>true</c> si une ligne de commande a été modifiée; sinon, <c>false</c>.
        /// </value>
        public bool IsUpdated { get; set; }

        // CECI EST DU CODE METIER CELA N A RIEN A FAIRE ICI : FAIRE UNE METHODE D EXTENSION
        /// <summary>
        ///   Obtient une concaténation du numéro de la commande et du libellé de la ligne de commande
        /// </summary>
        public string NumeroLibelle => Commande != null ? Commande.Numero + " " + Libelle : string.Empty;

        /// <summary>
        ///   Obtient ou définit la somme des soldes FAR de toutes les réceptions 
        /// </summary>
        public decimal SoldeFar { get; set; }

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
        /// Child DepenseTemporaires where [FRED_DEPENSE_TEMPORAIRE].[CommandeLigneId] point to this entity (FK_DEPENSE_TEMPORAIRE_COMMANDE_LIGNE)
        /// </summary>
        public virtual ICollection<DepenseTemporaireEnt> DepenseTemporaires { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la ligne d'avenant correspondante ou null si la ligne n'est pas un avenant.
        /// </summary>
        public int? AvenantLigneId { get; set; }

        /// <summary>
        /// Obtient ou définit la ligne d'avenant correspondante ou null si la ligne n'est pas un avenant.
        /// </summary>
        public CommandeLigneAvenantEnt AvenantLigne { get; set; }

        /// <summary>
        /// Obtient ou définit le numéro de la ligne de commande 
        /// </summary>
        public int? NumeroLigne { get; set; }

        /// <summary>
        /// Obtient ou définit un identifiant unique de matériel
        /// </summary>
        public int? MaterielId { get; set; }

        /// <summary>
        /// Obtient ou définit le matériel
        /// </summary>
        public MaterielEnt Materiel { get; set; }

        /// <summary>
        /// Obtient ou définit un identifiant unique de personnel
        /// </summary>
        public int? PersonnelId { get; set; }

        /// <summary>
        /// Obtient ou définit le personnel
        /// </summary>
        public PersonnelEnt Personnel { get; set; }

        /// <summary>
        /// Obtient ou définit le commentaire d'une ligne de commande énergie
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        /// Obtient ou définie le numéro externe de la commande ligne
        /// </summary>
        public string NumeroCommandeLigneExterne { get; set; }

        /// <summary>
        /// Obtient ou définit si la ligne est verouillée
        /// </summary>
        public bool IsVerrou { get; set; }

        /// <summary>
        ///   Supprimer les propriétés de l'objet
        /// </summary>
        public void CleanProperties()
        {
            Ressource = null;
            Tache = null;
            Unite = null;
            AllDepenses = null;
            Commande = null;
            Materiel = null;
            Personnel = null;
        }
    }
}
