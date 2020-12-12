using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Journal;
using Fred.Business.OperationDiverse;
using Fred.Business.Referential;
using Fred.Business.Referential.Nature;
using Fred.Business.ReferentielFixe;
using Fred.Business.Unite;
using Fred.Entities.EcritureComptable;
using Fred.Entities.Journal;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Framework.Models;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.EcritureComptable;
using Fred.Web.Shared.Models.Nature;

namespace Fred.Business.EcritureComptable.Import
{
    /// <summary>
    /// Classe avec des méthodes additionnelles pour le manager des écritures comtpables
    /// </summary>
    public class EcritureComptableImportHelper
    {
        private readonly IUniteManager uniteManager;
        private readonly IJournalManager journalManager;
        private readonly INatureManager natureManager;
        private readonly IFamilleOperationDiverseManager familleOperationDiverseManager;
        private readonly IDeviseManager deviseManager;
        private readonly IReferentielFixeManager referentielFixeManager;

        public EcritureComptableImportHelper(
            IUniteManager uniteManager,
            IJournalManager journalManager,
            INatureManager natureManager,
            IFamilleOperationDiverseManager familleOperationDiverseManager,
            IDeviseManager deviseManager,
            IReferentielFixeManager referentielFixeManager)
        {
            this.uniteManager = uniteManager;
            this.journalManager = journalManager;
            this.natureManager = natureManager;
            this.familleOperationDiverseManager = familleOperationDiverseManager;
            this.deviseManager = deviseManager;
            this.referentielFixeManager = referentielFixeManager;
        }

        /// <summary>
        /// Retourne la composition du champs NumeroPiece
        /// </summary>
        /// <param name="numeroPiece">NuméroPiece à tester</param>
        /// <returns>Tableau des différents champs composant le NumeroPiece</returns>
        internal List<Result<EcritureComptableRetreiveResult>> TestNumeroPieceResult(IEnumerable<EcritureComptableDto> ecritureComptableDtos)
        {
            List<Result<EcritureComptableRetreiveResult>> results = new List<Result<EcritureComptableRetreiveResult>>();

            foreach (EcritureComptableDto ecritureComptable in ecritureComptableDtos)
            {
                string[] champsNumeroPieceToCheck = ecritureComptable.NumeroPiece.Split('_');
                // Si une des valeurs composant le NumeroPiece est vide, NULL (a priori impossible mais au cas ou) ou == 0 on rejette la ligne
                // Composition du champs NumeroPiece : "{ dws }_{ dint }_{ dnolig }_{ ligana }"
                EcritureComptableRetreiveResult result = new EcritureComptableRetreiveResult { NumeroPiece = ecritureComptable.NumeroPiece, Montant = ecritureComptable.Montant };
                if (champsNumeroPieceToCheck.Any(r => string.IsNullOrEmpty(r)))
                {
                    results.Add(Result<EcritureComptableRetreiveResult>.CreateFailureWithData(FeatureEcritureComptable.EcritureComptableImportHelper_ErrorFormatNumeroPiece, result));
                }
                else
                {
                    results.Add(Result<EcritureComptableRetreiveResult>.CreateSuccess(result));
                }
            }
            return results;
        }
        public List<EcritureComptableMappingModel> RetrieveDevises(List<string> codeDevises, List<EcritureComptableFtpDto> ecritureComptables)
        {
            List<EcritureComptableMappingModel> mappingModels = new List<EcritureComptableMappingModel>();
            IReadOnlyList<DeviseEnt> devises = deviseManager.GetDevises(codeDevises.Distinct().ToList());

            List<EcritureComptableMappingModel> ecritureComptableMappingModels = (from ecritureComptable in ecritureComptables
                                                                                  join devise in devises on ecritureComptable.CodeDevise equals devise.IsoCode
                                                                                  into results
                                                                                  from result in results.DefaultIfEmpty()
                                                                                  select new EcritureComptableMappingModel
                                                                                  {
                                                                                      DeviseId = result?.DeviseId ?? 0,
                                                                                      DeviseCode = result != null ? result.IsoCode : ecritureComptable.CodeDevise,
                                                                                      NumeroPiece = ecritureComptable.NumeroPiece
                                                                                  }).ToList();

            foreach (EcritureComptableMappingModel item in ecritureComptableMappingModels)
            {
                EcritureComptableMappingModel mappingModel = GenerateEcritureComptableMappingModel(item);
                if (item.DeviseId == 0)
                {
                    mappingModel.Erreurs.Add(string.Format(FeatureEcritureComptable.EcritureComptableImportHelper_ErrorFormatRetrieveDevise, item.DeviseCode));
                }
                else
                {
                    mappingModel.Success = true;
                    mappingModel.DeviseId = item.DeviseId;
                }
                mappingModels.Add(mappingModel);
            }
            return mappingModels;
        }

