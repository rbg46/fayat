using System;
using System.Diagnostics;

namespace Fred.Entities.ActivitySummary
{

    /// <summary>
    /// Reprenste une ligne de jalon dans le mail recapitulatif des activité en cours
    /// </summary>
    [DebuggerDisplay("UserId = {UserId} CiId = {CiId} JalonTransfertFar = {JalonTransfertFar} JalonAvancementvalider = {JalonAvancementvalider} JalonClotureDepense = {JalonClotureDepense} JalonCiCloturer = {JalonCiCloturer}")]
    public class UserJalonSummary
    {

        /// <summary>
        /// L'id de l'utilisateur
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// CiId
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// Jalon TransfertFar
        /// </summary>
        public DateTime? JalonTransfertFar { get; set; }

        /// <summary>
        /// Jalon Avancement valider
        /// </summary>
        public DateTime? JalonAvancementvalider { get; set; }

        /// <summary>
        /// Jalon Cloture Depense
        /// </summary>
        public DateTime? JalonClotureDepense { get; set; }


        /// <summary>
        /// Jalon Cloture Depense
        /// </summary>
        public DateTime? JalonValidationControleBudgetaire { get; set; }

        /// <summary>
        /// Jalon Ci Cloturer (champ calculé)
        /// </summary>
        public DateTime? JalonCiCloturer { get; set; }

        /// <summary>
        /// Couleur Jalon TransfertFar
        /// </summary>
        public ColorJalon ColorJalonTransfertFar { get; set; }

        /// <summary>
        /// Couleur Jalon Avancement valider
        /// </summary>
        public ColorJalon ColorJalonAvancementvalider { get; set; }

        /// <summary>
        /// Couleur Jalon Cloture Depense
        /// </summary>
        public ColorJalon ColorJalonClotureDepense { get; set; }


        /// <summary>
        /// Couleur Jalon Cloture Depense
        /// </summary>
        public ColorJalon ColorJalonValidationControleBudgetaire { get; set; }

        /// <summary>
        /// Couleur Jalon Ci Cloturer (champ calculé)
        /// </summary>
        public ColorJalon ColorJalonCiCloturer { get; set; }
        /// <summary>
        /// Le label qui  represente le nom du ci
        /// </summary>
        public string Labelle { get; set; }
    }
}
