using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.Rapport;

namespace Fred.Business.Rapport.Pointage
{
    /// <summary>
    /// Classe builder pour le controle FIGGO / TIBCO
    /// </summary>
    public class TibcoListBuilder : List<TibcoModel>
    {

        /// <summary>
        /// Permet de controler le matricule personnel
        /// </summary>
        /// <param name="personnelCode">matricule personnel</param>
        /// <param name="referential">liste de reference</param>
        /// <returns>Builder</returns>
        public TibcoListBuilder ControleMatriculePersonnel(string personnelCode, List<PersonnelEnt> referential)
        {
            if (!referential.Any(r => r.Matricule.Equals(personnelCode)))
            {
                Add(new TibcoModel
                {
                    CodeErreur = "1",
                    MessageErreur = string.Format(FeatureRapport.Rapport_FIGGO_Error_Personnel, personnelCode)
                });
            }
            return this;
        }

        /// <summary>
        /// Permet de controler le matricule valideur
        /// </summary>
        /// <param name="valideurCode">matricule valideur</param>
        /// <param name="referential">liste de reference</param>
        /// <returns>Builder</returns>
        public TibcoListBuilder ControleMatriculeValideur(string valideurCode, List<PersonnelEnt> referential)
        {
            if (!referential.Any(r => r.Matricule.Equals(valideurCode)))
            {
                Add(new TibcoModel
                {
                    CodeErreur = "2",
                    MessageErreur = string.Format(FeatureRapport.Rapport_FIGGO_Error_Valideur, valideurCode)
                });
            }
            return this;
        }

        /// <summary>
        /// Permet de controler la societe du personnel
        /// </summary>
        /// <param name="societeCode">matricule societe</param>
        /// <param name="referential">liste de reference</param>
        /// <returns>Builder</returns>
        public TibcoListBuilder ControleSocietePersonnel(string societeCode, List<SocieteEnt> referential)
        {
            if (!referential.Any(r => r.Code.Equals(societeCode)))
            {
                Add(new TibcoModel
                {
                    CodeErreur = "3",
                    MessageErreur = string.Format(FeatureRapport.Rapport_FIGGO_Error_Societe_Personnel, societeCode)
                });
            }
            return this;
        }

        /// <summary>
        /// Permet de controler la societe du valideur
        /// </summary>
        /// <param name="societeCode">matricule societe</param>
        /// <param name="referential">liste de reference</param>
        /// <returns>Builder</returns>
        public TibcoListBuilder ControleSocieteValideur(string societeCode, List<SocieteEnt> referential)
        {
            if (!referential.Any(r => r.Code.Equals(societeCode)))
            {
                Add(new TibcoModel
                {
                    CodeErreur = "4",
                    MessageErreur = string.Format(FeatureRapport.Rapport_FIGGO_Error_Societe_Valideur, societeCode)
                });
            }
            return this;
        }

        /// <summary>
        /// Permet de controler le code absence
        /// </summary>
        /// <param name="absenceCode">code absence</param>
        /// <param name="referential">liste de reference</param>
        /// <returns>Builder</returns>
        public TibcoListBuilder ControleCodeAbsence(string absenceCode, List<CodeAbsenceEnt> referential)
        {
            if (!referential.Any(r => r.Code.Equals(absenceCode)))
            {
                Add(new TibcoModel
                {
                    CodeErreur = "5",
                    MessageErreur = string.Format(FeatureRapport.Rapport_FIGGO_Error_Code_Absence, absenceCode)
                });
            }
            return this;
        }

        /// <summary>
        /// Permet de controler le statut absence
        /// </summary>
        /// <param name="absenceStatut">statut absence</param>
        /// <param name="referential">liste de reference</param>
        /// <returns>Builder</returns>
        public TibcoListBuilder ControleAbsenceStatut(string absenceStatut, List<StatutAbsenceEnt> referential)
        {
            if (!referential.Any(r => r.Code.Equals(absenceStatut)))
            {
                Add(new TibcoModel
                {
                    CodeErreur = "6",
                    MessageErreur = string.Format(FeatureRapport.Rapport_FIGGO_Error_Statut, absenceStatut)
                });
            }
            return this;
        }

        /// <summary>
        /// Permet de controler le ci
        /// </summary>
        /// <param name="personnel">personnel</param>
        /// <param name="ciId">identifiant ci</param>
        /// <returns>Builder</returns>
        public TibcoListBuilder ControleCi(PersonnelEnt personnel, int ciId)
        {
            if (ciId == 0)
            {
                var personnelCode = personnel.Matricule;
                var societeCode = personnel.Societe.Code;
                Add(new TibcoModel
                {
                    CodeErreur = "7",
                    MessageErreur = string.Format(FeatureRapport.Rapport_FIGGO_Error_Statut, personnelCode, societeCode)
                });
            }
            return this;
        }
    }
}
