using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.IndemniteDeplacement;
using Fred.EntityFramework;
using System.Collections.Generic;

namespace Fred.DataAccess.RepriseDonnees.IndemniteDeplacement
{
    /// <summary>
    /// Repository des Reprises des données des Personnels
    /// </summary>
    public class RepriseIndemniteDeplacementRepository : IRepriseIndemniteDeplacementRepository
    {
        private readonly FredDbContext fredDbContext;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="unitOfWork">unitOfWork</param>
        public RepriseIndemniteDeplacementRepository(IUnitOfWork unitOfWork)
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
        /// Creation de Indemnite Deplacement
        /// </summary>
        /// <param name="indemniteDeplacementEnts">Les Indemnite Deplacement a créer</param>
        public void CreateIndemniteDeplacement(List<IndemniteDeplacementEnt> indemniteDeplacementEnts)
        {
            if (indemniteDeplacementEnts.Count > 0)
            {
                fredDbContext.IndemniteDeplacement.AddRange(indemniteDeplacementEnts);
            }
        }
    }
}
