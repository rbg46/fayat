using System.Collections.Generic;
using System.Linq;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Anael;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Excel.Input;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Converter;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Models;
using Fred.ImportExport.Models;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Context
{
    /// <summary>
    /// Service qui fournit les données necessaires a l'import des cis a partir d'un fichier excel
    /// </summary>
    public class ImportFournisseurByExcelAnaelSystemContextProvider : IImportFournisseurByExcelAnaelSystemContextProvider
    {
        private readonly List<string> defaultTypeSequences = new List<string> { "TIERS", "GROUPE", "TIERS2" };

        private readonly ISocieteManager societeManager;
        private readonly IPaysManager paysManager;

        public ImportFournisseurByExcelAnaelSystemContextProvider(ISocieteManager societeManager, IPaysManager paysManager)
        {
            this.societeManager = societeManager;
            this.paysManager = paysManager;
        }

        /// <summary>
        /// Fournit les données necessaires a l'import des cis
        /// </summary>
        /// <param name="input">Données d'entrées</param>
        /// <param name="logger">Logger</param>       
        /// <returns>les données necessaires a l'import des cis</returns>
        public ImportFournisseurContext<ImportFournisseursByExcelInputs> GetContext(ImportFournisseursByExcelInputs input, FournisseurImportExportLogger logger)
        {
            var result = new ImportFournisseurContext<ImportFournisseursByExcelInputs>
            {
                Input = input,
                TypeSequences = defaultTypeSequences
            };

            if (input != null && input.CodeSocietes != null && input.CodeSocietes.Any()
                && input.RepriseImportFournisseurs != null && input.RepriseImportFournisseurs.Any())
            {
                foreach (var societeCode in input.CodeSocietes)
                {
                    var societeContext = new ImportFournisseurSocieteContext();

                    societeContext.Societe = societeManager.GetSocieteByCodeSocieteComptable(societeCode);

                    if (societeContext.Societe != null)
                    {
                        var anaelFournisseurs = GetAnaelFournisseurs(
                            societeContext.Societe.CodeSocieteComptable,
                            GetTypeSequenceUsedInExcel(input.RepriseImportFournisseurs),
                            input.RegleGestion,
                            GetCodeFournisseurUsedInExcel(input.RepriseImportFournisseurs),
                            input.ModeleSociete);

                        //log fournisseurs anael
                        logger.LogAnaelModels(anaelFournisseurs);

                        //Convert to FournisseurFredModel to add to context
                        var converterToFredModel = new AnaelModelToFournisseurFredModelConverter(paysManager);
                        societeContext.AnaelFournisseurs = converterToFredModel.ConvertAnaelModelToFournisseurFredModel(anaelFournisseurs);
                    }
                    result.SocietesContexts.Add(societeContext);
                }
            }



            return result;
        }

        private List<string> GetCodeFournisseurUsedInExcel(List<RepriseImportFournisseur> repriseImportFournisseurs)
        {
            List<string> codeFournisseurs = new List<string>(repriseImportFournisseurs.Select(x => x.CodeFournisseur).Distinct());

            return codeFournisseurs;
        }

        private List<string> GetTypeSequenceUsedInExcel(List<RepriseImportFournisseur> repriseImportFournisseurs)
        {
            List<string> typeSequences = new List<string>(repriseImportFournisseurs.Select(x => x.TypeSequence).Distinct());

            return typeSequences;
        }

        private List<FournisseurAnaelModel> GetAnaelFournisseurs(string codeSocieteComptable, List<string> typeSequences, string regleGestion, List<string> codesFournisseurs, string modeleSociete)
        {
            var anaelFournisseurProvider = new AnaelFournisseurProvider();

            // je recupere les cis par 'liste de cis', donc je ne filtre les cis recus d'anael, 
            // en effet le ci a pu etre importé par fichier excel il faut donc qu'il soit possible de le mettre a jour
            return anaelFournisseurProvider.GetFournisseurFromAnael(codeSocieteComptable, typeSequences, regleGestion, codesFournisseurs, modeleSociete);
        }
    }
}
