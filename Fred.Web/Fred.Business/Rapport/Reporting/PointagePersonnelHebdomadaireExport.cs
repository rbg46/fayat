using Fred.Business.Common;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Framework.Exceptions;
using Fred.Framework.Models.Reporting;
using Fred.Framework.Reporting;
using Fred.Web.Models.PointagePersonnel;
using Fred.Web.Shared.App_LocalResources;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Fred.Business.Rapport.Reporting
{
    /// <summary>
    ///   Classe de gestion de l'export des pointages d'un intérimire pour une période donnée
    /// </summary>
    public static class PointagePersonnelHebdomadaireExport
    {
        private static readonly string Titre = FeatureRapport.PointagePersonnelHebdomadaireExport_Titre;
        private static readonly string ExcelTemplate = "Templates/PointagePersonnel/TemplatePointagePersonnelHebdomadaire.xlsx";
        private static readonly string A = BusinessResources.Pointage_Personnel_Hebdomadaire_Export_A;
        private static readonly string T = BusinessResources.Pointage_Personnel_Hebdomadaire_Export_T;

        /// <summary>
        ///   Construction du fichier excel
        /// </summary>
        /// <param name="pointagePersonnelExportModel">Objet pointage personnel export</param>
        /// <param name="pointages">Liste des pointage personnel</param>
        /// <param name="pathImage">Chemein du logo de la société</param>
        /// <returns>Tableau de bytes de l'excel</returns>
        public static byte[] ToExcel(PointagePersonnelExportModel pointagePersonnelExportModel, List<IEnumerable<IGrouping<int?, RapportLigneEnt>>> pointages, string pathImage)
        {
            var stream = new MemoryStream();

            IWorkbook wb = BuildWorkbook(pointagePersonnelExportModel, pointages, pathImage);

            wb.SaveAs(stream);

            wb.Close();

            return stream.ToArray();
        }

        /// <summary>
        ///   Rempli l'entête du document avec les informations du personnel
        /// </summary>
        /// <param name="excelFormat">Classe de mise en forme pour excel</param>
        /// <param name="ws">Feuille de travail</param>
        /// <param name="pointagePersonnelExportModel">Objet pointage personnel export</param>
        /// <param name="pathImage">Chemein du logo de la société</param>
        private static void FillHeader(ExcelFormat excelFormat, IWorksheet ws, PointagePersonnelExportModel pointagePersonnelExportModel, string pathImage)
        {
            var editePar = BusinessResources.Pointage_Interimaire_Export_EditePar + pointagePersonnelExportModel.Utilisateur.MatriculeNomPrenom;
            var dateEdition = BusinessResources.Pointage_Interimaire_Export_DateEdition + DateTime.Now.ToShortDateString() + A + DateTime.Now.ToShortTimeString();
            var buildHeaderModel = new BuildHeaderExcelModel(Titre, pointagePersonnelExportModel.Organisation.CodeLibelle, dateEdition, editePar, null, pathImage, new IndexHeaderExcelModel(4, 9, 11, 11));
            excelFormat.BuildHeader(ws, buildHeaderModel);
        }

#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
        /// <summary>
        ///   Création du document excel
        /// </summary>
        /// <param name="pointagePersonnelExportModel">Objet pointage personnel export</param>
        /// <param name="pointagesGrpList">Liste des pointages</param>
        /// <param name="pathImage">Chemein du logo de la société</param>
        /// <returns>Workbook excel</returns>
        private static IWorkbook BuildWorkbook(PointagePersonnelExportModel pointagePersonnelExportModel, List<IEnumerable<IGrouping<int?, RapportLigneEnt>>> pointagesGrpList, string pathImage)
#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high
        {
            IWorkbook wb;

            try
            {
                var excelFormat = new ExcelFormat();
                wb = excelFormat.OpenTemplateWorksheet(AppDomain.CurrentDomain.BaseDirectory + ExcelTemplate);

                IWorksheet ws = wb.Worksheets[0];

                FillHeader(excelFormat, ws, pointagePersonnelExportModel, pathImage);

                int rowIndex1 = 4;

                foreach (var pointagesGrp in pointagesGrpList)
                {
                    foreach (var pointageGrp in pointagesGrp)
                    {
                        double totalHeure = 0.00;
                        if (rowIndex1 != 10)
                        {
                            excelFormat.InsertRowFormatAsBefore(wb, rowIndex1);
                        }


                        PersonnelEnt personnel = pointageGrp.FirstOrDefault().Personnel;

                        ws.Range[rowIndex1, 1].Text = personnel.EtablissementRattachement != null ? personnel.EtablissementRattachement.Code : string.Empty;
                        ws.Range[rowIndex1, 2].Text = personnel.Societe.Code;
                        ws.Range[rowIndex1, 3].Text = personnel.Matricule;
                        ws.Range[rowIndex1, 4].Text = personnel.Nom;
                        ws.Range[rowIndex1, 5].Text = personnel.Prenom;
                        ws.Range[rowIndex1, 6].Text = PersonnelUtils.GetStatutCodeFromStatutId(personnel.Statut);
                        ws.Range[rowIndex1, 7].Text = CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(pointageGrp.FirstOrDefault().DatePointage, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday).ToString();
                        ws.Range[rowIndex1, 8].Text = pointageGrp.Key != 0 ? pointageGrp.FirstOrDefault().CodeAbsence.Code : T;

                        // Cas du cumul des heures d'absence
                        if (pointageGrp.Key != 0)
                        {
                            foreach (var pointage in pointageGrp)
                            {
                                totalHeure += pointage.HeureAbsence;
                            }

                        }
                        // Cas du cumul des heures travaillées
                        else
                        {
                            foreach (var pointage in pointageGrp)
                            {
                                totalHeure += pointage.HeureTotalTravail;
                            }
                        }



                        ws.Range[rowIndex1, 9].Number = totalHeure;

                        rowIndex1++;
                    }
                }

            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }

            return wb;
        }
    }
}
