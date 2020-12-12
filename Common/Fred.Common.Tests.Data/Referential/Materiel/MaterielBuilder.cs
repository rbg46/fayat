using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Referential;

namespace Fred.Common.Tests.Data.Referential.Materiel
{
    /// <summary>
    /// Builder de l'entité <see cref="TacheEnt"/>
    /// </summary>
    public class MaterielBuilder : ModelDataTestBuilder<MaterielEnt>
    {
        public MaterielEnt Prototype()
        {
            New();
            Model.MaterielId = 1;

            return Model;
        }
    }
}
