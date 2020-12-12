using System.Collections.Generic;
using Fred.ImportExport.DataAccess.WorkflowLogicielTiers;

namespace Fred.ImportExport.Business.WorkflowLogicielTiers
{

    public class WorkflowLogicielTiersManager : IWorkflowLogicielTiersManager
    {
        private readonly IWorkflowLogicielTiersRepository workflowLogicielTiersRepository;

        public WorkflowLogicielTiersManager(IWorkflowLogicielTiersRepository workflowLogicielTiersRepository)
        {
            this.workflowLogicielTiersRepository = workflowLogicielTiersRepository;
        }

        public IEnumerable<int> GetRapportLigneIdDejaEnvoye(int logicielTiersId, string flux)
        {
            return workflowLogicielTiersRepository.GetRapportLigneIdDejaEnvoye(logicielTiersId, flux);
        }

        public void SaveWorkflowLogicielTiers(IEnumerable<int> rapportLignesId, int logicielTiersId, int auteurId, string flux)
        {
            workflowLogicielTiersRepository.SaveWorkflowLogicielTiers(rapportLignesId, logicielTiersId, auteurId, flux);
        }
    }
}
