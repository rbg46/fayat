using Fred.ImportExport.Database.ImportExport;
using System;

namespace Fred.ImportExport.DataAccess.Common
{
  /// <summary>
  /// Interface du pattern UnitOfWork
  /// </summary>
  public interface IUnitOfWork : IDisposable
  {
    ImportExportContext ImportExportContext { get; }

    /// <summary>
    /// Sauvegarde les modifications en cours
    /// </summary>
    void Save();
  }
}
