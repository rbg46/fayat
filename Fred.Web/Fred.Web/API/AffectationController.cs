using AutoMapper;
using Fred.Business.Affectation;
using Fred.Entities.Affectation;
using Fred.Web.Shared.Models.Affectation;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace Fred.Web.API
{
    public class AffectationController : ApiControllerBase
    {
        #region Private attribute

        private readonly IAffectationManager affectationManager;
        private readonly IMapper mapper;

        #endregion

        #region Constructor

        public AffectationController(IAffectationManager affectationManager, IMapper mapper)
        {
            this.affectationManager = affectationManager;
            this.mapper = mapper;
        }

        #endregion

        #region Public method

        /// <summary>
        /// Ajouter ou modifier une list des affectations
        /// </summary>
        /// <param name="affectationsList">Liste des affectations</param>
        /// <returns>Http response message</returns>
        [HttpPost]
        [Route("api/Affectation/AddOrUpdateAffectationList")]
        public HttpResponseMessage AddOrUpdateAffectationList(CalendarAffectationViewModel affectationsList)
        {
            return this.Post(() => affectationManager.AddOrUpdateAffectationList(mapper.Map<CalendarAffectationViewEnt>(affectationsList)));
        }

        /// <summary>
        /// Get list des affectations by ci identifier
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="dateDebut">Date debut de la semaine</param>
        /// <param name="dateFin">Date fin de la semaine</param>
        /// <returns>Htttp response message</returns>
        [HttpGet]
        [Route("api/Affectation/GetAffectationListByCi/{ciId}/{dateDebut}/{dateFin}")]
        public HttpResponseMessage GetAffectationListByCi(int ciId, DateTime dateDebut, DateTime dateFin)
        {
            return this.Get(() => this.mapper.Map<CalendarAffectationViewModel>(affectationManager.GetAffectationListByCi(ciId, dateDebut, dateFin)));
        }

        /// <summary>
        /// Supprimer une liste des affectations 
        /// </summary>
        /// <param name="ciId">Ci Id</param>
        /// <param name="affectationListIds">liste des affectations identifiers</param>
        /// <returns>Htttp response message</returns>
        [HttpPost]
        [Route("api/Affectation/DeleteAffectations/{ciId}")]
        public HttpResponseMessage DeleteAffectations(int ciId, List<int> affectationListIds)
        {
            return this.Post(() => affectationManager.DeleteAffectations(ciId, affectationListIds));
        }

        /// <summary>
        /// Verifier si le personnel a des pointages avant de supprimer son affectation
        /// </summary>
        /// <param name="personnelId">Personnel identifier</param>
        /// <param name="ciId">Ci identifier</param>
        /// <returns>Htttp response message</returns>
        [HttpGet]
        [Route("api/Affectation/CheckPersonnelBeforDelete/{personnelId}/{ciId}")]
        public HttpResponseMessage CheckPersonnelBeforDelete(int personnelId, int ciId)
        {
            return this.Get(() => affectationManager.CheckPersonnelBeforeDelete(personnelId, ciId).ToString());
        }
        #endregion
    }
}