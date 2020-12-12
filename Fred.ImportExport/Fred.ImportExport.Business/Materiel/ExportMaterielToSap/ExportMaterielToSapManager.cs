using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Fred.Business.CI;
using Fred.Business.Rapport;
using Fred.Business.Rapport.Pointage;
using Fred.Business.Referential.Materiel;
using Fred.Business.Societe;
using Fred.Entities.CI;
using Fred.Entities.Rapport;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using Fred.Framework.Services;
using Fred.ImportExport.Business.ApplicationSap;
using Fred.ImportExport.Business.WorkflowLogicielTiers;
using Fred.ImportExport.Entities;
using Fred.ImportExport.Models.Materiel;
using NLog;

namespace Fred.ImportExport.Business.Materiel.ExportMaterielToSap
{
    public class ExportMaterielToSapManager : IExportMaterielToSapManager
    {
        private const string FluxName = "J3G$";
        private readonly string formatErrorFluxJ3G = "[MATERIEL][POINTAGE][FLUX_J3G$] [PARAMETERS] rapportId ({0}).";
        private readonly string formatMessageEnvoie = "[MATERIEL][POINTAGE][FLUX_J3G$] - Tentative d'envoie a SAP du rapport ({0}).";
        private readonly string formatMessageEnvoiePost = "[MATERIEL][POINTAGE][FLUX_J3G$] POST : {0}&ACTION=J3G$";
        private readonly string formatMessageReponseSap = "[MATERIEL][POINTAGE][FLUX_J3G$] Réponse de Sap : ";
        private readonly string formatMessageReponseSapDetail = "[MATERIEL][POINTAGE][FLUX_J3G$] Réponse de Sap détaillée : ";
        private readonly string formatMessageReponseSapContent = "[MATERIEL][POINTAGE][FLUX_J3G$] Réponse de Sap Content : ";
        private readonly string formatMessageReponseUndefined = "[MATERIEL][POINTAGE][FLUX_J3G$] Pas de réponse de Sap.";
        private readonly string formatErrorNoConfigurationForSociete = "[MATERIEL][POINTAGE][FLUX_J3G$] : Il n'y a pas de configuration correspondant à cette société ({0}).";
        private readonly string formatErrorNoSocieteParentForOraganisation = "[MATERIEL][POINTAGE][FLUX_J3G$] : Il n'y a pas de société correspondant à cette organisation ({0}).";
        private readonly string formatErrorRapportLigneErrors = "[MATERIEL][POINTAGE][FLUX_J3G$] : le rapport contient des lignes en erreurs. rapportId = {0}";

        private readonly IMapper mapper;
        private readonly IApplicationsSapManager applicationsSapManager;
        private readonly ILogicielTiersManager logicielTiersManager;
        private readonly IWorkflowPointageManager workflowPointageManager;
        private readonly ICIManager ciManager;
        private readonly IRapportManager rapportManager;
        private readonly IMaterielManager materielManager;
        private readonly IPointageManager pointageManager;
        private readonly ISocieteManager societeManager;
        private readonly ISepService sepService;

        public ExportMaterielToSapManager(
            IMapper mapper,
            IApplicationsSapManager applicationsSapManager,
            ILogicielTiersManager logicielTiersManager,
            IWorkflowPointageManager workflowPointageManager,
            ICIManager ciManager,
            IRapportManager rapportManager,
            IMaterielManager materielManager,
            IPointageManager pointageManager,
            ISocieteManager societeManager,
            ISepService sepService)
        {
            this.mapper = mapper;
            this.applicationsSapManager = applicationsSapManager;
            this.logicielTiersManager = logicielTiersManager;
            this.workflowPointageManager = workflowPointageManager;
            this.ciManager = ciManager;
            this.rapportManager = rapportManager;
            this.materielManager = materielManager;
            this.pointageManager = pointageManager;
            this.societeManager = societeManager;
            this.sepService = sepService;
        }


