using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Fred.Business.OrganisationFeature;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business
{
    public class SocieteValidator : AbstractValidator<SocieteEnt>, ISocieteValidator
    {
        private readonly ISocieteRepository societeRepository;
        private readonly ITypeSocieteRepository typeSocieteRepository;
        private readonly IOrganisationRelatedFeatureService organisationRelatedFeatureService;

        public SocieteValidator(ISocieteRepository societeRepository, ITypeSocieteRepository typeSocieteRepository, IOrganisationRelatedFeatureService organisationRelatedFeatureService)
        {
            this.societeRepository = societeRepository;
            this.typeSocieteRepository = typeSocieteRepository;
            this.organisationRelatedFeatureService = organisationRelatedFeatureService;

            RuleFor(x => x).Must(x => !FindSocieteInterimaire(x.GroupeId)).When(x => x.IsInterimaire && x.SocieteId == 0).WithMessage(FeatureSociete.OnlyOneSocieteInterimaireParGroupe);

            RuleFor(x => x.Libelle).NotEmpty().NotNull().WithMessage(BusinessResources.LibelleObligatoire);

            RuleFor(x => x.Code).NotEmpty().NotNull().WithMessage(BusinessResources.CodeObligatoire);

            RuleFor(x => x).Must(IsDeviseDeReferenceValid).WithMessage(FeatureSociete.Societe_Controller_DeviseReferenceRequired_Erreur);
        }

        /// <summary>
        ///   Vérifie si un groupe ne possède pas déjà une société intérimaire
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <returns>Vrai si le groupe possède une société intérimaire, sinon faux</returns>
        private bool FindSocieteInterimaire(int groupeId)
        {
            return societeRepository.FindSocieteInterimaire(groupeId);
        }

        internal bool IsDeviseDeReferenceValid(SocieteEnt societe)
        {
            if (!IsSocietePartenaire(societe) && organisationRelatedFeatureService.IsEnabledForCurrentUser(OrganisationRelatedFeatureConstants.SocieteDeviseDeReferenceRequired, true))
            {
                return IsDeviseDeReferenceDefined(societe);
            }

            return true;
        }

        private bool IsSocietePartenaire(SocieteEnt societe)
        {
            if (societe.TypeSocieteId.HasValue)
            {
                Entities.TypeSocieteEnt typeSociete = typeSocieteRepository.FindById(societe.TypeSocieteId.Value);
                if (typeSociete != null && typeSociete.Code == Entities.Constantes.TypeSociete.Partenaire)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsDeviseDeReferenceDefined(SocieteEnt societe)
        {
            DeviseEnt deviseDeReference = null;
            if (societe.SocieteDevises != null)
            {
                deviseDeReference =  societe.SocieteDevises.FirstOrDefault(d => d.DeviseDeReference)?.Devise;
            }
            else
            {
                deviseDeReference = societeRepository.GetDeviseRefBySocieteId(societe.SocieteId);
            }

            return deviseDeReference != null;
        }

        /// <summary>
        ///   Valideur.
        /// </summary>
        /// <param name="instance">Instance à valider.</param>
        /// <returns>Résultat de validation</returns>
        public new ValidationResult Validate(SocieteEnt instance)
        {
            return base.Validate(instance);
        }
    }
}
