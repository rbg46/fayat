using System;
using System.Collections.Generic;
using Fred.Entities.Rapport;
using Fred.ImportExport.DataAccess.WorkflowLogicielTiers;
using Fred.ImportExport.Entities;

namespace Fred.ImportExport.Business.WorkflowLogicielTiers
{

    public class WorkflowPointageManager : IWorkflowPointageManager
    {
        private readonly IWorkflowPointageRepository workflowPointageRepository;

        public WorkflowPointageManager(IWorkflowPointageRepository workflowPointageRepository)
        {
            this.workflowPointageRepository = workflowPointageRepository;
        }

        public IEnumerable<int> GetRapportLigneIdDejaEnvoye(int logicielTiersId, string fluxName)
        {
            return workflowPointageRepository.GetRapportLigneIdDejaEnvoyeALogiciel(logicielTiersId, fluxName);
        }

        public List<WorkflowPointageEnt> GetRapportLigneWorkflowPointages(List<int> rapportLigneIdList, int logicielTiersId, string fluxName)
        {
            return workflowPointageRepository.GetRapportLigneWorkflowPointages(rapportLigneIdList, logicielTiersId, fluxName);
        }

        public void SaveWorkflowPointage(IEnumerable<RapportLigneEnt> rapportLignes, int logicielTiersId, int auteurId, string hangFireJobId, string flux)
        {
            //rapportLignesId, logicielTiersId, auteurId, flux
            var workflowPointageList = BuildWorkflowPointage(rapportLignes, logicielTiersId, auteurId, hangFireJobId, flux);
            workflowPointageRepository.SaveWorkflowPointage(workflowPointageList);
        }

        private List<WorkflowPointageEnt> BuildWorkflowPointage(IEnumerable<RapportLigneEnt> rapportLignes, int logicielTiersId, int auteurId, string hangFireJobId, string flux)
        {
            var workflowPointageList = new List<WorkflowPointageEnt>();
            var date = DateTime.UtcNow;

            foreach (var rapportLigne in rapportLignes)
            {
                workflowPointageList.Add(
                    new WorkflowPointageEnt
                    {
                        RapportId = rapportLigne.RapportId,
                        RapportLigneId = rapportLigne.RapportLigneId,
                        LogicielTiersId = logicielTiersId,
                        AuteurId = auteurId,
                        Date = date,
                        FluxName = flux,
                        CiId = rapportLigne.CiId,
                        DatePointage = rapportLigne.DatePointage,
                        MaterielId = rapportLigne.MaterielId,
                        PersonnelId = rapportLigne.PersonnelId,
                        MaterielMarche = rapportLigne.MaterielMarche,
                        MaterielArret = rapportLigne.MaterielArret,
                        MaterielPanne = rapportLigne.MaterielPanne,
                        MaterielIntemperie = rapportLigne.MaterielIntemperie,
                        HeureNormale = rapportLigne.HeureNormale,
                        HeureMajoration = rapportLigne.HeureMajoration,
                        HeureAbsence = rapportLigne.HeureAbsence,
                        CodeAbsenceId = rapportLigne.CodeAbsenceId,
                        CodeMajorationId = rapportLigne.CodeMajorationId,
                        CodeDeplacementId = rapportLigne.CodeDeplacementId,
                        CodeZoneDeplacementId = rapportLigne.CodeZoneDeplacementId,
                        DeplacementIV = rapportLigne.DeplacementIV,
                        HangfireJobId = hangFireJobId,
                        Suppression = rapportLigne.DateSuppression.HasValue ? true : false,
                        DateEnvoiSuppression = rapportLigne.DateSuppression.HasValue ? rapportLigne.DateSuppression : null,
                    }
                );
            }

            return workflowPointageList;
        }
    }
}
