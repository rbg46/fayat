using System;
using System.Threading.Tasks;
using Fred.EntityFramework;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Interface du pattern Unit Of Work
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        ///   L''accés couche data
        /// </summary>
        FredDbContext Context { get; }

        /// <summary>
        ///   Sauvegarder les modificaions en cours.
        /// </summary>
        void Save();

        /// <summary>
        ///   Sauvegarder les modificaions en cours de manière async
        /// </summary>
        Task SaveAsync();

        /// <summary>
        /// Sauvegarde des modifications via une transaction.
        /// </summary>
        void SaveWithTransaction();
    }
}
