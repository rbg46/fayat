using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Fred.Business.AffectationMoyen;
using Fred.Business.ExternalService.Materiel;
using Fred.Business.Moyen;
using Fred.Business.Rapport.Pointage;
using Fred.Business.Utilisateur;
using Fred.Entities;
using Fred.Entities.Moyen;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using Fred.Framework.Reporting;
using Fred.Web.Models.Referential;
using Fred.Web.Models.ReferentielFixe;
using Fred.Web.Models.Societe;
using Fred.Web.Shared.Models.Moyen;

namespace Fred.Web.API
{
    /// <summary>
    /// Controller Web Api des affectations des moyens
    /// </summary>
    public class MoyenController : ApiControllerBase
    {
        private readonly string templateFolderPath = HttpContext.Current.Server.MapPath("/Templates/");

        private readonly IMapper mapper;
        private readonly IAffectationMoyenManager affectationMoyenManager;
        private readonly IPointageManager pointageManager;
        private readonly IMoyenManager moyenManager;
        private readonly IMoyenManagerExterne moyenManagerExterne;
        private readonly IMaterielLocationManager materielLocationManager;
        private readonly IUtilisateurManager utilisateurManager;

        public MoyenController(IMapper mapper,
                               IAffectationMoyenManager affectationMoyenManager,
                               IPointageManager pointageManager,
                               IMoyenManager moyenManager,
                               IMoyenManagerExterne moyenManagerExterne,
                               IMaterielLocationManager materielLocationManager,
                               IUtilisateurManager utilisateurManager)
        {
            this.mapper = mapper;
            this.affectationMoyenManager = affectationMoyenManager;
            this.pointageManager = pointageManager;
            this.moyenManager = moyenManager;
            this.moyenManagerExterne = moyenManagerExterne;
            this.materielLocationManager = materielLocationManager;
            this.utilisateurManager = utilisateurManager;
        }

        /// <summary>
        ///   POST Récupération des résultats de la recherche en fonction du filtre
        /// </summary>
        /// <param name="filters">Filtre de l'affectation de moyen</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Une réponse HTTP</returns>
        [HttpPost]
        [Route("api/AffectationMoyen/SearchWithFilters/{page?}/{pageSize?}")]
        public IHttpActionResult SearchWithFilters(SearchAffectationMoyenModel filters, int page = 1, int pageSize = 20)
        {
            return Ok(mapper.Map<IEnumerable<AffectationMoyenModel>>(affectationMoyenManager.SearchWithFilters(mapper.Map<SearchAffectationMoyenEnt>(filters), page, pageSize)));
        }

        /// <summary>
        /// Récupère la liste des lignes de rapport de moyen de la recherche en fonction du filtre
        /// </summary>
        /// <param name="filters">Filtre du rapport de moyen</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>IEnumerable contenant les lignes de rapport de moyen correspondant aux critères de recherche</returns>
        [HttpPost]
        [Route("api/RapportMoyen/SearchRapportLigneMoyenWithFilters/{page?}/{pageSize?}")]
        public IHttpActionResult SearchRapportLigneMoyenWithFilters(SearchRapportLigneMoyenModel filters, int page = 1, int pageSize = 20)
        {
            return Ok(mapper.Map<IEnumerable<RapportMoyenLigneModel>>(pointageManager.SearchPointageReelWithFilterByPage(mapper.Map<SearchRapportLigneMoyenEnt>(filters), page, pageSize)));
        }

