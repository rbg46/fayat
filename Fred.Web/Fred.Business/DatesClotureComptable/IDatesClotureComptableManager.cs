using Fred.Entities.DatesClotureComptable;
using Fred.Web.Shared.Models.DatesClotureComptable;
using System;
using System.Collections.Generic;

namespace Fred.Business.DatesClotureComptable
{
    /// <summary>
    /// Interface des gestionnaires des sociétés
    /// </summary>
    public interface IDatesClotureComptableManager : IManager<DatesClotureComptableEnt>
    {
        /// <summary>
        /// Retourne une liste de DatesClotureComptablesEnt par société et année
        /// </summary>
        /// <param name="ciId">Un CI</param>
        /// <param name="year">Une année</param>
        /// <returns>Renvoi une liste de clotures comptables</returns>
        IEnumerable<DatesClotureComptableEnt> GetCIListDatesClotureComptableByIdAndYear(int ciId, int year);

        /// <summary>
        /// Retourne une liste de DatesClotureComptableEnt par ci postérieur à une période donnée
        /// </summary>
        /// <param name="ciId">Le ci</param>
        /// <param name="periodeDebut">Une periode de début</param>
        /// <returns>Renvoi une liste de clotures comptables</returns>
        IEnumerable<DatesClotureComptableEnt> GetListDatesClotureComptableByCiGreaterThanPeriode(int ciId, int periodeDebut);

        /// <summary>
        /// Retourne une liste de DatesClotureComptableEnt par ci postérieur à une période donnée
        /// </summary>
        /// <param name="ciId">Le ci</param>
        /// <param name="month">Mois de début</param>
        /// <param name="year">Année de début</param>
        /// <returns>Renvoi une liste de clotures comptables</returns>
        IEnumerable<DatesClotureComptableEnt> GetListDatesClotureComptableByCiGreaterThanPeriode(int ciId, int month, int year);

        /// <summary>
        /// Retourne une liste de DatesClotureComptableEnt par société, année et mois
        /// </summary>
        /// <param name="ciId">Un id de la société</param>
        /// <param name="year">Une année</param>
        /// <param name="month">Un mois</param>
        /// <returns>Renvoi une liste de clotures comptables</returns>
        DatesClotureComptableEnt Get(int ciId, int year, int month);

        /// <summary>
        /// Renvoi vrai si aujourd'hui est dans la periode de clôture
        /// </summary>
        /// <param name="ciId">Le CI</param>
        /// <param name="year">L'année laquelle on souhaite faire le test de la cloture comptable</param>
        /// <param name="month">Le mois avec lequel on souhaite faire le test de la cloture comptable</param>
        /// <returns>Renvoi un booléen</returns>
        bool IsTodayInPeriodeCloture(int ciId, int year, int month);

        /// <summary>
        /// Permet de savoir si une periode est cloturée pour un CI.
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="year">year</param>
        /// <param name="month">month</param>
        /// <param name="today">permet de passé la date 'aujourd'hui' qui sera comparée, utile pour les tests unitaires.</param>
        /// <returns>true si la periode est cloturée</returns>
        bool IsPeriodClosed(int ciId, int year, int month, DateTime? today = null);

        /// <summary>
        /// Permet de savoir si un CI a au moins une periode de cloturée dans un intervalle de temps
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="startDate">Date de début</param>
        /// <param name="endDate">Date de fin</param>
        /// <param name="today">permet de passé la date 'aujourd'hui' qui sera comparée, utile pour les tests unitaires.</param>
        /// <returns>true si la periode est cloturée</returns>
        List<DatesClotureComptableEnt> GetClosedDatesClotureComptable(int ciId, DateTime startDate, DateTime endDate, DateTime? today = null);

        /// <summary>
        /// Recupere l'annee et le mois recedant et suivant
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="year">year</param>
        /// <returns>List de DatesClotureComptableEnt</returns>
        IEnumerable<DatesClotureComptableEnt> GetYearAndPreviousNextMonths(int ciId, int year);

        /// <summary>
        /// Ajoute un DatesClotureComptableEnt en base
        /// </summary>
        /// <param name="dcc">Une date clotures comptable</param>
        /// <returns>Renvoi l'id technique</returns>
        DatesClotureComptableEnt CreateDatesClotureComptable(DatesClotureComptableEnt dcc);

