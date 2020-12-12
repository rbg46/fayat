using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Fred.Business.Budget;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Recette;
using Fred.Entities.Referential;
using Fred.Framework.Extensions;

namespace Fred.Business.Referential.Tache
{
    /// <summary>
    ///   Implémentation de la validation d'une tache d'un budget
    /// </summary>
    public class TacheValidator : AbstractValidator<TacheEnt>, ITacheValidator
    {
        private readonly IRessourceTacheValidatorOld rtValidator;
        private readonly ITacheRecetteValidatorOld trvValidator;
        private readonly IRepository<BudgetRevisionEnt> budgetRevRepository;
        private readonly IBudgetRepositoryOld budgetRepository;
        private readonly ICIRepository ciRepository;

        /// <summary>
        ///   Constructeur
        /// </summary>
        /// <param name="rtv">Validator des Ressources taches pour une validation en cascade</param>
        /// <param name="trv">Validator des TachesRecettes</param>
        public TacheValidator(
            IRessourceTacheValidatorOld rtv,
            ITacheRecetteValidatorOld trv,
            IBudgetRepositoryOld budgetRepository,
            ICIRepository ciRepository,
            IRepository<BudgetRevisionEnt> budgetRevRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            this.rtValidator = rtv;
            this.trvValidator = trv;

            this.budgetRepository = budgetRepository;
            this.ciRepository = ciRepository;
            this.budgetRevRepository = budgetRevRepository;


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
            // hierarchie des niveaux
            RuleFor(e => e.Niveau)
              .Cascade(CascadeMode.StopOnFirstFailure)
              .Equal(1)
              .When(e => !e.ParentId.HasValue)
              .WithMessage(e => $"[{e.Code}] : Une tâche de premier niveau doit avoir un niveau égal à 1");

            RuleFor(e => e.Niveau)
              .Cascade(CascadeMode.StopOnFirstFailure)
              .Equal(e => e.Parent.Niveau + 1)
              .When(e => e.Parent != null)
              .When(e => e.ParentId.HasValue)
              .WithMessage(e => $"[{e.Code}] : Une tâche enfant doit être de niveau inférieur à la tâche parent.");
        }


