using System;
using Fred.ImportExport.DataAccess.Interfaces;
using Fred.ImportExport.Database.ImportExport;

namespace Fred.ImportExport.DataAccess.Common
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private bool disposed;
        private readonly ImportExportContext context;

        public UnitOfWork(ImportExportContext context)
        {
            this.context = context;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                context.Dispose();
            }

            disposed = true;
        }
    }
}
