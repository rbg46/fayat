using System;
using System.Collections.Generic;
using Fred.Entities.CI;
using Fred.Entities.Utilisateur;

namespace Fred.Entities.ObjectifFlash
{
    /// <summary>
    ///   Représente un objectif flash.
    /// </summary>
    public class ObjectifFlashEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un Objectif flash.
        /// </summary>
        public int ObjectifFlashId { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé de la commande.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de début du Objectif flash.
        /// </summary>
        public DateTime DateDebut { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de fin du Objectif flash.
        /// </summary>
        public DateTime DateFin { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'affaire de la commande.
        /// </summary>
        public int? CiId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité affaire reliée à la commande
        /// </summary>
        public CIEnt Ci { get; set; } = null;

        /// <summary>
        ///   Obtient ou définit le flag d'activation de l'objectif flash
        /// </summary>
        public bool IsActif { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de clôture de la commande.
        /// </summary>
        public DateTime? DateCloture { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ID du saisisseur de l'objectif flash.
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du utilisateur ayant saisi l'objectif flash.
        /// </summary>
        public UtilisateurEnt AuteurCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de saisie de l'objectif flash.
        /// </summary>
        public DateTime? DateCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ID de la personne ayant modifié l'objectif flash.
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du utilisateur ayant modifié l'objectif flash.
        /// </summary>
        public UtilisateurEnt AuteurModification { get; set; }

        /// <summary>
        ///   Obtient ou définit la dernière date de modification de l'objectif flash.
        /// </summary>
        public DateTime? DateModification { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ID de la personne ayant supprimé l'objectif flash.
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du utilisateur ayant supprimé l'objectif flash.
        /// </summary>
        public UtilisateurEnt AuteurSuppression { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de suppression de l'objectif flash.
        /// </summary>
        public DateTime? DateSuppression { get; set; }

        /// <summary>
        /// Obtient ou définit la somme objectif de l'Objectif Flash.
        /// </summary>
        public decimal? TotalMontantObjectif { get; set; }

        /// <summary>
        /// Obtient ou définit la somme du montant réalisé de l'Objectif Flash.
        /// </summary>
        public decimal? TotalMontantRealise { get; set; }

        /// <summary>
        /// Obtient ou définit la somme du montant réalisé de l'Objectif Flash.
        /// </summary>
        public decimal? EcartRealiseObjectif { get; set; }

        /// <summary>
        /// Obtient ou définit la somme du montant journalisé de l'Objectif Flash.
        /// </summary>
        public decimal? TotalMontantJournalise { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des lignes des objectifs flash
        /// </summary>
        public virtual ICollection<ObjectifFlashTacheEnt> Taches { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si l'objectif Flash est supprimé logiquement
        /// </summary>
        public bool IsDeleted => AuteurSuppression != null;

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si l'objectif Flash est clôturé
        /// </summary>
        public bool IsClosed { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des journalisations d'objectif flash
        /// </summary>
        public List<ObjectifFlashJournalisation> Journalisations { get; set; }

        /// <summary>
        ///   Supprimer les propriétés de l'objet
        /// </summary>
        public void CleanProperties()
        {
            Ci = null;
            AuteurCreation = null;
            AuteurModification = null;
            AuteurSuppression = null;
            foreach (var objectifFlashTache in Taches ?? new List<ObjectifFlashTacheEnt>())
            {
                objectifFlashTache.CleanProperties();
            }
        }
    }
}
