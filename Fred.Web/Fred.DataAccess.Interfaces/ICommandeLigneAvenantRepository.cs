
using Fred.Entities.Commande;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Représente un référentiel de données pour les lignes d'avenant de commande.
    /// </summary>
    public interface ICommandeLigneAvenantRepository : IRepository<CommandeLigneAvenantEnt>
    {

        /// <summary>
        /// Suppression d'une ligne d'avenant
        /// </summary>
        /// <param name="ligneAvenant">ligne d'avenant</param>
        void DeleteCommandeLigneAvenant(CommandeLigneAvenantEnt ligneAvenant);

    }
}
