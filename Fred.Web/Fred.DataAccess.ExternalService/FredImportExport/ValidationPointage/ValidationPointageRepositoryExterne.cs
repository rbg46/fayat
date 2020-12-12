using System;
using System.Threading.Tasks;
using Fred.Entities.RapportPrime;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Exceptions;
using Fred.Framework.ExternalServices.ImportExport;
using Fred.Framework.Services;
using Fred.Web.Shared.Models;

namespace Fred.DataAccess.ExternalService.FredImportExport.ValidationPointage
{
    public class ValidationPointageRepositoryExterne : BaseExternalRepositoy, IValidationPointageRepositoryExterne
    {
        public ValidationPointageRepositoryExterne(IImportExportServiceDescriptor importExportServiceDescriptor)
            : base(importExportServiceDescriptor)
        {
        }

        public async Task<ControlePointageResult> ExecuteControleVracAsync(int userId, int lotPointageId, PointageFiltre filtre)
        {
            try
            {
                string requestUri = string.Format(WebApiEndPoints.ValidationPointage_Post_ExecuteControleVrac, BaseUrl) + userId + "/" + lotPointageId;

                return await RestClient.PostAndEnsureSuccessAsync<ControlePointageResult>(requestUri, filtre);
            }
            catch (FredTechnicalException fte)
            {
                throw new FredRepositoryException(fte.Message, fte.InnerException);
            }
        }

        public async Task<RemonteeVracResult> ExecuteRemonteeVracAsync(int userId, DateTime periode, PointageFiltre filtre)
        {
            try
            {
                string requestUri = string.Format(WebApiEndPoints.ValidationPointage_Post_ExecuteRemonteeVrac, BaseUrl) + userId + "/" + $"{periode:MM-dd-yyyy}";

                return await RestClient.PostAndEnsureSuccessAsync<RemonteeVracResult>(requestUri, filtre);
            }
            catch (FredTechnicalException fte)
            {
                throw new FredRepositoryException(fte.Message, fte.InnerException);
            }
        }

        public async Task<RapportPrimeLignePrimeEnt> ExecuteRemonteePrimeAsync(int utilisateurId, DateTime periode)
        {
            try
            {
                string requestUri = string.Format(WebApiEndPoints.ValidationPointage_Post_ExecuteRemonteePrime, BaseUrl) + utilisateurId + "/" + $"{periode:MM-dd-yyyy}";

                return await RestClient.PostAndEnsureSuccessAsync<RapportPrimeLignePrimeEnt>(requestUri, periode);
            }
            catch (FredTechnicalException fte)
            {
                throw new FredRepositoryException(fte.Message, fte.InnerException);
            }
        }
    }
}
