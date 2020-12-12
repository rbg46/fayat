using Fred.DataAccess.Interfaces;
using Fred.Entities.Depense;
namespace Fred.Business.Depense
{
    /// <summary>
    ///   Gestionnaire des Lots de Far.
    /// </summary>
    public class LotFarManager : Manager<LotFarEnt, ILotFarRepository>, ILotFarManager
    {
        public LotFarManager(IUnitOfWork uow, ILotFarRepository lotFarRepository, ILotFarValidator validator)
          : base(uow, lotFarRepository, validator)
        {
        }

        /// <inheritdoc />
        public LotFarEnt AddLotFar(LotFarEnt lf)
        {
            this.BusinessValidation(lf);
            Repository.AddLotFar(lf);
            Save();

            return lf;
        }
    }
}