using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using Fred.Business.CI;
using Fred.Business.EcritureComptable;
using Fred.Business.EcritureComptable.Import;
using Fred.Business.Groupe;
using Fred.Business.Notification;
using Fred.Business.OperationDiverse;
using Fred.Business.Referential.Nature;
using Fred.Business.Societe;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.EcritureComptable;
using Fred.Entities.Groupe;
using Fred.ImportExport.Business.EcritureComptable.Validator;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.OperationDiverse;
using Fred.ImportExport.DataAccess.Interfaces;
using Fred.ImportExport.Models.EcritureComptable;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.EcritureComptable;
using Fred.Web.Shared.Models.Nature;
using Fred.Web.Shared.Models.OperationDiverse;

namespace Fred.GroupSpecific.Ftp.Societe
{
    public class FtpEcritureComptableFluxManager : EcritureComptableFluxManager
    {
        private const string GroupeFayatTpCode = "GFTP";
        private const string ErrorFamilly = "ERR";
        private const string MoFamilly = "MO";
        private const string AchatFamilly = "ACH";
        private const string MaterielImmobiliseFamilly = "MI";
        private const string MaterielInternePointeFamilly = "MIT";
        private const string AutreDepenseFamilly = "OTH";
        private const string RecetteFamilly = "RCT";

        private readonly IGroupeManager groupeManager;
        private readonly INatureManager natureManager;
        private readonly IEcritureComptableRejetManager ecritureComptableRejetManager;
        private readonly IFamilleOperationDiverseManager familleOperationDiverseManager;
        private readonly EcritureComptableFluxManagerFayatTPValidator validator;
        private readonly ImportExport.Business.EcritureComptable.EcritureComptableConverter ecritureComptableConverter;

        public FtpEcritureComptableFluxManager(
            IFluxManager fluxManager,
            IGroupeManager groupeManager,
            IEcritureComptableImportManager ecritureComptableImportManager,
            ICIManager ciManager,
            ISocieteManager societeManager,
            INotificationManager notificationManager,
            IGroupeRepository groupRepository,
            ISocieteRepository societeRepository,
            INatureManager natureManager,
            IEcritureComptableRejetManager ecritureComptableRejetManager,
            IFamilleOperationDiverseManager familleOperationDiverseManager,
            IFluxRepository fluxRepository)
            : base(fluxManager, null, ecritureComptableImportManager, ciManager, societeManager, notificationManager, groupRepository, societeRepository, fluxRepository)
        {
            this.groupeManager = groupeManager;
            this.natureManager = natureManager;
            this.ecritureComptableRejetManager = ecritureComptableRejetManager;
            this.familleOperationDiverseManager = familleOperationDiverseManager;

            validator = new EcritureComptableFluxManagerFayatTPValidator();
            ecritureComptableConverter = new ImportExport.Business.EcritureComptable.EcritureComptableConverter();
        }

        public override async Task<IEnumerable<EcritureComptableFtpSapModel>> ImportEcritureComptableAsync(List<EcritureComptableFayatTpImportModel> ecritureComptableFayatTps)
        {
            IList<EcritureComptableFtpDto> ecritureComptableFtpDtos = new List<EcritureComptableFtpDto>();
            var ecritureComptableValide = new List<EcritureComptableFayatTpModel>();
            var ecritureComptableInvalide = new List<EcritureComptableFayatTpModel>();
            var ecritureComptableFtpSapModels = new List<EcritureComptableFtpSapModel>();
            GroupeEnt groupe = groupeManager.GetGroupeByCode(Constantes.CodeGroupeFTP);
            IReadOnlyList<int> societeIds = SocieteManager.GetSocieteIdsByGroupeId(groupe.GroupeId);

            List<NatureFamilleOdModel> natureFamilleOds = GetOdFamilies(ecritureComptableFayatTps, societeIds);

            List<EcritureComptableFayatTpModel> ecritureComptableFayatTpModels = ecritureComptableConverter.Join(ecritureComptableFayatTps, natureFamilleOds);

            AssociateFamilies(ecritureComptableFayatTpModels);

            ValidateEcritureComptable(ecritureComptableValide, ecritureComptableInvalide, ecritureComptableFayatTpModels);

            ProcessInvalideEcritureComptable(ecritureComptableInvalide.Concat(ecritureComptableValide.Where(ecriture => ecriture.GroupeCode != GroupeFayatTpCode)).ToList());
            ecritureComptableInvalide = ecritureComptableInvalide.Concat(ecritureComptableValide.Where(ecriture => ecriture.GroupeCode != GroupeFayatTpCode)).ToList();
            List<EcritureComptableFtpDto> ecritureComptables = ecritureComptableConverter.ConvertToEcritureComptableFtpDto(ecritureComptableValide.Where(ecriture => ecriture.Errors == null).ToList());
            if (ecritureComptables.Count > 0)
            {
                ecritureComptableFtpDtos = await EcritureComptableImportManager.ManageImportEcritureComptablesAsync(ecritureComptables, societeIds.ToList()).ConfigureAwait(false);
            }

            ecritureComptableFtpSapModels.AddRange(ecritureComptableConverter.ConvertToEcritureComptableFtpSapModel(ecritureComptableInvalide));
            ecritureComptableFtpSapModels.AddRange(ecritureComptableConverter.ConvertToEcritureComptableFtpSapModel(ecritureComptableFtpDtos));

            return ecritureComptableFtpSapModels;
        }

