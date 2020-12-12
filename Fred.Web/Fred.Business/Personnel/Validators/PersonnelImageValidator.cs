using FluentValidation;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Framework.Images;

namespace Fred.Business.Personnel
{
    /// <summary>
    ///   Valideur des images du Personnels
    /// </summary>
    public class PersonnelImageValidator : AbstractValidator<PersonnelImageEnt>, IPersonnelImageValidator
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="PersonnelValidator" />.
        /// </summary>
        public PersonnelImageValidator(IPersonnelImageRepository personnelImageRepository)
        {
            RuleFor(x => x.PersonnelId)
              .NotNull().NotEmpty().WithMessage("L'identifiant du personnel est obligatoire.")
              .Must(x =>
              {
                  var persoImg = personnelImageRepository.Get(x);
                  return persoImg == null || persoImg?.PersonnelImageId > 0;
              }).WithMessage("Une personnel ne peut avoir qu'une seule relation PersonnelImage.");

            // RG dimension Photo de profil
            RuleFor(x => x.PhotoProfil).Must(x =>
            {
                return x.CheckImageSize(200, 200);
            })
            .When(x => x.PhotoProfil?.Length > 0)
            .WithMessage("La dimension maximale de la photo de profil est de 200 x 200 pixels.")
            .Must(x =>
            {
                var sizeInKo = x.Length / 1024;
                return sizeInKo <= 100;
            })
            .When(x => x.PhotoProfil?.Length > 0)
            .WithMessage("La taille maximale de l'image doit être de 100 ko.");

            // RG Dimension Signature
            RuleFor(x => x.Signature).Must(x =>
            {
                return x.CheckImageSize(500, 200);
            })
            .When(x => x.Signature?.Length > 0)
            .WithMessage("La dimension maximale de la photo de profil est de 500 x 200 pixels.")
            .Must(x =>
            {
                var sizeInKo = x.Length / 1024;
                return sizeInKo <= 100;
            })
            .When(x => x.Signature?.Length > 0)
            .WithMessage("La taille maximale de l'image doit être de 100 ko.");
        }
    }
}