        /// <summary>
        /// Générer fichier excel de rapport moyen de la recherche en fonction du filtre
        /// </summary>
        /// <param name="filters">Filtre du rapport de moyen</param>
        /// <returns>IEnumerable contenant les lignes de rapport de moyen correspondant aux critères de recherche</returns>
        [HttpPost]
        [Route("api/RapportMoyen/GenerateRapportLigneMoyenWithFilters")]
        public async Task<object> GenerateRapportLigneMoyenWithFiltersAsync(SearchRapportLigneMoyenModel filters)
        {
            try
            {
                string periode = Constantes.IndeterminatePeriod;
                if (!filters.DateFrom.HasValue && filters.DateTo.HasValue)
                {
                    periode = Constantes.BeforeDate + filters?.DateTo.Value.ToShortDateString();
                }
                else if (!filters.DateTo.HasValue && filters.DateFrom.HasValue)
                {
                    periode = Constantes.AfterDate + filters?.DateFrom.Value.ToShortDateString();
                }
                else if (filters.DateTo.HasValue && filters.DateFrom.HasValue)
                {
                    periode = filters?.DateFrom.Value.ToShortDateString() + " - " + filters?.DateTo.Value.ToShortDateString();
                }

                UtilisateurEnt user = await utilisateurManager.GetContextUtilisateurAsync();
                string userName = user.PrenomNom;
                List<RapportLigneEnt> listrapportLigneModel = pointageManager.SearchPointageReelWithMoyenFilter(mapper.Map<SearchRapportLigneMoyenEnt>(filters));

                MemoryStream stream = moyenManager.GenerateExcelMoyen(listrapportLigneModel, userName, periode, templateFolderPath);
                return stream.TransformMemoryStreamToPdfOrExcel(false);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Récupération des moyens en fonction des paramètres fournies 
        /// <para> 
        ///  NB (AJOUT DU PRAGMA !) Le nombre de paramétres de cette méthodes ne peuvent pas étre réduit vu que c'est utilisé par la lookup . 
        ///  La pragma peut étre enlévé dés que la restriction sur les paramétres ne conçernent plus les actions des controller Web API .
        /// </para>
        /// </summary>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="recherche">Text de la recherche</param>
        /// <param name="typeMoyen">Type de moyen</param>
        /// <param name="sousTypeMoyen">Sous type de moyen</param>
        /// <param name="modelMoyen">Model du moyen</param>
        /// <param name="isLocationView">True si la requéte vient de l'écran location . False dans le cas contraire</param>
        /// <param name="etablissementId">Id de l'etablissement comptable</param>
        /// <returns>Une réponse HTTP contenant la liste des moyens</returns>
        [HttpGet]
        [Route("api/Moyen/SearchLight/{page?}/{pageSize?}/{recherche?}/{typeMoyen?}/{sousTypeMoyen?}/{modelMoyen?}/{isLocationView?}/{etablissementId?}")]
#pragma warning disable S107 // Methods should not have too many parameters -
        public IHttpActionResult SearchLightForMoyen(int page = 1,
            int pageSize = 20,
            string recherche = "",
            string typeMoyen = null,
            string sousTypeMoyen = null,
            string modelMoyen = null,
            bool? isLocationView = null,
            int? etablissementId = null)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            SearchMoyenModel filters = new SearchMoyenModel
            {
                TextRecherche = recherche,
                TypeMoyen = typeMoyen,
                SousTypeMoyen = sousTypeMoyen,
                ModelMoyen = modelMoyen,
                IsLocationView = isLocationView,
                EtablissementId = etablissementId
            };

            return Ok(mapper.Map<IEnumerable<MoyenModel>>(moyenManager.SearchLightForMoyen(mapper.Map<SearchMoyenEnt>(filters), page, pageSize)));
        }

        /// <summary>
        ///  Récupération des sociétés de moyen en fonction des paramètres fournis 
        /// </summary>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="recherche">Text de la recherche</param>
        /// <param name="typeMoyen">Type de moyen</param>
        /// <param name="sousTypeMoyen">Sous type de moyen</param>
        /// <param name="modelMoyen">Model du moyen</param>
        /// <param name="societe">Société du moyen</param>
        /// <returns>Une réponse HTTP contenant la liste des sociétés</returns>
        [HttpGet]
        [Route("api/Moyen/Societe/SearchLight/{page?}/{pageSize?}/{recherche?}/{typeMoyen?}/{sousTypeMoyen?}/{modelMoyen?}/{societe?}")]
        public IHttpActionResult SearchLightForSociete(int page = 1, int pageSize = 30, string recherche = "", string typeMoyen = null, string sousTypeMoyen = null, string modelMoyen = null, string societe = null)
        {
            SearchSocieteMoyenModel filters = new SearchSocieteMoyenModel
            {
                Recherche = recherche,
                TypeMoyen = typeMoyen,
                SousTypeMoyen = sousTypeMoyen,
                ModelMoyen = modelMoyen,
                Societe = societe
            };
            return Ok(mapper.Map<IEnumerable<SocieteModel>>(moyenManager.SearchLightForSociete(mapper.Map<SearchSocieteMoyenEnt>(filters), page, pageSize)));
        }

        /// <summary>
        /// Récupération des établissements comptables de moyen en fonction des paramètres fournis
        /// <para>
        /// NB (AJOUT DU PRAGMA !) Le nombre de paramétres de cette méthodes ne peuvent pas étre réduit vu que c'est utilisé par la lookup . 
        /// La pragma peut étre enlévé dés que la restriction sur les paramétres ne conçernent plus les actions des controller Web API .
        /// </para>
        /// </summary>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="recherche">Text de la recherche</param>
        /// <param name="typeMoyen">Type de moyen</param>
        /// <param name="sousTypeMoyen">Sous type de moyen</param>
        /// <param name="modelMoyen">Model du moyen</param>
        /// <param name="societe">Société du moyen</param>
        /// <returns>Une réponse HTTP contenant la liste des établissements</returns>
        [HttpGet]
        [Route("api/Moyen/Etablissement/SearchLight/{page?}/{pageSize?}/{recherche?}/{typeMoyen?}/{sousTypeMoyen?}/{modelMoyen?}/{societe?}")]
#pragma warning disable S107 // Methods should not have too many parameters - 
        public IHttpActionResult SearchLightForEtablissement(
            int page = 1,
            int pageSize = 30,
            string recherche = "",
            string typeMoyen = null,
            string sousTypeMoyen = null,
            string modelMoyen = null,
            string societe = null)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            SearchEtablissementMoyenModel filters = new SearchEtablissementMoyenModel
            {
                Recherche = recherche,
                TypeMoyen = typeMoyen,
                SousTypeMoyen = sousTypeMoyen,
                ModelMoyen = modelMoyen,
                Societe = societe
            };
            return Ok(mapper.Map<IEnumerable<EtablissementComptableModel>>(moyenManager.SearchLightForEtablissementComptable(mapper.Map<SearchEtablissementMoyenEnt>(filters), page, pageSize)));
        }

