using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Fred.Business.EtatPaie.ControlePointage;
using Fred.Business.Images;
using Fred.Business.Params;
using Fred.Business.Personnel;
using Fred.Entities;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.Utilisateur;
using Fred.Framework.Models.Reporting;
using Fred.Framework.Reporting;
using Fred.Web.Models.Rapport;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.EtatPaie;
using Syncfusion.XlsIO;
using static Fred.Business.EtatPaie.EtatPaieManagerDayWorking;

namespace Fred.Business.EtatPaie
{
    /// <summary>
    /// Manager de l'édition Controle des pontages
    /// </summary>
    public class SalarieAcompteManager : ISalarieAcompteManager
    {
        private readonly IPersonnelManager personnelManager;
        private readonly IParamsManager paramsManager;
        private readonly IImageManager imageManager;

        /// <summary>
        /// Permet de gérer les états paie 
        /// </summary>
        /// <param name="paramsManager">Manager des parameters</param>
        /// <param name="personnelManager">Manager de personnel</param>
        public SalarieAcompteManager(IParamsManager paramsManager, IPersonnelManager personnelManager, IImageManager imageManager)
        {
            this.personnelManager = personnelManager;
            this.paramsManager = paramsManager;
            this.imageManager = imageManager;
        }

        /// <summary>
        /// Génère un pdf pour le controle des pointages
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'export </param>
        /// <param name="mensuelPerson">Liste des modèles de pointage mensuel</param>
        /// <param name="finallist">Liste des modèles résumés de pointage mensuel</param>
        /// <param name="user">Identifiant de l'utilisateur</param>
        /// <returns>Le fichier généré</returns>
        public MemoryStream GenerateMemoryStreamSalarieAcompte(EtatPaieExportModel etatPaieExportModel, List<PointageMensuelPersonEnt> mensuelPerson, List<SummaryMensuelPersoModel> finallist, UtilisateurEnt user, string templateFolderPath)
        {
            ExcelFormat excelFormat = new ExcelFormat();
            List<LigneAbsence> listabsence = mensuelPerson.Select(i => new LigneAbsence()
            {
                HeureAbsence = i.ListAbsences,
                PersonnelId = i.Personnel.PersonnelId
            }).ToList();
            List<MajorationParDay> majorations = mensuelPerson.Select(m => new MajorationParDay
            {
                ListHeuresMajo = m.ListHeuresMajo,
                PersonnelId = m.Personnel.PersonnelId
            }).ToList();

            List<AstreintesParDay> astreintes = DicHoursAstreintesByDay(mensuelPerson);
            int monthNumberOfDays = DateTime.DaysInMonth(etatPaieExportModel.Annee, etatPaieExportModel.Mois);
            List<SalarieLigneAcompte> listsalarie = LoadSalarieAcompte(etatPaieExportModel, finallist, listabsence, monthNumberOfDays, astreintes, majorations) ?? new List<SalarieLigneAcompte>();
            string pathName = Path.Combine(templateFolderPath, "TemplateSituationAcompteV2.xlsx");
            IWorkbook workbook = excelFormat.OpenTemplateWorksheet(pathName);

            excelFormat.InitVariables(workbook);
            excelFormat.AddVariable("Listsalarie", listsalarie);
            excelFormat.ApplyVariables();

            workbook.Worksheets[0].View = SheetView.Normal;
            var startRow = 3;
            var lastRow = listsalarie.Count + startRow;
            workbook.Worksheets[0].Range["B2:H" + lastRow].AutofitColumns();

            var editeur = personnelManager.GetPersonnel(user.UtilisateurId);
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + imageManager.GetLogoImage(editeur.SocieteId.Value).Path;
            string dateEdition = BusinessResources.Pointage_Interimaire_Export_DateEdition + DateTime.Now.ToShortDateString() + BusinessResources.Pointage_Interimaire_Export__Espace + DateTime.Now.ToShortTimeString();
            string editePar = BusinessResources.Pointage_Interimaire_Export_EditePar + editeur.CodeNomPrenom;
            string periode = BusinessResources.Pointage_Interimaire_Export_Periode + new DateTime(etatPaieExportModel.Annee, etatPaieExportModel.Mois, 1).ToString("MM/yyyy");
            var buildHeaderModel = new BuildHeaderExcelModel(FeatureRapport.Rapport_Situation_Acompte_Titre, null, dateEdition, editePar, periode, pathLogo, new IndexHeaderExcelModel(2, 5, 7, 8));
            excelFormat.BuildHeader(workbook.Worksheets[0], buildHeaderModel);
            foreach (var sheet in workbook.Worksheets)
            {
                foreach (var row in sheet.Rows)
                {
                    row.WrapText = true;
                }
            }

            MemoryStream stream = excelFormat.GeneratePdfOrExcel(etatPaieExportModel.Pdf, workbook);
            excelFormat.Dispose();
            return stream;
        }

