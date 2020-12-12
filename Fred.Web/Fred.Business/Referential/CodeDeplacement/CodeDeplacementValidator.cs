using FluentValidation;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;

namespace Fred.Business.Referential.CodeDeplacement
{
    /// <summary>
    ///   Valideur des Codes Deplacement
    /// </summary>
    public class CodeDeplacementValidator : AbstractValidator<CodeDeplacementEnt>, ICodeDeplacementValidator
    {
        private static string errorKey = "serverValidation.CodeDeplacement_CodeExistInSociete";


        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="CodeDeplacementValidator"/>.
        /// </summary>   
        /// <param name="uow">Repository des Unit of Work.</param>
        /// <param name="codeDeplacementRepository">Repository des Code deplacement</param>
        public CodeDeplacementValidator(IUnitOfWork uow, ICodeDeplacementRepository codeDeplacementRepository)
        {
            //Validation des contraintes de la base
            RuleFor(codeDeplacement => codeDeplacement.Code)
              .Length(0, 20)
              .WithMessage(CodeDeplacementResources.CodeDeplacement_CodeMaxLenghtValidation)
              .NotEmpty()
              .WithMessage(BusinessResources.CodeObligatoire);

            RuleFor(codeDeplacement => codeDeplacement.Libelle)
              .Length(0, 500)
              .WithMessage(CodeDeplacementResources.CodeDeplacement_LibelleMaxLenghtValidation)
              .NotEmpty()
              .WithMessage(BusinessResources.LibelleObligatoire);

            //Validation des regles metiers 
            RuleFor(codeDeplacement => codeDeplacement.KmMini)
              .GreaterThanOrEqualTo(codeDeplacement => 0)
              .WithMessage(CodeDeplacementResources.CodeDeplacement_kmMiniValidation)
              .Must((cd, kmin) => kmin < cd.KmMaxi)
              .WithMessage(CodeDeplacementResources.CodeDeplacement_KmMiniSuppToKmMaxi);

            RuleFor(codeDeplacement => codeDeplacement.KmMaxi)
              .LessThanOrEqualTo(codeDeplacement => 9999)
              .WithMessage(CodeDeplacementResources.CodeDeplacement_kmMaxiValidation);

            //////////////////////////////////////////////////////////////////////////////////////////
            //CONVENTION DE NOMMAGE IMPORTANTE !
            //////////////////////////////////////////////////////////////////////////////////////////
            //Toute validation, qui ne peut etre effectuée que par le serveur, doit avoir un identifiant commencant par 'serverValidation'
            //Ceci est fait pour gerer un affichage particulier coté front.
            RuleFor(c => c).Custom((codeDeplacement, context) =>
            {
                if (codeDeplacementRepository.CodeDeplacementExistForSocieteIdAndCode(codeDeplacement.CodeDeplacementId, codeDeplacement.Code, codeDeplacement.SocieteId))
                    context.AddFailure(CodeDeplacementValidator.CodeExistInSocieteErrorKey, CodeDeplacementResources.CodeDeplacement_CodeExistInSociete);
            });
        }

        /// <summary>
        /// Clé d'erreur lorsque le code existe deja pour un autre code deplacement et pour une meme société. 
        /// Get renvoie la clef d'erreur
        /// Set ne fait rien, car la clef d'erreur ne doit pas être modifiée.
        /// Cependant, on ne peut pas la mettre private ou readonly car le test Verify_CodeExistInSocieteError_When_Already_Code_Exist_For_the_same_compagnie_Error
        /// a besoin d'un setter. Le trick est donc d'assigner value à la valeur de la clef pour empêcher toute modification de cette variable malgres qu'elle soit public. 
        /// </summary>
        public static string CodeExistInSocieteErrorKey
        {
            get { return errorKey; }
            set { value = errorKey; errorKey = value; }
        }
    }
}