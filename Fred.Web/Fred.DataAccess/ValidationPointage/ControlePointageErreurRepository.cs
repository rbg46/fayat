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
    ///   Classe ControlePointageErreurRepository
    /// </summary>
    public class ControlePointageErreurRepository : FredRepository<ControlePointageErreurEnt>, IControlePointageErreurRepository
    {
        /// <summary>
        ///   Constructeur <seealso cref="ControlePointageRepository"/>
        /// </summary>
        /// <param name="logMgr">Gestionnaire des logs</param>
        /// <param name="uow">Unit of work</param>
        public ControlePointageErreurRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <inheritdoc />
        public ControlePointageErreurEnt AddControlePointageErreur(ControlePointageErreurEnt cpErreur)
        {
            Insert(cpErreur);

            return cpErreur;
        }

        /// <inheritdoc />
        public IEnumerable<ControlePointageErreurEnt> GetControlePointageErreurList(int controlePointageId, string searchText)
        {
            return Query()
                   .Include(x => x.Personnel.EtablissementPaie)
                   .Filter(x => x.ControlePointageId == controlePointageId)
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
