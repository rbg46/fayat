using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Referential;

namespace Fred.Common.Tests.Data.Referential.Prime.Mock
{
    /// <summary>
    /// Elements fictif des primes 
    /// </summary>
    public class PrimeMocks
    {
        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de PrimeEnt</returns>
        public FakeDbSet<PrimeEnt> GetFakeDbSet()
        {
            return new FakeDbSet<PrimeEnt>
            {
                new PrimeEnt
                {
                    PrimeId = 1,
                    Code = "101",
                    Libelle = "PRIME PUBLIQUE SOCIETE 1",
                    Actif = true,
                    SocieteId = 1,
                    GroupeId = 1,
                    PrimeType = ListePrimeType.PrimeTypeHoraire,
                    Publique = true
                },
                new PrimeEnt
                {
                    PrimeId = 2,
                    Code = "102",
                    Libelle = "PRIME PUBLIQUE SOCIETE 2",
                    Actif = true,
                    SocieteId = 2,
                    GroupeId = 2,
                    PrimeType = ListePrimeType.PrimeTypeHoraire,
                    Publique = true
                },
                 new PrimeEnt
                {
                    PrimeId = 3,
                    Code = "103",
                    Libelle = "PRIME PRIVEE SOCIETE 1",
                    Actif = true,
                    SocieteId = 1,
                    GroupeId = 1,
                    PrimeType = ListePrimeType.PrimeTypeHoraire,
                    Publique = false
                },
                new PrimeEnt
                {
                    PrimeId = 4,
                    Code = "104",
                    Libelle = "PRIME PUBLIQUE SOCIETE 1 GROUPE 2",
                    Actif = true,
                    SocieteId = 1,
                    GroupeId = 2,
                    PrimeType = ListePrimeType.PrimeTypeHoraire,
                    Publique = true
                },
                 new PrimeEnt
                {
                    PrimeId = 5,
                    Code = "105",
                    Libelle = "PRIME PRIVEE SOCIETE 1 GROUPE NULL",
                    Actif = true,
                    SocieteId = 1,
                    GroupeId = null,
                    PrimeType = ListePrimeType.PrimeTypeJournaliere,
                    Publique = false
                },
            };
        }
    }
}
