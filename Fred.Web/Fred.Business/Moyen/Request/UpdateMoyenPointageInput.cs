using System;
using System.Collections.Generic;
using Fred.Business.Moyen.Common;
using Fred.Entities.CI;
using Fred.Entities.Moyen;
using Fred.Entities.Rapport;
using Fred.Framework.DateTimeExtend;
using Fred.Web.Shared.Extentions;

namespace Fred.Business.Moyen
{
    /// <summary>
    /// Cette contient les éléments nécessaires pour initier la génération des pointages des moyens
    /// </summary>
    public class UpdateMoyenPointageInput
    {
        /// <summary>
        /// Update pointage input
        /// </summary>
        public UpdateMoyenPointageInput() { }

        /// <summary>
        /// Date de début de génération
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Date de fin de génération
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Liste des rapports des ci des affectations des moyens
        /// </summary>
        public IEnumerable<RapportEnt> CiAffectationRapportList { get; set; }

        /// <summary>
        /// Dictionnaire qui continent des affectations Ids qui vont étre générés avec leur plage de date correspondante .
        /// Ce dictionnaire a été conçus pour améliorer les performances
        /// </summary>
        public Dictionary<int, IEnumerable<DateTime>> CiAffectationsWorkingDaysDictionnary { get; set; }

        /// <summary>
        /// La liste des ligne de rapports qui corresponds à la génération de la requéte 
        /// </summary>
        public IEnumerable<RapportLigneEnt> AffectationsPersonnelsAndCisRapportLignes { get; set; }

        /// <summary>
        /// La liste des affectations concérnées par la génération
        /// </summary>
        public IEnumerable<AffectationMoyenEnt> AffectationMoyenList { get; set; }

        /// <summary>
        /// La liste des Cis qui conçerne la maintenance et la restitution . ces Ci vont étre identifiées par Code , Societe et EtablissementComptable
        /// </summary>
        public IEnumerable<CIEnt> RestitutionAndMaintenanceCiList { get; set; }

        /// <summary>
        /// La liste des pointages des personnels 
        /// </summary>
        public IEnumerable<PersonnelPointage> PersonnelPointageList { get; set; }

        /// <summary>
        /// Remplier le dictionnaire des affectations des moyens et leur correspondante plage de jour ouvrés
        /// </summary>
        /// <param name="affectationMoyenEnts">La liste des affectations des moyens</param>
        /// <param name="dateTimeExtManager">date time extensions manager</param>
        /// <param name="generationStartDate">Date de début de génération</param>
        /// <param name="generationEndDate">Date de fin de génération</param>
        /// <param name="helper">Moyen pointage helper</param>
        public void FillCiAffectationsWorkingDaysDictionnary(
            IEnumerable<AffectationMoyenEnt> affectationMoyenEnts,
            IDateTimeExtendManager dateTimeExtManager,
            DateTime generationStartDate,
            DateTime generationEndDate,
            MoyenPointageHelper helper)
        {
            if (affectationMoyenEnts.IsNullOrEmpty())
            {
                return;
            }

            if (CiAffectationsWorkingDaysDictionnary == null)
            {
                CiAffectationsWorkingDaysDictionnary = new Dictionary<int, IEnumerable<DateTime>>();
            }

            foreach (AffectationMoyenEnt affectationMoyen in affectationMoyenEnts)
            {
                IEnumerable<DateTime> dateTimes = helper.GetWorkingDays(affectationMoyen, generationStartDate, generationEndDate, dateTimeExtManager);
                CiAffectationsWorkingDaysDictionnary.Add(affectationMoyen.AffectationMoyenId, dateTimes);
            }
        }

    }
}