        private void ValidateEcritureComptable(List<EcritureComptableFayatTpModel> ecritureComptableValide, List<EcritureComptableFayatTpModel> ecritureComptableInvalide, List<EcritureComptableFayatTpModel> ecritureComptableFayatTpModels)
        {
            foreach (EcritureComptableFayatTpModel ecritureComptableFayatTpModel in ecritureComptableFayatTpModels)
            {
                ValidationResult resultat = ValidateRules(ecritureComptableFayatTpModel);
                if (resultat.IsValid && !string.IsNullOrEmpty(ecritureComptableFayatTpModel.NatureAnalytique))
                {
                    ecritureComptableValide.Add(ecritureComptableFayatTpModel);
                }
                else
                {
                    ecritureComptableFayatTpModel.Errors = new List<string>();
                    resultat.Errors.ToList().ForEach(error => ecritureComptableFayatTpModel.Errors.Add(error.ErrorMessage));
                    ecritureComptableInvalide.Add(ecritureComptableFayatTpModel);
                }
            }
        }

        private void ProcessInvalideEcritureComptable(List<EcritureComptableFayatTpModel> ecritureComptableInvalides)
        {
            if (ecritureComptableInvalides.Count > 0)
            {
                var ecritureComptableFayatTpRejetModels = new List<EcritureComptableFayatTpRejetModel>();
                foreach (EcritureComptableFayatTpModel ecritureComptableInvalide in ecritureComptableInvalides)
                {
                    CheckCodeGroupe(ecritureComptableInvalide);

                    CheckCodeSociete(ecritureComptableInvalide);

                    CheckNatureAnalytique(ecritureComptableInvalide);

                    ecritureComptableFayatTpRejetModels.Add(new EcritureComptableFayatTpRejetModel
                    {
                        CiID = 0,
                        DateRejet = DateTime.UtcNow,
                        NumeroPiece = ecritureComptableInvalide.NumeroPiece ?? ecritureComptableInvalide.Libelle ?? ecritureComptableInvalide.SocieteCode,
                        RejetMessage = ecritureComptableInvalide.Errors
                    });

                    ecritureComptableRejetManager.AddRejets(ecritureComptableFayatTpRejetModels);
                }
            }
        }

        private static void CheckNatureAnalytique(EcritureComptableFayatTpModel ecritureComptableInvalide)
        {
            if (ecritureComptableInvalide.Errors == null)
            {
                ecritureComptableInvalide.Errors = new List<string>();
            }

            if (string.IsNullOrEmpty(ecritureComptableInvalide.NatureAnalytique))
            {
                ecritureComptableInvalide.Errors.Add(FeatureEcritureComptable.EcritureComptable_Erreur_CodeNatureAnalytique_Obligatoire);
            }
            else if (ecritureComptableInvalide.NatureAnalytique.Contains("UNKNOW"))
            {
                ecritureComptableInvalide.Errors.Add(string.Format(FeatureEcritureComptable.EcritureComptable_Erreur_CodeNatureAnalytique_Inconnu, ecritureComptableInvalide.NatureAnalytique.Replace("UNKNOW_", string.Empty)));
            }
        }

