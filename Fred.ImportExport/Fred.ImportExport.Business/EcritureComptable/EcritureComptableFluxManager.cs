using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Fred.Business.CI;
using Fred.Business.EcritureComptable.Import;
using Fred.Business.Notification;
using Fred.Business.Societe;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.EcritureComptable;
using Fred.Entities.JobStatut;
using Fred.Framework.DateTimeExtend;
using Fred.Framework.Exceptions;
using Fred.Framework.Tool;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.EcritureComptable.Interfaces;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.Business.Hangfire.Parameters;
using Fred.ImportExport.DataAccess.Common;
using Fred.ImportExport.DataAccess.Interfaces;
using Fred.ImportExport.Models.CloturePeriode;
using Fred.ImportExport.Models.EcritureComptable;
using Hangfire;
using Hangfire.Common;

namespace Fred.ImportExport.Business.OperationDiverse
{
    public abstract class EcritureComptableFluxManager : AbstractFluxManager, IEcritureComptableFluxManager
    {
        private const string SqlScriptPath = "EcritureComptable.SELECT_ECRITURES_COMPTABLES.sql";
        private const string SqlScriptPartial = "EcritureComptable.SELECT_ECRITURES_COMPTABLES_PARTIEL.sql";
        private const string SqlScriptPathFromDInt = "EcritureComptable.SELECT_ECRITURES_COMPTABLES_FROM_DINT.sql";
        private const string SqlScriptPathForRequestByCi = "EcritureComptable.SELECT_ECRITURES_COMPTABLES_BY_CI.sql";
        private const string SqlScriptPathForRequestByCis = "EcritureComptable.SELECT_ECRITURES_COMPTABLES_BY_CIs.sql";

        private readonly ICIManager ciManager;
        private readonly INotificationManager notificationManager;
        private readonly IGroupeRepository groupRepository;
        private readonly string chaineConnexionAnael;
        private readonly ISocieteRepository societeRepository;
        private readonly IFluxRepository fluxRepository;

        protected IEcritureComptableImportManager EcritureComptableImportManager { get; }
        protected ISocieteManager SocieteManager { get; }

        protected EcritureComptableFluxManager(
            IFluxManager fluxManager,
            string chaineConnexionAnael,
            IEcritureComptableImportManager ecritureComptableImportManager,
            ICIManager ciManager,
            ISocieteManager societeManager,
            INotificationManager notificationManager,
            IGroupeRepository groupRepository,
            ISocieteRepository societeRepository,
            IFluxRepository fluxRepository)
            : base(fluxManager)
        {
            this.ciManager = ciManager;
            this.notificationManager = notificationManager;
            this.groupRepository = groupRepository;
            this.chaineConnexionAnael = chaineConnexionAnael;
            this.societeRepository = societeRepository;
            this.fluxRepository = fluxRepository;

            EcritureComptableImportManager = ecritureComptableImportManager;
            SocieteManager = societeManager;
        }

        public Predicate<Job> GetPredicateForCheckRunningJob(int societeId, int ciId, DateTime dateComptable)
        {
            Predicate<Job> jobSelector = (job) =>
            {
                try
                {
                    IReadOnlyList<object> arguments = job.Args;
                    if (arguments.Count != 4)
                    {
                        return false;
                    }

                    int societeIdArgument = (int)arguments[1];
                    int ciIdArgument = (int)arguments[2];
                    DateTime dateComptableArgument = (DateTime)arguments[3];

                    return societeIdArgument == societeId
                  || ciIdArgument == ciId
                  || dateComptableArgument.Year == dateComptable.Year
                  || dateComptableArgument.Month == dateComptable.Month;
                }
                catch (Exception)
                {
                    return false;
                }
            };

            return jobSelector;
        }

