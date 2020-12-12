using Fred.Business.GroupeFeacture.Services;
using Fred.Business.GroupeFeature.ViewModels;

namespace Fred.Business.GroupeFeature.Services
{
    public class GroupeFeatureServiceFtp : GroupeFeatureService, IGroupeFeatureServiceGFTP
    {
        public override FeatureViewModel GetFeaturesForGroupe()
        {
            var feature = GetDefault();
            feature.IsPossibleToCreateReceptionWithQuantityNegative = true;
            return feature;
        }
    }
}
