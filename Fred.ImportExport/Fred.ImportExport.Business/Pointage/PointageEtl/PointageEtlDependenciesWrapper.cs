using Fred.Business.CI;
using Fred.Business.Personnel;
using Fred.Business.Personnel.Interimaire;
using Fred.Business.Rapport.Pointage.FredIe;
using Fred.Business.Societe;
using Fred.ImportExport.Business.ApplicationSap;
using Fred.ImportExport.Business.WorkflowLogicielTiers;

namespace Fred.ImportExport.Business.Pointage.PointageEtl
{
    /// <summary>
    /// contient les dependances pour l'import
    /// </summary>
    public class PointageEtlDependenciesWrapper
    {
        /// <summary>
        /// PointageManager
        /// </summary>
        public IFredIePointageFluxService FredIePointageFluxService { get; set; }

        /// <summary>
        /// CIManager
        /// </summary>
        public ICIManager CIManager { get; set; }

        /// <summary>
        /// MatriculeExterneManager
        /// </summary>
        public IMatriculeExterneManager MatriculeExterneManager { get; set; }

        /// <summary>
        /// PersonnelManager
        /// </summary>
        public IPersonnelManager PersonnelManager { get; set; }

        /// <summary>
        /// SocieteManager
        /// </summary>
        public ISocieteManager SocieteManager { get; set; }

        /// <summary>
        /// ApplicationsSapManager
        /// </summary>
        public IApplicationsSapManager ApplicationsSapManager { get; set; }

        /// <summary>
        /// WorkflowLogicielTiersManager
        /// </summary>
        public IWorkflowLogicielTiersManager WorkflowLogicielTiersManager { get; set; }

        /// <summary>
        /// LogicielTiersManager
        /// </summary>
        public ILogicielTiersManager LogicielTiersManager { get; set; }

        /// <summary>
        /// WorkflowPointage Manager
        /// </summary>
        public IWorkflowPointageManager WorkflowPointageManager { get; set; }
    }
}