        public Predicate<Job> GetPredicateForCheckRunningJob(int societeId, int ciId, DateTime dateComptableDebut, DateTime dateComptableFin)
        {
            Predicate<Job> jobSelector = (job) =>
            {
                try
                {
                    IReadOnlyList<object> arguments = job.Args;
                    if (arguments.Count != 5)
                    {
                        return false;
                    }

                    int societeIdArgument = (int)arguments[1];
                    int ciIdArgument = (int)arguments[2];
                    DateTime dateComptableArgumentDebut = (DateTime)arguments[3];
                    DateTime dateComptableArgumentFin = (DateTime)arguments[3];

                    return societeIdArgument == societeId
                        || ciIdArgument == ciId
                        || dateComptableArgumentDebut.Year == dateComptableDebut.Year
                        || dateComptableArgumentDebut.Month == dateComptableDebut.Month
                        || dateComptableArgumentFin.Year == dateComptableFin.Year
                        || dateComptableArgumentFin.Month == dateComptableFin.Month;
                }
                catch (Exception)
                {
                    return false;
                }
            };

            return jobSelector;
        }

        #region EXECUTION JOB RECURRANT

        public JobStatutModel ExecuteImportRecurring(string societeCode, DateTime? dateComptable, string codeEtablissement)
        {
            if (Flux.IsActif)
            {
                string jobId = BackgroundJob.Enqueue(() => ImportationProcessRecurring(societeCode, dateComptable, codeEtablissement, Flux.Code));
                return JobInfoProvider.GetJobStatus(jobId);
            }
            else
            {
                throw CreateExceptionWithMessage(IEBusiness.FluxInactif); 
            }
        }

        public JobStatutModel ExecuteImportRecurring(string societeCode, DateTime? dateDebutComptable, DateTime? dateFinComptable, string codeEtablissement)
        {
            if (Flux.IsActif)
            {
                string jobId = BackgroundJob.Enqueue(() => ImportationProcessRecurring(societeCode, dateDebutComptable, dateFinComptable, codeEtablissement, Flux.Code));
                return JobInfoProvider.GetJobStatus(jobId);
            }
            else
            {
                throw CreateExceptionWithMessage(IEBusiness.FluxInactif); 
            }
        }

        public JobStatutModel ExecutePartialImportRecurring(string societeCode, DateTime? dateDebutComptable, DateTime? dateFinComptable, string codeEtablissement)
        {
            if (Flux.IsActif)
            {
                string jobId = BackgroundJob.Enqueue(() => ImportationPartialProcessRecurring(societeCode, dateDebutComptable, dateFinComptable, codeEtablissement));
                return JobInfoProvider.GetJobStatus(jobId);
            }
            else
            {
                throw CreateExceptionWithMessage(IEBusiness.FluxInactif);
            }
        }

        /// <summary>
        /// Planifie l'exécution du job selon un CRON
        /// </summary>
        /// <param name="cron">Command On Run</param>        
        /// <param name="codeSocieteComptable">Code société comptable ANAEL</param>
        public void ScheduleImportRecurring(string cron, string codeSocieteComptable)
        {
            if (!string.IsNullOrEmpty(cron))
            {
                RecurringJob.AddOrUpdate(Flux.Code, () => ImportationProcessRecurring(codeSocieteComptable, null, string.Empty, Flux.Code), cron);
            }
            else
            {
                string msg = $"Le CRON n'est pas paramétré pour le job {Flux.Code}";
                var exception = new FredBusinessException(msg);
                NLog.LogManager.GetCurrentClassLogger().Error(exception, msg);
                throw exception;
            }
        }

        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[IMPORT] Ecritures comptables importées de nuit(ANAEL => FRED)")]
        public async Task ImportationProcessRecurring(string societeCode, DateTime? dateComptable, string codeEtablissement, string fluxCode)
        {
            var parameter = new ImportationProcessRecurringParameter { SocieteCode = societeCode, DateComptable = dateComptable, CodeEtablissement = codeEtablissement, FluxCode = fluxCode };
            string groupCode = await groupRepository.GetGroupCodeByAccountingCompanyCodeAsync(societeCode);

            await JobRunnerApiRestHelper.PostAsync("ImportationProcessRecurring", groupCode, parameter);
        }

