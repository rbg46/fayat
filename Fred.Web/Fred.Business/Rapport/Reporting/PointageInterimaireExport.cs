using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Framework.Exceptions;
using Fred.Framework.Models.Reporting;
using Fred.Framework.Reporting;
using Fred.Web.Models.PointagePersonnel;
using Syncfusion.Pdf;
using Syncfusion.XlsIO;

namespace Fred.Business.Rapport.Reporting
{
    /// <summary>
    ///   Classe de gestion de l'export des pointages d'un intérimire pour une période donnée
    /// </summary>
    public static class PointageInterimaireExport
    {
        private static readonly string ExcelTemplate = "Templates/PointagePersonnel/TemplatePointageInterimaire.xlsx";
        private static readonly string Titre = BusinessResources.Pointage_Interimaire_Export_Titre;
        private static readonly string DateEdition = BusinessResources.Pointage_Interimaire_Export_DateEdition;
        private static readonly string EditePar = BusinessResources.Pointage_Interimaire_Export_EditePar;
        private static readonly string Periode = BusinessResources.Pointage_Interimaire_Export_Periode;
        private static readonly string Au = BusinessResources.Pointage_Interimaire_Export__Au;
        private static readonly string Espace = BusinessResources.Pointage_Interimaire_Export__Espace;

        /// <summary>
        ///   Construction du fichier excel
        /// </summary>
        /// <param name="pointagePersonnelExportModel">Objet pointage personnel export</param>
        /// <param name="pointages">Liste des pointage personnel</param>  
        /// <param name="pathImage">Chemin du logo de la société</param>
        /// <returns>Tableau de bytes de l'excel</returns>
        public static byte[] ToExcel(PointagePersonnelExportModel pointagePersonnelExportModel, IEnumerable<IGrouping<string, RapportLigneEnt>> pointages, string pathImage)
        {
            ExcelFormat excelFormat = new ExcelFormat();
            MemoryStream stream = new MemoryStream();

            IWorkbook wb = BuildWorkbook(excelFormat, pointagePersonnelExportModel, pointages, pathImage);

            wb.SaveAs(stream);

            wb.Close();

            return stream.ToArray();
        }

        /// <summary>
        ///   Construction du fichier PDF
        /// </summary> 
        /// <param name="pointagePersonnelExportModel">Objet pointage personnel export</param>
        /// <param name="pointages">Liste des pointages personnel</param>
        /// <param name="pathImage">Chemin du logo de la société</param>
        /// <returns>Tableau de bytes du PDF</returns>
        public static byte[] ToPdf(PointagePersonnelExportModel pointagePersonnelExportModel, IEnumerable<IGrouping<string, RapportLigneEnt>> pointages, string pathImage)
        {
            ExcelFormat excelFormat = new ExcelFormat();
            MemoryStream stream = new MemoryStream();
            IWorkbook wb = BuildWorkbook(excelFormat, pointagePersonnelExportModel, pointages, pathImage);

            //// Convert workbook to PDF
            PdfDocument pdf = excelFormat.PrintExcelToPdf(wb);

            //// Save the document stream
            pdf.Save(stream);

            wb.Close();
            return stream.ToArray();
        }

        #region Private functions : Gestion du document

        /// <summary>
        ///   Rempli l'entête du document avec les informations du personnel
        /// </summary>
        /// <param name="excelFormat">Classe de mise en forme pour excel</param>
        /// <param name="ws">Feuille de travail</param>
        /// <param name="pointagePersonnelExportModel">Objet pointage personnel export</param>
        /// <param name="pathImage">Chemin du logo de la société</param>
        private static void FillHeader(ExcelFormat excelFormat, IWorksheet ws, PointagePersonnelExportModel pointagePersonnelExportModel, string pathImage)
        {
            string dateEdition = DateEdition + DateTime.Now.ToShortDateString() + Espace + DateTime.Now.ToShortTimeString();
            string editePar = EditePar + pointagePersonnelExportModel.Utilisateur.MatriculeNomPrenom;
            string periode = Periode + pointagePersonnelExportModel.DateDebut.Value.ToShortDateString() + Au + pointagePersonnelExportModel.DateFin.Value.ToShortDateString();
            BuildHeaderExcelModel buildHeaderModel = new BuildHeaderExcelModel(Titre, pointagePersonnelExportModel.Organisation.CodeLibelle, dateEdition, editePar, periode, pathImage, new IndexHeaderExcelModel(3, 13, 19, 21));
            excelFormat.BuildHeader(ws, buildHeaderModel);
        }

