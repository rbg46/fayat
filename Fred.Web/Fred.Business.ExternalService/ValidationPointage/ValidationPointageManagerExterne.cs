using System;
using System.Threading.Tasks;
using Fred.Business.Utilisateur;
using Fred.DataAccess.ExternalService.FredImportExport.ValidationPointage;
using Fred.Entities.RapportPrime;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.Models;

namespace Fred.Business.ExternalService.ValidationPointage
{
    public class ValidationPointageManagerExterne : IValidationPointageManagerExterne
    {
        private readonly IUtilisateurManager userManager;
        private readonly IValidationPointageRepositoryExterne validationPointageRepository;

        public ValidationPointageManagerExterne(IUtilisateurManager userManager, IValidationPointageRepositoryExterne validationPointageRepository)
        {
            this.userManager = userManager;
            this.validationPointageRepository = validationPointageRepository;
        }

        public async Task<ControlePointageResult> ExecuteControleVracAsync(int lotPointageId, PointageFiltre filtre)
        {
            try
            {
                int userId = GetCurrentUserId();

                return await validationPointageRepository.ExecuteControleVracAsync(userId, lotPointageId, filtre);
            }
            catch (FredRepositoryException fre)
            {
                throw new FredBusinessException(fre.Message, fre.InnerException);
            }
        }

        public async Task<RemonteeVracResult> ExecuteRemonteeVracAsync(DateTime periode, PointageFiltre filtre)
        {
            try
            {
                int userId = GetCurrentUserId();

                return await validationPointageRepository.ExecuteRemonteeVracAsync(userId, periode, filtre);
            }
            catch (FredRepositoryException fre)
            {
                throw new FredBusinessException(fre.Message, fre.InnerException);
            }
        }

        public async Task<RapportPrimeLignePrimeEnt> ExecuteRemonteePrimeAsync(DateTime periode)
        {
            try
            {
                int userId = GetCurrentUserId();

                return await validationPointageRepository.ExecuteRemonteePrimeAsync(userId, periode);
            }
            catch (FredRepositoryException fre)
            {
                throw new FredBusinessException(fre.Message, fre.InnerException);
            }
        }

        private int GetCurrentUserId()
        {
            // TODO : vérifier les droits de l'utilisateur (Ici ou dans FRED Import Export ?)
            // CSP : pas d'accès
            // GSP : droit à tout
            return userManager.GetContextUtilisateurId();
        }
    }
}
