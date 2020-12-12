using System.Collections.Generic;
using Fred.Entities.DatesCalendrierPaie;

namespace Fred.DataAccess.Interfaces

{
  /// <summary>
  ///   Représente un référentiel de données pour les sociétés.
  /// </summary>
  public interface IDatesCalendrierPaieRepository : IRepository<DatesCalendrierPaieEnt>
  {
    /// <summary>
    ///   Retourne une liste de DatesCalendrierPaieEnt par société et année
    /// </summary>
    /// <param name="societeId">Un id de la société</param>
    /// <param name="year">Une année</param>
    /// <returns>Renvoi une liste de calendriers</returns>
    IEnumerable<DatesCalendrierPaieEnt> GetSocieteListDatesCalendrierPaieByIdAndYear(int societeId, int year);

    /// <summary>
    ///   Rerourne une liste de DatesCalendrierPaieEnt par société, année et mois
    /// </summary>
    /// <param name="societeId">Un id de la société</param>
    /// <param name="year">Une année</param>
    /// <param name="month">Un mois</param>
    /// <returns>Renvoi une liste de calendriers</returns>
    DatesCalendrierPaieEnt GetSocieteDatesCalendrierPaieByIdAndYearAndMonth(int societeId, int year, int month);

    /// <summary>
    ///   Ajoute un DatesCalendrierPaieEnt en base
    /// </summary>
    /// <param name="dcp">Le calendrier mensuel</param>
    /// <returns>Renvoi l'id technique</returns>
    int AddDatesCalendrierPaie(DatesCalendrierPaieEnt dcp);

    /// <summary>
    ///   Ajoute une liste de DatesCalendrierPaieEnt en base
    /// </summary>
    /// <param name="listDcp">Une liste de calendriers mensuels</param>
    void AddDatesCalendrierPaie(IEnumerable<DatesCalendrierPaieEnt> listDcp);

    /// <summary>
    ///   Met à jour un DatesCalendrierPaieEnt en base
    /// </summary>
    /// <param name="dcp">Un calendrier mensuel</param>
    void UpdateDatesCalendrierPaie(DatesCalendrierPaieEnt dcp);

    /// <summary>
    ///   Supprime tout le paramétrage d'une année pour une société
    /// </summary>
    /// <param name="societeId">Un id de la société</param>
    /// <param name="year">Une année</param>
    void DeleteSocieteDatesCalendrierPaieByIdAndYear(int societeId, int year);
  }
}