        /// <summary>
        /// Exporte les pointages materiel a partir d'un rapportId
        /// </summary>
        /// <param name="rapportId">rapportId</param>
        /// <param name="backgroundJobId">Background job identifiant</param>
        public async Task<List<Exception>> ExportPointageToStormAsync(int rapportId, string backgroundJobId)
        {
            var exceptions = new List<Exception>();

            try
            {
                RapportEnt dbRapport = rapportManager.FindById(rapportId);
                if (dbRapport != null)
                {
                    int? dbCiOrganisationId = ciManager.GetOrganisationIdByCiId(dbRapport.CiId);
                    if (dbCiOrganisationId.HasValue)
                    {
                        SocieteEnt societe = societeManager.GetSocieteParentByOrgaId(dbCiOrganisationId.Value);
                        CIEnt ciCompteInterne = null;
                        if (sepService.IsSep(societe))
                        {
                            ciCompteInterne = ciManager.GetCICompteInterneByCiid(dbRapport.CiId);
                            societe = ciCompteInterne != null && ciCompteInterne.Societe != null ? ciCompteInterne.Societe : societe;
                        }

                        await GetParametersAndSendToSapAsync(dbRapport, societe, ciCompteInterne, dbCiOrganisationId, backgroundJobId);
                    }
                }
            }
            catch (Exception exception)
            {
                LogError(exception, formatErrorFluxJ3G, rapportId);
                exceptions.Add(exception);
            }

            return exceptions;
        }

        /// <summary>
        ///     Envoi des pointages de chaque rapport de la liste 
        /// </summary>
        /// <param name="rapportIds">Liste de rapport</param>
        /// <param name="backgroundJobId">Background job identifiant</param>
        public async Task ExportPointageToStormAsync(List<int> rapportIds, string backgroundJobId)
        {
            var exceptions = new List<Exception>();
            foreach (int rapportId in rapportIds)
            {
                exceptions.AddRange(await ExportPointageToStormAsync(rapportId, backgroundJobId));
            }

            if (exceptions.Any())
            {
                throw exceptions.First();
            }
        }

        private async Task GetParametersAndSendToSapAsync(RapportEnt rapport, SocieteEnt societe, CIEnt ciCompteInterne, int? dbCiOrganisationId, string backgroundJobId)
        {
            if (societe == null)
            {
                LogError(formatErrorNoSocieteParentForOraganisation, dbCiOrganisationId);
                return;
            }

            ApplicationSapParameter param = applicationsSapManager.GetParametersForSociete(societe.SocieteId);

            if (param.IsFound)
            {
                var uri = new Uri(param.Url);
                string mandant = HttpUtility.ParseQueryString(uri.Query).Get("SAP-CLIENT");

                LogicielTiersEnt logicielSap = logicielTiersManager.GetOrCreateLogicielTiers("SAP", uri.Host, mandant);

                IEnumerable<RapportLigneEnt> ligneAEnvoyer = GetLignesAEnvoyer(rapport, logicielSap, backgroundJobId);
                await SendToSapAsync(ligneAEnvoyer, societe, ciCompteInterne, param);
            }
            else
            {
                LogError(formatErrorNoConfigurationForSociete, societe.SocieteId);
            }
        }

        private IEnumerable<PointageMaterielStormModel> Transform(SocieteEnt societe, CIEnt ciCompteInterne, List<RapportLigneEnt> pointages)
        {
            //On récupérer le validateur du rapport : RG A3.5
            //Si ValideurDRCId Vide, prendre alors ValideurCDTid. Si ValideurCDTid Vide prendre alors ValideurCDCId 
            RapportEnt rapport = pointages[0].Rapport;
            UtilisateurEnt utilisateur;

            if (rapport.ValideurDRC != null)
            {
                utilisateur = rapport.ValideurDRC;
            }
            else if (rapport.ValideurCDT != null)
            {
                utilisateur = rapport.ValideurCDT;
            }
            else if (rapport.ValideurCDC != null)
            {
                utilisateur = rapport.ValideurCDC;
            }
            else if (rapport.AuteurVerrou != null)
            {
                utilisateur = rapport.AuteurVerrou;
            }
            else if (rapport.AuteurModification != null)
            {
                utilisateur = rapport.AuteurModification;
            }
            else
            {
                utilisateur = rapport.AuteurCreation;
            }

            var pointageMaterielStormModels = mapper.Map<List<PointageMaterielStormModel>>(pointages);

            foreach (PointageMaterielStormModel pointageModel in pointageMaterielStormModels)
            {
                pointageModel.MaterielSocieteCode = ciCompteInterne != null ? pointageModel.MaterielSocieteCode : societe.Code;
                pointageModel.CiCode = ciCompteInterne != null ? ciCompteInterne.Code : pointageModel.CiCode;
                pointageModel.SocieteComptableCode = societe.CodeSocieteComptable;
                pointageModel.ValideurMatricule = utilisateur.Personnel.Matricule;
                pointageModel.ValideurSocieteCode = utilisateur.Personnel.Societe.Code;
                pointageModel.TotalHeuresTaches = pointageModel.Taches.Sum(x => x.HeureTache);
            }

            return pointageMaterielStormModels;
        }