        /// <summary>
        ///  Récupération des immatriculations des moyens
        /// <para>
        /// NB (AJOUT DU PRAGMA !) Le nombre de paramétres de cette méthodes ne peuvent pas étre réduit vu que c'est utilisé par la lookup . 
        /// La pragma peut étre enlévé dés que la restriction sur les paramétres ne conçernent plus les actions des controller Web API .
        /// </para>
        /// </summary>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="recherche">Text de la recherche</param>
        /// <param name="typeMoyen">Type de moyen</param>
        /// <param name="sousTypeMoyen">Sous type de moyen</param>
        /// <param name="modelMoyen">Model du moyen</param>
        /// <param name="societe">Société du moyen</param>
        /// <param name="etablissementId">Etablissement Id du moyen</param>
        /// <param name="numParc">Numéro de parc (code de moyen)</param>
        /// <returns>Une réponse HTTP contenant la liste des immatriculations</returns>
        [HttpGet]
        [Route("api/Moyen/Immatriculation/SearchLight/{page?}/{pageSize?}/{recherche?}/{typeMoyen?}/{sousTypeMoyen?}/{modelMoyen?}/{societe?}/{etablissement?}/{numParc?}")]
#pragma warning disable S107 // Methods should not have too many parameters
        public IHttpActionResult SearchLightForImmatriculation(int page = 1,
            int pageSize = 30,
            string recherche = "",
            string typeMoyen = null,
            string sousTypeMoyen = null,
            string modelMoyen = null,
            string societe = null,
            int? etablissementId = null,
            string numParc = null)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            SearchImmatriculationMoyenModel filters = new SearchImmatriculationMoyenModel
            {
                TextRecherche = recherche,
                TypeMoyen = typeMoyen,
                SousTypeMoyen = sousTypeMoyen,
                ModelMoyen = modelMoyen,
                Societe = societe,
                EtablissementId = etablissementId,
                NumParc = numParc
            };

