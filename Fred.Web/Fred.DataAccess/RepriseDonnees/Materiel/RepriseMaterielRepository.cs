using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using System.Collections.Generic;

namespace Fred.DataAccess.RepriseDonnees.Materiel
{
    /// <summary>
    /// Repository des Reprises des données des Materiels
    /// </summary>
    public class RepriseMaterielRepository : IRepriseMaterielRepository
    {
        private readonly FredDbContext fredDbContext;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="unitOfWork">unitOfWork</param>
        public RepriseMaterielRepository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            fredDbContext = (UnitOfWork as UnitOfWork)?.Context;
            //// this.fredDbContext.Database.Log = s => Debug.WriteLine(s) 
        }

        /// <summary>
        /// UnitOfWork
        /// </summary>
        public IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// Creation des Materiels
        /// </summary>
        /// <param name="materielEnts">Le Matériel a créer</param>
        public void CreateMateriel(List<MaterielEnt> materielEnts)
        {
            if (materielEnts.Count > 0)
            {
                fredDbContext.Materiels.AddRange(materielEnts);
            }
        }
    }
}
