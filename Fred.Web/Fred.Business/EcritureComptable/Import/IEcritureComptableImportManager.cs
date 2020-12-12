using Fred.Entities.EcritureComptable;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fred.Business.EcritureComptable.Import
{
    /// <summary>
    /// Manager des import des EcritureComptableEnt
    /// </summary>
    public interface IEcritureComptableImportManager : IManager
    {
        /// <summary>
        /// Gere l'import des ecriture comptables pour un mois
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="dateComptable">dateComptable</param>
        /// <param name="ecritureComptablesDtos">ecritureComptablesDtos</param>
        /// <param name="codeEtablissement">code de l'établissement comptable</param>
        /// <returns>Nombre d'ecriture comptables qui sont insérées dans la base (nouvelles)</returns>
        Task<int> ManageImportEcritureComptablesAsync(int? societeId, DateTime dateComptable, IEnumerable<EcritureComptableDto> ecritureComptablesDtos, string codeEtablissement);

        /// <summary>
        /// Gere l'import des ecriture comptables pour une periode
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="dateComptableDebut">Date de début</param>
        /// <param name="dateComptableFin">Date de fin</param>
        /// <param name="ecritureComptablesDtos">ecritureComptablesDtos</param>
        /// <param name="codeEtablissement">code de l'établissement comptable</param>
        /// <returns>Nombre d'ecriture comptables qui sont insérées dans la base (nouvelles)</returns>
        Task<int> ManageImportEcritureComptablesAsync(int? societeId, DateTime dateComptableDebut, DateTime dateComptableFin, IEnumerable<EcritureComptableDto> ecritureComptablesDtos, string codeEtablissement);

        /// <summary>
        /// Gére l'import des écritures comptables pour FAYAT TP
        /// </summary>
        /// <param name="ecritureComptables">Liste de <see cref="EcritureComptableFtpDto"/></param>
        /// <returns>Le model qui sera envoyé a SAP</returns>
        Task<IList<EcritureComptableFtpDto>> ManageImportEcritureComptablesAsync(List<EcritureComptableFtpDto> ecritureComptables, List<int> societeIds);
    }
}