        private EcritureComptableMappingModel GenerateEcritureComptableMappingModel(EcritureComptableMappingModel item)
        {
            return new EcritureComptableMappingModel
            {
                Success = false,
                NumeroCommande = item.NumeroCommande,
                NumeroPiece = item.NumeroPiece,
                DeviseCode = item.DeviseCode,
                NatureCode = item.NatureCode,
                RessourceId = item.RessourceId,
                ParentFamilyODWithOrder = item.ParentFamilyODWithOrder,
                ParentFamilyODWithoutOrder = item.ParentFamilyODWithoutOrder,
                CiId = item.CiId,
                Erreurs = new List<string>()
            };
        }

        public List<EcritureComptableMappingModel> RetrieveUnites(List<string> codeUnites, List<EcritureComptableFtpDto> ecritureComptables, List<EcritureComptableMappingModel> famillesOD)
        {
            List<EcritureComptableMappingModel> mappingModels = new List<EcritureComptableMappingModel>();
            IReadOnlyList<UniteEnt> unites = uniteManager.GetUnites(codeUnites.Distinct().ToList());


            List<EcritureComptableFtpDto> ecritureComptablesWithFamille = (from ecritureComptable in ecritureComptables
                                                                           join famille in famillesOD on ecritureComptable.NumeroPiece equals famille.NumeroPiece
                                                                           select new EcritureComptableFtpDto
                                                                           {
                                                                               NumeroPiece = ecritureComptable.NumeroPiece,
                                                                               FamilleOperationDiversesCode = famille.FamilleOperationDiverse?.Code,
                                                                               Unite = ecritureComptable.Unite

                                                                           }).ToList();


            List<EcritureComptableMappingModel> ecritureComptableMappingModels = (from ecritureComptable in ecritureComptablesWithFamille
                                                                                  join unite in unites on ecritureComptable.Unite equals unite.Code
                                                                                  into results
                                                                                  from result in results.DefaultIfEmpty()
                                                                                  select new EcritureComptableMappingModel
                                                                                  {
                                                                                      UniteId = result?.UniteId ?? 0,
                                                                                      UniteCode = result != null ? result.Code : ecritureComptable.Unite,
                                                                                      NumeroPiece = ecritureComptable.NumeroPiece,
                                                                                      FamilleOperationDiverseCode = ecritureComptable.FamilleOperationDiversesCode
                                                                                  }).ToList();

            foreach (EcritureComptableMappingModel item in ecritureComptableMappingModels)
            {
                EcritureComptableMappingModel mappingModel = GenerateEcritureComptableMappingModel(item);
                if (item.UniteId == 0 && IsUniteIsRequired(item))
                {
                    mappingModel.Erreurs.Add(string.Format(FeatureEcritureComptable.EcritureComptableImportHelper_ErrorFormatRetrieveUnite, item.UniteCode));
                }
                else
                {
                    mappingModel.Success = true;
                    mappingModel.UniteId = item.UniteId == 0 ? uniteManager.GetUnite("FRT").UniteId : item.UniteId;
                    mappingModel.UniteCode = item.UniteCode;
                }
                mappingModels.Add(mappingModel);
            }
            return mappingModels;
        }

