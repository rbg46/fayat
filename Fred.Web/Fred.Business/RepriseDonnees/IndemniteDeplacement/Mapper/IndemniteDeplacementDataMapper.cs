using System;
using System.Collections.Generic;
using Fred.Business.RepriseDonnees.Common.Selector;
using Fred.Business.RepriseDonnees.IndemniteDeplacement.Models;
using Fred.Entities.CI;
using Fred.Entities.IndemniteDeplacement;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.RepriseDonnees.Excel;
using Fred.Framework.Extensions;

namespace Fred.Business.RepriseDonnees.IndemniteDeplacement.Mapper
{
    /// <summary>
    /// Transform les données du fichier excel en Indemnité Personnel
    /// </summary>
    public class IndemniteDeplacementDataMapper : IIndemniteDeplacementDataMapper
    {
        /// <summary>
        /// Creer des IndeMnité Deplacements à partir d'une liste de RepriseExcelIndemniteDeplacement
        /// </summary>
        /// <param name="context">Contexte contenant les data nécessaires à l'import</param>
        /// <param name="listRepriseExcelIndemniteDeplacement">Indemnite Deplacement sous la forme Excel</param>
        /// <returns>Liste d'Indemnite Déplacement</returns>
        public List<IndemniteDeplacementEnt> Transform(ContextForImportIndemniteDeplacement context, List<RepriseExcelIndemniteDeplacement> listRepriseExcelIndemniteDeplacement)
        {
            List<IndemniteDeplacementEnt> result = new List<IndemniteDeplacementEnt>();

            foreach (RepriseExcelIndemniteDeplacement repriseExcelIndemniteDeplacement in listRepriseExcelIndemniteDeplacement)
            {
                result.Add(MapIndemniteDeplacement(context, repriseExcelIndemniteDeplacement));
            }

            return result;
        }

        /// <summary>
        /// Transforme un repriseExcelIndemniteDeplacement en IndemniteDeplacementEnt
        /// </summary>
        /// <param name="context">Contexte pour aider à remplir le IndemniteDeplacementEnt</param>
        /// <param name="repriseExcelIndemniteDeplacement">Ligne excel</param>
        /// <returns>Un IndemniteDeplacementEnt</returns>
        private IndemniteDeplacementEnt MapIndemniteDeplacement(ContextForImportIndemniteDeplacement context, RepriseExcelIndemniteDeplacement repriseExcelIndemniteDeplacement)
        {
            CommonFieldSelector commonSelector = new CommonFieldSelector();

            CodeZoneDeplacementEnt codeZoneDeplacement = context.CodesZoneDeplacementUsedInExcel.Find(x => x.Code == repriseExcelIndemniteDeplacement.CodeZoneDeplacement);
            CodeDeplacementEnt codeDeplacement = context.CodesDeplacementUsedInExcel.Find(x => x.Code == repriseExcelIndemniteDeplacement.CodeDeplacement);
            PersonnelEnt personnel = context.PersonnelsUsedInExcel.Find(x => x.Matricule == repriseExcelIndemniteDeplacement.MatriculePersonnel
                                                                    && x.Societe.Code == repriseExcelIndemniteDeplacement.SocietePersonnel);
            CIEnt ci = context.CIsUsedInExcel.Find(x => x.Code == repriseExcelIndemniteDeplacement.CodeCI
                                                        && x.Societe.Code == repriseExcelIndemniteDeplacement.SocieteCI);

            DateTime? dateDernierCalcul = null;
            if (!repriseExcelIndemniteDeplacement.DateDernierCalcul.IsNullOrEmpty())
            {
                dateDernierCalcul = commonSelector.GetDate(repriseExcelIndemniteDeplacement.DateDernierCalcul);
            }

            return new IndemniteDeplacementEnt()
            {
                PersonnelId = personnel.PersonnelId,
                CiId = ci.CiId,
                NombreKilometres = commonSelector.GetDouble(repriseExcelIndemniteDeplacement.NbKlm),
                NombreKilometreVODomicileRattachement = commonSelector.GetDouble(repriseExcelIndemniteDeplacement.NbKlmVODomicileRattachement),
                NombreKilometreVODomicileChantier = commonSelector.GetDouble(repriseExcelIndemniteDeplacement.NbKlmVODomicileChantier),
                NombreKilometreVOChantierRattachement = commonSelector.GetDouble(repriseExcelIndemniteDeplacement.NbKlmVOChantierRattachement),
                DateDernierCalcul = dateDernierCalcul,
                IVD = string.Equals(repriseExcelIndemniteDeplacement.IVD.Trim(), "o", StringComparison.OrdinalIgnoreCase),
                // Champs facultatif
                CodeDeplacementId = codeDeplacement?.CodeDeplacementId,
                // Champs facultatif
                CodeZoneDeplacementId = codeZoneDeplacement?.CodeZoneDeplacementId,
                SaisieManuelle = string.Equals(repriseExcelIndemniteDeplacement.SaisieManuelle.Trim(), "o", StringComparison.OrdinalIgnoreCase),
                DateCreation = DateTime.UtcNow,
                DateModification = null,
                DateSuppression = null,
                AuteurCreation = context.FredIeUser.UtilisateurId,
                AuteurModification = null,
                AuteurSuppression = null
            };
        }
    }
}
