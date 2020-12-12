using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.EcritureComptable;
using Fred.Entities.JobStatut;
using Fred.GroupSpecific.Infrastructure;
using Fred.ImportExport.Business.Hangfire.Parameters;
using Fred.ImportExport.Models.CloturePeriode;
using Fred.ImportExport.Models.EcritureComptable;
using Hangfire.Common;

namespace Fred.ImportExport.Business.EcritureComptable.Interfaces
{
    public interface IEcritureComptableFluxManager : IGroupAwareService
    {
        Predicate<Job> GetPredicateForCheckRunningJob(int societeId, int ciId, DateTime dateComptable);
        Task<JobStatutModel> ExecuteImportAsync(int userId, int societeId, int ciId, DateTime dateComptable, string empty);
        Predicate<Job> GetPredicateForCheckRunningJob(int societeId, int ciId, DateTime dateComptableDebut, DateTime dateComptableFin);
        Task<JobStatutModel> ExecuteImportAsync(int userId, int societeId, int ciId, DateTime dateComptableDebut, DateTime dateComptableFin);
        Task<IEnumerable<EcritureComptableDto>> ImportEcrituresComptablesFromAnaelAsync(CloturePeriodeIEModel cloturePeriodeModel);
        Task<IEnumerable<EcritureComptableFtpSapModel>> ImportEcritureComptableAsync(List<EcritureComptableFayatTpImportModel> ecritureComptableFayatTps);
        JobStatutModel ExecuteImportRecurring(string societeCode, DateTime? dateComptable, string codeEtablissement);
        JobStatutModel ExecuteImportRecurring(string societeCode, DateTime? dateDebutComptable, DateTime? dateFinComptable, string codeEtablissement);
        JobStatutModel ExecutePartialImportRecurring(string societeCode, DateTime? dateDebutComptable, DateTime? dateFinComptable, string codeEtablissement);
        void ScheduleImportRecurring(string cron, string codeSocieteComptable);
        Task ImportationProcessRecurringJobAsync(ImportationProcessRecurringParameter parameter);
        Task ImportationProcessRecurringRangeJobAsync(ImportationProcessRecurringRangeParameter parameter);
        Task ImportationPartialProcessRecurringRangeJobAsync(ImportationProcessRecurringRangeParameter parameter);
        Task ImportationProcessJobAsync(ImportationProcessParameter parameter);
        Task ImportationProcessRangeJobAsync(ImportationProcessRangeParameter parameter);
    }
}
