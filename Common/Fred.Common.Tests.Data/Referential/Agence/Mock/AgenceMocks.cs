using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Referential;

namespace Fred.Common.Tests.Data.Referential.Agence.Mock
{
    internal class AgenceMocks
    {

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de AgenceEnt</returns>
        internal FakeDbSet<AgenceEnt> GetFakeDbSet()
        {
            return new FakeDbSet<AgenceEnt>
            {
                new AgenceEnt
                {
                    AgenceId = 1
                },
                new AgenceEnt
                {
                    AgenceId = 2
                }
            };
        }
    }
}
