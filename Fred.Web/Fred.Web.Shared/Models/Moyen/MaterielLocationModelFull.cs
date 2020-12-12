using System;

namespace Fred.Web.Shared.Models.Moyen
{
    /// <summary>
    /// Model MaterielLocation renvoyé au front dans la pop up de gestion des locations
    /// </summary>
    public class MaterielLocationModelFull
    {
        /// <summary>
        /// Societe
        /// </summary>
        public string Societe { get; set; }

        /// <summary>
        /// Etablissement
        /// </summary>
        public string Etablissement { get; set; }

        /// <summary>
        /// Numéro du Parc
        /// </summary>
        public string NumeroParc { get; set; }

        /// <summary>
        /// Libelle
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Immmatriculation
        /// </summary>
        public string Immatriculation { get; set; }

        /// <summary>
        /// Identifiant de la location
        /// </summary>
        public int MaterielLocationId { get; set; }

        /// <summary>
        /// <see cref="FicheGeneriqueModel"/>
        /// </summary>
        public FicheGeneriqueModel FicheGeneriqueModel { get; set; }

        /// <summary>
        /// Identifiant du Materiel
        /// </summary>
        public int MaterielId { get; set; }

        /// <summary>
        /// Identifiant de l'auteur de création
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        /// Identifiant de l'auteur de modification
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        /// Identifiant de l'auteur de suppression
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        /// Date de création
        /// </summary>
        public DateTime? DateCreation { get; set; }

        /// <summary>
        /// <see cref="ModeleLocationModel"/>
        /// </summary>
        public ModeleLocationModel ModeleLocation { get; set; }
    }
}
