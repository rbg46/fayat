using System.Collections.Generic;
using Fred.Entities.DatesCalendrierPaie;

namespace Fred.Business.DatesCalendrierPaie
{
  /// <summary>
  ///   Interface des gestionnaires des sociétés
  /// </summary>
  public interface IDatesCalendrierPaieManager : IManager<DatesCalendrierPaieEnt>
  {
    /// <summary>
    ///   Retourne une liste de DatesCalendrierPaieEnt par société et année
    /// </summary>
    /// <param name="societeId">Un id de la société</param>
    /// <param name="year">Une année</param>
    /// <returns>Renvoi une liste de calendriers</returns>
    IEnumerable<DatesCalendrierPaieEnt> GetSocieteListDatesCalendrierPaieByIdAndYear(int societeId, int year);

    /// <summary>
    ///   Retourne une liste de DatesCalendrierPaieEnt par société, année et mois
    /// </summary>
    /// <param name="societeId">Un id de la société</param>
    /// <param name="year">Une année</param>
    /// <param name="month">Un mois</param>
    /// <returns>Renvoi une liste de calendriers</returns>
    DatesCalendrierPaieEnt GetSocieteDatesCalendrierPaieByIdAndYearAndMonth(int societeId, int year, int month);

    /// <summary>
    ///   Ajoute un DatesCalendrierPaieEnt en base
    /// </summary>
    /// <param name="dcp">Un calendrier mensuel</param>
    /// <returns>Renvoi l'id technique</returns>
    int AddDatesCalendrierPaie(DatesCalendrierPaieEnt dcp);

    /// <summary>
    ///   Met à jour un DatesCalendrierPaieEnt en base
    /// </summary>
    /// <param name="dcp">Le calendrier mensuel</param>
    void UpdateDatesCalendrierPaie(DatesCalendrierPaieEnt dcp);

    /// <summary>
    ///   Ajoute ou met à jour un DatesCalendrierPaieEnt en fonction de son existence
    /// </summary>
    /// <param name="dcp">Un calendrier mensuel</param>
    void AddOrUpdateDatesCalendrierPaie(DatesCalendrierPaieEnt dcp);

    /// <summary>
    ///   Supprime tout le paramétrage d'une année pour une société
    /// </summary>
    /// <param name="societeId">Un id de la société</param>
    /// <param name="year">Une année</param>
    void DeleteSocieteDatesCalendrierPaieByIdAndYear(int societeId, int year);

    /// <summary>
    ///   Renvoi vrai si la date de fin de pointage est inférieure à la date de transfert des pointages
    /// </summary>
    /// <param name="dcp">Un calendrier mensuel</param>
    /// <returns>Renvoi vrai si les dates sont bonnes</returns>
    bool IsDateFinPtgLowerThanDateTransfertPtg(DatesCalendrierPaieEnt dcp);
  }
}