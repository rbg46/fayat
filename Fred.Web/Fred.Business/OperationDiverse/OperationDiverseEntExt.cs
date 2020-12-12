using System.Collections.Generic;
using System.Linq;
using Fred.Entities.OperationDiverse;

namespace Fred.Business.OperationDiverse
{
    /// <summary>
    /// Extension pour OperationDiverseEntExt
    /// </summary>
    public static class OperationDiverseEntExt
    {
        /// <summary>
        /// Filtre les OperationDiverseEnt par famille d'OD:
        /// </summary>
        /// <param name="allOperationDiverses">liste OperationDiverseEnt</param>
        /// <param name="odFamilly">Identifiant de famille d'OD</param>
        /// <returns>Liste OperationDiverseEnt filtre</returns>
        public static IEnumerable<OperationDiverseEnt> ContainedInOdFamilly(this IEnumerable<OperationDiverseEnt> allOperationDiverses, int odFamilly)
        {
            return allOperationDiverses.Where(op => op.FamilleOperationDiverseId == odFamilly).ToList();
        }

        /// <summary>
        /// retourne le montant des od p.
        /// </summary>
        /// <param name="operationDiverses">operationDiverses</param>   
        /// <returns>un montant total</returns>
        public static decimal GetMontantTotal(this IEnumerable<OperationDiverseEnt> operationDiverses)
        {
            decimal result = 0;

            foreach (OperationDiverseEnt operationDiverse in operationDiverses)
            {
                result += operationDiverse.Montant;
            }
            return result;
        }
    }
}
