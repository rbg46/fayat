using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Business.CI;
using Fred.Business.Referential;
using Fred.Entities.CI;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.DataAccess.Interfaces;
using Hangfire;

namespace Fred.ImportExport.Business.Stair.ExportCI
{
    /// <summary>
    /// StairCiFluxManager
    /// </summary>
    public class StairCiFluxManager : AbstractFluxManager
    {
        private readonly ICIManager cIManager;
        private readonly IEtablissementComptableManager etabComptaManager;
        private readonly IFluxRepository fluxRepository;

        private readonly string pathOut = ConfigurationManager.AppSettings["Stair:Export:PathOutput"];
        public string ExportJobId { get; } = ConfigurationManager.AppSettings["flux:stair:export:ci"];

        public StairCiFluxManager(
            IFluxManager fluxManager,
            ICIManager cIManager,
            IEtablissementComptableManager etabComptaManager,
            IFluxRepository fluxRepository)
            : base(fluxManager)
        {
            this.cIManager = cIManager;
            this.etabComptaManager = etabComptaManager;
            this.fluxRepository = fluxRepository;
        }

        public void ExecuteExport()
        {
            BackgroundJob.Enqueue(() => Export());
        }

        public void ScheduleExport(string cron)
        {
            if (!string.IsNullOrEmpty(cron))
            {
                RecurringJob.AddOrUpdate(ExportJobId, () => Export(), cron);
            }
            else
            {
                string msg = $"Le CRON n'est pas paramétré pour le job {ExportJobId}";
                var exception = new FredBusinessException(msg);
                NLog.LogManager.GetCurrentClassLogger().Error(exception, msg);
                throw exception;
            }
        }

        [AutomaticRetry(Attempts = 0, LogEvents = true)]
        [DisplayName("[EXPORT] STAIR CI")]
        [DisableConcurrentExecution(ConcurrentExecutionTimeoutInSeconds)]
        public async Task Export()
        {
            string groupCode = await fluxRepository.GetGroupCodeByFluxCodeAsync(ExportJobId);

            await JobRunnerApiRestHelper.PostAsync("ExportStairCi", groupCode);
        }

        public void ExportJob()
        {
            IEnumerable<CIEnt> cIs = FilterCIExport(cIManager.GetCIList());
            using (var sw = new StreamWriter(pathOut + "PROJETS_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv", append: true))
            {
                foreach (CIEnt ci in cIs)
                {
                    sw.WriteLine(WriteCI(ci));
                }
            }
        }

        private IEnumerable<CIEnt> FilterCIExport(IEnumerable<CIEnt> input)
        {
            string[] filters = ConfigurationManager.AppSettings.AllKeys.Where(k => k.Contains("Stair:ExportCI:Filter:")).ToArray();

            IEnumerable<CIEnt> cIs = FiltreCIExportGroupeID(input, filters.Where(f => f.Contains("GroupeID")).ToArray());

            cIs = FiltreCIExportSocieteEtab(cIs, filters.Where(f => f.Contains("Societe:")).ToArray());

            return cIs;

        }

        private IEnumerable<CIEnt> FiltreCIExportSocieteEtab(IEnumerable<CIEnt> input, string[] filters)
        {
            IEnumerable<CIEnt> cis = input;

            foreach (string filter in filters)
            {
                string societe = filter.Replace("Stair:ExportCI:Filter:Societe:", string.Empty);
                string[] etablissement = ConfigurationManager.AppSettings[filter].Split(',');

                cis = cis.Where(ci => !(ci.Societe.CodeSocieteComptable == societe && etablissement.Contains(ci.EtablissementComptable?.Code)));
            }

            return cis;

        }

        private IEnumerable<CIEnt> FiltreCIExportGroupeID(IEnumerable<CIEnt> input, string[] filter)
        {
            return input.Where(ci => ci.Societe?.GroupeId == int.Parse(ConfigurationManager.AppSettings[filter[0]]));
        }

        private string WriteCI(CIEnt ci)
        {
            var ligne = new StringBuilder();
            ligne.Append(value: (ci.Societe == null ? string.Empty : ci.Societe.CodeSocieteComptable ?? string.Empty) + ";");
            ligne.Append(value: (ci.Societe == null ? string.Empty : ci.Societe.Libelle) + ";");
            TestEtablissementNeeded(ci, ligne);
            ligne.Append(value: (ci.Code ?? string.Empty) + ";");
            ligne.Append(value: (ci.Libelle ?? string.Empty) + ";");
            ligne.Append(value: (ci.DateOuverture == null ? string.Empty : ci.DateOuverture.Value.ToString("yyyyMMdd")) + "; ");
            ligne.Append(value: (ci.DateFermeture == null ? string.Empty : ci.DateFermeture.Value.ToString("yyyyMMdd")));
            return ligne.ToString();
        }

        private void TestEtablissementNeeded(CIEnt ci, StringBuilder ligne)
        {
            string codeSocieteComptableCI = ci.Societe.CodeSocieteComptable ?? string.Empty;
            string codeEtablissementComptableCI = ci.EtablissementComptable == null ? string.Empty : ci.EtablissementComptable.Code;

            if (ConfigurationManager.AppSettings["Stair:ExportCI:Societe:EtablissementNeeded"].Split(',').Contains(codeSocieteComptableCI) && string.IsNullOrEmpty(codeEtablissementComptableCI))
            {
                ligne.Append(value: (ci.Code.Substring(1, 2)) + ";");
                int? idEtabComptab = etabComptaManager.GetEtablissementComptableIdByCode(ci.Code.Substring(1, 2));
                if (idEtabComptab == null)
                {
                    ligne.Append(value: (ci.EtablissementComptable == null ? string.Empty : ci.EtablissementComptable.Libelle) + ";");
                }
                else
                {
                    ligne.Append(value: (etabComptaManager.GetEtablissementComptableById((int)idEtabComptab).Libelle) + ";");
                }

            }
            else
            {
                ligne.Append(value: (ci.EtablissementComptable == null ? string.Empty : ci.EtablissementComptable.Code) + ";");
                ligne.Append(value: (ci.EtablissementComptable == null ? string.Empty : ci.EtablissementComptable.Libelle) + ";");
            }
        }
    }
}