        /// <summary>
        ///   Rempli le tableau contenant les totaux des heures
        /// </summary>
        /// <param name="ws">Feuille de travail</param>
        /// <param name="rowIndex1">Index de la dernière ligne</param>
        private static void FillTableTotal(IWorksheet ws, int rowIndex1)
        {
            ws.Range[6, 2].Formula = "=I" + rowIndex1;
            ws.Range[7, 2].Formula = "=J" + rowIndex1;
            ws.Range[8, 2].Formula = "=K" + rowIndex1;
            ws.Range[6, 3].Formula = "=L" + rowIndex1;
            ws.Range[7, 3].Formula = "=M" + rowIndex1;
            ws.Range[8, 3].Formula = "=N" + rowIndex1;
            ws.Range[6, 4].Formula = "=O" + rowIndex1;
            ws.Range[7, 4].Formula = "=P" + rowIndex1;
            ws.Range[8, 4].Formula = "=Q" + rowIndex1;
            ws.Range[6, 5].Formula = "=R" + rowIndex1;
            ws.Range[7, 5].Formula = "=S" + rowIndex1;
            ws.Range[8, 5].Formula = "=T" + rowIndex1;

            ws.Range[6, 6].Formula = "=SUM(B6:E6)";
            ws.Range[7, 6].Formula = "=SUM(B7:E7)";
            ws.Range[8, 6].Formula = "=SUM(B8:E8)";
        }

        /// <summary>
        ///   Rempli la ligne contenant les totaux des heures
        /// </summary>
        /// <param name="ws">Feuille de travail</param>
        /// <param name="rowIndex1">Index de la dernière ligne</param>
        private static void FillTotal(IWorksheet ws, int rowIndex1)
        {
            int lastRow = rowIndex1 - 1;

            ws.Range[rowIndex1, 9].Formula = "=SUM(I12:I" + lastRow.ToString() + ")";
            ws.Range[rowIndex1, 10].Formula = "=SUM(J12:J" + lastRow.ToString() + ")";
            ws.Range[rowIndex1, 11].Formula = "=SUM(K12:K" + lastRow.ToString() + ")";
            ws.Range[rowIndex1, 12].Formula = "=SUM(L12:L" + lastRow.ToString() + ")";
            ws.Range[rowIndex1, 13].Formula = "=SUM(M12:M" + lastRow.ToString() + ")";
            ws.Range[rowIndex1, 14].Formula = "=SUM(N12:N" + lastRow.ToString() + ")";
            ws.Range[rowIndex1, 15].Formula = "=SUM(O12:O" + lastRow.ToString() + ")";
            ws.Range[rowIndex1, 16].Formula = "=SUM(P12:P" + lastRow.ToString() + ")";
            ws.Range[rowIndex1, 17].Formula = "=SUM(Q12:Q" + lastRow.ToString() + ")";
            ws.Range[rowIndex1, 18].Formula = "=SUM(R12:R" + lastRow.ToString() + ")";
            ws.Range[rowIndex1, 19].Formula = "=SUM(S12:S" + lastRow.ToString() + ")";
            ws.Range[rowIndex1, 20].Formula = "=SUM(T12:T" + lastRow.ToString() + ")";
        }

        /// <summary>
        ///   Rempli la ligne contenant les moyennes
        /// </summary>
        /// <param name="ws">Feuille de travail</param>
        /// <param name="rowIndex1">Index de la dernière ligne</param>
        private static void FillMoyenne(IWorksheet ws, int rowIndex1)
        {
            int totalRow = rowIndex1 - 1;
            int lastRow = rowIndex1 - 2;

            ws.Range[rowIndex1, 9].Formula = "=IF(I" + totalRow.ToString() + "=0; 0; ROUND(AVERAGE(I12:I" + lastRow.ToString() + ");2))";
            ws.Range[rowIndex1, 10].Formula = "=IF(J" + totalRow.ToString() + "=0; 0; ROUND(AVERAGE(J12:J" + lastRow.ToString() + ");2))";
            ws.Range[rowIndex1, 11].Formula = "=IF(K" + totalRow.ToString() + "=0; 0; ROUND(AVERAGE(K12:K" + lastRow.ToString() + ");2))";
            ws.Range[rowIndex1, 12].Formula = "=IF(L" + totalRow.ToString() + "=0; 0; ROUND(AVERAGE(L12:L" + lastRow.ToString() + ");2))";
            ws.Range[rowIndex1, 13].Formula = "=IF(M" + totalRow.ToString() + "=0; 0; ROUND(AVERAGE(M12:M" + lastRow.ToString() + ");2))";
            ws.Range[rowIndex1, 14].Formula = "=IF(N" + totalRow.ToString() + "=0; 0; ROUND(AVERAGE(N12:N" + lastRow.ToString() + ");2))";
            ws.Range[rowIndex1, 15].Formula = "=IF(O" + totalRow.ToString() + "=0; 0; ROUND(AVERAGE(O12:O" + lastRow.ToString() + ");2))";
            ws.Range[rowIndex1, 16].Formula = "=IF(P" + totalRow.ToString() + "=0; 0; ROUND(AVERAGE(P12:P" + lastRow.ToString() + ");2))";
            ws.Range[rowIndex1, 17].Formula = "=IF(Q" + totalRow.ToString() + "=0; 0; ROUND(AVERAGE(Q12:Q" + lastRow.ToString() + ");2))";
            ws.Range[rowIndex1, 18].Formula = "=IF(R" + totalRow.ToString() + "=0; 0; ROUND(AVERAGE(R12:R" + lastRow.ToString() + ");2))";
            ws.Range[rowIndex1, 19].Formula = "=IF(S" + totalRow.ToString() + "=0; 0; ROUND(AVERAGE(S12:S" + lastRow.ToString() + ");2))";
            ws.Range[rowIndex1, 20].Formula = "=IF(T" + totalRow.ToString() + "=0; 0; ROUND(AVERAGE(T12:T" + lastRow.ToString() + ");2))";
        }

