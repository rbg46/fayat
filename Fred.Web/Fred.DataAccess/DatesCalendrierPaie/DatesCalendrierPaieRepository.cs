using System.Collections.Generic;
using System.Data;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.DatesCalendrierPaie;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.DatesCalendrierPaie
{
    /// <summary>
    ///   Référentiel de données pour les sociétés.
    /// </summary>
    public class DatesCalendrierPaieRepository : FredRepository<DatesCalendrierPaieEnt>, IDatesCalendrierPaieRepository
    {
        private readonly ILogManager logManager;

        public DatesCalendrierPaieRepository(FredDbContext context, ILogManager logManager)
          : base(context)
        {
            this.logManager = logManager;
        }

        /// <summary>
        ///   Retourne une liste de calendriers mensuels par société et année
        /// </summary>
        /// <param name="societeId">Un id de la société</param>
        /// <param name="year">Une année</param>
        /// <returns>La liste des calendriers mensuels</returns>
        public IEnumerable<DatesCalendrierPaieEnt> GetSocieteListDatesCalendrierPaieByIdAndYear(int societeId, int year)
        {
            foreach (DatesCalendrierPaieEnt dcp in Context.DatesCalendrierPaies.Where(o => o.SocieteId == societeId && o.DateFinPointages.HasValue && o.DateFinPointages.Value.Year == year).OrderBy(o => o.DateFinPointages.Value.Year).ThenBy(o => o.DateFinPointages.Value.Month))
            {
                yield return dcp;
            }
        }

        /// <summary>
        ///   Retourne une liste de calendriers mensuels par société, année, mois
        /// </summary>
        /// <param name="societeId">Un id de la société</param>
        /// <param name="year">Une année</param>
        /// <param name="month">Un mois</param>
        /// <returns>Un calendrier mensuel</returns>
        public DatesCalendrierPaieEnt GetSocieteDatesCalendrierPaieByIdAndYearAndMonth(int societeId, int year, int month)
        {
            return GetSocieteListDatesCalendrierPaieByIdAndYear(societeId, year).FirstOrDefault(o => o.DateFinPointages.HasValue && o.DateFinPointages.Value.Month == month);
        }

        /// <summary>
        ///   Ajoute un calendrier mensuel
        /// </summary>
        /// <param name="dcp">Un calendrier mensuel</param>
        /// <returns>Renvoi l'id technique</returns>
        public int AddDatesCalendrierPaie(DatesCalendrierPaieEnt dcp)
        {
            try
            {
                Context.DatesCalendrierPaies.Add(dcp);
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }

            return dcp.DatesCalendrierPaieId;
        }

        /// <summary>
        ///   Met à jour un calendrier mensuel
        /// </summary>
        /// <param name="dcp">Un calendrier mensuel</param>
        public void UpdateDatesCalendrierPaie(DatesCalendrierPaieEnt dcp)
        {
            try
            {
                Context.Entry(dcp).State = EntityState.Modified;
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        ///   Ajoute une liste de calendriers mensuels
        /// </summary>
        /// <param name="listDcp">Une liste des calendriers mensuels</param>
        public void AddDatesCalendrierPaie(IEnumerable<DatesCalendrierPaieEnt> listDcp)
        {
            foreach (DatesCalendrierPaieEnt dcp in listDcp)
            {
                AddDatesCalendrierPaie(dcp);
            }
        }

        /// <summary>
        ///   Supprime tout le paramétrage d'une année pour une société
        /// </summary>
        /// <param name="societeId">Un id de la société</param>
        /// <param name="year">Une année</param>
        public void DeleteSocieteDatesCalendrierPaieByIdAndYear(int societeId, int year)
        {
            IEnumerable<DatesCalendrierPaieEnt> listDcp = Context.DatesCalendrierPaies.Where(rm => rm.SocieteId == societeId && rm.DateFinPointages.HasValue && rm.DateFinPointages.Value.Year == year);
            if (!listDcp.Any())
            {
                DataException objectNotFoundException = new DataException();
                this.logManager.TraceException(objectNotFoundException.Message, objectNotFoundException);
                throw objectNotFoundException;


            }

            foreach (DatesCalendrierPaieEnt dcp in listDcp)
            {
                Context.DatesCalendrierPaies.Remove(dcp);
            }

            try
            {
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
        }
    }
}