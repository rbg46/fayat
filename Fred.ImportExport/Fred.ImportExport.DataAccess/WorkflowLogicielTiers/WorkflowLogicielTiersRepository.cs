using System;
using System.Collections.Generic;
using System.Linq;
using Fred.ImportExport.DataAccess.Common;
using Fred.ImportExport.DataAccess.Interfaces;
using Fred.ImportExport.Database.ImportExport;
using Fred.ImportExport.Entities;

namespace Fred.ImportExport.DataAccess.WorkflowLogicielTiers
{
    public class WorkflowLogicielTiersRepository : AbstractRepository<WorkflowLogicielTiersEnt>, IWorkflowLogicielTiersRepository
    {
        private readonly IUnitOfWork unitOfWork;

        public WorkflowLogicielTiersRepository(IUnitOfWork unitOfWork, ImportExportContext context)
            : base(context)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<int> GetRapportLigneIdDejaEnvoye(int logicielTiersId, string flux)
        {
            return GetWorkflowEnvoyeALogiciel(logicielTiersId, flux).Select(w => w.RapportLigneId);
        }


        public void SaveWorkflowLogicielTiers(IEnumerable<int> rapportLignesId, int logicielTiersId, int auteurId, string flux)
        {
            var workflows = GetWorkflowEnvoyeALogiciel(logicielTiersId, flux);

            var lookup = rapportLignesId.ToLookup(rl => !workflows.Any(w => w.RapportLigneId == rl && w.FluxName == flux));

            var nouvellesLignesJamaisEnvoyee = lookup[true];
            var lignesDejaEnvoyee = lookup[false];

            foreach (var ligneRapportId in nouvellesLignesJamaisEnvoyee)
            {
                Add(new WorkflowLogicielTiersEnt
                {
                    AuteurId = auteurId,
                    LogicielTiersId = logicielTiersId,
                    RapportLigneId = ligneRapportId,
                    Date = DateTime.UtcNow,
                    FluxName = flux
                });
            }

            foreach (var ligneRapportId in lignesDejaEnvoyee)
            {
                var workflow = workflows.Single(w => w.RapportLigneId == ligneRapportId);
                workflow.AuteurId = auteurId;
                workflow.Date = DateTime.UtcNow;

                Update(workflow);
            }


            unitOfWork.Save();
        }

        private IEnumerable<WorkflowLogicielTiersEnt> GetWorkflowEnvoyeALogiciel(int logicielTiersId, string flux)
        {
            return Get().Where(w => w.LogicielTiersId == logicielTiersId && w.FluxName == flux);
        }
    }


}
