using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fred.Business.Referential.Tache;
using Fred.Business.RepriseDonnees.PlanTaches.ContextProviders;
using Fred.Business.RepriseDonnees.PlanTaches.ExcelDataExtractor;
using Fred.Business.RepriseDonnees.PlanTaches.Mapper;
using Fred.Business.RepriseDonnees.PlanTaches.Models;
using Fred.Business.RepriseDonnees.PlanTaches.Validators;
using Fred.Business.RepriseDonnees.PlanTaches.Validators.Results;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.Entities.RepriseDonnees;

namespace Fred.Business.RepriseDonnees.PlanTaches
{
    public class RepriseDonneesPlanTachesManager : IRepriseDonneesPlanTachesManager
    {
        private readonly IUnitOfWork uow;
        private readonly IPlanTachesDataMapper planTachesDataMapper;
        private readonly IPlanTachesContextProvider planTachesContextProvider;
        private readonly IReprisePlanTachesRepository reprisePlanTachesRepository;
        private readonly IPlanTachesExtractorService planTachesExtractorService;
        private readonly IPlanTachesValidatorService planTachesValidatorService;
        private readonly ITacheManager tacheManager;


        public RepriseDonneesPlanTachesManager(
            IUnitOfWork uow,
            IPlanTachesDataMapper planTachesDataMapper,
            IPlanTachesContextProvider planTachesContextProvider,
            IReprisePlanTachesRepository reprisePlanTachesRepository,
            IPlanTachesExtractorService planTachesExtractorService,
            IPlanTachesValidatorService planTachesValidatorService,
            ITacheManager tacheManager)
        {
            this.uow = uow;
            this.planTachesDataMapper = planTachesDataMapper;
            this.planTachesContextProvider = planTachesContextProvider;
            this.reprisePlanTachesRepository = reprisePlanTachesRepository;
            this.planTachesExtractorService = planTachesExtractorService;
            this.planTachesValidatorService = planTachesValidatorService;
            this.tacheManager = tacheManager;
        }

        /// <summary>
        /// Importation des Plan de tâches
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>le resultat de l'import</returns>
        public ImportPlanTachesResult CreatePlanTaches(int groupeId, Stream stream)
        {
            ImportPlanTachesResult result = new ImportPlanTachesResult();

            // Recuperation des données de la feuille excel.
            ParsePlanTachesResult parsageResult = planTachesExtractorService.ParseExcelFile(stream);

            // Récuperations de toutes les données qui sont necessaires pour faire un import Plan de tâches(mise a jour).
            ContextForImportPlanTaches context = planTachesContextProvider.GetContextForImportPlanTaches(groupeId, parsageResult.PlanTaches);

            // Verification des règles(RG) de l'import.
            PlanTachesImportRulesResult importRulesResult = planTachesValidatorService.VerifyImportRules(parsageResult.PlanTaches, context);

            if (importRulesResult.AllLignesAreValid())
            {
                result.IsValid = true;

                // Ici je crée les Plan de tâches
                List<TacheEnt> planTachesEnts = planTachesDataMapper.Transform(context, parsageResult.PlanTaches);

                if (planTachesEnts.Count > 0)
                {
                    // Gestion des tâches par défaut (si on en importe une, il faut que l'ancienne présente en BDD ne soit plus ParDefaut)
                    HandleTachesParDefaut(planTachesEnts);

                    reprisePlanTachesRepository.CreatePlanTaches(planTachesEnts);

                    uow.Save();
                }
            }
            else
            {
                result.ErrorMessages = importRulesResult.ImportRuleResults.Where(x => !x.IsValid).Select(x => x.ErrorMessage).ToList();
            }
            return result;
        }

        private void HandleTachesParDefaut(List<TacheEnt> planTachesEnts)
        {
            // 1- Récupération de chaque tache par défaut de la liste planTachesEnts de tâches à importer
            List<TacheEnt> nouvellesTachesParDefaut = planTachesEnts.Where(t => t.TacheParDefaut).ToList();
            if (nouvellesTachesParDefaut.Count > 0)
            {
                // 2- Pour chaque tache par défaut on cherche pour ce CI (donc plan de tache) sa tache par défaut actuelle en BDD et on la met à jour (ParDefaut = faux)
                List<TacheEnt> anciennesTachesParDefaut = new List<TacheEnt>();

                foreach (TacheEnt tache in nouvellesTachesParDefaut)
                {
                    TacheEnt tacheToUpdate = tacheManager.GetTacheParDefaut(tache.CiId);
                    tacheToUpdate.TacheParDefaut = false;
                    tacheToUpdate.DateModification = DateTime.UtcNow;

                    anciennesTachesParDefaut.Add(tacheToUpdate);
                }

                if (anciennesTachesParDefaut.Count > 0)
                {
                    // 3- Mise à jour en BDD
                    reprisePlanTachesRepository.UpdateTachesParDefaut(anciennesTachesParDefaut);
                }
            }
        }
    }
}
