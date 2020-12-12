using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Fred.Business.Journal;
using Fred.Business.Referential.Nature;
using Fred.DataAccess.Interfaces;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Referential;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.Framework.Reporting;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.Journal;
using Fred.Web.Shared.Models.OperationDiverse.ExportExcel;
using Syncfusion.XlsIO;

namespace Fred.Business.OperationDiverse
{
    public class FamilleOperationDiverseExportExcelManager : Manager<FamilleOperationDiverseEnt, IFamilleOperationDiverseRepository>, IFamilleOperationDiverseExportExcelManager
    {
        private readonly INatureManager natureManager;
        private readonly IJournalManager journalManager;
        private readonly Color colorRowForJournal = Color.FromArgb(230, 242, 222);
        private readonly Color colorRowForFamille = Color.FromArgb(239, 245, 251);
        private const string ExcelTemplateFamillesJournal = "Templates/FamilleOperationDiverse/TemplateFamilleODFamillesJournal.xlsx";
        private const string ExcelTemplateJournalFamilles = "Templates/FamilleOperationDiverse/TemplateFamilleODJournalFamilles.xlsx";
        private const string Journal = "Journal";
        private const string Nature = "Nature";

        public FamilleOperationDiverseExportExcelManager(
            IUnitOfWork uow,
            IFamilleOperationDiverseRepository familleOperationDiverseRepository,
            INatureManager natureManager,
            IJournalManager journalManager)
            : base(uow, familleOperationDiverseRepository)
        {
            this.natureManager = natureManager;
            this.journalManager = journalManager;
        }

        /// <summary>
        /// Génère le byte[] pour l'export Excel journal
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="typeDonnee">Permet de savoir quel type de données on récupère</param>
        /// <returns>le byte[] pour l'export Excel journal</returns>
        public byte[] GetExportExcelForJournal(int societeId, string typeDonnee)
        {
            try
            {
                string pathName = AppDomain.CurrentDomain.BaseDirectory;
                ExcelFormat excelFormat = new ExcelFormat();
                Action<IWorkbook> customTransformation = null;

                if (typeDonnee == Journal)
                {
                    pathName += ExcelTemplateJournalFamilles;
                    customTransformation = CustomTransformationJournal;
                }
                else
                {
                    pathName += ExcelTemplateFamillesJournal;
                    customTransformation = CustomTransformationDefault;
                }

                IReadOnlyList<FamilleOperationDiverseExportModel> dataForExportList = PrepareDataForExportJournal(societeId, typeDonnee);
                return excelFormat.GenerateExcel(pathName, dataForExportList, customTransformation);
            }
            catch (Exception ex)
            {
                throw new FredBusinessException(FeatureExportExcel.Invalid_FileFormat, ex);
            }
        }

        /// <summary>
        /// Prépare les données des familles OD et journaux pour l'export.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="typeDonnee">Permet de savoir quel type de données on récupère</param>
        /// <returns>Les données des familles OD et journaux pour l'export</returns>
        private IReadOnlyList<FamilleOperationDiverseExportModel> PrepareDataForExportJournal(int societeId, string typeDonnee)
        {
            List<FamilleOperationDiverseExportModel> familleOperationDiverseExportModelList = new List<FamilleOperationDiverseExportModel>();
            IEnumerable<FamilleOperationDiverseEnt> allFamily = GetFamiliesBySociety(societeId);

            if (typeDonnee == Journal)
            {
                /*** Journal > Famille ***/
                familleOperationDiverseExportModelList.AddRange(GetJournalAndFamily(societeId, allFamily));
            }
            else
            {
                /*** Famille > Journal ***/
                familleOperationDiverseExportModelList.AddRange(GetFamilyAndJournal(societeId, allFamily));
            }

            return familleOperationDiverseExportModelList;
        }

        /// <summary>
        /// Récupère la liste des familles d'OD pour une société
        /// </summary>
        /// <param name="societeId">Identifiant de la société à laquelle les familles sont rattachées</param>
        /// <returns>Liste des familles d'OD de la société</returns>
        /// <remarks>A des fins d'optimisation, la liste est stockée en cache</remarks>
        private IEnumerable<FamilleOperationDiverseEnt> GetFamiliesBySociety(int societeId)
        {
            return Repository.GetFamilyBySociety(societeId).ToList();
        }

