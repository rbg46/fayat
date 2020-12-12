using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Personnel;
using Fred.EntityFramework;
using System.Collections.Generic;

namespace Fred.DataAccess.RepriseDonnees.Personnel
{
    /// <summary>
    /// Repository des Reprises des données d'un Personnel
    /// </summary>
    public class ReprisePersonnelRepository : IReprisePersonnelRepository
    {
        private readonly FredDbContext fredDbContext;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="unitOfWork">unitOfWork</param>       
        public ReprisePersonnelRepository(IUnitOfWork unitOfWork)
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
        /// Creation des Personnels
        /// </summary>
        /// <param name="personnelEnts">Le Personnel a créer</param>
        public void CreatePersonnel(List<PersonnelEnt> personnelEnts)
        {
            if (personnelEnts.Count > 0)
            {
                fredDbContext.Personnels.AddRange(personnelEnts);
            }
        }
    }
}
