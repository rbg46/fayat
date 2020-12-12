using System;
using Fred.Entities.Commande;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;

namespace Fred.Business.CommandeEnergies
{
    /// <summary>
    /// Modèle Commande Ligne Enérgie
    /// </summary>
    public class CommandeEnergieLigne
    {
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
        ///   Obtient ou définit le libellé/désignation d'une ligne de commande.
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
        ///   Obtient ou définit la quantité (ligne supplémentaire) OU la quantité ajustée (pointage et ajustement)
        /// </summary>        
        public decimal? Quantite { get; set; }

        /// <summary>
        ///   Obtient ou définit la quantité pointée : somme des heures travaillées pointées sur ce Personnel pour le CI SEP et la Période
        /// </summary>        
        public decimal? QuantitePointee { get; set; }

        /// <summary>
        ///   Obtient ou définit la quantité convertie
        /// </summary>        
        public decimal? QuantiteConvertie { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'unité barème d’exploitation en application pour le personnel et pour la période
        /// </summary>        
        public int? UniteBaremeId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'unité barème d’exploitation en application pour le personnel et pour la période
        /// </summary>        
        public UniteEnt UniteBareme { get; set; }

        /// <summary>
        /// Obtient ou définit la valeur du barème d’exploitation en application pour le personnel et pour la période
        /// </summary>
        public decimal? Bareme { get; set; }

        /// <summary>
        ///   Obtient ou définit le prix unitaire (ligne supplémentaire) OU prix unitaire ajusté (pointage et ajustement)
        /// </summary>        
        public decimal PUHT { get; set; }

        /// <summary>
        /// Obtient ou définit le montant valorisé
        /// </summary>
        public decimal MontantValorise { get; set; }

        /// <summary>
        /// Obtient ou définit le montant HT énergie
        /// </summary>
        public decimal MontantHT { get; set; }

        /// <summary>
        /// Obtient ou définit le commentaire d'une ligne
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'unité de pointage
        /// </summary>        
        public int? UniteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'unité 
        /// </summary>        
        public UniteEnt Unite { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du personnel
        /// </summary>
        public int? PersonnelId { get; set; }

        /// <summary>
        /// Obtient ou définit le personnel
        /// </summary>
        public PersonnelEnt Personnel { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du matériel
        /// </summary>
        public int? MaterielId { get; set; }

        /// <summary>
        /// Obtient ou définit le matériel
        /// </summary>
        public MaterielEnt Materiel { get; set; }

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
        /// Obtient l'écart sur prix unitaire (Bareme et PUHT)
        /// </summary>
        public decimal EcartPu { get; set; }

        /// <summary>
        /// Obtient l'écart sur quantité (Quantité convertie et Quantité)
        /// </summary>
        public decimal EcartQuantite { get; set; }

        /// <summary>
        /// Obtient l'écart sur montant (MontantHT et Montant valorisé)
        /// </summary>
        public decimal EcartMontant { get; set; }

        /// <summary>
        /// Définit si la ligne doit être supprimée ou non
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Définit si la ligne doit être mise à jour ou non
        /// </summary>
        public bool IsUpdated { get; set; }
    }
}