        /// <summary>
        /// Retourne la liste des journaux et leurs familles
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="allFamily">Liste des familles</param>
        /// <returns>La liste des journaux et leurs familles</returns>
        private List<FamilleOperationDiverseExportModel> GetJournalAndFamily(int societeId, IEnumerable<FamilleOperationDiverseEnt> allFamily)
        {
            List<FamilleOperationDiverseExportModel> journalAndFamily = new List<FamilleOperationDiverseExportModel>();
            IEnumerable<JournalFamilleODModel> allJournaux = journalManager.GetJournauxActifs(societeId).Distinct();
            foreach (JournalFamilleODModel journal in allJournaux)
            {
                if (journal.ParentFamilyODWithOrder == 0 && journal.ParentFamilyODWithoutOrder == 0)
                {
                    journalAndFamily.Add(GetFamilyOfJournal(journal, allFamily, journal.ParentFamilyODWithoutOrder));
                }
                else
                {
                    journalAndFamily.Add(GetFamilyOfJournal(journal, allFamily, journal.ParentFamilyODWithOrder));
                    journalAndFamily.Add(GetFamilyOfJournal(journal, allFamily, journal.ParentFamilyODWithoutOrder));
                }
            }

            return journalAndFamily;
        }

        private FamilleOperationDiverseExportModel GetFamilyOfJournal(JournalFamilleODModel journal, IEnumerable<FamilleOperationDiverseEnt> allFamily, int parentFamilyOdOrder)
        {
            FamilleOperationDiverseEnt familleAssociatedWithJournal = null;
            if (parentFamilyOdOrder != 0)
            {
                familleAssociatedWithJournal = FamilleAssociatedWithJournal(allFamily, parentFamilyOdOrder);
            }

            return CreateDataJournalAndFamilleForExport(journal, familleAssociatedWithJournal);
        }

        private FamilleOperationDiverseExportModel CreateDataJournalAndFamilleForExport(JournalFamilleODModel journal, FamilleOperationDiverseEnt familleAssociatedWithJournal)
        {
            return new FamilleOperationDiverseExportModel()
            {
                CodeJournal = journal.Code,
                LibelleJournal = journal.Libelle,
                CodeFamille = familleAssociatedWithJournal != null ? familleAssociatedWithJournal.Code : string.Empty,
                LibelleFamille = familleAssociatedWithJournal != null ? familleAssociatedWithJournal.Libelle : string.Empty,
                IsNumCommandeSaveInCompta = familleAssociatedWithJournal != null ? IsNumCommandeSaveInCompta(journal, familleAssociatedWithJournal.FamilleOperationDiverseId) : false
            };
        }

        /// <summary>
        /// Retourne la famille associée au journal
        /// </summary>
        /// <param name="allFamily">Liste des familles</param>
        /// <param name="parentFamilyODOrder">Identifiant de la famille</param>
        /// <returns>La famille associée au journal</returns>
        private static FamilleOperationDiverseEnt FamilleAssociatedWithJournal(IEnumerable<FamilleOperationDiverseEnt> allFamily, int parentFamilyODOrder)
        {
            return allFamily.FirstOrDefault(famille => famille.FamilleOperationDiverseId == parentFamilyODOrder);
        }

        /// <summary>
        /// Retourne la liste des familles et leurs journaux
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="allFamily">Liste des familles</param>
        /// <returns>La liste des familles et leurs journaux</returns>
        private List<FamilleOperationDiverseExportModel> GetFamilyAndJournal(int societeId, IEnumerable<FamilleOperationDiverseEnt> allFamily)
        {
            List<JournalFamilleODModel> journalList = journalManager.GetJournauxActifs(societeId);
            List<FamilleOperationDiverseExportModel> familyAndJournal = new List<FamilleOperationDiverseExportModel>();

            // Liste des famille avec journal
            familyAndJournal.AddRange(GetListFamilyWithJournal(allFamily, journalList));

            // Liste des familles sans journal
            familyAndJournal.AddRange(GetListFamilyWithoutJournal(allFamily, journalList));

            return familyAndJournal;
        }