            return Ok(moyenManager.SearchLightForImmatriculation(mapper.Map<SearchImmatriculationMoyenEnt>(filters), page));
        }

        /// <summary>
        ///  Récupération des types des affectations moyens
        /// </summary>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="recherche">Text de la recherche</param>
        /// <returns>Une réponse HTTP contenant la liste des types des affectations</returns>
        [HttpGet]
        [Route("api/AffectationMoyen/Type/SearchLight/{page?}/{pageSize?}/{recherche?}")]
        public IHttpActionResult SearchLightForAffectationMoyenType(int page = 1, int pageSize = 20, string recherche = "")
        {
            return Ok(mapper.Map<IEnumerable<AffectationMoyenTypeModel>>(moyenManager.SearchLightForAffectationMoyenType(page, pageSize, recherche)));
        }

        /// <summary>
        ///  Récupération des types des moyens
        /// </summary>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="recherche">Text de la recherche</param>
        /// <returns>Une réponse HTTP contenant la liste des types des moyens</returns>
        [HttpGet]
        [Route("api/Moyen/Type/SearchLight/{page?}/{pageSize?}/{recherche?}")]
        public IHttpActionResult SearchLightForTypeMoyen(int page = 1, int pageSize = 20, string recherche = "")
        {
            return Ok(mapper.Map<IEnumerable<ChapitreModel>>(moyenManager.SearchLightForTypeMoyen(page, pageSize, recherche)));
        }

        /// <summary>
        ///  Récupération des sous types d'un moyen
        /// </summary>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="recherche">Text de la recherche</param>
        /// <param name="typeMoyen">Type de moyen</param>
        /// <returns>Une réponse HTTP contenant la liste des sous types des moyens</returns>
        [HttpGet]
        [Route("api/Moyen/SousType/SearchLight/{page?}/{pageSize?}/{recherche?}/{typeMoyen?}")]
        public IHttpActionResult SearchLightForSousTypeMoyen(int page = 1, int pageSize = 20, string recherche = "", string typeMoyen = null)
        {
            return Ok(mapper.Map<IEnumerable<SousChapitreModel>>(moyenManager.SearchLightForSousTypeMoyen(page, pageSize, recherche, typeMoyen)));
        }

        /// <summary>
        ///  Récupération des modèles d'un moyen
        /// </summary>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="recherche">Text de la recherche</param>
        /// <param name="typeMoyen">Type de moyen</param>
        /// <param name="sousTypeMoyen">Sous type de moyen</param>
        /// <returns>Une réponse HTTP contenant la liste des modèles des moyens</returns>
        [HttpGet]
        [Route("api/Moyen/Model/SearchLight/{page?}/{pageSize?}/{recherche?}/{typeMoyen?}/{sousTypeMoyen?}")]
        public IHttpActionResult SearchLightForModelMoyen(int page = 1, int pageSize = 20, string recherche = "", string typeMoyen = null, string sousTypeMoyen = null)
        {
            return Ok(mapper.Map<IEnumerable<RessourceModel>>(moyenManager.SearchLightForModelMoyen(page, pageSize, recherche, typeMoyen, sousTypeMoyen)));
        }

        [HttpGet]
        [Route("api/Moyen/FicheGenerique/SearchLight/{page?}/{pageSize?}/{recherche?}")]
        public IHttpActionResult SearchLightForFicheGenerique(int page = 1, int pageSize = 20, string recherche = "")
        {
            return Ok(moyenManager.SearchLightForFicheGenerique(page, pageSize, recherche));
        }

