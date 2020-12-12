using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;

namespace Fred.Business.Commande
{
    /// <summary>
    /// Gestionaire des Statuts Commandes
    /// </summary>
    public class CommandeTypeManager : Manager<CommandeTypeEnt>, ICommandeTypeManager
    {
        public CommandeTypeManager(IUnitOfWork uow, IRepository<CommandeTypeEnt> commandeTypeRepo)
        : base(uow, commandeTypeRepo)
        {
        }

        /// <inheritdoc/>
        public CommandeTypeEnt Get(int commandeTypeId)
        {
            return Repository.Query().Get().FirstOrDefault(x => x.CommandeTypeId == commandeTypeId);
        }

        /// <inheritdoc/>
        public CommandeTypeEnt GetByCode(string code)
        {
            return Repository.Query().Get().FirstOrDefault(x => x.Code == code);
        }

        /// <inheritdoc />
        public List<CommandeTypeEnt> GetAll()
        {
            return Repository.Query().Get().ToList();
        }
    }
}
