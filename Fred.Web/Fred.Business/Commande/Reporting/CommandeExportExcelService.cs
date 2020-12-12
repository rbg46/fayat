using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fred.Business.Commande
{
    /// <summary>
    /// Service pour l'export excel des commandes
    /// </summary>
    public class CommandeExportExcelService : ICommandeExportExcelService
    {
        private readonly string totalHt = BusinessResources.Commande_Export_Excel_Service_TotalHT;

        /// <summary>
        ///   Génère le fichier excel contenant la liste des commandes
        /// </summary>
        /// <typeparam name="T">Commande</typeparam>  
        /// <param name="modelList">Liste de commandes</param>
        /// <returns>action de customisation d'un workbook</returns>
        public Action<IWorkbook> CustomizeExcelFileForExport<T>(IEnumerable<T> modelList)
        {
            Action<IWorkbook> customAction = (workbook) =>
            {
                int count = modelList.ToList().Count;
                int sumColumnMax = count + 2;
                int sumRowIndex = count + 3;

                // Somme des montants commandés, montants réceptionnés, soldes commande
                // /!\ Si ajout de colonnes dans le template xlsx, il faut modifier les colonnes pour les sommes
                string sumMontantCommande = "SUM(M3:M" + sumColumnMax + ")";
                string sumMontantReceptionne = "SUM(N3:N" + sumColumnMax + ")";
                string sumSoldeCommande = "SUM(O3:O" + sumColumnMax + ")";
                string sumMontantFacture = "SUM(P3:P" + sumColumnMax + ")";
                string sumSoldeFar = "SUM(Q3:Q" + sumColumnMax + ")";
                string index = "L" + sumRowIndex;

                workbook.ActiveSheet.Range[index].Text = totalHt;

                workbook.ActiveSheet.SetFormula(sumRowIndex, 13, sumMontantCommande);
                workbook.ActiveSheet.SetFormula(sumRowIndex, 14, sumMontantReceptionne);
                workbook.ActiveSheet.SetFormula(sumRowIndex, 15, sumSoldeCommande);
                workbook.ActiveSheet.SetFormula(sumRowIndex, 16, sumMontantFacture);
                workbook.ActiveSheet.SetFormula(sumRowIndex, 17, sumSoldeFar);

                // Application de style 
                IStyle boldStyle = workbook.Styles.Add("BoldFont");
                boldStyle.Font.Bold = true;

                index = "L" + sumRowIndex + ":Q" + sumRowIndex;
                workbook.ActiveSheet.Range[index].CellStyle = boldStyle;
            };
            return customAction;
        }
    }
}
