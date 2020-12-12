using System;
using System.Collections.Generic;
using Fred.Entities.LogImport;

namespace Fred.DataAccess.Interfaces

{
  /// <summary>
  ///   Représente un référentiel de données pour les logs d'import.
  /// </summary>
  public interface ILogImportRepository : IRepository<LogImportEnt>
  {
    /// <summary>
    ///   Retourne une nouvelle instance de LogImport
    /// </summary>
    /// <returns>Retourne un objet initialisé de log import</returns>
    LogImportEnt GetNew();

    /// <summary>
    ///   Retourne la liste de tout les logs import
    /// </summary>
    /// <returns>Une liste trié de log import</returns>
    IEnumerable<LogImportEnt> GetAllLogImport();

    /// <summary>
    ///   Retourne la liste des logs import pour un type d'import passé en parametre
    /// </summary>
    /// <param name="typeImport">Type d'import recherché</param>
    /// <returns>Une liste triée de log import</returns>
    IEnumerable<LogImportEnt> GetLogImportByType(string typeImport);

    /// <summary>
    ///   Retourne la liste des logs import pour une date passée en parametre
    /// </summary>
    /// <param name="dateimport">Date recherché</param>
    /// <returns>Une liste trié de log import</returns>
    IEnumerable<LogImportEnt> GetLogImportByDate(DateTime dateimport);

    /// <summary>
    ///   Insertion en base d'un log import
    /// </summary>
    /// <param name="logImport">Le log import à enregistrer</param>
    /// <returns>Retourne l'identifiant unique du log</returns>
    int Add(LogImportEnt logImport);
  }
}