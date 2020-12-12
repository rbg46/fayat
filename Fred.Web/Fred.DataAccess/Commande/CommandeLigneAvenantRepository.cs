using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Fred.EntityFramework;
using Fred.Framework;

namespace Fred.DataAccess.Commande
{
    /// <summary>
    /// Représente un référentiel de données pour les lignes d'avenant de commande.
    /// </summary>
    public class CommandeLigneAvenantRepository : FredRepository<CommandeLigneAvenantEnt>, ICommandeLigneAvenantRepository
    {

        /// <summary>
        /// Context fred
        /// </summary>
        private readonly FredDbContext context;


        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="CommandeLigneAvenantRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        /// <param name="context">context</param>
        public CommandeLigneAvenantRepository(FredDbContext context)
          : base(context)
        {
            this.context = context;
        }


        /// <summary>
        /// Suppression d'une ligne d'avenant
        /// </summary>
        /// <param name="ligneAvenant">ligne d'avenant</param>
        public void DeleteCommandeLigneAvenant(CommandeLigneAvenantEnt ligneAvenant)
        {
            context.Remove(ligneAvenant);
        }
    }
}