        /// <summary>
        /// Ajoute un DatesClotureComptableEnt en base
        /// </summary>
        /// <param name="dcc">Une date clotures comptable</param>
        /// <returns>Renvoi l'id technique</returns>
        DatesClotureComptableEnt ModifyDatesClotureComptable(DatesClotureComptableEnt dcc);

        /// <summary>
        /// GetPreviousCurrentAndNextMonths
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <returns>Une liste de PeriodeClotureEnt </returns>
        IEnumerable<PeriodeClotureEnt> GetPreviousCurrentAndNextMonths(int ciId);

        /// <summary>
        /// Permet d'ajouter des dates de clôture comptable en masse
        /// </summary>
        /// <param name="datesClotureComptables">Liste des dates de clôture comptable</param>
        void BulkInsert(List<DatesClotureComptableEnt> datesClotureComptables);

        /// <summary>
        /// Détermine si le CI est bloqué en réception pour une période donnée
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="year">Année</param>
        /// <param name="month">Mois</param>
        /// <returns>Vrai si le CI est bloqué en réception, sinon faux</returns>
        bool IsBlockedInReception(int ciId, int year, int month);

        /// <summary>
        /// Récupère la période non bloquée en réception pour un CI
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="currentDate">Date courante</param>
        /// <returns>Période non bloquée</returns>
        DateTime GetNextUnblockedInReceptionPeriod(int ciId, DateTime currentDate);

        /// <summary>
        /// Retourne l'ensemble des dates de clôture d'un CI.
        /// </summary>
        /// <param name="ciId">L'identifiant du CI.</param>
        /// <returns>L'ensemble des dates de clôture du CI.</returns>
        IEnumerable<DatesClotureComptableEnt> Get(int ciId);

        /// <summary>
        /// Retourne l'ensemble des dates de clôture pour la synchronisation mobile.
        /// </summary>
        /// <returns>L'ensemble des dates de clôture.</returns>
        IEnumerable<DatesClotureComptableEnt> GetAllSync();

        /// <summary>
        /// Retourne la dernière date de cloture pour un CI
        /// </summary>
        /// <param name="ciId">Identifiant unique d'un CI</param>
        /// <returns>La dernière date de cloture pour un ci</returns>
        DateTime GetLastDateClotureByCiID(int ciId);

        /// <summary>
        /// Renvoi une liste de DatesClotureComptableEnt pour calculer si la date est cloturée
        /// </summary>
        /// <param name="ciIds">Les CiIds</param>
        /// <param name="year">L'année laquelle on souhaite faire le test de la cloture comptable</param>
        /// <param name="month">Le mois avec lequel on souhaite faire le test de la cloture comptable</param>
        /// <returns>Une liste de DatesClotureComptableEnt</returns>
        IEnumerable<DatesClotureComptableEnt> GetDatesClotureComptableForCiIds(List<int> ciIds, int year, int month);

        /// <summary>
        /// Retourne une liste de <see cref="DatesClotureComptableEnt"/> pour une liste de <see cref="DateClotureComptableWithOptionModel"/>
        /// </summary>
        /// <param name="dateClotureComptableWithOptionModels">Liste de <see cref="DateClotureComptableWithOptionModel"/></param>
        /// <returns>Liste de <see cref="DatesClotureComptableEnt"/></returns>
        List<DatesClotureComptableEnt> GetDatesClotureComptableForCiId(List<DateClotureComptableWithOptionModel> dateClotureComptableWithOptionModels);

        /// <summary>
        /// Renvoie un DatesClotureComptableEnt pour calculer si la date est cloturée
        /// </summary>
        /// <param name="ciId"> CiId</param>
        /// <param name="year">L'année laquelle on souhaite faire le test de la cloture comptable</param>
        /// <param name="month">Le mois avec lequel on souhaite faire le test de la cloture comptable</param>
        /// <returns>Une liste de DatesClotureComptableEnt</returns>
        DatesClotureComptableEnt GetDatesClotureComptableForCiId(int ciId, int year, int month);

        /// <summary>
        /// Permet de savoir si une periode est cloturée pour un CI a partir d'une liste de DatesClotureComptableEnt
        /// </summary>
        /// <param name="datesClotureComptables">datesClotureComptables</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="year">year</param>
        /// <param name="month">month</param>   
        /// <returns>true si la periode est cloturée</returns>
        bool IsPeriodClosed(IEnumerable<DatesClotureComptableEnt> datesClotureComptables, int ciId, int year, int month);

        DateTime? GetDernierePeriodeComptableCloturee(int ciId);
    }
}