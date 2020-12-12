using AutoMapper;
using Fred.Business.CI;
using Fred.Business.DatesClotureComptable;
using Fred.Business.ExternalService.EcritureComptable;
using Fred.Business.RepartitionEcart;
using Fred.Business.Valorisation;
using Fred.Entities.CI;
using Fred.Entities.DatesClotureComptable;
using Fred.Web.Models.DatesClotureComptable;
using Fred.Web.Shared.Models.DatesClotureComptable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fred.Web.API
{
    public class DatesClotureComptableController : ApiControllerBase
    {
        private readonly IDatesClotureComptableManager dateClotureManager;
        private readonly IValorisationManager valorisationManager;
        private readonly IMapper mapper;
        private readonly IRepartitionEcartManager repartitionEcartManager;
        private readonly IEcritureComptableManagerExterne ecritureComptableManagerExterne;
        private readonly ICIManager ciManager;

        public DatesClotureComptableController(
          IDatesClotureComptableManager dateClotureManager,
          IMapper mapper,
          IRepartitionEcartManager repartitionEcartManager,
          IValorisationManager valorisationManager,
          IEcritureComptableManagerExterne ecritureComptableManagerExterne,
          ICIManager ciManager)
        {
            this.dateClotureManager = dateClotureManager;
            this.mapper = mapper;
            this.repartitionEcartManager = repartitionEcartManager;
            this.valorisationManager = valorisationManager;
            this.ecritureComptableManagerExterne = ecritureComptableManagerExterne;
            this.ciManager = ciManager;
        }

        /// <summary>
        /// Méthode GET de récupération des DatesClotureComptables
        /// </summary>
        /// <param name="ciId">Le CI</param>
        /// <param name="year">L' année</param>
        /// <returns>Retourne la liste des DatesClotureComptables</returns>
        [HttpGet]
        [Route("api/DatesClotureComptable/{ciId}/{year}/")]
        public IHttpActionResult Search(int ciId, int year)
        {
            return Ok(mapper.Map<IEnumerable<DatesClotureComptableModel>>(dateClotureManager.GetCIListDatesClotureComptableByIdAndYear(ciId, year)));
        }

        /// <summary>
        /// Indique si la période comptable est fermée pour un CI
        /// </summary>
        /// <param name="ciId">Le CI</param>
        /// <param name="year">L'année laquelle on souhaite faire le test de la cloture comptable</param>
        /// <param name="month">Le mois avec lequel on souhaite faire le test de la cloture comptable</param>
        /// <returns>Renvoi vrai si la periode comptable est clôturée</returns>
        [HttpGet]
        [Route("api/DatesClotureComptable/IsPeriodClosed/{ciId}/{year}/{month}")]
        public IHttpActionResult IsPeriodClosed(int ciId, int year, int month)
        {
            return Ok(dateClotureManager.IsPeriodClosed(ciId, year, month));
        }

        /// <summary>
        /// Renvoie la période de fermeture comptable pour une liste de CI
        /// </summary>
        /// <param name="ciIds">Les CI</param>
        /// <param name="year">L'année laquelle on souhaite faire le test de la cloture comptable</param>
        /// <param name="month">Le mois avec lequel on souhaite faire le test de la cloture comptable</param>
        /// <returns>Renvoi vrai si la periode comptable est clôturée</returns>
        [HttpGet]
        [Route("api/DatesClotureComptable/GetDatesClotureComptableForCiIds/")]
        public IHttpActionResult GetDatesClotureComptableForCiIds(string ciIds, int year, int month)
        {
            string[] ciIdsSplitted = ciIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            List<int> ciIdentifiants = ciIdsSplitted.Select(int.Parse).ToList();
            return Ok(dateClotureManager.GetDatesClotureComptableForCiIds(ciIdentifiants, year, month));
        }

        /// <summary>
        /// Renvoie la période de fermeture comptable pour un CI
        /// </summary>
        /// <param name="ciId">Le CI</param>
        /// <param name="year">L'année laquelle on souhaite faire le test de la cloture comptable</param>
        /// <param name="month">Le mois avec lequel on souhaite faire le test de la cloture comptable</param>
        /// <returns>Renvoi vrai si la periode comptable est clôturée</returns>
        [HttpGet]
        [Route("api/DatesClotureComptable/GetDatesClotureComptableForCiId/")]
        public IHttpActionResult GetDatesClotureComptableForCiId(int ciId, int year, int month)
        {
            return Ok(dateClotureManager.GetDatesClotureComptableForCiId(ciId, year, month));
        }

        /// <summary>
        /// Renvoie la liste des DateCloturesComptable pour une pour liste de <see cref="DateClotureComptableWithOptionModel"/>
        /// </summary>
        /// <param name="dateClotureComptableWithOptionModels">Liste de <see cref="DateClotureComptableWithOptionModel"/></param>
        /// <returns>la liste des DateCloturesComptableEnt</returns>
        [HttpPost]
        [Route("api/DatesClotureComptable/GetDatesClotureComptableFromList/")]
        public IHttpActionResult GetDatesClotureComptableFromList(List<DateClotureComptableWithOptionModel> dateClotureComptableWithOptionModels)
        {
            return Ok(dateClotureManager.GetDatesClotureComptableForCiId(dateClotureComptableWithOptionModels));
        }

        /// <summary>
        /// Ouvre et ferme une liste de periode comptable depuis une liste de <see cref="DatesClotureComptableEnt"/> en prenant regénérant les OD associés
        /// </summary>
        /// <param name="datesClotureComptableEnts">liste de <see cref="DatesClotureComptableEnt"/></param>
        /// <returns><see cref="HttpResponseMessage"/></returns>
        [HttpPost]
        [Route("api/DatesClotureComptable/OpenAndCloseDatesClotureComptableFromList/")]
        public async Task<IHttpActionResult> OpenAndCloseDatesClotureComptableFromListAsync(List<DatesClotureComptableEnt> datesClotureComptableEnts)
        {
            foreach (DatesClotureComptableEnt datesClotureComptable in datesClotureComptableEnts)
            {
                dateClotureManager.ModifyDatesClotureComptable(mapper.Map<DatesClotureComptableEnt>(datesClotureComptable));

                DateTime dateComptable = new DateTime(datesClotureComptable.Annee, datesClotureComptable.Mois, 15);
                await repartitionEcartManager.DeClotureAsync(datesClotureComptable.CiId, dateComptable).ConfigureAwait(false);
                await repartitionEcartManager.ClotureAsync(datesClotureComptable.CiId, dateComptable).ConfigureAwait(false);
                valorisationManager.UpdateVerrouPeriodeValorisation(datesClotureComptable.CiId, datesClotureComptable.Annee, datesClotureComptable.Mois, true);
            }
            return Ok();
        }

        /// <summary>
        /// Indique si un CI est fermée pour une periode données
        /// </summary>
        /// <param name="ciId">Le CI</param>
        /// <param name="startDate">Date de début</param>
        /// <param name="endDate">Date de fin</param>
        /// <returns>Renvoi vrai si la periode comptable est clôturée</returns>
        [HttpGet]
        [Route("api/DatesClotureComptable/IsPeriodClosedForRange/{ciId}/{startDate?}/{endDate}")]
        public IHttpActionResult GetClosedDatesClotureComptable(int ciId, DateTime? startDate, DateTime endDate)
        {
            startDate = startDate ?? new DateTime(1900, 1, 1);

            return Ok(dateClotureManager.GetClosedDatesClotureComptable(ciId, startDate.Value, endDate));
        }

        /// <summary>
        /// Méthode GET de récupération des DatesClotureComptables avec le mois suivant et precedent.
        /// </summary>
        /// <param name="ciId">Le CI</param>
        /// <param name="year">L' année</param>
        /// <returns>Retourne la liste des DatesClotureComptables</returns>
        [HttpGet]
        [Route("api/DatesClotureComptable/GetYearAndPreviousNextMonths/{ciId}/{year}/")]
        public IHttpActionResult GetYearAndPreviousNextMonths(int ciId, int year)
        {
            return Ok(mapper.Map<IEnumerable<DatesClotureComptableModel>>(dateClotureManager.GetYearAndPreviousNextMonths(ciId, year)));
        }

        /// <summary>
        /// Méthode de creation de DatesClotureComptables
        /// </summary>
        /// <param name="datesClotureComptableModel">datesClotureComptableModel</param>
        /// <returns>Retourne un DatesClotureComptableModel</returns>
        [HttpPost]
        [Route("api/DatesClotureComptable/Add")]
        public async Task<IHttpActionResult> CreateAsync(DatesClotureComptableModel datesClotureComptableModel)
        {
            DatesClotureComptableEnt datesClotureComptableEnt = mapper.Map<DatesClotureComptableEnt>(datesClotureComptableModel);
            DatesClotureComptableEnt result = dateClotureManager.CreateDatesClotureComptable(mapper.Map<DatesClotureComptableEnt>(datesClotureComptableEnt));
            bool monthIsClosed = dateClotureManager.IsPeriodClosed(datesClotureComptableModel.CiId, datesClotureComptableModel.Annee, datesClotureComptableModel.Mois);

            if (monthIsClosed)
            {
                await ProcessImportAndClotureAsync(datesClotureComptableModel);
            }
            else
            {
                DateTime dateComptable = new DateTime(datesClotureComptableModel.Annee, datesClotureComptableModel.Mois, 15);
                await repartitionEcartManager.DeClotureAsync(datesClotureComptableModel.CiId, dateComptable).ConfigureAwait(false);
            }
            return Ok(mapper.Map<DatesClotureComptableModel>(result));

        }

        private async Task ProcessImportAndClotureAsync(DatesClotureComptableModel datesClotureComptableModel)
        {
            DateTime dateComptable = new DateTime(datesClotureComptableModel.Annee, datesClotureComptableModel.Mois, 15);
            List<int> ciids = new List<int>();
            ciids.Add(datesClotureComptableModel.CiId);
            CIEnt ciEnt = ciManager.GetCiById(ciids[0], true);
            await ecritureComptableManagerExterne.ImportEcrituresComptablesFromAnaelAsync(dateComptable, ciids, ciEnt.SocieteId, ciEnt.Societe.Code);
            await repartitionEcartManager.ClotureAsync(datesClotureComptableModel.CiId, dateComptable).ConfigureAwait(false);
            valorisationManager.UpdateVerrouPeriodeValorisation(datesClotureComptableModel.CiId, datesClotureComptableModel.Annee, datesClotureComptableModel.Mois, true);
        }

        /// <summary>
        /// Permet de réouvrir une période pour un ci pour une liste de date
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateTimes">List de date</param>
        /// <returns>Retourne un DatesClotureComptableModel</returns>
        [HttpPost]
        [Route("api/DatesClotureComptable/OpenPeriode")]
        public async Task<IHttpActionResult> OpenPeriodeAsync(string ciId, List<DateTime> dateTimes)
        {
            foreach (DateTime date in dateTimes)
            {
                bool monthIsClosed = dateClotureManager.IsPeriodClosed(int.Parse(ciId), date.Year, date.Month);

                if (monthIsClosed)
                {
                    DatesClotureComptableEnt datesClotureComptableEnt = dateClotureManager.Get(int.Parse(ciId), date.Year, date.Month);
                    datesClotureComptableEnt.DateModification = DateTime.UtcNow;
                    datesClotureComptableEnt.DateCloture = null;
                    dateClotureManager.ModifyDatesClotureComptable(datesClotureComptableEnt);
                    await repartitionEcartManager.DeClotureAsync(int.Parse(ciId), date).ConfigureAwait(false);
                }
            }

            return Ok();
        }

        /// <summary>
        /// Méthode de mise a jour de DatesClotureComptables
        /// </summary>
        /// <param name="datesClotureComptableModel">datesClotureComptableModel</param>
        /// <returns>Retourne un DatesClotureComptableModel</returns>
        [HttpPut]
        [Route("api/DatesClotureComptable/Update")]
        public async Task<IHttpActionResult> UpdateAsync(DatesClotureComptableModel datesClotureComptableModel)
        {
            DatesClotureComptableEnt datesClotureComptableEnt = mapper.Map<DatesClotureComptableEnt>(datesClotureComptableModel);

            DatesClotureComptableEnt result = dateClotureManager.ModifyDatesClotureComptable(mapper.Map<DatesClotureComptableEnt>(datesClotureComptableEnt));

            bool monthIsClosed = dateClotureManager.IsPeriodClosed(datesClotureComptableModel.CiId, datesClotureComptableModel.Annee, datesClotureComptableModel.Mois);
            if (monthIsClosed)
            {
                await ProcessImportAndClotureAsync(datesClotureComptableModel);
            }
            else
            {
                DateTime dateComptable = new DateTime(datesClotureComptableModel.Annee, datesClotureComptableModel.Mois, 15);
                await repartitionEcartManager.DeClotureAsync(datesClotureComptableModel.CiId, dateComptable).ConfigureAwait(false);
                // Désactivé en attendant le retour de la MOA
                ////valorisationManager.UnlockValorisation(datesClotureComptableModel.CiId, datesClotureComptableModel.Annee, datesClotureComptableModel.Mois)
            }
            return Ok(mapper.Map<DatesClotureComptableModel>(result));
        }

        [HttpGet]
        [Route("api/DatesClotureComptable/GePreviousCurrentAndNextMonths/{ciId}")]
        public IHttpActionResult GetPreviousCurrentAndNextMonths(int ciId)
        {

            IEnumerable<PeriodeClotureEnt> result = dateClotureManager.GetPreviousCurrentAndNextMonths(ciId);
            return Ok(mapper.Map<IEnumerable<PeriodeClotureModel>>(result));

        }
    }
}