        /// <summary>
        ///   Ajout des règles de validation métiers
        /// </summary>
        private void AddBusinessRules()
        {
            // temporaire, a activer une fois le code effectué
            //// RuleFor(t => t.DateCreation).NotNull().WithMessage(e => $"[{e.Code}] : La date de création doit être renseignée.");
            //// RuleFor(t => t.AuteurCreationId).NotNull().WithMessage(e => $"[{e.Code}] : L'auteur doit être renseigné.");
            //// RuleFor(t => t.BudgetRevision).NotNull().WithMessage(e => $"[{e.Code}] : Une tache doit appartenir à une révision.");


            // RGs non spécifiées mais implicite (ou spécifié dans une US...)
            RuleFor(e => e.Code).NotEmpty().WithMessage(e => $"[{e.Code}] : Le code d'une tâche est obligatoire.");
            RuleFor(e => e.Code).NotEmpty().WithMessage(e => $"[{e.Code}] : Le libellé d'une tâche est obligatoire.");
            RuleFor(e => e.Code).Length(0, 15).WithMessage(e => $"[{e.Code}] : Le code d'une tâche doit être inférieur à 15 caractères.");
            RuleFor(e => e.Niveau).InclusiveBetween(1, 4).WithMessage(e => $"[{e.Code}] : Le niveau d'une tache doit être compris entre 1 et 4.");
            //REWORK Data Budget : 
            //RuleFor(e => e.TypeAvancement).InclusiveBetween((int)TypeAvancementBudget.Quantite, (int)TypeAvancementBudget.Pourcentage).WithMessage(e => $"[{e.Code}] : Le type d'avancement doit être Quantité ou Pourcentage.")


            // RG_1132_001: Seules les tâches de niveau 4 peuvent être rattachées à des ressources
            RuleFor(e => e.RessourceTaches).Must(Validate_RG_1132_001).WithMessage(e => $"[{e.Code}] : Une ressource ne peut-être attachée qu'à une tâche de niveau 4.");


            // RG_1132_003: Les champs de quantité ne peuvent excéder 999 999 999 999,99 avec une précision maximale de 2 décimales (+ERO : pour prendre en compte les devises avec un cours très faible(ex 1 EUR = 1 384, 56 MMK)
            const double max = 999999999999.99;
            RuleFor(e => e.QuantiteARealise).InclusiveBetween(0, max).WithMessage(e => $"[{e.Code}] : La quantité à réaliser doit être comprise entre 0 et " + max + ".");
            RuleFor(e => e.QuantiteBase).InclusiveBetween(0, max).WithMessage(e => $"[{e.Code}] : La quantité de base doit être comprise entre 0 et " + max + ".");
            RuleFor(e => e.TotalHeureMO).InclusiveBetween(0, max).WithMessage(e => $"[{e.Code}] : Le total d'heure MO doit être compris entre 0 et " + max + ".");
            RuleFor(e => e.HeureMOUnite).InclusiveBetween(0, max).WithMessage(e => $"[{e.Code}] : Le nombre d'unité de l'heure MO doit être compris entre 0 et " + max + ".");
            //REWORK Data Budget : 
            //RuleFor(e => e.TotalHeureMOT4).InclusiveBetween(0, max).WithMessage(e => $"[{e.Code}] : Le total d'heure MO d'une tâche de niveau 4 doit être compris entre 0 et " + max + ".")
            //RuleFor(e => e.TotalT4).InclusiveBetween(0, max).WithMessage(e => $"[{e.Code}] : Le total d'une tâche de niveau 4 doit être compris entre 0 et " + max + ".")


            // RG_1132_004: Les champs de montant/ P.U.ne peuvent excéder 999 999 999 999,99 avec une précision maximale de 2 décimales
            RuleFor(e => e.PrixTotalQB).InclusiveBetween(0, max).WithMessage(e => $"[{e.Code}] : Le prix total QB doit être compris entre 0 et " + max + ".");
            RuleFor(e => e.PrixUnitaireQB).InclusiveBetween(0, max).WithMessage(e => $"[{e.Code}] : Le prix unitaire QB doit être compris entre 0 et " + max + ".");
            //REWORK Data Budget : 
            //RuleFor(e => e.PrixUnitaireT4).InclusiveBetween(0, max).WithMessage(e => $"[{e.Code}] : Le le prix unitaire d'une tâche de niveau 4 doit être compris entre 0 et " + max + ".")


            // RG_1132_012: Validation du budget - Une affectation de ressource à chaque tâche de niveau 4 est indispensable avant le changement de statut "à valider" ou "validé"
            RuleFor(e => e.RessourceTaches).Must(Validate_RG_1132_012).WithMessage(e => $"[{e.Code}] : Une ressource doit être affectée à une tâche de niveau 4 pour les statuts A Valider et Validé.");

            // RG_1132_032 : Somme des recettes
            RuleFor(e => e.TacheRecettes).Must(Validate_RG_1132_032).WithMessage(e => $"[{e.Code}] : La somme des recettes n'est pas valide.");

            // RG_1132_041 : Valorisation : Le niveau d'avancement peut se faire sur n'importe quel niveau de tache : De T1 à T3 = en % - Pour T4 = en qté ou % : qté par défaut.
            //REWORK Data Budget : 
            //RuleFor(e => e.TypeAvancement)
            //  .Equal((int)TypeAvancementBudget.Pourcentage)
            //  .When(e => e.Niveau < 4)
            //  .When(e => e.TypeAvancement.HasValue)
            //  .WithMessage(e => $"[{e.Code}] : Une tâche de niveau 1 2 ou 3 doit avoir un type d'avancement en pourcentage.")
        }

        /// <summary>
        ///   Vérifie la règle RG_1132_012: Validation du budget - Une affectation de ressource à chaque tâche de niveau 4 est
        ///   indispensable avant le changement de statut "à valider" ou "validé"
        /// </summary>
        /// <param name="tacheEnt">La tâche à tester</param>
        /// <param name="ressourceTacheEnts">Liste des ressources-tâches de cette tâche</param>
        /// <returns>vrai si la règle est vérifiée, sinon faux </returns>
        private bool Validate_RG_1132_012(TacheEnt tacheEnt, ICollection<RessourceTacheEnt> ressourceTacheEnts)
        {
            //REWORK Data Budget : 
            //BudgetRevisionEnt revision = tacheEnt.BudgetRevision ?? this.budgetRevRepository.FindById(tacheEnt.BudgetRevisionId)

            //Si revision null return true

            //return TacheValidatorStatut.ValidateUs1132Rg012(tacheEnt, (StatutBudget)revision.Statut)
            return true;
        }

