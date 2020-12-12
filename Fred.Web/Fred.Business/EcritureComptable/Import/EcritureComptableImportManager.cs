using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.Commande;
using Fred.Business.Facturation;
using Fred.Business.FeatureFlipping;
using Fred.Business.OperationDiverse;
using Fred.Business.Organisation;
using Fred.Business.Reception;
using Fred.Business.Referential;
using Fred.Business.Referential.Nature;
using Fred.Business.ReferentielFixe;
using Fred.Business.Valorisation;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.EcritureComptable;
using Fred.Entities.Journal;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using Fred.Framework.DateTimeExtend;
using Fred.Framework.Extensions;
using Fred.Framework.FeatureFlipping;
using Fred.Framework.Models;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.Commande;
using Fred.Web.Shared.Models.EcritureComptable;
using Fred.Web.Shared.Models.Facture;
using Fred.Web.Shared.Models.OperationDiverse;
using Fred.Web.Shared.Models.Reception;
using Fred.Web.Shared.Models.Valorisation;

namespace Fred.Business.EcritureComptable.Import
{
    public class EcritureComptableImportManager : IEcritureComptableImportManager
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IOrganisationManager organisationManager;
        private readonly IEcritureComptableManager ecritureComptableManager;
        private readonly IDeviseManager deviseManager;
        private readonly IEcritureComptableCumulManager ecritureComptableCumulManager;
        private readonly IEcritureComptableRejetManager ecritureComptableRejetManager;
        private readonly IOperationDiverseManager operationDiverseManager;
        private readonly IReferentielFixeManager referentielFixeManager;
        private readonly INatureManager natureManager;
        private readonly IValorisationManager valorisationManager;
        private readonly ICommandeManager commandeManager;
        private readonly IReceptionManager receptionManager;
        private readonly IFacturationManager facturationManager;
        private readonly IFeatureFlippingManager featureFlippingManager;

        private readonly EcritureComptableImportHelper ecritureComptableImportHelper;
        private readonly ISocieteRepository reposociete;
        private readonly ICIRepository repoCi;
        private readonly IEtablissementComptableRepository repoEtablissement;
        private readonly EcritureComptableMapper ecritureComptableMapper;
        private readonly OperationDiverseMapper operationDiverseMapper;
        private readonly EcritureComptableConverter ecritureComptableConverter;

        public EcritureComptableImportManager(
            IUnitOfWork unitOfWork,
            IOrganisationManager OrganisationManager,
            IEcritureComptableManager EcritureComptableManager,
            IDeviseManager DeviseManager,
            IEcritureComptableRejetManager EcritureComptableRejetManager,
            IEcritureComptableCumulManager EcritureComptableCumulManager,
            IOperationDiverseManager OperationDiverseManager,
            IReferentielFixeManager ReferentielFixeManager,
            INatureManager NatureManager,
            IValorisationManager ValorisationManager,
            ICommandeManager CommandeManager,
            IReceptionManager ReceptionManager,
            IFacturationManager FacturationManager,
            EcritureComptableImportHelper ecritureComptableImportHelper,
            ISocieteRepository reposociete,
            ICIRepository repoCi,
            IEtablissementComptableRepository repoEtablissement,
            IFeatureFlippingManager FeatureFlippingManager)
        {
            this.unitOfWork = unitOfWork;
            this.ecritureComptableImportHelper = ecritureComptableImportHelper;
            this.reposociete = reposociete;
            this.repoCi = repoCi;
            this.repoEtablissement = repoEtablissement;
            organisationManager = OrganisationManager;
            ecritureComptableManager = EcritureComptableManager;
            deviseManager = DeviseManager;
            ecritureComptableRejetManager = EcritureComptableRejetManager;
            ecritureComptableCumulManager = EcritureComptableCumulManager;
            operationDiverseManager = OperationDiverseManager;
            referentielFixeManager = ReferentielFixeManager;
            natureManager = NatureManager;
            valorisationManager = ValorisationManager;
            commandeManager = CommandeManager;
            receptionManager = ReceptionManager;
            facturationManager = FacturationManager;
            featureFlippingManager = FeatureFlippingManager;
            ecritureComptableMapper = new EcritureComptableMapper();
            operationDiverseMapper = new OperationDiverseMapper();
            ecritureComptableConverter = new EcritureComptableConverter();
        }

        /// <summary>
        /// Gere l'import des ecriture comptables
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="dateComptable">startDate</param>
        /// <param name="ecritureComptablesDtos">ecritureComptablesDtos</param>
        /// <param name="codeEtablissement">code de l'établissement comptable</param>
        /// <returns>Nombre d'ecriture comptables qui sont insérées dans la base (nouvelles)</returns>
        public async Task<int> ManageImportEcritureComptablesAsync(int? societeId, DateTime dateComptable, IEnumerable<EcritureComptableDto> ecritureComptablesDtos, string codeEtablissement)
        {
            List<EcritureComptableEnt> ecritureComptablesToInsert = new List<EcritureComptableEnt>();
            if (societeId.HasValue)
            {
                // EXECUTION DES REQUETTES 
                // 1- recherche des ci de la societe => dico ciid<>ciCode
                Dictionary<int, string> dicoCiIdCiCode = GetListCi(societeId, codeEtablissement);
                List<int> cisOfSociete = dicoCiIdCiCode.Select(d => d.Key).ToList();

                // 2 - recuperation ecriture comptable de l'exercice de la societe
                // pour savoir si l'ecriture existe déjà en base et donc ne pas encore l'inserérer dans FRED.
                MonthLimits limitesDeImportEcritureComptable = dateComptable.GetLimitsOfMonth();
                limitesDeImportEcritureComptable.StartDate = limitesDeImportEcritureComptable.StartDate.AddDays(-1);
                List<string> numeroPiecesOfExistingEcritureComptableForExercice = (await ecritureComptableManager.GetListOfNumeroPiecesAsync(cisOfSociete, limitesDeImportEcritureComptable).ConfigureAwait(false)).Select(x => x.NumeroPiece).ToList();
                List<JournalEnt> journauxOfSociete = ecritureComptableImportHelper.GetJournauxComptables(societeId.Value).ToList();
                List<FamilleOperationDiverseEnt> famillesODOfSociete = ecritureComptableImportHelper.GetODFamilies(societeId.Value).ToList();
                List<NatureEnt> naturesOfSociete = ecritureComptableImportHelper.GetNatures(societeId.Value).ToList();

                ecritureComptablesDtos = ecritureComptablesDtos.Where(q => dicoCiIdCiCode.ContainsValue(q.AnaelCodeCi)).ToList();
                List<EcritureComptableDto> ecritureComptablesExistantes = EcritureComptableAlreadyExist(ecritureComptablesDtos, numeroPiecesOfExistingEcritureComptableForExercice);
                ecritureComptablesDtos = ecritureComptablesDtos.Where(ecriture => !ecritureComptablesExistantes.Contains(ecriture));
                List<EcritureComptableEnt> ecritureComptableToInsert = ManageImportEcritureComptable(ecritureComptablesDtos, dicoCiIdCiCode, journauxOfSociete, famillesODOfSociete, naturesOfSociete);

                ecritureComptablesToInsert.AddRange(ecritureComptableToInsert);
                // D - Gestion des cumuls et Sauvegade
                ecritureComptablesToInsert = await SaveLinesAsync(ecritureComptablesToInsert).ConfigureAwait(false);
            }
            return ecritureComptablesToInsert.Count;
        }

        public async Task<int> ManageImportEcritureComptablesAsync(int? societeId, DateTime dateComptableDebut, DateTime dateComptableFin, IEnumerable<EcritureComptableDto> ecritureComptablesDtos, string codeEtablissement)
        {
            if (!societeId.HasValue || !ecritureComptablesDtos.Any())
                return 0;

            // EXECUTION DES REQUETTES 
            // 1- recherche des ci de la societe => dico ciid<>ciCode
            Dictionary<int, string> dicoCiIdCiCode = GetListCi(societeId, codeEtablissement);

            List<int> cisOfSociete = dicoCiIdCiCode.Select(d => d.Key).ToList();
            List<string> numeroPiecesOfExistingEcritureComptableForExercice = await GetExistingPartNumber(dateComptableDebut, dateComptableFin, cisOfSociete).ConfigureAwait(false);

            ecritureComptablesDtos = GetEcritureComptables(ecritureComptablesDtos, dicoCiIdCiCode);
            List<EcritureComptableDto> ecritureComptablesExistantes = EcritureComptableAlreadyExist(ecritureComptablesDtos, numeroPiecesOfExistingEcritureComptableForExercice).Distinct().ToList();
            ecritureComptablesDtos = ecritureComptablesDtos.Where(ecriture => !ecritureComptablesExistantes.Contains(ecriture));

            List<JournalEnt> journauxOfSociete = ecritureComptableImportHelper.GetJournauxComptables(societeId.Value).ToList();
            List<FamilleOperationDiverseEnt> famillesODOfSociete = ecritureComptableImportHelper.GetODFamilies(societeId.Value).ToList();
            List<NatureEnt> naturesOfSociete = ecritureComptableImportHelper.GetNatures(societeId.Value).ToList();
            List<EcritureComptableEnt> ecritureComptableToInsert = ManageImportEcritureComptable(ecritureComptablesDtos, dicoCiIdCiCode, journauxOfSociete, famillesODOfSociete, naturesOfSociete);

            List<EcritureComptableEnt> ecritureComptablesToInsert = await SaveLinesAsync(ecritureComptableToInsert).ConfigureAwait(false);

            return ecritureComptablesToInsert.Count;
        }

