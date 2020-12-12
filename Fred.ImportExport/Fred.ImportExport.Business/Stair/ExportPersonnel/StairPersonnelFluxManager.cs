using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Fred.Business.Personnel;
using Fred.Business.Referential;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Hangfire;
using Fred.ImportExport.DataAccess.Interfaces;
using Hangfire;

namespace Fred.ImportExport.Business.Stair.ExportPersonnel
{
    public class StairPersonnelFluxManager : AbstractFluxManager
    {
        private readonly IPersonnelManager personnelManager;
        private readonly IEtablissementPaieManager etablissementPaieManager;
        private readonly IFluxRepository fluxRepository;
        private readonly string pathOut = ConfigurationManager.AppSettings["Stair:Export:PathOutput"];

        public string ExportJobId { get; } = ConfigurationManager.AppSettings["flux:stair:export:personnel"];

        public StairPersonnelFluxManager(
            IFluxManager fluxManager,
            IPersonnelManager personnelManager,
            IEtablissementPaieManager etablissementPaieManager,
            IFluxRepository fluxRepository)
            : base(fluxManager)
        {
            this.personnelManager = personnelManager;
            this.etablissementPaieManager = etablissementPaieManager;
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
        [DisplayName("[EXPORT] STAIR Personnel RZB")]
        [DisableConcurrentExecution(ConcurrentExecutionTimeoutInSeconds)]
        public async Task Export()
        {
            string groupCode = await fluxRepository.GetGroupCodeByFluxCodeAsync(ExportJobId);

            await JobRunnerApiRestHelper.PostAsync("ExportStairPersonnel", groupCode);
        }

        public void ExportJob()
        {
            IEnumerable<PersonnelEnt> personnels = personnelManager.GetPersonnelList();
            var ligne = new StringBuilder();

            using (var sw = new StreamWriter(pathOut + "CONTACTS_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv", append: true))
            {
                foreach (PersonnelEnt personnel in personnels)
                {
                    if (personnel.Societe.GroupeId == 1)
                    {
                        EtablissementPaieEnt etablissementPaie = null;
                        // traitement particulier GEOBIO (1600) & MOULIN BTP (550)
                        if (personnel.EtablissementPaieId != null && (personnel.Societe.CodeSocieteComptable == "1600" || personnel.Societe.CodeSocieteComptable == "550"))
                        {
                            etablissementPaie = etablissementPaieManager.GetEtablissementPaieById((int)personnel.EtablissementPaieId);
                            etablissementPaie.Code = "01";
                        }
                        else if (personnel.EtablissementPaieId != null)
                        {
                            etablissementPaie = etablissementPaieManager.GetEtablissementPaieById((int)personnel.EtablissementPaieId);
                        }
                        WritePersonnel(ligne, etablissementPaie, sw, personnel);
                    }
                }
            }
        }

        private static void WritePersonnel(StringBuilder ligne, EtablissementPaieEnt etablissementPaie, StreamWriter sw, PersonnelEnt personnel)
        {
            ligne.Clear();
            ligne.Append((personnel.Societe.CodeSocieteComptable ?? string.Empty) + ";");
            ligne.Append((personnel.Societe == null ? string.Empty : personnel.Societe.Libelle) + ";");
            ligne.Append((etablissementPaie == null ? string.Empty : etablissementPaie.Code) + ";");
            ligne.Append((etablissementPaie == null ? string.Empty : etablissementPaie.Libelle) + ";");
            ligne.Append(((personnel.Matricule ?? string.Empty) + ";"));
            ligne.Append((personnel.NomPrenom ?? string.Empty) + ";");
            ligne.Append((personnel.DateEntree == null ? string.Empty : personnel.DateEntree.Value.Date.ToString("yyyyMMdd")) + ";");
            ligne.Append((personnel.DateSortie == null ? string.Empty : personnel.DateSortie.Value.Date.ToString("yyyyMMdd")));
            sw.WriteLine(ligne);
        }
    }
}