        public async Task ImportationProcessRecurringJobAsync(ImportationProcessRecurringParameter parameter)
        {
            try
            {
                Flux = FluxManager.GetByCode(parameter.FluxCode);
                string societeCode = parameter.SocieteCode;
                DateTime? dateComptable = parameter.DateComptable;
                string codeEtablissement = parameter.CodeEtablissement;

                if (!dateComptable.HasValue)
                {
                    dateComptable = DateTime.UtcNow;
                }

                int? societeId = SocieteManager.GetSocieteIdByCodeSocieteComptable(societeCode);
                if (societeId.HasValue)
                {
                    IEnumerable<EcritureComptableDto> ecritureComptables = GetEcrituresComptablesFromAnael(dateComptable.Value);
                    await EcritureComptableImportManager.ManageImportEcritureComptablesAsync(societeId.Value, dateComptable.Value, ecritureComptables, codeEtablissement).ConfigureAwait(false);
                }

                Flux.DateDerniereExecution = DateTime.UtcNow;
                FluxManager.Update(Flux);
            }
            catch (Exception ex)
            {
                throw CreateExceptionWithMessageAndInnerException(ex, IEBusiness.FluxErreurImport);
            }
        }

        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[IMPORT] Ecritures comptables importées de nuit(ANAEL => FRED)")]
        public async Task ImportationProcessRecurring(string societeCode, DateTime? dateDebutComptable, DateTime? dateFinComptable, string codeEtablissement, string fluxCode)
        {
            var parameter = new ImportationProcessRecurringRangeParameter
            {
                SocieteCode = societeCode,
                DateDebutComptable = dateDebutComptable,
                DateFinComptable = dateFinComptable,
                CodeEtablissement = codeEtablissement,
                FluxCode = fluxCode
            };
            string groupCode = await groupRepository.GetGroupCodeByAccountingCompanyCodeAsync(societeCode);

            await JobRunnerApiRestHelper.PostAsync("ImportationProcessRecurringRange", groupCode, parameter);
        }

        public async Task ImportationProcessRecurringRangeJobAsync(ImportationProcessRecurringRangeParameter parameter)
        {
            try
            {
                Flux = FluxManager.GetByCode(parameter.FluxCode);
                string societeCode = parameter.SocieteCode;
                DateTime? dateDebutComptable = parameter.DateDebutComptable;
                DateTime? dateFinComptable = parameter.DateFinComptable;
                string codeEtablissement = parameter.CodeEtablissement;

                if (!dateDebutComptable.HasValue)
                {
                    dateDebutComptable = DateTime.UtcNow;
                }

                int? societeId = SocieteManager.GetSocieteIdByCodeSocieteComptable(societeCode);
                if (societeId.HasValue)
                {
                    IEnumerable<EcritureComptableDto> ecritureComptables = GetEcrituresComptablesFromAnael(dateDebutComptable.Value, dateFinComptable.Value);
                    await EcritureComptableImportManager.ManageImportEcritureComptablesAsync(societeId.Value, dateDebutComptable.Value, dateFinComptable.Value, ecritureComptables, codeEtablissement).ConfigureAwait(false);
                }

                Flux.DateDerniereExecution = DateTime.UtcNow;
                FluxManager.Update(Flux);
            }
            catch (Exception ex)
            {
                throw CreateExceptionWithMessageAndInnerException(ex, IEBusiness.FluxErreurImport);
            }
        }

        public async Task ImportationPartialProcessRecurring(string societeCode, DateTime? dateDebutComptable, DateTime? dateFinComptable, string codeEtablissement)
        {
            ImportationProcessRecurringRangeParameter parameter = new ImportationProcessRecurringRangeParameter
            {
                SocieteCode = societeCode,
                DateDebutComptable = dateDebutComptable,
                DateFinComptable = dateFinComptable,
                CodeEtablissement = codeEtablissement,
                FluxCode = Flux.Code
            };

            string groupCode = await groupRepository.GetGroupCodeByAccountingCompanyCodeAsync(societeCode);

            await JobRunnerApiRestHelper.PostAsync("ImportationPartialProcessRecurringRange", groupCode, parameter);
        }