        private static IEnumerable<EcritureComptableDto> GetEcritureComptables(IEnumerable<EcritureComptableDto> ecritureComptablesDtos, Dictionary<int, string> dicoCiIdCiCode)
        {
            return ecritureComptablesDtos.Where(q => dicoCiIdCiCode.ContainsValue(q.AnaelCodeCi)).ToList();
        }

        private async Task<List<string>> GetExistingPartNumber(DateTime dateComptableDebut, DateTime dateComptableFin, List<int> cisOfSociete)
        {
            MonthLimits limitesDeImportEcritureComptableDebut = dateComptableDebut.GetLimitsOfMonth();
            MonthLimits limitesDeImportEcritureComptableFin = dateComptableFin.GetLimitsOfMonth();
            MonthLimits limitesDeImportEcritureComptableRange = new MonthLimits { StartDate = limitesDeImportEcritureComptableDebut.StartDate.AddDays(-1), EndDate = limitesDeImportEcritureComptableFin.EndDate };
            return (await ecritureComptableManager.GetListOfNumeroPiecesAsync(cisOfSociete, limitesDeImportEcritureComptableRange).ConfigureAwait(false)).Select(x => x.NumeroPiece).ToList();
        }

        private Dictionary<int, string> GetListCi(int? societeId, string codeEtablissement)
        {
            return !string.IsNullOrEmpty(codeEtablissement) ? GetCiListForEtablissement(codeEtablissement, societeId.Value) : GetCiList(societeId.Value);
        }

        private async Task<List<EcritureComptableEnt>> SaveLinesAsync(List<EcritureComptableEnt> ecritureComptablesToInsert)
        {
            List<EcritureComptableEnt> ecritureComptableEnts = new List<EcritureComptableEnt>();

            //Ajout de toutes les lignes qui ne sont pas en cumul
            ecritureComptablesToInsert.Where(q => !q.FamilleOperationDiverse.IsAccrued).ToList().ForEach(x => ecritureComptableEnts.Add(
              new EcritureComptableEnt
              {
                  DateCreation = x.DateCreation,
                  DateComptable = x.DateComptable,
                  Libelle = x.Libelle,
                  CiId = x.CiId,
                  Montant = x.Montant,
                  NumeroPiece = x.NumeroPiece,
                  DeviseId = x.DeviseId,
                  JournalId = x.JournalId,
                  CommandeId = x.CommandeId,
                  FamilleOperationDiverseId = x.FamilleOperationDiverseId,
                  NatureId = x.NatureId,
                  Quantite = x.Quantite,
                  CodeRef = x.CodeRef,
                  NumeroFactureSAP = x.NumeroFactureSAP
              }));

            //Récupération des familles d'OD et des lignes cumulées qui y correspondent
            if (ecritureComptablesToInsert.Where(q => q.FamilleOperationDiverse.IsAccrued).GroupBy(x => x.FamilleOperationDiverseId).Any())
            {
                foreach (int odFamilly in ecritureComptablesToInsert.Where(q => q.FamilleOperationDiverse.IsAccrued).GroupBy(x => x.FamilleOperationDiverseId).Select(q => q.Key))
                {
                    List<EcritureComptableEnt> ecritureComptablesCumulGroup = ecritureComptablesToInsert.Where(q => q.FamilleOperationDiverseId == odFamilly).ToList();

                    //Vérification de la présence des lignes de cumul, si elle existent déjà en base, elles sont retirées de la liste
                    List<EcritureComptableEnt> existingAccountingEntryWithStackedLines = await GetCumulativeAccountingEntryLinesAsync(ecritureComptablesCumulGroup).ConfigureAwait(false);
                    existingAccountingEntryWithStackedLines.ForEach(x => ecritureComptablesCumulGroup.Remove(x));

                    //Ajout des lignes
                    ecritureComptableEnts.AddRange(AddLines(ecritureComptablesToInsert, odFamilly, ecritureComptablesCumulGroup));
                }
            }

            await ProcessStackedLinesAsync(ecritureComptableEnts).ConfigureAwait(false);

            //Etape 2 : Bulk Insert des autres lignes (celles qui ne sont pas en entête)
            ecritureComptableManager.InsertListByTransaction(ecritureComptableEnts.Where(q => q.EcritureComptableCumul == null));


            return ecritureComptableEnts;
        }

        private static IEnumerable<EcritureComptableEnt> AddLines(List<EcritureComptableEnt> ecritureComptablesToInsert, int odFamilly, List<EcritureComptableEnt> ecritureComptablesCumulGroup)
        {
            return ecritureComptablesToInsert.Where(q => q.FamilleOperationDiverse.IsAccrued && q.FamilleOperationDiverse.FamilleOperationDiverseId == odFamilly).GroupBy(x => new { x.FamilleOperationDiverseId, x.DateComptable.Value.GetLimitsOfMonth().EndDate.Date, x.CiId }).Select(x => new EcritureComptableEnt
            {
                DateCreation = DateTime.UtcNow,
                DateComptable = x.First().DateComptable.Value.GetLimitsOfMonth().EndDate.Date,
                Libelle = string.Concat(values: x.First().FamilleOperationDiverse.Libelle + " " + x.First().DateComptable.Value.ToString("MMMM", System.Globalization.CultureInfo.CreateSpecificCulture("fr")).ToUpperFirstLetter() + " " + x.First().DateComptable.Value.Year), //Il va falloir gérér le multilangue
                CiId = x.First().CiId,
                Montant = x.Sum(m => m.Montant),
                JournalId = x.First().JournalId,
                DeviseId = x.First().DeviseId, //La devise sera toujours de l'Euro
                FamilleOperationDiverseId = odFamilly,
                NatureId = x.First().NatureId,
                EcritureComptableCumul = ecritureComptablesCumulGroup.Where(q => q.DateComptable.Value.GetLimitsOfMonth().EndDate.Date == x.Key.Date.GetLimitsOfMonth().EndDate.Date && q.CiId == x.Key.CiId).Select(e => new EcritureComptableCumulEnt
                {
                    CiId = e.CiId,
                    DateComptable = e.DateComptable,
                    DateCreation = e.DateCreation,
                    EcritureComptableId = e.EcritureComptableId,
                    Montant = e.Montant,
                    NumeroPiece = e.NumeroPiece
                }).ToList()
            });
        }

        /// <summary>
        /// Traite les lignes cumulées
        /// </summary>
        /// <param name="ecritureComptableEnts">Liste écritures comptable</param>
        private async Task ProcessStackedLinesAsync(List<EcritureComptableEnt> ecritureComptableEnts)
        {
            List<EcritureComptableEnt> listEcritureComptableStaked = ecritureComptableEnts.Where(q => q.EcritureComptableCumul != null).ToList();

            IReadOnlyList<EcritureComptableEnt> headEcritureComptables = await CheckHeadingAccruedLineAsync(listEcritureComptableStaked).ConfigureAwait(false);

            var joinedList = listEcritureComptableStaked.Join(
                    headEcritureComptables,
                    ecritureComptableStaked => new { ecritureComptableStaked.CiId, ecritureComptableStaked.Libelle },
                    headEcritureComptable => new { headEcritureComptable.CiId, headEcritureComptable.Libelle },
                    (ecritureComptableStaked, headEcritureComptable) => new { ecritureComptableStaked, headEcritureComptable }
                ).ToList();

            foreach (var item in joinedList)
            {
                EcritureComptableEnt ecritureComptableStacked = item.ecritureComptableStaked;
                EcritureComptableEnt headEcritureComptable = item.headEcritureComptable;
                int headEcritureComptableId = headEcritureComptable.EcritureComptableId;

                ecritureComptableStacked.EcritureComptableId = headEcritureComptableId;
                ecritureComptableStacked.EcritureComptableCumul.ForEach(x => x.EcritureComptableId = headEcritureComptableId);

                if (ecritureComptableStacked.Montant == headEcritureComptable.Montant)
                    continue;

                if (ecritureComptableStacked.EcritureComptableCumul.Count > 0)
                {
                    ecritureComptableCumulManager.InsertListByTransaction(ecritureComptableStacked.EcritureComptableCumul);
                }

                await ecritureComptableManager.UpdateMontantEcritureComptableAsync(headEcritureComptable).ConfigureAwait(false);
            }

            List<EcritureComptableEnt> toCreate = listEcritureComptableStaked.Where(q => !joinedList.Select(j => j.ecritureComptableStaked).Contains(q)).ToList();
            foreach (EcritureComptableEnt item in toCreate)
            {
                ecritureComptableManager.InsertByTransaction(item);
            }
        }