        /// <summary>
        ///   Retourne le numéro de colonne en fonction du statut du personnel
        /// </summary>
        /// <param name="moreColumn">Ajout du nombre de colonne</param>
        /// <param name="statut">type statut</param>
        /// <returns>Le numéro de colonne</returns>
        private static int ChoiceColumnTotalHour(int moreColumn, int statut)
        {
            switch (statut)
            {
                case 1:
                    return 9 + moreColumn;
                case 2:
                case 4:
                case 5:
                    return 10 + moreColumn;
                case 3:
                    return 11 + moreColumn;
            }
            return 0;
        }

        /// <summary>
        ///   Création du document excel
        /// </summary>
        /// <param name="excelFormat">Classe de mise en forme pour excel</param>
        /// <param name="pointagePersonnelExportModel">Objet pointage personnel export</param>
        /// <param name="pointages">Liste des pointages</param>
        /// <param name="pathImage">Chemin du logo de la société</param>
        /// <returns>Workbook excel</returns>
        private static IWorkbook BuildWorkbook(ExcelFormat excelFormat, PointagePersonnelExportModel pointagePersonnelExportModel, IEnumerable<IGrouping<string, RapportLigneEnt>> pointages, string pathImage)
        {
            IWorkbook wb = null;

            try
            {
                wb = excelFormat.OpenTemplateWorksheet(AppDomain.CurrentDomain.BaseDirectory + ExcelTemplate);

                IWorksheet ws = wb.Worksheets[0];

                FillHeader(excelFormat, ws, pointagePersonnelExportModel, pathImage);

                int rowIndex1 = 12;

                foreach (IGrouping<string, RapportLigneEnt> pointage in pointages)
                {
                    if (rowIndex1 != 12)
                    {
                        excelFormat.InsertRowFormatAsBefore(wb, rowIndex1);
                    }

                    double totalHeure = 0.00;
                    RapportLigneEnt ptg = pointage.FirstOrDefault();
                    PersonnelEnt personnel = ptg.Personnel;

                    ws.Range[rowIndex1, 1].Text = personnel.Nom;
                    ws.Range[rowIndex1, 2].Text = personnel.Prenom;
                    ws.Range[rowIndex1, 3].Text = personnel.ContratActif.Qualification;
                    ws.Range[rowIndex1, 4].Text = personnel.ContratActif.DateDebut.ToString("dd/MM/yy", CultureInfo.InvariantCulture);
                    ws.Range[rowIndex1, 5].Text = personnel.ContratActif.DateFin.ToString("dd/MM/yy", CultureInfo.InvariantCulture);
                    ws.Range[rowIndex1, 6].Text = ptg.Ci.Code;
                    ws.Range[rowIndex1, 7].Text = ptg.Ci.Libelle;
                    ws.Range[rowIndex1, 8].Text = personnel.ContratActif.Fournisseur.Libelle;

                    foreach (RapportLigneEnt p in pointage)
                    {
                        totalHeure = totalHeure + p.HeureTotalTravail;
                    }

                    var statutContrat = personnel.ContratActif.Statut.HasValue ? personnel.ContratActif.Statut.Value : 0;
                    switch (personnel.ContratActif.MotifRemplacement.Code)
                    {
                        case "AA":
                            ws.Range[rowIndex1, ChoiceColumnTotalHour(0, statutContrat)].Number = totalHeure;
                            ws.Range[rowIndex1, 21].Text = Espace;
                            break;
                        case "TU":
                            ws.Range[rowIndex1, ChoiceColumnTotalHour(3, statutContrat)].Number = totalHeure;
                            ws.Range[rowIndex1, 21].Text = Espace;
                            break;
                        case "QR":
                            ws.Range[rowIndex1, ChoiceColumnTotalHour(6, statutContrat)].Number = totalHeure;
                            ws.Range[rowIndex1, 21].Text = Espace;
                            break;
                        case "RS":
                            ws.Range[rowIndex1, ChoiceColumnTotalHour(9, statutContrat)].Number = totalHeure;
                            ws.Range[rowIndex1, 21].Text = personnel.ContratActif.PersonnelRemplace.Nom;
                            break;
                    }

                    rowIndex1++;
                }

                FillTotal(ws, rowIndex1);
                FillMoyenne(ws, rowIndex1 + 1);
                FillTableTotal(ws, rowIndex1);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }

            return wb;
        }
        #endregion
    }
}
