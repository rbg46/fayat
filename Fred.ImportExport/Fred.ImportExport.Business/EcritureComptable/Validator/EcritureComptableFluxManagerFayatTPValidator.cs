using FluentValidation;
using FluentValidation.Results;
using Fred.ImportExport.Models.EcritureComptable;

namespace Fred.ImportExport.Business.EcritureComptable.Validator
{
    /// <summary>
    /// Validator des ecritures comptable Fayat TP
    /// </summary>
    public class EcritureComptableFluxManagerFayatTPValidator : AbstractValidator<EcritureComptableFayatTpModel>
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        public EcritureComptableFluxManagerFayatTPValidator()
        {
            Include(new MoPointeFayatTpValidator());
            Include(new AchatAvecCommandeFayatTpValidator());
            Include(new MaterielInternePointeFayatTpValidator());
            Include(new AutreDepenseFayatTpValidator());
            Include(new MaterielImmobiliseFayatTpValidator());
            Include(new RecettesFayatTpValidator());
        }

        /// <summary>
        /// Valide les RG pour la famille MO Pointé
        /// </summary>
        /// <param name="instance"><see cref="EcritureComptableFayatTpModel"/></param>
        /// <returns><see cref="ValidationResult"/></returns>
        public ValidationResult ValidateMoRules(EcritureComptableFayatTpModel instance)
        {
            MoPointeFayatTpValidator validator = new MoPointeFayatTpValidator();
            return validator.Validate(instance, ruleSet: "MoPointe");
        }

        /// <summary>
        /// Valide les RG pour la famille Achat avce commande
        /// </summary>
        /// <param name="instance"><see cref="EcritureComptableFayatTpModel"/></param>
        /// <returns><see cref="ValidationResult"/></returns>
        public ValidationResult ValidateAchatRules(EcritureComptableFayatTpModel instance)
        {
            AchatAvecCommandeFayatTpValidator validator = new AchatAvecCommandeFayatTpValidator();
            return validator.Validate(instance, ruleSet: "AchatAvecCommande");
        }

        /// <summary>
        /// Valide les RG pour la famille Materiel Interne Pointé
        /// </summary>
        /// <param name="instance"><see cref="EcritureComptableFayatTpModel"/></param>
        /// <returns><see cref="ValidationResult"/></returns>
        public ValidationResult ValidateMaterielInternePointeRules(EcritureComptableFayatTpModel instance)
        {
            MaterielInternePointeFayatTpValidator validator = new MaterielInternePointeFayatTpValidator();
            return validator.Validate(instance, ruleSet: "MaterielInternePointe");
        }

        /// <summary>
        /// Valide les RG pour la famille Autre dépense sans commande
        /// </summary>
        /// <param name="instance"><see cref="EcritureComptableFayatTpModel"/></param>
        /// <returns><see cref="ValidationResult"/></returns>
        public ValidationResult ValidateAutreDepensesRules(EcritureComptableFayatTpModel instance)
        {
            AutreDepenseFayatTpValidator validator = new AutreDepenseFayatTpValidator();
            return validator.Validate(instance, ruleSet: "AutreDepense");
        }

        /// <summary>
        /// Valide les RG pour la famille Materiel Immobilise
        /// </summary>
        /// <param name="instance"><see cref="EcritureComptableFayatTpModel"/></param>
        /// <returns><see cref="ValidationResult"/></returns>
        public ValidationResult ValidateMaterielImmobiliseRules(EcritureComptableFayatTpModel instance)
        {
            MaterielImmobiliseFayatTpValidator validator = new MaterielImmobiliseFayatTpValidator();
            return validator.Validate(instance, ruleSet: "MatrielImmobilise");
        }

        /// <summary>
        /// Valide les RG pour la famille Recettes
        /// </summary>
        /// <param name="instance"><see cref="EcritureComptableFayatTpModel"/></param>
        /// <returns><see cref="ValidationResult"/></returns>
        public ValidationResult ValidateRecetteRules(EcritureComptableFayatTpModel instance)
        {
            RecettesFayatTpValidator validator = new RecettesFayatTpValidator();
            return validator.Validate(instance, ruleSet: "Recettes");
        }
    }
}
