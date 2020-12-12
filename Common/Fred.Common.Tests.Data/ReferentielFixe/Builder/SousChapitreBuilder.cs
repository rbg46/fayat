using Fred.Common.Tests.EntityFramework;
using Fred.Entities.ReferentielFixe;

namespace Fred.Common.Tests.Data.ReferentielFixe.Builder
{
    /// <summary>
    /// Classe builder de <see cref=""/>
    /// </summary>
    public class SousChapitreBuilder : ModelDataTestBuilder<SousChapitreEnt>
    {
        private readonly ChapitreBuilder ChapitreBuilder = new ChapitreBuilder();

        public override SousChapitreEnt New()
        {
            base.New();
            Model.Code = "10-100";
            Model.Libelle = "DIRECTEUR";
            Model.ChapitreId = ChapitreBuilder.New().ChapitreId;
            Model.Chapitre = ChapitreBuilder.Model;
            return Model;
        }
    }
}
