using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Depense;
using Fred.EntityFramework;
using Fred.Framework;


namespace Fred.DataAccess.Depense
{
    /// <summary>
    ///   Référentiel de données pour les dépenses.
    /// </summary>
    public class GroupeRemplacementTacheRepository : FredRepository<GroupeRemplacementTacheEnt>, IGroupeRemplacementTacheRepository
    {
        private readonly ILogManager logManager;

        public GroupeRemplacementTacheRepository(FredDbContext context, ILogManager logManager)
          : base(context)
        {
            this.logManager = logManager;
        }

        /// <inheritdoc />
        public GroupeRemplacementTacheEnt AddGroupeRemplacementTache(GroupeRemplacementTacheEnt groupe)
        {
            Insert(groupe);

            return groupe;
        }

        /// <inheritdoc />
        public void DeleteGroupeRemplacementTacheById(int groupeId)
        {
            GroupeRemplacementTacheEnt groupe = Context.GroupeRemplacementTache.Find(groupeId);

            if (groupe == null)
            {
                System.Data.DataException objectNotFoundException = new System.Data.DataException();
                this.logManager.TraceException(objectNotFoundException.Message, objectNotFoundException);
                throw objectNotFoundException;
            }

            Delete(groupe);
        }

        /// <inheritdoc />
        public GroupeRemplacementTacheEnt GetGroupeRemplacementTacheById(int groupeId)
        {
            return Context.GroupeRemplacementTache
                          .SingleOrDefault(r => r.GroupeRemplacementTacheId.Equals(groupeId));
        }

    }
}