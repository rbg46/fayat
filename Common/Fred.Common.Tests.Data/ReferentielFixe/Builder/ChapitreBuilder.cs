using Fred.Common.Tests.EntityFramework;
using Fred.Entities.ReferentielFixe;

namespace Fred.Common.Tests.Data.ReferentielFixe.Builder
{
    /// <summary>
    /// Builder de <see cref="ChapitreEnt"/>
    /// </summary>
    public class ChapitreBuilder : ModelDataTestBuilder<ChapitreEnt>
    {
        public override ChapitreEnt New()
        {
            base.New();
            Model.ChapitreId = 1;
            Model.Code = "10";
            Model.Libelle = "MO ENCADREMENT";
            Model.GroupeId = 1;
            return Model;
        }
    }
}
