using Fred.Business.GroupeFeature.ViewModels;
using Fred.GroupSpecific.Infrastructure;

namespace Fred.Business.GroupeFeature.Services
{
    public interface IGroupeFeatureService : IGroupAwareService
    {
        FeatureViewModel GetFeaturesForGroupe();
    }
}
