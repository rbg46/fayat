using System.Collections.Generic;
using System.Linq;
using Fred.Common.Tests.Data.ReferentielEtendu.Builder;
using Fred.Entities.ReferentielEtendu;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.ReferentielFixe;

namespace Fred.Common.Tests.Data.ReferentielFixe.Mock
{
    public class RessourceMocks
    {
        private readonly ReferentielEtenduBuilder ReferentielEtenduBuilder = new ReferentielEtenduBuilder();

        /// <summary>
        /// Liste des Types de ressources
        /// </summary>
        public enum TypeRessource
        {
            personnel = 1,
            materiel = 2
        }

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de RessourceEnt</returns>
        public FakeDbSet<RessourceEnt> GetFakeDbSet()
        {
            FakeDbSet<RessourceEnt> fakeDbSet = new FakeDbSet<RessourceEnt>();
            foreach (var item in GetFakeList())
            {
                fakeDbSet.Add(item);
            }
            return fakeDbSet;
        }

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de RessourceEnt</returns>
        public List<RessourceEnt> GetFakeList()
        {
            return new List<RessourceEnt>
            {
                new RessourceEnt{
                    RessourceId = 1,
                    Code = "ENCA-01",
                    Libelle = "DIRECTEUR",
                    TypeRessource = new TypeRessourceEnt { TypeRessourceId = 1 },
                    TypeRessourceId = (int)TypeRessource.personnel,
                    SousChapitre = new SousChapitreEnt { Chapitre = new ChapitreEnt{ GroupeId = 1 } },
                    ReferentielEtendus = new List<ReferentielEtenduEnt>(),
                    Active = true
                },
                new RessourceEnt{
                    RessourceId = 2,
                    Code = "ASCEN-001",
                    Libelle = "ASCENCEUR",
                    TypeRessource = new TypeRessourceEnt { TypeRessourceId = 2  },
                    TypeRessourceId = (int)TypeRessource.materiel,
                    SousChapitre = new SousChapitreEnt { Chapitre = new ChapitreEnt{ GroupeId = 1 } },
                    ReferentielEtendus = new List<ReferentielEtenduEnt>(),
                    Active = true
                },
                new RessourceEnt{
                    RessourceId = 3,
                    Code="PCR-02",
                    Libelle="PIÈCES MÉCANIQUES DIVERSES",
                    TypeRessource = new TypeRessourceEnt { TypeRessourceId = 1  },
                    TypeRessourceId = (int)TypeRessource.personnel,
                    SousChapitre = new SousChapitreEnt { Chapitre = new ChapitreEnt{ GroupeId = 1 } },
                    ReferentielEtendus = ReferentielEtenduBuilder.BuildNObjects(2, true).ToList(),
                    Active = true,
                    SpecifiqueCiId = 1
                }
            };
        }
    }
}
