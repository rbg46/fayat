using FluentValidation;
using Fred.Business.Utilisateur;
using Fred.Entities.Depense;

namespace Fred.Business.Depense
{
    /// <summary>
    ///   Valideur des Dépenses
    /// </summary>
    public class DepenseValidator : AbstractValidator<DepenseAchatEnt>, IDepenseValidator
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="IDepenseValidator" />.
        /// </summary>
        /// <param name="usrManager">Gestionnaire des utilisateurs.</param>    
        public DepenseValidator(IUtilisateurManager usrManager)
        {
            // Le CI est obligatoire
            RuleFor(c => c.CiId).NotNull().WithMessage(BusinessResources.CIObligatoire);

            // La date est obligatoire
            RuleFor(c => c.Date).NotNull().WithMessage(DepenseResources.DateDepenseObligatoire);

            // Le libellé est obligatoire
            RuleFor(c => c.Libelle).NotEmpty().WithMessage(BusinessResources.LibelleObligatoire);

            // Le Fournisseur est obligatoire
            RuleFor(c => c.FournisseurId).NotEmpty().WithMessage(BusinessResources.FournisseurObligatoire);

            // Le numéro de bon de livraison est obligatoire
            // TSA : Commenté pour l'US 52, Faire un point avec BA pour connaitre les bonnes RG de validation d'une dépense
            //// RuleFor(c => c.NumeroBL).NotEmpty().WithMessage(DepenseResources.NumBonLivraisonObligatoire);

            // La ressource est obligatoire
            RuleFor(c => c.RessourceId).NotEmpty().WithMessage(BusinessResources.RessourceObligatoire);

            // La tâche est obligatoire
            RuleFor(c => c.TacheId).NotEmpty().WithMessage(BusinessResources.TacheObligatoire);

            // La devise est obligatoire
            RuleFor(c => c.DeviseId).NotEmpty().WithMessage(BusinessResources.DeviseObligatoire);

            // L'unité est obligatoire
            RuleFor(c => c.UniteId).NotEmpty().WithMessage(BusinessResources.UniteObligatoire);
        }
    }
}
