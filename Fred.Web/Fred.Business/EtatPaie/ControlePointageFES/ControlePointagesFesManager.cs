using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using Fred.Business.EtatPaie.ControlePointage;
using Fred.Business.Images;
using Fred.Business.Organisation;
using Fred.Business.Params;
using Fred.Business.Personnel;
using Fred.Entities;
using Fred.Entities.Organisation;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Rapport;
using Fred.Framework.Exceptions;
using Fred.Framework.Models.Reporting;
using Fred.Framework.Reporting;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.EtatPaie;
using MoreLinq;
using Syncfusion.XlsIO;

namespace Fred.Business.EtatPaie
{
    /// <summary>
    /// Manager de l'édition Controle des pontages
    /// </summary>
    public class ControlePointagesFesManager : IControlePointagesFesManager
    {
        private readonly IOrganisationManager organisationManager;
        private readonly IPersonnelManager personnelManager;
        private readonly IImageManager imageManager;
        private readonly IParamsManager paramsManager;

        private const int IndexColOrigin = 1;
        private const int IndexColTotal = 33;
        private int compteur = 0;
        private int indexRow;
        private int indexColEnd;

        /// <summary>
        /// Permet de gérer les états paie 
        /// </summary>
        /// <param name="imageManager">Manager d'image</param>
        /// <param name="organisationManager">Manager d'organisation</param>
        /// <param name="personnelManager">Manager de personnel</param>
        /// <param name="paramsManager">Manager des paramètres</param>
        public ControlePointagesFesManager(IOrganisationManager organisationManager, IPersonnelManager personnelManager, IImageManager imageManager, IParamsManager paramsManager)
        {
            this.organisationManager = organisationManager;
            this.personnelManager = personnelManager;
            this.imageManager = imageManager;
            this.paramsManager = paramsManager;
        }

        /// <summary>
        /// Génère un pdf pour le controle des pointages
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'export </param>
        /// <param name="listePointageMensuel">Liste des models de pointage mensuel</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <returns>Le fichier généré</returns>
        public MemoryStream GenerateMemoryStreamControlePointagesFES(EtatPaieExportModel etatPaieExportModel, List<PointageMensuelPersonEnt> listePointageMensuel, int userId, string templateFolderPath)
        {
            ExcelFormat excelFormat = new ExcelFormat();
            // controle du nombre de personnel exportés dans le cas d'un export pdf
            if (etatPaieExportModel.Pdf && listePointageMensuel.Count > 500)
            {
                throw new FredBusinessMessageResponseException(FeatureRapport.Pointage_Export_Controle_Pointage_Pdf_Personnel_Limit_Error);
            }

            IWorkbook workbookMaster = excelFormat.GetNewWorbook();
            OrganisationEnt organisation = organisationManager.GetOrganisationById(etatPaieExportModel.OrganisationId);
            foreach (PointageMensuelPersonEnt pointage in listePointageMensuel)
            {
                IWorkbook workbook = GenerateExcelParPerson(excelFormat, pointage, etatPaieExportModel, organisation, userId, templateFolderPath);

                if (workbook != null)
                {
                    workbookMaster.Worksheets.AddCopy(workbook.Worksheets[0]);
                }
            }

            if (workbookMaster.Worksheets.Count > 1)
            {
                workbookMaster.Worksheets.Remove(0);
            }

            MemoryStream stream = excelFormat.GeneratePdfOrExcel(etatPaieExportModel.Pdf, workbookMaster);
            excelFormat.Dispose();
            return stream;
        }

