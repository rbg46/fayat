using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.Affectation;
using Fred.Business.Astreinte;
using Fred.Business.CI;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Affectation;
using Fred.Entities.Rapport;
using Fred.Entities.RapportPrime;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.RapportPrime.Update;

namespace Fred.Business.RapportPrime
{
    public class RapportPrimeLigneManager : Manager<RapportPrimeLigneEnt, IRapportPrimeLigneRepository>, IRapportPrimeLigneManager
    {
        private readonly IRapportPrimeLigneAstreinteRepository rapportPrimeLigneAstreinteRepository;
        private readonly IAffectationManager affectationManager;
        private readonly IAstreinteManager astreinteManager;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly ICIManager cIManager;

        public RapportPrimeLigneManager(
            IUnitOfWork uow,
            IRapportPrimeLigneRepository rapportPrimeLigneRepository,
            IRapportPrimeLigneAstreinteRepository rapportPrimeLigneAstreinteRepository,
            IAffectationManager affectationManager,
            IAstreinteManager astreinteManager,
            IUtilisateurManager utilisateurManager,
            ICIManager cIManager)
          : base(uow, rapportPrimeLigneRepository)
        {
            this.rapportPrimeLigneAstreinteRepository = rapportPrimeLigneAstreinteRepository;
            this.affectationManager = affectationManager;
            this.astreinteManager = astreinteManager;
            this.utilisateurManager = utilisateurManager;
            this.cIManager = cIManager;
        }

        public IEnumerable<RapportPrimeLigneEnt> GetListeRapportPrimeLigneByMonth(int year, int month, TypeFiltreEtatPaie typeFiltre, int organisationId, bool tri, int? personnelId)
        {
            DateTime dateRapportPrimeLigneMin = new DateTime(year, month, 1);
            DateTime dateRapportPrimeLigneMax = dateRapportPrimeLigneMin.AddMonths(1);

            IEnumerable<RapportPrimeLigneEnt> listeRapportPrimeLigneByMonth = SearchRapportPrimeLigneReelWithFilter(dateRapportPrimeLigneMin, dateRapportPrimeLigneMax, personnelId);
            if (!personnelId.HasValue)
            {
                int userId = utilisateurManager.GetContextUtilisateurId();
                switch (typeFiltre)
                {
                    case TypeFiltreEtatPaie.Population:
                        listeRapportPrimeLigneByMonth = GetRapportPrimeLigneVerrouillesByUserId(userId, year, month);
                        break;
                    case TypeFiltreEtatPaie.Perimetre:
                        List<int> allCisByUser = utilisateurManager.GetAllCIbyUser(userId).ToList();
                        listeRapportPrimeLigneByMonth = listeRapportPrimeLigneByMonth.Where(p => p.CiId.HasValue && allCisByUser.Contains(p.CiId.Value));
                        break;
                    case TypeFiltreEtatPaie.Autre:
                        List<int> allCisByOrga = cIManager.GetAllCIbyOrganisation(organisationId).ToList();
                        listeRapportPrimeLigneByMonth = listeRapportPrimeLigneByMonth.Where(p => p.CiId.HasValue && allCisByOrga.Contains(p.CiId.Value));
                        break;
                }
            }

            return tri
                ? listeRapportPrimeLigneByMonth.OrderBy(x => x.Personnel != null ? x.Personnel.Matricule : string.Empty)
                : listeRapportPrimeLigneByMonth.OrderBy(x => x.Personnel != null ? x.Personnel.Nom : string.Empty);
        }

        public IEnumerable<RapportPrimeLigneEnt> SearchRapportPrimeLigneReelWithFilter(DateTime dateRapportPrimeLigneMin, DateTime dateRapportPrimeLigneMax, int? personnelId)
        {
            IEnumerable<RapportPrimeLigneEnt> rapportPrimeLignes = Repository.SearchRapportPrimeLigneWithFilter(p => p.RapportPrime.DateRapportPrime >= dateRapportPrimeLigneMin
                                                                                    && p.RapportPrime.DateRapportPrime < dateRapportPrimeLigneMax
                                                                                    && (personnelId.HasValue && p.PersonnelId == personnelId.Value || !personnelId.HasValue));

            // Retourne rapportPrimeLignes s'il est != null sinon retourne une liste vide
            return rapportPrimeLignes ?? (new RapportPrimeLigneEnt[] { });
        }

        public bool IsLineValidated(RapportPrimeLigneEnt rapportPrimeLigne)
        {
            return rapportPrimeLigne.IsValidated;
        }

