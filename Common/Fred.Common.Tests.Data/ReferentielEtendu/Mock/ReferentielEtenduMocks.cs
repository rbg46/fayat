using System.Collections.Generic;
using System.Linq;
using Fred.Common.Tests.Data.ReferentielFixe.Mock;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.ReferentielFixe;

namespace Fred.Common.Tests.Data.ReferentielEtendu.Mock
{
    /// <summary>
    /// Classe des élements fictifs d'un referentiel etendu
    /// </summary>
    public class ReferentielEtenduMocks
    {
        /// <summary>
        /// Liste des ressources fictives
        /// </summary>
        private readonly List<RessourceEnt> Ressources = new RessourceMocks().GetFakeDbSet().ToList();

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de ReferentielEtenduEnt</returns>
        public FakeDbSet<ReferentielEtenduEnt> GetFakeDbSet()
        {
            return new FakeDbSet<ReferentielEtenduEnt>
            {
                new ReferentielEtenduEnt{
                    ReferentielEtenduId = 1,
                    RessourceId = Ressources[0].RessourceId,
                    Ressource = Ressources[0],
                    SocieteId = 1,
                    NatureId = 1

                },
                new ReferentielEtenduEnt{
                    ReferentielEtenduId = 2,
                    RessourceId = Ressources[0].RessourceId,
                    Ressource = Ressources[0],
                    SocieteId = 1,
                    NatureId = 1

                },
                new ReferentielEtenduEnt{
                    ReferentielEtenduId = 3,
                    RessourceId = Ressources[1].RessourceId,
                    Ressource = Ressources[1],
                    SocieteId = 1,
                    NatureId = 1

                },
                new ReferentielEtenduEnt{
                    ReferentielEtenduId = 4,
                    RessourceId = Ressources[1].RessourceId,
                    Ressource = Ressources[1],
                    SocieteId = 1,
                    NatureId = 1
                },
                new ReferentielEtenduEnt{
                    ReferentielEtenduId = 4,
                    RessourceId = Ressources[2].RessourceId,
                    Ressource = Ressources[2],
                    SocieteId = 1,
                    NatureId = 1
                }
            };
        }
    }
}