        [HttpGet]
        [Route("api/AffectationMoyen/GetAffectationMoyenFamilleByTypeModel")]
        public IHttpActionResult GetAffectationMoyenFamilleByType()
        {
            return Ok(affectationMoyenManager.GetAffectationMoyenFamilleByType());
        }

        [HttpPost]
        [Route("api/AffectationMoyen/ValidateAffectationMoyen")]
        public IHttpActionResult ValidateAffectationMoyen(ValidateAffectationMoyenModel model)
        {
            affectationMoyenManager.ValidateAffectationMoyen(model);

            return Ok();
        }

        [HttpPost]
        [Route("api/AffectationMoyen/CreateMoyenLocation")]
        public IHttpActionResult CreateMoyenLocation(MoyenModel model)
        {
            moyenManager.CreateMoyenEnLocation(mapper.Map<MaterielEnt>(model));

            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Created));
        }

        /// <summary>
        /// Update MaterielLocation 
        /// </summary>
        /// <param name="materielLocationToUpdate">Model de materiel location a mettre a jour</param>
        /// <returns>idOfMaterielUpdated</returns>
        [HttpPut]
        [Route("api/AffectationMoyen/UpdateMoyenLocation")]
        public IHttpActionResult UpdateMoyenLocation(MaterielLocationModel materielLocationToUpdate)
        {
            return Ok(moyenManager.UpdateMaterielLocation(mapper.Map<MaterielLocationEnt>(materielLocationToUpdate)));
        }

        /// <summary>
        /// Update du pointage des moyens
        /// </summary>
        /// <param name="dateDebut">Date de début de génération</param>
        /// <param name="dateFin">Date de fin de génération</param>
        /// <returns>Update pointage moyen result</returns>
        [HttpGet]
        [Route("api/Moyen/UpdateMoyenPointage/{dateDebut}/{dateFin}")]
        public IHttpActionResult UpdateMoyenPointage(DateTime dateDebut, DateTime dateFin)
        {
            return Ok(moyenManager.UpdatePointageMoyen(dateDebut, dateFin));
        }

        /// <summary>
        /// Envoi du pointage des moyens
        /// </summary>
        /// <param name="dateDebut">Date de début</param>
        /// <param name="dateFin">Date de fin</param>
        /// <returns>Resultat de l'update</returns>
        [HttpGet]
        [Route("api/Moyen/ExportPointageMoyen/{dateDebut}/{dateFin}")]
        public async Task<IHttpActionResult> ExportPointageMoyen(DateTime dateDebut, DateTime dateFin)
        {
            return Ok(await moyenManagerExterne.ExportPointageMoyenAsync(dateDebut, dateFin));
        }

        /// <summary>
        /// Chercher tous les location qui ont une date de suppression null 
        /// </summary>
        /// <returns>List des Locations</returns>
        [HttpGet]
        [Route("api/MoyenLocation/GetAllActiveLocation")]
        public IHttpActionResult GetAllActiveLocation()
        {
            return Ok(materielLocationManager.GetAllActiveLocation());
        }

        /// <summary>
        /// Methode permet de recuperer la liste des affectation lie a un materiel de location 
        /// </summary>
        /// <param name="materiellocationid">l id de materiel en location</param>
        /// <returns>return la liste des affectation </returns>
        [HttpGet]
        [Route("api/AffectationMoyen/GetAffectationOfMaterielLocation/{materiellocationid}")]
        public IHttpActionResult GetAffectationOfMaterileLocation(int materiellocationid)
        {
            return Ok(affectationMoyenManager.GetAllAffectationByMaterielLocationId(materiellocationid));
        }

        /// <summary>
        /// Supprimer une location 
        /// </summary>
        /// <param name="materielLocationid">le  materiel a supprimer</param>
        /// <returns><see cref="HttpResponseMessage"/></returns>
        [HttpDelete]
        [Route("api/AffectationMoyen/DeleteMoyenLocation/{materielLocationid}")]
        public IHttpActionResult DeleteMaterielLocation(int materielLocationid)
        {
            moyenManager.DeleteMaterielLocation(materielLocationid);

            return Ok();
        }
    }
}
