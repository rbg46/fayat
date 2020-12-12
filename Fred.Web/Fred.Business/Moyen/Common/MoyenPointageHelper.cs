using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Business.Moyen.Common;
using Fred.Entities;
using Fred.Entities.Moyen;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Framework.DateTimeExtend;

namespace Fred.Business.Moyen
{
    /// <summary>
    /// Classe helper pour le module des moyens qui traite les conditions spéciales
    /// </summary>
    public class MoyenPointageHelper
    {
        /// <summary>
        /// Retounre le predicate à utiliser pour retourner les affectations des moyens à considérer pour la génération entre 2 dates
        /// </summary>
        /// <param name="dateFrom">Date de début de génération</param>
        /// <param name="dateTo">Date de fin de génération</param>
        /// <returns>Expression d'un predicate pour retourner la liste des moyens</returns>
        public Expression<Func<AffectationMoyenEnt, bool>> GetAffectationMoyenDatesPredicate(DateTime? dateFrom, DateTime? dateTo)
        {
            return a =>
                    (
                     // cas où pas de dates renseignées dans le filtre
                     !dateFrom.HasValue && !dateTo.HasValue)

                    // cas où DateTo équivaut à DateFrom renseigné
                    || (dateFrom.HasValue && !dateTo.HasValue && a.DateDebut <= dateFrom.Value && (!a.DateFin.HasValue || dateFrom.Value <= a.DateFin))

                    // cas où DateFrom équivaut à DateTo renseigné
                    || (!dateFrom.HasValue && dateTo.HasValue && a.DateDebut <= dateTo.Value && (!a.DateFin.HasValue || dateTo.Value <= a.DateFin))

                    // cas où 2 dates renseignées dans le filtre
                    || (dateFrom.HasValue && dateTo.HasValue && a.DateDebut <= dateTo.Value && (!a.DateFin.HasValue || dateFrom.Value <= a.DateFin));
        }

        /// <summary>
        /// Predicate pour chercher les types d'affectation éligbles à la génération du pointage matériel
        /// </summary>
        /// <returns>Expression de predicate pour la recherche</returns>
        public Expression<Func<AffectationMoyenEnt, bool>> GetPointageMoyenAffectationTypePredicate()
        {
            List<AffectationMoyenTypeCode> typeListEnum = GetRestitutionAndMaintenanceCodes().ToList();
            typeListEnum.Add(AffectationMoyenTypeCode.Personnel);
            typeListEnum.Add(AffectationMoyenTypeCode.CI);
            IEnumerable<int> typeList = typeListEnum.Select(v => (int)v);

            return a => typeList.Contains(a.AffectationMoyenTypeId);
        }

        /// <summary>
        /// Renvoie la liste des codes qui conçernent la maintenance et la restitution
        /// </summary>
        /// <returns>Liste des codes pour la maintenance et la restitution</returns>
        public IEnumerable<AffectationMoyenTypeCode> GetRestitutionAndMaintenanceCodes()
        {
            IEnumerable<AffectationMoyenTypeCode> codeList = new[]
            {
                AffectationMoyenTypeCode.Depot,
                AffectationMoyenTypeCode.Parking,
                AffectationMoyenTypeCode.Stock,
                AffectationMoyenTypeCode.Reparation,
                AffectationMoyenTypeCode.Entretien,
                AffectationMoyenTypeCode.Controle
            };

            return codeList;
        }

        /// <summary>
        /// La liste des types d'affectation considéré comme affectation pour un Ci
        /// </summary>
        /// <returns>IEnumerable d'AffectationMoyenTypeCode</returns>
        public IEnumerable<AffectationMoyenTypeCode> GetCiAffectationTypeCodeList()
        {
            List<AffectationMoyenTypeCode> typeListEnum = GetRestitutionAndMaintenanceCodes().ToList();
            typeListEnum.Add(AffectationMoyenTypeCode.CI);

            return typeListEnum;
        }

