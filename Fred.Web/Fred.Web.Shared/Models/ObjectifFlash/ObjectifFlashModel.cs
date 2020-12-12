using System;
using System.Collections.Generic;
using Fred.Web.Models.CI;
using Fred.Web.Models.Utilisateur;

namespace Fred.Web.Models.ObjectifFlash
{
    public class ObjectifFlashModel
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un Objectif flash.
        /// </summary>
        public int ObjectifFlashId { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé de l'Objectif Flash.
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
        ///   Obtient ou définit l'identifiant unique de l'affaire de l'Objectif Flash.
        /// </summary>
        public int? CiId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité affaire reliée à l'Objectif Flash.
        /// </summary>
        public CIModel Ci { get; set; } = null;

        /// <summary>
        ///   Obtient ou définit le flag d'activation de l'objectif flash
        /// </summary>
        public bool IsActif { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de clôture de l'Objectif Flash.
        /// </summary>
        public DateTime? DateCloture { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ID du saisisseur de l'Objectif Flash.
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du utilisateur ayant saisi l'Objectif Flash.
        /// </summary>
        public UtilisateurLightModel AuteurCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de saisie de l'Objectif Flash.
        /// </summary>
        public DateTime? DateCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit l'ID de la personne ayant modifié l'Objectif Flash.
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du utilisateur ayant modifier l'Objectif Flash
        /// </summary>
        public UtilisateurLightModel AuteurModification { get; set; }

        /// <summary>
        ///   Obtient ou définit la dernière date de modification de l'Objectif Flash.
        /// </summary>
        public DateTime? DateModification { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des lignes d'un Objectif Flash.
        /// </summary>
        public ICollection<ObjectifFlashTacheModel> Taches { get; set; }

        /// <summary>
        /// Obtient ou définit la somme objectif de l'Objectif Flash.
        /// </summary>
        public double TotalMontantObjectif { get; set; }

        /// <summary>
        /// Obtient ou définit la somme du réalisé de l'Objectif Flash.
        /// </summary>
        public double TotalMontantRealise { get; set; }

        /// <summary>
        /// Obtient ou définit la somme du réalisé de l'Objectif Flash.
        /// </summary>
        public double TotalMontantJournalise { get; set; }



        /// <summary>
        ///   Obtient ou définit une valeur indiquant si l'objectif Flash est clôturé
        /// </summary>
        public bool IsClosed { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur définissant les journalisations d'objectif flash (date et jours feriés) 
        /// </summary>
        public List<ObjectifFlashJournalisationModel> Journalisations { get; set; }
    }
}