        private bool IsUniteIsRequired(EcritureComptableMappingModel item)
        {
            return item.FamilleOperationDiverseCode == "MO" || item.FamilleOperationDiverseCode == "ACH" || item.FamilleOperationDiverseCode == "MIT";
        }

        public List<EcritureComptableMappingModel> RetrieveNatures(List<string> codeNatures, List<int> societeIds, List<EcritureComptableFtpDto> ecritureComptables)
        {
            List<EcritureComptableMappingModel> mappingModels = new List<EcritureComptableMappingModel>();

            IReadOnlyList<NatureFamilleOdModel> natures = natureManager.GetCodeNatureAndFamilliesOD(codeNatures.Distinct().ToList(), societeIds);
            IReadOnlyList<RessourceEnt> ressources = referentielFixeManager.GetRessourceList(ecritureComptables.Select(q => q.RessourceCode).ToList());
            List<EcritureComptableMappingModel> ecritureComptableMappingModels = (from ecritureComptable in ecritureComptables
                                                                                  join nature in natures on new { ecritureComptable.NatureAnalytique, ecritureComptable.SocieteCode } equals new { nature.NatureAnalytique, nature.SocieteCode }
                                                                                  into ecritureNatures
                                                                                  from ecritureNature in ecritureNatures.DefaultIfEmpty()
                                                                                  join ressource in ressources on ecritureComptable.RessourceCode equals ressource.Code
                                                                                  into ecritureNatureRessources
                                                                                  from ecritureNatureRessource in ecritureNatureRessources.DefaultIfEmpty()
                                                                                  select new EcritureComptableMappingModel
                                                                                  {
                                                                                      NatureId = ecritureNature?.Nature?.NatureId ?? 0,
                                                                                      Nature = ecritureNature.Nature,
                                                                                      NumeroPiece = ecritureComptable.NumeroPiece,
                                                                                      ParentFamilyODWithOrder = ecritureNature.ParentFamilyODWithOrder,
                                                                                      ParentFamilyODWithoutOrder = ecritureNature.ParentFamilyODWithoutOrder,
                                                                                      RessourceId = GetRessourceId(ecritureComptable, ecritureNature.Nature, ecritureNatureRessource),
                                                                                      RessourceCode = ecritureComptable.RessourceCode
                                                                                  }).ToList();

            foreach (EcritureComptableMappingModel item in ecritureComptableMappingModels)
            {
                EcritureComptableMappingModel mappingModel = GenerateEcritureComptableMappingModel(item);
                if (item.NatureId == 0)
                {
                    mappingModel.Erreurs.Add(string.Format(FeatureEcritureComptable.EcritureComptableImportHelper_ErrorFormatRetrieveNature, mappingModel.NatureCode));
                }
                if (item.RessourceId == 0)
                {
                    mappingModel.Erreurs.Add(FeatureEcritureComptable.EcritureComptableImportHelper_ErrorFormatRetrieveRessource);
                }
                if (item.RessourceId == -1)
                {
                    mappingModel.Erreurs.Add(FeatureEcritureComptable.EcritureComptableImportHelper_ErrorFormatRetrieveRessourceObligatoire);
                }
                if (item.RessourceId == -2)
                {
                    mappingModel.Erreurs.Add(string.Format(FeatureEcritureComptable.EcritureComptableImportHelper_ErrorFormatRetrieveRessourceUnknow, item.RessourceCode));
                }
                if (item.NatureId != 0)
                {
                    mappingModel.Success = true;
                    mappingModel.NatureId = item.NatureId;
                    mappingModel.Nature = item.Nature;
                    if (item.RessourceId != 0)
                    {
                        mappingModel.RessourceId = item.RessourceId;
                    }
                }
                mappingModels.Add(mappingModel);
            }
            return mappingModels;
        }