        public async Task ImportationPartialProcessRecurringRangeJobAsync(ImportationProcessRecurringRangeParameter parameter)
        {
            try
            {
                Flux = FluxManager.GetByCode(parameter.FluxCode);
                string societeCode = parameter.SocieteCode;
                DateTime? dateDebutComptable = parameter.DateDebutComptable;
                DateTime? dateFinComptable = parameter.DateFinComptable;
                string codeEtablissement = parameter.CodeEtablissement;

                if (!dateDebutComptable.HasValue)
                {
                    dateDebutComptable = DateTime.UtcNow;
                }

                int? societeId = SocieteManager.GetSocieteIdByCodeSocieteComptable(societeCode);
                if (societeId.HasValue)
                {
                    IEnumerable<EcritureComptableDto> ecritureComptables = GetPartialEcritureComptables(dateDebutComptable.Value, dateFinComptable.Value);
                    await EcritureComptableImportManager.ManageImportEcritureComptablesAsync(societeId.Value, dateDebutComptable.Value, dateFinComptable.Value, ecritureComptables, codeEtablissement).ConfigureAwait(false);
                }

                Flux.DateDerniereExecution = DateTime.UtcNow;
                FluxManager.Update(Flux);
            }
            catch (Exception ex)
            {
                throw CreateExceptionWithMessageAndInnerException(ex, IEBusiness.FluxErreurImport);
            }
        }

        #endregion

        #region EXECUTION JOB A LA DEMANDE (PAR CI)

        public async Task<JobStatutModel> ExecuteImportAsync(int userId, int societeId, int ciId, DateTime dateComptable, string codeEtablissement)
        {
            string fluxCode = await GetFluxCodeBySocieteIdAsync(societeId);
            SetFlux(fluxCode);

            if (Flux.IsActif)
            {
                string jobId = BackgroundJob.Enqueue(() => ImportationProcess(userId, societeId, ciId, dateComptable, codeEtablissement));
                return JobInfoProvider.GetJobStatus(jobId);
            }
            else
            {
                throw CreateExceptionWithMessage(IEBusiness.FluxInactif);
            }
        }

        public async Task<JobStatutModel> ExecuteImportAsync(int userId, int societeId, int ciId, DateTime dateComptableDebut, DateTime dateComptableFin)
        {
            string fluxCode = await GetFluxCodeBySocieteIdAsync(societeId);
            SetFlux(fluxCode);

            if (Flux.IsActif)
            {
                string jobId = BackgroundJob.Enqueue(() => ImportationProcess(userId, societeId, ciId, dateComptableDebut, dateComptableFin));
                return JobInfoProvider.GetJobStatus(jobId);
            }
            else
            {
                throw CreateExceptionWithMessage(IEBusiness.FluxInactif);
            }
        }

        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[IMPORT] Ecritures comptables (ANAEL => FRED)")]
        public async Task ImportationProcess(int userId, int societeId, int ciId, DateTime dateComptable, string codeEtablissement)
        {
            string codeFlux = await GetFluxCodeBySocieteIdAsync(societeId);

            var parameter = new ImportationProcessParameter { UserId = userId, SocieteId = societeId, CiId = ciId, DateComptable = dateComptable, CodeEtablissement = codeEtablissement, FluxCode = codeFlux };
            string groupCode = await groupRepository.GetGroupCodeByCompanyIdAsync(societeId);

            await JobRunnerApiRestHelper.PostAsync("ImportationProcess", groupCode, parameter);
        }

        private async Task<string> GetFluxCodeBySocieteIdAsync(int societeId)
        {
            string societeCode = await societeRepository.GetCodeSocieteComptableByIdAsync(societeId);
            string codeFlux = await fluxRepository.GetCodeStartingWithBySocieteCodeAsync("ECRITURE_COMPTABLE", societeCode);

            return codeFlux;
        }

