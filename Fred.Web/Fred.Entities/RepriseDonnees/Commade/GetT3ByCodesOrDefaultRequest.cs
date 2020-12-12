using System.Diagnostics;

namespace Fred.Entites.RepriseDonnees.Commande
{
    /// <summary>
    /// Container de la requettes faite a la base pour obtenir la tache associée a une commande ligne
    /// </summary>
    [DebuggerDisplay("CiId = {CiId} Code = {Code} ")]
    public class GetT3ByCodesOrDefaultRequest
    {
        /// <summary>
        /// le ciId
        /// </summary>
        public int CiId { get; set; }
        /// <summary>
        /// Le code de la tache
        /// </summary>
        public string Code { get; set; }
    }
}