        private static int GetRessourceId(EcritureComptableFtpDto ecritureComptable, NatureEnt ecritureNature, RessourceEnt ecritureNatureRessource)
        {
            if (ecritureComptable.RessourceId != 0)
            {
                return ecritureComptable.RessourceId;
            }
            else if (ecritureNatureRessource != null && ecritureNatureRessource.RessourceId != 0)
            {
                return ecritureNatureRessource.RessourceId;
            }
            else if (ecritureNature != null && ecritureNature.RessourceId.HasValue)
            {
                return ecritureNature.RessourceId.Value;
            }
            else if (string.IsNullOrEmpty(ecritureComptable.RessourceCode) && ecritureNatureRessource == null)
            {
                return -1;
            }
            else if (!string.IsNullOrEmpty(ecritureComptable.RessourceCode) && ecritureNatureRessource == null)
            {
                return -2;
            }
            else
            {
                return 0;
            }
        }

        public List<Result<EcritureComptableRetreiveResult>> RetrieveNatures(IEnumerable<EcritureComptableDto> ecritureComptableDtos, IEnumerable<NatureEnt> naturesOfSociete)
        {
            List<Result<EcritureComptableRetreiveResult>> results = new List<Result<EcritureComptableRetreiveResult>>();
            foreach (EcritureComptableDto ecritureComptableDto in ecritureComptableDtos)
            {
                results.Add(RetrieveNatures(ecritureComptableDto, naturesOfSociete));
            }
            return results;
        }

        private Result<EcritureComptableRetreiveResult> RetrieveNatures(EcritureComptableDto ecriture, IEnumerable<NatureEnt> naturesOfSociete)
        {
            NatureEnt nature = naturesOfSociete.FirstOrDefault(n => string.Equals(n.Code, ecriture.AnaelCodeNature, StringComparison.OrdinalIgnoreCase));
            EcritureComptableRetreiveResult result = new EcritureComptableRetreiveResult { Nature = nature, NumeroPiece = ecriture.NumeroPiece };
            if (nature != null)
            {
                return Result<EcritureComptableRetreiveResult>.CreateSuccess(result);
            }
            string messageError = string.Format(FeatureEcritureComptable.EcritureComptableImportHelper_ErrorFormatRetrieveJournal, ecriture.AnaelCodeJournal);
            return Result<EcritureComptableRetreiveResult>.CreateFailureWithData(messageError, result);
        }

        public List<Result<EcritureComptableRetreiveResult>> RetrieveCIs(IEnumerable<EcritureComptableDto> ecritureComptableDtos, Dictionary<int, string> dicoCiIdCiCode)
        {
            List<Result<EcritureComptableRetreiveResult>> results = new List<Result<EcritureComptableRetreiveResult>>();
            foreach (EcritureComptableDto ecriture in ecritureComptableDtos)
            {
                results.Add(RetrieveCI(ecriture, dicoCiIdCiCode));
            }
            return results;
        }

        public List<EcritureComptableMappingModel> RetrieveCIs(Dictionary<int, string> cisOfSociete, IEnumerable<EcritureComptableFtpDto> ecritureComptables)
        {
            List<EcritureComptableMappingModel> mappingModels = new List<EcritureComptableMappingModel>();

            IEnumerable<EcritureComptableMappingModel> ecritureComptableMappingModels = (from ecritureComptable in ecritureComptables
                                                                                         join ci in cisOfSociete on ecritureComptable.CodeCi equals ci.Value
                                                                                         into results
                                                                                         from result in results.DefaultIfEmpty()
                                                                                         select new EcritureComptableMappingModel
                                                                                         {
                                                                                             NumeroPiece = ecritureComptable.NumeroPiece,
                                                                                             NumeroCommande = ecritureComptable.NumeroCommande,
                                                                                             CiId = result.Key,
                                                                                             CodeCi = ecritureComptable.CodeCi
                                                                                         });

            foreach (EcritureComptableMappingModel item in ecritureComptableMappingModels)
            {
                EcritureComptableMappingModel mappingModel = GenerateEcritureComptableMappingModel(item);
                if (item.CiId == 0)
                {
                    mappingModel.Erreurs.Add(string.Format(FeatureEcritureComptable.EcritureComptableImportHelper_ErrorFormatRetrieveCi, item.CodeCi));
                }
                else
                {
                    mappingModel.Success = true;
                }
                mappingModels.Add(mappingModel);
            }
            return mappingModels;
        }

