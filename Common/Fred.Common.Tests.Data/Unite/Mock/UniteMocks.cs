using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Referential;

namespace Fred.Common.Tests.Data.Unite.Mock
{
    /// <summary>
    /// Elements fictifs de Societe
    /// </summary>
    public class UniteMocks
    {
        /// <summary>
        /// ctor
        /// </summary>
        public UniteMocks()
        {
        }

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de SocieteClassificationEnt</returns>
        public FakeDbSet<UniteEnt> GetFakeDbSet()
        {
            return new FakeDbSet<UniteEnt>
            {
                new UniteEnt{
                    UniteId=1,
                    Code="FRT",
                    Libelle="Forfait"
                },

                new UniteEnt{
                    UniteId=4,
                    Code="H",
                    Libelle="Heure"
                }
            };
        }
    }
}
