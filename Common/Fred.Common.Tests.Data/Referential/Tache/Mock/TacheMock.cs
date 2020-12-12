using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Referential;

namespace Fred.Common.Tests.Data.Referential.Tache.Mock
{
    public class TacheMock
    {
        public FakeDbSet<TacheEnt> GetFakeDbSet()
        {
            return new FakeDbSet<TacheEnt>
            {
                new TacheEnt
                {
                    TacheId = 1,
                    Code = "00",
                    Libelle = "",
                    Active = true,
                    Niveau = 1
                },
                new TacheEnt
                {
                    TacheId = 2,
                    Code = "021",
                    Libelle = "",
                    Active = true,
                    Niveau = 2
                },
                new TacheEnt
                {
                    TacheId = 3,
                    Code = "031",
                    Libelle = "",
                    Active = true,
                    Niveau = 3
                }
            };
        }
    }
}
