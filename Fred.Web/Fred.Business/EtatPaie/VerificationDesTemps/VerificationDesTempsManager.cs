using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Fred.Business.EtatPaie.ControlePointage;
using Fred.Business.FeatureFlipping;
using Fred.Business.Images;
using Fred.Business.Organisation;
using Fred.Business.Personnel;
using Fred.Business.Societe;
using Fred.Entities;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Framework.FeatureFlipping;
using Fred.Framework.Models.Reporting;
using Fred.Framework.Reporting;
using Fred.Web.Models.Rapport;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.EtatPaie;
using MoreLinq;
using Syncfusion.XlsIO;

namespace Fred.Business.EtatPaie
{
    /// <summary>
    /// Manager de l'édition Controle des pontages
    /// </summary>
    public class VerificationDesTempsManager : IVerificationDesTempsManager
    {
        private readonly IFeatureFlippingManager featureFlippingManager;
        private readonly IOrganisationManager organisationManager;
        private readonly IPersonnelManager personnelManager;
        private readonly IImageManager imageManager;
        private readonly ISocieteManager societeManager;

        /// <summary>
        /// Permet de gérer les états paie 
        /// </summary>
        /// <param name="societeManager">Manager de société</param>
        /// <param name="featureFlippingManager">Manager de featureFlipping</param>
        /// <param name="imageManager">Manager d'image</param>
        /// <param name="organisationManager">Manager d'organisation</param>
        /// <param name="personnelManager">Manager de personnel</param>
        public VerificationDesTempsManager(ISocieteManager societeManager, IFeatureFlippingManager featureFlippingManager, IOrganisationManager organisationManager, IPersonnelManager personnelManager, IImageManager imageManager)
        {
            this.societeManager = societeManager;
            this.featureFlippingManager = featureFlippingManager;
            this.organisationManager = organisationManager;
            this.personnelManager = personnelManager;
            this.imageManager = imageManager;
        }

        /// <summary>
        /// Génère un pdf pour le controle des pointages
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'export</param>
        /// <param name="listePointageMensuel">Liste des models de pointage mensuel</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <returns>Le fichier généré</returns>
        public MemoryStream GenerateMemoryStreamVerificationDesTemps(EtatPaieExportModel etatPaieExportModel, List<SummaryMensuelPersoModel> listePointageMensuel, int userId, string templateFolderPath)
        {
            string pathName = Path.Combine(templateFolderPath, GetVerificationTempsFileName());
            ExcelFormat excelFormat = new ExcelFormat();

            HandlePointages(etatPaieExportModel, listePointageMensuel);
            if (etatPaieExportModel.Tri)
            {
                listePointageMensuel = listePointageMensuel.OrderBy(x => x.Matricule).ToList();
            }
            else
            {
                listePointageMensuel = listePointageMensuel.OrderBy(x => x.Libelle).ToList();
            }

            string affaireLib = "TOUTES";

            if (etatPaieExportModel.OrganisationId != 0)
            {
                var orga = organisationManager.GetOrganisationById(etatPaieExportModel.OrganisationId);
                if (orga != null)
                {
                    affaireLib = string.Concat(orga.Code, " - ", orga.Libelle);
                }
            }

            // RG_5172_001-5
            if (etatPaieExportModel.Filtre == TypeFiltreEtatPaie.Population && featureFlippingManager.IsActivated(EnumFeatureFlipping.EditionsPaieAmeliorations))
            {
                affaireLib += " (uniquement mes rapports verrouillés)";
            }

            // RG_5172_001-4
            var firstDayRow = 2;
            var workbook = excelFormat.OpenTemplateWorksheet(pathName);
            var lastColumnIndex = SetDaysInWorksheet(workbook, 0, etatPaieExportModel.Annee, etatPaieExportModel.Mois, excelFormat, firstDayRow, 1);

            excelFormat.InitVariables(workbook);
            excelFormat.AddVariable("ListPointage", listePointageMensuel);
            excelFormat.ApplyVariables();
            workbook.Worksheets[0].View = SheetView.Normal;
            var startRow = 3;
            var lastRow = listePointageMensuel.Count + startRow;
            workbook.Worksheets[0].Range["B2:AG" + lastRow].AutofitColumns();

            // Gestion de l'entête
            var editeur = personnelManager.GetPersonnel(userId);
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + imageManager.GetLogoImage(editeur.SocieteId.Value).Path;
            string dateEdition = BusinessResources.Pointage_Interimaire_Export_DateEdition + DateTime.Now.ToShortDateString() + BusinessResources.Pointage_Interimaire_Export__Espace + DateTime.Now.ToShortTimeString();
            string editePar = BusinessResources.Pointage_Interimaire_Export_EditePar + editeur.CodeNomPrenom;
            string periode = BusinessResources.Pointage_Interimaire_Export_Periode + new DateTime(etatPaieExportModel.Annee, etatPaieExportModel.Mois, 1).ToString("MM/yyyy");
            var buildHeaderModel = new BuildHeaderExcelModel(FeatureRapport.EtatPaie_VerificationTemps_Titre, affaireLib, dateEdition, editePar, periode, pathLogo, new IndexHeaderExcelModel(3, 20, lastColumnIndex - 3, lastColumnIndex));
            excelFormat.BuildHeader(workbook.Worksheets[0], buildHeaderModel);

            MemoryStream stream = excelFormat.GeneratePdfOrExcel(etatPaieExportModel.Pdf, workbook);
            excelFormat.Dispose();
            return stream;
        }

