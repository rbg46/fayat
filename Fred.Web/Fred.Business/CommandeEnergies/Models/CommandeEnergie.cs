using System;
using System.Collections.Generic;
using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Referential;

namespace Fred.Business.CommandeEnergies
{
    /// <summary>
    /// Modèle Commande Enérgie
    /// </summary>
    public class CommandeEnergie
    {
        /// <summary>
        ///   Obtient ou définit le code d'une ligne de commande.
        /// </summary>        
        public int CommandeId { get; set; }

        /// <summary>
        ///   Obtient ou définit le numéro d'une commande.
        /// </summary>        
        public string Numero { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'affaire de la commande.
        /// </summary>        
        public int CiId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité affaire reliée à la commande
        /// </summary>        
        public CIEnt CI { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé de la commande.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de la commande.
        /// </summary>        
        public DateTime Date { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ID fournisseur de la commande.
        /// </summary>        
        public int FournisseurId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du fournisseur de la commande.
        /// </summary>        
        public FournisseurEnt Fournisseur { get; set; }

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
        ///   Obtient ou définit l'ID du statut de la commande.
        /// </summary>        
        public int StatutCommandeId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du statut relié à la commande
        /// </summary>        
        public StatutCommandeEnt StatutCommande { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des lignes d'une commande
        /// </summary>
        public ICollection<CommandeEnergieLigne> Lignes { get; set; }

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
        ///   Obtient ou définit la devise de référence du CI
        /// </summary>        
        public DeviseEnt Devise { get; set; }

        /// <summary>
        ///   Obtient ou définit le numéro externe de la commande.
        /// </summary>        
        public string NumeroCommandeExterne { get; set; }

        /// <summary>
        ///   Obtient ou définit si la commande est énergie   
        /// </summary>        
        public bool IsEnergie { get; } = true;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique du type d'énergie de la commande 
        /// </summary>        
        public int TypeEnergieId { get; set; }

        /// <summary>
        ///   Obtient ou définit le type d'énergie de la commande
        /// </summary>        
        public TypeEnergieEnt TypeEnergie { get; set; }

        /// <summary>
        /// Obtient ou définit le Montant HT
        /// </summary>
        public decimal MontantHT { get; set; }

        /// <summary>
        /// Obtient le numéro de job hangfire lors de l'export de la commande énergie vers SAP
        /// </summary>
        public string HangfireJobId { get; set; }
    }
}