        /// <summary>
        /// Liste des familles n'ayant pas de journal
        /// </summary>
        /// <param name="allFamily">Liste des familles</param>
        /// <param name="journalList">Liste des journaux</param>
        /// <returns>La liste des familles n'ayant pas de journal</returns>
        private List<FamilleOperationDiverseExportModel> GetListFamilyWithoutJournal(IEnumerable<FamilleOperationDiverseEnt> allFamily, List<JournalFamilleODModel> journalList)
        {
            List<FamilleOperationDiverseExportModel> familyWithoutJournal = new List<FamilleOperationDiverseExportModel>();
            IEnumerable<int> idFamilyOfJournal = journalList.Select(w => w.ParentFamilyODWithOrder).Union(journalList.Select(o => o.ParentFamilyODWithoutOrder)).Distinct();
            IEnumerable<FamilleOperationDiverseEnt> familiesWithoutJournalList = allFamily.Where(q => !idFamilyOfJournal.Contains(q.FamilleOperationDiverseId)).ToList();

            familiesWithoutJournalList.ForEach(x => familyWithoutJournal.Add(new FamilleOperationDiverseExportModel()
            {
                CodeJournal = string.Empty,
                LibelleJournal = string.Empty,
                CodeFamille = x.Code,
                LibelleFamille = x.Libelle,
                IsNumCommandeSaveInCompta = false
            }));

            return familyWithoutJournal;
        }

        /// <summary>
        /// Liste des familles ayant au moins un journal
        /// </summary>
        /// <param name="allFamily">Liste des familles</param>
        /// <param name="journalList">Liste des journaux</param>
        /// <returns>La liste des familles ayant au moins un journal</returns>
        private List<FamilleOperationDiverseExportModel> GetListFamilyWithJournal(IEnumerable<FamilleOperationDiverseEnt> allFamily, List<JournalFamilleODModel> journalList)
        {
            List<FamilleOperationDiverseExportModel> familyAndJournal = new List<FamilleOperationDiverseExportModel>();
            IEnumerable<int> idFamilyOfJournal = journalList.Select(w => w.ParentFamilyODWithOrder).Union(journalList.Select(o => o.ParentFamilyODWithoutOrder)).Distinct();
            List<FamilleOperationDiverseEnt> familyODWithOrderWithJournal = allFamily.Where(q => idFamilyOfJournal.Contains(q.FamilleOperationDiverseId)).ToList();
            List<JournalFamilleODModel> journalWithFamille = new List<JournalFamilleODModel>(journalList.Where(x => x.ParentFamilyODWithOrder != 0 || x.ParentFamilyODWithoutOrder != 0));

            foreach (FamilleOperationDiverseEnt familleOperationDiverse in familyODWithOrderWithJournal)
            {
                foreach (JournalFamilleODModel journalFamilleOD in journalWithFamille)
                {
                    string libelleJournal = string.Empty;
                    string codeJournal = string.Empty;
                    bool isNumCommandeSaveInCompta = false;

                    if (journalFamilleOD.ParentFamilyODWithOrder == familleOperationDiverse.FamilleOperationDiverseId
                        || journalFamilleOD.ParentFamilyODWithoutOrder == familleOperationDiverse.FamilleOperationDiverseId)
                    {
                        libelleJournal = journalFamilleOD.Libelle;
                        codeJournal = journalFamilleOD.Code;
                        isNumCommandeSaveInCompta = IsNumCommandeSaveInCompta(journalFamilleOD, familleOperationDiverse.FamilleOperationDiverseId);

                        familyAndJournal.Add(new FamilleOperationDiverseExportModel()
                        {
                            CodeJournal = codeJournal,
                            LibelleJournal = libelleJournal,
                            CodeFamille = familleOperationDiverse.Code,
                            LibelleFamille = familleOperationDiverse.Libelle,
                            IsNumCommandeSaveInCompta = isNumCommandeSaveInCompta
                        });
                    }
                }
            }
            return familyAndJournal;
        }

        /// <summary>
        /// Custom Transformation pour l'export Famille > Nature
        /// </summary>
        /// <param name="workbook">Workbook à transformer</param>
        private void CustomTransformationFamille(IWorkbook workbook)
        {
            IWorksheet sheet = workbook.Worksheets[0];
            sheet.Range[1, 4].Value = Nature;

            ChangeColumnsColorInExcel(colorRowForFamille, colorRowForJournal, workbook, true);
        }

