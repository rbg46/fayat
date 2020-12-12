using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.FonctionnaliteDesactive;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.FonctionnaliteDesactive
{
    /// <summary>
    ///   Référentiel de données pour les FonctionnaliteDesactive
    /// </summary>
    public class FonctionnaliteDesactiveRepository : FredRepository<FonctionnaliteDesactiveEnt>, IFonctionnaliteDesactiveRepository
    {
        public FonctionnaliteDesactiveRepository(FredDbContext context)
          : base(context)
        {

        }

        /// <summary>
        /// Desactive un Fonctionnalite pour une societe
        /// </summary>
        /// <param name="fonctionnaliteId">fonctionnaliteId</param>
        /// <param name="societeId">societeId</param>
        /// <returns>l'id de l'element FonctionnaliteDesactiveEnt nouvellement créé.</returns>
        public int DisableFonctionnaliteForSocieteId(int fonctionnaliteId, int societeId)
        {
            var newFonctionnaliteDesactive = new FonctionnaliteDesactiveEnt()
            {
                SocieteId = societeId,
                FonctionnaliteId = fonctionnaliteId
            };
            this.Insert(newFonctionnaliteDesactive);
            return newFonctionnaliteDesactive.FonctionnaliteDesactiveId;
        }

        /// <summary>
        /// Retourne une liste de FonctionnaliteDesactiveEnt.
        /// Un Fonctionnalite est desactive des lors qu'il y a un 'entree'(ligne) dans la base.
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <returns>Une liste de FonctionnaliteDesactiveEnt</returns>
        public IEnumerable<FonctionnaliteDesactiveEnt> GetInactifFonctionnalitesForSocieteId(int societeId)
        {
            return Query()
              .Filter(m => m.SocieteId == societeId)
              .Get()
              .AsNoTracking()
              .ToList();
        }
    }
}