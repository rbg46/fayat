using Fred.Entities.Referential;
using System.Diagnostics;

namespace Fred.Entites.RepriseDonnees.Commande
{
    /// <summary>
    /// Container de la response faite a la base pour obtenir la tache associée a une commande ligne
    /// </summary>
    [DebuggerDisplay("CiId = {CiId} Code = {Code} Tache = {Tache?.TacheId}")]
    public class GetT3ByCodesOrDefaultResponse
    {
        /// <summary>
        /// Le ciId
        /// </summary>
        public int CiId { get; set; }
        /// <summary>
        /// Le code de la tache demmandée
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// La tache recupérée
        /// </summary>
        public TacheEnt Tache { get; set; }
    }
}
