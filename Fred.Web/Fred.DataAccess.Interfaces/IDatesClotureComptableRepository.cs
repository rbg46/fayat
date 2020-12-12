
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.Entities.DatesClotureComptable;
namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    ///   Représente un référentiel de données pour les clotures comptables
    /// </summary>
    public interface IDatesClotureComptableRepository : IRepository<DatesClotureComptableEnt>
    {
        /// <summary>
        ///   Retourne une liste de DatesClotureComptable par CI et année
        /// </summary>
        /// <param name="ciId">Le ci</param>
        /// <param name="year">Une année</param>
        /// <returns>Renvoi une liste de clotures comptables</returns>
        IEnumerable<DatesClotureComptableEnt> GetCIListDatesClotureComptableByIdAndYear(int ciId, int year);

        /// <summary>
        ///   Retourne une liste de DatesClotureComptableEnt par ci postérieur à une période donnée
        /// </summary>
        /// <param name="ciId">Le ci</param>
        /// <param name="periodeDebut">Une periode de début</param>
        /// <returns>Renvoi une liste de clotures comptables</returns>
        IEnumerable<DatesClotureComptableEnt> GetListDatesClotureComptableByCiGreaterThanPeriode(int ciId, int periodeDebut);

        /// <summary>
        ///   Retourne une liste de DatesClotureComptableEnt par ci postérieur à une période donnée
        /// </summary>
        /// <param name="ciId">Le ci</param>
        /// <param name="month">Mois de début</param>
        /// <param name="year">Année de début</param>
        /// <returns>Renvoi une liste de clotures comptables</returns>
        IEnumerable<DatesClotureComptableEnt> GetListDatesClotureComptableByCiGreaterThanPeriode(int ciId, int month, int year);

        /// <summary>
        ///   Rerourne une liste de DatesClotureComptableEnt par CI, année et mois
        /// </summary>
        /// <param name="ciId">Le ci</param>
        /// <param name="year">Une année</param>
        /// <param name="month">Un mois</param>
        /// <returns>Renvoi une clotures comptable</returns>
        DatesClotureComptableEnt Get(int ciId, int year, int month);


        /// <summary>
        /// Retourne une liste de date cloture comptable pour une période comprise entre deux date
        /// </summary>
        /// <param name="ciId">Un CI</param>
        /// <param name="startDate">Date de début</param>
        /// <param name="endDate">Date de fin</param>
        /// <returns>Une cloture comptable</returns>
        List<DatesClotureComptableEnt> Get(int ciId, DateTime startDate, DateTime endDate);

        /// <summary>
        ///   Rerourne une liste de DatesClotureComptableEnt pour une liste de CI,une année et un mois
        /// </summary>
        /// <param name="ciIds">liste de Ci</param>
        /// <param name="year">Une année</param>
        /// <param name="month">Un mois</param>
        /// <returns>Renvoi une liste de clotures comptables</returns>
        IEnumerable<DatesClotureComptableEnt> Get(List<int> ciIds, int year, int month);

        /// <summary>
        /// Recupere l'annee et le mois recedant et suivant
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="year">year</param>
        /// <returns>List de DatesClotureComptableEnt</returns>
        IEnumerable<DatesClotureComptableEnt> GetYearAndPreviousNextMonths(int ciId, int year);

        /// <summary>
        ///   Ajoute un DatesClotureComptableEnt en base
        /// </summary>
        /// <param name="dcc">Une date clotures comptable</param>
        /// <param name="currentUserId">currentUserId</param>
        /// <returns>l'entité créée</returns>
        DatesClotureComptableEnt CreateDatesClotureComptable(DatesClotureComptableEnt dcc, int currentUserId);

        /// <summary>
        ///   Ajoute un DatesClotureComptableEnt en base
        /// </summary>
        /// <param name="dcc">Une date clotures comptable</param>
        /// <param name="currentUserId">currentUserId</param>
        /// <returns>l'entité modifié</returns>
        DatesClotureComptableEnt ModifyDatesClotureComptable(DatesClotureComptableEnt dcc, int currentUserId);

        /// <summary>
        ///   Rerourne une liste de DatesClotureComptableEnt par CI, année et mois
        /// </summary>
        /// <param name="ciId">Le ci</param>
        /// <param name="year">Une année</param>
        /// <param name="month">Un mois</param>
        /// <returns>Renvoi une liste de clotures comptables</returns>
        DatesClotureComptableEnt GetCIDatesClotureComptableByIdAndYearAndMonthOrDefault(int ciId, int year, int month);

        /// <summary>
        /// Permet d'ajouter des dates de clôture comptable en masse
        /// </summary>
        /// <param name="datesClotureComptables">Liste des dates de clôture comptable</param>
        void InsertInMass(List<DatesClotureComptableEnt> datesClotureComptables);

        /// <summary>
        /// Retourne l'ensemble des dates de clôture d'un CI.
        /// </summary>
        /// <param name="ciId">L'identifiant du CI.</param>
        /// <returns>L'ensemble des dates de clôture du CI.</returns>
        IEnumerable<DatesClotureComptableEnt> Get(int ciId);

        /// <summary>
        /// Retourne l'ensemble des dates de clôture d'un CI pour la synchronisation mobile.
        /// </summary>
        /// <returns>L'ensemble des dates de clôture du CI.</returns>
        IEnumerable<DatesClotureComptableEnt> GetAllSync();

        /// <summary>
        /// Retourne la dernière date de cloture pour un CI
        /// </summary>
        /// <param name="ciId">Identifiant unique d'un CI</param>
        /// <returns>La dernière date de cloture pour un ci</returns>
        DateTime GetLastDateClotureByCiID(int ciId);

        /// <summary>
        /// Permet d'executer un lot de requette OR avec Ef.       
        /// </summary>
        /// <param name="queries">Les requettes</param>
        /// <param name="batchSize">Permet de faire des bache de requette OR (default 50 OR)</param>
        /// <returns>les DatesClotureComptableEnt</returns>
        List<DatesClotureComptableEnt> ExecuteOrQueries(List<Expression<Func<DatesClotureComptableEnt, bool>>> queries, int batchSize = 50);

        void AddRange(IEnumerable<DatesClotureComptableEnt> datesComptableAndCis);
        List<DatesClotureComptableEnt> GetLastDateClotures(List<int> ciIds);
    }
}
