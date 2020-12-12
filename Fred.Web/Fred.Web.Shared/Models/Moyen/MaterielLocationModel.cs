using System;
using Fred.Web.Models.Utilisateur;

namespace Fred.Entities.Moyen
{
    /// <summary>
    /// Table qui contient les matériels de type location crée par le module gestion des moyens . 
    /// </summary>
    public class MaterielLocationModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant d'une location de matériel
        /// </summary>
        public int MaterielLocationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du moyen
        /// </summary>
        public int MaterielId { get; set; }

        /// <summary>
        /// Obtient ou définit l'immatriculation machine 
        /// </summary>
        public string Immatriculation { get; set; }

        /// <summary>
        /// Obtient ou définit l'immatriculation machine 
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit la date de creation
        /// </summary>
        public DateTime DateCreation { get; set; }

        /// <summary>
        /// Obtient ou définit la date de modification
        /// </summary>
        public DateTime? DateModification { get; set; }

        /// <summary>
        /// Obtient ou définit la date de suppression
        /// </summary>
        public DateTime? DateSuppression { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'auteur de la création
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'auteur de la création
        /// </summary>
        public UtilisateurModel AuteurCreation { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'auteur de la modification
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'auteur de la modification
        /// </summary>
        public UtilisateurModel AuteurModification { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'auteur de la suppression
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        /// Obtient ou définit l'auteur de la suppression
        /// </summary>
        public UtilisateurModel AuteurSuppression { get; set; }
    }
}
