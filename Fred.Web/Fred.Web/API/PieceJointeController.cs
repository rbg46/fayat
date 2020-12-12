using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Fred.Business.Common.ExportDocument;
using Fred.Business.PieceJointe.Services;
using Fred.Business.Utilisateur;
using Fred.Entities;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.Models.PieceJointe;

namespace Fred.Web.API
{
    [Authorize]
    public class PieceJointeController : ApiControllerBase
    {
        private readonly string commonFolder = ConfigurationManager.AppSettings["attachment:folder"];
        private readonly string commandeFolder = ConfigurationManager.AppSettings["attachment:subfolder:commandes"];
        private readonly string receptionFolder = ConfigurationManager.AppSettings["attachment:subfolder:receptions"];
        private readonly string cgaFolder = ConfigurationManager.AppSettings["cga:folder"];

        private readonly IMapper mapper;
        private readonly IExportDocumentService exportDocumentService;
        private readonly IPieceJointeService pieceJointeService;
        private readonly IUtilisateurManager utilisateurManager;

        public PieceJointeController(
            IMapper mapper,
            IExportDocumentService exportDocumentService,
            IPieceJointeService pieceJointeService,
            IUtilisateurManager utilisateurManager)
        {
            this.mapper = mapper;
            this.exportDocumentService = exportDocumentService;
            this.pieceJointeService = pieceJointeService;
            this.utilisateurManager = utilisateurManager;
        }

        /// <summary>
        /// Récupérer toutes les pièces jointes attachées à une entité
        /// </summary>
        /// <param name="typeEntite">Filtre sur le type de l'entité (Commande / Dépense / ...)</param>
        /// <param name="entiteId">Filtre sur l'id de l'entité</param>
        /// <returns>Liste des pièces jointes attachées à une entité</returns>
        [HttpGet]
        [Route("api/PieceJointe/GetPiecesJointes/{typeEntite}/{entiteId}")]
        public HttpResponseMessage GetPiecesJointes(PieceJointeTypeEntite typeEntite, int entiteId)
        {
            return this.Get(() =>
            {
                List<PieceJointeEnt> pjs = pieceJointeService.GetPiecesJointes(typeEntite, entiteId);

                return mapper.Map<IEnumerable<PieceJointeModel>>(pjs);
            });
        }

        /// <summary>
        /// Télécharger la pièce jointe suivant l'ID renseigné
        /// </summary>
        /// <param name="pieceJointeId">Identifiant de la pièce jointe</param>
        /// <returns>Retourne le fichier correspondant à la pièce jointe</returns>
        [HttpGet]
        [Route("api/PieceJointe/Download/{pieceJointeId}")]
        public HttpResponseMessage Download(int pieceJointeId)
        {
            try
            {
                PieceJointeEnt pj = pieceJointeService.GetPieceJointeWithFile(commonFolder, pieceJointeId);
                return exportDocumentService.CreateResponseForDownloadDocument(pj.Libelle, pj.PieceJointeArray);
            }
            catch (FredBusinessNotFoundException e)
            {
                logger.Error(e, e.Message);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, e);
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Télécharger la pièce jointe suivant le nom du fichier CGA de l'EtablissementComptable renseigné
        /// </summary>
        /// <param name="pieceJointeCGAName">Nom de la pièce jointe CGA</param>
        /// <returns>Retourne le fichier correspondant à la pièce jointe CGA</returns>
        [HttpGet]
        [Route("api/PieceJointe/DownloadCGA/{pieceJointeCGAName}")]
        public HttpResponseMessage DownloadCGA(string pieceJointeCGAName)
        {
            try
            {
                byte[] bytes = null;
                string filePath = $"{cgaFolder}{pieceJointeCGAName}";
                if (File.Exists(filePath))
                {
                    bytes = File.ReadAllBytes(filePath);
                }
                return exportDocumentService.CreateResponseForDownloadDocument(pieceJointeCGAName, bytes);
            }
            catch (FredBusinessNotFoundException e)
            {
                logger.Error(e, e.Message);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, e);
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Ajouter une pièces jointe et l'attacherà une entité (Commande / Depense / ...)
        /// </summary>
        /// <returns>Liste des pièce jointes mises à jour</returns>
        [HttpPost]
        [Route("api/PieceJointe/Add")]
        public async Task<HttpResponseMessage> AddAsync()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            MultipartFormDataStreamProvider provider = new MultipartFormDataStreamProvider(root);

            // Lire le multipart et le sauvegarder dans répertoire temporaire (App_Data)
            await Request.Content.ReadAsMultipartAsync(provider);

            return Post(() =>
            {
                // Récupérer les données du multi-part
                PieceJointeTypeEntite pieceJointeTypeEntite = (PieceJointeTypeEntite)Enum.Parse(typeof(PieceJointeTypeEntite), provider.FormData.Get("PieceJointeTypeEntite"));

                // Sélection du path spécifique selon le type de l'entité
                string specificPath = null;
                switch (pieceJointeTypeEntite)
                {
                    case PieceJointeTypeEntite.Commande:
                        specificPath = commandeFolder;
                        break;
                    case PieceJointeTypeEntite.Reception:
                        specificPath = receptionFolder;
                        break;
                }

                // Récupérer l'utilisateur
                int userId = utilisateurManager.GetContextUtilisateurId();

                // Récupérer l'ID de l'entité
                int entiteId = Convert.ToInt32(provider.FormData.Get("EntiteId"));

                // Récupérer le nom du fichier
                string fileName = provider.FileData[0].Headers.ContentDisposition.FileName.Trim('\"');

                // Pièce ajoutée
                PieceJointeEnt pieceJointeAdded = pieceJointeService.Add(
                    userId,
                    pieceJointeTypeEntite,
                    entiteId,
                    commonFolder,
                    specificPath,
                    fileName,
                    provider.FileData[0].LocalFileName);

                // Mapper la pièce jointe
                return mapper.Map<PieceJointeModel>(pieceJointeAdded);
            });
        }


        /// <summary>
        /// Supprimer une pièces jointe et la détacher d'une entité (Commande / Depense / ...)
        /// </summary>
        /// <param name="idPieceJointe">L'ID de la pièce jointe</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpDelete]
        [Route("api/PieceJointe/Delete/{idPieceJointe}")]
        public HttpResponseMessage Delete(int idPieceJointe)
        {
            // Supprimer une pièce jointe
            return Delete(() => pieceJointeService.Delete(commonFolder, idPieceJointe));
        }

    }
}
