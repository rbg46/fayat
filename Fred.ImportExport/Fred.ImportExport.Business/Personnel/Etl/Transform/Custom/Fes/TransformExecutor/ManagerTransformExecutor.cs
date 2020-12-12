using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Import;

namespace Fred.ImportExport.Business.Personnel.Etl.Transform.Custom.Fes.TransformExecutor
{
    public class ManagerTransformExecutor
    {
        /// <summary>
        /// Execute les transformations pour le managerl'entite PersonnelEnt
        /// </summary>
        /// <param name="personnelAffectationResults">Resultat de la precedante transformation</param>
        /// <param name="importPersonnelsSocieteData">Donnée globales</param>
        /// <returns>retourne la liste recue en entrée</returns>
        public List<PersonnelAffectationResult> ExecuteTransformManagerOfPersonnels(List<PersonnelAffectationResult> personnelAffectationResults, ImportPersonnelsGlobalData importPersonnelsSocieteData)
        {
            foreach (var personnel in personnelAffectationResults.Where(p => p.HasManager).ToList())
            {
                AffecteManagerEntity(personnel, personnelAffectationResults, importPersonnelsSocieteData);
            }

            return personnelAffectationResults;
        }

        private void AffecteManagerEntity(PersonnelAffectationResult personnelTransformResult, List<PersonnelAffectationResult> newOrUpdatedPersonnels, ImportPersonnelsGlobalData importPersonnelsSocieteData)
        {
            if (ManagerNotExisteInFred(personnelTransformResult.Personnel))
            {
                personnelTransformResult.ManagerIsNotCreatedInFred = true;
                //nettoyage pour evité les echecs de sauvegarde
                personnelTransformResult.Personnel.Manager = null;
                personnelTransformResult.Personnel.ManagerId = null;

                var societeDataOfManager = importPersonnelsSocieteData.GetSocieteData(personnelTransformResult.PersonnelModel.SocieteManager);
                if (societeDataOfManager == null || societeDataOfManager.Societe == null)
                {
                    personnelTransformResult.ManagerCanNotBeProcessed = true;
                }
                var managerNotInfred = newOrUpdatedPersonnels.Select(p => p.Personnel).FirstOrDefault(p => p.Matricule == personnelTransformResult.PersonnelModel.MatriculeManager && p.SocieteId == societeDataOfManager.Societe.SocieteId);
                if (managerNotInfred != null)
                {
                    personnelTransformResult.Manager = managerNotInfred;
                }
                else
                {
                    // impossible de trouvé dans la liste des personnel nouvellement crée le personnel correspondant  au manager                  
                    personnelTransformResult.ManagerCanNotBeProcessed = true;
                }
            }
        }

        private bool ManagerNotExisteInFred(PersonnelEnt personnel)
        {
            return personnel.ManagerId == -1;
        }
    }
}