        private IEnumerable<RapportPrimeLigneEnt> GetRapportPrimeLigneVerrouillesByUserId(int userid, int annee, int mois)
        {
            return Repository.GetRapportPrimeLigneVerrouillesByUserId(userid, annee, mois);
        }

        public async Task AddLinesAsync(int rapportPrimeId, List<RapportPrimeLigneUpdateModel> lines, int userId)
        {
            foreach (var line in lines)
            {
                if (IsLineValidForCreation(line))
                {
                    RapportPrimeLigneEnt rapportPrimeLine = await CreateNewRapportPrimeLine(rapportPrimeId, userId, line);

                    await Repository.AddAsync(rapportPrimeLine);
                }
            }

            bool IsLineValidForCreation(RapportPrimeLigneUpdateModel line) => line.PersonnelId != 0
                       && ((line.ListAstreintes.Count > 0 && line.CiId != null) || line.ListAstreintes.Count == 0);
        }

        private async Task<RapportPrimeLigneEnt> CreateNewRapportPrimeLine(int rapportPrimeId, int userId, RapportPrimeLigneUpdateModel line)
        {
            var rapportPrimeLine = new RapportPrimeLigneEnt
            {
                RapportPrimeId = rapportPrimeId,
                PersonnelId = line.PersonnelId,
                CiId = line.CiId,
                AuteurCreationId = userId,
                DateValidation = line.DateValidation,
                DateCreation = DateTime.UtcNow
            };

            if (line.IsValidated)
            {
                rapportPrimeLine.Validate(userId);
            }

            rapportPrimeLine.ListPrimes = CreateNewPrimes(rapportPrimeLine.RapportPrimeLigneId, line.ListPrimes);

            if (line.CiId.HasValue && line.ListAstreintes?.Count > 0)
            {
                rapportPrimeLine.ListAstreintes = await CreateNewAstreintes(rapportPrimeLine.RapportPrimeLigneId, line);
            }

            return rapportPrimeLine;
        }

        private List<RapportPrimeLignePrimeEnt> CreateNewPrimes(int rapportPrimeLigneId, List<RapportPrimeLignePrimeUpdateModel> linePrimes)
        {
            return linePrimes.Select(p => new RapportPrimeLignePrimeEnt
            {
                RapportPrimeLigneId = rapportPrimeLigneId,
                PrimeId = p.PrimeId,
                Montant = p.Montant
            }).ToList();
        }

        private async Task<List<RapportPrimeLigneAstreinteEnt>> CreateNewAstreintes(int rapportPrimeLigneId, RapportPrimeLigneUpdateModel line)
        {
            AffectationEnt affectation = await affectationManager.GetOrNewAffectationAsync(line.PersonnelId, line.CiId.Value);

            return await ProcessAstreintesToEntityAsync(rapportPrimeLigneId, line.ListAstreintes, affectation);
        }

        public async Task UpdateLinesAsync(int rapportPrimeId, List<RapportPrimeLigneUpdateModel> lines, int userId)
        {
            List<RapportPrimeLigneEnt> existingLines = await Repository.GetListWithLinkedPropertiesAsync(lines.Select(l => l.RapportPrimeLigneId));

            foreach (var line in lines)
            {
                if (IsLineValidForUpdate(line))
                {
                    var lineToUpdate = GetRapportPrimeLineFromList(line.RapportPrimeLigneId, existingLines);

                    await UpdateRapportPrimeLigne(userId, lineToUpdate, line);

                    Repository.UpdateRapportPrimeLigne(lineToUpdate);
                }
            }

            bool IsLineValidForUpdate(RapportPrimeLigneUpdateModel line) => line.CiId != null && line.ListAstreintes.Count > 0 || line.ListPrimes.Count > 0;
        }

        private RapportPrimeLigneEnt GetRapportPrimeLineFromList(int rapportPrimeLigneId, List<RapportPrimeLigneEnt> list)
        {
            RapportPrimeLigneEnt lineToUpdate = list.SingleOrDefault(e => e.RapportPrimeLigneId == rapportPrimeLigneId);

            if (lineToUpdate == null)
                throw new FredBusinessNotFoundException(FeatureRapportPrime.RapportPrime_Error_UpdateRapportPrimeLine_RapportPrimeNotFound);

            return lineToUpdate;
        }

        private async Task UpdateRapportPrimeLigne(int userId, RapportPrimeLigneEnt lineToUpdate, RapportPrimeLigneUpdateModel line)
        {
            lineToUpdate.PersonnelId = line.PersonnelId;
            lineToUpdate.CiId = line.CiId;
            lineToUpdate.DateModification = DateTime.UtcNow;
            lineToUpdate.AuteurModificationId = userId;

            if (line.IsValidated)
            {
                lineToUpdate.Validate(userId);
            }
            else
            {
                lineToUpdate.Invalidate();
            }

            UpdatePrimes(lineToUpdate, line);

            await UpdateAstreintesAsync(lineToUpdate, line);
        }

