using System.Diagnostics;

namespace Fred.Entities.ActivitySummary
{
    /// <summary>
    /// Contient le resultat d'une requette a la db pour un Ci et un type de requette
    /// Exemple : pour la requette => 'CommandeAvalider' pour le CiId = 419 il y a un Count de 4 Commandes a valider
    /// </summary>
    [DebuggerDisplay("CiId = {CiId} RequestName = {RequestName} Count = {Count}")]
    public class ActivityRequestWithCountResult
    {

        /// <summary>
        /// Nom de la requette
        /// </summary>
        public TypeActivity RequestName { get; set; }




        /// <summary>
        /// Ciid
        /// </summary>
        public int CiId { get; set; }


        /// <summary>
        /// Nombre correspondant au nombre actions a faire pour une requette et pour un CI
        /// </summary>
        public int Count { get; set; }
    }
}
