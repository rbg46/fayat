using Fred.Common.Tests.Data.Referential.Tache.Builder;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Rapport;

namespace Fred.Common.Tests.Data.Rapport.Builder
{
    public class RapportLigneTacheBuilder : ModelDataTestBuilder<RapportLigneTacheEnt>
    {
        public RapportLigneTacheEnt Prototype()
        {
            Model.TacheId = 1;
            Model.RapportLigneId = 1;
            Model.RapportLigneTacheId = 1;
            Model.Tache = new TacheBuilder().Niveau1();
            Model.HeureTache = 13;

            return Model;
        }
    }
}