        private void HandlePointages(EtatPaieExportModel etatPaieExportModel, List<SummaryMensuelPersoModel> listePointageMensuel)
        {
            //Ajout du personnel sans pointage pour l'édition des pointage FES
            if (etatPaieExportModel.Filtre == TypeFiltreEtatPaie.Autre)
            {
                // Recherche de la societe parente de l'organisation et son groupe
                var societeId = societeManager.GetSocieteByOrganisationId(etatPaieExportModel.OrganisationId).SocieteId;
                var societe = societeManager.GetSocieteById(societeId, true);
                IEnumerable<int> listSummaryMensuelPersoId = listePointageMensuel.Select(p => p.PersonnelId);

                if (societe.Groupe.Code == Constantes.CodeGroupeFES)
                {
                    var personnel = GetPersonnelFilteredBySocieteId(societe.SocieteId, etatPaieExportModel);
                    IEnumerable<PersonnelEnt> personnelNotRegisterdList = personnel?.Where(affec => !listSummaryMensuelPersoId.Contains(affec.PersonnelId) && (!etatPaieExportModel.StatutPersonnelList.Any() || etatPaieExportModel.StatutPersonnelList.Contains(affec.Statut)));

                    List<SummaryMensuelPersoModel> personnelSummaryNotRegistred = personnelNotRegisterdList.Select(i => new SummaryMensuelPersoModel
                    {
                        Libelle = i.NomPrenom,
                        Matricule = i.Matricule,
                        PersonnelId = i.PersonnelId,
                        TotalHeuresTravaillees = default(int).ToString(),
                        PersonnelCodeSociete = featureFlippingManager.IsActivated(EnumFeatureFlipping.EditionsPaieAmeliorations) && i.Societe != null ? i.Societe.Code : string.Empty
                    }).ToList();

                    listePointageMensuel.AddRange(personnelSummaryNotRegistred);
                }
            }
        }

        /// <summary>
        /// Liste du personnel filtré par société et etablissements comptables
        /// </summary>
        /// <param name="societeId">Id de société</param>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>Liste de personnel</returns>
        private IEnumerable<PersonnelEnt> GetPersonnelFilteredBySocieteId(int societeId, EtatPaieExportModel etatPaieExportModel)
        {
            return personnelManager.Get(new List<Expression<Func<PersonnelEnt, bool>>>
                {
                x => x.SocieteId == societeId,
                x => !etatPaieExportModel.EtablissementPaieIdList.Any() || etatPaieExportModel.EtablissementPaieIdList.Contains(x.EtablissementPaieId),
                x => !x.DateSortie.HasValue || (x.DateSortie.Value.Month >= etatPaieExportModel.Mois && x.DateSortie.Value.Year >= etatPaieExportModel.Annee)
                }, null,
                new List<Expression<Func<PersonnelEnt, object>>>
                {
                    x => x.Societe.Organisation,
                    x => x.Ressource,
                    x => x.Manager,
                    x => x.EtablissementPaie,
                    x => x.EtablissementRattachement
                }
            );
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
        private int SetDaysInWorksheet(IWorkbook workbook, int worksheetIndex, int annee, int mois, ExcelFormat excelFormat, int firstDayRow, int firstDayCol)
        {
            var dayInMonth = DateTime.DaysInMonth(annee, mois);
            var ci = CultureInfo.CreateSpecificCulture("fr-FR");
            var dtfi = ci.DateTimeFormat;
            for (int i = 1; i <= dayInMonth; i++)
            {
                DateTime iDay = new DateTime(annee, mois, i);
                var dayName = i + "\n" + dtfi.AbbreviatedDayNames[(int)iDay.DayOfWeek].Replace(".", string.Empty);
                excelFormat.SetCellValue(workbook, firstDayRow, firstDayCol + i, dayName);
            }
            var worksheet = workbook.Worksheets[worksheetIndex];
            var lastDayInMonthIndex = firstDayCol + dayInMonth;
            worksheet.DeleteColumn(lastDayInMonthIndex + 1, 31 - dayInMonth);
            return lastDayInMonthIndex + 1;
        }

        /// <summary>
        /// Retourne le chemin du fichier à utiliser pour la génération de l'édition de la vérification des temps.
        /// </summary>
        /// <returns>Le chemin du fichier à utiliser.</returns>
        private string GetVerificationTempsFileName()
        {

            if (featureFlippingManager.IsActivated(EnumFeatureFlipping.EditionsPaieAmeliorations))
            {
                return "TemplateVerificationTempsV2.xlsx";
            }
            else
            {
                return "TemplateVerificationTemps.xlsx";
            }
        }

    }
}
