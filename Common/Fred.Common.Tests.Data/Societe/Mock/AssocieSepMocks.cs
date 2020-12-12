using Fred.Common.Tests.EntityFramework;
using Fred.Entities;

namespace Fred.Common.Tests.Data.Societe.Mock
{
    /// <summary>
    /// Elements fictifs des Associes Sep
    /// </summary>
    public class AssocieSepMocks
    {

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de AssocieSepEnt</returns>
        public FakeDbSet<AssocieSepEnt> GetFakeDbSet()
        {
            return new FakeDbSet<AssocieSepEnt>
            {
                new AssocieSepEnt
                {
                    SocieteAssocieeId = 1
                },
                new AssocieSepEnt
                {
                    SocieteAssocieeId = 2
                }
            };
        }

    }
}