        private async Task SendToSapAsync(IEnumerable<RapportLigneEnt> lignes, SocieteEnt societe, CIEnt ciCompteInterne, ApplicationSapParameter param)
        {
            if (lignes.Any())
            {
                IEnumerable<PointageMaterielStormModel> pointageMaterielStormModels = Transform(societe, ciCompteInterne, lignes.ToList());

                LogMessage(formatMessageEnvoie, lignes.FirstOrDefault().RapportId);
                var restClientSap = new RestClient(param.Login, param.Password);
                string url = $"{param.Url}&ACTION=J3G$";

                //On log dans la base de données même si l'envoi a SAP échoue 

                LogMessage(formatMessageEnvoiePost, url);
                HttpResponseMessage response = await restClientSap.PostAsync(url, pointageMaterielStormModels);
                LogResponse(response);
                response.EnsureSuccessStatusCode();
            }
        }

        public IEnumerable<RapportLigneEnt> GetLignesAEnvoyer(RapportEnt rapport, LogicielTiersEnt logicielSap, string backgroundJobId)
        {
            int auteurId = rapport.AuteurVerrouId ?? rapport.AuteurModificationId ?? rapport.AuteurCreationId.Value;

            List<RapportLigneEnt> rapportLignes = pointageManager.GetAllPointagesForMaterielStorm(rapport.RapportId);
            List<RapportLigneEnt> lignesMaterielOnly = rapportLignes.Where(x => x.Materiel != null && x.Materiel.IsStorm && !x.Materiel.MaterielLocation).ToList();

            pointageManager.CheckListPointagesMaterielOnly(lignesMaterielOnly);

            if (lignesMaterielOnly.Any(x => x.ListErreurs.Any() && !x.DateSuppression.HasValue))
            {
                LogError(formatErrorRapportLigneErrors, rapport.RapportId);
                return Enumerable.Empty<RapportLigneEnt>();
            }

            IEnumerable<RapportLigneEnt> lignesAEnvoyer = FiltreLignesRapportAEnvoyer(lignesMaterielOnly, logicielSap.FredLogicielTiersId);
            // recherche des lignes à supprimer sur toutes les lignes du rapport
            List<RapportLigneEnt> lignesASupprimer = GetLignesRapportASupprimer(rapportLignes, logicielSap.FredLogicielTiersId);
            IEnumerable<RapportLigneEnt> lignes = lignesASupprimer.Concat(lignesAEnvoyer);

            if (lignes.Any())
            {
                workflowPointageManager.SaveWorkflowPointage(lignes, logicielSap.FredLogicielTiersId, auteurId, backgroundJobId, FluxName);
                return lignes;
            }

            return Enumerable.Empty<RapportLigneEnt>();
        }

        /// <summary>
        /// On n'envoi au logiciel que les lignes de rapport ayant des changements 
        /// On retire de la liste des lignes à envoyer toutes les lignes ayant été supprimé avant du premier envoi du rapport a SAP
        /// Si l'id du logiciel passé en paramètre est null, alors la fonction retourne les lignes passées en paramètre
        /// </summary>
        /// <param name="lignes">lignes contenu dans le rapport</param>
        /// <param name="logicielTiersId">logiciel tiers vers lequel on doit exporter ces lignes</param>
        /// <returns>Une liste de ligne de rapport, potentiellement vide, jamais null</returns>
        private IEnumerable<RapportLigneEnt> FiltreLignesRapportAEnvoyer(IEnumerable<RapportLigneEnt> lignes, int? logicielTiersId)
        {
            if (!logicielTiersId.HasValue)
            {
                return lignes;
            }

            IEnumerable<int> lignesDejaEnvoyees = workflowPointageManager.GetRapportLigneIdDejaEnvoye(logicielTiersId.Value, FluxName);

            IEnumerable<RapportLigneEnt> filtreLignesRapportAEnvoyer = lignes.Where(l =>
                l.DateSuppression == null
                ||
                (l.DateSuppression != null && lignesDejaEnvoyees.Contains(l.RapportLigneId)));
            return filtreLignesRapportAEnvoyer.ToList();
        }

