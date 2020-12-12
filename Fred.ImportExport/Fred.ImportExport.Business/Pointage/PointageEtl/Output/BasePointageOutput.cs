using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Fred.Framework.Services;
using Fred.ImportExport.Business.ApplicationSap;
using Fred.ImportExport.Business.Pointage.PointageEtl.Parameter;
using Fred.ImportExport.Business.Pointage.PointageEtl.Settings;
using Fred.ImportExport.Business.Pointage.PointageEtl.Transform;
using Fred.ImportExport.Framework.Etl.Output;
using Fred.ImportExport.Framework.Etl.Result;
using NLog;

namespace Fred.ImportExport.Business.Pointage.PointageEtl.Output
{
    /// <summary>
    /// Outupt vers SAP des pointages personnels
    /// </summary>
    internal class BasePointageOutput : IEtlOutput<ExportRapportLigneModel>
    {

        private readonly string logLocation = "[SAP OUTPUT]";

        private readonly EtlPointageParameter parameter;
        private readonly EtlExecutionHelper etlExecutionHelper;

        public BasePointageOutput(EtlPointageParameter parameter, EtlExecutionHelper etlExecutionHelper)
        {
            this.parameter = parameter;
            this.etlExecutionHelper = etlExecutionHelper;
        }

        public async Task ExecuteAsync(IEtlResult<ExportRapportLigneModel> result)
        {
            if (result?.Items.Count > 0)
            {
                LogInfo($"{parameter.LogPrefix} : {logLocation} : INFO : Envoie vers SAP (RapportId = {parameter.RapportId})(société={parameter.SocieteLibelle})");

                var applicationSapParameter = this.parameter.EtlDependencies.ApplicationsSapManager.GetParametersForSociete(parameter.SocieteId);

                if (applicationSapParameter.IsFound)
                {
                    var uri = new Uri(applicationSapParameter.Url);
                    var mandant = HttpUtility.ParseQueryString(uri.Query).Get("SAP-CLIENT");

                    var flux = "CAT2";
                    var logicielSap = this.parameter.EtlDependencies.LogicielTiersManager.GetOrCreateLogicielTiers("SAP", uri.Host, mandant);

                    //On log dans la base de données même si l'envoi a SAP échoue 
                    if (result.Items.Any())
                    {
                        var lignesAHistoriser = parameter.ListRapportLignesEnt.Where(p => result.Items.Select(l => l.RapportLigneId).Contains(p.RapportLigneId));
                        parameter.EtlDependencies.WorkflowPointageManager.SaveWorkflowPointage(lignesAHistoriser, logicielSap.FredLogicielTiersId, parameter.AuteurId, parameter.BackgroundJobId, flux);

                        LogInfo($"{parameter.LogPrefix} : {logLocation} : INFO : Log de la ligne du rapport dans la base de données (RapportId = {parameter.RapportId})(société={parameter.SocieteLibelle})");

                        await SendToSapAsync(result.Items, applicationSapParameter);

                        LogInfo($"{parameter.LogPrefix} : {logLocation} : SUCCESS : société({parameter.SocieteId})({parameter.SocieteLibelle}).");
                    }
                    else
                    {
                        LogInfo($"{parameter.LogPrefix} : {logLocation} : INFO : Aucune ligne à envoyer (RapportId = {parameter.RapportId})(société={parameter.SocieteLibelle})");
                    }

                }
                else
                {
                    LogError($"{parameter.LogPrefix} : {logLocation} : ERROR : Il n'y a pas de configuration correspondant à cette société({parameter.SocieteId})({parameter.SocieteLibelle}).");
                }
            }
            else
            {
                LogInfo($"{parameter.LogPrefix} : {logLocation} : INFO : Pas d'envoie vers SAP, aucun pointage personnel (RapportId = {parameter.RapportId})(société={parameter.SocieteLibelle}) ");
            }
        }

        private async Task SendToSapAsync(IEnumerable<ExportRapportLigneModel> result, ApplicationSapParameter applicationSapParameter)
        {
            var urlSap = $"{applicationSapParameter.Url}&ACTION=CAT2";

            etlExecutionHelper.Log($"{parameter.LogPrefix} : {logLocation} : INFO : URL SAP ({urlSap})");

            var restClient = new RestClient(applicationSapParameter.Login, applicationSapParameter.Password);

            etlExecutionHelper.LogAndSerialize($"{parameter.LogPrefix} : {logLocation} : INFO : Data envoyés à SAP : ", result);

            await restClient.PostAndEnsureSuccessAsync(urlSap, result);
        }

        protected void LogError(string error)
        {
            etlExecutionHelper.Log(error);
            LogManager.GetCurrentClassLogger()
             .Error(error);
        }

        protected void LogInfo(string info)
        {
            etlExecutionHelper.Log(info);
            LogManager.GetCurrentClassLogger()
             .Info(info);
        }
    }
}