        public async Task ImportationProcessJobAsync(ImportationProcessParameter parameter)
        {
            Flux = FluxManager.GetByCode(parameter.FluxCode);
            int userId = parameter.UserId;
            int societeId = parameter.SocieteId;
            int ciId = parameter.CiId;
            DateTime dateComptable = parameter.DateComptable;
            string codeEtablissement = parameter.CodeEtablissement;

            CIEnt ci = ciManager.FindById(ciId);
            try
            {
                if (ci != null)
                {
                    string ciCode = ci.Code;
                    IEnumerable<EcritureComptableDto> ecritureComptables = GetEcrituresComptablesFromAnael(dateComptable, ciCode);
                    int newEcrituresCount = await EcritureComptableImportManager.ManageImportEcritureComptablesAsync(societeId, dateComptable, ecritureComptables, codeEtablissement).ConfigureAwait(false);

                    await notificationManager.CreateNotificationAsync(userId,
                                                              Fred.Entities.Notification.TypeNotification.NotificationUtilisateur,
                                                              $"L'import des écritures comptables, pour le centre d'imputation {ci?.Code} et la date comptable : {dateComptable:MM-yyyy}, a été effectué avec succès.({newEcrituresCount})");
                }

                Flux.DateDerniereExecution = DateTime.UtcNow;
                FluxManager.Update(Flux);
            }
            catch (Exception ex)
            {
                FredBusinessException exception = CreateExceptionWithMessageAndInnerException(ex, IEBusiness.FluxErreurImport);
                await notificationManager.CreateNotificationAsync(userId,
                                                      Fred.Entities.Notification.TypeNotification.NotificationUtilisateur,
                                                      $"L'import des écritures comptables pour le centre d'imputation {ci?.Code} et la date comptable : {dateComptable.ToShortDateString()}, a échoué.");
                throw exception;
            }
        }

        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[IMPORT] Ecritures comptables (ANAEL => FRED)")]
        public async Task ImportationProcess(int userId, int societeId, int ciId, DateTime dateComptableDebut, DateTime dateComptableFin)
        {
            string fluxCode = await GetFluxCodeBySocieteIdAsync(societeId);

            var parameter = new ImportationProcessRangeParameter { UserId = userId, SocieteId = societeId, CiId = ciId, DateDebutComptable = dateComptableDebut, DateFinComptable = dateComptableFin, FluxCode = fluxCode };
            string groupCode = await groupRepository.GetGroupCodeByCompanyIdAsync(societeId);

            await JobRunnerApiRestHelper.PostAsync("ImportationProcessRange", groupCode, parameter);
        }

        public async Task ImportationProcessRangeJobAsync(ImportationProcessRangeParameter parameter)
        {
            Flux = FluxManager.GetByCode(parameter.FluxCode);
            int userId = parameter.UserId;
            int societeId = parameter.SocieteId;
            int ciId = parameter.CiId;
            DateTime dateComptableDebut = parameter.DateDebutComptable;
            DateTime dateComptableFin = parameter.DateFinComptable;

            CIEnt ci = ciManager.FindById(ciId);
            try
            {
                if (ci != null)
                {
                    string ciCode = ci.Code;
                    IEnumerable<EcritureComptableDto> ecritureComptables = GetEcrituresComptablesFromAnael(dateComptableDebut, dateComptableFin, ciCode);
                    int newEcrituresCount = await EcritureComptableImportManager.ManageImportEcritureComptablesAsync(societeId, dateComptableDebut, dateComptableFin, ecritureComptables, string.Empty).ConfigureAwait(false);

                    await notificationManager.CreateNotificationAsync(userId,
                                                              Fred.Entities.Notification.TypeNotification.NotificationUtilisateur,
                                                              $"L'import des écritures comptables, pour le centre d'imputation {ci?.Code} et pour la période du {dateComptableDebut:MM-yyyy} au {dateComptableFin.ToString("MM-yyyy")}, a été effectué avec succès.({newEcrituresCount})");
                }

                Flux.DateDerniereExecution = DateTime.UtcNow;
                FluxManager.Update(Flux);
            }
            catch (Exception ex)
            {
                FredBusinessException exception = CreateExceptionWithMessageAndInnerException(ex, IEBusiness.FluxErreurImport);
                await notificationManager.CreateNotificationAsync(userId, Fred.Entities.Notification.TypeNotification.NotificationUtilisateur,
                                                      $"L'import des écritures comptables pour le centre d'imputation {ci?.Code} et  pour la période du {dateComptableDebut:MM-yyyy} au {dateComptableFin:MM-yyyy}, a échoué.");
                throw exception;
            }
        }

