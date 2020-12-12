using System;
using Fred.ImportExport.Database.ImportExport;

namespace Fred.ImportExport.DataAccess.Common
{
    /// <summary>
    ///  Pattern de UnifOfWork
    /// </summary>
    public class UnitOfWorkStair : IDisposable
    {
        private bool disposed;
        private readonly StairContext stairContext;

        /// <summary>
        /// Initialise une nouvelle instance.
        /// </summary>
        public UnitOfWorkStair()
        {
            stairContext = new StairContext();
        }

        /// <summary>
        /// Initialise une nouvelle instance.
        /// </summary>
        /// <param name="context">Le context import/export</param>
        public UnitOfWorkStair(StairContext context)
        {
            stairContext = context;
        }

        /// <summary>
        /// Récupère l'instance du context import/export
        /// </summary>
        internal StairContext StairContext
        {
            get
            {
                return stairContext;
            }
        }

        /// <summary>
        /// Sauvegarde les modifications en cours
        /// </summary>
        public void Save()
        {
            StairContext.SaveChanges();
        }


        /// <summary>
        ///   Libère les ressources non managées et - managées - en option.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///   Libère les ressources non managées et - managées - en option.
        /// </summary>
        /// <param name="disposing">
        ///   <c>true</c> pour libérer les ressources non managées et managées.; <c>false</c> pour libérer
        ///   uniquement les ressources non managées.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                StairContext.Dispose();
            }

            this.disposed = true;
        }
    }
}
