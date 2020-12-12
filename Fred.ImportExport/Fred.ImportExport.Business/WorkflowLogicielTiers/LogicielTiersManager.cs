using Fred.ImportExport.DataAccess.WorkflowLogicielTiers;
using Fred.ImportExport.Entities;

namespace Fred.ImportExport.Business.WorkflowLogicielTiers
{
    public class LogicielTiersManager : ILogicielTiersManager
    {
        private readonly ILogicielTiersRepository logicielTiersRepository;

        public LogicielTiersManager(ILogicielTiersRepository logicielTiersRepository)
        {
            this.logicielTiersRepository = logicielTiersRepository;
        }


        public LogicielTiersEnt GetOrCreateLogicielTiers(string nomLogiciel, string nomServeur, string mandant)
        {
            var logiciel = logicielTiersRepository.GetLogicielTiers(nomLogiciel, nomServeur, mandant);
            if (logiciel == null)
            {
                logiciel = logicielTiersRepository.CreateLogicielTiers(nomLogiciel, nomServeur, mandant);
            }

            return logiciel;
        }
    }
}