        #endregion

        public IEnumerable<EcritureComptableDto> GetEcrituresComptablesFromAnael(DateTime dateComptable, string ciCode = null)
        {
            List<EcritureComptableDto> ecritureComptables = new List<EcritureComptableDto>();
            try
            {
                // Connexion à la base de données source (ANAËL FINANCE)
                using (DataAccess.Common.Database sourceDB = DatabaseFactory.GetNewDatabase(TypeBdd.Db2, chaineConnexionAnael))
                {
                    string query = string.Empty;

                    MonthLimits limitesDeImportEcritureComptable = dateComptable.GetLimitsOfMonth();

                    if (ciCode == null)
                    {
                        query = SqlScriptProvider.GetEmbeddedSqlScriptContent(Assembly.GetExecutingAssembly(), SqlScriptPathFromDInt);
                        query = string.Format(query, Flux.SocieteCode, TransformDate(dateComptable));
                    }
                    else
                    {
                        query = SqlScriptProvider.GetEmbeddedSqlScriptContent(Assembly.GetExecutingAssembly(), SqlScriptPathForRequestByCi);
                        query = string.Format(query, Flux.SocieteCode, FormatDateForAs400(limitesDeImportEcritureComptable.StartDate), FormatDateForAs400(limitesDeImportEcritureComptable.EndDate), ciCode);
                    }

                    // Récupération des écritures comptables
                    ecritureComptables = GetEcritureFromAnaelDb(sourceDB, query);
                }
            }
            catch (Exception ex)
            {
                throw CreateExceptionWithMessageAndInnerException(ex, IEBusiness.FluxErreurRecuperation);
            }

            return ecritureComptables;
        }

        public IEnumerable<EcritureComptableDto> GetEcrituresComptablesFromAnael(DateTime dateComptableDebut, DateTime dateComptableFin, string ciCode = null)
        {
            List<EcritureComptableDto> ecritureComptables = new List<EcritureComptableDto>();
            try
            {
                // Connexion à la base de données source (ANAËL FINANCE)
                using (DataAccess.Common.Database sourceDB = DatabaseFactory.GetNewDatabase(TypeBdd.Db2, chaineConnexionAnael))
                {
                    string query;

                    MonthLimits limitesDeImportEcritureComptableDebut = dateComptableDebut.GetLimitsOfMonth();
                    MonthLimits limitesDeImportEcritureComptableFin = dateComptableFin.GetLimitsOfMonth();
                    if (ciCode == null)
                    {
                        query = SqlScriptProvider.GetEmbeddedSqlScriptContent(Assembly.GetExecutingAssembly(), SqlScriptPath);
                        query = string.Format(query, Flux.SocieteCode, FormatDateForAs400(limitesDeImportEcritureComptableDebut.StartDate), FormatDateForAs400(limitesDeImportEcritureComptableFin.EndDate));
                    }
                    else
                    {
                        query = SqlScriptProvider.GetEmbeddedSqlScriptContent(Assembly.GetExecutingAssembly(), SqlScriptPathForRequestByCi);
                        query = string.Format(query, Flux.SocieteCode, FormatDateForAs400(limitesDeImportEcritureComptableDebut.StartDate), FormatDateForAs400(limitesDeImportEcritureComptableFin.EndDate), ciCode);
                    }

                    ecritureComptables = GetEcritureFromAnaelDb(sourceDB, query);
                }
            }
            catch (Exception ex)
            {
                throw CreateExceptionWithMessageAndInnerException(ex, IEBusiness.FluxErreurRecuperation);
            }

            return ecritureComptables;
        }

