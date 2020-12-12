using System.Collections.Generic;
using Fred.ImportExport.Business.ValidationPointage.Common;

namespace Fred.ImportExport.Business.ValidationPointage.Fon.Common
{
    public interface IFonQueryExecutor
    {
        /// <summary>
        /// Déversement des lignes de pointages et des primes dans l'AS400
        /// </summary>
        /// <typeparam name="T">ControlePointageEnt ou RemonteeVracEnt</typeparam>
        /// <param name="globalData">Données globale au controle vrac</param>
        /// <param name="pointagesAndPrimesQueries">Requets d'insertions</param>
        /// <param name="instance">Représente soit ControlePointageEnt ou RemonteeVracEnt</param>
        void ExecuteAnaelInserts<T>(ValidationPointageContextData globalData, IEnumerable<QueryInfo> pointagesAndPrimesQueries, T instance) where T : class;
    }
}
