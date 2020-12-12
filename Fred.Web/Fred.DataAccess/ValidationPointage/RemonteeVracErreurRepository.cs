using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.ValidationPointage;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.ValidationPointage
{
    /// <summary>
    ///   Classe RemonteeVracErreurRepository
    /// </summary>
    public class RemonteeVracErreurRepository : FredRepository<RemonteeVracErreurEnt>, IRemonteeVracErreurRepository
    {
        /// <summary>
        ///   Constructeur <seealso cref="RemonteeVracErreurRepository"/>
        /// </summary>
        /// <param name="logMgr">Gestionnaire des logs</param>
        /// <param name="uow">Unit of work</param>
        public RemonteeVracErreurRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <inheritdoc/>
        public RemonteeVracErreurEnt AddRemonteeVracErreur(RemonteeVracErreurEnt remonteeVracErreur)
        {
            Insert(remonteeVracErreur);

            return remonteeVracErreur;
        }

        /// <inheritdoc/>
        public IEnumerable<RemonteeVracErreurEnt> Get(int remonteeVracId, string searchText)
        {
            return Query()
                   .Include(x => x.Personnel.EtablissementPaie)
                   .Include(x => x.Societe)
                   .Include(x => x.EtablissementPaie)
                   .Filter(x => x.RemonteeVracId == remonteeVracId)
                   .Filter(x => string.IsNullOrEmpty(searchText) ||
                                x.Personnel.Matricule.Contains(searchText) ||
                                x.Personnel.Nom.ToUpper().Contains(searchText.ToUpper()) ||
                                x.Personnel.Prenom.ToUpper().Contains(searchText.ToUpper()))
                   .OrderBy(x => x.OrderBy(y => y.Personnel.Matricule).ThenBy(y => y.Personnel.Prenom).ThenBy(y => y.Personnel.Nom))
                   .Get()
                   .AsNoTracking();
        }
    }
}
