using System;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget.Avancement;

namespace Fred.Business.Budget.Avancement
{
    /// <summary>
    /// Gestionnaire de workflow d'avancement
    /// </summary>
    public class AvancementWorkflowManager : Manager<AvancementWorkflowEnt, IAvancementWorkflowRepository>, IAvancementWorkflowManager
    {
        public AvancementWorkflowManager(IUnitOfWork uow, IAvancementWorkflowRepository avancementWorkflowRepository)
        : base(uow, avancementWorkflowRepository)
        {
        }

        /// <inheritdoc />
        public void Add(AvancementEnt avancement, int etatCibleId, int utilisateurId, bool creation = false)
        {
            AvancementWorkflowEnt workflow;
            if (avancement.AvancementId != 0)
            {
                workflow = new AvancementWorkflowEnt()
                {
                    AuteurId = utilisateurId,
                    AvancementId = avancement.AvancementId,
                    Date = DateTime.Now,
                    EtatCibleId = etatCibleId
                };
            }
            else
            {
                workflow = new AvancementWorkflowEnt()
                {
                    AuteurId = utilisateurId,
                    Avancement = avancement,
                    Date = DateTime.Now,
                    EtatCibleId = etatCibleId
                };
            }

            if (!creation)
            {
                workflow.EtatInitialId = avancement.AvancementEtatId;
            }
            Repository.Insert(workflow);
        }
    }
}
