using AutoMapper;
using Fred.Business.CI;
using Fred.Business.DatesClotureComptable;
using Fred.Business.ExternalService.EcritureComptable;
using Fred.Business.Societe;
using Fred.Entities.CI;
using Fred.Entities.CloturesPeriodes;
using Fred.Web.Models.CloturesPeriodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fred.Web.API
{
    /// <summary>
    /// Controller WEB API des CloturesPeriodes
    /// </summary>
    public class CloturesPeriodesController : ApiControllerBase
    {
        private readonly IMapper mapper;

        private readonly ICloturesPeriodesManager cloturesPeriodesManager;
        private readonly IEcritureComptableManagerExterne ecritureComptableManagerExterne;
        private readonly ICIManager ciManager;
        private readonly ISocieteManager societeManager;

        public CloturesPeriodesController(IMapper mapper, ICloturesPeriodesManager cloturesPeriodesManager, IEcritureComptableManagerExterne ecritureComptableManagerExterne, ICIManager ciManager, ISocieteManager societeManager)
        {
            this.mapper = mapper;
            this.cloturesPeriodesManager = cloturesPeriodesManager;
            this.ecritureComptableManagerExterne = ecritureComptableManagerExterne;
            this.ciManager = ciManager;
            this.societeManager = societeManager;
        }

        /// <summary>
        /// POST Récupération des résultats de la recherche en fonction du filtre
        /// </summary>
        /// <param name="filter">Filtre de la date clôture comptable</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpPost]
        [Route("api/clotures_periodes/dates_cloture_comptable")]
        public IHttpActionResult SearchFilter(SearchCloturesPeriodesForCiModel filter)
        {
            return Ok(cloturesPeriodesManager.SearchFilter(mapper.Map<SearchCloturesPeriodesForCiEnt>(filter)));
        }

        /// <summary>
        /// Permet de clôturer une liste de periode comptable
        /// </summary>
        /// <param name="payload">payload Thanks You Captain Oblivious ! </param>
        /// <returns>Vrai si la clôture s'est bien passée</returns>
        [HttpPost]
        [Route("api/clotures_periodes/dates_cloture_comptable/depenses")]
        public async Task<IHttpActionResult> CloturerAllDepensesInclusSelectionnesAsync([FromBody] PlageCisDatesClotureComptableDto payload)
        {
            DateTime date = payload.Date;
            int annee = payload.Filter.Year;
            int mois = payload.Filter.Month;
            SearchCloturesPeriodesForCiEnt filter = payload.Filter;
            List<int> identifiantsSelected = payload.Identifiants;
            List<CIEnt> cIEnts = ciManager.GetCisByIds(identifiantsSelected);
            string societeCode = societeManager.GetSocieteById(cIEnts.Select(q => q.SocieteId).FirstOrDefault().Value).Code;

            // Décloturer un ou plusieurs CIs
            if (payload.IsModeDecloturer)
            {
                return Ok(await cloturesPeriodesManager.DecloturerSeulementDepensesSelectionneesAsync(date, annee, mois, mapper.Map<SearchCloturesPeriodesForCiEnt>(filter), identifiantsSelected).ConfigureAwait(false));
            }

            // Clôturer tous les CIs
            if (payload.IsModeBlocToutSelectionner)
            {
                await ecritureComptableManagerExterne.ImportEcrituresComptablesFromAnaelAsync(new DateTime(annee, mois, 15), identifiantsSelected, cIEnts.Select(q => q.SocieteId).FirstOrDefault(), societeCode).ConfigureAwait(false);
                return Ok(await cloturesPeriodesManager.CloturerToutesDepensesSaufSelectionneesAsync(date, annee, mois, mapper.Map<SearchCloturesPeriodesForCiEnt>(filter), identifiantsSelected).ConfigureAwait(false));
            }

            // Clôturer les CIs sélectionnés
            await ecritureComptableManagerExterne.ImportEcrituresComptablesFromAnaelAsync(new DateTime(annee, mois, 15), identifiantsSelected, cIEnts.Select(q => q.SocieteId).FirstOrDefault(), societeCode).ConfigureAwait(false);
            return Ok(await cloturesPeriodesManager.CloturerSeulementDepensesSelectionneesAsync(date, annee, mois, mapper.Map<SearchCloturesPeriodesForCiEnt>(filter), identifiantsSelected).ConfigureAwait(false));
        }
    }
}
