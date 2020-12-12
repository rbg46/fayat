using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Fred.Business.Journal;
using Fred.Entities.Journal;
using Fred.Entities.Referential;
using Fred.Web.Models.Journal;
using Fred.Web.Models.Referential;
using Fred.Web.Shared.Models.Journal;

namespace Fred.Web.API
{
    /// <summary>
    /// Controller WEB API des journaux comptables
    /// </summary>
    public class JournalController : ApiControllerBase
    {
        /// <summary>
        /// Manager business des journaux comptables
        /// </summary>
        protected readonly IJournalManager JournalMgr;

        /// <summary>
        /// Mapper Model / ModelVue
        /// </summary>
        protected readonly IMapper Mapper;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="JournalController"/>
        /// </summary>
        /// <param name="journalMgr">Manager des journaux comptables</param>
        /// <param name="mapper">Mapper Model / ModelVue</param>
        public JournalController(IJournalManager journalMgr, IMapper mapper)
        {
            this.JournalMgr = journalMgr;
            this.Mapper = mapper;
        }

        /// <summary>
        /// Méthode de recherche des journaux comptables
        /// </summary>
        /// <param name="filters">Filtre de recherche</param>
        /// <returns>Retourne la liste des journaux comptables</returns>
        [HttpPost]
        [Route("api/Journal/GetFilteredJournalList")]
        public HttpResponseMessage GetFilteredJournalList(SearchValueAndSocietyActiveModel filters)
        {
            var journauxComptables = this.JournalMgr.GetFilteredJournalList(this.Mapper.Map<SearchCriteriaEnt<JournalEnt>>(filters));
            return Request.CreateResponse(HttpStatusCode.OK, this.Mapper.Map<IEnumerable<JournalModel>>(journauxComptables));
        }

        /// <summary>
        /// Indique si le journal comptable est utilisé
        /// </summary>
        /// <param name="journalId">Identifiant du journal comptable</param>
        /// <returns>True si le journal comptable est utilisé</returns>
        [HttpGet]
        [Route("api/Journal/IsAlreadyUsed/{journalId}")]
        public HttpResponseMessage IsAlreadyUsed(int journalId)
        {
            return Get(() => JournalMgr.IsAlreadyUsed(journalId));
        }

        /// <summary>
        /// Crée un nouveau journal comptable
        /// </summary>
        /// <param name="journal">Journal à créer</param>
        /// <returns>Journal créée</returns>
        [HttpPost]
        [Route("api/Journal/CreateJournal/")]
        public HttpResponseMessage CreateJournal(JournalModel journal)
        {
            return Post(() => Mapper.Map<JournalModel>(JournalMgr.AddJournal(Mapper.Map<JournalEnt>(journal))));
        }

        /// <summary>
        /// Mets à jour un journal comptable
        /// </summary>
        /// <param name="journal">Journal à mettre à jour</param>
        /// <returns>Journal modifié</returns>
        [HttpPost]
        [Route("api/Journal/UpdateJournal")]
        public HttpResponseMessage UpdateJournal(JournalModel journal)
        {
            return Post(() => Mapper.Map<JournalModel>(JournalMgr.UpdateJournal(Mapper.Map<JournalEnt>(journal))));
        }

        /// <summary>
        /// Mets à jour un journal comptable avec uniquement les champs nécessaire (Code + Libelle + DateCloture)
        /// </summary>
        /// <param name="journal">Journal à mettre à jour</param>
        /// <returns>Journal modifié</returns>
        [HttpPost]
        [Route("api/Journal/UpdateJournalCodeLibelleDateCloture")]
        public HttpResponseMessage UpdateJournalCodeLibelleDateCloture(JournalModel journal)
        {
            List<Expression<Func<JournalEnt, object>>> fieldToUpdate = new List<Expression<Func<JournalEnt, object>>>
            {
                x => x.Code,
                x => x.Libelle,
                x => x.DateCloture
            };

            return Post(() => JournalMgr.UpdateJournal(journal, fieldToUpdate));
        }

        /// <summary>
        /// Supprime un journal comptable
        /// </summary>
        /// <param name="journalId">Identifiant du journal à supprimer</param>
        /// <returns>Code de retour de la requête</returns>
        [HttpDelete]
        [Route("api/Journal/DeleteJournal/{journalId}")]
        public HttpResponseMessage DeleteJournal(int journalId)
        {
            return Delete(() => JournalMgr.DeleteJournal(journalId));
        }

        /// <summary>
        /// Récupère la liste des journaux comptable pour une société
        /// </summary>
        /// <param name="societeId">Identifiant de la societe</param>
        /// <returns>Code de retour de la requête</returns>
        [HttpGet]
        [Route("api/Journal/GetJournaux/{societeId}")]
        public HttpResponseMessage GetJournaux(int societeId)
        {
            return Get(() => JournalMgr.GetJournaux(societeId));
        }

        /// <summary>
        /// Récupère la liste des journaux comptable actifs pour une société
        /// </summary>
        /// <param name="societeId">Identifiant de la societe</param>
        /// <returns>Code de retour de la requête</returns>
        [HttpGet]
        [Route("api/Journal/GetJournauxActifs/{societeId}")]
        public HttpResponseMessage GetJournauxActifs(int societeId)
        {
            return Get(() => JournalMgr.GetJournauxActifs(societeId));
        }

        /// <summary>
        /// Mets à jour une liste de journaux comptable
        /// </summary>
        /// <param name="journaux"><see cref="JournalFamilleODModel"/></param>
        /// <returns>Code de retour de la requête</returns>
        [HttpPost]
        [Route("api/Journal/UpdateJournaux")]
        public HttpResponseMessage UpdateJournaux(List<JournalFamilleODModel> journaux)
        {
            return Post(() => JournalMgr.UpdateJournaux(journaux));
        }
    }
}
