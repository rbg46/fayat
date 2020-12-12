using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;

namespace Fred.Business.Commande
{
    /// <summary>
    /// Gestionaire des Statuts Commandes
    /// </summary>
    public class StatutCommandeManager : Manager<StatutCommandeEnt>, IStatutCommandeManager
    {
        public StatutCommandeManager(IUnitOfWork uow, IRepository<StatutCommandeEnt> statutCommandeRepo)
        : base(uow, statutCommandeRepo)
        {
        }

        /// <inheritdoc/>
        public StatutCommandeEnt Get(int statutCommandeId)
        {
            return Repository.Query().Get().FirstOrDefault(x => x.StatutCommandeId == statutCommandeId);
        }

        /// <inheritdoc/>
        public StatutCommandeEnt GetByCode(string code)
        {
            return Repository.Query().Get().FirstOrDefault(x => x.Code == code);
        }

        /// <inheritdoc />
        public List<StatutCommandeEnt> GetAll()
        {
            return Repository.Query().Get().ToList();
        }
    }
}
