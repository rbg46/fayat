using System.Collections.Generic;
using Fred.Entities.Personnel.Import;

namespace Fred.ImportExport.Business.Personnel.Logs
{
    public interface IImportPersonnelLogs
    {
        void LogStartRequestAllData();

        void LogStartTransormPersonnel();

        void LogDetectionModificationOfPersonnel(List<PersonnelAffectationResult> personnelAffectationResults);

        void LogNoModificationOfPersonnel(List<PersonnelAffectationResult> personnelAffectationResults);

        void LogDetectionCreationPersonnel(List<PersonnelAffectationResult> personnelAffectationResults);

        string ErrorNoSocieteWithCodeSocietePaie(List<string> codeSocietePaies);


        string ErrorDifferentGroupe(List<string> codeSocietePaies);

        string ErrorSocietesMissing(List<string> codeSocietePaies);

        void WarnManagerNotFound(List<PersonnelAffectationResult> personnelAffectationResults);


        void WarnEtablissementPaieNotFound(List<PersonnelAffectationResult> personnelAffectationResults);


        void WarnRessourceNotFound(List<PersonnelAffectationResult> personnelAffectationResults);


        void WarnNoCiFound(List<PersonnelAffectationResult> personnelAffectationResults);

    }
}