        /// <summary>
        /// Custom Transformation pour l'export Journal > Famille
        /// </summary>
        /// <param name="workbook">Workbook à transformer</param>
        private void CustomTransformationJournal(IWorkbook workbook)
        {
            IWorksheet sheet = workbook.Worksheets[0];
            sheet.Range[1, 1].Value = Journal;

            ChangeColumnsColorInExcel(colorRowForJournal, colorRowForFamille, workbook, false);
        }

        /// <summary>
        /// Custom Transformation pour l'export Nature > Famille
        /// </summary>
        /// <param name="workbook">Workbook à transformer</param>
        private void CustomTransformationNature(IWorkbook workbook)
        {
            IWorksheet sheet = workbook.Worksheets[0];
            sheet.Range[1, 1].Value = Nature;

            ChangeColumnsColorInExcel(colorRowForJournal, colorRowForFamille, workbook, false);
        }

        /// <summary>
        /// Custom Transformation pour l'export Famille > Journal
        /// </summary>
        /// <param name="workbook">Workbook à transformer</param>
        private void CustomTransformationDefault(IWorkbook workbook)
        {
            ChangeColumnsColorInExcel(colorRowForFamille, colorRowForJournal, workbook, true);
        }

        private void ChangeColumnsColorInExcel(Color colorFirstColumn, Color colorSecondColumn, IWorkbook workbook, bool familleInFirstCol)
        {
            int indexColOrigin = 1;
            int indexFirstColEnd = 2;
            int indexSecondColStart = 3;
            int indexSecondColEnd = 5;

            if (familleInFirstCol)
            {
                SetStyle(colorFirstColumn, workbook, indexColOrigin, indexFirstColEnd + 1);
                SetStyle(colorSecondColumn, workbook, indexSecondColStart + 1, indexSecondColEnd);
            }
            else
            {
                SetStyle(colorFirstColumn, workbook, indexColOrigin, indexFirstColEnd);
                SetStyle(colorSecondColumn, workbook, indexSecondColStart, indexSecondColEnd);
            }
        }

        private void SetStyle(Color colorColumn, IWorkbook workbook, int indexColBegin, int indexColEnd)
        {
            ExcelFormat excelFormat = new ExcelFormat();
            int indexRowBegin = 3;
            int indexRowEnd = workbook.ActiveSheet.Rows.Last().LastRow;

            string cellBegin = excelFormat.GetCellAdress(workbook, indexRowBegin, indexColBegin);
            string cellTotal = excelFormat.GetCellAdress(workbook, indexRowEnd, indexColEnd);
            string range = cellBegin + ":" + cellTotal;
            excelFormat.ChangeColor(workbook, range, colorColumn);
            excelFormat.ChangeBorderStyle(workbook, range, ExcelBordersIndex.EdgeRight, ExcelLineStyle.Medium);
        }

        /// <summary>
        /// Génère le byte[] pour l'export Excel nature
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="typeDonnee">Permet de savoir quel type de données on récupère</param>
        /// <returns>le byte[] pour l'export Excel nature</returns>
        public byte[] GetExportExcelForNature(int societeId, string typeDonnee)
        {
            try
            {
                string pathName = AppDomain.CurrentDomain.BaseDirectory;
                ExcelFormat excelFormat = new ExcelFormat();
                Action<IWorkbook> customTransformation = null;

                if (typeDonnee == Nature)
                {
                    pathName += ExcelTemplateJournalFamilles;
                    customTransformation = CustomTransformationNature;
                }
                else
                {
                    pathName += ExcelTemplateFamillesJournal;
                    customTransformation = CustomTransformationFamille;
                }

                IReadOnlyList<FamilleOperationDiverseExportModel> dataForExportList = PrepareDataForExportNature(societeId, typeDonnee);
                return excelFormat.GenerateExcel(pathName, dataForExportList, customTransformation);
            }
            catch (Exception ex)
            {
                throw new FredBusinessException(FeatureExportExcel.Invalid_FileFormat, ex);
            }
        }

        /// <summary>
        /// Prépare les données des familles OD et natures pour l'export.
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <param name="typeDonnee">Permet de savoir quel type de données on récupère</param>
        /// <returns>Les données des familles OD et natures pour l'export</returns>
        private IReadOnlyList<FamilleOperationDiverseExportModel> PrepareDataForExportNature(int societeId, string typeDonnee)
        {
            List<FamilleOperationDiverseExportModel> familleOperationDiverseExportModelList = new List<FamilleOperationDiverseExportModel>();

            if (typeDonnee == Nature)
            {
                /*** Nature > Famille ***/
                familleOperationDiverseExportModelList.AddRange(GetNatureAndFamily(societeId));
            }
            else
            {
                /*** Famille > Nature ***/
                familleOperationDiverseExportModelList.AddRange(GetFamilyAndNature(societeId));
            }

            return familleOperationDiverseExportModelList;
        }