        private static void CheckCodeSociete(EcritureComptableFayatTpModel ecritureComptableInvalide)
        {
            if (ecritureComptableInvalide.Errors == null)
            {
                ecritureComptableInvalide.Errors = new List<string>();
            }

            if (string.IsNullOrEmpty(ecritureComptableInvalide.SocieteCode))
            {
                ecritureComptableInvalide.Errors.Add(FeatureEcritureComptable.EcritureComptable_Erreur_CodeSocieteInvalide);
            }
        }

        private static void CheckCodeGroupe(EcritureComptableFayatTpModel ecritureComptableInvalide)
        {
            if (ecritureComptableInvalide.Errors == null)
            {
                ecritureComptableInvalide.Errors = new List<string>();
            }

            if (string.IsNullOrEmpty(ecritureComptableInvalide.GroupeCode))
            {
                ecritureComptableInvalide.Errors.Add(FeatureEcritureComptable.EcritureComptable_Erreur_CodeGroupeObligatoire);
            }
            else if (ecritureComptableInvalide.GroupeCode != GroupeFayatTpCode)
            {
                ecritureComptableInvalide.Errors.Add(FeatureEcritureComptable.EcritureComptable_Erreur_CodeGroupeInvalide);
            }
        }

        private ValidationResult ValidateRules(EcritureComptableFayatTpModel ecritureComptableFayatTpModel)
        {
            switch (ecritureComptableFayatTpModel.CodeFamille)
            {
                case MoFamilly:
                    return validator.ValidateMoRules(ecritureComptableFayatTpModel);
                case AchatFamilly:
                    return validator.ValidateAchatRules(ecritureComptableFayatTpModel);
                case MaterielImmobiliseFamilly:
                    return validator.ValidateMaterielImmobiliseRules(ecritureComptableFayatTpModel);
                case MaterielInternePointeFamilly:
                    return validator.ValidateMaterielInternePointeRules(ecritureComptableFayatTpModel);
                case AutreDepenseFamilly:
                    return validator.ValidateAutreDepensesRules(ecritureComptableFayatTpModel);
                case RecetteFamilly:
                    return validator.ValidateRecetteRules(ecritureComptableFayatTpModel);
                case ErrorFamilly:
                    return UnableToDetermineFamily();
                default:
                    return MissingFamily();
            }
        }

        private ValidationResult UnableToDetermineFamily()
        {
            var validationResult = new ValidationResult();
            var fail = new ValidationFailure("CodeFamille", FeatureEcritureComptable.EcritureComptable_Erreur_FamilleOperationDiverse_Indeterminee);
            validationResult.Errors.Add(fail);
            return validationResult;
        }

        private ValidationResult MissingFamily()
        {
            var validationResult = new ValidationResult();
            var fail = new ValidationFailure("CodeFamille", FeatureEcritureComptable.EcritureComptable_Erreur_FamilleOperationDiverse_Inconnue);
            validationResult.Errors.Add(fail);
            return validationResult;
        }