        private async Task<IReadOnlyList<EcritureComptableEnt>> CheckHeadingAccruedLineAsync(List<EcritureComptableEnt> ecritureComptable)
        {
            return await ecritureComptableManager.GetByCiIdAndLabelAsync(ecritureComptable).ConfigureAwait(false);
        }

        private async Task<List<EcritureComptableEnt>> GetCumulativeAccountingEntryLinesAsync(List<EcritureComptableEnt> ecrituresComptables)
        {
            List<EcritureComptableEnt> listEcritureComptablesExist = new List<EcritureComptableEnt>();
            List<EcritureComptableCumulEnt> ecritureComptableCumuls = new List<EcritureComptableCumulEnt>();

            foreach (EcritureComptableEnt ecritureComptable in ecrituresComptables)
            {
                EcritureComptableCumulEnt ecritureComptableCumulEnt = new EcritureComptableCumulEnt { CiId = ecritureComptable.CiId, DateComptable = ecritureComptable.DateComptable, NumeroPiece = ecritureComptable.NumeroPiece };
                ecritureComptableCumuls.Add(ecritureComptableCumulEnt);
            }

            IReadOnlyList<EcritureComptableCumulEnt> ecritureComptableCumulEnts = await ecritureComptableCumulManager.GetEcritureComptableCumulByCiIdAndPartNumberAsync(ecritureComptableCumuls).ConfigureAwait(false);
            listEcritureComptablesExist.AddRange(ecrituresComptables.Join(ecritureComptableCumulEnts, ec => new { ec.CiId, ec.NumeroPiece }, ecc => new { ecc.CiId, ecc.NumeroPiece }, (ec, ecc) => ec).ToList());

            return listEcritureComptablesExist;
        }

        /// <summary>
        /// Recupere la liste des ci pour une societe
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>Dictionnalire ciID - codeCi</returns>
        public Dictionary<int, string> GetCiList(int societeId)
        {
            SocieteEnt societe = reposociete.Query().Include(s => s.Organisation).Filter(s => s.SocieteId == societeId).Get().FirstOrDefault();
            int typeOrgaCiId = organisationManager.GetTypeOrganisationIdByCode(Constantes.OrganisationType.CodeCi);
            IEnumerable<int> orgaIdList = organisationManager.GetOrganisationsAvailable(types: new List<int> { typeOrgaCiId }, organisationIdPere: societe.Organisation.OrganisationId).Select(x => x.OrganisationId);
            return repoCi.Query()
                .Include(ci => ci.Organisation)
                // US 7655 : Ne prendre que les CI qui sont des chantiers FRED
                .Filter(ci => orgaIdList.Contains(ci.Organisation.OrganisationId))
                .Filter(ci => ci.ChantierFRED)
                .Get()
                .ToDictionary(ci => ci.CiId, ci => ci.Code);
        }

        /// <summary>
        /// Recupere la liste des ci pour une societe
        /// </summary>
        /// <param name="societeIds">Liste d'identifiant des sociétés</param>
        /// <returns>Dictionnalire ciID - codeCi</returns>
        public Dictionary<int, string> GetCiList(List<int> societeIds)
        {
            SocieteEnt societe = reposociete.Query().Include(s => s.Organisation).Filter(s => societeIds.Contains(s.SocieteId)).Get().FirstOrDefault();
            int typeOrgaCiId = organisationManager.GetTypeOrganisationIdByCode(Constantes.OrganisationType.CodeCi);
            IEnumerable<int> orgaIdList = organisationManager.GetOrganisationsAvailable(types: new List<int> { typeOrgaCiId }, organisationIdPere: societe.Organisation.OrganisationId).Select(x => x.OrganisationId);
            return repoCi.Query()
                .Include(ci => ci.Organisation)
                // US 7655 : Ne prendre que les CI qui sont des chantiers FRED
                .Filter(ci => orgaIdList.Contains(ci.Organisation.OrganisationId))
                .Filter(ci => ci.ChantierFRED)
                .Get()
                .ToDictionary(ci => ci.CiId, ci => ci.Code);
        }

        /// <summary>
        /// Recupere la liste des ci pour une societe
        /// </summary>
        /// <param name="codeEtablissement">code de l'établissement comptable</param>
        /// <param name="societeId">Identifiant de la societe</param>
        /// <returns>Dictionnalire ciID - codeCi</returns>
        public Dictionary<int, string> GetCiListForEtablissement(string codeEtablissement, int societeId)
        {
            EtablissementComptableEnt etablissement = repoEtablissement.Query().Include(s => s.Organisation).Filter(q => q.Code == codeEtablissement && q.SocieteId == societeId).Get().FirstOrDefault();
            int typeOrgaCiId = organisationManager.GetTypeOrganisationIdByCode(Constantes.OrganisationType.CodeCi);
            IEnumerable<int> orgaIdList = organisationManager.GetOrganisationsAvailable(types: new List<int> { typeOrgaCiId }, organisationIdPere: etablissement.Organisation.OrganisationId).Select(x => x.OrganisationId);
            return repoCi.Query()
                .Include(ci => ci.Organisation)
                .Filter(ci => orgaIdList.Contains(ci.Organisation.OrganisationId))
                .Filter(ci => ci.ChantierFRED)
                .Get()
                .ToDictionary(ci => ci.CiId, ci => ci.Code);
        }

        private List<EcritureComptableEnt> ManageImportEcritureComptable(IEnumerable<EcritureComptableDto> ecritureComptableDtos, Dictionary<int, string> dicoCiIdCiCode, IEnumerable<JournalEnt> journauxOfSociete, IEnumerable<FamilleOperationDiverseEnt> famillesODOfSociete, IEnumerable<NatureEnt> naturesOfSociete)
        {
            List<EcritureComptableEnt> ecritureComptables = ecritureComptableMapper.MapEcritureComptableDtoWithNewEntity(ecritureComptableDtos);

            List<Result<EcritureComptableRetreiveResult>> ecritureComptableRetreiveResults = new List<Result<EcritureComptableRetreiveResult>>();

            TreatPartNumber(ecritureComptableDtos, ecritureComptableRetreiveResults);

            TreatCi(ecritureComptableDtos, dicoCiIdCiCode, ecritureComptableRetreiveResults);

            TreatCodeNature(ecritureComptableDtos, naturesOfSociete, ecritureComptableRetreiveResults);

            TreatJounauxComptables(ecritureComptableDtos, journauxOfSociete, ecritureComptableRetreiveResults);

            if (featureFlippingManager.IsActivated(EnumFeatureFlipping.ActivationUS13085_6000))
            {
                TreatFamilleOperationDiverses(ecritureComptableDtos, journauxOfSociete, naturesOfSociete, famillesODOfSociete, ecritureComptableRetreiveResults);
            }
            else
            {
                TreatFamilleOperationDiverses(ecritureComptableDtos, journauxOfSociete, famillesODOfSociete, ecritureComptableRetreiveResults);
            }
            return GetEcritureComptableSucced(ecritureComptables, ecritureComptableRetreiveResults);
        }

        private List<EcritureComptableEnt> GetEcritureComptableSucced(List<EcritureComptableEnt> ecritureComptables, List<Result<EcritureComptableRetreiveResult>> ecritureComptableRetreiveResults)
        {
            int? euroDevise = deviseManager.GetDeviseIdByCode("EUR");
            if (!euroDevise.HasValue)
                return ecritureComptables;

            var successedJoinEcritureComptableRetreiveResult = (
                from ecritureComptableRetreiveResult in ecritureComptableRetreiveResults.Where(q => q.Success)
                from ecritureComptable in ecritureComptables.Where(x => x.NumeroPiece == ecritureComptableRetreiveResult.Value.NumeroPiece && x.Montant == ecritureComptableRetreiveResult.Value.Montant).DefaultIfEmpty()
                select new { ecritureComptable, ecritureComptableRetreiveResult }
            ).ToList();

            foreach (var item in successedJoinEcritureComptableRetreiveResult)
            {
                AssignValues(euroDevise.Value, item.ecritureComptableRetreiveResult, item.ecritureComptable);
            }

            return successedJoinEcritureComptableRetreiveResult.Select(item => item.ecritureComptable).ToList();
        }

