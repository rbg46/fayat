using FluentValidation;
using Fred.Web.Shared.Models.Budget.BibliothequePrix;

namespace Fred.Business.Budget.BibliothequePrix.Validator
{
    /// <summary>
    ///   Valideur des Bibliotheques de Prix
    /// </summary>
    public class BibliothequePrixSaveModelValidator : AbstractValidator<BibliothequePrixSave.Model>, IBibliothequePrixModelLoaderValidator
    {

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="BibliothequePrixSaveModelValidator" />
        /// </summary>    
        public BibliothequePrixSaveModelValidator()
        {
            RuleForEach(m => m.Items).NotNull()
                .WithMessage("Le Model doit contenir des items");
            RuleForEach(m => m.Items).Must(i => (i.Prix != null && i.UniteId != null) || (i.Prix == null && i.UniteId == null))
                .WithMessage("Prix et Unité doivent être renseignés tous les deux ou doivent être vides");

        }
    }
}
