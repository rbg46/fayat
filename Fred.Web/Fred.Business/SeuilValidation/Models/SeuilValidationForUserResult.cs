using System.Diagnostics;

namespace Fred.Business.SeuilValidation.Models
{
    /// <summary>
    /// Classe contenant le result d'un calcul de seuil pour un ci donné et une devise donnée
    /// </summary>
    [DebuggerDisplay("DeviseId = {DeviseId} UtilisateurId = {UtilisateurId} Seuil = {Seuil}")]
    public class SeuilValidationForUserResult
    {
        /// <summary>
        /// DeviseId
        /// </summary>
        public decimal DeviseId { get; set; }
        /// <summary>
        /// UtilisateurId
        /// </summary>
        public int UtilisateurId { get; set; }
        /// <summary>
        /// Seuil
        /// </summary>
        public decimal? Seuil { get; set; }
    }
}
