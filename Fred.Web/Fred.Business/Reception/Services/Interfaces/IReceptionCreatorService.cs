using Fred.Entities.Depense;

namespace Fred.Business.Reception.Services
{
    public interface IReceptionCreatorService
    {
        DepenseAchatEnt Create(int commandeLigneId);
    }
}
