using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Fred.Business.Referential;
using Fred.Business.Referential.Fournisseur.Common;
using Fred.Business.Referential.Materiel;
using Fred.Business.ReferentielFixe;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using Fred.Framework.Services;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models.Materiel;

namespace Fred.ImportExport.Business.Materiel
{
    public class ServiceImportMateriel : AbstractFluxManager, IServiceImportMateriel
    {
        private readonly IMaterielManager materielManager;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly ISocieteManager societeManager;
        private readonly IReferentielFixeManager referentielFixeManager;
        private readonly IFournisseurManager fournisseurManager;
        private readonly IMapper mapper;
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public ServiceImportMateriel(
            IFluxManager fluxManager,
            IMaterielManager materielManager,
            IUtilisateurManager utilisateurManager,
            ISocieteManager societeManager,
            IReferentielFixeManager referentielFixeManager,
            IFournisseurManager fournisseurManager,
            IMapper mapper)
            : base(fluxManager)
        {
            this.materielManager = materielManager;
            this.utilisateurManager = utilisateurManager;
            this.societeManager = societeManager;
            this.referentielFixeManager = referentielFixeManager;
            this.fournisseurManager = fournisseurManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Permet d'importer les materiels depuis STORM/BRIDGE.
        /// </summary>
        /// <param name="date">La date de modification. Format : yyyy-MM-dd</param>
        /// <param name="restClient">Le Rest qui permet l'appel a SAP</param>
        /// <param name="webApiStormUrl">La base de l URL de SAP</param>
        /// <param name="importJobId">LE Job ID </param>
        /// <param name="codeSocieteComptables">Les Codes des societes comptables</param>
        public async Task ImportMaterielFromStormAsync(string date, RestClient restClient, string webApiStormUrl, string importJobId, List<string> codeSocieteComptables)
        {
            try
            {
                // On vérifie le format de la date.
                var dateRegex = new Regex(@"^[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$");
                if (!dateRegex.IsMatch(date))
                {
                    throw new FredBusinessException("[MATERIEL] Import Materiel : le format de la date \"date\"n'est pas correcte.");
                }

                foreach (string codeSocieteComptable in codeSocieteComptables)
                {
                    string url = $"{webApiStormUrl}&ACTION=IE03&AEDAT={date}&SOCIETE_CODE={codeSocieteComptable}";

                    //On récupère les matériels de STORM.
                    logger.Info($"GET : {url}");
                    var materielModels = await restClient.GetAsync<List<MaterielStormModel>>(url);

                    //On récupère l'utilisateur générique pour STORM.
                    UtilisateurEnt utilisateur = utilisateurManager.GetByLogin("fred_ie");
                    if (!dateRegex.IsMatch(date))
                    {
                        throw new FredBusinessException("[MATERIEL] Import Materiel : L'utilisateur \"fred_ie\" n'existe pas.");
                    }

                    //On récupère la ressource générique pour STORM.
                    RessourceEnt ressource = referentielFixeManager.GetRessource("STORM");
                    if (ressource == null)
                    {
                        throw new FredBusinessException("[MATERIEL] Import Materiel : Aucune ressource pour le code \"STORM\"");
                    }

                    //On récupère la société.                    
                    int? societeId = societeManager.GetSocieteIdByCodeSocieteComptable(codeSocieteComptable);
                    if (societeId == null)
                    {
                        throw new FredBusinessException($"[MATERIEL] Import Materiel {codeSocieteComptable} : Aucune société pour le code ANAEL \"{codeSocieteComptable}\"");
                    }

                    //On ajout ou met à jour les matériels.
                    materielManager.InsertOrUpdate(Transform(materielModels, utilisateur, ressource, societeId));
                }

                // On met à jour la date du flux.

                Flux = FluxManager.GetByCode(importJobId);
                Flux.DateDerniereExecution = DateTime.UtcNow;
                FluxManager.Update(Flux);
            }
            catch (Exception exception)
            {
                throw new FredIeBusinessException(exception.Message, exception);
            }
        }

        /// <summary>
        /// Mapping de la liste model du matériel (SAP) vers la liste MaterielEnt (FRED)
        /// </summary>
        /// <param name="source">Liste source de données</param>
        /// <param name="utilisateur">Utilisateur FRED IE</param>
        /// <param name="ressource">Ressource</param>
        /// <param name="societeId">Idetifiant de la société</param>
        /// <returns>Liste des matériels (FRED)</returns>
        private List<MaterielEnt> Transform(List<MaterielStormModel> source, UtilisateurEnt utilisateur, RessourceEnt ressource, int? societeId)
        {
            List<MaterielEnt> materiels = new List<MaterielEnt>();
            List<MaterielEnt> existingMateriels = materielManager.GetMateriels(societeId.Value, x => source.Select(q => q.Code).Contains(x.Code)).ToList();
            List<string> listOfFournisseurCode = source.Select(k => k.FournisseurCode).Distinct().ToList();
            List<FournisseurIdCodeModel> listOfFournisseurIdCodeModel = fournisseurManager.GetAllIdFournisseurForListOfCode(listOfFournisseurCode);
            foreach (MaterielStormModel materielModel in source)
            {
                try
                {
                    MaterielEnt materiel = mapper.Map<MaterielEnt>(materielModel);
                    FournisseurIdCodeModel fournisseurIdCodeModel = listOfFournisseurIdCodeModel.Find(x => x.CodeFournisseur == materielModel.FournisseurCode);
                    MaterielEnt existingMateriel = existingMateriels.Find(q => q.Code == materielModel.Code);
                    materiel.AuteurCreationId = utilisateur.UtilisateurId;
                    materiel.AuteurModificationId = utilisateur.UtilisateurId;
                    materiel.SocieteId = societeId.Value;
                    materiel.FournisseurId = fournisseurIdCodeModel?.IdFournisseur;
                    materiel.RessourceId = existingMateriel != null && existingMateriel.RessourceId != ressource.RessourceId ? existingMateriel.RessourceId : ressource.RessourceId;
                    materiel.IsStorm = true;

                    materiels.Add(materiel);
                }
                catch (Exception exception)
                {
                    logger.Error(exception);
                }
            }

            return materiels;
        }
    }
}
