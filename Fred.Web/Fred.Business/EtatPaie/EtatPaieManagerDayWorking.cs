using Fred.Entities;
using Fred.Entities.Rapport;
using Fred.Web.Models.Rapport;
using Fred.Web.Shared.Extentions;
using Fred.Web.Shared.Models.EtatPaie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Fred.Entities.Rapport.PointageMensuelPersonEnt;

namespace Fred.Business.EtatPaie
{
    /// <summary>
    /// Méthodes pour le traitement de la réflection
    /// </summary>
    public class EtatPaieManagerDayWorking
    {
        /// <summary>
        /// CONST du code absence FERIE
        /// </summary>
        private const string CodeFerie = "FERIE";

        /// <summary>
        ///  Total des jours d'absence avec le code FERIE
        /// </summary>
        public int DayWithCodeFerie { get; set; } = 0;

        /// <summary>
        /// Total des jours travaillés du lundi au vendredi sans prendre en compte les jours travaillés pour lesquels un code absence FERIE existe
        /// </summary>
        public int TotalWorkedDayNotFerie { get; set; } = 0;

        /// <summary>
        /// Cette méthode retoune un status du jour  en se basant sur le SummaryMensuelPersoMdel
        /// Un jour travaillé doit avoir un nombre travaillé supérieur à une limite et différent du weekend .
        /// </summary>
        /// <param name="etatPaieExportModel">Export Model</param>
        /// <param name="item">Le model summary mensuel perso model </param>
        /// <param name="dayIndex">Index du jour</param>
        /// <param name="workingHoursThreshold">Un seuil d'heures travaillés qui pour determiner un jour travaillé</param>
        /// <param name="absenceList">La liste qui représente les abscences</param>
        /// <param name="astreinte">La liste qui représente les astreintes</param>
        /// <param name="majoration">La liste qui représente les majorations</param>
        /// <returns>True si l'index d'un jour travaillé . False autrement</returns>
        public Dictionary<DayWorkingStatus, float> GetWorkingDayStatus(
            EtatPaieExportModel etatPaieExportModel,
            SummaryMensuelPersoModel item,
            int dayIndex,
            double workingHoursThreshold,
            List<PointageMensuelPersonEnt.Absences> absenceList,
            Dictionary<int, double> astreinte,
            HeuresMajo majoration,
            string status = null
            )
        {
            if (item == null && astreinte == null && majoration == null)
            {
                return new Dictionary<DayWorkingStatus, float>() { { DayWorkingStatus.NotRegistred, 1 } };
            }

            Dictionary<DayWorkingStatus, float> dicResult = new Dictionary<DayWorkingStatus, float>();
            string dayIndexPropName = $"{Constantes.EtatPaie.Jour}{dayIndex}";

            string workedHoursItem = GetReferenceTypePropValue<string>(item, dayIndexPropName);
            string majoHoursItem = GetReferenceTypePropValue<string>(majoration, dayIndexPropName);

            double astreinteHours = astreinte != null && astreinte.ContainsKey(dayIndex) ? astreinte[dayIndex] : 0;
            double workedHours = double.TryParse(workedHoursItem, out workedHours) ? workedHours : 0;
            double majoHours = double.TryParse(majoHoursItem, out majoHours) ? majoHours : 0;

            DateTime dateByindex = new DateTime(etatPaieExportModel.Annee, etatPaieExportModel.Mois, dayIndex);
            bool? isWeekend = IsWeekEnd(dateByindex);

            double totalHoursWorked = workedHours + majoHours + astreinteHours;
            bool isWorked = IsWorkedDay(dicResult, totalHoursWorked, workingHoursThreshold);

            List<Absences> listAbsenceFerie = absenceList.Where(a => a.Libelle.Contains(CodeFerie)).ToList();
            bool isFerie = ToAbsenceHoursList(listAbsenceFerie, dayIndexPropName).Sum() > 0;
            DayWithCodeFerie += isFerie ? 1 : 0;

            if (!isFerie && !isWeekend.GetValueOrDefault() && isWorked)
            {
                TotalWorkedDayNotFerie++;
            }

            absenceList = absenceList.Where(a => !a.Libelle.Contains(CodeFerie)).ToList();
            IEnumerable<double> absenceHoursList = ToAbsenceHoursList(absenceList, dayIndexPropName, status);
            bool isAbsence = IsAbsenceDay(dicResult, absenceHoursList, workingHoursThreshold, isWeekend);

            //not worked and not absence
            if (!isWorked && !isAbsence)
            {
                dicResult.Add(DayWorkingStatus.NotRegistred, 1);
            }

            return dicResult;
        }


