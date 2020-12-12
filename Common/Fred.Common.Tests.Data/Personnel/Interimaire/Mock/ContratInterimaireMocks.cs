using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Personnel.Interimaire;

namespace Fred.Common.Tests.Data.Personnel.Interimaire.Mock
{
    /// <summary>
    /// Elements fictifs des contrats intérimaires
    /// </summary>
    public class ContratInterimaireMocks
    {
        public IEnumerable<ContratInterimaireEnt> ContratsInterimairesExpected => contratsInterimaires ?? (contratsInterimaires = GetFakeDbSet().ToList());

        private IEnumerable<ContratInterimaireEnt> contratsInterimaires;

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de ContratInterimaireEnt</returns>
        public FakeDbSet<ContratInterimaireEnt> GetFakeDbSet()
        {
            return new FakeDbSet<ContratInterimaireEnt>
            {
                new ContratInterimaireEnt{
                    ContratInterimaireId = 1,
                    SocieteId = 1,
                    InterimaireId = 1,
                    FournisseurId = 2,
                    DateDebut = new DateTime(2019, 06, 01),
                    DateFin = new DateTime(2019, 06, 10),
                    CiId = 1
                },
                new ContratInterimaireEnt{
                    ContratInterimaireId = 2,
                    SocieteId = 1,
                    InterimaireId = 1,
                    FournisseurId = 1,
                    DateDebut = new DateTime(2019, 06, 11),
                    DateFin = new DateTime(2019, 06, 20),
                    CiId = 1
                },
                new ContratInterimaireEnt{
                    ContratInterimaireId = 3,
                    SocieteId = 1,
                    InterimaireId = 1,
                    FournisseurId = 3,
                    DateDebut = new DateTime(2019, 06, 21),
                    DateFin = new DateTime(2019, 06, 30),
                    CiId = 1
                }
            };
        }        
    }
}
