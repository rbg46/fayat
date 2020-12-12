using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Fred.Business.IndemniteDeplacement;
using Fred.Business.Referential.TypeRattachement;
using Fred.Entities.IndemniteDeplacement;
using Fred.Web.Models.IndemniteDeplacement;

namespace Fred.Web.API
{
    /// <summary>
    ///   Controller WEB API des Indemnites de Déplacement
    /// </summary>
    public class IndemniteDeplacementController : ApiControllerBase
    {
        private readonly IIndemniteDeplacementManager indemniteDeplacementMgr;
        private readonly IMapper mapper;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="IndemniteDeplacementController" />.
        /// </summary>
        /// <param name="indemniteDeplacementMgr">Manager de IndemniteDeplacement</param>    
        /// <param name="mapper">Mapper Model / ModelVue</param>
        public IndemniteDeplacementController(IIndemniteDeplacementManager indemniteDeplacementMgr, IMapper mapper)
        {
            this.indemniteDeplacementMgr = indemniteDeplacementMgr;
            this.mapper = mapper;
        }

        /// <summary>
        /// Méthode GET de récupération d'une nouvelle instance d'indemnité de déplacement intialisée.
        /// </summary>
        /// <returns>Retourne une nouvelle instance code zone deplacement intialisée</returns>
        [HttpGet]
        [Route("api/IndemniteDeplacement/New")]
        public HttpResponseMessage New()
        {
            return Get(() => this.mapper.Map<IndemniteDeplacementModel>(this.indemniteDeplacementMgr.GetNewIndemniteDeplacement()));
        }

        /// <summary>
        /// Méthode GET de recherche des CodeZoneDeplacements
        /// </summary>
        /// <param name="idPersonnel">Identifiant du Personnel</param>
        /// <returns>Retourne la liste des CodeZoneDeplacements par personnel</returns>
        [HttpGet]
        [Route("api/IndemniteDeplacement/GetIndemniteDeplacementByPersonnel/{idPersonnel}")]
        public HttpResponseMessage GetIndemniteDeplacementByPersonnel(int idPersonnel)
        {
            return Get(() =>
            {
                var indemniteDeplacements = this.mapper.Map<IEnumerable<IndemniteDeplacementModel>>(this.indemniteDeplacementMgr.GetIndemniteDeplacementByPersonnel(idPersonnel));

                foreach (IndemniteDeplacementModel indemn in indemniteDeplacements)
                {
                    if (!indemn.DateSuppression.HasValue || (indemn.DateSuppression.HasValue && indemn.DateSuppression.Value == DateTime.MinValue))
                    {
                        indemn.Actif = true;
                    }
                }
                return this.mapper.Map<IEnumerable<IndemniteDeplacementModel>>(indemniteDeplacements);
            });
        }

        /// <summary>
        /// Méthode GET de recherche des CodeZoneDeplacements
        /// </summary>
        /// <param name="idPersonnel">Identifiant du personnel</param>
        /// <param name="idCi">Identifiant du CI</param>
        /// <returns>Retourne la liste des CodeZoneDeplacements</returns>
        [HttpGet]
        [Route("api/IndemniteDeplacement/GetIndemniteDeplacementByCi/{idPersonnel}/{idCi}")]
        public HttpResponseMessage GetIndemniteDeplacementByCi(int idPersonnel, int idCi)
        {
            return Get(() =>
            {
                var indemniteDeplacements = this.mapper.Map<IEnumerable<IndemniteDeplacementModel>>(this.indemniteDeplacementMgr.GetIndemniteDeplacementByCi(idPersonnel, idCi));

                foreach (IndemniteDeplacementModel indemn in indemniteDeplacements)
                {
                    if (!indemn.DateSuppression.HasValue || (indemn.DateSuppression.HasValue && indemn.DateSuppression.Value == DateTime.MinValue))
                    {
                        indemn.Actif = true;
                    }
                }
                return this.mapper.Map<IEnumerable<IndemniteDeplacementModel>>(indemniteDeplacements);
            });
        }

        /// <summary>
        ///   POST Création d'une indemnité de déplacement
        /// </summary>
        /// <param name="indemniteDeplacementModel">Indemnité de déplacement à créer</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpPost]
        [Route("api/IndemniteDeplacement/")]
        public HttpResponseMessage Create(IndemniteDeplacementModel indemniteDeplacementModel)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    if (this.indemniteDeplacementMgr.IsIndemniteDeplacementUnique(indemniteDeplacementModel.PersonnelId, indemniteDeplacementModel.CiId))
                    {
                        indemniteDeplacementModel.CodeZoneDeplacementId = this.indemniteDeplacementMgr.AddIndemniteDeplacement(this.mapper.Map<IndemniteDeplacementEnt>(indemniteDeplacementModel));
                        return Request.CreateResponse(HttpStatusCode.Created, indemniteDeplacementModel);
                    }

                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ControllerRessources.Indemnite_Deplacement_Controller_IndemniteExistante);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        ///   PUT Mise à jour d'une indemnité de déplacement
        /// </summary>
        /// <param name="indemniteDeplacementModel">Indemnité de déplacement</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpPut]
        [Route("api/IndemniteDeplacement/")]
        public HttpResponseMessage Update(IndemniteDeplacementModel indemniteDeplacementModel)
        {
            return Put(() =>
            {
                if (this.indemniteDeplacementMgr.IsIndemniteDeplacementUnique(indemniteDeplacementModel.PersonnelId, indemniteDeplacementModel.CiId, indemniteDeplacementModel.IndemniteDeplacementId))
                {
                    indemniteDeplacementModel.IndemniteDeplacementId = this.indemniteDeplacementMgr.UpdateIndemniteDeplacementWithHistorical(this.mapper.Map<IndemniteDeplacementEnt>(indemniteDeplacementModel));

                    return indemniteDeplacementModel;
                }
                return indemniteDeplacementModel;
            });
        }

