using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Net.Http;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Framework.Etl.Input;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models.Stair.Sphinx;

namespace Fred.ImportExport.Business.Stair.ImportSphinxEtl.Input
{
    public class SphinxEtlInput : IEtlInput<SphinxApiModel>
    {

        private readonly string url = ConfigurationManager.AppSettings["Stair:Sphinx:url"];
        private readonly string auth = ConfigurationManager.AppSettings["Stair:Sphinx:auth"];
        private readonly string urlSurveyName = ConfigurationManager.AppSettings["Stair:Sphinx:urlSurveyName"];
        private readonly string identifiant = ConfigurationManager.AppSettings["Stair:Sphinx:login"];
        private readonly string token = ConfigurationManager.AppSettings["Stair:Sphinx:token"];
        private RestClientSphinx client;

        public IList<SphinxApiModel> Items { get; set; }

        public void Execute()
        {
            Items = GetSphinxApiModels();
        }

        private List<SphinxApiModel> GetSphinxApiModels()
        {
            var sphinxApiModels = new List<SphinxApiModel>();
            HttpResponseMessage message;

            try
            {
                client = new RestClientSphinx(identifiant, token, url + auth, "FR", "sphinxapiclient");
                message = client.Get(url + urlSurveyName);
            }
            catch(Exception ex)
            {

                throw new FredIeBusinessException(string.Format(IEBusiness.FluxErreurRecuperation, "Erreur de connexion à Sphinx Identifiant ou mot de passe incorrect"), ex);

            }
            
            string surveysName = message.Content.ReadAsStringAsync().Result;

            foreach (string l in surveysName.Split(']').Where(s => s.Contains("surveyName")))
            {
                string surveyName = ExtractSurveyName(l);
                Stream surveyData = GetSurveyDataBySurveyName(surveyName);
                sphinxApiModels.Add(NewSphinxApiModel(l, surveyData));
            }

            return sphinxApiModels;
        }

        private SphinxApiModel NewSphinxApiModel(string surveyName_, Stream surveyData_)
        {
            return new SphinxApiModel()
            {
                SurveyName = surveyName_,
                SurveyData = surveyData_
            };


        }

        private Stream GetSurveyDataBySurveyName(string surveyName)
        {
            HttpResponseMessage message = client.Get(url + urlSurveyName + '/' + surveyName + "/data");
            return message.Content.ReadAsStreamAsync().Result;
        }

        private string ExtractSurveyName(string resultContent)
        {
            resultContent = resultContent.Split(',')[0];
            resultContent = resultContent.Split(':')[1];
            resultContent = resultContent.Replace("\\", string.Empty).Replace("\"", string.Empty);
            return resultContent;
        }
    }
}