        private Result<EcritureComptableRetreiveResult> RetrieveCI(EcritureComptableDto ecriture, Dictionary<int, string> dicoCiIdCiCode)
        {
            EcritureComptableRetreiveResult result = new EcritureComptableRetreiveResult();
            if (dicoCiIdCiCode.ContainsValue(ecriture.AnaelCodeCi))
            {
                int ciId = dicoCiIdCiCode.FirstOrDefault(kvp => kvp.Value == ecriture.AnaelCodeCi).Key;
                result.CiId = ciId;
                result.NumeroPiece = ecriture.NumeroPiece;
                return Result<EcritureComptableRetreiveResult>.CreateSuccess(result);
            }
            string messageError = string.Format(FeatureEcritureComptable.EcritureComptableImportHelper_ErrorFormatRetrieveCi, ecriture);
            return Result<EcritureComptableRetreiveResult>.CreateFailureWithData(messageError, result);
        }

        public List<Result<EcritureComptableRetreiveResult>> RetrieveJournaux(IEnumerable<EcritureComptableDto> ecritureComptableDtos, IEnumerable<JournalEnt> journauxOfSociete)
        {
            List<Result<EcritureComptableRetreiveResult>> results = new List<Result<EcritureComptableRetreiveResult>>();
            foreach (EcritureComptableDto ecriture in ecritureComptableDtos)
            {
                results.Add(RetrieveJournal(ecriture, journauxOfSociete));
            }
            return results;
        }

        private Result<EcritureComptableRetreiveResult> RetrieveJournal(EcritureComptableDto ecriture, IEnumerable<JournalEnt> journauxOfSociete)
        {
            JournalEnt journal = journauxOfSociete.FirstOrDefault(j => string.Equals(j.Code, ecriture.AnaelCodeJournal, StringComparison.OrdinalIgnoreCase));
            EcritureComptableRetreiveResult result = new EcritureComptableRetreiveResult { Journal = journal, NumeroPiece = ecriture.NumeroPiece };
            if (journal != null)
            {
                return Result<EcritureComptableRetreiveResult>.CreateSuccess(result);
            }
            string messageError = string.Format(FeatureEcritureComptable.EcritureComptableImportHelper_ErrorFormatRetrieveJournal, ecriture.AnaelCodeJournal);
            return Result<EcritureComptableRetreiveResult>.CreateFailureWithData(messageError, result);
        }