        private void UpdatePrimes(RapportPrimeLigneEnt lineToUpdate, RapportPrimeLigneUpdateModel line)
        {
            foreach (var prime in line.ListPrimes)
            {
                var primeToUpdate = lineToUpdate.ListPrimes.FirstOrDefault(p => p.RapportPrimeLignePrimeId == prime.RapportPrimeLignePrimeId);

                if (primeToUpdate == null)
                {
                    lineToUpdate.ListPrimes.Add(new RapportPrimeLignePrimeEnt
                    {
                        RapportPrimeLigneId = lineToUpdate.RapportPrimeLigneId,
                        PrimeId = prime.PrimeId,
                        Montant = prime.Montant
                    });
                }
                else
                {
                    primeToUpdate.Montant = prime.Montant;
                }
            }
        }

        private async Task UpdateAstreintesAsync(RapportPrimeLigneEnt lineToUpdate, RapportPrimeLigneUpdateModel line)
        {
            List<RapportPrimeLigneAstreinteEnt> lineAstreintes = await CreateNewAstreintes(lineToUpdate.RapportPrimeLigneId, line);

            IEnumerable<RapportPrimeLigneAstreinteEnt> listAstreinteToDelete = lineToUpdate.ListAstreintes.Except(lineAstreintes, new RapportPrimeLigneAstreinteComparer());

            if (listAstreinteToDelete.Any())
            {
                rapportPrimeLigneAstreinteRepository.RemoveRange(listAstreinteToDelete);
            }

            lineToUpdate.ListAstreintes.AddRange(lineAstreintes.Where(a => a.IsCreated));
        }

        public async Task DeleteLinesAsync(List<int> lines, int userId)
        {
            List<RapportPrimeLigneEnt> existingLines = await Repository.GetListAsync(lines);

            foreach (var line in existingLines)
            {
                line.Delete(userId);

                Repository.UpdateRapportPrimeLigne(line);
            }
        }

        private async Task<List<RapportPrimeLigneAstreinteEnt>> ProcessAstreintesToEntityAsync(int rapportPrimeLigneId, List<DateTime> listDateAstreintes, AffectationEnt affectation)
        {
            List<AstreinteEnt> astreintes = await GetAstreintesAsync(listDateAstreintes, affectation);

            List<RapportPrimeLigneAstreinteEnt> rapportPrimeLigneAstreintes = await CreateRapportPrimeLineAstreintesAsync(rapportPrimeLigneId, astreintes);

            return rapportPrimeLigneAstreintes;
        }


        private async Task<List<AstreinteEnt>> GetAstreintesAsync(List<DateTime> listDateAstreintes, AffectationEnt affectation)
        {
            List<AstreinteEnt> astreintes = new List<AstreinteEnt>();

            foreach (DateTime date in listDateAstreintes)
            {
                astreintes.Add(await astreinteManager.GetOrNewAstreinteAsync(affectation, DateTime.Parse(date.ToShortDateString())));
            }

            return astreintes;
        }

        private async Task<List<RapportPrimeLigneAstreinteEnt>> CreateRapportPrimeLineAstreintesAsync(int rapportPrimeLigneId, List<AstreinteEnt> astreintes)
        {
            List<RapportPrimeLigneAstreinteEnt> rapportPrimeLigneAstreintes = new List<RapportPrimeLigneAstreinteEnt>();

            foreach (AstreinteEnt astreinte in astreintes)
            {
                RapportPrimeLigneAstreinteEnt rapportPrimeLigneAstreinte = await rapportPrimeLigneAstreinteRepository.GetRapportPrimeLigneAstreinteAsync(astreinte.AstreintId, rapportPrimeLigneId);

                rapportPrimeLigneAstreintes.Add(rapportPrimeLigneAstreinte ?? CreateNewAstreinte(rapportPrimeLigneId, astreinte));
            }

            return rapportPrimeLigneAstreintes;
        }

        private RapportPrimeLigneAstreinteEnt CreateNewAstreinte(int rapportPrimeLigneId, AstreinteEnt astreinte)
        {
            var rapportPrimeLigneAstreinte = new RapportPrimeLigneAstreinteEnt
            {
                AstreinteId = astreinte.AstreintId,
                RapportPrimeLigneId = rapportPrimeLigneId
            };

            if (astreinte.AstreintId == 0)
            {
                rapportPrimeLigneAstreinte.Astreinte = astreinte;
            }

            return rapportPrimeLigneAstreinte;
        }
    }
}
