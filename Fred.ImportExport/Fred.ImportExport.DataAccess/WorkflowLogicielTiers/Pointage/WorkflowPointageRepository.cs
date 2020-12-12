using System.Collections.Generic;
using System.Linq;
using Fred.ImportExport.DataAccess.Common;
using Fred.ImportExport.DataAccess.Interfaces;
using Fred.ImportExport.Database.ImportExport;
using Fred.ImportExport.Entities;

namespace Fred.ImportExport.DataAccess.WorkflowLogicielTiers
{
    public class WorkflowPointageRepository : AbstractRepository<WorkflowPointageEnt>, IWorkflowPointageRepository
    {
        private readonly IUnitOfWork unitOfWork;

        public WorkflowPointageRepository(IUnitOfWork unitOfWork, ImportExportContext context)
            : base(context)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<int> GetRapportLigneIdDejaEnvoyeALogiciel(int logicielTiersId, string fluxName)
        {
            return Get().Where(w => w.LogicielTiersId == logicielTiersId && w.FluxName == fluxName).Select(w => w.RapportLigneId);
        }

        public List<WorkflowPointageEnt> GetRapportLigneWorkflowPointages(List<int> rapportLigneIdList, int logicielTiersId, string fluxName)
        {
            return Get().Where(w => w.LogicielTiersId == logicielTiersId
                            && w.FluxName == fluxName
                            && rapportLigneIdList.Contains(w.RapportLigneId)).ToList();

        }

        public void SaveWorkflowPointage(List<WorkflowPointageEnt> workflowPointageList)
        {
            workflowPointageList.ForEach(worflow => Add(worflow));
            unitOfWork.Save();
        }
    }


}