        /// <summary>
        /// Retourne la liste des natures et leurs familles
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>La liste des natures et leurs familles</returns>
        private List<FamilleOperationDiverseExportModel> GetNatureAndFamily(int societeId)
        {
            List<FamilleOperationDiverseExportModel> natureAndFamily = new List<FamilleOperationDiverseExportModel>();
            IEnumerable<FamilleOperationDiverseEnt> allFamily = GetFamiliesBySociety(societeId);
            IEnumerable<NatureEnt> allNatures = natureManager.GetNatureActiveBySocieteId(societeId).Distinct();

            foreach (NatureEnt nature in allNatures)
            {
                FamilleOperationDiverseEnt familleAssociatedWithNature = null;
                if (nature.ParentFamilyODWithOrder != 0 || nature.ParentFamilyODWithoutOrder != 0)
                {
                    familleAssociatedWithNature = FamilleAssociatedWithNature(allFamily, nature);
                }

                natureAndFamily.Add(new FamilleOperationDiverseExportModel()
                {
                    CodeJournal = nature.Code,
                    LibelleJournal = nature.Libelle,
                    CodeFamille = familleAssociatedWithNature != null ? familleAssociatedWithNature.Code : string.Empty,
                    LibelleFamille = familleAssociatedWithNature != null ? familleAssociatedWithNature.Libelle : string.Empty,
                    IsNumCommandeSaveInCompta = familleAssociatedWithNature != null && IsNumCommandeSaveInCompta(nature, familleAssociatedWithNature.FamilleOperationDiverseId)
                });
            }

            return natureAndFamily;
        }

        /// <summary>
        /// Retourne la liste des familles et leurs natures
        /// </summary>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>La liste des familles et leurs natures</returns>
        private List<FamilleOperationDiverseExportModel> GetFamilyAndNature(int societeId)
        {
            List<FamilleOperationDiverseExportModel> familyAndNature = new List<FamilleOperationDiverseExportModel>();
            IEnumerable<FamilleOperationDiverseEnt> allFamily = GetFamiliesBySociety(societeId);
            IEnumerable<NatureEnt> allNatures = natureManager.GetNatureActiveBySocieteId(societeId);

            // Liste des famille avec nature
            familyAndNature.AddRange(GetListFamilyWithNature(allFamily, allNatures));

            // Liste des familles sans nature
            familyAndNature.AddRange(GetListFamilyWithoutNature(allFamily, allNatures));

            return familyAndNature;
        }

        /// <summary>
        /// Retourne la famille associée à la nature
        /// </summary>
        /// <param name="allFamily">Liste des familles</param>
        /// <param name="nature">Nature</param>
        /// <returns>La famille associée à la nature</returns>
        private static FamilleOperationDiverseEnt FamilleAssociatedWithNature(IEnumerable<FamilleOperationDiverseEnt> allFamily, NatureEnt nature)
        {
            return allFamily.FirstOrDefault(a =>
            (nature.ParentFamilyODWithOrder == 0
            && nature.ParentFamilyODWithoutOrder != 0
            && nature.ParentFamilyODWithoutOrder == a.FamilleOperationDiverseId)
            || (nature.ParentFamilyODWithoutOrder == 0
            && nature.ParentFamilyODWithOrder != 0
            && nature.ParentFamilyODWithOrder == a.FamilleOperationDiverseId)
            || nature.ParentFamilyODWithOrder != 0
            && nature.ParentFamilyODWithoutOrder != 0
            && nature.ParentFamilyODWithOrder == a.FamilleOperationDiverseId
            || nature.ParentFamilyODWithoutOrder == a.FamilleOperationDiverseId);
        }

