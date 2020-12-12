using Fred.Business;
using Fred.Business.Budget;
using Fred.Business.Budget.BibliothequePrix.Validator;
using Fred.Business.Carburant;
using Fred.Business.CI;
using Fred.Business.Commande.Validators;
using Fred.Business.CommandeEnergies;
using Fred.Business.Depense;
using Fred.Business.EcritureComptable.Validator;
using Fred.Business.FeatureFlipping.Validators;
using Fred.Business.Journal;
using Fred.Business.Module;
using Fred.Business.ObjectifFlash;
using Fred.Business.Personnel;
using Fred.Business.Personnel.Interimaire;
using Fred.Business.Rapport;
using Fred.Business.Rapport.Pointage;
using Fred.Business.Reception;
using Fred.Business.Reception.Validators;
using Fred.Business.Referential.CodeDeplacement;
using Fred.Business.Referential.Tache;
using Fred.Business.ReferentielEtendu;
using Fred.Business.ReferentielFixe;
using Fred.Business.Role;
using Fred.Business.Societe;
using Fred.Business.Societe.Interfaces;
using Fred.Business.Societe.Validators;
using Fred.Business.ValidationPointage;

namespace Fred.DesignPatterns.DI.FluentValidation
{
    public class ValidatorsRegistrar : DependencyRegistrar
    {
        public ValidatorsRegistrar(IDependencyInjectionService dependencyInjectionService) : base(dependencyInjectionService) { }

        public override void RegisterTypes()
        {
            DependencyInjectionService.RegisterType<IAssocieSepValidator, AssocieSepValidator>();
            DependencyInjectionService.RegisterType<IBibliothequePrixModelLoaderValidator, BibliothequePrixSaveModelValidator>();
            DependencyInjectionService.RegisterType<IBudgetRevisionValidatorOld, BudgetRevisionValidatorOld>();
            DependencyInjectionService.RegisterType<ICIValidator, CIValidator>();
            DependencyInjectionService.RegisterType<ICodeDeplacementValidator, CodeDeplacementValidator>();
            DependencyInjectionService.RegisterType<ICommandeAvenantSaveValidator, CommandeAvenantSaveValidator>();
            DependencyInjectionService.RegisterType<ICommandeAvenantValidateValidator, CommandeAvenantValidateValidator>();
            DependencyInjectionService.RegisterType<ICommandeDeleteValidator, CommandeDeleteValidator>();
            DependencyInjectionService.RegisterType<ICommandeEnergieLigneValidator, CommandeEnergieLigneValidator>();
            DependencyInjectionService.RegisterType<ICommandeEnergieValidator, CommandeEnergieValidator>();
            DependencyInjectionService.RegisterType<ICommandeHeaderValidator, CommandeHeaderValidator>();
            DependencyInjectionService.RegisterType<ICommandeLigneValidator, CommandeLigneValidator>();
            DependencyInjectionService.RegisterType<ICommandeLigneLockValidator, CommandeLigneLockValidator>();
            DependencyInjectionService.RegisterType<ICommandeLigneUnlockValidator, CommandeLigneUnlockValidator>();
            DependencyInjectionService.RegisterType<IUpdateReceptionListValidator, ReceptionListValidator>();
            DependencyInjectionService.RegisterType<ICommandeValidator, CommandeValidator>();
            DependencyInjectionService.RegisterType<IContratInterimaireValidator, ContratInterimaireValidator>();
            DependencyInjectionService.RegisterType<IControlePointageValidator, ControlePointageValidator>();
            DependencyInjectionService.RegisterType<IDepenseValidator, DepenseValidator>();
            DependencyInjectionService.RegisterType<IEcritureComptableCumulValidator, EcritureComptableCumulValidator>();
            DependencyInjectionService.RegisterType<IEcritureComptableRejetValidator, EcritureComptableRejetValidator>();
            DependencyInjectionService.RegisterType<IFeatureFlippingValidator, FeatureFlippingValidator>();
            DependencyInjectionService.RegisterType<IFonctionnaliteValidator, FonctionnaliteValidator>();
            DependencyInjectionService.RegisterType<IJournalValidator, JournalValidator>();
            DependencyInjectionService.RegisterType<ILotFarValidator, LotFarValidator>();
            DependencyInjectionService.RegisterType<ILotPointageValidator, LotPointageValidator>();
            DependencyInjectionService.RegisterType<IModuleValidator, ModuleValidator>();
            DependencyInjectionService.RegisterType<IObjectifFlashValidator, ObjectifFlashValidator>();
            DependencyInjectionService.RegisterType<IParametrageCarburantValidator, ParametrageCarburantValidator>();
            DependencyInjectionService.RegisterType<IParametrageReferentielEtenduValidator, ParametrageReferentielEtenduValidator>();
            DependencyInjectionService.RegisterType<IPersonnelImageValidator, PersonnelImageValidator>();
            DependencyInjectionService.RegisterType<IPersonnelValidator, PersonnelValidator>();
            DependencyInjectionService.RegisterType<IPointageValidator, PointageValidator>();
            DependencyInjectionService.RegisterType<IRapportValidator, RapportValidator>();
            DependencyInjectionService.RegisterType<IReceptionValidator, ReceptionValidator>();
            DependencyInjectionService.RegisterType<IReceptionViserRulesValidator, ReceptionViserRulesValidator>();
            DependencyInjectionService.RegisterType<IReferentielEtenduValidator, ReferentielEtenduValidator>();
            DependencyInjectionService.RegisterType<IRemonteeVracValidator, RemonteeVracValidator>();
            DependencyInjectionService.RegisterType<IRessourceTacheDeviseValidatorOld, RessourceTacheDeviseValidatorOld>();
            DependencyInjectionService.RegisterType<IRessourceTacheValidatorOld, RessourceTacheValidatorOld>();
            DependencyInjectionService.RegisterType<IRessourceValidator, RessourceValidator>();
            DependencyInjectionService.RegisterType<IRoleValidator, RoleValidator>();
            DependencyInjectionService.RegisterType<ISeuilValidationValidator, SeuilValidationValidator>();
            DependencyInjectionService.RegisterType<ISocieteClassificationValidator, SocieteClassificationValidator>();
            DependencyInjectionService.RegisterType<ISocieteValidator, SocieteValidator>();
            DependencyInjectionService.RegisterType<ITacheRecetteValidatorOld, TacheRecetteValidatorOld>();
            DependencyInjectionService.RegisterType<ITacheValidator, TacheValidator>();
        }
    }
}
