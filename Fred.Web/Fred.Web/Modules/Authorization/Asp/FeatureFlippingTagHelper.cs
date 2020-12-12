using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using CommonServiceLocator;
using Fred.Business.FeatureFlipping;
using Fred.Business.FeatureFlipping.Model;
using Fred.Entities.FeatureFlipping;
using Newtonsoft.Json;


namespace Fred.Web.Modules.Authorization.Asp
{
    public static class FeatureFlippingTagHelper
    {
        public static MvcHtmlString GetFeatureFlipping(this HtmlHelper helper)
        {
            IFeatureFlippingManager featureFlippingManager = ServiceLocator.Current.GetInstance<IFeatureFlippingManager>();
            IEnumerable<FeatureFlippingEnt> features = featureFlippingManager.GetFeatureFlippings();

            // La ligne suivante utilisant Automapper semble ne pas être thread safe 
            //// IMapper mapper = ServiceLocator.Current.GetInstance<IMapper>();
            //// var featureFlippingMapped = mapper.Map<List<FeatureFlippingForTagHelperModel>>(features);
            // Cette ligne de code provoque des erreurs aléatoires en production
            // L'automapper est donc remplacé directement par un mapping manuel.
            // WI 6591

            IList<FeatureFlippingForTagHelperModel> models = new List<FeatureFlippingForTagHelperModel>();
            foreach (FeatureFlippingEnt feature in features)
            {
                models.Add(new FeatureFlippingForTagHelperModel
                {
                    Name = feature.Name,
                    IsActived = feature.IsActived
                });
            }

            string json = JsonConvert.SerializeObject(models);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<script> var featureFlippingConfig = {0}</script>", json);
            return MvcHtmlString.Create(sb.ToString());
        }
    }
}
