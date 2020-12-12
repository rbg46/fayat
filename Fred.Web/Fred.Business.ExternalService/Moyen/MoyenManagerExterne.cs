using System;
using System.Threading.Tasks;
using Fred.DataAccess.ExternalService.FredImportExport.Moyen;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.Models.Moyen;

namespace Fred.Business.ExternalService.Materiel
{
    /// <summary>
    /// Définit un manager de moyen externe
    /// </summary>
    public class MoyenManagerExterne : IMoyenManagerExterne
    {
        private readonly IMoyenRepositoryExterne moyenRepoExterne;

        public MoyenManagerExterne(IMoyenRepositoryExterne moyenRepoExterne)
        {
            this.moyenRepoExterne = moyenRepoExterne;
        }

        /// <summary>
        /// Export des pointage des moyens
        /// </summary>
        /// <param name="startDate">Date de de début</param>
        /// <param name="endDate">Date de fin</param>
        /// <returns>Envoi pointage moyen result model</returns>
        public async Task<EnvoiPointageMoyenResultModel> ExportPointageMoyenAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await moyenRepoExterne.ExportPointageMoyenAsync(startDate, endDate);
            }
            catch (FredRepositoryException fre)
            {
                throw new FredBusinessException(fre.Message, fre.InnerException);
            }
        }
    }
}
