using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Fred.Business.CI;
using Fred.Business.IndemniteDeplacement;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.IndemniteDeplacement;
using Fred.Framework.Exceptions;
using Fred.Framework.Tool;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.DataAccess.Common;
using Fred.ImportExport.DataAccess.Interfaces;
using Hangfire;
using NLog;

namespace Fred.ImportExport.Business.Kilometre
{
    public class KlmFluxManager : AbstractFluxManager
    {
        private static Logger Logger => LogManager.GetCurrentClassLogger();

        #region Paramètres des fichiers sql

        // Paramètres de SELECT_RVG_KLM_CHANTIER_FRED_EXISTS.sql
        // {0} : liste des CI à prendre en compte au format 'CI1' [, 'CI2' [, 'CI3']]

        // Paramètres de INSERT_ANAEL_RZB_KLM.sql
        //  {0} : Code affaire
        //  {1} : Matricule personnel
        //  {2} : La source : RVG ou FRED
        //  {3} : Code déplacement
        //  {4} : Code zone
        //  {5} : Kilométrage réel domicile / chantier
        //  {6} : Véhiculé
        //  {7} : IVD
        //  {8} : Kilométrage oiseau rattachement / chantier
        //  {9} : Kilométrage oiseau domicile / chantier
        // {10} : Société de paie

        #endregion

        private const string SelectRvgKlmChantierFredExistsSqlScriptPath = "Kilometre.SELECT_RVG_KLM_CHANTIER_FRED_EXISTS.sql";
        private const string SelectRvgKlmNoChantierFredExistsSqlScriptPath = "Kilometre.SELECT_RVG_KLM_NO_CHANTIER_FRED_EXISTS.sql";
        private const string InitAnaelKlmSqlScriptPath = "Kilometre.INIT_ANAEL_KLM.sql";
        private const string InsertAnaelKlmHeaderSqlScriptPath = "Kilometre.INSERT_ANAEL_KLM_HEADER.sql";
        private const string InsertAnaelKlmToFormat = "('KLM', '{10}', '', '{0}_{1}', '{0}_{1}_{2}', '', '{3}', '{4}', '', '', '',{5},{6},{7},{8},{9})";
        private const int SocieteRazelBecId = 1;
        private const int GroupeRazelBecId = 1;
        private const int AnaelInsertBlock = 100;
        private const int RvgCommandeTimeout = 10 * 60;

        private readonly string anaelConnectionString = ConfigurationManager.AppSettings["ANAEL:ConnectionString:Groupe:GRZB"];
        private readonly string rvgConnectionString = ConfigurationManager.AppSettings["RVG:ConnectionString:Groupe:GRZB"];
        private readonly IIndemniteDeplacementManager indemniteDeplacementManager;
        private readonly ICIManager ciManager;
        private readonly IFluxRepository fluxRepository;

        public string ImportJobId => ConfigurationManager.AppSettings["flux:klm"];

        public KlmFluxManager(
            IFluxManager fluxManager,
            IIndemniteDeplacementManager indemniteDeplacementManager,
            ICIManager ciManager,
            IFluxRepository fluxRepository)
            : base(fluxManager)
        {
            Flux = FluxManager.GetByCode(ImportJobId);

            this.indemniteDeplacementManager = indemniteDeplacementManager;
            this.ciManager = ciManager;
            this.fluxRepository = fluxRepository;
        }

        public void ExecuteExport()
        {
            BackgroundJob.Enqueue(() => Export());
        }

