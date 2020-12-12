using System;
using System.Collections.Generic;
using System.Text;
using Fred.ImportExport.Business.Stair.ImportSphinxEtl.Result;
using Fred.ImportExport.Framework.Etl.Input;
using Fred.ImportExport.Framework.Etl.Result;
using Fred.ImportExport.Framework.Etl.Transform;
using Fred.ImportExport.Models.Stair.Sphinx;
using Microsoft.VisualBasic.FileIO;

namespace Fred.ImportExport.Business.Stair.ImportSphinxEtl.Transform
{
    public class SphinxEtlTransform : IEtlTransform<SphinxApiModel, SphinxFormulaireModel>
    {

        private readonly List<SphinxFormulaireModel> formulaires = new List<SphinxFormulaireModel>();
        private readonly List<SphinxQuestionModel> questions =  new List<SphinxQuestionModel>();
        public void Execute(IEtlInput<SphinxApiModel> input, ref IEtlResult<SphinxFormulaireModel> result)
        {
            if (result == null)
            {
                result = new SphinxEtlResult();
            }
            foreach (var sphinxApiModel in input.Items)
            {

                SphinxApiModelToBddModel(sphinxApiModel);
                result.Items.Add(NewFormulaire(sphinxApiModel));
            }
        }

        private void SphinxApiModelToBddModel(SphinxApiModel sphinxApiModel)
        {
                using (TextFieldParser csvParser = new TextFieldParser(sphinxApiModel.SurveyData, Encoding.UTF8))
                {
                    csvParser.SetDelimiters(new string[] { ";" });
                    csvParser.HasFieldsEnclosedInQuotes = true;
                    NewQuestion(csvParser);
                int k = 0;

                while (!csvParser.EndOfData)
                    {
                        string[] reponsesCSV = csvParser.ReadFields();
                        int i = 0;
                        foreach (string reponse in reponsesCSV)
                        {
                            int j = i++ % reponsesCSV.Length;
                        if (j == 0)
                        {
                            k++;
                        }

                        questions[j].Reponses.Add(
                        new SphinxReponseModel
                        {
                            SPHINXReponseId = i,
                            LibelleReponse = reponse,
                            NumeroQuestionnaire = k

                        });
                        }
                    }
                }
        }

        private void NewQuestion(TextFieldParser csvParser)
        {

            string[] nomQuestions, libelleQuestions;
            nomQuestions = csvParser.ReadFields();
            libelleQuestions = csvParser.ReadFields();
            int i = 0;
            foreach (string nomQuestion in nomQuestions)
            {
                questions.Add(new SphinxQuestionModel
                {
                    SPHINXQuestionId = questions.Count,
                    TitreQuestion = nomQuestion,
                    LibelleQuestion = libelleQuestions[i++],
                    Reponses = new List<SphinxReponseModel>()
                });
            }
        }

        private SphinxFormulaireModel NewFormulaire(SphinxApiModel sphinxApiModel)
        {
            string libelleFormulaire = ExtractSurveyName(sphinxApiModel.SurveyName);
            string dateCreationFormulaire = ExtractDateCreationFormulaire(sphinxApiModel.SurveyName);
            int nbRecord = ExtractRecodsCount(sphinxApiModel.SurveyName);
            string dateDerniereReponse = ExtractDateDerniereReponse(sphinxApiModel.SurveyName);
            bool isOpen = ExtractIsOpen(sphinxApiModel.SurveyName);
            return (new SphinxFormulaireModel
            {
                SPHINXFormulaireId = formulaires.Count,
                TitreFormulaire = libelleFormulaire,
                NombreEnregistrement = nbRecord,
                DateCreationFormulaire = dateCreationFormulaire,
                DateDerniereReponse = dateDerniereReponse,
                IsOpen = isOpen,
                Questions = questions
            });
        }

        private string ExtractDateCreationFormulaire(string resultContent)
        {
            resultContent = resultContent.Split(',')[1];
            resultContent = resultContent.Split('"')[3];
            resultContent = resultContent.Replace("\"", string.Empty);
            return resultContent;
        }

        private int ExtractRecodsCount(string resultContent)
        {
            resultContent = resultContent.Split(',')[2];
            resultContent = resultContent.Split(':')[1];
            resultContent = resultContent.Replace("\\", string.Empty).Replace("\"", string.Empty);
            return Convert.ToInt32(resultContent);
        }


        private string ExtractDateDerniereReponse(string resultContent)
        {
            resultContent = resultContent.Split(',')[3];
            resultContent = resultContent.Split('"')[3];
            resultContent = resultContent.Replace("\"", string.Empty);
            return resultContent;
        }


        private bool ExtractIsOpen(string resultContent)
        {
            resultContent = resultContent.Split(',')[4];
            resultContent = resultContent.Split(':')[1];
            resultContent = resultContent.Replace("}", string.Empty);
            return Convert.ToBoolean(resultContent);
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