        /// <summary>
        /// Génère un classeur excel pour le pointage mensuel d'un personnel
        /// </summary>
        /// <param name="excelFormat">ExcelFormat</param>
        /// <param name="pointage">Un pointage mensuel pour un personnel</param>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="organisation">Organisation entité</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <returns>Le classeur généré</returns>
        private IWorkbook GenerateExcelParPerson(ExcelFormat excelFormat, PointageMensuelPersonEnt pointage, EtatPaieExportModel etatPaieExportModel, OrganisationEnt organisation, int userId, string templateFolderPath)
        {
            string pathName = Path.Combine(templateFolderPath, "TemplateControlePointageMensuelParPersonne.xlsx");
            IWorkbook workbook = excelFormat.OpenTemplateWorksheet(pathName);
            workbook.Worksheets[0].Name = pointage.Personnel.Matricule;
            string affaireLib = "TOUTES";
            compteur = 0;

            if (organisation != null)
            {
                affaireLib = string.Concat(organisation.Code, " - ", organisation.Libelle);
            }
            var entete = new
            {
                Affaire = affaireLib,
                PeriodeComptable = new DateTime(etatPaieExportModel.Annee, etatPaieExportModel.Mois, 1).ToString("MM/yyyy")
            };

            var perso = personnelManager.GetPersonnel(pointage.Personnel.PersonnelId);
            ContratInterimaireEnt affectationInterim = personnelManager.GetAffectationInterimaireActive(pointage.Personnel.PersonnelId, new DateTime(etatPaieExportModel.Annee, etatPaieExportModel.Mois, 1));
            string codeInterim = affectationInterim != null ? affectationInterim.Fournisseur.Code + " - " + affectationInterim.Fournisseur.Libelle : string.Empty;
            string personnelStatut = GetPersonnelStatut(perso.Statut);
            var personnel = new
            {
                perso.CodeNomPrenom,
                Interim = codeInterim,
                Entreprise = perso.Societe != null ? perso.Societe.Libelle : string.Empty,
                Fonction = perso.Ressource != null ? perso.Ressource.Libelle : string.Empty,
                Statut = personnelStatut,
                ResponsableHirar = perso.Manager != null ? perso.Manager.NomPrenom : string.Empty
            };
            excelFormat.InitVariables(workbook);
            excelFormat.AddVariable("Entete", entete);
            excelFormat.AddVariable("Personnel", personnel);
            excelFormat.AddVariable("HeuresNonMajo", pointage.ListHeuresNormales);
            excelFormat.AddVariable("HeuresMajo", pointage.ListHeuresMajo);
            excelFormat.AddVariable("HeuresAbsence", pointage.ListHeuresAbsence);
            excelFormat.AddVariable("HeuresPointees", pointage.ListHeuresPointees);
            excelFormat.AddVariable("HeuresTravaillees", pointage.ListHeuresTravaillees);
            excelFormat.ApplyVariables();
            var dayInMonth = DateTime.DaysInMonth(etatPaieExportModel.Annee, etatPaieExportModel.Mois);
            indexRow = 8;
            indexColEnd = dayInMonth + 1;

            HandleAstreintes(excelFormat, workbook, pointage.ListAstreintes);
            HandleAbsences(excelFormat, workbook, pointage.ListAbsences);

            indexRow += 2;
            List<PointageMensuelPersonEnt.Primes> igd = pointage.ListPrimes.Where(i => i != null && i.Libelle.Contains("IGD")).ToList();
            List<PointageMensuelPersonEnt.Primes> ipd = pointage.ListPrimes.Where(i => i != null && i.Libelle.Contains("IPD")).ToList();
            if (personnel.Statut != Constantes.PersonnelStatutValue.Cadre)
            {
                string heuresSemaine = 35d.ToString();
                if (pointage.Personnel.Societe != null && pointage.Personnel.Societe.Organisation != null)
                {
                    heuresSemaine = paramsManager.GetParamValue(pointage.Personnel.Societe.Organisation.OrganisationId, "HeuresSemaineOuvrierETAM") ?? 35d.ToString();
                }
                HandleHeuresSupplementaire(excelFormat, workbook, etatPaieExportModel, pointage, heuresSemaine);
            }
            //Gestion des heures supplémentaire            

            HandleIGD(excelFormat, workbook, igd);
            HandleIPD(excelFormat, workbook, ipd);
            HandlePrimes(excelFormat, workbook, pointage.ListPrimes.Where(i => !i.Libelle.Contains("IGD") && !i.Libelle.Contains("IPD")).ToList(), pointage.Totalprime);
            HandleZonesDeplacement(excelFormat, workbook, pointage.ListCodeZoneDeplacements);

            var firstDayRow = 4;
            var firstDayCol = 1;
            CultureInfo ci = CultureInfo.CreateSpecificCulture("fr-FR");
            DateTimeFormatInfo dtfi = ci.DateTimeFormat;
            for (int i = 1; i <= dayInMonth; i++)
            {
                DateTime iDay = new DateTime(etatPaieExportModel.Annee, etatPaieExportModel.Mois, i);
                var dayName = i + "\n" + dtfi.AbbreviatedDayNames[(int)iDay.DayOfWeek].Replace(".", string.Empty);
                excelFormat.SetCellValue(workbook, firstDayRow, firstDayCol + i, dayName);
            }
            IWorksheet ws = workbook.Worksheets[0];
            var lastDayInMonthIndex = firstDayCol + dayInMonth;
            ws.DeleteColumn(lastDayInMonthIndex + 1, 31 - dayInMonth);

            int maxRowIndex = workbook.Worksheets[0].Rows.Index().Max(i => i.Key);
            workbook.Worksheets[0].DeleteRow(indexRow, maxRowIndex - indexRow);

            // Gestion de l'entête
            var editeur = personnelManager.GetPersonnel(userId);
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + imageManager.GetLogoImage(editeur.SocieteId.Value).Path;
            string dateEdition = BusinessResources.Pointage_Interimaire_Export_DateEdition + DateTime.Now.ToShortDateString() + BusinessResources.Pointage_Interimaire_Export__Espace + DateTime.Now.ToShortTimeString();
            string editePar = BusinessResources.Pointage_Interimaire_Export_EditePar + editeur.CodeNomPrenom;
            string periode = BusinessResources.Pointage_Interimaire_Export_Periode + new DateTime(etatPaieExportModel.Annee, etatPaieExportModel.Mois, 1).ToString("MM/yyyy");
            var buildHeaderModel = new BuildHeaderExcelModel(FeatureRapport.EtatPaie_ControlePointage_Titre, affaireLib, dateEdition, editePar, periode, pathLogo, new IndexHeaderExcelModel(3, 21, lastDayInMonthIndex - 1, lastDayInMonthIndex + 1));
            excelFormat.BuildHeader(workbook.Worksheets[0], buildHeaderModel);

            for (int i = 6; i <= 10 + compteur; i++)
            {
                string cellBegin = excelFormat.GetCellAdress(workbook, i, IndexColOrigin);
                string cellEnd = excelFormat.GetCellAdress(workbook, i, indexColEnd);

                string formula = string.Concat("SUM(", cellBegin, ":", cellEnd, ")");
                excelFormat.SetFormula(workbook, i, IndexColTotal - (31 - dayInMonth), formula);
            }

            return workbook;
        }

