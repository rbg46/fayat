using System.Collections.Generic;
using Fred.Entities.Depense;

namespace Fred.Business.Depense.Models
{
    public class GroupeRemplacementTacheHistory
    {

        public GroupeRemplacementTacheHistory(int groupeRemplacementTacheId, List<RemplacementTacheEnt> historyOfGroupeRemplacementTache)
        {
            GroupeRemplacementTacheId = groupeRemplacementTacheId;
            HistoryOfGroupeRemplacementTache = historyOfGroupeRemplacementTache;
        }

        public int GroupeRemplacementTacheId { get; private set; }

        public List<RemplacementTacheEnt> HistoryOfGroupeRemplacementTache { get; private set; }

    }
}
