using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using Fred.Business.EtatPaie.ControlePointage;
using Fred.Business.Organisation;
using Fred.Business.Params;
using Fred.Business.Personnel;
using Fred.Entities;
using Fred.Entities.Organisation;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Rapport;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
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
    public class ControlePointagesHebdomadaireManager : IControlePointagesHebdomadaireManager
    {
        private readonly IOrganisationManager organisationManager;
        private readonly IPersonnelManager personnelManager;
        private readonly IParamsManager paramsManager;

        private int indexRowAbsence;
        private int indexRowPrimes;
        private int indexRowAstreintes;
        private const int IndexColOrigin = 5;
        private const int IndexColEnd = 11;
        private const int IndexRowJour = 6;
        private const int IndexColLibelle = 1;
        private const int IndexColTotal = 12;

        /// <summary>
        /// Permet de gérer les états paie 
        /// </summary>
        /// <param name="organisationManager">Manager d'organisation</param>
        /// <param name="personnelManager">Manager de personnel</param>
        /// <param name="paramsManager">Manager des paramètres</param>
        public ControlePointagesHebdomadaireManager(IOrganisationManager organisationManager, IPersonnelManager personnelManager, IParamsManager paramsManager)
        {
            this.organisationManager = organisationManager;
            this.personnelManager = personnelManager;
            this.paramsManager = paramsManager;
        }

        /// <summary>
        /// Génère un pdf pour le controle des pointages
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'export </param>
        /// <param name="listePointageMensuel">Model de l'etat de paie pour l'export</param>
        /// <param name="firstDay">Premier jour de la semaine</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <returns>Le fichier généré</returns>
        public MemoryStream GenerateMemoryStreamControlePointagesHebdomadaire(EtatPaieExportModel etatPaieExportModel, List<PointageMensuelPersonEnt> listePointageMensuel, DateTime firstDay, int userId, string templateFolderPath)
        {
            ExcelFormat excelFormat = new ExcelFormat();
            // La date de début de période doit etre définie pour réaliser ce traitement.
            if (!etatPaieExportModel.Date.HasValue)
            {
                throw new FredBusinessMessageResponseException(FeatureRapport.Pointage_Export_Controle_Pointage_Periode_Non_Definie_Error);
            }
            // controle du nombre de personnel exportés dans le cas d'un export pdf
            if (etatPaieExportModel.Pdf && listePointageMensuel.Count > 500)
            {
                throw new FredBusinessMessageResponseException(FeatureRapport.Pointage_Export_Controle_Pointage_Pdf_Personnel_Limit_Error);
            }
            OrganisationEnt organisation = organisationManager.GetOrganisationById(etatPaieExportModel.OrganisationId);
            IWorkbook workbookMaster = excelFormat.GetNewWorbook();
            foreach (PointageMensuelPersonEnt pointage in listePointageMensuel)
            {
                IWorkbook workbook = GenerateExcelHebdomadairePerPerson(excelFormat, pointage, firstDay.Year, firstDay.Month, firstDay.Day, organisation, templateFolderPath);
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
        /// Génère un classeur excel pour le pointage Hebdomadaire d'un personnel 
        /// </summary>
        /// <param name="excelFormat">ExcelFormat</param>
        /// <param name="pointage">Un pointage mensuel pour un personnel</param>
        /// <param name="annee">Année de filtrage pour l'édition</param>
        /// <param name="mois">Mois de filtrage pour l'édition</param>
        /// <param name="jour">jour de filtrage pour l'édition</param>
        /// <param name="organisation">Organisation Entité</param>
        /// <returns>Le classeur généré</returns>
        private IWorkbook GenerateExcelHebdomadairePerPerson(ExcelFormat excelFormat, PointageMensuelPersonEnt pointage, int annee, int mois, int jour, OrganisationEnt organisation, string templateFolderPath)
        {
            string pathName = Path.Combine(templateFolderPath, "TemplateControlePointageHebdomadaire.xlsx");
            IWorkbook workbook = excelFormat.OpenTemplateWorksheet(pathName);
            workbook.Worksheets[0].Name = pointage.Personnel.Matricule;
            string affaireLib = "TOUTES";

            if (organisation != null)
            {
                affaireLib = string.Concat(organisation.Code, " - ", organisation.Libelle);
            }
            int weekOfYear = GetIso8601WeekOfYear(new DateTime(annee, mois, jour).AddDays(2));
            var entete = new
            {
                Affaire = affaireLib,
                PeriodeComptable = "S" + weekOfYear + "/" + (mois == 12 && weekOfYear == 1 ? annee + 1 : annee)
            };
            ContratInterimaireEnt affectationInterim = personnelManager.GetAffectationInterimaireActive(pointage.Personnel.PersonnelId, new DateTime(annee, mois, 1));
            string codeInterim = affectationInterim != null ? affectationInterim.Fournisseur.Code + " - " + affectationInterim.Fournisseur.Libelle : string.Empty;
            var personnel = new
            {
                pointage.Personnel.CodeNomPrenom,
                Interim = codeInterim,
                Entreprise = pointage.Personnel.Societe != null ? pointage.Personnel.Societe.Libelle : string.Empty,
                Fonction = pointage.Personnel.Ressource != null ? pointage.Personnel.Ressource.Libelle : string.Empty,
                Statut = GetPersonnelStatut(pointage.Personnel.Statut),
                responsable = pointage.Personnel.Manager != null ? pointage.Personnel.Manager.PrenomNom : string.Empty
            };
            List<int> joursNumber = new List<int>();
            for (int i = 0; i < 7; i++)
            {
                joursNumber.Add((new DateTime(annee, mois, jour)).AddDays(i).Day);
            }

            excelFormat.InitVariables(workbook);
            excelFormat.AddVariable("Entete", entete);
            excelFormat.AddVariable("Personnel", personnel);
            excelFormat.AddVariable("HeuresNonMajo", pointage.ListHeuresNormales);
            excelFormat.AddVariable("HeuresMajo", pointage.ListHeuresMajo);
            excelFormat.AddVariable("HeuresTravaillees", pointage.ListHeuresTravaillees);
            excelFormat.AddVariable("HeuresAbsence", pointage.ListHeuresAbsence);
            excelFormat.AddVariable("HeuresPointees", pointage.ListHeuresPointees);
            excelFormat.ApplyVariables();            
            
            int indexRowHeuresSupplementaire = pointage.ListAstreintes.Count + pointage.ListAbsences.Count + 11;

            //ajout des jours
            var daysWithNames = GetDayNumbersWithNames(joursNumber, mois, annee);
            if (daysWithNames != null)
            {
                foreach (var dayWithName in daysWithNames)
                {
                    excelFormat.SetCellValue(workbook, IndexRowJour, IndexColOrigin + joursNumber.IndexOf(dayWithName.Key), dayWithName.Value);
                }
            }

            //un changement de mois en cours de semaine 
            var rowEnd = 11;
            if (joursNumber.Contains(1))
            {
                string cellBegin = excelFormat.GetCellAdress(workbook, 6, IndexColOrigin + joursNumber.IndexOf(1));
                string cellEnd = excelFormat.GetCellAdress(workbook, rowEnd, IndexColOrigin + joursNumber.IndexOf(1));
                string range = cellBegin + ":" + cellEnd;
                excelFormat.ChangeBorderStyle(workbook, range, ExcelBordersIndex.EdgeLeft, ExcelLineStyle.Thick);
            }

            HandleAstreintes(excelFormat, workbook, pointage.ListAstreintes);
            HandleAbsence(excelFormat, workbook, pointage.ListAbsences);
            HandleIGD(excelFormat, workbook, pointage.ListPrimes, pointage.Totalprime);
            HandleIPD(excelFormat, workbook, pointage.ListPrimes, pointage.Totalprime);
            HandleAutresPrimes(excelFormat, workbook, pointage.ListPrimes, pointage.Totalprime);
            HandleHeuresSupplementaires(excelFormat, workbook, personnel.Statut, pointage.Personnel.Societe.Organisation.OrganisationId, indexRowHeuresSupplementaire);           

            return workbook;
        }

        /// <summary>
        /// Get Day Numbers With Names
        /// </summary>
        /// <param name="listJour">La liste des jours</param>
        /// <param name="mois">Le numero du mois</param>
        /// <param name="annee">Le numero de l'année</param>
        /// <returns>Dictionnaire des jours</returns>
        private Dictionary<int, string> GetDayNumbersWithNames(List<int> listJour, int mois, int annee)
        {
            Dictionary<int, string> daysWithNumbers = new Dictionary<int, string>();

            if (listJour?.Count > 0)
            {
                var cultinfo = CultureInfo.CreateSpecificCulture("fr-FR");
                var dateformat = cultinfo.DateTimeFormat;
                string dayName = string.Empty;
                DateTime iDay = new DateTime();

                foreach (var jour in listJour)
                {
                    // vérifier si le jour courant appartient à un autre mois
                    if (jour == 1 && listJour.First() != 1)
                    {
                        // incrémentation du mois et de l'année
                        if (mois == 12)
                        {
                            annee++;
                            mois = 1;
                        }
                        else
                        {
                            mois++;
                        }
                    }

                    iDay = new DateTime(annee, mois, jour);
                    dayName = jour + "\n" + dateformat.AbbreviatedDayNames[(int)iDay.DayOfWeek].Replace(".", string.Empty);
                    daysWithNumbers.Add(jour, dayName ?? string.Empty);
                }
            }

            return daysWithNumbers;
        }

        private void HandleAstreintes(ExcelFormat excelFormat, IWorkbook workbook, List<PointageMensuelPersonEnt.Astreintes> listAstreintes)
        {
            indexRowAstreintes = 10;
            //ajout des astreintes
            foreach (var astreinte in listAstreintes)
            {
                excelFormat.InsertRowFormatAsBefore(workbook, indexRowAstreintes);
                excelFormat.SetCellValue(workbook, indexRowAstreintes, IndexColLibelle, astreinte.Libelle);
                excelFormat.SetCellValue(workbook, indexRowAstreintes, IndexColOrigin + 0, astreinte.Jour1);
                excelFormat.SetCellValue(workbook, indexRowAstreintes, IndexColOrigin + 1, astreinte.Jour2);
                excelFormat.SetCellValue(workbook, indexRowAstreintes, IndexColOrigin + 2, astreinte.Jour3);
                excelFormat.SetCellValue(workbook, indexRowAstreintes, IndexColOrigin + 3, astreinte.Jour4);
                excelFormat.SetCellValue(workbook, indexRowAstreintes, IndexColOrigin + 4, astreinte.Jour5);
                excelFormat.SetCellValue(workbook, indexRowAstreintes, IndexColOrigin + 5, astreinte.Jour6);
                excelFormat.SetCellValue(workbook, indexRowAstreintes, IndexColOrigin + 6, astreinte.Jour7);

                string cellBegin = excelFormat.GetCellAdress(workbook, indexRowAstreintes, IndexColOrigin);
                string cellEnd = excelFormat.GetCellAdress(workbook, indexRowAstreintes, IndexColEnd);
                string cellTotal = excelFormat.GetCellAdress(workbook, indexRowAstreintes, IndexColTotal);
                string range = cellBegin + ":" + cellTotal;
                if (astreinte.Libelle.Contains("sortie"))
                {
                    string formula = string.Concat("SUM(", cellBegin, ":", cellEnd, ")");
                    excelFormat.SetFormula(workbook, indexRowAstreintes, IndexColTotal, formula);
                }
                else
                {
                    excelFormat.SetCellValue(workbook, indexRowAstreintes, IndexColTotal, astreinte.TotalAstreintes.ToString());
                }

                excelFormat.ChangeBorderStyle(workbook, range, ExcelBordersIndex.EdgeBottom, ExcelLineStyle.Thin);
                excelFormat.ChangeColor(workbook, range, Color.White);
                excelFormat.ChangeBorderStyle(workbook, cellTotal, ExcelBordersIndex.EdgeLeft, ExcelLineStyle.Thin);
                indexRowAstreintes += 1;
            }
        }

        private void HandleAbsence(ExcelFormat excelFormat, IWorkbook workbook, List<PointageMensuelPersonEnt.Absences> listAbsences)
        {
            // Ajout des absences
            indexRowAbsence = indexRowAstreintes;
            foreach (PointageMensuelPersonEnt.Absences absence in listAbsences)
            {
                excelFormat.InsertRowFormatAsBefore(workbook, indexRowAbsence);
                excelFormat.SetCellValue(workbook, indexRowAbsence, IndexColLibelle, absence.Libelle);
                excelFormat.SetCellValue(workbook, indexRowAbsence, IndexColOrigin + 0, new string(absence.Jour1.DefaultIfEmpty('0').ToArray()));
                excelFormat.SetCellValue(workbook, indexRowAbsence, IndexColOrigin + 1, new string(absence.Jour2.DefaultIfEmpty('0').ToArray()));
                excelFormat.SetCellValue(workbook, indexRowAbsence, IndexColOrigin + 2, new string(absence.Jour3.DefaultIfEmpty('0').ToArray()));
                excelFormat.SetCellValue(workbook, indexRowAbsence, IndexColOrigin + 3, new string(absence.Jour4.DefaultIfEmpty('0').ToArray()));
                excelFormat.SetCellValue(workbook, indexRowAbsence, IndexColOrigin + 4, new string(absence.Jour5.DefaultIfEmpty('0').ToArray()));
                excelFormat.SetCellValue(workbook, indexRowAbsence, IndexColOrigin + 5, new string(absence.Jour6.DefaultIfEmpty('0').ToArray()));
                excelFormat.SetCellValue(workbook, indexRowAbsence, IndexColOrigin + 6, new string(absence.Jour7.DefaultIfEmpty('0').ToArray()));

                string cellBegin = excelFormat.GetCellAdress(workbook, indexRowAbsence, IndexColOrigin);
                string cellEnd = excelFormat.GetCellAdress(workbook, indexRowAbsence, IndexColEnd);
                string cellTotal = excelFormat.GetCellAdress(workbook, indexRowAbsence, IndexColTotal);
                string range = cellBegin + ":" + cellTotal;
                string formula = string.Concat("SUM(", cellBegin, ":", cellEnd, ")");
                excelFormat.SetFormula(workbook, indexRowAbsence, IndexColTotal, formula);
                excelFormat.ChangeBorderStyle(workbook, range, ExcelBordersIndex.EdgeBottom, ExcelLineStyle.Thin);
                excelFormat.ChangeColor(workbook, range, Color.White);
                excelFormat.ChangeBorderStyle(workbook, cellTotal, ExcelBordersIndex.EdgeLeft, ExcelLineStyle.Thin);
                indexRowAbsence += 1;
            }
        }

        private void HandleIGD(ExcelFormat excelFormat, IWorkbook workbook, List<PointageMensuelPersonEnt.Primes> listPrimes, Dictionary<string,double> totalPrime)
        {
            //Ajout des primes IGD & IPD
            indexRowPrimes = indexRowAbsence + 2;
            var igd = listPrimes.Where(i => i.Libelle.Contains("IGD"));

            if (igd.Any())
            {
                excelFormat.InsertRowFormatAsBefore(workbook, indexRowPrimes);
                excelFormat.SetCellValue(workbook, indexRowPrimes, IndexColLibelle, "Prime IGD");
                for (int i = 1; i <= 31; i++)
                {
                    foreach (var prime in igd)
                    {
                        var value = prime.GetType().GetProperty("Jour" + i).GetValue(prime).ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            excelFormat.SetCellValue(workbook, indexRowPrimes, IndexColOrigin + (i - 1), value);

                            string valueTotal = totalPrime.FirstOrDefault(t => prime.Libelle.Contains(t.Key)).Value.ToString();
                            excelFormat.SetCellValue(workbook, indexRowPrimes, IndexColTotal, valueTotal);
                        }
                    }
                }
                indexRowPrimes += 1;
            }            
        }

        private void HandleIPD(ExcelFormat excelFormat, IWorkbook workbook, List<PointageMensuelPersonEnt.Primes> listPrimes, Dictionary<string, double> totalPrime)
        {
            var ipd = listPrimes.Where(i => i.Libelle.Contains("IPD"));
            if (ipd.Any())
            {
                excelFormat.InsertRowFormatAsBefore(workbook, indexRowPrimes);
                excelFormat.SetCellValue(workbook, indexRowPrimes, IndexColLibelle, "Prime IPD");
                for (int i = 1; i <= 31; i++)
                {
                    foreach (var prime in ipd)
                    {
                        var value = prime.GetType().GetProperty("Jour" + i).GetValue(prime).ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            excelFormat.SetCellValue(workbook, indexRowPrimes, IndexColOrigin + (i - 1), value);
                            string valueTotal = totalPrime.FirstOrDefault(t => prime.Libelle.Contains(t.Key)).Value.ToString();
                            excelFormat.SetCellValue(workbook, indexRowPrimes, IndexColTotal, valueTotal);

                        }
                    }
                }
                indexRowPrimes += 1;
            }

        }

        private void HandleAutresPrimes(ExcelFormat excelFormat, IWorkbook workbook, List<PointageMensuelPersonEnt.Primes> listPrimes, Dictionary<string, double> totalPrime)
        {
            var autresPrimes = listPrimes.Where(i => !i.Libelle.Contains("IGD") && !i.Libelle.Contains("IPD"));
            //Ajout des primes
            foreach (PointageMensuelPersonEnt.Primes prime in autresPrimes)
            {
                excelFormat.InsertRowFormatAsBefore(workbook, indexRowPrimes);
                excelFormat.SetCellValue(workbook, indexRowPrimes, IndexColLibelle, prime.Libelle.Split('-')[1]);
                excelFormat.SetCellValue(workbook, indexRowPrimes, IndexColOrigin + 0, prime.Jour1);
                excelFormat.SetCellValue(workbook, indexRowPrimes, IndexColOrigin + 1, prime.Jour2);
                excelFormat.SetCellValue(workbook, indexRowPrimes, IndexColOrigin + 2, prime.Jour3);
                excelFormat.SetCellValue(workbook, indexRowPrimes, IndexColOrigin + 3, prime.Jour4);
                excelFormat.SetCellValue(workbook, indexRowPrimes, IndexColOrigin + 4, prime.Jour5);
                excelFormat.SetCellValue(workbook, indexRowPrimes, IndexColOrigin + 5, prime.Jour6);
                excelFormat.SetCellValue(workbook, indexRowPrimes, IndexColOrigin + 6, prime.Jour7);

                string cellBegin = excelFormat.GetCellAdress(workbook, indexRowPrimes, IndexColOrigin);
                string cellTotal = excelFormat.GetCellAdress(workbook, indexRowPrimes, IndexColTotal);
                string range = cellBegin + ":" + cellTotal;
                string valueTotal = totalPrime.FirstOrDefault(i => prime.Libelle.Contains(i.Key)).Value.ToString();
                excelFormat.SetCellValue(workbook, indexRowPrimes, IndexColTotal, valueTotal);

                excelFormat.ChangeBorderStyle(workbook, range, ExcelBordersIndex.EdgeBottom, ExcelLineStyle.Thin);
                excelFormat.ChangeColor(workbook, range, Color.White);
                excelFormat.ChangeBorderStyle(workbook, cellTotal, ExcelBordersIndex.EdgeLeft, ExcelLineStyle.Thin);
                indexRowPrimes += 1;
            }
        }

        private void HandleHeuresSupplementaires(ExcelFormat excelFormat, IWorkbook workbook, string statut, int organisationId, int indexRowHeuresSupplementaire)
        {
            // « Heures supplémentaires » pour Ouvrier et ETAM
            if (statut != Constantes.PersonnelStatutValue.Cadre)
            {
                var heuresSemaine = paramsManager.GetParamValue(organisationId, "HeuresSemaineOuvrierETAM") ?? 35d.ToString();

                excelFormat.SetCellValue(workbook, indexRowHeuresSupplementaire, 13, "Heures supplémentaires:");
                var borderTypes = ((ExcelBordersIndex[])Enum.GetValues(typeof(ExcelBordersIndex))).Where(i => i != ExcelBordersIndex.DiagonalDown && i != ExcelBordersIndex.DiagonalUp);
                foreach (var border in borderTypes)
                {
                    if (border.ToIntValue() != 7)
                    {
                        excelFormat.ChangeBorderStyle(workbook, "M" + indexRowHeuresSupplementaire, border, ExcelLineStyle.Thin);
                        excelFormat.ChangeBorderStyle(workbook, "N" + indexRowHeuresSupplementaire, border, ExcelLineStyle.Thin);
                    }
                    excelFormat.ChangeBorderStyle(workbook, "O" + indexRowHeuresSupplementaire, border, ExcelLineStyle.Thin);
                }
                string cellBegin = excelFormat.GetCellAdress(workbook, indexRowHeuresSupplementaire, IndexColOrigin);
                string cellEnd = excelFormat.GetCellAdress(workbook, indexRowHeuresSupplementaire, IndexColEnd);
                string sumFormula = string.Concat("SUM(", cellBegin, ":", cellEnd, ")", "-", heuresSemaine);
                string formula = string.Concat("IF(", sumFormula, ">0", ";", sumFormula, ";0)");
                excelFormat.SetFormula(workbook, indexRowHeuresSupplementaire, 15, formula);
            }
        }

        // Cela suppose que les semaines commencent par lundi.
        // La semaine 1 est la première semaine de l'année avec un jeudi.
        private static int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
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
