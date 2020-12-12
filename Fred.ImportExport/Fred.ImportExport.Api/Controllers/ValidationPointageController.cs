using System;
using System.Net.Http;
using System.Web.Http;
using Fred.Entities.ValidationPointage;
using Fred.ImportExport.Business.ValidationPointage.Factory.Interfaces;
using Fred.Web.Shared.Models;

namespace Fred.ImportExport.Api.Controllers
{
    /// <summary>
    /// Controller Asp.Net des validations de pointage
    /// N'utilise pas le versionning : Utilisé par Fred.Web uniquement
    /// </summary>
    [Authorize(Roles = "service")]
    public class ValidationPointageController : ApiControllerBase
    {
        private readonly IValidationPointageFluxFactory validationPointageFluxFactory;

        public ValidationPointageController(IValidationPointageFluxFactory validationPointageFluxFactory)
        {
            this.validationPointageFluxFactory = validationPointageFluxFactory;
        }

        /// <summary>
        ///   Exécute le contrôle vrac
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        /// <param name="lotPointageId">Identifiant Lot de pointage</param>    
        /// <param name="filtre">Filtre sur le lot de pointage</param>
        /// <returns>Lot de pointage avec statut</returns>
        /// <response code="201">Controle vrac effectué avec succés</response>
        /// <response code="400">Requête invalide.</response>
        [HttpPost]
        [Route("api/ValidationPointage/ControleVrac/{utilisateurId}/{lotPointageId}")]
        public HttpResponseMessage ControleVrac(int utilisateurId, int lotPointageId, PointageFiltre filtre)
        {
            return Post(() =>
            {
                IValidationPointageFluxManager fluxManager = validationPointageFluxFactory.GetFluxManager(utilisateurId);
                ControlePointageResult controlePointage = fluxManager.ExecuteControleVrac(utilisateurId, lotPointageId, filtre);

                return controlePointage;
            });
        }

        /// <summary>
        ///   Exécute la remontée vrac
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>    
        /// <param name="periode">Periode choisie</param>    
        /// <param name="filtre">Filtre sur le lot de pointage</param>
        /// <returns>Lot de pointage avec statut</returns>
        /// <response code="201">Remontée vrac effectué avec succés</response>
        /// <response code="400">Requête invalide.</response>
        [HttpPost]
        [Route("api/ValidationPointage/RemonteeVrac/{utilisateurId}/{periode}")]
        public HttpResponseMessage RemonteeVrac(int utilisateurId, DateTime periode, PointageFiltre filtre)
        {
            return Post(() =>
            {
                IValidationPointageFluxManager fluxManager = validationPointageFluxFactory.GetFluxManager(utilisateurId);
                RemonteeVracResult remonteeVrac = fluxManager.ExecuteRemonteeVrac(utilisateurId, periode, filtre);

                return remonteeVrac;
            });
        }

        /// <summary>
        ///   Exécute la remontée Primes
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>    
        /// <param name="periode">Periode choisie</param>    
        /// <returns>Lot de pointage avec statut</returns>
        /// <response code="201">Remontée vrac effectué avec succés</response>
        /// <response code="400">Requête invalide.</response>
        [HttpPost]
        [Route("api/ValidationPointage/RemonteePrimes/{utilisateurId}/{periode}")]
        public HttpResponseMessage RemonteePrimes(int utilisateurId, DateTime periode)
        {
            return Post(() =>
            {
                IValidationPointageFluxManager fluxManager = validationPointageFluxFactory.GetFluxManager(utilisateurId);
                fluxManager.ExecuteRemonteePrimes(periode, utilisateurId);
            });
        }
    }
}