        /// <summary>
        ///   DELETE Suppression d'une indemnité de déplacement
        /// </summary>
        /// <param name="indemniteDepId">Indemnite de déplacement</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpDelete]
        [Route("api/IndemniteDeplacement/{indemniteDepId}")]
        public HttpResponseMessage Delete(int indemniteDepId)
        {
            return Delete(() => this.indemniteDeplacementMgr.DeleteIndemniteDeplacementById(indemniteDepId));
        }

        /// <summary>
        /// Méthode POST de récupération des filtres de recherche sur IndemniteDeplacement
        /// </summary>
        /// <returns>Retourne la liste des filtres de recherche sur IndemniteDeplacement</returns>
        [HttpGet]
        [Route("api/IndemniteDeplacement/Filter/")]
        public HttpResponseMessage Filters()
        {
            return Get(() => this.mapper.Map<SearchIndemniteDeplacementModel>(this.indemniteDeplacementMgr.GetDefaultFilter()));
        }

        /// <summary>
        /// Méthode GET de recherche des codes zone deplacement
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne la liste des codes zone deplacement</returns>
        [HttpPost]
        [Route("api/IndemniteDeplacement/SearchIndemniteDeplacementByCiId/{personnelId}/{ciId}")]
        public HttpResponseMessage SearchIndemniteDeplacementByCiId(int personnelId, int ciId)
        {
            return Post(() =>
            {
                var indemniteDeplacements = this.mapper.Map<IEnumerable<IndemniteDeplacementModel>>(this.indemniteDeplacementMgr.GetIndemniteDeplacementByCi(personnelId, ciId));

                foreach (IndemniteDeplacementModel indemniteDep in indemniteDeplacements)
                {
                    if (!indemniteDep.DateSuppression.HasValue || (indemniteDep.DateSuppression.HasValue && indemniteDep.DateSuppression.Value == DateTime.MinValue))
                    {
                        indemniteDep.Actif = true;
                    }
                }
                return indemniteDeplacements;
            });
        }

        /// <summary>
        /// Méthode GET de recherche des Indémnité de Déplacement d'un Personnel avec filtre
        /// </summary>
        /// <param name="filters">Filter</param>
        /// <param name="personnelId">Identifiant personnel</param>
        /// <param name="recherche">Texte à rechercher</param>
        /// <returns> Retourne la liste Indémnité de Déplacement filtré </returns>
        [HttpPost]
        [Route("api/IndemniteDeplacement/SearchByPersonnel/{personnelId}/{recherche?}")]
        public HttpResponseMessage SearchIndemniteDeplacementWithFilter(SearchIndemniteDeplacementModel filters, int personnelId, string recherche = "")
        {
            return Post(() => this.mapper.Map<IEnumerable<IndemniteDeplacementModel>>(this.indemniteDeplacementMgr.SearchIndemniteDeplacementWithFilters(recherche, this.mapper.Map<SearchIndemniteDeplacementEnt>(filters), personnelId)));
        }

        /// <summary>
        /// Méthode post de calcul des distance KM
        /// </summary>
        /// <param name="indemniteDeplacement">Indemnité de déplacement à calculer</param>
        /// <returns>l'indémnité de déplacement calculé avec les codes appropriés</returns>
        [HttpPost]
        [Route("api/IndemniteDeplacement/CalculKm/")]
        public HttpResponseMessage CalculKm(IndemniteDeplacementModel indemniteDeplacement)
        {
            var indemniteDeplacementEnt = this.indemniteDeplacementMgr.GetCalculIndemniteDeplacement(this.mapper.Map<IndemniteDeplacementEnt>(indemniteDeplacement));
            var ret = mapper.Map<IndemniteDeplacementModel>(indemniteDeplacementEnt);

            if ( indemniteDeplacement.Personnel.TypeRattachement == TypeRattachement.Domicile && ret != null && !indemniteDeplacementEnt.NombreKilometreVODomicileChantier.HasValue)
            {
                // NPI : FeatureIndemniteDeplacement.resx semble cassé, le Designer.cs correspondant ... ne correspond pas
                // C'est pour cela que la chaine est en dur ici
                // Voir avec Jérome quoi faire
                ret.Warning = "Tous les calculs n'ont pas été effectués car les coordonnées GPS du chantier ou du domicile du personnel ne sont pas renseignées";
            }
            else if (indemniteDeplacement.Personnel.TypeRattachement != TypeRattachement.Domicile && ret != null && (!indemniteDeplacementEnt.NombreKilometreVODomicileChantier.HasValue
             || !indemniteDeplacementEnt.NombreKilometreVODomicileRattachement.HasValue
             || !indemniteDeplacementEnt.NombreKilometreVOChantierRattachement.HasValue))
            {
                // NPI : FeatureIndemniteDeplacement.resx semble cassé, le Designer.cs correspondant ... ne correspond pas
                // C'est pour cela que la chaine est en dur ici
                // Voir avec Jérome quoi faire
                ret.Warning = "Tous les calculs n'ont pas été effectués car les coordonnées GPS de l'agence, du chantier ou du domicile du personnel ne sont pas renseignées";
            }

            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }
    }
}
