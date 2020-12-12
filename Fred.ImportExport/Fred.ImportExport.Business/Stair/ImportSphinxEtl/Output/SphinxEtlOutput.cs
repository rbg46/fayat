using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.ImportExport.Entities.Stair;
using Fred.ImportExport.Framework.Etl.Output;
using Fred.ImportExport.Framework.Etl.Result;
using Fred.ImportExport.Models.Stair.Sphinx;

namespace Fred.ImportExport.Business.Stair.ImportSphinxEtl.Output
{
    public class SphinxEtlOutput : IEtlOutput<SphinxFormulaireModel>
    {
        private readonly SphinxFormulaireManager formulaireManager = new SphinxFormulaireManager();

        public async Task ExecuteAsync(IEtlResult<SphinxFormulaireModel> result)
        {
            await Task.Run(() =>
            {
                var questions = new List<StairSphinxQuestionEnt>();
                var reponses = new List<StairSphinxReponseEnt>();
                var formulaires = new List<StairSphinxFormulaireEnt>();

                foreach (SphinxFormulaireModel formulaire in result.Items)
                {
                    StairSphinxFormulaireEnt formulaireInDB = formulaireManager.Get(formulaire.TitreFormulaire, formulaire.DateCreationFormulaire);
                    if (formulaireInDB != null && formulaireInDB.IsOpen)
                    {
                        // Suppression du formulaire de ses questions et de ses réponses
                        formulaireManager.Remove(formulaireInDB);
                    }

                    InitFormulaire(questions, reponses, formulaires, formulaire);
                }

                formulaireManager.Add(formulaires);
            });
        }

        private static void InitFormulaire(List<StairSphinxQuestionEnt> questions, List<StairSphinxReponseEnt> reponses, List<StairSphinxFormulaireEnt> formulaires, SphinxFormulaireModel formulaire)
        {
            foreach (SphinxQuestionModel question in formulaire.Questions)
            {

                foreach (SphinxReponseModel reponse in question.Reponses)
                {

                    reponses.Add(new StairSphinxReponseEnt
                    {
                        LibelleReponse = reponse.LibelleReponse,
                        NumeroQuestionnaire = reponse.NumeroQuestionnaire
                    });

                }
                questions.Add(new StairSphinxQuestionEnt
                {
                    TitreQuestion = question.TitreQuestion,
                    LibelleQuestion = question.LibelleQuestion,
                    Reponses = new List<StairSphinxReponseEnt>(reponses)
                });
                reponses.Clear();
            }

            formulaires.Add(new StairSphinxFormulaireEnt
            {
                TitreFormulaire = formulaire.TitreFormulaire,
                NombreEnregistrement = formulaire.NombreEnregistrement,
                DateCreationFormulaire = formulaire.DateCreationFormulaire,
                DateDerniereReponse = formulaire.DateDerniereReponse,
                IsOpen = formulaire.IsOpen,
                Questions = new List<StairSphinxQuestionEnt>(questions)
            });
            questions.Clear();
        }
    }
}
