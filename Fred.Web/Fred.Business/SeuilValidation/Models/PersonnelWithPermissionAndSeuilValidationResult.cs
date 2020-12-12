using System.Diagnostics;

namespace Fred.Business.SeuilValidation.Models
{
    /// <summary>
    /// Model du domain contenant les information du personnel et s'il le seuil de validation Minimum demandé
    /// </summary>
    [DebuggerDisplay("PersonnelId = {PersonnelId} HaveMinimunSeuilValidation = {HaveMinimunSeuilValidation} Nom = {Nom}")]
    public class PersonnelWithPermissionAndSeuilValidationResult
    {
        /// <summary>
        /// PersonnelId
        /// </summary>
        public int PersonnelId { get; set; }
        /// <summary>
        /// HaveMinimunSeuilValidation
        /// </summary>
        public bool HaveMinimunSeuilValidation { get; set; }
        /// <summary>
        /// Nom
        /// </summary>
        public string Nom { get; set; }
        /// <summary>
        /// Prenom
        /// </summary>
        public string Prenom { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Matricule
        /// </summary>
        public string Matricule { get; internal set; }
        /// <summary>
        /// SocieteCode
        /// </summary>
        public string SocieteCode { get; internal set; }
    }
}