        private List<AstreintesParDay> DicHoursAstreintesByDay(List<PointageMensuelPersonEnt> pointages)
        {
            List<AstreintesParDay> listastreintesParDay = new List<AstreintesParDay>();
            pointages.ForEach(pointage =>
            {
                AstreintesParDay astreintesParDay = new AstreintesParDay();
                astreintesParDay.PersonnelId = pointage.Personnel.PersonnelId;

                pointage.ListRapportLigneAstreintes.ForEach(astreinte =>
                {
                    DicHoursAstreinteByDay(astreinte, astreintesParDay);
                });
                listastreintesParDay.Add(astreintesParDay);
            });
            return listastreintesParDay;
        }

        private void DicHoursAstreinteByDay(RapportLigneAstreinteEnt astreinte, AstreintesParDay astreintesParDay)
        {
            if (astreinte.DateDebutAstreinte.Day == astreinte.DateFinAstreinte.Day)
            {
                int day = astreinte.DateDebutAstreinte.Day;
                double value = (astreinte.DateFinAstreinte - astreinte.DateDebutAstreinte).TotalHours;

                if (astreintesParDay.DicDayHoursAstreintes.ContainsKey(day))
                {
                    astreintesParDay.DicDayHoursAstreintes[day] += value;
                }
                else
                {
                    astreintesParDay.DicDayHoursAstreintes.Add(day, value);
                }
            }
            else
            {
                int day = astreinte.DateDebutAstreinte.Day;
                DateTime secDate = new DateTime(astreinte.DateDebutAstreinte.Year, astreinte.DateDebutAstreinte.Month, astreinte.DateDebutAstreinte.Day, 23, 59, 59);
                double value = (secDate - astreinte.DateDebutAstreinte).TotalHours;
                if (astreintesParDay.DicDayHoursAstreintes.ContainsKey(day))
                {
                    astreintesParDay.DicDayHoursAstreintes[day] += value;
                }
                else
                {
                    astreintesParDay.DicDayHoursAstreintes.Add(day, value);
                }
                day = astreinte.DateFinAstreinte.Day;
                secDate = new DateTime(astreinte.DateFinAstreinte.Year, astreinte.DateFinAstreinte.Month, astreinte.DateFinAstreinte.Day);
                value = (astreinte.DateFinAstreinte - secDate).TotalHours;
                if (astreintesParDay.DicDayHoursAstreintes.ContainsKey(day))
                {
                    astreintesParDay.DicDayHoursAstreintes[day] += value;
                }
                else
                {
                    astreintesParDay.DicDayHoursAstreintes.Add(day, value);
                }
            }
        }

