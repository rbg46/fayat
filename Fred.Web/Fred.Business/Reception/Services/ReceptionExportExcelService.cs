using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Utilisateur;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Web.Models;
using Syncfusion.XlsIO;

namespace Fred.Business.Reception.Services
{
    public class ReceptionExportExcelService : IReceptionExportExcelService
    {
        private readonly IUtilisateurManager utilisateurManager;
        private readonly ISearchReceptionsService searchReceptionsService;

        public ReceptionExportExcelService(
            IUtilisateurManager utilisateurManager,
            ISearchReceptionsService searchReceptionsService)
        {
            this.utilisateurManager = utilisateurManager;
            this.searchReceptionsService = searchReceptionsService;
        }

        public IEnumerable<DepenseAchatEnt> TransformCommandesToDepensesForExport(IEnumerable<CommandeEnt> commandes)
        {
            foreach (var commande in commandes)
            {
                if (commande.Lignes != null)
                {
                    foreach (var ligne in commande.Lignes)
                    {
                        ligne.Commande = commande;
                        foreach (var reception in ligne.DepensesReception)
                        {
                            reception.CommandeLigne = ligne;
                        }
                    }
                }
            }

            return commandes.SelectMany(GetDepensesForCommande).ToList();
        }

        private IEnumerable<DepenseAchatEnt> GetDepensesForCommande(CommandeEnt commande)
        {
            return commande.Lignes != null ? commande.Lignes.SelectMany(l => l.DepensesReception).ToList() : new List<DepenseAchatEnt>();
        }

        public string CustomizeExcelFileForExport(string path, IEnumerable<ReceptionExportModel> results)
        {
            var excelFormat = new Framework.Reporting.ExcelFormat();
            Action<IWorkbook> action = (workbook) =>
            {
                var totalCount = results.Count() + 2; // +2 -> header and excel start index at 1
                workbook.Worksheets[0].InsertRow(totalCount, 1, ExcelInsertOptions.FormatAsBefore);
                workbook.Worksheets[0].Range["P" + totalCount].Text = BusinessResources.Reception_Manager_TotalHT;
                workbook.Worksheets[0].Range["Q" + totalCount].Formula = "=SUM(L2:L" + (totalCount - 1) + ")";
            };

            return excelFormat.GenerateExcelAndSaveOnServer(path, results, action);
        }

        /// <summary>
        /// Génération du fichiers excel de la liste de réceptions
        /// </summary>
        /// <param name="filtre">Filtre</param>
        /// <returns>Tableau de byte excel</returns>
        public byte[] GetReceptionsExcel(SearchDepenseEnt filtre)
        {
            //ici j'utilise le service qui recherche les reception que l'on voit a l'ecran
            TableauReceptionResult result = searchReceptionsService.SearchReceptionsWithTotals(filtre, null, null);

            return ReceptionExport.ToExcel(result.Receptions);
        }

        /// <summary>
        /// Récupération du nom de fichier de l'export de la liste des réceptions
        /// </summary>
        /// <returns>Nom du fichier excel</returns>
        public string GetReceptionsFilename()
        {
            return "ExportReceptions_" + utilisateurManager.GetContextUtilisateur()?.Nom;
        }
    }
}
