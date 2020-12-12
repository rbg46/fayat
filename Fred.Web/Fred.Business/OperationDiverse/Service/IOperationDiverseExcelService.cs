using System;
using System.Threading.Tasks;

namespace Fred.Business.OperationDiverse.Service
{
    /// <summary>
    /// Interface pour le service excel des opérations diverses
    /// </summary>
    public interface IOperationDiverseExcelService : IService
    {
        /// <summary>
        /// Retourne l'export excel généré sous forme de tableau de byte
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">dateComptable</param>
        /// <param name="baseDirectory">chemin du repertoire du template</param>
        /// <returns>Fichier Excel au format byte[]</returns>
        Task<byte[]> GetFichierExempleChargementODAsync(int ciId, DateTime dateComptable, string baseDirectory);
    }
}
