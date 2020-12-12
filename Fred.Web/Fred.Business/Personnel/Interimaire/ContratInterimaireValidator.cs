using System.Linq;
using FluentValidation;
using Fred.Business.FeatureFlipping;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Referential;
using Fred.Framework.FeatureFlipping;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.Personnel.Interimaire
{
    /// <summary>
    ///     Validator des contrats intérimaire
    /// </summary>
    public class ContratInterimaireValidator : AbstractValidator<ContratInterimaireEnt>, IContratInterimaireValidator
    {
        private readonly IFournisseurRepository fournisseurRepository;
        private readonly IFeatureFlippingManager featureFlippingManager;

        public ContratInterimaireValidator(IFeatureFlippingManager featureFlippingManager, IFournisseurRepository fournisseurRepository)
        {
            this.fournisseurRepository = fournisseurRepository;
            this.featureFlippingManager = featureFlippingManager;

            AddBusinessRules();
        }

        /// <summary>
        /// Règles de gestion métiers
        /// </summary>
        private void AddBusinessRules()
        {
            RuleFor(c => c).Custom((c, context) =>
            {
                if (this.featureFlippingManager.IsActivated(EnumFeatureFlipping.BlocageFournisseursSansSIRET))
                {
                    FournisseurEnt f = fournisseurRepository.Query().Get().FirstOrDefault(x => x.FournisseurId == c.FournisseurId);
                    int siren;
                    bool isNumber = int.TryParse(f?.SIREN, out siren);
                    bool isSirenEmptyOrZero = string.IsNullOrEmpty(f?.SIREN) || (isNumber && siren == 0);
                    if (isSirenEmptyOrZero)
                        context.AddFailure("Fournisseur", FeatureCommande.CmdManager_FournisseurETTSirenObligatoire);
                }
            });
        }
    }
}
