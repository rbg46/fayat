using System.Linq;
using FluentValidation;
using Fred.Business.Referential.TypeRattachement;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Personnel;

namespace Fred.Business.Personnel
{
    /// <summary>
    ///   Valideur des Personnels
    /// </summary>
    public class PersonnelValidator : AbstractValidator<PersonnelEnt>, IPersonnelValidator
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="PersonnelValidator" />.
        /// </summary>
        /// <param name="personnelRepo">Interface Personnel Repository</param>
        public PersonnelValidator(IPersonnelRepository personnelRepo)
        {
            RuleFor(p => p.SocieteId).NotNull().WithMessage("La société est obligatoire.");

            RuleFor(p => p.Matricule).NotEmpty().NotNull().WithMessage("Le matricule est obligatoire.");

            RuleFor(p => p.Nom).NotEmpty().NotNull().WithMessage("Le nom est obligatoire.");

            RuleFor(p => p.Prenom).NotNull().NotEmpty().WithMessage("Le prénom est obligatoire.");

            RuleFor(p => p.DateEntree).NotNull().WithMessage("La date d'entrée est obligatoire.");

            RuleFor(p => p.RessourceId).NotNull().When(p => !p.IsInterimaire).WithMessage("La ressource est obligatoire.");

            //// RG_394_003: Les champs Etablissement de paie, Etablissement de rattachement et type de rattachement
            //// ne sont pas obligatoires pour le personnel externe      
            RuleFor(p => p.TypeRattachement).NotEmpty().NotNull().When(p => p.IsInterne).WithMessage("Le type de rattachement est obligatoire.");

            //// RG_394_003: Les champs Etablissement de paie, Etablissement de rattachement et type de rattachement
            //// ne sont pas obligatoires pour le personnel externe
            RuleFor(p => p.EtablissementPaieId).NotNull().When(p => p.IsInterne).WithMessage("L'établissement de paie est obligatoire.");

            //// RG_394_003: Les champs Etablissement de paie, Etablissement de rattachement et type de rattachement
            //// ne sont pas obligatoires pour le personnel externe
            RuleFor(p => p.EtablissementRattachementId).NotNull().When(p => p.IsInterne && p.TypeRattachement != TypeRattachement.Domicile).WithMessage("L'établissement de rattachement est obligatoire.");

            RuleFor(p => p).Must(p =>
            {
                var perso = personnelRepo.GetPersonnel(p.SocieteId.Value, p.Matricule);

                return perso == null || perso.PersonnelId == p.PersonnelId;
            })
            .WithMessage("Ce matricule existe déjà dans la société choisie.");

            // Test d'existance de l'email
            RuleFor(p => p).Must(p =>
            {
                //search
                var emailFound = personnelRepo.Query()
                    .Filter(s => !string.IsNullOrEmpty(s.Email))
                    .Filter(s => !s.PersonnelId.Equals(p.PersonnelId))
                    .Filter(s => s.Email.Equals(p.Email))
                    .Get()
                    .Any();
                //must not found
                return !emailFound;
            }).WithMessage("Veuillez saisir une adresse courriel qui n'est pas déjà utilisée");
        }
    }
}
