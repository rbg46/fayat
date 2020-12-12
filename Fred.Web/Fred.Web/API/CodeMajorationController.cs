using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Fred.Business.CI;
using Fred.Business.Referential;
using Fred.Business.Utilisateur;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using Fred.Web.Models.CI;
using Fred.Web.Models.Referential;

namespace Fred.Web.API
{
    /// <summary>
    ///   Controller WEB API des codesMajoration
    /// </summary>
    public class CodeMajorationController : ApiControllerBase
    {
        /// <summary>
        ///   Manager business des codesMajoration
        /// </summary>
        protected readonly ICodeMajorationManager CodeMajorationMgr;

        /// <summary>
        /// Manager business des primes
        /// </summary>
        protected readonly ICIManager CiMgr;

        /// <summary>
        ///   Mapper Model / ModelVue
        /// </summary>
        protected readonly IMapper Mapper;

        /// <summary>
        ///   Manager des utilisateurs
        /// </summary>
        protected readonly IUtilisateurManager UtilisateurManager;

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="CodeMajorationController" />.
        /// </summary>
        /// <param name="codeMajorationMgr">Manager de codesMajoration</param>
        /// <param name="utilisateurManager">Manager des utilisateurs</param>
        /// <param name="ciMgr">Manager de CI</param>
        /// <param name="mapper">Mapper Model / ModelVue</param>
        public CodeMajorationController(ICodeMajorationManager codeMajorationMgr, IUtilisateurManager utilisateurManager, ICIManager ciMgr, IMapper mapper)
        {
            this.CodeMajorationMgr = codeMajorationMgr;
            this.UtilisateurManager = utilisateurManager;
            this.CiMgr = ciMgr;
            this.Mapper = mapper;
        }

        /// <summary>
        ///   Méthode GET de récupération d'une nouvelle instance code majoration intialisée.
        /// </summary>
        /// <returns>Retourne une nouvelle instance code majoration intialisée</returns>
        [HttpGet]
        [Route("api/CodeMajoration/GetNew")]
        public HttpResponseMessage GetNewCodeMajoration()
        {
            return Get(() => this.Mapper.Map<CodeMajorationModel>(this.CodeMajorationMgr.GetNewCodeMajoration()));
        }

        /// <summary>
        ///   Méthode GET de récupération des codesMajoration
        /// </summary>
        /// <returns>Retourne la liste des codesMajoration</returns>
        /// <param name="recherche">Mot a rechercher</param>
        [HttpGet]
        [Route("api/CodeMajoration/ListCodesMajoration/{recherche?}")]
        public HttpResponseMessage ListCodesMajoration(string recherche = null)
        {
            return Get(() =>
            {
                UtilisateurEnt utilisateur = this.UtilisateurManager.GetContextUtilisateur();
                var codesMajoration = this.CodeMajorationMgr.GetCodeMajorationList(utilisateur, recherche);
                return this.Mapper.Map<IEnumerable<CodeMajorationModel>>(codesMajoration);
            });
        }

        /// <summary>
        ///   Méthode GET de récupération des codesMajoration
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="ciId">ciId</param>
        /// <returns>Retourne la liste des codesMajoration</returns>
        [HttpGet]
        [Route("api/CodeMajoration/ListActifsCodesMajorationBySocieteId/{id}/{ciId}")]
        public IEnumerable<CodeMajorationModel> ListActifsCodesMajorationBySocieteId(int id, int ciId)
        {
            var codesMajoration = this.CodeMajorationMgr.GetActifsCodeMajorationListBySocieteId(id, ciId);
            return this.Mapper.Map<IEnumerable<CodeMajorationModel>>(codesMajoration);
        }

        /// <summary>
        ///   Méthode GET de récupération des codesMajoration
        /// </summary>
        /// <param name="codeMaj">codeMaj</param>
        /// <returns>Retourne la liste des codesMajoration</returns>
        [HttpPost]
        [Route("api/CodeMajoration")]
        public HttpResponseMessage Post(CodeMajorationModel codeMaj)
        {
            return Post(() =>
            {
                UtilisateurEnt createur = this.UtilisateurManager.GetContextUtilisateur();
                return this.CodeMajorationMgr.AddCodeMajoration(this.Mapper.Map<CodeMajorationEnt>(codeMaj), createur);
            });
        }