        private void HandleAstreintes(ExcelFormat excelFormat, IWorkbook workbook, List<PointageMensuelPersonEnt.Astreintes> listAstreinte)
        {
            //ajout des astreintes
            foreach (PointageMensuelPersonEnt.Astreintes astreinte in listAstreinte)
            {
                excelFormat.InsertRowFormatAsBefore(workbook, indexRow);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin, astreinte.Libelle);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 1, astreinte.Jour1);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 2, astreinte.Jour2);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 3, astreinte.Jour3);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 4, astreinte.Jour4);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 5, astreinte.Jour5);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 6, astreinte.Jour6);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 7, astreinte.Jour7);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 8, astreinte.Jour8);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 9, astreinte.Jour9);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 10, astreinte.Jour10);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 11, astreinte.Jour11);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 12, astreinte.Jour12);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 13, astreinte.Jour13);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 14, astreinte.Jour14);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 15, astreinte.Jour15);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 16, astreinte.Jour16);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 17, astreinte.Jour17);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 18, astreinte.Jour18);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 19, astreinte.Jour19);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 20, astreinte.Jour20);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 21, astreinte.Jour21);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 22, astreinte.Jour22);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 23, astreinte.Jour23);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 24, astreinte.Jour24);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 25, astreinte.Jour25);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 26, astreinte.Jour26);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 27, astreinte.Jour27);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 28, astreinte.Jour28);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 29, astreinte.Jour29);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 30, astreinte.Jour30);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 31, astreinte.Jour31);
                string cellBegin = excelFormat.GetCellAdress(workbook, indexRow, IndexColOrigin);
                string cellEnd = excelFormat.GetCellAdress(workbook, indexRow, indexColEnd);
                string cellTotal = excelFormat.GetCellAdress(workbook, indexRow, IndexColTotal);
                string range = cellBegin + ":" + cellTotal;
                if (astreinte.Libelle.Contains("sortie"))
                {
                    string formula = string.Concat("SUM(", cellBegin, ":", cellEnd, ")");
                    excelFormat.SetFormula(workbook, indexRow, IndexColTotal, formula);
                }
                else
                {
                    excelFormat.SetCellValue(workbook, indexRow, IndexColTotal, astreinte.TotalAstreintes.ToString());
                }

                excelFormat.ChangeBorderStyle(workbook, range, ExcelBordersIndex.EdgeBottom, ExcelLineStyle.Dashed);
                excelFormat.ChangeColor(workbook, range, Color.White);
                excelFormat.ChangeBorderStyle(workbook, cellTotal, ExcelBordersIndex.EdgeLeft, ExcelLineStyle.Thin);
                indexRow += 1;
                compteur++;
            }
        }

        private void HandleAbsences(ExcelFormat excelFormat, IWorkbook workbook, List<PointageMensuelPersonEnt.Absences> listAbsences)
        {
            // Ajout des absences
            foreach (PointageMensuelPersonEnt.Absences absence in listAbsences)
            {
                excelFormat.InsertRowFormatAsBefore(workbook, indexRow);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin, absence.Libelle);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 1, absence.Jour1);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 2, absence.Jour2);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 3, absence.Jour3);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 4, absence.Jour4);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 5, absence.Jour5);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 6, absence.Jour6);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 7, absence.Jour7);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 8, absence.Jour8);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 9, absence.Jour9);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 10, absence.Jour10);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 11, absence.Jour11);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 12, absence.Jour12);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 13, absence.Jour13);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 14, absence.Jour14);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 15, absence.Jour15);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 16, absence.Jour16);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 17, absence.Jour17);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 18, absence.Jour18);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 19, absence.Jour19);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 20, absence.Jour20);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 21, absence.Jour21);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 22, absence.Jour22);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 23, absence.Jour23);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 24, absence.Jour24);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 25, absence.Jour25);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 26, absence.Jour26);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 27, absence.Jour27);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 28, absence.Jour28);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 29, absence.Jour29);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 30, absence.Jour30);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 31, absence.Jour31);
                string cellBegin = excelFormat.GetCellAdress(workbook, indexRow, IndexColOrigin);
                string cellTotal = excelFormat.GetCellAdress(workbook, indexRow, IndexColTotal);
                string range = cellBegin + ":" + cellTotal;
                excelFormat.ChangeBorderStyle(workbook, range, ExcelBordersIndex.EdgeBottom, ExcelLineStyle.Dashed);
                excelFormat.ChangeColor(workbook, range, Color.White);
                excelFormat.ChangeBorderStyle(workbook, cellTotal, ExcelBordersIndex.EdgeLeft, ExcelLineStyle.Thin);
                indexRow += 1;
                compteur++;
            }
        }

        private void HandlePrimes(ExcelFormat excelFormat, IWorkbook workbook, List<PointageMensuelPersonEnt.Primes> listPrimes, Dictionary<string, double> totalPrime)
        {
            //Ajout des primes
            foreach (PointageMensuelPersonEnt.Primes prime in listPrimes)
            {
                excelFormat.InsertRowFormatAsBefore(workbook, indexRow);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin, prime.Libelle);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 1, prime.Jour1);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 2, prime.Jour2);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 3, prime.Jour3);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 4, prime.Jour4);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 5, prime.Jour5);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 6, prime.Jour6);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 7, prime.Jour7);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 8, prime.Jour8);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 9, prime.Jour9);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 10, prime.Jour10);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 11, prime.Jour11);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 12, prime.Jour12);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 13, prime.Jour13);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 14, prime.Jour14);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 15, prime.Jour15);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 16, prime.Jour16);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 17, prime.Jour17);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 18, prime.Jour18);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 19, prime.Jour19);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 20, prime.Jour20);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 21, prime.Jour21);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 22, prime.Jour22);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 23, prime.Jour23);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 24, prime.Jour24);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 25, prime.Jour25);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 26, prime.Jour26);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 27, prime.Jour27);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 28, prime.Jour28);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 29, prime.Jour29);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 30, prime.Jour30);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 31, prime.Jour31);
                string cellBegin = excelFormat.GetCellAdress(workbook, indexRow, IndexColOrigin);
                string cellTotal = excelFormat.GetCellAdress(workbook, indexRow, IndexColTotal);
                string range = cellBegin + ":" + cellTotal;
                string valueTotal = totalPrime.FirstOrDefault(i => prime.Libelle.Contains(i.Key)).Value.ToString();
                excelFormat.SetCellValue(workbook, indexRow, IndexColTotal, valueTotal);
                excelFormat.ChangeBorderStyle(workbook, range, ExcelBordersIndex.EdgeBottom, ExcelLineStyle.Dashed);
                excelFormat.ChangeBorderStyle(workbook, cellTotal, ExcelBordersIndex.EdgeLeft, ExcelLineStyle.Thin);
                indexRow += 1;
            }
        }

        private void HandleIGD(ExcelFormat excelFormat, IWorkbook workbook, IEnumerable<PointageMensuelPersonEnt.Primes> igd)
        {
            //Gestion des primes IGD
            if (igd.Any())
            {
                int cptIGD = 0;
                excelFormat.InsertRowFormatAsBefore(workbook, indexRow);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin, "Prime IGD");

                for (int i = 1; i <= 31; i++)
                {
                    foreach (var item in igd)
                    {
                        string value = item.GetType().GetProperty("Jour" + i).GetValue(item).ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + i, value);
                            cptIGD++;
                        }
                    }
                }
                string cellBegin = excelFormat.GetCellAdress(workbook, indexRow + 1, IndexColOrigin);
                string cellTotal = excelFormat.GetCellAdress(workbook, indexRow + 1, IndexColTotal);
                string range = cellBegin + ":" + cellTotal;

                excelFormat.SetFormula(workbook, indexRow, IndexColTotal, cptIGD.ToString());
                excelFormat.ChangeBorderStyle(workbook, range, ExcelBordersIndex.EdgeBottom, ExcelLineStyle.Dashed);
                excelFormat.ChangeColor(workbook, range, Color.White);
                excelFormat.ChangeBorderStyle(workbook, cellTotal, ExcelBordersIndex.EdgeLeft, ExcelLineStyle.Thin);
                indexRow += 1;
            }
        }

        private void HandleIPD(ExcelFormat excelFormat, IWorkbook workbook, IEnumerable<PointageMensuelPersonEnt.Primes> ipd)
        {
            //Gestion des primes IPD
            if (ipd.Any())
            {
                int cptIPD = 0;
                excelFormat.InsertRowFormatAsBefore(workbook, indexRow);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin, "Prime IPD");
                for (int i = 1; i <= 31; i++)
                {
                    foreach (var item in ipd)
                    {
                        string value = item.GetType().GetProperty("Jour" + i).GetValue(item).ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + i, value);
                            cptIPD++;
                        }
                    }
                }
                string cellBegin = excelFormat.GetCellAdress(workbook, indexRow + 1, IndexColOrigin);
                string cellTotal = excelFormat.GetCellAdress(workbook, indexRow + 1, IndexColTotal);
                string range = cellBegin + ":" + cellTotal;
                excelFormat.SetFormula(workbook, indexRow, IndexColTotal, cptIPD.ToString());
                excelFormat.ChangeBorderStyle(workbook, range, ExcelBordersIndex.EdgeBottom, ExcelLineStyle.Dashed);
                excelFormat.ChangeColor(workbook, range, Color.White);
                excelFormat.ChangeBorderStyle(workbook, cellTotal, ExcelBordersIndex.EdgeLeft, ExcelLineStyle.Thin);
                indexRow += 1;
            }
        }

        private void HandleHeuresSupplementaire(ExcelFormat excelFormat, IWorkbook workbook, EtatPaieExportModel etatPaieExportModel, PointageMensuelPersonEnt pointage, string heuresSemaine)
        {
            DateTime date = new DateTime(etatPaieExportModel.Annee, etatPaieExportModel.Mois, 1);

            int nbrDay = DateTime.DaysInMonth(date.Year, date.Month);
            Dictionary<int, double> heureSup = new Dictionary<int, double>();
            double sommeSemaine = 0;

            for (int i = 0; i < nbrDay; i++)
            {
                DateTime datenow = date.AddDays(i);
                if (datenow.DayOfWeek == DayOfWeek.Friday)
                {
                    double somme = !string.IsNullOrEmpty(pointage.ListHeuresPointees.GetType().GetProperty("Jour" + (i + 1))?.GetValue(pointage.ListHeuresPointees).ToString()) ? double.Parse(pointage.ListHeuresPointees.GetType().GetProperty("Jour" + (i + 1))?.GetValue(pointage.ListHeuresPointees).ToString()) : 0;
                    double calcul = (sommeSemaine + somme) - double.Parse(heuresSemaine);

                    heureSup.Add(i, calcul > 0 ? calcul : 0);
                    sommeSemaine = 0;
                }
                else
                {
                    double somme = !string.IsNullOrEmpty(pointage.ListHeuresPointees.GetType().GetProperty("Jour" + (i + 1))?.GetValue(pointage.ListHeuresPointees).ToString()) ? double.Parse(pointage.ListHeuresPointees.GetType().GetProperty("Jour" + (i + 1))?.GetValue(pointage.ListHeuresPointees).ToString()) : 0;
                    sommeSemaine += somme;
                }
            }
            excelFormat.InsertRowFormatAsBefore(workbook, indexRow);
            excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin, "Heure Supplémentaire");

            foreach (var item in heureSup)
            {
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + item.Key + 1, item.Value.ToString());
            }

            string cellBegin = excelFormat.GetCellAdress(workbook, indexRow + 1, IndexColOrigin);
            string cellTotal = excelFormat.GetCellAdress(workbook, indexRow + 1, IndexColTotal);
            string range = cellBegin + ":" + cellTotal;
            excelFormat.ChangeBorderStyle(workbook, range, ExcelBordersIndex.EdgeBottom, ExcelLineStyle.Dashed);
            excelFormat.ChangeColor(workbook, range, Color.White);
            excelFormat.ChangeBorderStyle(workbook, cellTotal, ExcelBordersIndex.EdgeLeft, ExcelLineStyle.Thin);
            indexRow += 1;
            compteur++;
        }

        private void HandleZonesDeplacement(ExcelFormat excelFormat, IWorkbook workbook, PointageMensuelPersonEnt.CodeZoneDeplacements listCodeZoneDeplacements)
        {
            // Ajout des codes zone déplacement
            if (listCodeZoneDeplacements != null)
            {
                var codeZoneDeplacement = listCodeZoneDeplacements;
                excelFormat.InsertRowFormatAsBefore(workbook, indexRow);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin, codeZoneDeplacement.Libelle);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 1, codeZoneDeplacement.Jour1);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 2, codeZoneDeplacement.Jour2);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 3, codeZoneDeplacement.Jour3);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 4, codeZoneDeplacement.Jour4);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 5, codeZoneDeplacement.Jour5);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 6, codeZoneDeplacement.Jour6);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 7, codeZoneDeplacement.Jour7);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 8, codeZoneDeplacement.Jour8);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 9, codeZoneDeplacement.Jour9);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 10, codeZoneDeplacement.Jour10);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 11, codeZoneDeplacement.Jour11);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 12, codeZoneDeplacement.Jour12);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 13, codeZoneDeplacement.Jour13);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 14, codeZoneDeplacement.Jour14);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 15, codeZoneDeplacement.Jour15);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 16, codeZoneDeplacement.Jour16);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 17, codeZoneDeplacement.Jour17);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 18, codeZoneDeplacement.Jour18);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 19, codeZoneDeplacement.Jour19);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 20, codeZoneDeplacement.Jour20);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 21, codeZoneDeplacement.Jour21);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 22, codeZoneDeplacement.Jour22);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 23, codeZoneDeplacement.Jour23);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 24, codeZoneDeplacement.Jour24);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 25, codeZoneDeplacement.Jour25);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 26, codeZoneDeplacement.Jour26);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 27, codeZoneDeplacement.Jour27);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 28, codeZoneDeplacement.Jour28);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 29, codeZoneDeplacement.Jour29);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 30, codeZoneDeplacement.Jour30);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 31, codeZoneDeplacement.Jour31);
                excelFormat.SetCellValue(workbook, indexRow, IndexColOrigin + 32, codeZoneDeplacement.Total);

                string cellBegin = excelFormat.GetCellAdress(workbook, indexRow, IndexColOrigin);
                string cellTotal = excelFormat.GetCellAdress(workbook, indexRow, IndexColTotal);
                string range = cellBegin + ":" + cellTotal;
                excelFormat.ChangeBorderStyle(workbook, range, ExcelBordersIndex.EdgeBottom, ExcelLineStyle.Dashed);
                excelFormat.ChangeBorderStyle(workbook, cellTotal, ExcelBordersIndex.EdgeLeft, ExcelLineStyle.Thin);

                indexRow += 1;
            }
        }

        /// <summary>
        /// Get personnel statut by statut id
        /// </summary>
        /// <param name="statutId">Statut identifier</param>
        /// <returns>Statut</returns>
        private static string GetPersonnelStatut(string statutId)
        {
            switch (statutId)
            {
                case "1":
                    return Constantes.PersonnelStatutValue.Ouvrier;
                case "2":
                    return Constantes.PersonnelStatutValue.ETAM;
                case "3":
                    return Constantes.PersonnelStatutValue.Cadre;
                case "4":
                    return Constantes.PersonnelStatutValue.ETAM;
                case "5":
                    return Constantes.PersonnelStatutValue.ETAM;
                default:
                    return string.Empty;
            }
        }
    }
}