        public IEnumerable<EcritureComptableDto> GetPartialEcritureComptables(DateTime dateComptableDebut, DateTime dateComptableFin)
        {
            List<EcritureComptableDto> ecritureComptables = new List<EcritureComptableDto>();
            try
            {
                // Connexion à la base de données source (ANAËL FINANCE)
                using (DataAccess.Common.Database sourceDB = DatabaseFactory.GetNewDatabase(TypeBdd.Db2, chaineConnexionAnael))
                {
                    string query;

                    MonthLimits limitesDeImportEcritureComptableDebut = dateComptableDebut.GetLimitsOfMonth();
                    MonthLimits limitesDeImportEcritureComptableFin = dateComptableFin.GetLimitsOfMonth();
                    query = SqlScriptProvider.GetEmbeddedSqlScriptContent(Assembly.GetExecutingAssembly(), SqlScriptPartial);
                    query = string.Format(query, Flux.SocieteCode, FormatDate(limitesDeImportEcritureComptableDebut.StartDate), FormatDate(limitesDeImportEcritureComptableFin.EndDate));

                    ecritureComptables.AddRange(GetEcritureFromAnaelDb(sourceDB, query));
                }
            }
            catch (Exception ex)
            {
                throw CreateExceptionWithMessageAndInnerException(ex, IEBusiness.FluxErreurRecuperation);
            }

            return ecritureComptables;
        }

        private List<EcritureComptableDto> GetEcritureFromAnaelDb(DataAccess.Common.Database sourceDB, string query)
        {
            List<EcritureComptableDto> ecritureComptableDtos = new List<EcritureComptableDto>();
            using (IDataReader resultsEtab = sourceDB.ExecuteReader(query))
            {
                while (resultsEtab.Read())
                {
                    EcritureComptableDto dto = CreateEcritureComptableDto(resultsEtab);
                    if (dto != null)
                    {
                        ecritureComptableDtos.Add(dto);
                    }
                }
            }
            return ecritureComptableDtos;
        }


