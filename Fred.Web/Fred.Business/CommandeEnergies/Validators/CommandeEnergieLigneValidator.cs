using FluentValidation;
using Fred.Entities.Commande;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.CommandeEnergies
{
    /// <summary>
    ///   Valideur des lignes de commande énergie
    /// </summary>
    public class CommandeEnergieLigneValidator : AbstractValidator<CommandeLigneEnt>, ICommandeEnergieLigneValidator
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="CommandeEnergieLigneValidator" />.
        /// </summary>        
        public CommandeEnergieLigneValidator()
        {
            RuleFor(l => l.Libelle).NotEmpty().WithMessage(FeatureCommande.CmdManager_Designation_Energie_Obligatoire);

            RuleFor(l => l.RessourceId).NotEmpty().WithMessage(FeatureCommande.CmdManager_Ressource_Energie_Obligatoire);

            RuleFor(l => l.TacheId).NotEmpty().WithMessage(FeatureCommande.CmdManager_Tache_Energie_Obligatoire);

            RuleFor(l => l.Quantite).GreaterThan(0.0M).WithMessage(FeatureCommande.CmdManager_Quantite_Energie_Obligatoire);

            RuleFor(l => l.UniteId).NotEmpty().WithMessage(FeatureCommande.CmdManager_Unite_Energie_Obligatoire);

            RuleFor(l => l.PUHT).GreaterThan(0.0M).WithMessage(FeatureCommande.CmdManager_PuHt_Energie_Obligatoire);
        }
    }
}
