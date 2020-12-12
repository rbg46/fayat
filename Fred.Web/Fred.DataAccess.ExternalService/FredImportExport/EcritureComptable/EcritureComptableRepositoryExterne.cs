using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.JobStatut;
using Fred.Framework.Exceptions;
using Fred.Framework.ExternalServices.ImportExport;

namespace Fred.DataAccess.ExternalService.FredImportExport.EcritureComptable
{
    /// <summary>
    /// <see cref="EcritureComptableRepositoryExterne"/>
    /// </summary>
    public class EcritureComptableRepositoryExterne : BaseExternalRepositoy, IEcritureComptableRepositoryExterne
    {
        public EcritureComptableRepositoryExterne(IImportExportServiceDescriptor importExportServiceDescriptor)
            : base(importExportServiceDescriptor)
        {
        }

        /// <summary>
        /// Verifie si on peux faire l'import
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="societeId">societeId</param>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">dateComptable</param>
        /// <returns>JobStatutModel</returns>
        public async Task<JobStatutModel> CheckImportEcritureComptablesAsync(int userId, int societeId, int ciId, DateTime dateComptable)
        {
            try
            {
                string requestUri = string.Format(WebApiEndPoints.EcritureComptable_Get_Check_Import, BaseUrl, userId, societeId, ciId, dateComptable.ToString("yyyy-MM-dd"));
                return await RestClient.GetAsync<JobStatutModel>(requestUri);
            }
            catch (FredTechnicalException fte)
            {
                throw new FredRepositoryException(fte.Message, fte.InnerException);
            }
        }

        /// <summary>
        /// ImportEcritureComptables
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="societeId">societeId</param>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">dateComptable</param>
        /// <returns>JobStatutModel</returns>
        public async Task<JobStatutModel> ImportEcritureComptablesAsync(int userId, int societeId, int ciId, DateTime dateComptable)
        {
            try
            {
                string requestUri = string.Format(WebApiEndPoints.EcritureComptable_Post_Import, BaseUrl, userId, societeId, ciId, dateComptable.ToString("yyyy-MM-dd"));
                return await RestClient.PostAndEnsureSuccessAsync<JobStatutModel>(requestUri, null);
            }
            catch (FredTechnicalException fte)
            {
                throw new FredRepositoryException(fte.Message, fte.InnerException);
            }
        }

        /// <summary>
        /// ImportEcritureComptables
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="societeCode">societeCode</param>
        /// <param name="societeId">societeId</param>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptableDebut">Date de début</param>
        /// <param name="dateComptableFin">Date de fin</param>
        /// <returns>JobStatutModel</returns>
        public async Task<JobStatutModel> ImportEcritureComptablesAsync(int userId, string societeCode, int societeId, int ciId, DateTime dateComptableDebut, DateTime dateComptableFin)
        {
            try
            {
                string requestUri = string.Format(WebApiEndPoints.EcritureComptable_Post_ImportRange, BaseUrl, userId, societeCode, societeId, ciId, dateComptableDebut.ToString("yyyy-MM-dd"), dateComptableFin.ToString("yyyy-MM-dd"));
                return await RestClient.PostAndEnsureSuccessAsync<JobStatutModel>(requestUri, null);
            }
            catch (FredTechnicalException fte)
            {
                throw new FredRepositoryException(fte.Message, fte.InnerException);
            }
        }

        public async Task ImportEcrituresComptablesFromAnaelAsync(DateTime dateComptable, List<int> ciids, int? societeId, string societeCode)
        {
            try
            {
                CloturePeriodeWebModel cloturePeriodeModel = new CloturePeriodeWebModel { DateComptable = dateComptable, CiIds = ciids, SocieteId = societeId, SocieteCode = societeCode };
                string requestUri = string.Format(WebApiEndPoints.EcritureComptable_ImportEcrituresComptablesFromAnael, BaseUrl);
                await RestClient.PostAsync(requestUri, cloturePeriodeModel);
            }
            catch (FredTechnicalException fte)
            {
                throw new FredRepositoryException(fte.Message, fte.InnerException);
            }
        }
    }
}