        private static void AssignValues(int euroDevise, Result<EcritureComptableRetreiveResult> item, EcritureComptableEnt ecriture)
        {
            ecriture.CiId = item.Value.CiId;
            ecriture.DeviseId = euroDevise;
            ecriture.Journal = item.Value.Journal;
            ecriture.JournalId = item.Value.Journal.JournalId;
            ecriture.FamilleOperationDiverse = item.Value.FamilleOperationDiverse;
            ecriture.FamilleOperationDiverseId = item.Value.FamilleOperationDiverse.FamilleOperationDiverseId;
            ecriture.Nature = item.Value.Nature;
            ecriture.NatureId = item.Value.Nature.NatureId;
        }

        private void TreatFamilleOperationDiverses(IEnumerable<EcritureComptableDto> ecritureComptableDtos, IEnumerable<JournalEnt> journauxOfSociete, IEnumerable<NatureEnt> naturesOfSociete, IEnumerable<FamilleOperationDiverseEnt> famillesODOfSociete, List<Result<EcritureComptableRetreiveResult>> ecritureComptableRetreiveResults)
        {
            List<Result<EcritureComptableRetreiveResult>> retrieveODFamilies = ecritureComptableImportHelper.RetrieveODFamilies(journauxOfSociete, naturesOfSociete, ecritureComptableDtos, famillesODOfSociete);
            UpdateRange(ecritureComptableRetreiveResults, retrieveODFamilies);
            ecritureComptableRejetManager.TreatRejet(retrieveODFamilies.Where(q => !q.Success));
        }

        private void TreatFamilleOperationDiverses(IEnumerable<EcritureComptableDto> ecritureComptableDtos, IEnumerable<JournalEnt> journauxOfSociete, IEnumerable<FamilleOperationDiverseEnt> famillesODOfSociete, List<Result<EcritureComptableRetreiveResult>> ecritureComptableRetreiveResults)
        {
            List<Result<EcritureComptableRetreiveResult>> retrieveODFamilies = ecritureComptableImportHelper.RetrieveODFamilies(journauxOfSociete, ecritureComptableDtos, famillesODOfSociete);
            UpdateRange(ecritureComptableRetreiveResults, retrieveODFamilies);
            ecritureComptableRejetManager.TreatRejet(retrieveODFamilies.Where(q => !q.Success));
        }

        private void TreatJounauxComptables(IEnumerable<EcritureComptableDto> ecritureComptableDtos, IEnumerable<JournalEnt> journauxOfSociete, List<Result<EcritureComptableRetreiveResult>> ecritureComptableRetreiveResults)
        {
            List<Result<EcritureComptableRetreiveResult>> retrieveJournalResults = ecritureComptableImportHelper.RetrieveJournaux(ecritureComptableDtos, journauxOfSociete);
            UpdateRange(ecritureComptableRetreiveResults, retrieveJournalResults);
            ecritureComptableRejetManager.TreatRejet(retrieveJournalResults.Where(q => !q.Success));
        }

        private void TreatCodeNature(IEnumerable<EcritureComptableDto> ecritureComptableDtos, IEnumerable<NatureEnt> naturesOfSociete, List<Result<EcritureComptableRetreiveResult>> ecritureComptableRetreiveResults)
        {
            List<Result<EcritureComptableRetreiveResult>> retrieveCodeNatureResults = ecritureComptableImportHelper.RetrieveNatures(ecritureComptableDtos, naturesOfSociete);
            UpdateRange(ecritureComptableRetreiveResults, retrieveCodeNatureResults);
            ecritureComptableRejetManager.TreatRejet(retrieveCodeNatureResults.Where(q => !q.Success));
        }

        private void TreatCi(IEnumerable<EcritureComptableDto> ecritureComptableDtos, Dictionary<int, string> dicoCiIdCiCode, List<Result<EcritureComptableRetreiveResult>> ecritureComptableRetreiveResults)
        {
            List<Result<EcritureComptableRetreiveResult>> retrieveCiResults = ecritureComptableImportHelper.RetrieveCIs(ecritureComptableDtos, dicoCiIdCiCode);
            UpdateRange(ecritureComptableRetreiveResults, retrieveCiResults);
            ecritureComptableRejetManager.TreatRejet(retrieveCiResults.Where(q => !q.Success));
        }

        private void TreatPartNumber(IEnumerable<EcritureComptableDto> ecritureComptableDtos, List<Result<EcritureComptableRetreiveResult>> ecritureComptableRetreiveResults)
        {
            List<Result<EcritureComptableRetreiveResult>> testNumeroPieceResult = ecritureComptableImportHelper.TestNumeroPieceResult(ecritureComptableDtos);
            ecritureComptableRetreiveResults.AddRange(testNumeroPieceResult);
            ecritureComptableRejetManager.TreatRejet(testNumeroPieceResult.Where(q => !q.Success));
        }

        private void UpdateRange(List<Result<EcritureComptableRetreiveResult>> results, List<Result<EcritureComptableRetreiveResult>> retrieveResults)
        {
            var resultRetreiveResults = results.Join(retrieveResults, result => result.Value.NumeroPiece, retreiveResult => retreiveResult.Value.NumeroPiece, (result, retreiveResult) => new { result, retreiveResult }).ToList();
            foreach (var item in resultRetreiveResults)
            {
                if (item.retreiveResult.Success)
                {
                    AssignNewValues(item.result, item.retreiveResult);
                }
                else if (!item.retreiveResult.Success)
                {
                    item.result.Success = item.retreiveResult.Success;
                }
            }
        }

        private static void AssignNewValues(Result<EcritureComptableRetreiveResult> result, Result<EcritureComptableRetreiveResult> retreiveResult)
        {
            result.Value.NumeroPiece = result.Value.NumeroPiece;
            result.Value.CiId = result.Value.CiId == 0 ? retreiveResult.Value.CiId : result.Value.CiId;
            result.Value.FamilleOperationDiverse = result.Value.FamilleOperationDiverse ?? retreiveResult.Value.FamilleOperationDiverse;
            result.Value.Journal = result.Value.Journal ?? retreiveResult.Value.Journal;
            result.Value.Nature = result.Value.Nature ?? retreiveResult.Value.Nature;
        }

