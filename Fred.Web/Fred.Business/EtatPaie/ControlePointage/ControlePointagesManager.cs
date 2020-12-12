using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using Fred.Business.EtatPaie.ControlePointage;
using Fred.Business.FeatureFlipping;
using Fred.Business.Images;
using Fred.Business.Organisation;
using Fred.Business.Personnel;
using Fred.Business.Personnel.Interimaire;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Framework.FeatureFlipping;
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
    public class ControlePointagesManager : IControlePointagesManager
    {
        private readonly IContratInterimaireManager contratInterimaireManager;
        private readonly IFeatureFlippingManager featureFlippingManager;
        private readonly IOrganisationManager organisationManager;
        private readonly IPersonnelManager personnelManager;
        private readonly IImageManager imageManager;

        private const int IndexColOrigin = 1;
        private const int FirstDayColIndex = 2;
        private const int ArrayFirstColIndex = 1;
        private int indexRow;
        private int indexRowAbsence;
        private int indexColEnd;

        /// <summary>
        /// Permet de gérer les états paie 
        /// </summary>
        /// <param name="contratInterimaireManager">Manager de contrat intérimaire</param>
        /// <param name="featureFlippingManager">Manager de featureFlipping</param>
        /// <param name="imageManager">Manager d'image</param>
        /// <param name="organisationManager">Manager d'organisation</param>
        /// <param name="personnelManager">Manager de personnel</param>
        public ControlePointagesManager(IContratInterimaireManager contratInterimaireManager, IFeatureFlippingManager featureFlippingManager, IOrganisationManager organisationManager, IPersonnelManager personnelManager, IImageManager imageManager)
        {
            this.contratInterimaireManager = contratInterimaireManager;
            this.featureFlippingManager = featureFlippingManager;
            this.organisationManager = organisationManager;
            this.personnelManager = personnelManager;
            this.imageManager = imageManager;
        }

        /// <summary>
        /// Génère un pdf pour le controle des pointages
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'export </param>
        /// <param name="listePointageMensuel">Liste des models de pointage mensuel</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <returns>Le fichier généré</returns>
        public MemoryStream GenerateMemoryStreamControlePointages(EtatPaieExportModel etatPaieExportModel, List<PointageMensuelPersonEnt> listePointageMensuel, int userId, string templateFolderPath)
        {
            ExcelFormat excelFormat = new ExcelFormat();

            List<IWorkbook> listWorbook = new List<IWorkbook>();
            foreach (PointageMensuelPersonEnt pointage in listePointageMensuel)
            {
                IWorkbook workbook = GenerateExcelPerPerson(pointage, etatPaieExportModel.Annee, etatPaieExportModel.Mois, etatPaieExportModel.OrganisationId, userId, etatPaieExportModel.Filtre, templateFolderPath);
                if (workbook != null)
                {
                    listWorbook.Add(workbook);
                }
            }

            IWorkbook workbookMaster = excelFormat.GetNewWorbook();
            foreach (IWorkbook workbook in listWorbook)
            {
                workbookMaster.Worksheets.AddCopy(workbook.Worksheets[0]);
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
        /// <param name="pointage">Un pointage mensuel pour un personnel</param>
        /// <param name="annee">Année de filtrage pour l'édition</param>
        /// <param name="mois">Mois de filtrage pour l'édition</param>
        /// <param name="organisationId">Identifiant de l'organisation</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="filtre">Le filtre à utiliser</param>
        /// <returns>Le classeur généré</returns>
        private IWorkbook GenerateExcelPerPerson(PointageMensuelPersonEnt pointage, int annee, int mois, int organisationId, int userId, TypeFiltreEtatPaie filtre, string templateFolderPath)
        {
            string pathName = Path.Combine(templateFolderPath, GetControlePointagesFileName());
            var ameliorationsActivated = featureFlippingManager.IsActivated(EnumFeatureFlipping.EditionsPaieAmeliorations);
            ExcelFormat excelFormat = new ExcelFormat();
            IWorkbook workbook = excelFormat.OpenTemplateWorksheet(pathName);
            var workbookInfo = new WorkbookInfo() { ExcelFormat = excelFormat, Workbook = workbook };
            workbook.Worksheets[0].Name = pointage.Personnel.Matricule;
            string affaireLib = "TOUTES";
            if (organisationId != 0)
            {
                var orga = organisationManager.GetOrganisationById(organisationId);
                if (orga != null)
                {
                    affaireLib = string.Concat(orga.Code, " - ", orga.Libelle);
                }
            }

            // RG_5172_002-4
            if (ameliorationsActivated && filtre == TypeFiltreEtatPaie.Population)
            {
                affaireLib += " (uniquement mes rapports verrouillés)";
            }

            var lastColumnIndex = 32;
            // RG_5172_002-1 & RG_5172_002-2
            if (ameliorationsActivated)
            {
                lastColumnIndex = SetDaysInWorksheet(workbookInfo, 0, annee, mois, 4, 1);
            }

            // Gestion de l'entête
            var editeur = personnelManager.GetPersonnel(userId);
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + imageManager.GetLogoImage(editeur.SocieteId.Value).Path;
            string dateEdition = BusinessResources.Pointage_Interimaire_Export_DateEdition + DateTime.Now.ToShortDateString() + BusinessResources.Pointage_Interimaire_Export__Espace + DateTime.Now.ToShortTimeString();
            string editePar = BusinessResources.Pointage_Interimaire_Export_EditePar + editeur.CodeNomPrenom;
            string periode = BusinessResources.Pointage_Interimaire_Export_Periode + new DateTime(annee, mois, 1).ToString("MM/yyyy");
            var buildHeaderModel = new BuildHeaderExcelModel(FeatureRapport.EtatPaie_ControlePointage_Titre, affaireLib, dateEdition, editePar, periode, pathLogo, new IndexHeaderExcelModel(3, 21, lastColumnIndex - 2, lastColumnIndex));
            workbookInfo.ExcelFormat.BuildHeader(workbook.Worksheets[0], buildHeaderModel);

            string codeInterim;
            string codeFonction;

            HandleInterimaire(pointage.Personnel.PersonnelId, annee, mois, out codeInterim, out codeFonction);

            var personnel = new
            {
                CodeNomPrenom = pointage.Personnel.CodeNomPrenom,
                Interim = codeInterim,
                Entreprise = pointage.Personnel.Societe != null ? pointage.Personnel.Societe.Libelle : string.Empty,
                Fonction = codeFonction != string.Empty ? codeFonction : pointage.Personnel.Ressource != null ? pointage.Personnel.Ressource.Libelle : string.Empty
            };

            workbookInfo.ExcelFormat.InitVariables(workbook);
            workbookInfo.ExcelFormat.AddVariable("Personnel", personnel);
            workbookInfo.ExcelFormat.AddVariable("HeuresNonMajo", pointage.ListHeuresNormales);
            workbookInfo.ExcelFormat.AddVariable("HeuresMajo", pointage.ListHeuresMajo);
            workbookInfo.ExcelFormat.AddVariable("HeuresAPied", pointage.ListHeuresAPied);
            workbookInfo.ExcelFormat.AddVariable("HeuresAbsence", pointage.ListHeuresAbsence);
            workbookInfo.ExcelFormat.AddVariable("HeuresPointees", pointage.ListHeuresPointees);
            workbookInfo.ExcelFormat.AddVariable("HeuresTravaillees", pointage.ListHeuresTravaillees);
            workbookInfo.ExcelFormat.ApplyVariables();

            if (ameliorationsActivated)
            {
                indexRow = 12;
                indexRowAbsence = 12;
                indexColEnd = DateTime.DaysInMonth(annee, mois) + 1;

                // Ajoute les formules des totaux sur les 6 premières lignes
                var indexFirstRow = 6;
                for (var i = 0; i < 6; i++)
                {
                    AddSumFormulaAndSetStyleToControlePointageExport(workbookInfo, indexFirstRow + i, indexColEnd, lastColumnIndex);
                }
            }
            else
            {
                indexRow = 12;
                indexRowAbsence = 10;
                indexColEnd = 32;
            }

            HandleAbsences( workbookInfo, pointage.ListAbsences, ameliorationsActivated, lastColumnIndex);
            HandlePrimes(workbookInfo, pointage.ListPrimes, ameliorationsActivated, lastColumnIndex);
            HandleDeplacements( workbookInfo, pointage.ListDeplacements, ameliorationsActivated, lastColumnIndex);
            HandleIVD(workbookInfo, pointage.ListIVD, ameliorationsActivated, lastColumnIndex);
            HandleZonesDeplacement(workbookInfo, pointage.ListCodeZoneDeplacements, lastColumnIndex);

            int maxRowIndex = workbook.Worksheets[0].Rows.Index().Max(i => i.Key);
            workbook.Worksheets[0].DeleteRow(indexRow + 1, maxRowIndex - indexRow);
            
            return workbook;
        }

        private void HandleInterimaire(int personnelId, int annee, int mois, out string codeInterim, out string codeFonction)
        {
            codeInterim = string.Empty;
            codeFonction = string.Empty;
            List<ContratInterimaireEnt> listContratInterimaire = contratInterimaireManager.GetListContratInterimaireOpenOnPeriod(personnelId, new DateTime(annee, mois, 1));
            if (listContratInterimaire.Count > 0)
            {
                List<FournisseurEnt> listFournisseur = listContratInterimaire.DistinctBy(c => c.FournisseurId).Select(c => c.Fournisseur).ToList();
                List<RessourceEnt> listRessource = listContratInterimaire.DistinctBy(c => c.RessourceId).Select(c => c.Ressource).ToList();
                foreach (var fournisseur in listFournisseur)
                {
                    if (listFournisseur.IndexOf(fournisseur) != 0)
                    {
                        codeInterim = string.Concat(codeInterim, " / ");
                    }
                    codeInterim = string.Concat(codeInterim, fournisseur.Libelle);
                }

                foreach (var ressource in listRessource)
                {
                    if (listRessource.IndexOf(ressource) != 0)
                    {
                        codeFonction = string.Concat(codeFonction, " / ");
                    }
                    codeFonction = string.Concat(codeFonction, ressource.Libelle);
                }
            }
        }

        private void HandleAbsences(WorkbookInfo workbookInfo, List<PointageMensuelPersonEnt.Absences> listAbsences, bool ameliorationsActivated, int lastColumnIndex)
        {
            IEnumerable<IGrouping<string, PointageMensuelPersonEnt.Absences>> typesAbsences = listAbsences.GroupBy(l => l.Libelle);
            // Ajout des absences
            foreach (IEnumerable<PointageMensuelPersonEnt.Absences> typeAbsence in typesAbsences)
            {
                workbookInfo.ExcelFormat.InsertRowFormatAsBefore(workbookInfo.Workbook, indexRowAbsence);
                foreach (PointageMensuelPersonEnt.Absences absence in typeAbsence)
                {
                    workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRowAbsence, IndexColOrigin, absence.Libelle);

                    var listJour = absence.GetJours();

                    foreach (var jour in listJour)
                    {
                        if (jour.Item2 != string.Empty)
                        {
                            workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRowAbsence, IndexColOrigin + jour.Item1, jour.Item2);
                        }
                    }
                }
                if (ameliorationsActivated)
                {
                    AddSumFormulaAndSetStyleToControlePointageExport(workbookInfo, indexRowAbsence, indexColEnd, lastColumnIndex);
                }
                else
                {
                    string cellBegin = workbookInfo.ExcelFormat.GetCellAdress(workbookInfo.Workbook, indexRowAbsence, IndexColOrigin);
                    string cellEnd = workbookInfo.ExcelFormat.GetCellAdress(workbookInfo.Workbook, indexRowAbsence, indexColEnd);
                    string cellTotal = workbookInfo.ExcelFormat.GetCellAdress(workbookInfo.Workbook, indexRowAbsence, lastColumnIndex);
                    string range = cellBegin + ":" + cellTotal;
                    string formula = string.Concat("SUM(", cellBegin, ":", cellEnd, ")");
                    workbookInfo.ExcelFormat.SetFormula(workbookInfo.Workbook, indexRowAbsence, lastColumnIndex, formula);
                    workbookInfo.ExcelFormat.ChangeBorderStyle(workbookInfo.Workbook, range, ExcelBordersIndex.EdgeBottom, ExcelLineStyle.Dashed);
                    workbookInfo.ExcelFormat.ChangeColor(workbookInfo.Workbook, range, Color.White);
                    workbookInfo.ExcelFormat.ChangeBorderStyle(workbookInfo.Workbook, cellTotal, ExcelBordersIndex.EdgeLeft, ExcelLineStyle.Thin);
                }
                indexRowAbsence += 1;
                indexRow += 1;
            }
        }

        private void HandlePrimes(WorkbookInfo workbookInfo, List<PointageMensuelPersonEnt.Primes> listPrimes, bool ameliorationsActivated, int lastColumnIndex)
        {
            // Ajout des primes
            foreach (PointageMensuelPersonEnt.Primes prime in listPrimes)
            {
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin, prime.Libelle);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 1, prime.Jour1);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 2, prime.Jour2);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 3, prime.Jour3);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 4, prime.Jour4);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 5, prime.Jour5);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 6, prime.Jour6);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 7, prime.Jour7);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 8, prime.Jour8);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 9, prime.Jour9);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 10, prime.Jour10);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 11, prime.Jour11);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 12, prime.Jour12);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 13, prime.Jour13);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 14, prime.Jour14);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 15, prime.Jour15);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 16, prime.Jour16);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 17, prime.Jour17);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 18, prime.Jour18);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 19, prime.Jour19);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 20, prime.Jour20);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 21, prime.Jour21);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 22, prime.Jour22);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 23, prime.Jour23);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 24, prime.Jour24);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 25, prime.Jour25);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 26, prime.Jour26);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 27, prime.Jour27);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 28, prime.Jour28);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 29, prime.Jour29);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 30, prime.Jour30);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 31, prime.Jour31);
                if (ameliorationsActivated)
                {
                    AddSumFormulaAndSetStyleToControlePointageExport(workbookInfo, indexRow, indexColEnd, lastColumnIndex);
                }
                else
                {
                    string cellBegin = workbookInfo.ExcelFormat.GetCellAdress(workbookInfo.Workbook, indexRow, IndexColOrigin);
                    string cellTotal = workbookInfo.ExcelFormat.GetCellAdress(workbookInfo.Workbook, indexRow, lastColumnIndex);
                    string range = cellBegin + ":" + cellTotal;
                    workbookInfo.ExcelFormat.ChangeBorderStyle(workbookInfo.Workbook, range, ExcelBordersIndex.EdgeBottom, ExcelLineStyle.Dashed);
                    workbookInfo.ExcelFormat.ChangeBorderStyle(workbookInfo.Workbook, cellTotal, ExcelBordersIndex.EdgeLeft, ExcelLineStyle.Thin);
                }
                indexRow += 1;
            }
        }

        private void HandleDeplacements(WorkbookInfo workbookInfo, List<PointageMensuelPersonEnt.Deplacements> listDeplacements, bool ameliorationsActivated, int lastColumnIndex)
        {
            // Ajout des déplacements
            foreach (PointageMensuelPersonEnt.Deplacements deplacement in listDeplacements)
            {
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin, deplacement.Libelle);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 1, deplacement.Jour1);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 2, deplacement.Jour2);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 3, deplacement.Jour3);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 4, deplacement.Jour4);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 5, deplacement.Jour5);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 6, deplacement.Jour6);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 7, deplacement.Jour7);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 8, deplacement.Jour8);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 9, deplacement.Jour9);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 10, deplacement.Jour10);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 11, deplacement.Jour11);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 12, deplacement.Jour12);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 13, deplacement.Jour13);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 14, deplacement.Jour14);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 15, deplacement.Jour15);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 16, deplacement.Jour16);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 17, deplacement.Jour17);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 18, deplacement.Jour18);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 19, deplacement.Jour19);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 20, deplacement.Jour20);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 21, deplacement.Jour21);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 22, deplacement.Jour22);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 23, deplacement.Jour23);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 24, deplacement.Jour24);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 25, deplacement.Jour25);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 26, deplacement.Jour26);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 27, deplacement.Jour27);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 28, deplacement.Jour28);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 29, deplacement.Jour29);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 30, deplacement.Jour30);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 31, deplacement.Jour31);
                if (ameliorationsActivated)
                {
                    AddSumFormulaAndSetStyleToControlePointageExport(workbookInfo, indexRow, indexColEnd, lastColumnIndex);
                }
                else
                {
                    string cellBegin = workbookInfo.ExcelFormat.GetCellAdress(workbookInfo.Workbook, indexRow, IndexColOrigin);
                    string cellTotal = workbookInfo.ExcelFormat.GetCellAdress(workbookInfo.Workbook, indexRow, lastColumnIndex);
                    string range = cellBegin + ":" + cellTotal;
                    workbookInfo.ExcelFormat.ChangeBorderStyle(workbookInfo.Workbook, range, ExcelBordersIndex.EdgeBottom, ExcelLineStyle.Dashed);
                    workbookInfo.ExcelFormat.ChangeBorderStyle(workbookInfo.Workbook, cellTotal, ExcelBordersIndex.EdgeLeft, ExcelLineStyle.Thin);
                }
                indexRow += 1;
            }
        }

        private void HandleIVD(WorkbookInfo workbookInfo, PointageMensuelPersonEnt.Ivd listIVD, bool ameliorationsActivated, int lastColumnIndex)
        {
            if (listIVD != null)
            {
                var ivd = listIVD;
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin, ivd.Libelle);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 1, ivd.Jour1);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 2, ivd.Jour2);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 3, ivd.Jour3);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 4, ivd.Jour4);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 5, ivd.Jour5);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 6, ivd.Jour6);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 7, ivd.Jour7);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 8, ivd.Jour8);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 9, ivd.Jour9);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 10, ivd.Jour10);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 11, ivd.Jour11);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 12, ivd.Jour12);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 13, ivd.Jour13);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 14, ivd.Jour14);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 15, ivd.Jour15);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 16, ivd.Jour16);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 17, ivd.Jour17);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 18, ivd.Jour18);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 19, ivd.Jour19);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 20, ivd.Jour20);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 21, ivd.Jour21);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 22, ivd.Jour22);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 23, ivd.Jour23);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 24, ivd.Jour24);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 25, ivd.Jour25);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 26, ivd.Jour26);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 27, ivd.Jour27);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 28, ivd.Jour28);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 29, ivd.Jour29);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 30, ivd.Jour30);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 31, ivd.Jour31);
                if (ameliorationsActivated)
                {
                    AddSumFormulaAndSetStyleToControlePointageExport(workbookInfo, indexRow, indexColEnd, lastColumnIndex);
                }
                else
                {
                    string cellBeginIVD = workbookInfo.ExcelFormat.GetCellAdress(workbookInfo.Workbook, indexRow, IndexColOrigin);
                    string cellTotalIVD = workbookInfo.ExcelFormat.GetCellAdress(workbookInfo.Workbook, indexRow, lastColumnIndex);
                    string rangeIVD = cellBeginIVD + ":" + cellTotalIVD;
                    workbookInfo.ExcelFormat.ChangeBorderStyle(workbookInfo.Workbook, rangeIVD, ExcelBordersIndex.EdgeBottom, ExcelLineStyle.Dashed);
                    workbookInfo.ExcelFormat.ChangeBorderStyle(workbookInfo.Workbook, cellTotalIVD, ExcelBordersIndex.EdgeLeft, ExcelLineStyle.Thin);
                }
                indexRow += 1;
            }
        }

        private void HandleZonesDeplacement(WorkbookInfo workbookInfo, PointageMensuelPersonEnt.CodeZoneDeplacements listCodeZoneDeplacements, int lastColumnIndex)
        {
            // Ajout des codes zone déplacement
            if (listCodeZoneDeplacements != null)
            {
                var codeZoneDeplacement = listCodeZoneDeplacements;
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin, codeZoneDeplacement.Libelle);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 1, codeZoneDeplacement.Jour1);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 2, codeZoneDeplacement.Jour2);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 3, codeZoneDeplacement.Jour3);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 4, codeZoneDeplacement.Jour4);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 5, codeZoneDeplacement.Jour5);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 6, codeZoneDeplacement.Jour6);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 7, codeZoneDeplacement.Jour7);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 8, codeZoneDeplacement.Jour8);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 9, codeZoneDeplacement.Jour9);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 10, codeZoneDeplacement.Jour10);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 11, codeZoneDeplacement.Jour11);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 12, codeZoneDeplacement.Jour12);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 13, codeZoneDeplacement.Jour13);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 14, codeZoneDeplacement.Jour14);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 15, codeZoneDeplacement.Jour15);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 16, codeZoneDeplacement.Jour16);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 17, codeZoneDeplacement.Jour17);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 18, codeZoneDeplacement.Jour18);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 19, codeZoneDeplacement.Jour19);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 20, codeZoneDeplacement.Jour20);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 21, codeZoneDeplacement.Jour21);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 22, codeZoneDeplacement.Jour22);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 23, codeZoneDeplacement.Jour23);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 24, codeZoneDeplacement.Jour24);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 25, codeZoneDeplacement.Jour25);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 26, codeZoneDeplacement.Jour26);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 27, codeZoneDeplacement.Jour27);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 28, codeZoneDeplacement.Jour28);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 29, codeZoneDeplacement.Jour29);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 30, codeZoneDeplacement.Jour30);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, IndexColOrigin + 31, codeZoneDeplacement.Jour31);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, indexRow, lastColumnIndex, codeZoneDeplacement.Total);

                string cellBegin = workbookInfo.ExcelFormat.GetCellAdress(workbookInfo.Workbook, indexRow, IndexColOrigin);
                string cellTotal = workbookInfo.ExcelFormat.GetCellAdress(workbookInfo.Workbook, indexRow, lastColumnIndex);
                string range = cellBegin + ":" + cellTotal;
                workbookInfo.ExcelFormat.ChangeBorderStyle(workbookInfo.Workbook, range, ExcelBordersIndex.EdgeBottom, ExcelLineStyle.Dashed);
                workbookInfo.ExcelFormat.ChangeBorderStyle(workbookInfo.Workbook, cellTotal, ExcelBordersIndex.EdgeLeft, ExcelLineStyle.Thin);
            }
        }

        private void AddSumFormulaAndSetStyleToControlePointageExport(WorkbookInfo workbookInfo, int rowIndex, int lastDayColIndex, int totalColIndex)
        {
            // La formule
            var firstDayCellAdress = workbookInfo.ExcelFormat.GetCellAdress(workbookInfo.Workbook, rowIndex, FirstDayColIndex);
            var lastDayCellAdress = workbookInfo.ExcelFormat.GetCellAdress(workbookInfo.Workbook, rowIndex , lastDayColIndex);
            var formula = $"COUNTIF({firstDayCellAdress}: {lastDayCellAdress}; \"X\") + SUM({firstDayCellAdress}: {lastDayCellAdress})";
            workbookInfo.ExcelFormat.SetFormula(workbookInfo.Workbook, rowIndex, totalColIndex, formula);

            // Le style du tableau
            var arrayFirstCellAdress = workbookInfo.ExcelFormat.GetCellAdress(workbookInfo.Workbook, rowIndex, ArrayFirstColIndex);
            var arrayLastCellAdress = workbookInfo.ExcelFormat.GetCellAdress(workbookInfo.Workbook, rowIndex, totalColIndex);
            var totalCellAdress = workbookInfo.ExcelFormat.GetCellAdress(workbookInfo.Workbook, rowIndex, totalColIndex);
            var range = arrayFirstCellAdress + ":" + arrayLastCellAdress;
            workbookInfo.ExcelFormat.ChangeBorderStyle(workbookInfo.Workbook, range, ExcelBordersIndex.EdgeBottom, ExcelLineStyle.Dashed);
            workbookInfo.ExcelFormat.ChangeBorderStyle(workbookInfo.Workbook, totalCellAdress, ExcelBordersIndex.EdgeLeft, ExcelLineStyle.Thin);
        }

        /// <summary>
        /// Met en forme les jours dans une feuille excel sous la forme "1 mer, 2 jeu, ..."
        /// Supprime les colonnes dont le jour n'appartient pas au mois (par ex le 30 février)
        /// </summary>
        /// <param name="workbook">Le classeur Excel concerné.</param>
        /// <param name="worksheetIndex">L'index de la feuille concernée dans le classeur.</param>
        /// <param name="annee">L'année concernée.</param>
        /// <param name="mois">Le mois concerné.</param>
        /// <param name="excelFormat">Le générateur de documents Excel.</param>
        /// <param name="firstDayRow">La ligne de la cellule du 1er jour.</param>
        /// <param name="firstDayCol">La colonne de la cellule du 1er jour.</param>
        /// <returns>Retourne l'index de la dernière colonne</returns>
        private int SetDaysInWorksheet(WorkbookInfo workbookInfo, int worksheetIndex, int annee, int mois, int firstDayRow, int firstDayCol)
        {
            var dayInMonth = DateTime.DaysInMonth(annee, mois);
            var ci = CultureInfo.CreateSpecificCulture("fr-FR");
            var dtfi = ci.DateTimeFormat;
            for (int i = 1; i <= dayInMonth; i++)
            {
                DateTime iDay = new DateTime(annee, mois, i);
                var dayName = i + "\n" + dtfi.AbbreviatedDayNames[(int)iDay.DayOfWeek].Replace(".", string.Empty);
                workbookInfo.ExcelFormat.SetCellValue(workbookInfo.Workbook, firstDayRow, firstDayCol + i, dayName);
            }
            var worksheet = workbookInfo.Workbook.Worksheets[worksheetIndex];
            var lastDayInMonthIndex = firstDayCol + dayInMonth;
            worksheet.DeleteColumn(lastDayInMonthIndex + 1, 31 - dayInMonth);
            return lastDayInMonthIndex + 1;
        }

        /// <summary>
        /// Retourne le chemin du fichier à utiliser pour la génération de l'édition du contrôle des pointages.
        /// </summary>
        /// <returns>Le chemin du fichier à utiliser.</returns>
        private string GetControlePointagesFileName()
        {
            if (featureFlippingManager.IsActivated(EnumFeatureFlipping.EditionsPaieAmeliorations))
            {
                return "TemplatePointageMensuelV2.xlsx";
            }
            else
            {
                return "TemplatePointageMensuel.xlsx";
            }
        }
    }
}