        private void AssociateFamilies(List<EcritureComptableFayatTpModel> ecritureComptableFayatTpModels)
        {
            foreach (EcritureComptableFayatTpModel ecritureComptableFayatTpModel in ecritureComptableFayatTpModels)
            {
                if (IsAssignedToOneFamlilyOnly(ecritureComptableFayatTpModel))
                {
                    if (IsOrderNumberIsPresent(ecritureComptableFayatTpModel) && IsRapportLigneIdIsPresent(ecritureComptableFayatTpModel))
                    {
                        ecritureComptableFayatTpModel.FamilleOperationDiverseId = -2;
                        ecritureComptableFayatTpModel.CodeFamille = ErrorFamilly;
                    }
                    else if (IsOrderNumberIsPresent(ecritureComptableFayatTpModel) && !IsRapportLigneIdIsPresent(ecritureComptableFayatTpModel))
                    {
                        // Si que commande alors on prends la famille avec commande
                        ecritureComptableFayatTpModel.FamilleOperationDiverseId = ecritureComptableFayatTpModel.ParentFamilyODWithOrder;
                        ecritureComptableFayatTpModel.CodeFamille = ecritureComptableFayatTpModel.CodeFamilleWithOrder;
                    }
                    else if (!IsOrderNumberIsPresent(ecritureComptableFayatTpModel))
                    {
                        ecritureComptableFayatTpModel.FamilleOperationDiverseId = ecritureComptableFayatTpModel.ParentFamilyODWithoutOrder;
                        ecritureComptableFayatTpModel.CodeFamille = ecritureComptableFayatTpModel.CodeFamilleWithoutOrder;
                    }
                }
                else if (IsOrderNumberIsPresent(ecritureComptableFayatTpModel) && IsRapportLigneIdIsPresent(ecritureComptableFayatTpModel))
                {
                    ecritureComptableFayatTpModel.FamilleOperationDiverseId = -2;
                    ecritureComptableFayatTpModel.CodeFamille = ErrorFamilly;
                }
                else if (IsOrderNumberIsPresent(ecritureComptableFayatTpModel))
                {
                    ecritureComptableFayatTpModel.FamilleOperationDiverseId = ecritureComptableFayatTpModel.ParentFamilyODWithOrder;
                    ecritureComptableFayatTpModel.CodeFamille = ecritureComptableFayatTpModel.CodeFamilleWithOrder;
                }
                else if (!IsOrderNumberIsPresent(ecritureComptableFayatTpModel))
                {
                    ecritureComptableFayatTpModel.FamilleOperationDiverseId = ecritureComptableFayatTpModel.ParentFamilyODWithoutOrder;
                    ecritureComptableFayatTpModel.CodeFamille = ecritureComptableFayatTpModel.CodeFamilleWithoutOrder;
                }
                else
                {
                    ecritureComptableFayatTpModel.FamilleOperationDiverseId = -1;
                    ecritureComptableFayatTpModel.CodeFamille = string.Empty;
                }
            }
        }

        private bool IsOrderNumberIsPresent(EcritureComptableFayatTpModel ecritureComptable)
        {
            return !string.IsNullOrEmpty(ecritureComptable.NumeroCommande);
        }

        private bool IsRapportLigneIdIsPresent(EcritureComptableFayatTpModel ecritureComptable)
        {
            return !string.IsNullOrEmpty(ecritureComptable.RapportLigneId);
        }

        private bool IsAssignedToOneFamlilyOnly(EcritureComptableFayatTpModel natureFamilleOd)
        {
            return natureFamilleOd.ParentFamilyODWithOrder == natureFamilleOd.ParentFamilyODWithoutOrder || natureFamilleOd.ParentFamilyODWithOrder == 0 && natureFamilleOd.ParentFamilyODWithoutOrder != 0 || natureFamilleOd.ParentFamilyODWithOrder != 0 && natureFamilleOd.ParentFamilyODWithoutOrder == 0;
        }

        private List<NatureFamilleOdModel> GetOdFamilies(List<EcritureComptableFayatTpImportModel> ecritureComptableFayatTps, IReadOnlyList<int> societeIds)
        {
            List<NatureFamilleOdModel> natures = natureManager.GetCodeNatureAndFamilliesOD(ecritureComptableFayatTps.Select(q => q.NatureAnalytique).ToList(), societeIds.ToList()).ToList();
            IReadOnlyList<FamilleOperationDiverseModel> famillesWithOrder = familleOperationDiverseManager.GetFamilles(natures.Select(nature => nature.ParentFamilyODWithOrder).ToList());
            IReadOnlyList<FamilleOperationDiverseModel> famillesWithoutOrder = familleOperationDiverseManager.GetFamilles(natures.Select(nature => nature.ParentFamilyODWithoutOrder).ToList());

            foreach (NatureFamilleOdModel item in natures.Where(nature => nature.ParentFamilyODWithOrder != 0 || nature.ParentFamilyODWithoutOrder != 0))
            {
                item.CodeFamilleODWithOrder = famillesWithOrder.Where(famille => famille.FamilleOperationDiverseId == item.ParentFamilyODWithOrder).Select(q => q.Code).FirstOrDefault();
                item.CodeFamilleODWhitoutOrder = famillesWithoutOrder.Where(famille => famille.FamilleOperationDiverseId == item.ParentFamilyODWithoutOrder).Select(q => q.Code).FirstOrDefault();
            }
            return natures;
        }
    }
}