        private List<Result<EcritureComptableEnt>> ManageImportEcritureComptable(List<int> societeIds, Dictionary<int, string> cisOfSociete, List<EcritureComptableFtpDto> ecritureComptablesInsert, List<FamilleOperationDiverseEnt> famillesODOfSociete, List<CommandeEcritureComptableOdModel> commandes)
        {
            List<Result<EcritureComptableEnt>> resultsEcritureComptableEnts = new List<Result<EcritureComptableEnt>>();
            List<EcritureComptableMappingModel> cis = ecritureComptableImportHelper.RetrieveCIs(cisOfSociete, ecritureComptablesInsert);
            List<EcritureComptableMappingModel> devises = ecritureComptableImportHelper.RetrieveDevises(ecritureComptablesInsert.Select(x => x.CodeDevise).ToList(), ecritureComptablesInsert);
            List<EcritureComptableMappingModel> natures = ecritureComptableImportHelper.RetrieveNatures(ecritureComptablesInsert.Select(x => x.NatureAnalytique).ToList(), societeIds, ecritureComptablesInsert);
            List<EcritureComptableMappingModel> famillesOD = ecritureComptableImportHelper.RetrieveODFamilies(natures.Where(n => n.Success).Select(n => n.Nature).ToList(), ecritureComptablesInsert, famillesODOfSociete);
            List<EcritureComptableMappingModel> unites = ecritureComptableImportHelper.RetrieveUnites(ecritureComptablesInsert.Select(x => x.Unite).ToList(), ecritureComptablesInsert, famillesOD);
            List<EcritureComptableMappingModel> ecritureComptableMappingModels = cis.Join(devises, ci => ci.NumeroPiece, devise => devise.NumeroPiece, (ci, devise) => new EcritureComptableMappingModel
            {
                NumeroCommande = ci.NumeroCommande,
                NumeroPiece = ci.NumeroPiece,
                CiId = ci.CiId,
                Devise = devise.Devise,
                DeviseCode = devise.DeviseCode,
                DeviseId = devise.DeviseId,
                Erreurs = ci.Erreurs.Concat(devise.Erreurs).ToList(),
            })
            .Join(natures, ciDevise => ciDevise.NumeroPiece, nature => nature.NumeroPiece, (ciDevise, nature) => new EcritureComptableMappingModel
            {
                NumeroCommande = ciDevise.NumeroCommande,
                NumeroPiece = ciDevise.NumeroPiece,
                CiId = ciDevise.CiId,
                Devise = ciDevise.Devise,
                DeviseCode = ciDevise.DeviseCode,
                DeviseId = ciDevise.DeviseId,
                Erreurs = ciDevise.Erreurs.Concat(nature.Erreurs).ToList(),
                Nature = nature.Nature,
                NatureCode = nature.NatureCode,
                NatureId = nature.NatureId,
            })
            .Join(famillesOD, ciDeviseNature => ciDeviseNature.NumeroPiece, famille => famille.NumeroPiece, (ciDeviseNature, famille) => new EcritureComptableMappingModel
            {
                NumeroCommande = ciDeviseNature.NumeroCommande,
                NumeroPiece = ciDeviseNature.NumeroPiece,
                CiId = ciDeviseNature.CiId,
                Devise = ciDeviseNature.Devise,
                DeviseCode = ciDeviseNature.DeviseCode,
                DeviseId = ciDeviseNature.DeviseId,
                Erreurs = ciDeviseNature.Erreurs.Concat(famille.Erreurs).ToList(),
                Nature = ciDeviseNature.Nature,
                NatureCode = ciDeviseNature.NatureCode,
                NatureId = ciDeviseNature.NatureId,
                FamilleOperationDiverse = famille.FamilleOperationDiverse,
                FamilleOperationDiverseId = famille.FamilleOperationDiverseId
            })
            .Join(unites, ciDeviseNatureFamille => ciDeviseNatureFamille.NumeroPiece, unite => unite.NumeroPiece, (ciDeviseNatureFamille, unite) => new EcritureComptableMappingModel
            {
                NumeroCommande = ciDeviseNatureFamille.NumeroCommande,
                NumeroPiece = ciDeviseNatureFamille.NumeroPiece,
                CiId = ciDeviseNatureFamille.CiId,
                Devise = ciDeviseNatureFamille.Devise,
                DeviseCode = ciDeviseNatureFamille.DeviseCode,
                DeviseId = ciDeviseNatureFamille.DeviseId,
                Erreurs = ciDeviseNatureFamille.Erreurs.Concat(unite.Erreurs).ToList(),
                Nature = ciDeviseNatureFamille.Nature,
                NatureCode = ciDeviseNatureFamille.NatureCode,
                NatureId = ciDeviseNatureFamille.NatureId,
                FamilleOperationDiverse = ciDeviseNatureFamille.FamilleOperationDiverse,
                FamilleOperationDiverseId = ciDeviseNatureFamille.FamilleOperationDiverseId,
                UniteId = unite.UniteId,
                UniteCode = unite.UniteCode
            })
            .ToList();

            foreach (EcritureComptableMappingModel ecritureComptableMappingModel in ecritureComptableMappingModels)
            {
                ecritureComptableMappingModel.Success = ecritureComptableMappingModel.Erreurs.Count == 0;
                ecritureComptableMappingModel.CommandeId = commandes.Find(q => q.NumeroCommande == ecritureComptableMappingModel.NumeroCommande)?.CommandeId;
            }

            List<EcritureComptableFayatTpRejetModel> rejets = new List<EcritureComptableFayatTpRejetModel>();

            foreach (EcritureComptableMappingModel ecritureComptableMappingModel in ecritureComptableMappingModels.Where(q => !q.Success))
            {
                EcritureComptableEnt ecritureComptable = ecritureComptableMapper.MapEcritureComptableDtoWithNewEntity(ecritureComptablesInsert.Find(q => q.NumeroPiece == ecritureComptableMappingModel.NumeroPiece));

                rejets.Add(new EcritureComptableFayatTpRejetModel
                {
                    CiID = ecritureComptableMappingModel.CiId,
                    DateRejet = DateTime.UtcNow,
                    NumeroPiece = ecritureComptableMappingModel.NumeroPiece,
                    RejetMessage = ecritureComptableMappingModel.Erreurs
                });
                resultsEcritureComptableEnts.Add(Result<EcritureComptableEnt>.CreateFailureWithData(ecritureComptableMappingModel.Erreurs, ecritureComptable));
            }

            ecritureComptableRejetManager.AddRejets(rejets);

            foreach (EcritureComptableMappingModel item in ecritureComptableMappingModels.Where(q => q.Success))
            {
                EcritureComptableEnt ecritureComptable = ecritureComptableMapper.MapEcritureComptableDtoWithNewEntity(ecritureComptablesInsert.Find(q => q.NumeroPiece == item.NumeroPiece));

                ecritureComptable.CiId = item.CiId;
                ecritureComptable.DeviseId = item.DeviseId;
                ecritureComptable.FamilleOperationDiverse = item.FamilleOperationDiverse;
                ecritureComptable.FamilleOperationDiverseId = item.FamilleOperationDiverseId;
                ecritureComptable.DateComptable = ecritureComptable.DateComptable.Value.Date;
                ecritureComptable.NatureId = item.NatureId;
                ecritureComptable.CommandeId = item.CommandeId;
                resultsEcritureComptableEnts.Add(Result<EcritureComptableEnt>.CreateSuccess(ecritureComptable));
            }
            return resultsEcritureComptableEnts;
        }


        private static List<EcritureComptableDto> EcritureComptableAlreadyExist(IEnumerable<EcritureComptableDto> ecritureComptables, IEnumerable<string> numeroPiecesOfEcritureComptableForExercice)
        {
            return (from ecritureComptableExiste in numeroPiecesOfEcritureComptableForExercice
                    join ecriture in ecritureComptables on ecritureComptableExiste equals ecriture.NumeroPiece
                    select ecriture).ToList();
        }

        /// <summary>
        /// Verifie que ecriture comptable n'existe pas en base en se basant sur le numéro de pièce, le montant et la quantité
        /// </summary>
        /// <param name="ecritureComptables">Ecriture Comptable</param>
        /// <param name="existingEcritureComptableForExercice">Liste d'écriture comptable déjà en BDD</param>
        /// <returns>true si l'écriture existe déjà</returns>
        private static List<EcritureComptableFtpDto> EcritureComptableAlreadyExist(List<EcritureComptableFtpDto> ecritureComptables, List<EcritureComptableEnt> existingEcritureComptableForExercice)
        {
            return (from ecritureComptableExiste in existingEcritureComptableForExercice
                    join ecriture in ecritureComptables on new { ecritureComptableExiste.Montant, ecritureComptableExiste.NumeroPiece, ecritureComptableExiste.Quantite } equals new { ecriture.Montant, ecriture.NumeroPiece, ecriture.Quantite }
                    select ecriture).ToList();
        }

        private List<EcritureComptableEnt> MapEcritureComptable(List<EcritureComptableFtpDto> ecritureComptables, Dictionary<int, string> cisOfSociete, List<EcritureComptableEnt> existingEcritureComptableForExercice, List<int> societeIds, List<FamilleOperationDiverseEnt> famillesODOfSociete, List<CommandeEcritureComptableOdModel> commandes)
        {
            List<EcritureComptableEnt> ecritureComptablesToInsert = new List<EcritureComptableEnt>();
            List<EcritureComptableFtpDto> ecritureComptablesExistantes = EcritureComptableAlreadyExist(ecritureComptables, existingEcritureComptableForExercice);

            List<Result<EcritureComptableEnt>> mappingResults = ManageImportEcritureComptable(societeIds, cisOfSociete, ecritureComptables, famillesODOfSociete, commandes);

            foreach (EcritureComptableFtpDto ecritureComptable in ecritureComptables.Where(ecriture => !ecritureComptablesExistantes.Contains(ecriture)))
            {
                Result<EcritureComptableEnt> mappingResult = mappingResults.Find(q => q.Value.NumeroPiece == ecritureComptable.NumeroPiece);
                if (mappingResult?.Success == true)
                {
                    ecritureComptable.CiId = mappingResult.Value.CiId;
                    ecritureComptable.TacheId = mappingResult.Value.FamilleOperationDiverse.TacheId;
                    ecritureComptable.DeviseId = mappingResult.Value.DeviseId;
                    ecritureComptable.NatureId = mappingResult.Value.NatureId.Value;
                    ecritureComptable.Quantite = mappingResult.Value.Quantite;
                    ecritureComptable.CodeRef = mappingResult.Value.CodeRef;
                    ecritureComptablesToInsert.Add(mappingResult.Value);
                }
                else
                {
                    ecritureComptable.ErrorMessages = mappingResult?.Errors;
                }
            }
            return ecritureComptablesToInsert;
        }