        public List<Result<EcritureComptableRetreiveResult>> RetrieveODFamilies(IEnumerable<JournalEnt> journaux, IEnumerable<NatureEnt> natures, IEnumerable<EcritureComptableDto> ecritureComptables, IEnumerable<FamilleOperationDiverseEnt> famillesODOfSociete)
        {
            List<Result<EcritureComptableRetreiveResult>> results = new List<Result<EcritureComptableRetreiveResult>>();
            IEnumerable<EcritureComptableDto> ecritureWithOrder = ecritureComptables.Where(q => string.IsNullOrEmpty(q.AnaelCodeCommande));
            IEnumerable<EcritureComptableDto> ecritureWithoutOrder = ecritureComptables.Where(q => !string.IsNullOrEmpty(q.AnaelCodeCommande));

            //Pour chaque écriture, je regarde son journal, je cherche si j'ai un journal associée, si oui ok, si non result erreur
            foreach (EcritureComptableDto ecriture in ecritureWithOrder)
            {
                EcritureComptableRetreiveResult result = new EcritureComptableRetreiveResult { NumeroPiece = ecriture.NumeroPiece };
                if (journaux.Any(j => j.Code == ecriture.AnaelCodeJournal) && natures.Any(n => n.Code == ecriture.AnaelCodeNature))
                {
                    JournalEnt journal = journaux.FirstOrDefault(j => j.Code == ecriture.AnaelCodeJournal);
                    NatureEnt nature = natures.FirstOrDefault(n => n.Code == ecriture.AnaelCodeNature);
                    result.Journal = journal;
                    result.Nature = nature;
                    results.Add(RetrieveODFamily(journal, nature, ecriture, famillesODOfSociete));
                }
                else
                {
                    results.Add(Result<EcritureComptableRetreiveResult>.CreateFailureWithData(FeatureEcritureComptable.EcritureComptableImportHelper_ErrorFormatRetrieveODFamily, result));
                }
            }

            foreach (EcritureComptableDto ecriture in ecritureWithoutOrder)
            {
                EcritureComptableRetreiveResult result = new EcritureComptableRetreiveResult { NumeroPiece = ecriture.NumeroPiece };
                if (journaux.Any(j => j.Code == ecriture.AnaelCodeJournal) && natures.Any(n => n.Code == ecriture.AnaelCodeNature))
                {
                    JournalEnt journal = journaux.FirstOrDefault(j => j.Code == ecriture.AnaelCodeJournal);
                    NatureEnt nature = natures.FirstOrDefault(n => n.Code == ecriture.AnaelCodeNature);
                    result.Journal = journal;
                    result.Nature = nature;
                    results.Add(RetrieveODFamily(journal, nature, ecriture, famillesODOfSociete));
                }
                else
                {
                    results.Add(Result<EcritureComptableRetreiveResult>.CreateFailureWithData(FeatureEcritureComptable.EcritureComptableImportHelper_ErrorFormatRetrieveODFamily, result));
                }
            }

            return results;
        }

        private Result<EcritureComptableRetreiveResult> RetrieveODFamily(JournalEnt journalEnt, NatureEnt natureEnt, EcritureComptableDto ecriture, IEnumerable<FamilleOperationDiverseEnt> famillesODOfSociete)
        {
            EcritureComptableRetreiveResult result = new EcritureComptableRetreiveResult { NumeroPiece = ecriture.NumeroPiece };
            try
            {
                FamilleOperationDiverseEnt familleEcriture;
                if (!string.IsNullOrEmpty(ecriture.AnaelCodeCommande))
                {
                    familleEcriture = famillesODOfSociete.FirstOrDefault(f => f.FamilleOperationDiverseId == journalEnt.ParentFamilyODWithOrder && f.FamilleOperationDiverseId == natureEnt.ParentFamilyODWithOrder);
                }
                else
                {
                    familleEcriture = famillesODOfSociete.FirstOrDefault(f => f.FamilleOperationDiverseId == journalEnt.ParentFamilyODWithoutOrder && f.FamilleOperationDiverseId == natureEnt.ParentFamilyODWithoutOrder);
                }

                result.FamilleOperationDiverse = familleEcriture;
                //La famille doit-elle avoir un BC associé ?
                return familleEcriture == null
                  ? Result<EcritureComptableRetreiveResult>.CreateFailureWithData(FeatureEcritureComptable.EcritureComptableImportHelper_ErrorFormatRetrieveODFamily, result)
                  : Result<EcritureComptableRetreiveResult>.CreateSuccess(result);
            }
            catch (Exception)
            {
                return Result<EcritureComptableRetreiveResult>.CreateFailureWithData(FeatureEcritureComptable.EcritureComptableImportHelper_ErrorFormatRetrieveODFamily, result);
            }
        }

