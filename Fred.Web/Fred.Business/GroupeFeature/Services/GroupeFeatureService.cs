using Fred.Business.GroupeFeature.ViewModels;

namespace Fred.Business.GroupeFeature.Services
{
    public class GroupeFeatureService : IGroupeFeatureService
    {

        protected FeatureViewModel GetDefault()
        {
            return new FeatureViewModel()
            {
                IsPossibleToCreateReceptionWithQuantityNegative = false
            };
        }

        public virtual FeatureViewModel GetFeaturesForGroupe()
        {
            return GetDefault();
        }
    }
}