        /// <summary>
        /// Pour chaque ligne de rapport, recherche de la dernière ligne envoyée à SAP stocké dans WorkflowPointage, 
        /// si le matériel a changé, créée une ligne à supprimer avec l'ancien matériel et l'ajoute à la collection de lignes à supprimer
        /// </summary>
        /// <param name="lignes">Liste de ligges de rapport</param>
        /// <param name="logicielTiersId">logiciel tiers vers lequel on doit exporter ces lignes</param>
        /// <returns>Une liste de ligne de rapport, potentiellement vide, jamais null</returns>
        private List<RapportLigneEnt> GetLignesRapportASupprimer(IEnumerable<RapportLigneEnt> lignes, int logicielTiersId)
        {
            var listeLigneSuppression = new List<RapportLigneEnt>();

            // recherche de l'historique des workflow pointages du rapport
            List<WorkflowPointageEnt> rapportWorkflowPointages = workflowPointageManager.GetRapportLigneWorkflowPointages(lignes.Select(x => x.RapportLigneId).ToList(), logicielTiersId, FluxName);
            if (!rapportWorkflowPointages.Any())
            {
                return listeLigneSuppression;
            }

            foreach (RapportLigneEnt ligne in lignes.Where(x => !x.DateSuppression.HasValue))
            {
                // recherche le dernier workflowpointage envoyé pour le rapporLigneId
                WorkflowPointageEnt dernierWorkflowPointageEnvoye = rapportWorkflowPointages
                    .Where(x => x.RapportLigneId == ligne.RapportLigneId)
                        .OrderByDescending(x => x.WorkflowId)
                    .FirstOrDefault();
                // si le personnel a changé depuis le dernier envoi (et qu'il n'est pas supprimé), création d'une ligne de suppression avec l'ancien personnel
                if (dernierWorkflowPointageEnvoye != null
                    && dernierWorkflowPointageEnvoye.MaterielId != ligne.MaterielId
                    && !dernierWorkflowPointageEnvoye.DateEnvoiSuppression.HasValue)
                {
                    RapportLigneEnt ligneASupprimer = ligne.Duplicate();
                    ligneASupprimer.RapportLigneId = ligne.RapportLigneId;
                    ligneASupprimer.DatePointage = ligne.DatePointage;
                    ligneASupprimer.AuteurCreationId = ligne.AuteurCreationId;
                    ligneASupprimer.AuteurCreation = ligne.AuteurCreation;
                    ligneASupprimer.DateCreation = ligne.DateCreation;
                    ligneASupprimer.AuteurModificationId = ligne.AuteurModificationId;
                    ligneASupprimer.AuteurModification = ligne.AuteurModification;
                    ligneASupprimer.DateModification = ligne.DateModification;
                    // Informations de suppression
                    ligneASupprimer.AuteurSuppressionId = ligne.AuteurModificationId ?? ligne.AuteurCreationId;
                    ligneASupprimer.AuteurSuppression = ligne.AuteurModification ?? ligne.AuteurCreation;
                    ligneASupprimer.DateSuppression = ligne.DateModification ?? DateTime.Now;
                    // Informations de materiel à supprimer
                    ligneASupprimer.MaterielId = dernierWorkflowPointageEnvoye.MaterielId;
                    ligneASupprimer.Materiel = materielManager.GetMaterielByIdWithSociete(dernierWorkflowPointageEnvoye.MaterielId.Value);
                    ligneASupprimer.MaterielMarche = dernierWorkflowPointageEnvoye.MaterielMarche;
                    ligneASupprimer.MaterielArret = dernierWorkflowPointageEnvoye.MaterielArret;
                    ligneASupprimer.MaterielIntemperie = dernierWorkflowPointageEnvoye.MaterielIntemperie;
                    ligneASupprimer.MaterielPanne = dernierWorkflowPointageEnvoye.MaterielPanne;
                    // pas de suppression si pas de matériel trouvé
                    if (ligneASupprimer.Materiel != null)
                    {
                        listeLigneSuppression.Add(ligneASupprimer);
                    }
                }
            }

            return listeLigneSuppression;
        }

        private void LogError(string stringformatError, params object[] infos)
        {
            LogManager.GetCurrentClassLogger().Error(string.Format(stringformatError, infos));
        }

        private void LogError(Exception exception, string stringformatError, params object[] infos)
        {
            LogManager.GetCurrentClassLogger().Error(exception, stringformatError, infos);
        }

        private void LogMessage(string formatMessage, params object[] infos)
        {
            LogManager.GetCurrentClassLogger().Info(string.Format(formatMessage, infos));
        }

        /// <summary>
        /// Log un message détaillé de la réponse Http
        /// </summary>
        /// <param name="response">réponse http</param>
        private void LogResponse(HttpResponseMessage response)
        {
            if (response != null)
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"{formatMessageReponseSap} {(int)response.StatusCode} : {response.StatusCode.ToString()}");
                NLog.LogManager.GetCurrentClassLogger().Info($"{formatMessageReponseSapDetail} {response.ToString()}");
                NLog.LogManager.GetCurrentClassLogger().Info($"{formatMessageReponseSapContent} {response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult()}");
            }
            else
            {
                NLog.LogManager.GetCurrentClassLogger().Error(formatMessageReponseUndefined);
            }
        }
    }
}