        private bool IsWorkedDay(Dictionary<DayWorkingStatus, float> dicResult, double totalHoursWorked, double workingHoursThreshold)
        {
            bool isWorked = false;
            if (totalHoursWorked > workingHoursThreshold)
            {
                isWorked = true;
                dicResult.Add(DayWorkingStatus.Worked, 1);

            }
            else if (totalHoursWorked.Equals(workingHoursThreshold))
            {
                isWorked = true;
                dicResult.Add(DayWorkingStatus.Worked, 0.5f);
            }
            return isWorked;
        }
        private bool IsAbsenceDay(Dictionary<DayWorkingStatus, float> dicResult, IEnumerable<double> absenceHoursList, double workingHoursThreshold, bool? isWeekend)
        {
            bool isAbsence = false;
            if (absenceHoursList.Sum() > workingHoursThreshold && !isWeekend.GetValueOrDefault())
            {
                isAbsence = true;
                dicResult.Add(DayWorkingStatus.Absence, 1);
            }
            else if (absenceHoursList.Sum().Equals(workingHoursThreshold) && !isWeekend.GetValueOrDefault())
            {
                isAbsence = true;
                dicResult.Add(DayWorkingStatus.Absence, 0.5f);
            }
            return isAbsence;
        }
        /// <summary>
        /// Get la liste des heures d'absence en se basant sur le model : PointageMensuelPersonEnt.Absences
        /// </summary>
        /// <param name="absenceList">Absence list</param>
        /// <param name="dayIndexPropName">Day index item : (Jour i )</param>
        /// <returns>IEnumerable des heures d'absences</returns>
        private IEnumerable<double> ToAbsenceHoursList(List<PointageMensuelPersonEnt.Absences> absenceList, string dayIndexPropName, string status = null)
        {
            if (absenceList.IsNullOrEmpty())
            {
                return new List<double>();
            }
            var hours = absenceList
                .Where(p => !string.IsNullOrEmpty(GetReferenceTypePropValue<string>(p, dayIndexPropName)))
                .Select(value =>
                {
                    double val;
                    bool parsed = double.TryParse(GetReferenceTypePropValue<string>(value, dayIndexPropName), out val);
                    return new { val, parsed };
                }).Where(r => r.parsed)
            .Select(r => r.val);
            if (GetPersonnelStatut(status) == Constantes.PersonnelStatutValue.Cadre)
            {
                return hours.Select(x => x * 7);
            }
            return hours;
        }

        /// <summary>
        /// Détermine si la date corresponds à un weekend : Samedi ou dimanche .
        /// </summary>
        /// <param name="dateTime">DateTime à utiliser</param>
        /// <returns>True si c'est un weekend . False si non .</returns>
        public bool IsWeekEnd(DateTime dateTime)
        {
            return dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Get propriété par son nom  : Version générique 
        /// </summary>
        /// <typeparam name="T">Type générique</typeparam>
        /// <param name="obj">L'object contenant la propriété</param>
        /// <param name="name">Nom de la propriété</param>
        /// <returns>Valeur de la propriété</returns>
        public T GetReferenceTypePropValue<T>(object obj, string name) where T : class
        {
            object retval = GetPropValue(obj, name);
            if (retval == null) { return default(T); }
            return retval as T;
        }

        /// <summary>
        /// Get propriété par son nom .
        /// </summary>
        /// <param name="obj">L'object contenant la propriété</param>
        /// <param name="name">Nom de la propriété</param>
        /// <returns>Valeur de la propriété</returns>
        public object GetPropValue(object obj, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            foreach (string part in name.Split('.'))
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }

        /// <summary>
        /// Get personnel statut by statut id
        /// </summary>
        /// <param name="statutId">Statut identifier</param>
        /// <returns>Statut</returns>
        public static string GetPersonnelStatut(string statutId)
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

        /// <summary>
        /// total des jours ouvrés par mois
        /// </summary>
        /// <param name="date">date du mois</param>
        /// <returns>total des jours ouvrés</returns>
        public int GetWorkDayInMonth(DateTime date)
        {
            DateTime first = new DateTime(date.Year, date.Month, 1);
            int monthNumberOfDays = DateTime.DaysInMonth(first.Year, first.Month);
            int sumDayOpen = 0;
            for (int dayIndex = 0; dayIndex < monthNumberOfDays; dayIndex++)
            {
                DateTime dateNow = first.AddDays(dayIndex);
                if (dateNow.DayOfWeek != DayOfWeek.Saturday && dateNow.DayOfWeek != DayOfWeek.Sunday)
                {
                    sumDayOpen++;
                }
            }
            return sumDayOpen;
        }

        /// <summary>
        /// Class des Astreintes
        /// </summary>
        public class AstreintesParDay
        {
            /// <summary>
            /// Dictionary Day Hours Astreintes
            /// </summary>
            public Dictionary<int, double> DicDayHoursAstreintes { get; set; } = new Dictionary<int, double>();

            /// <summary>
            /// personnel Id
            /// </summary>
            public int PersonnelId { get; set; }
        }

        /// <summary>
        ///  Class des Majoration
        /// </summary>
        public class MajorationParDay
        {
            /// <summary>
            /// list des Heures Majoration
            /// </summary>
            public HeuresMajo ListHeuresMajo { get; set; } = new HeuresMajo();

            /// <summary>
            /// personnel Id
            /// </summary>
            public int PersonnelId { get; set; }


        }
    }


}