        /// <summary>
        ///   Méthode GET de récupération des codesMajoration
        /// </summary>
        /// <param name="codeMaj">codeMaj</param>
        /// <returns>Retourne la liste des codesMajoration</returns>
        [HttpPut]
        [Route("api/CodeMajoration")]
        public HttpResponseMessage Put(CodeMajorationModel codeMaj)
        {
            return Put(() =>
            {
                this.CodeMajorationMgr.UpdateCodeMajoration(this.Mapper.Map<CodeMajorationEnt>(codeMaj));
                return codeMaj.CodeMajorationId;
            });
        }

        /// <summary>
        ///   GET api/controller/5 Permet de déterminer si un code majoration ayant un code similaire pour la même groupe existe ou
        ///   non
        /// </summary>
        /// <param name="codecodeMajoration">Code du code majoration courant</param>
        /// <param name="idCourant">Identifiant du groupe lié</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpGet]
        [Route("api/CodeMajoration/CheckExistsCode/{codecodeMajoration}/{idCourant}/")]
        public HttpResponseMessage IsCodeMajorationExistsByCode(string codecodeMajoration, int idCourant)
        {
            return Get(() =>
            {
                UtilisateurEnt createur = this.UtilisateurManager.GetContextUtilisateur();
                int groupeId = createur.Personnel.Societe.GroupeId;
                bool exists = this.CodeMajorationMgr.IsCodeMajorationExistsByCodeInGroupe(idCourant, codecodeMajoration, groupeId);
                return exists;
            });
        }

        /// <summary>
        ///   DELETE api/controller/5
        /// </summary>
        /// <param name="codeMajoration">code majoration à supprimer</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/CodeMajoration/Delete")]
        public HttpResponseMessage Delete(CodeMajorationModel codeMajoration)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }

                if (codeMajoration != null)
                {
                    if (this.CodeMajorationMgr.DeleteCodeMajorationById(this.Mapper.Map<CodeMajorationEnt>(codeMajoration)))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        #region Associations Codes majoration / CI

        /// <summary>
        ///   POST Ajout ou Mise à jour des CICodeMajoration
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="ciCodeMajorationList">Liste des relations CI/CodeMajoration</param>    
        /// <returns>Une réponse HTTP</returns>
        [HttpPost]
        [Route("api/CodeMajoration/ManageCICodeMajoration/{ciId}")]
        public HttpResponseMessage ManageCICodeMajoration(int ciId, IEnumerable<CICodeMajorationModel> ciCodeMajorationList)
        {
            return Post(() => Mapper.Map<IEnumerable<CICodeMajorationModel>>(CodeMajorationMgr.ManageCICodeMajoration(ciId, Mapper.Map<IEnumerable<CICodeMajorationEnt>>(ciCodeMajorationList))));
        }

        #endregion

        /// <summary>
        ///   Rechercher les référentiels CodeDeplacement
        /// </summary>
        /// <param name="page">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="recherche">text</param>
        /// <param name="ciId">ciId</param>
        /// <param name="groupeId">groupeId</param>
        /// <param name="isHeureNuit">filter sur les codes majorations</param>
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/CodeMajoration/SearchLight/{page?}/{pageSize?}/{recherche?}/{ciId?}/{groupeId?}/{isOuvrier?}/{isEtam?}/{isCadre?}")]
        public HttpResponseMessage SearchLight(int page = 1, int pageSize = 20, string recherche = "", int? ciId = null, int? groupeId = null, bool? isHeureNuit = null, bool? isOuvrier = null, bool? isEtam = null, bool? isCadre = null)
        {
            if (!groupeId.HasValue && ciId.HasValue)
            {
                groupeId = CiMgr.GetSocieteByCIId(ciId.Value).GroupeId;
            }
            IEnumerable<CodeMajorationEnt> list = this.CodeMajorationMgr.SearchLight(recherche, page, pageSize, groupeId, ciId, isHeureNuit, isOuvrier, isEtam, isCadre).ToList();
            return Get(() => this.Mapper.Map<IEnumerable<CodeMajorationModel>>(list));
        }

        /// <summary>
        ///  Verifie si l'entité est déja utilisée.
        /// </summary>
        /// <param name="codeMajorationId">codeMajorationId</param>  
        /// <returns>retouner une liste de CI</returns>
        [HttpGet]
        [Route("api/CodeMajoration/IsAlreadyUsed/{codeMajorationId}")]
        public HttpResponseMessage IsAlreadyUsed(int codeMajorationId)
        {
            return Get(() => new
            {
                id = codeMajorationId,
                isAlreadyUsed = this.CodeMajorationMgr.IsAlreadyUsed(codeMajorationId)
            });
        }
    }
}