        /// <summary>
        ///   Vérifie la règle RG_1132_001 : Seules les tâches de niveau 4 peuvent être rattachées à des ressources
        /// </summary>
        /// <param name="tacheEnt">La tâche à tester</param>
        /// <param name="ressourceTacheEnts">Liste des ressources-tâches de cette tâche</param>
        /// <returns>vrai si la règle est vérifiée, sinon faux </returns>
        private bool Validate_RG_1132_001(TacheEnt tacheEnt, ICollection<RessourceTacheEnt> ressourceTacheEnts)
        {
            if (tacheEnt == null)
            {
                return false;
            }

            if (tacheEnt.Niveau != 4)
            {
                if (ressourceTacheEnts == null)
                {
                    return true;
                }

                if (ressourceTacheEnts.Any())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///   Calcule la recette total pour une devise donnée
        /// </summary>
        /// <param name="recettes">Liste de recette</param>
        /// <param name="deviseId">Identifiant de la devise</param>
        /// <returns>somme des recettes pour la devise donnée</returns>
        private double GetTotalRecette(ICollection<TacheRecetteEnt> recettes, int deviseId)
        {
            if (recettes == null)
            {
                return 0.0;
            }

            var result = from recette in recettes
                         where recette.DeviseId == deviseId
                         where recette.Recette.HasValue
                         select recette.Recette.Value;

            return result.Sum();
        }

        /// <summary>
        ///   Retourne la devise de référence d'une tâche
        /// </summary>
        /// <param name="tache">la tâche</param>
        /// <returns>Entité Devise</returns>
        private DeviseEnt GetBudgetDeviseReference(TacheEnt tache)
        {
            // todo : a mettre en cache

            if (tache == null || this.budgetRevRepository == null || this.budgetRepository == null)
            {
                return null;
            }

            BudgetEnt budget = this.budgetRepository.FindById(0);
            if (budget == null)
            {
                return null;
            }

            return this.ciRepository.GetDeviseRef(budget.CiId);
        }

        /// <summary>
        ///   Règle de validation pour RG_1132_032
        ///   "RG_1132_032 : Recette : Les montants inférieurs s'additionneront pour calculer le montant du niveau supérieur,
        ///   si un niveau supérieur est saisi les niveaux inférieurs sont remis à 0."
        /// </summary>
        /// <param name="tache">Tâche à valider</param>
        /// <param name="tacheRecettes">liste des recettes de la tâche</param>
        /// <returns>true si la validation est ok, sinon false</returns>
        private bool Validate_RG_1132_032(TacheEnt tache, ICollection<TacheRecetteEnt> tacheRecettes)
        {
            // On peut traduire la règle de gestion par la phrase suivante : 
            // Pour la même devise, la recette du parent doit être égale à la somme des recettes des enfants sauf si tous les enfants sont à zero

            // 0) Vérification
            if (tache == null)
            {
                return false;
            }

            if (tacheRecettes == null || tacheRecettes.Any() == false)
            {
                return true; // il n'y a pas de recettes sur cette tâche, donc la RG ne peut-être validée, donc c'est ok.
            }

            if (tache.TachesEnfants == null || tache.TachesEnfants.Any() == false)
            {
                return true; // il n'y a pas de tâches enfants sur cette tâche, donc c'est forcement ok.
            }

            // 1) récupération de la devise de référence
            DeviseEnt deviseRef = GetBudgetDeviseReference(tache);
            if (deviseRef == null)
            {
                // Impossible de récupérer la devise de référence.
                return false;
            }

            // 2) calcul de la recette de cette tâche
            double recetteParent = GetTotalRecette(tacheRecettes, deviseRef.DeviseId);

            // 3) calcul de la recette des tâches enfants
            double recetteEnfant = tache.TachesEnfants.Sum(enfant => GetTotalRecette(enfant.TacheRecettes, deviseRef.DeviseId));

            // 4) si tous les enfants sont à zero, alors on ne vérifie pas la recette.
            if (recetteEnfant.IsZero())
            {
                return true;
            }

            // 5) si les enfants ne sont pas à zéro, alors la somme des enfants doit être égale à la somme du parent
            if (recetteEnfant.IsEqual(recetteParent))
            {
                return true;
            }

            // 6) nous sommes dans aucun des cas passants, donc il y a une erreur
            return false;
        }

        /// <summary>
        ///   Ajout des règles des éléments enfants pour une validation en cascade
        /// </summary>
        private void AddChildRules()
        {
            if (this.rtValidator != null)
            {
                RuleFor(t => t.RessourceTaches)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                  .SetCollectionValidator(t => this.rtValidator);
            }

            if (this.trvValidator != null)
            {
                RuleFor(t => t.TacheRecettes)
                  .Cascade(CascadeMode.StopOnFirstFailure)
                  .SetCollectionValidator(t => this.trvValidator);
            }

            // attention : récursif !
            // non appelé dans la classe enfant TacheValidatorStatut
            ////RuleFor(t => t.TachesEnfants)
            ////  .Cascade(CascadeMode.StopOnFirstFailure)
            ////  .SetCollectionValidator(this);
        }

        /// <summary>
        ///   Valider une tâche.
        /// </summary>
        /// <param name="instance">Tâche à valider.</param>
        /// <returns>Résultat de validation</returns>
        public new ValidationResult Validate(TacheEnt instance)
        {
            // rien de spécial ici pour le moment.
            return base.Validate(instance);
        }
    }
}