        /// <summary>
        /// Calcul la situation des salarie acompte
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="finallist">Liste des modèles résumés de pointage mensuel</param>
        /// <param name="listabsence">liste d'heure d'absence avec son personnel</param>
        /// <param name="monthNumberOfDays">le nombre de jours dans le mois utilisé en fonction de l'année</param>
        /// <param name="listastreintes">liste des astreintes</param>
        /// <param name="listmajorations">liste des majorations</param>
        /// <returns>ListSalarie</returns>
        private List<SalarieLigneAcompte> LoadSalarieAcompte(EtatPaieExportModel etatPaieExportModel, List<SummaryMensuelPersoModel> finallist, List<LigneAbsence> listabsence, int monthNumberOfDays, List<AstreintesParDay> listastreintes, List<MajorationParDay> listmajorations)
        {
            List<SalarieLigneAcompte> listsalarie = new List<SalarieLigneAcompte>();
            if (finallist != null)
            {
                foreach (SummaryMensuelPersoModel item in finallist)
                {
                    var sumJourTravail = 0d;
                    var sumJourAbsence = 0d;
                    var sumJourNp = 0d;

                    double workingHoursThreshold = GetWorkingHoursInDayByStatut(item.Personnel.Societe.Organisation.OrganisationId, item.Personnel.Statut) / 2;

                    List<PointageMensuelPersonEnt.Absences> absence = listabsence?.FirstOrDefault(i => i.PersonnelId == item.PersonnelId)?.HeureAbsence ?? new List<PointageMensuelPersonEnt.Absences>();
                    Dictionary<int, double> astreinte = listastreintes?.FirstOrDefault(i => i.PersonnelId == item.PersonnelId).DicDayHoursAstreintes ?? new Dictionary<int, double>();
                    PointageMensuelPersonEnt.HeuresMajo majoration = listmajorations?.FirstOrDefault(i => i.PersonnelId == item.PersonnelId).ListHeuresMajo ?? new PointageMensuelPersonEnt.HeuresMajo();

                    SalarieLigneAcompte salarie = new SalarieLigneAcompte();
                    salarie.Personnel = item.PersonnelLibelle;
                    PersonnelEnt persone = personnelManager.GetPersonnelById(item.PersonnelId);
                    salarie.Statut = GetPersonnelStatut(persone.Statut);
                    salarie.Etablissement = persone.EtablissementPaie?.Libelle;
                    EtatPaieManagerDayWorking etatPaieManagerHelper = new EtatPaieManagerDayWorking();

                    for (int dayIndex = 1; dayIndex <= monthNumberOfDays; dayIndex++)
                    {
                        Dictionary<DayWorkingStatus, float> dayWorkingStatus = etatPaieManagerHelper.GetWorkingDayStatus(
                            etatPaieExportModel,
                            item,
                            dayIndex,
                            workingHoursThreshold,
                            absence,
                            astreinte,
                            majoration,
                            item.Personnel.Statut
                            );
                        foreach (var status in dayWorkingStatus)
                        {
                            switch (status.Key)
                            {
                                case DayWorkingStatus.Worked:
                                    sumJourTravail += status.Value;
                                    break;
                                case DayWorkingStatus.Absence:
                                    sumJourAbsence += status.Value;
                                    break;
                                case DayWorkingStatus.NotRegistred:
                                    sumJourNp += status.Value;
                                    break;
                            }
                        }
                    }
                    int dayToWork = etatPaieManagerHelper.GetWorkDayInMonth(new DateTime(etatPaieExportModel.Annee, etatPaieExportModel.Mois, 1));
                    sumJourNp = (dayToWork - etatPaieManagerHelper.DayWithCodeFerie) - etatPaieManagerHelper.TotalWorkedDayNotFerie - sumJourAbsence;

                    salarie.NbJoursTravailles = sumJourTravail.ToString() + " J ";
                    salarie.NbJoursNonPointe = sumJourNp.ToString() + " J ";
                    salarie.NbJoursAbsence = sumJourAbsence.ToString() + " J ";
                    salarie.NbHeuresTravaille = item.TotalHeuresTravaillees.ToString() + " H ";
                    listsalarie.Add(salarie);
                }
            }
            return listsalarie;
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

        private double GetWorkingHoursInDayByStatut(int organisationId, string statut)
        {
            double workingHours;
            if (GetPersonnelStatut(statut) == Constantes.PersonnelStatutValue.Cadre)
            {
                return double.TryParse(paramsManager.GetParamValue(organisationId, "HeuresJourIAC"), out workingHours) ? workingHours : 7d;
            }
            else
            {
                return double.TryParse(paramsManager.GetParamValue(organisationId, "HeuresJourOuvrierETAM"), out workingHours) ? workingHours : 7d;
            }
        }
    }
}
