using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.DataAccess.ExternalService.FredImportExport.EcritureComptable;
using Fred.Entities.JobStatut;
using Fred.Framework.Exceptions;

namespace Fred.Business.ExternalService.EcritureComptable
{
    public class EcritureComptableManagerExterne : IEcritureComptableManagerExterne
    {
        private readonly IEcritureComptableRepositoryExterne ecritureComptableRepositoryExterne;
        private readonly ISocieteManager societeManager;
        private readonly IDatesClotureComptableManager datesClotureComptableManager;
        private readonly IUtilisateurManager utilisateurManager;

        public EcritureComptableManagerExterne(IEcritureComptableRepositoryExterne ecritureComptableRepositoryExterne,
            ISocieteManager societeManager,
            IDatesClotureComptableManager datesClotureComptableManager,
            IUtilisateurManager utilisateurManager)
        {
            this.ecritureComptableRepositoryExterne = ecritureComptableRepositoryExterne;
            this.societeManager = societeManager;
            this.datesClotureComptableManager = datesClotureComptableManager;
            this.utilisateurManager = utilisateurManager;
        }

        /// <summary>
        /// Demande si on peux faire un import
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">date</param>
        /// <returns>bool</returns>
        public async Task<bool> CanImportEcritureComptablesAsync(int societeId, int ciId, DateTime dateComptable)
        {
            bool monthIsClosedForCi = datesClotureComptableManager.IsPeriodClosed(ciId, dateComptable.Year, dateComptable.Month);
            if (monthIsClosedForCi)
            {
                return false;
            }
            try
            {
                int userId = utilisateurManager.GetContextUtilisateurId();
                JobStatutModel jobStatut = await ecritureComptableRepositoryExterne.CheckImportEcritureComptablesAsync(userId, societeId, ciId, dateComptable);
                return !jobStatut.IsEnqueued && !jobStatut.IsRunning;
            }
            catch (FredRepositoryException fre)
            {
                throw new FredBusinessException(fre.Message, fre.InnerException);
            }
        }

        /// <summary>
        /// Demande l'execution de l'import  des ecritures comptable.
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">date</param>
        /// <returns>JobStatutModel</returns>
        public async Task<JobStatutModel> ImportEcritureComptablesAsync(int societeId, int ciId, DateTime dateComptable)
        {
            try
            {
                int userId = utilisateurManager.GetContextUtilisateurId();
                return await ecritureComptableRepositoryExterne.ImportEcritureComptablesAsync(userId, societeId, ciId, dateComptable);
            }
            catch (FredRepositoryException fre)
            {
                throw new FredBusinessException(fre.Message, fre.InnerException);
            }
        }

        /// <summary>
        /// Demande l'execution de l'import  des ecritures comptable.
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptableDebut">Date de début</param>
        /// <param name="dateComptableFin">Date de fin</param>
        /// <returns>JobStatutModel</returns>
        public async Task<JobStatutModel> ImportEcritureComptablesAsync(int societeId, int ciId, DateTime dateComptableDebut, DateTime dateComptableFin)
        {
            try
            {
                int userId = utilisateurManager.GetContextUtilisateurId();
                string societeCode = societeManager.GetSocieteById(societeId).Code;
                return await ecritureComptableRepositoryExterne.ImportEcritureComptablesAsync(userId, societeCode, societeId, ciId, dateComptableDebut, dateComptableFin);
            }
            catch (FredRepositoryException fre)
            {
                throw new FredBusinessException(fre.Message, fre.InnerException);
            }
        }

        /// <summary>
        /// Import une liste d'écriture comptable depuis l'écran de clôture
        /// </summary>
        /// <param name="dateComptable">Date comptable</param>
        /// <param name="ciids">Liste d'identifiant de CI</param>
        /// <param name="societeId">Societe ID</param>
        /// <param name="societeCode">Code de la societe</param>
        public async Task ImportEcrituresComptablesFromAnaelAsync(DateTime dateComptable, List<int> ciids, int? societeId, string societeCode)
        {
            try
            {
                await ecritureComptableRepositoryExterne.ImportEcrituresComptablesFromAnaelAsync(dateComptable, ciids, societeId, societeCode);
            }
            catch (FredTechnicalException fte)
            {
                throw new FredRepositoryException(fte.Message, fte.InnerException);
            }
        }
    }
}