        /// <summary>
        /// Récupération des écritures comptables d'AS400 
        /// </summary>
        /// <param name="cloturePeriodeModel"><see cref="CloturePeriodeIEModel"/></param>
        /// <returns>Liste des écritures comptables</returns>
        public async Task<IEnumerable<EcritureComptableDto>> ImportEcrituresComptablesFromAnaelAsync(CloturePeriodeIEModel cloturePeriodeModel)
        {
            List<EcritureComptableDto> ecritureComptables = new List<EcritureComptableDto>();
            try
            {
                // Connexion à la base de données source (ANAËL FINANCE)
                using (DataAccess.Common.Database sourceDB = DatabaseFactory.GetNewDatabase(TypeBdd.Db2, chaineConnexionAnael))
                {
                    string codeSocieteComptable = await societeRepository.GetCodeSocieteComptableByCodeAsync(cloturePeriodeModel.SocieteCode);
                    List<CIEnt> cIEnts = ciManager.GetCisByIds(cloturePeriodeModel.CiIds);
                    string ciList = string.Join(",", cIEnts.Select(q => $"'{q.Code}'"));
                    MonthLimits limitesDeImportEcritureComptable = cloturePeriodeModel.DateComptable.GetLimitsOfMonth();

                    if (cloturePeriodeModel.CiIds.Count != 0)
                    {
                        string query = SqlScriptProvider.GetEmbeddedSqlScriptContent(Assembly.GetExecutingAssembly(), SqlScriptPathForRequestByCis);
                        query = string.Format(query, codeSocieteComptable, FormatDateForAs400(limitesDeImportEcritureComptable.StartDate), FormatDateForAs400(limitesDeImportEcritureComptable.EndDate), ciList);

                        // Récupération des écritures comptables
                        ecritureComptables = GetEcritureFromAnaelDb(sourceDB, query);
                    }
                }

                // L'appel provenant de l'écran de clôture des périodes en masse, on ne doit pas importé les écritures comptable anterieur au mois d'Octobre 2018
                // Il existe (dans ANAEL) une écriture qui regroupe toutes les écritures anterieur à cette date.
                if (ecritureComptables.Count != 0)
                {
                    await EcritureComptableImportManager.ManageImportEcritureComptablesAsync(cloturePeriodeModel.SocieteId.Value, cloturePeriodeModel.DateComptable, ecritureComptables.Where(q => q.DateComptable >= new DateTime(2018, 10, 1)), string.Empty).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw CreateExceptionWithMessageAndInnerException(ex, IEBusiness.FluxErreurRecuperation); 
            }

            return ecritureComptables;
        }

        public virtual Task<IEnumerable<EcritureComptableFtpSapModel>> ImportEcritureComptableAsync(List<EcritureComptableFayatTpImportModel> ecritureComptableFayatTps) => throw new NotImplementedException();

        private void SetFlux(string codeFlux)
        {
            Flux = FluxManager.GetByCode(codeFlux);
        }

        private EcritureComptableDto CreateEcritureComptableDto(IDataReader dataReader)
        {
            string dws = Convert.ToString(dataReader["Dws"]);
            string dint = Convert.ToString(dataReader["Dint"]);
            string dnolig = Convert.ToString(dataReader["Dnolig"]);
            string ligana = Convert.ToString(dataReader["Ligana"]);
            string numeroPiece = $"{ dws }_{ dint }_{ dnolig }_{ ligana }";

            return new EcritureComptableDto
            {
                DateComptable = Convert.ToDateTime(dataReader["DateComptable"]),
                Libelle = Convert.ToString(dataReader["Libelle"]),
                NumeroPiece = numeroPiece,
                AnaelCodeNature = Convert.ToString(dataReader["AnaelCodeNature"]),
                Montant = Convert.ToDecimal(dataReader["Montant"], new CultureInfo("en-US")),
                AnaelCodeCi = Convert.ToString(dataReader["AnaelCodeCi"]),
                AnaelCodeCommande = Convert.ToString(dataReader["AnaelCodeCommande"]),
                AnaelCodeJournal = Convert.ToString(dataReader["AnaelCodeJournal"]),
                CodeDevise = Convert.ToString(dataReader["Devise"]),
                MontantDevise = Convert.ToDecimal(dataReader["MontantDevise"], new CultureInfo("en-US"))
            };
        }

        private string FormatDateForAs400(DateTime dateToFormat)
        {
            return dateToFormat.ToString("yyyy-MM-dd");
        }

        private string FormatDate(DateTime dateToFormat)
        {
            return dateToFormat.ToLocalTime().ToString("yyyyMMdd");
        }

        private string TransformDate(DateTime dateTime)
        {
            return "2" + dateTime.Date.Year.ToString().Substring(2, 2) + (dateTime.Date.DayOfYear - 1).ToString("000") + "00000000";
        }

        private FredBusinessException CreateExceptionWithMessageAndInnerException(Exception ex, string errorTitle)
        {
            string msg = string.Format(errorTitle, Flux.Code);
            FredBusinessException exception = new FredBusinessException(msg, ex);
            NLog.LogManager.GetCurrentClassLogger().Error(exception, exception.Message);

            return exception;
        }

        private FredBusinessException CreateExceptionWithMessage(string errorTitle)
        {
            string msg = string.Format(errorTitle, Flux.Code);
            FredBusinessException exception = new FredBusinessException(msg);
            NLog.LogManager.GetCurrentClassLogger().Error(exception, msg);

            return exception;
        }
    }
}
