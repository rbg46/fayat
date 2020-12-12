using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.RapportPrime;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.RapportPrime.Get;
using Fred.Web.Shared.Models.RapportPrime.Update;

namespace Fred.Business.RapportPrime
{
    public class RapportPrimeManager : Manager<RapportPrimeEnt, IRapportPrimeRepository>, IRapportPrimeManager
    {
        private readonly IUtilisateurManager userManager;
        private readonly IRapportPrimeLigneManager rapportPrimeLigneManager;

        public RapportPrimeManager(
            IUnitOfWork uow,
            IRapportPrimeRepository rapportPrimeRepository,
            IUtilisateurManager userManager,
            IRapportPrimeLigneManager rapportPrimeLigneManager)
          : base(uow, rapportPrimeRepository)
        {
            this.userManager = userManager;
            this.rapportPrimeLigneManager = rapportPrimeLigneManager;
        }

        public async Task<RapportPrimeGetModel> GetAsync(DateTime dateRapportPrime)
        {
            IEnumerable<int> userCiIds = await userManager.GetAllCIbyUserAsync(userManager.GetContextUtilisateurId());

            return await Repository.GetRapportPrimeByDateAsync(dateRapportPrime, userCiIds.ToList());
        }

        public async Task<RapportPrimeEnt> AddAsync()
        {
            DateTime currentDate = DateTime.UtcNow;

            bool rapportPrimeExists = await Repository.RapportPrimeExistsAsync(currentDate);

            if (rapportPrimeExists)
            {
                throw new FredBusinessException(FeatureRapportPrime.RapportPrime_Error_AddNewRapportPrime_RapportPrimeAlreadyExist);
            }

            UtilisateurEnt utilisateur = await userManager.GetContextUtilisateurAsync();

            RapportPrimeEnt rapportPrime = new RapportPrimeEnt
            {
                AuteurCreationId = utilisateur.UtilisateurId,
                DateCreation = currentDate,
                DateRapportPrime = currentDate,
                SocieteId = utilisateur.Personnel.SocieteId.Value
            };

            await Repository.AddAsync(rapportPrime);
            await SaveAsync();

            return rapportPrime;
        }

        public RapportPrimeEnt GetRapportPrime(DateTime dateRapportPrime, int utilisateurId)
        {
            List<int> listCiId = userManager.GetAllCIbyUser(utilisateurId).ToList();

            if (listCiId.Count == 0)
            {
                listCiId = userManager.GetAllCIbyUser(1).ToList();
            }

            return Repository.GetRapportPrimeByDate(dateRapportPrime, listCiId);
        }

        public async Task UpdateAsync(int rapportPrimeId, RapportPrimeUpdateModel rapportPrimeModel)
        {
            if (rapportPrimeModel == null)
            {
                throw new ArgumentNullException(nameof(rapportPrimeModel));
            }

            int userId = userManager.GetContextUtilisateurId();

            if (rapportPrimeModel.LinesToCreate.Any())
            {
                await rapportPrimeLigneManager.AddLinesAsync(rapportPrimeId, rapportPrimeModel.LinesToCreate, userId);
            }
            if (rapportPrimeModel.LinesToUpdate.Any())
            {
                await rapportPrimeLigneManager.UpdateLinesAsync(rapportPrimeId, rapportPrimeModel.LinesToUpdate, userId);
            }
            if (rapportPrimeModel.LinesToDelete.Any())
            {
                await rapportPrimeLigneManager.DeleteLinesAsync(rapportPrimeModel.LinesToDelete, userId);
            }

            SaveWithTransaction();
        }
    }
}