        public void ScheduleExport(string cron)
        {
            if (string.IsNullOrEmpty(cron))
            {
                string msg = $"Le CRON n'est pas paramétré pour le job {ImportJobId}";
                var exception = new FredBusinessException(msg);
                Logger.Error(exception, msg);
                throw exception;
            }

            RecurringJob.AddOrUpdate(ImportJobId, () => Export(), cron);
        }

        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[EXPORT] Indemnités de déplacement (FRED & RVG => ANAEL")]
        [DisableConcurrentExecution(ConcurrentExecutionTimeoutInSeconds)]
        public async Task Export()
        {
            string groupCode = await fluxRepository.GetGroupCodeByFluxCodeAsync(ImportJobId);

            await JobRunnerApiRestHelper.PostAsync("ExportKlm", groupCode);
        }

        public void ExportJob()
        {
            if (Flux == null)
            {
                var sb = new StringBuilder(string.Format(IEBusiness.FluxInconnu, ImportJobId));
                sb.AppendLine();

                NameValueCollection appSettings = ConfigurationManager.AppSettings;
                foreach (object appSetting in appSettings)
                {
                    if (appSetting is string key)
                    {
                        if (key.ToLower().StartsWith("flux"))
                        {
                            string value = ConfigurationManager.AppSettings[key];
                            sb.AppendLine(key + " : " + value);
                        }
                    }
                }

                string msg = sb.ToString();
                var exception = new FredBusinessException(msg);
                Logger.Error(exception, msg);
                throw exception;
            }

            if (!Flux.IsActif)
            {
                string msg = string.Format(IEBusiness.FluxInactif, ImportJobId);
                var exception = new FredBusinessException(msg);
                Logger.Error(exception, msg);
                throw exception;
            }

            try
            {
                Logger.Info("FLUX KLM : Début de l'export FRED & RVG -> ANAEL");

                var anaelIndemniteDeplacements = new List<AnaelIndemniteDeplacement>();

                Logger.Debug($"FLUX KLM : Chaine de connexion a ANAEL : {anaelConnectionString}");
                Logger.Debug($"FLUX KLM : Chaine de connexion a RVG : {rvgConnectionString}");
                Logger.Debug($"FLUX KLM : Chaine de connexion a FRED : {ConfigurationManager.ConnectionStrings["FredConnection"]}");

                TraiteFredIndemniteDeplacement(anaelIndemniteDeplacements);

                TraiteRvgIndemniteDeplacement(rvgConnectionString, anaelIndemniteDeplacements);

                EnregistreDansAnael(anaelConnectionString, anaelIndemniteDeplacements);

                Flux.DateDerniereExecution = DateTime.UtcNow;
                this.FluxManager.Update(Flux);
            }
            catch (Exception e)
            {
                string msg = string.Format(IEBusiness.FluxErreurImport, ImportJobId);
                var exception = new FredBusinessException(msg, e);
                Logger.Error(exception, exception.Message);
                throw exception;
            }
        }

        private void TraiteFredIndemniteDeplacement(List<AnaelIndemniteDeplacement> anaelIndemniteDeplacements)
        {
            Logger.Info("FLUX KLM : FRED : >>>> Debut de la récupération des indemnités de déplacements");

            // Seuls les chantiers gérés par FRED sont traités ici
            // Si des doublons existent, la date du dernier calcule fait foi
            // Note : comme cette date est nullable, c'est l'id de l'indemnité de déplacement qui fera foi...
            var indemniteDeplacementsToProcess = new List<IndemniteDeplacementEnt>();
            foreach (IndemniteDeplacementEnt indemniteDeplacement in indemniteDeplacementManager.GetIndemniteDeplacementForExportKlm(SocieteRazelBecId))
            {
                IndemniteDeplacementEnt existing = indemniteDeplacementsToProcess.FirstOrDefault(a => a.CI.Code == indemniteDeplacement.CI.Code && a.Personnel.Matricule == indemniteDeplacement.Personnel.Matricule);
                if (existing != null)
                {
                    if (existing.IndemniteDeplacementId < indemniteDeplacement.IndemniteDeplacementId)
                    {
                        indemniteDeplacementsToProcess.Remove(existing);
                        indemniteDeplacementsToProcess.Add(indemniteDeplacement);
                    }
                }
                else
                {
                    indemniteDeplacementsToProcess.Add(indemniteDeplacement);
                }
            }

            Logger.Info($"FLUX KLM : FRED : Nombre d'indemnités de déplacement à envoyer à ANAEL : {indemniteDeplacementsToProcess.Count}");
            foreach (IndemniteDeplacementEnt indemniteDeplacement in indemniteDeplacementsToProcess)
            {
                AnaelIndemniteDeplacement iDaNael = AnaelIndemniteDeplacement.FromFred(indemniteDeplacement);
                anaelIndemniteDeplacements.Add(iDaNael);
                Logger.Trace($"FLUX KLM : FRED : A envoyer à ANAEL : id = {indemniteDeplacement.IndemniteDeplacementId}, code affaire = {iDaNael.CodeAffaire}, code deplacement = {iDaNael.CodeDeplacement}, Matricule personnel = {iDaNael.MatriculePersonnel}");
            }

            Logger.Info("FLUX KLM : FRED : <<<< Fin de la récupération des indemnités de déplacement");
        }

        private void TraiteRvgIndemniteDeplacement(string connexionChaine, List<AnaelIndemniteDeplacement> anaelIndemniteDeplacements)
        {
            Logger.Info("FLUX KLM : RVG : >>>> Debut de la récupération des indemnités de déplacements");

            var anaelIndemniteDeplacementsToProcess = new List<AnaelIndemniteDeplacement>();

            // Seuls les chantiers RVG non gérés par FRED sont traités ici
            // Si des doublons existent, le 1er ajouté sera le celui utilisé
            string rvgQuery = GetRvgQuery();

            Logger.Debug($"FLUX KLM : RVG : Requête utilisée : {rvgQuery}");

            using (DataAccess.Common.Database rvgDatabase = DatabaseFactory.GetNewDatabase(TypeBdd.SqlServer, connexionChaine))
            {
                rvgDatabase.CommandeTimeout = RvgCommandeTimeout;
                using (IDataReader reader = rvgDatabase.ExecuteReader(rvgQuery))
                {
                    while (reader.Read())
                    {
                        AnaelIndemniteDeplacement anaelIndemniteDeplacement = AnaelIndemniteDeplacement.FromRvg(reader);
                        AnaelIndemniteDeplacement existing = anaelIndemniteDeplacementsToProcess.FirstOrDefault(a => a.CodeAffaire == anaelIndemniteDeplacement.CodeAffaire && a.MatriculePersonnel == anaelIndemniteDeplacement.MatriculePersonnel);
                        if (existing == null)
                        {
                            anaelIndemniteDeplacementsToProcess.Add(anaelIndemniteDeplacement);
                        }
                    }
                }
            }

            Logger.Info($"FLUX KLM : RVG : Nombre d'indemnités de déplacement à envoyer à ANAEL : {anaelIndemniteDeplacementsToProcess.Count}");
            foreach (AnaelIndemniteDeplacement anaelIndemniteDeplacement in anaelIndemniteDeplacementsToProcess)
            {
                anaelIndemniteDeplacements.Add(anaelIndemniteDeplacement);
                Logger.Trace($"FLUX KLM : RVG : A envoyer à ANAEL : code affaire : {anaelIndemniteDeplacement.CodeAffaire}, code deplacement {anaelIndemniteDeplacement.CodeDeplacement}, Matricule Personnel {anaelIndemniteDeplacement.MatriculePersonnel}");
            }

            Logger.Info("FLUX KLM : RVG : <<<< Fin de la récupération des indemnités de déplacements");
        }

        private string GetRvgQuery()
        {
            // Récupère la liste de CI gérés par FRED pour les ignorer dans la requête
            List<CIEnt> ciChantierFreds = ciManager.GetCIList(true, GroupeRazelBecId).ToList();

            Logger.Info($"FLUX KLM : RVG : Nombre de CI FRED ignorés : {ciChantierFreds.Count}");

            if (ciChantierFreds.Count == 0)
            {
                // Récupère la requête de sélection des indemnités de déplacement dans RVG lorsqu'aucun chantier géré par FRED existe
                string selectRvgKlmSqlScript = SqlScriptProvider.GetEmbeddedSqlScriptContent(Assembly.GetExecutingAssembly(), SelectRvgKlmNoChantierFredExistsSqlScriptPath);
                return selectRvgKlmSqlScript;
            }
            else
            {
                // Récupère la requête de sélection des indemnités de déplacement dans RVG lorsqu'au moins un chantier géré par FRED existe
                string selectRvgKlmSqlScriptToFormat = SqlScriptProvider.GetEmbeddedSqlScriptContent(Assembly.GetExecutingAssembly(), SelectRvgKlmChantierFredExistsSqlScriptPath);

                // Création des paramètres de cette requête
                var rvgRequeteParametre0 = new StringBuilder();
                for (int i = 0; i < ciChantierFreds.Count; i++)
                {
                    if (i > 0)
                    {
                        rvgRequeteParametre0.Append(", ");
                    }
                    rvgRequeteParametre0.Append("'");
                    rvgRequeteParametre0.Append(ciChantierFreds[i].Code);
                    rvgRequeteParametre0.Append("'");
                }

                return string.Format(selectRvgKlmSqlScriptToFormat, rvgRequeteParametre0);
            }
        }

        private void EnregistreDansAnael(string connexionChaine, List<AnaelIndemniteDeplacement> anaelIndemniteDeplacements)
        {
            Logger.Info("FLUX KLM : ANAEL : >>>> Début de l'envoi des données");
            Logger.Info($"FLUX KLM : ANAEL : Nombre d'indemnités de déplacements à envoyer : {anaelIndemniteDeplacements.Count}");

            if (anaelIndemniteDeplacements.Count > 0)
            {
                using (DataAccess.Common.Database anaelDatabase = DatabaseFactory.GetNewDatabase(TypeBdd.Db2, connexionChaine))
                {
                    try
                    {
                        EnregistreDansAnael(anaelDatabase, anaelIndemniteDeplacements);
                    }
                    catch (Exception ex)
                    {
                        string msg = $"Erreur durant l'export vers l'AS400 pour le job {ImportJobId}";
                        var exception = new FredBusinessException(msg, ex);

                        if (ex is AggregateException aggregateException)
                        {
                            foreach (Exception innerException in aggregateException.InnerExceptions)
                            {
                                Logger.Error(innerException, msg);
                            }
                        }

                        Logger.Error(exception, msg);
                        throw exception;
                    }
                }
            }

            Logger.Info("FLUX KLM : ANAEL : <<<< Fin de l'envoi des données");
        }

        private void EnregistreDansAnael(DataAccess.Common.Database database, List<AnaelIndemniteDeplacement> anaelIndemniteDeplacements)
        {
            // Récupère et exécute la requête d'initialisation d'ANAEL avant insertion
            string initAnaelKlmSqlScript = SqlScriptProvider.GetEmbeddedSqlScriptContent(Assembly.GetExecutingAssembly(), InitAnaelKlmSqlScriptPath);
            Logger.Debug($"FLUX KLM : ANAEL : Initialisation avant insertion : {initAnaelKlmSqlScript}");
            database.ExecuteNonQuery(initAnaelKlmSqlScript);
            Logger.Debug("FLUX KLM : ANAEL : Initialisation réussie");
            // Récupère l'en-tête de la requête d'insertion des indemnités de déplacement dans ANAEL
            string insertAnaelKlmHeaderSqlScript = SqlScriptProvider.GetEmbeddedSqlScriptContent(Assembly.GetExecutingAssembly(), InsertAnaelKlmHeaderSqlScriptPath);

            var fullQuery = new StringBuilder(insertAnaelKlmHeaderSqlScript);

            Logger.Debug($"FLUX KLM : Entête de la requête d'insertion : {fullQuery}");

            var exceptions = new List<FredBusinessException>();
            int pageSize = AnaelInsertBlock;
            var count = 0;

            while (anaelIndemniteDeplacements.Count > count)
            {
                AppendAnaelSingleQuery(fullQuery, anaelIndemniteDeplacements.Skip(count).Take(pageSize));
                count += pageSize;

                try
                {
                    database.ExecuteNonQuery(fullQuery.ToString());
                    Logger.Debug($"FLUX KLM : ANAEL : Execution réussie : {fullQuery}");
                }
                catch (Exception ex)
                {
                    exceptions.Add(new FredBusinessException(fullQuery.ToString(), ex));
                }

                fullQuery = new StringBuilder(insertAnaelKlmHeaderSqlScript);
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException("Encountered errors while trying to do something.", exceptions);
            }
        }

        private void AppendAnaelSingleQuery(StringBuilder fullQuery, IEnumerable<AnaelIndemniteDeplacement> anaelIndemniteDeplacements)
        {
            string queryValuesClause = string.Join(",", GetQueryValues());
            fullQuery.Append(queryValuesClause);

            IEnumerable<string> GetQueryValues() => anaelIndemniteDeplacements.Select(aid => GetQueryValue(aid));
        }

        private string GetQueryValue(AnaelIndemniteDeplacement aid)
        {
            return string.Format(InsertAnaelKlmToFormat,
                aid.CodeAffaire,
                aid.MatriculePersonnel,
                GetSourceForAnaelRequest(),
                aid.CodeDeplacement,
                aid.CodeZoneDeplacement,
                Math.Round(aid.KilometrageReelDomicileChantier),
                aid.Vehicule ? "1" : "0",
                aid.IVD ? "1" : "0",
                Math.Round(aid.KilometrageVolOiseauChantierRattachement),
                Math.Round(aid.KilometrageVolOiseauDomicileChantier),
                aid.SocietePaie);

            string GetSourceForAnaelRequest()
            {
                switch (aid.Source)
                {
                    case AnaelIndemniteDeplacement.SourceType.FRED:
                        return "F";
                    case AnaelIndemniteDeplacement.SourceType.RVG:
                        return "R";
                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }
}
