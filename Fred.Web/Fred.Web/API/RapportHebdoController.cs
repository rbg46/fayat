using AutoMapper;
using Fred.Business.Equipe;
using Fred.Business.Rapport.Pointage;
using Fred.Business.Rapport.RapportHebdo;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Web.Shared.Models.Rapport;
using Fred.Web.Shared.Models.Rapport.RapportHebdo;
using Fred.Web.Shared.Models.Referential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fred.Web.API
{
    /// <summary>
    /// Rapport hebdo controller
    /// </summary>
    public class RapportHebdoController : ApiControllerBase
    {
        #region Fields and propertites
        /// <summary>
        /// Equipe manager
        /// </summary>
        private readonly IEquipeManager equipeManager;

        /// <summary>
        /// Gestionnaire des rapports hebdomadaires
        /// </summary>
        private readonly IRapportHebdoManager rapportHebdoManager;

        /// <summary>
        /// Mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Pointage Manager
        /// </summary>
        private readonly IPointageManager pointageManager;

        #endregion

        /// <summary>
        /// Rapport hebdo controller constructor
        /// </summary>
        /// <param name="equipeManager">Equipe manager</param>
        /// <param name="rapportHebdoManager">Gestionnaire des rapports hebdomadaires</param>
        /// <param name="mapper">Mapper</param>
        /// <param name="pointageManager">Pointage Manager</param>
        public RapportHebdoController(IEquipeManager equipeManager, IRapportHebdoManager rapportHebdoManager, IMapper mapper, IPointageManager pointageManager)
        {
            this.equipeManager = equipeManager;
            this.rapportHebdoManager = rapportHebdoManager;
            this.mapper = mapper;
            this.pointageManager = pointageManager;
        }

        #region public method

        /// <summary>
        /// Obtient le summary du pointage du rapport hebdomadaire pour un utilisateur donné
        /// </summary>
        /// <param name="personnelStatut">Personnel statut : Ouvrier, ETAM, IAC</param>
        /// <param name="mondayDate">La date du lundi</param>
        /// <returns>Http response message</returns>
        [HttpGet]
        [Route("api/RapportHebdo/GetUserPointageHebdoSummary/{personnelStatut}/{mondayDate}")]
        public HttpResponseMessage GetUserPointageHebdoSummary(string personnelStatut, DateTime mondayDate)
        {
            return this.Get(() => equipeManager.GetUserPointageHebdoSummary(personnelStatut, mondayDate));
        }

        /// <summary>
        /// Obtient le model du rapport hebdomadaire
        /// </summary>
        /// <param name="model">Rapport hebdo entree view model</param>
        /// <returns>Http response message</returns>
        [HttpPost]
        [Route("api/RapportHebdo/GetRapportHebdoByCi")]
        public HttpResponseMessage GetRapportHebdoByCi(RapportHebdoEntreeViewModel model)
        {
            if (model == null)
            {
                return new HttpResponseMessage();
            }

            DateTime mondayDate = model.Mondaydate;
            Dictionary<int, List<int>> ciPersonnelListPairs = model.RapportHebdoEntreeList?.ToDictionary(v => v.CiId, v => v.OuvrierListId?.ToList());

            return this.Get(() => rapportHebdoManager.GetRapportHebdoByCi(ciPersonnelListPairs, mondayDate));
        }

        /// <summary>
        /// Obtient le model du rapport hebdomadaire
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="mondayDate">La date du lundi choisi</param>
        /// <param name="allCi">Booléan indique s'il faut récupérer tous les CI pour le pointage</param>
        /// <returns>Http response message</returns>
        [HttpGet]
        [Route("api/RapportHebdo/GetRapportHebdoByWorker/{personnelId}/{mondayDate}/{allCi}")]
        public HttpResponseMessage GetRapportHebdoByWorker(int personnelId, DateTime mondayDate, bool allCi)
        {
            return this.Get(() => new List<RapportHebdoNode<PointageCell>> { rapportHebdoManager.GetRapportHebdoByEmployee(personnelId, mondayDate, allCi) });
        }

        /// <summary>
        /// Enregistrer le rapport hebdomadaire
        /// </summary>
        /// <param name="rapportHebdoSaveViewModel">Model pour enregistrer le rapport hebdomadaire</param>
        /// <returns>Http response message</returns>
        [HttpPost]
        [Route("api/RapportHebdo/SaveRapportHebdo")]
        public HttpResponseMessage SaveRapportHebdo(RapportHebdoSaveViewModel rapportHebdoSaveViewModel)
        {
            return this.Post(() => rapportHebdoManager.SaveRapportHebdo(rapportHebdoSaveViewModel));
        }

        /// <summary>
        /// Vérifier et valider le rapport hebdomadaire
        /// </summary>
        /// <param name="rapportHebdoSaveViewModel">Model de Rapport hebdomadaire</param>
        /// <param name="isEtamIac">Boolean indique si la validation concerne ETAM \ IAC ou Ouvrier</param>
        /// <returns>Http response message</returns>
        [HttpPost]
        [Route("api/RapportHebdo/CheckAndValidateRapportHebdo/{isEtamIac}")]
        public HttpResponseMessage CheckAndValidateRapportHebdo(RapportHebdoSaveViewModel rapportHebdoSaveViewModel, bool isEtamIac)
        {
            return this.Post(() => rapportHebdoManager.CheckAndValidateRapportHebdo(rapportHebdoSaveViewModel, isEtamIac));
        }

        /// <summary>
        /// Get synthese mensuelle rapport hebdo Etam Iac
        /// </summary>
        /// <param name="utilisateurId">Identifiant utilisateur</param>
        /// <param name="monthDate">le premier jour du mois</param>
        /// <returns>Http response message</returns>
        [HttpGet]
        [Route("api/RapportHebdo/GetSyntheseMensuelleEtamIac/{utilisateurId}/{monthDate}")]
        public HttpResponseMessage GetSyntheseMensuelleEtamIac(int utilisateurId, DateTime monthDate)
        {
            return this.Get(() => this.mapper.Map<IEnumerable<RapportHebdoSyntheseMensuelleModel>>(rapportHebdoManager.GetSyntheseMensuelleRapportHebdo(utilisateurId, monthDate)));
        }

        /// <summary>
        /// Validation du pointage synthese mensuelle Etam Iac
        /// </summary>
        /// <param name="validateSyntheseMensuelleModel">Synthese mensuelle validation Model</param>
        /// <returns>Http response message</returns>
        [HttpPost]
        [Route("api/RapportHebdo/ValiderSyntheseMensuelleEtamIac")]
        public HttpResponseMessage ValiderSyntheseMensuelleEtamIac(ValidateSyntheseMensuelleModel validateSyntheseMensuelleModel)
        {
            return this.Post(() => rapportHebdoManager.ValiderSyntheseMensuelleEtamIac(validateSyntheseMensuelleModel));
        }

        /// <summary>
        /// Récupere les heures normales des personnels passé en paramétre selon une période
        /// </summary>
        /// <param name="rapportHebdoPersonnelWithAllCiModel">model contenant une liste de personnel avec la période</param>
        /// <returns>Http response message</returns>
        [HttpGet]
        [Route("api/RapportHebdo/GetPointageByPersonnelIDAndInterval")]
        public HttpResponseMessage GetPointageByPersonnelIDAndInterval([FromUri] RapportHebdoPersonnelWithAllCiModel rapportHebdoPersonnelWithAllCiModel)
        {
            return this.Get(() => this.mapper.Map<IEnumerable<RapportHebdoPersonnelWithTotalHourModel>>(pointageManager.GetPointageByPersonnelIDAndInterval(this.mapper.Map<RapportHebdoPersonnelWithAllCiEnt>(rapportHebdoPersonnelWithAllCiModel))));
        }

        /// <summary>
        /// GetRapportLigneStatutForNewPointage
        /// </summary>
        /// <param name="personnelId">Personnel identifier</param>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="mondayDate">Date du lundi</param>
        /// <returns>Http response message</returns>
        [HttpGet]
        [Route("api/RapportHebdo/GetRapportLigneStatutForNewPointage/{personnelId}/{ciId}/{mondayDate}")]
        public HttpResponseMessage GetRapportLigneStatutForNewPointage(int personnelId, int ciId, DateTime mondayDate)
        {
            return this.Get(() => this.mapper.Map<IEnumerable<RapportHebdoNewPointageStatutModel>>(rapportHebdoManager.GetRapportLigneStatutForNewPointage(personnelId, ciId, mondayDate)));
        }

        /// <summary>
        /// GetRapportLigneStatutForNewPointage
        /// </summary>
        /// <param name="personnelId">Personnel identifier</param>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="mondayDate">Date du lundi</param>
        /// <returns>Http response message</returns>
        [HttpGet]
        [Route("api/RapportHebdo/GetSortie/{personnelId}/{ciId}/{mondayDate}")]
        public HttpResponseMessage GetSortie(int personnelId, int ciId, DateTime mondayDate)
        {
            return this.Get(() => rapportHebdoManager.GetSortie(personnelId, ciId, mondayDate));
        }

        /// <summary>
        /// Get list des primes affected to list of personnel
        /// </summary>
        /// <param name="primePersonnelModel">prime Personnel get model</param>
        /// <returns>List des primes affected</returns>
        [HttpGet]
        [Route("api/RapportHebdo/PrimePersonnelAffected")]
        public HttpResponseMessage PrimePersonnelAffected([FromUri] PrimesPersonnelsGetModel primePersonnelModel)
        {
            return this.Get(() => this.mapper.Map<IEnumerable<PrimePersonnelAffectationModel>>(pointageManager.PrimePersonnelAffected(this.mapper.Map<PrimesPersonnelsGetEnt>(primePersonnelModel))));
        }

        /// <summary>
        /// Get Validation Affaires 
        /// </summary>
        /// <param name="dateDebut">Date du lundi d'une semaine</param>
        /// <returns>Http action result</returns>
        [HttpGet]
        [Route("api/RapportHebdo/GetValidationAffairesByResponsableAsync/{dateDebut}")]
        public async Task<IHttpActionResult> GetValidationAffairesByResponsableAsync(DateTime dateDebut)
        {
            IEnumerable<SyntheseValidationAffairesModel> syntheseValidationAffairesModels = await rapportHebdoManager.GetValidationAffairesByResponsableAsync(dateDebut);
            return Ok(syntheseValidationAffairesModels);
        }


        /// <summary>
        ///  Validation Pointage (Staut V2) par CiList and Affected ouvrier id List
        /// </summary>
        /// <param name="validationAffaireModel">Validation affaire mopdel</param>
        /// <returns>List du synthese validation affaires model</returns>
        [HttpGet]
        [Route("api/RapportHebdo/ValidateAffairesByResponsableAsync")]
        public async Task<IHttpActionResult> ValidateAffairesByResponsableAsync([FromUri] ValidationAffaireModel validationAffaireModel)
        {
            await rapportHebdoManager.ValidateAffairesByResponsableAsync(validationAffaireModel);
            return Ok();
        }
        #endregion

        #region FIGGO

        /// <summary>
        /// Get Logs des absence FIGGO importées
        /// </summary>
        /// <param name="dateDebut">Date de debut</param>
        /// <param name="dateFin">Date de fin</param>
        /// <returns>Logs des absences FIGGO importées</returns>
        [HttpGet]
        [Route("api/RapportHebdo/GetLogFiggo/{dateDebut}/{dateFin}")]
        public HttpResponseMessage GetLogFiggo(DateTime dateDebut, DateTime dateFin)
        {
            return Get(() => pointageManager.GetLogFiggo(dateDebut, dateFin));
        }

        #endregion
    }
}
