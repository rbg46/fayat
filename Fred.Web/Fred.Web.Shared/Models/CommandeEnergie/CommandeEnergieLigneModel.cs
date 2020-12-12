using System;
using Fred.Web.Models.Personnel;
using Fred.Web.Models.Referential;
using Fred.Web.Models.ReferentielFixe;

namespace Fred.Web.Shared.Models
{
    /// <summary>
    /// View Model Commande Ligne Enérgie
    /// </summary>
    public class CommandeEnergieLigneModel
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une ligne de commande.
        /// </summary>        
        public int CommandeLigneId { get; set; }

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
        public TacheModel Tache { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Id de la ressource d'une ligne de commande.
        /// </summary>        
        public int? RessourceId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'objet ressource
        /// </summary>        
        public RessourceModel Ressource { get; set; }

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
        public UniteModel UniteBareme { get; set; }

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
        public UniteModel Unite { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du personnel
        /// </summary>
        public int? PersonnelId { get; set; }

        /// <summary>
        /// Obtient ou définit le personnel
        /// </summary>
        public PersonnelLightModel Personnel { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du matériel
        /// </summary>
        public int? MaterielId { get; set; }

        /// <summary>
        /// Obtient ou définit le matériel
        /// </summary>
        public MaterielModel Materiel { get; set; }

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
        ///   Obtient ou définit l'ID de la personne ayant supprimer la ligne de commande.
        /// </summary>        
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        ///   Obtient ou définit la dernière date de supprimer de la ligne de commande.
        /// </summary>        
        public DateTime? DateSuppression { get; set; }

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
