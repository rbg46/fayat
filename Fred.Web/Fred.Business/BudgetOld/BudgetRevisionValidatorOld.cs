using FluentValidation;
using FluentValidation.Results;
using Fred.Business.Referential;
using Fred.Business.Referential.Tache;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Budget;

namespace Fred.Business.Budget
{
    /// <summary>
    ///   Implémentation de la validation du budget
    /// </summary>
    public class BudgetRevisionValidatorOld : AbstractValidator<BudgetRevisionEnt>, IBudgetRevisionValidatorOld
    {
        private readonly ITacheValidator tacheValidator;
        private readonly IBudgetRepositoryOld budgetRepository;


        /// <summary>
        ///   Constructeur
        /// </summary>
        /// <param name="tv">Validator des taches enfants d'une révision</param>
        /// <param name="uow">Unit of work</param>
        /// <param name="budgetRepositoryOld"></param>
        public BudgetRevisionValidatorOld(ITacheValidator tv, IUnitOfWork uow, IBudgetRepositoryOld budgetRepository)
        {
            this.CascadeMode = CascadeMode.StopOnFirstFailure;
            this.tacheValidator = tv;
            this.budgetRepository = budgetRepository;

            // Règles de gestions techniques
            AddTechnicalRules();

            // Règles de gestions métiers
            AddBusinessRules();

            // Règles de gestion en cascade
            AddChildRules();
        }

        /// <summary>
        ///   Ajout de règles de validation techniques
        /// </summary>
        private void AddTechnicalRules()
        {
            RuleFor(e => e.Statut).InclusiveBetween((int)StatutBudget.Brouillon, (int)StatutBudget.Valider).WithMessage("Le statut du budget n'est pas valide.");
        }

        /// <summary>
        ///   Ajout des règles de validation métiers
        /// </summary>
        private void AddBusinessRules()
        {
            ////RuleFor(e => e.AuteurCreationId).NotNull().WithMessage("L'auteur du budget doit être indiqué.");
            ////RuleFor(e => e.DateCreation).NotNull().WithMessage("La date de création du budget doit être indiquée.");

            // RG_1132_003: Les champs de quantité ne peuvent excéder 999 999 999 999,99 avec une précision maximale de 2 décimales (+ERO : pour prendre en compte les devises avec un cours très faible(ex 1 EUR = 1 384, 56 MMK)
            const double max = 999999999999.99;
            RuleFor(e => e.Recettes).InclusiveBetween(0, max).WithMessage("La recette doit être comprise entre 0 et " + max + ".");
            RuleFor(e => e.Depenses).InclusiveBetween(0, max).WithMessage("La dépence doit être comprise entre 0 et " + max + ".");
            RuleFor(e => e.MargeNette).InclusiveBetween(0, max).WithMessage("La marge nette doit être comprise entre 0 et " + max + ".");
            RuleFor(e => e.MargeBrute).InclusiveBetween(0, max).WithMessage("La marge brute doit être comprise entre 0 et " + max + ".");
            RuleFor(e => e.MargeNettePercent).InclusiveBetween(0, 100).WithMessage("Le pourcentage de marge nette doit être compris entre 0 et 100.");
            RuleFor(e => e.MargeBrutePercent).InclusiveBetween(0, 100).WithMessage("Le pourcentage de marge brute doit être compris entre 0 et 100.");

            // RG_1132_013: Validation du budget - Pour chaque devise affectée au C.I.dans FRED, un budget dans la même devise doit être créé.
            // Non vérifiable : Toujours le cas, et des tâches peuvent ne pas avoir été crées.
        }

        /// <summary>
        ///   Ajout des règles des éléments enfants pour une validation en cascade
        /// </summary>
        private void AddChildRules()
        {
            if (this.tacheValidator != null)
            {
                RuleFor(e => e.Taches)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                  .SetCollectionValidator(br => this.tacheValidator);
            }
        }




        /// <summary>
        /// Valide toutes les taches d'une révision en les récupérants de la base
        /// </summary>
        /// <param name="revision">révision à valider</param>
        /// <returns>ValidationResult avec la liste des erreurs</returns>
        private ValidationResult ValidateChangementDeStatut(BudgetRevisionEnt revision)
        {
            if (revision.Statut == (int)StatutBudget.AValider || revision.Statut == (int)StatutBudget.Valider)
            {
                var taches = this.budgetRepository.GetBudgetRevisionTachesLevel4(revision.BudgetRevisionId);
                var validatorStatut = new TacheValidatorStatut((StatutBudget)revision.Statut, taches);
                return validatorStatut.Validate();
            }

            return new ValidationResult();
        }


        /// <summary>
        ///   Valider la révision.
        /// </summary>
        /// <param name="instance">Revision à valider.</param>
        /// <returns>Résultat de validation</returns>
        public new ValidationResult Validate(BudgetRevisionEnt instance)
        {
            // 1)
            // validation de la révision et des tâches modifiées dans le front
            // s'il y a une erreur, elle est remontée pour que l'utilisateur la traite
            ValidationResult resultFront = base.Validate(instance);
            if (!resultFront.IsValid)
            {
                return resultFront;
            }

            // 2)
            // s'il n'y a pas d'erreurs, alors il faut valider les tâches en base en cas de statut != brouillon (en effet, elles ne sont pas envoyées par le front si elles n'ont pas été modifiées)
            return ValidateChangementDeStatut(instance);
        }
    }
}