        /// <summary>
        /// Gere l'import des écritures comptables pour Fayat TP
        /// </summary>
        /// <param name="ecritureComptables">Liste de <see cref="EcritureComptableFtpDto"/></param>
        /// <param name="societeIds">Liste d'identifiant des sociétés</param>
        /// <returns>La liste des EcritureComptableFtpDto qui ont été inséré ou non</returns>
        public async Task<IList<EcritureComptableFtpDto>> ManageImportEcritureComptablesAsync(List<EcritureComptableFtpDto> ecritureComptables, List<int> societeIds)
        {
            List<EcritureComptableFtpDto> ecritureComptableFtpDtos = new List<EcritureComptableFtpDto>();
            ecritureComptables = ecritureComptables.GroupBy(x => x.NumeroPiece).Select(ecriture => ecriture.First()).ToList();

            Dictionary<int, string> dicoCiIdCiCode = GetCiList(societeIds);
            List<int> cisOfSociete = dicoCiIdCiCode.Select(d => d.Key).ToList();

            List<FamilleOperationDiverseEnt> famillesODOfSociete = ecritureComptableImportHelper.GetODFamilies(societeIds).ToList();
            MonthLimits limitesDeImportEcritureComptableRange = GetLimitsOfMonth(ecritureComptables);

            List<EcritureComptableFtpDto> ecritureComptableCommandes = GetEcritureComptableCommande(ecritureComptables);
            IReadOnlyList<CommandeEcritureComptableOdModel> commandes = GetCommandes(ecritureComptableImportHelper.RetrieveCIs(dicoCiIdCiCode, ecritureComptableCommandes));

            List<EcritureComptableEnt> existingEcritureComptableForExercice = (await ecritureComptableManager.GetByCiIdsAndPeriodAsync(cisOfSociete, limitesDeImportEcritureComptableRange).ConfigureAwait(false)).ToList();
            List<EcritureComptableEnt> ecritureComptablesToInsert = MapEcritureComptable(ecritureComptables, dicoCiIdCiCode, existingEcritureComptableForExercice, societeIds, famillesODOfSociete, commandes.ToList());
            ecritureComptableFtpDtos.AddRange(GetEcritureToTreat(ecritureComptables, existingEcritureComptableForExercice));

            // D - Gestion des cumuls et Sauvegade
            if (ecritureComptablesToInsert.Count > 0)
            {
                List<EcritureComptableFtpDto> pointages = GetPointages(ecritureComptables);
                IReadOnlyList<ValorisationEcritureComptableODModel> valorisations = GetValorisations(pointages);
                UpdateTypeOfPointage(pointages);

                IEnumerable<EcritureComptableFtpDto> unknowValorisation = GetUnknowValorisation(valorisations, pointages);
                IEnumerable<EcritureComptableFtpDto> invalidateValorisation = GetInvalideValorisation(valorisations, pointages);
                invalidateValorisation = invalidateValorisation.Concat(unknowValorisation);
                RemoveInvalideEcritureComptable(ecritureComptables, ecritureComptablesToInsert, pointages, invalidateValorisation);

                ecritureComptablesToInsert = await SaveLinesAsync(ecritureComptablesToInsert).ConfigureAwait(false);

                foreach (EcritureComptableFtpDto item in invalidateValorisation)
                {
                    ecritureComptableFtpDtos.Remove(item);
                }

                ecritureComptableFtpDtos.AddRange(invalidateValorisation);

                ecritureComptables = ecritureComptables.Where(q => ecritureComptablesToInsert.Select(x => x.NumeroPiece).Contains(q.NumeroPiece)).ToList();
                ecritureComptables.Join(ecritureComptablesToInsert, ec => ec.NumeroPiece, insert => insert.NumeroPiece, (ec, insert) => new { ec, insert }).ForEach(item => item.ec.EcritureComptableId = item.insert.EcritureComptableId);

                List<OperationDiverseCommandeFtpModel> operationDiversesCommande = ecritureComptableMapper.MapEcritureComptableWithCommande(ecritureComptableCommandes, commandes);
                operationDiversesCommande = await CalculMontantODAsync(operationDiversesCommande, commandes).ConfigureAwait(false);

                List<EcritureComptableEnt> ecritureComptableCancelled = GetEcritureComptablesCancelled(ecritureComptableCommandes, pointages, existingEcritureComptableForExercice);
                await operationDiverseManager.GenerateRevertedOperationDiverseAsync(ecritureComptableCancelled).ConfigureAwait(false);

                ecritureComptables = ecritureComptables.Where(q => !ecritureComptableCancelled.Select(x => x.NumeroPiece).Contains(q.NumeroPiece)).ToList();

                List<OperationDiverseCommandeFtpModel> fullListOperationDiverses = GetFullOperationDiverses(ecritureComptables, pointages, valorisations, operationDiversesCommande);

                GenerateOperationDiverse(ecritureComptables, societeIds, fullListOperationDiverses);

                return ecritureComptableFtpDtos;
            }

            return ecritureComptableFtpDtos;
        }

        private List<EcritureComptableFtpDto> GetEcritureToTreat(List<EcritureComptableFtpDto> ecritureComptables, List<EcritureComptableEnt> numeroPiecesOfExistingEcritureComptableForExercice)
        {
            return (from item in ecritureComptables
                    where !numeroPiecesOfExistingEcritureComptableForExercice.Select(ex => new { ex.CI.Code, ex.NumeroPiece }).ToList().Any(q => q.Code == item.CodeCi && q.NumeroPiece == item.NumeroPiece)
                    select item).ToList();
        }

        private void GenerateOperationDiverse(List<EcritureComptableFtpDto> ecritureComptables, List<int> societeIds, List<OperationDiverseCommandeFtpModel> fullListOperationDiverses)
        {
            List<OperationDiverseCommandeFtpModel> operationDiverses = new List<OperationDiverseCommandeFtpModel>();
            operationDiverses.AddRange(fullListOperationDiverses.Where(od => od.RessourceId != 0));
            List<OperationDiverseCommandeFtpModel> operationDiversesWithoutResssourceId = fullListOperationDiverses.Where(q => q.RessourceId == 0).ToList();

            List<RessourceEnt> ressources = referentielFixeManager.GetRessourceList(ecritureComptables.Where(q => q.RessourceCode != null).Select(x => x.RessourceCode).ToList(), societeIds).ToList();
            if (ressources.Count > 0 && operationDiversesWithoutResssourceId.Count > 0)
            {
                operationDiverses.AddRange(operationDiverseMapper.MapOperationDiverseWithRessource(operationDiversesWithoutResssourceId, ressources));
            }

            List<NatureEnt> natures = natureManager.GetNatures(ecritureComptables.Where(q => q.RessourceCode == null || string.IsNullOrEmpty(q.RessourceCode)).Select(x => x.NatureId).ToList(), societeIds).ToList();
            if (natures.Count > 0 && operationDiversesWithoutResssourceId.Count > 0)
            {
                operationDiverses.AddRange(operationDiverseMapper.MapOperationDiverseWithNature(operationDiversesWithoutResssourceId, natures));
            }

            operationDiverseManager.GenerateOperationDiverse(operationDiverses.Where(od => od.RessourceId != 0 && od.Montant != 0));
        }

        private void UpdateTypeOfPointage(IEnumerable<EcritureComptableFtpDto> pointages)
        {
            pointages.ForEach(pointage =>
            {
                if (pointage.FamilleOperationDiversesCode == "MO")
                {
                    pointage.IsPointagePersonnel = true;
                    pointage.IsPointageMateriel = false;
                }
                else if (pointage.FamilleOperationDiversesCode == "MIT")
                {
                    pointage.IsPointagePersonnel = false;
                    pointage.IsPointageMateriel = true;
                }
            });
        }

        private List<OperationDiverseCommandeFtpModel> GetFullOperationDiverses(List<EcritureComptableFtpDto> ecritureComptables, List<EcritureComptableFtpDto> pointages, IReadOnlyList<ValorisationEcritureComptableODModel> valorisations, List<OperationDiverseCommandeFtpModel> operationDiversesCommande)
        {
            List<EcritureComptableFtpDto> ecritureComptableWithSAPInvoice = ecritureComptables.Where(ec => ec.NumFactureSAP != null).ToList();

            List<OperationDiverseCommandeFtpModel> operationDiversesValorisation = ecritureComptableMapper.MapEcritureComptableWithValorisation(pointages, valorisations);
            List<OperationDiverseCommandeFtpModel> fullListOperationDiverses = operationDiversesCommande.Concat(operationDiversesValorisation).ToList();
            List<OperationDiverseCommandeFtpModel> depensesHorsFRED = GetDepensesHorsFred(ecritureComptables);
            fullListOperationDiverses = fullListOperationDiverses.Concat(depensesHorsFRED).ToList();

            if (ecritureComptableWithSAPInvoice.Count != 0)
            {
                fullListOperationDiverses = GetOdsWithoutEcritureComptableWithSAPInvoice(ecritureComptableWithSAPInvoice, fullListOperationDiverses);
            }

            return fullListOperationDiverses;
        }