        /// <summary>
        /// Liste des familles ayant au moins une nature
        /// </summary>
        /// <param name="allFamily">Liste des familles</param>
        /// <param name="natureList">Liste des natures</param>
        /// <returns>La liste des familles ayant au moins une nature</returns>
        private List<FamilleOperationDiverseExportModel> GetListFamilyWithNature(IEnumerable<FamilleOperationDiverseEnt> allFamily, IEnumerable<NatureEnt> natureList)
        {
            List<FamilleOperationDiverseExportModel> familyAndNature = new List<FamilleOperationDiverseExportModel>();
            IEnumerable<int> idFamilyOfNature = natureList.Select(w => w.ParentFamilyODWithOrder).Union(natureList.Select(o => o.ParentFamilyODWithoutOrder)).Distinct();
            List<FamilleOperationDiverseEnt> familyODWithOrderWithNature = allFamily.Where(q => idFamilyOfNature.Contains(q.FamilleOperationDiverseId)).ToList();
            List<NatureEnt> natureWithFamille = new List<NatureEnt>(natureList.Where(x => x.ParentFamilyODWithOrder != 0 || x.ParentFamilyODWithoutOrder != 0));

            foreach (FamilleOperationDiverseEnt familleOperationDiverse in familyODWithOrderWithNature)
            {
                foreach (NatureEnt nature in natureWithFamille)
                {
                    string libelleJournal = string.Empty;
                    string codeJournal = string.Empty;
                    bool isNumCommandeSaveInCompta = false;

                    if (nature.ParentFamilyODWithOrder == familleOperationDiverse.FamilleOperationDiverseId
                        || nature.ParentFamilyODWithoutOrder == familleOperationDiverse.FamilleOperationDiverseId)
                    {
                        libelleJournal = nature.Libelle;
                        codeJournal = nature.Code;
                        isNumCommandeSaveInCompta = IsNumCommandeSaveInCompta(nature, familleOperationDiverse.FamilleOperationDiverseId);

                        familyAndNature.Add(new FamilleOperationDiverseExportModel()
                        {
                            CodeJournal = codeJournal,
                            LibelleJournal = libelleJournal,
                            CodeFamille = familleOperationDiverse.Code,
                            LibelleFamille = familleOperationDiverse.Libelle,
                            IsNumCommandeSaveInCompta = isNumCommandeSaveInCompta
                        });
                    }
                }
            }
            return familyAndNature;
        }

        private bool IsNumCommandeSaveInCompta(NatureEnt nature, int familleOperationDiverseId)
        {
            bool isNumCommandeSaveInCompta = false;

            if (nature.ParentFamilyODWithOrder == familleOperationDiverseId)
            {
                isNumCommandeSaveInCompta = nature.ParentFamilyODWithOrder > 0;
            }
            return isNumCommandeSaveInCompta;
        }

        private bool IsNumCommandeSaveInCompta(JournalFamilleODModel journalFamilleOD, int familleOperationDiverseId)
        {
            bool isNumCommandeSaveInCompta = false;

            if (journalFamilleOD.ParentFamilyODWithOrder == familleOperationDiverseId)
            {
                isNumCommandeSaveInCompta = journalFamilleOD.ParentFamilyODWithOrder > 0;
            }
            return isNumCommandeSaveInCompta;
        }

        /// <summary>
        /// Liste des familles n'ayant pas de nature
        /// </summary>
        /// <param name="allFamily">Liste des familles</param>
        /// <param name="natureList">Liste des natures</param>
        /// <returns>La liste des familles n'ayant pas de nature</returns>
        private List<FamilleOperationDiverseExportModel> GetListFamilyWithoutNature(IEnumerable<FamilleOperationDiverseEnt> allFamily, IEnumerable<NatureEnt> natureList)
        {
            List<FamilleOperationDiverseExportModel> familyWithoutNature = new List<FamilleOperationDiverseExportModel>();
            IEnumerable<int> idFamilyOfNature = natureList.Select(w => w.ParentFamilyODWithOrder).Union(natureList.Select(o => o.ParentFamilyODWithoutOrder)).Distinct();
            IEnumerable<FamilleOperationDiverseEnt> familiesWithoutNatureList = allFamily.Where(q => !idFamilyOfNature.Contains(q.FamilleOperationDiverseId)).ToList();

            familiesWithoutNatureList.ForEach(x => familyWithoutNature.Add(new FamilleOperationDiverseExportModel()
            {
                CodeJournal = string.Empty,
                LibelleJournal = string.Empty,
                CodeFamille = x.Code,
                LibelleFamille = x.Libelle,
                IsNumCommandeSaveInCompta = false
            }));

            return familyWithoutNature;
        }
    }
}
