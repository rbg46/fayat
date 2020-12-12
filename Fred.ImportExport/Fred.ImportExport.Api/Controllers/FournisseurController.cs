using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Fred.ImportExport.Business;
using Fred.ImportExport.Business.Fournisseur;
using Fred.ImportExport.Models;

namespace Fred.ImportExport.Api.Controllers
{
    /// <summary>
    /// Controller Asp.Net des import d'ANAEL vers FRED
    /// N'utilise pas le versionning
    /// </summary>
    public class FournisseurController : ApiControllerBase
    {
        private readonly FournisseurFluxManager fournisseurFluxManager;

        public FournisseurController(FournisseurFluxManager fournisseurFluxManager)
        {
            this.fournisseurFluxManager = fournisseurFluxManager;
        }

        /// <summary>
        ///   Exécute l'import des fournisseurs d'ANAEL ver FRED
        /// </summary>    
        /// <param name="codeSocieteComptable">Code société comptable ANAEL</param>
        /// <param name="isStormOutputDesactivated">Détermine si on active l'export vers STORM ou pas</param>
        /// <returns>toujours vrai</returns>
        /// <response code="201">Get Statut effectué avec succés</response>
        /// <response code="400">Requête invalide.</response>
        [HttpGet]
        [Route("api/Fournisseur/Import/{codeSocieteComptable}/{isStormOutputDesactivated?}")]
        [Authorize(Roles = "service")]
        public HttpResponseMessage ExecuteImportFournisseur(string codeSocieteComptable, bool isStormOutputDesactivated = false)
        {
            const string typeSequences = "'TIERS', 'TIERS2', 'GROUPE'";
            const string regleGestions = "'F'";

            return Get(() => { fournisseurFluxManager.ExecuteImport(false, codeSocieteComptable, typeSequences, regleGestions, isStormOutputDesactivated); return true; });
        }

        /// <summary>
        /// Permet d'importer une liste de fourniseurs;
        /// </summary>
        /// <returns>Une reponse HTTP.</returns>
        /// <param name="fournisseurs">Une liste de fournisseur.</param>
        /// <response code="200">Succès de la requête.</response>
        /// <response code="400">La syntaxe de la requête est erronée.</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpPost]
        [Route("api/Fournisseur/ImportFournisseurs")]
        [Authorize(Users = "userserviceFes, userserviceFTP", Roles = "service")]
        public async Task<IHttpActionResult> ImportFournisseursAsync(List<FournisseurModel> fournisseurs)
        {
            return await PostTaskAsync(async () => await fournisseurFluxManager.ImportFournisseursAsync(fournisseurs));
        }

        /// <summary>
        /// Permet d'importer un fournisseur simple, un fournisseur avec agence ou une agence simple;
        /// </summary>
        /// <returns>Une reponse HTTP.</returns>
        /// <param name="fournisseur">Le modele fournisseur envoye de l'exterieur.</param>
        /// <response code="200">Succès de la requête.</response>
        /// <response code="400">La syntaxe de la requête est erronée.</response>
        /// <response code="401">L'utilisateur non authentifié.</response>
        /// <response code="403">Accès refusé.</response>
        /// <response code="500">Erreur serveur.</response>
        [HttpPost]
        [Route("api/Fournisseur/ImportFournisseur")]
        [Authorize(Users = "userserviceFTP", Roles = "service")]
        public HttpResponseMessage ImportFournisseur(ImportFournisseurModel fournisseur)
        {
            return Post(() =>
            {
                if (fournisseur != null)
                {
                    fournisseurFluxManager.AddOrUpdateFournisseur(fournisseur);
                }
            });
        }
    }
}
