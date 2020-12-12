using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget.Avancement;

namespace Fred.Business.Budget.Avancement
{
    /// <summary>
    /// Gestionnaire d'état d'avancement
    /// </summary>
    public class AvancementEtatManager : Manager<AvancementEtatEnt, IAvancementEtatRepository>, IAvancementEtatManager
    {
        public AvancementEtatManager(IUnitOfWork uow, IAvancementEtatRepository avancementEtatRepository)
          : base(uow, avancementEtatRepository)
        {
        }

        /// <summary>
        /// Retourne l'état de l'avancement en fonction d'un code
        /// </summary>
        /// <param name="code">Code de l'état</param>
        /// <returns>L'état de l'avancement</returns>
        public AvancementEtatEnt GetByCode(string code)
        {
            return Repository.GetByCode(code);
        }
    }
}