        private List<OperationDiverseCommandeFtpModel> GetOdsWithoutEcritureComptableWithSAPInvoice(List<EcritureComptableFtpDto> ecritureComptableWithSAPInvoice, List<OperationDiverseCommandeFtpModel> fullListOperationDiverses)
        {
            List<string> numeroPieces = GetOdToRemove(ecritureComptableWithSAPInvoice).Select(o => o.NumeroPiece).ToList();
            return fullListOperationDiverses.Where(od => !numeroPieces.Contains(od.NumeroPiece)).ToList();
        }

        private List<OperationDiverseCommandeFtpModel> GetOdToRemove(List<EcritureComptableFtpDto> ecritureComptableWithSAPInvoice)
        {
            IReadOnlyList<FactureEcritureComptableModel> invoices = facturationManager.GetExistingNumeroFactureSap(ecritureComptableWithSAPInvoice.Select(ec => ec.NumFactureSAP).ToList());
            List<FactureEcritureComptableModel> filteredInvoices = invoices.Where(f => f.DepenseAchatReceptionId != null || f.DepenseAchatFactureEcartId != null || f.DepenseAchatFactureId != null || f.DepenseAchatFarId != null).ToList();
            List<EcritureComptableFtpDto> joinedInvoiceAndEntries = ecritureComptableWithSAPInvoice.Join(filteredInvoices, ec => ec.NumFactureSAP, invoice => invoice.NumeroFactureSAP, (ec, invoice) => new { ec, invoice }).Select(result => result.ec).ToList();
            List<OperationDiverseCommandeFtpModel> odToRemove = ecritureComptableConverter.Convert(joinedInvoiceAndEntries);
            return odToRemove;
        }

        private MonthLimits GetLimitsOfMonth(List<EcritureComptableFtpDto> ecritureComptables)
        {
            MonthLimits limitesDeImportEcritureComptableDebut = ecritureComptables.Min(q => q.DateComptable).GetLimitsOfMonth();
            MonthLimits limitesDeImportEcritureComptableFin = ecritureComptables.Max(q => q.DateComptable).GetLimitsOfMonth();
            return new MonthLimits { StartDate = limitesDeImportEcritureComptableDebut.StartDate, EndDate = limitesDeImportEcritureComptableFin.EndDate };
        }

        private static List<EcritureComptableFtpDto> GetPointages(List<EcritureComptableFtpDto> ecritureComptables)
        {
            return ecritureComptables.Where(q => q.NumeroCommande == null && q.RapportLigneId != null).ToList();
        }

        private static List<EcritureComptableFtpDto> GetEcritureComptableCommande(List<EcritureComptableFtpDto> ecritureComptables)
        {
            return ecritureComptables.Where(q => q.NumeroCommande != null && q.RapportLigneId == null).ToList();
        }

        private List<OperationDiverseCommandeFtpModel> GetDepensesHorsFred(List<EcritureComptableFtpDto> ecritureComptables)
        {
            List<EcritureComptableFtpDto> list = ecritureComptables.Where(q => q.NumeroCommande == null && q.RapportLigneId == null).ToList();
            List<OperationDiverseCommandeFtpModel> listConverted = ecritureComptableConverter.Convert(list);
            return listConverted;
        }

        private List<EcritureComptableEnt> GetEcritureComptablesCancelled(List<EcritureComptableFtpDto> ecritureComptableCommandes, List<EcritureComptableFtpDto> pointages, List<EcritureComptableEnt> numeroPiecesOfExistingEcritureComptableForExercice)
        {
            List<EcritureComptableEnt> ecritureComptableCancelled = new List<EcritureComptableEnt>();
            ecritureComptableCancelled.AddRange(GetInvalideEcritureComptableCommande(ecritureComptableCommandes, numeroPiecesOfExistingEcritureComptableForExercice));
            ecritureComptableCancelled.AddRange(GetInvalideEcritureComptablePointage(pointages, numeroPiecesOfExistingEcritureComptableForExercice));
            return ecritureComptableCancelled;
        }

        private static IEnumerable<EcritureComptableFtpDto> GetUnknowValorisation(IReadOnlyList<ValorisationEcritureComptableODModel> valorisations, IEnumerable<EcritureComptableFtpDto> pointages)
        {
            return (from pointage in pointages
                    join valorisation in valorisations on pointage.RapportLigneId equals valorisation.RapportLigneId
                    into results
                    from result in results.DefaultIfEmpty()
                    select new EcritureComptableFtpDto
                    {
                        NumeroPiece = pointage.NumeroPiece,
                        RapportLigneId = result?.RapportLigneId,
                        ErrorMessages = result == null ? new List<string> { string.Format("La ligne de rapport avec le code {0} est inconnue dans la base FRED", pointage.RapportLigneId) } : null
                    }).Where(q => q.RapportLigneId == null);
        }

        private async Task<List<OperationDiverseCommandeFtpModel>> CalculMontantODAsync(List<OperationDiverseCommandeFtpModel> operationDiverses, IReadOnlyList<CommandeEcritureComptableOdModel> commandes)
        {
            IReadOnlyList<ReceptionEcritureComptableOdModel> receptions = GetReceptions(commandes);
            IReadOnlyList<OperationDiverseEcritureComptableModel> operationDiverseEcritureComptables = await GetOperationDiversesAsync(commandes).ConfigureAwait(false);
            IReadOnlyList<EcritureComptableModel> ecritureComptableModels = await GetEcritureComptablesAsync(commandes).ConfigureAwait(false);

            foreach (OperationDiverseCommandeFtpModel item in operationDiverses)
            {
                item.MontantODCommande = ecritureComptableModels.Where(ec => ec.CommandeId == item.CommandeId && item.DateComptable.GetLimitsOfMonth().EndDate.Date == ec.DateComptable.Value.GetLimitsOfMonth().EndDate.Date).Sum(ec => ec.Montant)
                - receptions.Where(r => r.CommandeId == item.CommandeId && item.DateComptable.GetLimitsOfMonth().EndDate.Date == r.DateComptable.GetLimitsOfMonth().EndDate.Date).Sum(r => r.Total)
                - operationDiverseEcritureComptables.Where(od => od.CommandeId == item.CommandeId && item.DateComptable.GetLimitsOfMonth().EndDate.Date == od.DateComptable.GetLimitsOfMonth().EndDate.Date).Sum(od => od.Total);
            }

            return operationDiverses;
        }

        private static void RemoveInvalideEcritureComptable(List<EcritureComptableFtpDto> ecritureComptables, List<EcritureComptableEnt> ecritureComptablesToInsert, List<EcritureComptableFtpDto> pointages, IEnumerable<EcritureComptableFtpDto> invalidateValorisation)
        {
            var listInvalidateValorisationEcritureComptable = ecritureComptablesToInsert.Join(invalidateValorisation, insert => insert.NumeroPiece, invalide => invalide.NumeroPiece, (insert, invalide) => new { insert, invalide }).ToList();

            foreach (var invalideValorisationEcritureComptable in listInvalidateValorisationEcritureComptable)
            {
                ecritureComptablesToInsert.Remove(invalideValorisationEcritureComptable.insert);
                ecritureComptables.Remove(ecritureComptables.Where(ec => invalideValorisationEcritureComptable.invalide.NumeroPiece == ec.NumeroPiece).Select(ec => ec).FirstOrDefault());
                pointages.Remove(pointages.Where(ec => invalideValorisationEcritureComptable.invalide.NumeroPiece == ec.NumeroPiece).Select(ec => ec).FirstOrDefault());
            }
        }

        private static List<EcritureComptableEnt> GetInvalideEcritureComptablePointage(List<EcritureComptableFtpDto> pointages, List<EcritureComptableEnt> numeroPiecesOfExistingEcritureComptableForExercice)
        {
            List<EcritureComptableEnt> ecritureComptables = new List<EcritureComptableEnt>();
            var invertedPointages = (from pointage in pointages
                                     join existingEcriture in numeroPiecesOfExistingEcritureComptableForExercice on new { pointage.CiId, pointage.NumeroPiece } equals new { existingEcriture.CiId, existingEcriture.NumeroPiece }
                                     where existingEcriture.Montant == -pointage.Montant && pointage.Quantite == -existingEcriture.Quantite
                                     select new { existingEcriture, pointage }).ToList();

            foreach (var item in invertedPointages)
            {
                pointages.Remove(item.pointage);
                ecritureComptables.Add(item.existingEcriture);
            }

            return ecritureComptables;
        }