        public List<Result<EcritureComptableRetreiveResult>> RetrieveODFamilies(IEnumerable<JournalEnt> journaux, IEnumerable<EcritureComptableDto> ecritureComptables, IEnumerable<FamilleOperationDiverseEnt> famillesODOfSociete)
        {
            List<Result<EcritureComptableRetreiveResult>> results = new List<Result<EcritureComptableRetreiveResult>>();
            IEnumerable<EcritureComptableDto> ecritureWithOrder = ecritureComptables.Where(q => string.IsNullOrEmpty(q.AnaelCodeCommande));
            IEnumerable<EcritureComptableDto> ecritureWithoutOrder = ecritureComptables.Where(q => !string.IsNullOrEmpty(q.AnaelCodeCommande));

            //Pour chaque écriture, je regarde son journal, je cherche si j'ai un journal associée, si oui ok, si non result erreur
            foreach (EcritureComptableDto ecriture in ecritureWithOrder)
            {
                EcritureComptableRetreiveResult result = new EcritureComptableRetreiveResult { NumeroPiece = ecriture.NumeroPiece };
                if (journaux.Any(j => j.Code == ecriture.AnaelCodeJournal))
                {
                    JournalEnt journal = journaux.FirstOrDefault(j => j.Code == ecriture.AnaelCodeJournal);
                    result.Journal = journal;
                    results.Add(RetrieveODFamily(journal, ecriture, famillesODOfSociete));
                }
                else
                {
                    results.Add(Result<EcritureComptableRetreiveResult>.CreateFailureWithData(FeatureEcritureComptable.EcritureComptableImportHelper_ErrorFormatRetrieveODFamily, result));
                }
            }

            foreach (EcritureComptableDto ecriture in ecritureWithoutOrder)
            {
                foreach (JournalEnt journal in journaux.Distinct())
                {
                    if (journal.Code == ecriture.AnaelCodeJournal)
                    {
                        results.Add(RetrieveODFamily(journal, ecriture, famillesODOfSociete));
                    }
                }
            }

            return results;
        }

        private Result<EcritureComptableRetreiveResult> RetrieveODFamily(JournalEnt journalEnt, EcritureComptableDto ecriture, IEnumerable<FamilleOperationDiverseEnt> famillesODOfSociete)
        {
            EcritureComptableRetreiveResult result = new EcritureComptableRetreiveResult { NumeroPiece = ecriture.NumeroPiece };
            try
            {
                FamilleOperationDiverseEnt familleEcriture;
                if (!string.IsNullOrEmpty(ecriture.AnaelCodeCommande))
                {
                    familleEcriture = famillesODOfSociete.FirstOrDefault(f => f.FamilleOperationDiverseId == journalEnt.ParentFamilyODWithOrder);
                }
                else
                {
                    familleEcriture = famillesODOfSociete.FirstOrDefault(f => f.FamilleOperationDiverseId == journalEnt.ParentFamilyODWithoutOrder);
                }

                result.FamilleOperationDiverse = familleEcriture;
                //La famille doit-elle avoir un BC associé ?
                return familleEcriture == null
                  ? Result<EcritureComptableRetreiveResult>.CreateFailureWithData(FeatureEcritureComptable.EcritureComptableImportHelper_ErrorFormatRetrieveODFamily, result)
                  : Result<EcritureComptableRetreiveResult>.CreateSuccess(result);
            }
            catch (Exception)
            {
                return Result<EcritureComptableRetreiveResult>.CreateFailureWithData(FeatureEcritureComptable.EcritureComptableImportHelper_ErrorFormatRetrieveODFamily, result);
            }
        }

