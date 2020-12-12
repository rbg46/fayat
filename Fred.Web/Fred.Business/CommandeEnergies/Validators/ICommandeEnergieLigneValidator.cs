using FluentValidation;
using Fred.Entities.Commande;

namespace Fred.Business.CommandeEnergies
{
    /// <summary>
    ///   Interface du valideur des lignes de commande énergie
    /// </summary>
    public interface ICommandeEnergieLigneValidator : IValidator<CommandeLigneEnt>
    {
    }
}