        /// <summary>
        /// Retourne la liste des jours ouvrés entre 2 dates en se basant sur la date de génération et l'affectation du moyen
        /// </summary>
        /// <param name="affectationMoyen">Affectation du moyen</param>
        /// <param name="generationStartDate">Date de début de génération du pointage</param>
        /// <param name="generationEndDate">Date de fin de génération du pointage</param>
        /// <param name="dateTimeExtendManager">Date time extend manager</param>
        /// <returns>Liste des dates des ouvrés entre les 2 dates</returns>
        public IEnumerable<DateTime> GetWorkingDays(
            AffectationMoyenEnt affectationMoyen,
            DateTime generationStartDate,
            DateTime generationEndDate,
            IDateTimeExtendManager dateTimeExtendManager)
        {
            DateTime affectationStartDate = affectationMoyen.DateDebut >= generationStartDate ? affectationMoyen.DateDebut : generationStartDate;
            DateTime affectationEndDate = affectationMoyen.DateFin.HasValue
                ? (affectationMoyen.DateFin.Value <= generationEndDate ? affectationMoyen.DateFin.Value : generationEndDate)
                : generationEndDate;

            IEnumerable<DateTime> dateTimes = dateTimeExtendManager.GetWorkingDays(affectationStartDate.Date, affectationEndDate.Date);
            return dateTimes;
        }



        /// <summary>
        /// Calcul la somme des heures d'astreinte travaillées
        /// </summary>
        /// <param name="rapportLignesAstreintes">Les lignes de rapport astreinte</param>
        /// <returns>La somme des heures des sorties d'astreintes</returns>
        internal double GetAstreinteHours(IEnumerable<RapportLigneAstreinteEnt> rapportLignesAstreintes)
        {
            return rapportLignesAstreintes?.Sum(x => x.DateFinAstreinte.GetHourDifference(x.DateDebutAstreinte)) ?? 0;
        }

        /// <summary>
        /// calcul de l'heure machine : RG-046
        /// </summary>
        /// <param name="personnelPointage">Pointage list du personnel</param>
        /// <param name="ligne">Rapport ligne</param>
        /// <returns>Heure machine qui corresponds au pointage</returns>
        internal double CalculHeureMachine(PersonnelPointage personnelPointage, RapportLigneEnt ligne)
        {
            double result = 0;
            if (ligne == null || personnelPointage == null)
            {
                return result;
            }

            double hoursRegistred = personnelPointage.TotalHeures;
            if (hoursRegistred.Equals(0))
            {
                return result;
            }

            if (ligne.PersonnelId.HasValue)
            {
                double totalHeuresAstreintes = GetAstreinteHours(ligne.ListRapportLigneAstreintes);
                double totalHeuresMajorations = ligne.ListRapportLigneMajorations?.Sum(d => d.HeureMajoration) ?? 0;
                double totalHeuresTaches = ligne.ListRapportLigneTaches?.Sum(b => b.HeureTache) ?? 0;
                double totalHours = totalHeuresAstreintes + totalHeuresMajorations + totalHeuresTaches;
                result = (totalHours / hoursRegistred) * 7;
            }
            else if (ligne.MaterielId.HasValue)
            {
                result = Commun.Constantes.DefaultHourMachine;
            }


            return Math.Round(result, 1);
        }

        /// <summary>
        /// Get materiel groupe filter (Restrictions des chapitres par code groupe)
        /// </summary>
        /// <param name="chapitresIds">chapitres Ids</param>
        /// <returns>Expression Materiel > bool</returns>
        public Expression<Func<MaterielEnt, bool>> GetMaterielGroupeFilter(IEnumerable<int> chapitresIds)
        {
            return i => i.Ressource != null
                            && i.Ressource.SousChapitre != null
                            && i.Ressource.SousChapitre.Chapitre != null
                            && i.Ressource.SousChapitre.Chapitre.Code != null
                            && chapitresIds.Any(f => f == i.Ressource.SousChapitre.Chapitre.ChapitreId);
        }

        /// <summary>
        /// Get sous chapitre groupe filter (Restrictions des chapitres par code groupe)
        /// </summary>
        /// <param name="chapitresIds">chapitres Ids</param>
        /// <returns>Expression Sous Chapitre > bool</returns>
        public Expression<Func<SousChapitreEnt, bool>> GetSousChapitreGroupeFilter(IEnumerable<int> chapitresIds)
        {
            return sc => sc.Chapitre != null
                            && sc.Chapitre.Code != null
                            && chapitresIds.Any(f => f == sc.Chapitre.ChapitreId);
        }

        /// <summary>
        /// Get ressource groupe filter (Restrictions des ressources par code groupe)
        /// </summary>
        /// <param name="chapitresIds">chapitres Ids</param>
        /// <returns>Expression ressource > bool</returns>
        public Expression<Func<RessourceEnt, bool>> GetRessourceGroupeFilter(IEnumerable<int> chapitresIds)
        {
            return rs => rs.SousChapitre != null
                            && rs.SousChapitre.Chapitre != null
                            && rs.SousChapitre.Chapitre.Code != null
                            && chapitresIds.Any(f => f == rs.SousChapitre.Chapitre.ChapitreId);
        }
    }

}
