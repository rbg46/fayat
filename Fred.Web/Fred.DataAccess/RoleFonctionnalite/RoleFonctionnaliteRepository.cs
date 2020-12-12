using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.RoleFonctionnalite;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.RoleFonctionnalite
{
    public class RoleFonctionnaliteRepository : FredRepository<RoleFonctionnaliteEnt>, IRoleFonctionnaliteRepository
    {
        private readonly FredDbContext context;

        public RoleFonctionnaliteRepository(ILogManager logManager, IUnitOfWork uow, FredDbContext context)
          : base(context)
        {
            this.context = context;
        }

        public async Task<List<RoleFonctionnaliteEnt>> GetByUserIdAndListFonctionnaliteAsync(int userId, List<string> fonctionnaliteCodeList)
        {
            return await context.RoleFonctionnalites
                .Include(x => x.Role)
                .Include(x => x.Fonctionnalite)
                .Where(x => x.Role.AffectationSeuilUtilisateurs.Select(y => y.UtilisateurId).Contains(userId) &&
                            fonctionnaliteCodeList.Contains(x.Fonctionnalite.Code))
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Get role et fonctionnalite by utilisateur id and fonctionnalite libelle
        /// </summary>
        /// <param name="userId">Utilisateur Id</param>
        /// <param name="fonctionnaliteLibelle">Fonctionnalite libelle</param>
        /// <returns>List des roles fonctionnalites</returns>
        public async Task<List<RoleFonctionnaliteEnt>> GetRoleFonctionnaliteByUserIdAsync(int userId, string fonctionnaliteLibelle)
        {
            return await context.RoleFonctionnalites.Include(x => x.Fonctionnalite)
                                                    .Include(x => x.Role.AffectationSeuilUtilisateurs).ThenInclude(y => y.Organisation.Societe.Organisation.TypeOrganisation)
                                                    .Include(x => x.Role.AffectationSeuilUtilisateurs).ThenInclude(y => y.Organisation.Pere.Societe.Organisation.TypeOrganisation)
                                                    .Include(x => x.Role.AffectationSeuilUtilisateurs).ThenInclude(y => y.Organisation.TypeOrganisation)
                                                    .Where(x => x.Role.AffectationSeuilUtilisateurs.Select(y => y.UtilisateurId).Contains(userId) &&
                                                    x.Fonctionnalite.Libelle.Equals(fonctionnaliteLibelle)).AsNoTracking().ToListAsync().ConfigureAwait(false);
        }
    }
}
