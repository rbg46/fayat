using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget.Avancement;
using Fred.EntityFramework;

namespace Fred.DataAccess.Budget.Avancement
{
    /// <summary>
    ///   Référentiel de données pour les barèmes exploitation CI.
    /// </summary>
    public class AvancementEtatRepository : FredRepository<AvancementEtatEnt>, IAvancementEtatRepository
    {
        public AvancementEtatRepository(FredDbContext context)
          : base(context)
        { }

        /// <inheritdoc />
        public AvancementEtatEnt GetByCode(string code)
        {
            return this.Context.AvancementEtats.FirstOrDefault(e => e.Code == code);
        }
    }
}
