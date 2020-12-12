using FluentValidation;
using Fred.ImportExport.Models.EcritureComptable;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.ImportExport.Business.EcritureComptable.Validator
{
    public class MaterielImmobiliseFayatTpValidator : AbstractValidator<EcritureComptableFayatTpModel>
    {
        public MaterielImmobiliseFayatTpValidator()
        {
            RuleSet("MatrielImmobilise", () =>
            {
                RuleFor(x => x.DateCreation).NotEmpty().WithMessage(FeatureEcritureComptable.EcritureCompable_Erreur_DateCreation_Obligatoire);
                RuleFor(x => x.DateComptable).NotEmpty().WithMessage(FeatureEcritureComptable.EcritureCompable_Erreur_DateComptable_Obligatoire);
                RuleFor(x => x.Libelle).NotNull().NotEmpty().WithMessage(FeatureEcritureComptable.EcritureCompable_Erreur_Libelle_Obligatoire);
                RuleFor(x => x.NatureAnalytique).NotNull().NotEmpty().WithMessage(FeatureEcritureComptable.EcritureComptable_Erreur_CodeNatureAnalytique_Obligatoire);
                RuleFor(x => x.MontantDeviseInterne).NotEmpty().WithMessage(FeatureEcritureComptable.EcritureComptable_Erreur_MontantDeviseInterne_Obligatoire);
                RuleFor(x => x.DeviseInterne).NotNull().NotEmpty().WithMessage(FeatureEcritureComptable.EcritureComptable_Erreur_DeviseInterne_Obligatoire);
                RuleFor(x => x.MontantDeviseTransaction).NotEmpty().WithMessage(FeatureEcritureComptable.EcritureComptable_Erreur_MontantTransactionDevise_Obligatoire);
                RuleFor(x => x.DeviseTransaction).NotNull().NotEmpty().WithMessage(FeatureEcritureComptable.Ecriture_Comptable_Erreur_DeviseTransaction_Obligatoire);
                RuleFor(x => x.CiCode).NotNull().NotEmpty().WithMessage(FeatureEcritureComptable.Ecriture_Comptable_Erreur_CI_Obligatoire);
                RuleFor(x => x.NumeroPiece).NotNull().NotEmpty().WithMessage(FeatureEcritureComptable.Ecriture_Comptable_Erreur_NumeroPiece_Obligatoire);
                RuleFor(x => x.NumeroCommande).Null().WithMessage(FeatureEcritureComptable.Ecriture_Comptable_Erreur_NumeroCommande_Vide);
                RuleFor(x => x.RapportLigneId).Null().WithMessage(FeatureEcritureComptable.Ecriture_Comptable_Erreur_NumeroLigneDePointage_Vide);
            });
        }
    }
}
