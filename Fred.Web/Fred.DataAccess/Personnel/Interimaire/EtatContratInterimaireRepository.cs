using System.Collections.Generic;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Personnel.Interimaire;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Personnel.Interimaire
{
    /// <summary>
    ///   Référentiel de données pour les Contrats Interimaire 
    /// </summary>
    public class EtatContratInterimaireRepository : FredRepository<EtatContratInterimaireEnt>, IEtatContratInterimaireRepository
    {

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="ContratInterimaireRepository" />.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        /// <param name="uow">Unit of work</param>
        public EtatContratInterimaireRepository(FredDbContext context)
          : base(context) { }

        /// <summary>
        /// Récupére la liste des états d'un contrat intérimaire.
        /// </summary>
        /// <returns>List des état d'un contrat intérimaire.</returns>
        public IEnumerable<EtatContratInterimaireEnt> GetEtatContratInterimaireList()
        {
            return Query().Get().AsNoTracking();
        }
    }
}
