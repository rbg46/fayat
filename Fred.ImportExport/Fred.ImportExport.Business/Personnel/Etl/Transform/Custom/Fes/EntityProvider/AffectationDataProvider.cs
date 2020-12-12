using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Affectation;
using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Imports;
using Fred.Entities.Referential;

namespace Fred.ImportExport.Business.Personnel.Etl.Transform.Custom.Fes.EntityProvider
{
    public class AffectationDataProvider
    {

        /// <summary>
        /// Get default affectation 
        /// </summary>
        /// <param name="personnelId">Personnel identifier</param>
        /// <param name="affectationsList">Affectation List</param>
        /// <returns>Default affectation</returns>
        public static AffectationEnt GetDefaultAffectation(int personnelId, List<AffectationEnt> affectationsList)
        {
            return affectationsList.FirstOrDefault(x => x.PersonnelId == personnelId && x.IsDefault);
        }

        /// <summary>
        /// Get Affectation
        /// </summary>
        /// <param name="personnel">Pesrsonnel entity</param>
        /// <param name="model">Model anael</param>
        /// <param name="importPersonnelsGlobalData">import personnel global data</param>
        /// <returns>Affectation</returns>
        public static AffectationEnt GetAffectation(PersonnelEnt personnel, PersonnelModel model, ImportPersonnelsGlobalData importPersonnelsGlobalData)
        {
            AffectationDataProvider affectationDataProvider = new AffectationDataProvider();
            int ciId = affectationDataProvider.GetCiId(personnel, model, importPersonnelsGlobalData);
            return importPersonnelsGlobalData.Affectations.FirstOrDefault(x => x.PersonnelId == personnel.PersonnelId && x.CiId == ciId);
        }

        public int GetCiId(PersonnelEnt personnel, PersonnelModel model, ImportPersonnelsGlobalData importPersonnelsGlobalData)
        {
            List<CIEnt> cis;
            int? etablissmeentComptableId;
            if (importPersonnelsGlobalData.IsFes)
            {
                ImportPersonnelsSocieteData societeComptableData = importPersonnelsGlobalData.GetSocieteDataByCode(model.SocieteComptable);
                EtablissementComptableEnt etablissementComptable = societeComptableData.EtablissementComptablesForSociete.FirstOrDefault(e => e.Code.Equals(model.EtablissementComptable));
                cis = societeComptableData.Cis;
                etablissmeentComptableId = etablissementComptable?.EtablissementComptableId;
            }
            else
            {
                ImportPersonnelsSocieteData societeData = importPersonnelsGlobalData.GetSocieteData(model.CodeSocietePaye);
                cis = societeData.Cis;
                etablissmeentComptableId = personnel.EtablissementPaie?.EtablissementComptableId;
            }

            return GetCiId(cis, model.Section, etablissmeentComptableId);
        }

        private int GetCiId(List<CIEnt> cis, string ciCode, int? etablissementComptableId)
        {
            CIEnt result = cis.FirstOrDefault(x => !string.IsNullOrEmpty(x.CodeExterne) && x.CodeExterne.Trim() == ciCode.Trim()
                && (!etablissementComptableId.HasValue || x.EtablissementComptableId == etablissementComptableId));
            return result == null ? 0 : result.CiId;
        }
    }
}