        internal List<EcritureComptableMappingModel> RetrieveODFamilies(List<NatureEnt> natures, List<EcritureComptableFtpDto> ecritureComptableFtpDto, List<FamilleOperationDiverseEnt> famillesODOfSociete)
        {
            List<EcritureComptableMappingModel> mappingModels = new List<EcritureComptableMappingModel>();

            List<EcritureComptableMappingModel> ecritureComptables = (from ecritureComptable in ecritureComptableFtpDto
                                                                      join nature in natures.Where(n => n.Code != null).Distinct().ToList() on ecritureComptable.NatureAnalytique equals nature.Code
                                                                      into results
                                                                      from result in results.DefaultIfEmpty()
                                                                      select new EcritureComptableMappingModel
                                                                      {
                                                                          NatureId = result?.NatureId ?? 0,
                                                                          Nature = result,
                                                                          NumeroPiece = ecritureComptable.NumeroPiece,
                                                                          ParentFamilyODWithOrder = result?.ParentFamilyODWithOrder ?? 0,
                                                                          ParentFamilyODWithoutOrder = result?.ParentFamilyODWithoutOrder ?? 0,
                                                                          NumeroCommande = ecritureComptable.NumeroCommande
                                                                      }).ToList();

            List<EcritureComptableMappingModel> ecrituresWithOrder = ecritureComptables.Where(x => !string.IsNullOrEmpty(x.NumeroCommande)).ToList();
            List<EcritureComptableMappingModel> ecrituresWithoutOrder = ecritureComptables.Where(x => string.IsNullOrEmpty(x.NumeroCommande)).ToList();
            List<EcritureComptableMappingModel> order = (from ecriture in ecrituresWithOrder
                                                         join famille in famillesODOfSociete on ecriture.ParentFamilyODWithOrder equals famille.FamilleOperationDiverseId
                                                         into results
                                                         from result in results.DefaultIfEmpty()
                                                         select new EcritureComptableMappingModel
                                                         {
                                                             FamilleOperationDiverse = result,
                                                             NumeroPiece = ecriture.NumeroPiece,
                                                             FamilleOperationDiverseId = ecriture.ParentFamilyODWithOrder,
                                                             ParentFamilyODWithOrder = ecriture.ParentFamilyODWithOrder,
                                                             Erreurs = new List<string>()
                                                         }).ToList();

            List<EcritureComptableMappingModel> withoutOrder = (from ecriture in ecrituresWithoutOrder
                                                                join famille in famillesODOfSociete on ecriture.ParentFamilyODWithoutOrder equals famille.FamilleOperationDiverseId
                                                                into results
                                                                from result in results.DefaultIfEmpty()
                                                                select new EcritureComptableMappingModel
                                                                {
                                                                    FamilleOperationDiverse = result,
                                                                    NumeroPiece = ecriture.NumeroPiece,
                                                                    FamilleOperationDiverseId = ecriture.ParentFamilyODWithoutOrder,
                                                                    ParentFamilyODWithoutOrder = ecriture.ParentFamilyODWithoutOrder,
                                                                    Erreurs = new List<string>()
                                                                }).ToList();

            foreach (EcritureComptableMappingModel item in order.Concat(withoutOrder).ToList())
            {
                EcritureComptableMappingModel mappingModel = GenerateEcritureComptableMappingModel(item);

                if (item.ParentFamilyODWithOrder == 0 && item.ParentFamilyODWithoutOrder == 0)
                {
                    mappingModel.Erreurs.Add(FeatureEcritureComptable.EcritureComptableImportHelper_ErrorFormatRetrieveODFamily);
                }
                else
                {
                    mappingModel.Success = true;
                    mappingModel.FamilleOperationDiverse = item.FamilleOperationDiverse;
                    mappingModel.FamilleOperationDiverseId = item.FamilleOperationDiverseId;
                }
                mappingModels.Add(mappingModel);
            }
            return mappingModels;
        }

        internal IEnumerable<NatureEnt> GetNatures(int societeId)
        {
            return natureManager.GetNatureBySocieteId(societeId);
        }

        internal IEnumerable<JournalEnt> GetJournauxComptables(int societeId)
        {
            return journalManager.GetJournalList(societeId);
        }

        internal IEnumerable<FamilleOperationDiverseEnt> GetODFamilies(int societeId)
        {
            return familleOperationDiverseManager.GetFamiliesBySociety(societeId);
        }

        internal IEnumerable<FamilleOperationDiverseEnt> GetODFamilies(List<int> societeIds)
        {
            return familleOperationDiverseManager.GetFamiliesBySociety(societeIds);
        }
    }
}
