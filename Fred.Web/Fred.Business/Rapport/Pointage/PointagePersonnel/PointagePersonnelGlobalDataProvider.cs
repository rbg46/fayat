using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.Affectation;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Personnel;
using Fred.Business.Rapport.Pointage.PointagePersonnel.Interfaces;
using Fred.Business.RapportTache;
using Fred.Business.Utilisateur;
using Fred.Entities;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;

namespace Fred.Business.Rapport.Pointage.PointagePersonnel
{
    public class PointagePersonnelGlobalDataProvider : IPointagePersonnelGlobalDataProvider
    {
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IAffectationManager affectationManager;
        private readonly IDatesClotureComptableManager datesClotureComptableManager;
        private readonly IRapportTacheManager rapportTacheManager;
        private readonly IPersonnelManager personnelManager;

        public PointagePersonnelGlobalDataProvider(
            IUtilisateurManager utilisateurManager,
            IAffectationManager affectationManager,
            IDatesClotureComptableManager datesClotureComptableManager,
            IRapportTacheManager rapportTacheManager,
            IPersonnelManager personnelManager)
        {
            this.utilisateurManager = utilisateurManager;
            this.affectationManager = affectationManager;
            this.datesClotureComptableManager = datesClotureComptableManager;
            this.rapportTacheManager = rapportTacheManager;
            this.personnelManager = personnelManager;
        }

        /// <summary>
        /// Permet de recuperer les pointages vace les infomations calculer et les message d'erreurs
        /// </summary>
        /// <param name="listPointages">Les lignes de rapports</param>
        /// <param name="personnelId">l'id du personnel</param>
        /// <param name="periode">le mois sur lequel la recheche est effectuée.</param>
        /// <returns>Les données necessaires pour calculé les champs caclulés et determiner les message d'erreurs</returns>
        public async Task< PointagePersonnelGlobalData> GetDataForFormatRapportLignesForViewAsync(IEnumerable<RapportLigneEnt> listPointages, int personnelId, DateTime periode)
        {
            var currentUserId = utilisateurManager.GetContextUtilisateurId();
            var currentUserIsInGroupeGFES = utilisateurManager.IsUtilisateurOfGroupe(Constantes.CodeGroupeFES);
            var currentUserCiIds = await GetCurrentUserIdsAsync(currentUserId, periode, listPointages).ConfigureAwait(false);
            var ciIdsOfRapportLignes = listPointages.Select(p => p.CiId).Distinct().ToList();
            var datesPointagesOfPointages = listPointages.Select(p => p.DatePointage).Distinct().ToList();
            var rapportIdsOfPointages = listPointages.Select(p => p.RapportId).Distinct().ToList();
            var datesClotureComptablesForCisOfPointages = this.datesClotureComptableManager.GetDatesClotureComptableForCiIds(ciIdsOfRapportLignes, periode.Year, periode.Month);
            var astreintesOfPersonnelOnCisOnDates = affectationManager.GetAstreintes(ciIdsOfRapportLignes, personnelId, datesPointagesOfPointages);
            var rapportsTachesOfRapportsOfRapportsLignes = rapportTacheManager.GetRapportTachesByRapportIds(rapportIdsOfPointages);

            return new PointagePersonnelGlobalData()
            {
                PersonnelId = personnelId,
                CurrentUserId = currentUserId,
                CurrentUserIsInGroupeGFES = currentUserIsInGroupeGFES,
                CurrentUserCiIds = currentUserCiIds,
                CiIdsOfRapportLignes = ciIdsOfRapportLignes,
                DatesPointagesOfRapportsLignes = datesPointagesOfPointages,
                DatesClotureComptablesForCisOfPointages = datesClotureComptablesForCisOfPointages,
                AstreintesOfPersonnelOnCisOnDates = astreintesOfPersonnelOnCisOnDates,
                RapportsTachesOfRapportsOfRapportsLignes = rapportsTachesOfRapportsOfRapportsLignes
            };
        }

        private async Task<List<int>> GetCurrentUserIdsAsync(int currentUserId, DateTime periode, IEnumerable<RapportLigneEnt> listPointages)
        {
            List<int> retunedValue = new List<int>();

            retunedValue = this.utilisateurManager.GetAllCIbyUser(currentUserId)?.ToList();

            // CB 13923 : Pour FES => le gestionnaire de paye doit voir le personnel de la/les société(s) auxquels il est habilité, 
            // ET doit pouvoir modifier l'ensemble du pointage de son personnel, qu'il pointe sur des CI de sociétés différentes. 
            if (this.utilisateurManager.IsUtilisateurOfGroupe("GFES"))
            {
                int periodeChantier = 0;
                if (int.TryParse($"{periode.Year}{periode.Month.ToString("00", CultureInfo.InvariantCulture)}", out periodeChantier))
                {
                    IEnumerable<PersonnelEnt> personnelsList = await this.personnelManager.SearchLightAsync(new SearchLightPersonnelModel
                    {
                        PeriodeChantier = periodeChantier,
                        Page = 1,
                        PageSize = int.MaxValue
                    }).ConfigureAwait(false);

                    if (personnelsList != null && personnelsList.Any())
                    {
                        List<int> personnelsIdList = personnelsList.Select(p => p.PersonnelId).Distinct().ToList();
                        List<int> cisList = listPointages.Where(p => p.PersonnelId.HasValue && personnelsIdList.Contains(p.PersonnelId.Value)).Select(p => p.CiId).Distinct().ToList();

                        retunedValue.AddRange(cisList);
                        retunedValue = retunedValue.Distinct().ToList();
                    }
                }
            }

            return retunedValue;
        }
    }
}