        private static List<EcritureComptableEnt> GetInvalideEcritureComptableCommande(List<EcritureComptableFtpDto> ecritureComptableCommandes, List<EcritureComptableEnt> numeroPiecesOfExistingEcritureComptableForExercice)
        {
            List<EcritureComptableEnt> ecritureComptables = new List<EcritureComptableEnt>();
            var invertedEcritureComptableCommande = (from ecritureComptableCommande in ecritureComptableCommandes
                                                     join existingEcriture in numeroPiecesOfExistingEcritureComptableForExercice on new { ecritureComptableCommande.CiId, ecritureComptableCommande.NumeroPiece } equals new { existingEcriture.CiId, existingEcriture.NumeroPiece }
                                                     where existingEcriture.Montant == -ecritureComptableCommande.Montant && ecritureComptableCommande.Quantite == -existingEcriture.Quantite
                                                     select new { existingEcriture, ecritureComptableCommande }).ToList();

            foreach (var item in invertedEcritureComptableCommande)
            {
                ecritureComptableCommandes.Remove(item.ecritureComptableCommande);
                ecritureComptables.Add(item.existingEcriture);
            }
            return ecritureComptables;
        }

        /// <summary>
        /// Retourne la liste des valorisation qui ont une unité différente
        /// </summary>
        /// <param name="valorisations">Liste de <see cref="ValorisationEcritureComptableODModel"/> </param>
        /// <param name="pointages">Liste de <see cref="EcritureComptableFtpDto"/></param>
        /// <returns>La liste des écritures en erreur</returns>
        private static IEnumerable<EcritureComptableFtpDto> GetInvalideValorisation(IReadOnlyList<ValorisationEcritureComptableODModel> valorisations, IEnumerable<EcritureComptableFtpDto> pointages)
        {
            List<EcritureComptableValorisationPointageFayatTpModel> valoAndPointage = JoinValorisationsAndPointages(valorisations, pointages);
            List<EcritureComptableValorisationPointageFayatTpModel> valoAndPointagePersonnel = GetValorisationsAndPointagesPersonnel(valoAndPointage);
            List<EcritureComptableValorisationPointageFayatTpModel> valoAndPointageMateriel = GetValorisationsAndPointagesMateriel(valoAndPointage);
            return VerifyUnit(valoAndPointagePersonnel.Concat(valoAndPointageMateriel));
        }

        private static List<EcritureComptableValorisationPointageFayatTpModel> JoinValorisationsAndPointages(IReadOnlyList<ValorisationEcritureComptableODModel> valorisations, IEnumerable<EcritureComptableFtpDto> pointages)
        {
            return pointages.Join(valorisations, pointage => pointage.RapportLigneId, valo => valo.RapportLigneId, (pointage, valo) => new EcritureComptableValorisationPointageFayatTpModel
            {
                Pointage = pointage,
                Valorisation = valo
            }).ToList();
        }

        private static List<EcritureComptableValorisationPointageFayatTpModel> GetValorisationsAndPointagesMateriel(List<EcritureComptableValorisationPointageFayatTpModel> valoAndPointage)
        {
            return valoAndPointage.Where(q => q.Pointage.IsPointageMateriel == true && q.Pointage.IsPointagePersonnel == false && q.Valorisation.MaterielId != null && q.Valorisation.PersonnelId == null).ToList();
        }

        private static List<EcritureComptableValorisationPointageFayatTpModel> GetValorisationsAndPointagesPersonnel(List<EcritureComptableValorisationPointageFayatTpModel> valoAndPointage)
        {
            return valoAndPointage.Where(q => q.Pointage.IsPointagePersonnel == true && q.Pointage.IsPointageMateriel == false && q.Valorisation.MaterielId == null && q.Valorisation.PersonnelId != null).ToList();
        }

        private static List<EcritureComptableFtpDto> VerifyUnit(IEnumerable<EcritureComptableValorisationPointageFayatTpModel> valoAndPointage)
        {
            List<EcritureComptableFtpDto> ecritureComptables = new List<EcritureComptableFtpDto>();
            valoAndPointage.ForEach(item =>
            {
                if (item.Pointage.ErrorMessages == null)
                {
                    item.Pointage.ErrorMessages = new List<string>();
                }

                if (item.Valorisation.Unite != item.Pointage.Unite)
                {
                    item.Pointage.ErrorMessages.Add(FeatureEcritureComptable.EcritureComptable_Erreur_UniteDifferente);
                }

                if (item.Pointage.ErrorMessages.Count > 0)
                {
                    ecritureComptables.Add(item.Pointage);
                }
            });
            return ecritureComptables;
        }

        private IReadOnlyList<CommandeEcritureComptableOdModel> GetCommandes(List<EcritureComptableMappingModel> commandes)
        {
            int ciId = commandes.Select(q => q.CiId).FirstOrDefault();
            return commandeManager.GetCommandeEcritureComptableOdModelByNumeros(commandes.Select(q => q.NumeroCommande).ToList(), ciId);
        }

        private IReadOnlyList<ValorisationEcritureComptableODModel> GetValorisations(IEnumerable<EcritureComptableFtpDto> valorisations)
        {
            List<ValorisationEcritureComptableODModel> valorisationList = new List<ValorisationEcritureComptableODModel>();

            foreach (EcritureComptableFtpDto valorisation in valorisations)
            {
                List<int> rapportLigneIds = new List<int> { valorisation.RapportLigneId.Value };
                List<ValorisationEcritureComptableODModel> valorisationsByFamille = valorisationManager.GetValorisationByRapportLigneIds(rapportLigneIds).ToList();

                if (valorisation.FamilleOperationDiversesCode == "MO")
                {
                    valorisationsByFamille = valorisationsByFamille.Where(v => v.PersonnelId != null && v.MaterielId == null).ToList();
                }
                else if (valorisation.FamilleOperationDiversesCode == "MIT")
                {
                    valorisationsByFamille = valorisationsByFamille.Where(v => v.PersonnelId == null && v.MaterielId != null).ToList();
                }

                if (!valorisationsByFamille.Any())
                {
                    if (valorisation.ErrorMessages == null)
                    {
                        valorisation.ErrorMessages = new List<string>();
                    }

                    valorisation.ErrorMessages.Add(FeatureEcritureComptable.EcritureComptable_Erreur_TypeFamilleTypeValoIncoherent);
                    continue;
                }
                valorisationList.AddRange(valorisationsByFamille);
            }

            return valorisationList;
        }

        private IReadOnlyList<ReceptionEcritureComptableOdModel> GetReceptions(IReadOnlyList<CommandeEcritureComptableOdModel> commandes)
        {
            List<ReceptionEcritureComptableOdModel> receptions = new List<ReceptionEcritureComptableOdModel>();
            foreach (CommandeEcritureComptableOdModel commande in commandes)
            {
                receptions.Add(new ReceptionEcritureComptableOdModel
                {
                    CommandeId = commande.CommandeId,
                    Total = receptionManager.GetMontant(commande.CommandeLigneId),
                    DateComptable = commande.DateComptable
                });
            }
            return receptions;
        }

        private async Task<IReadOnlyList<EcritureComptableModel>> GetEcritureComptablesAsync(IReadOnlyList<CommandeEcritureComptableOdModel> commandes)
        {
            return await ecritureComptableManager.GetByCommandeIdsAsync(commandes.Select(q => q.CommandeId).ToList()).ConfigureAwait(false);
        }

        private async Task<IReadOnlyList<OperationDiverseEcritureComptableModel>> GetOperationDiversesAsync(IReadOnlyList<CommandeEcritureComptableOdModel> commandes)
        {
            List<OperationDiverseEcritureComptableModel> operationDiverses = new List<OperationDiverseEcritureComptableModel>();
            IReadOnlyList<OperationDiverseEnt> operationDiverseEnts = await operationDiverseManager.GetOperationDiverseListAsync(commandes.Select(q => q.CommandeId).ToList()).ConfigureAwait(false);

            foreach (OperationDiverseEnt operationDiverse in operationDiverseEnts)
            {
                operationDiverses.Add(new OperationDiverseEcritureComptableModel
                {
                    CommandeId = operationDiverse.CommandeId,
                    Total = operationDiverse.Montant,
                    DateComptable = operationDiverse.DateComptable.Value
                });
            }
            return operationDiverses;
        }
    }
}
