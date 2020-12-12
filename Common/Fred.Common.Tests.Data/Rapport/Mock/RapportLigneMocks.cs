using System.Collections.Generic;
using System.Linq;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Rapport;

namespace Fred.Common.Tests.Data.Rapport.Mock
{
    /// <summary>
    /// Classe static des éléments fictives du rapport
    /// </summary>
    public class RapportLigneMocks
    {
        /// <summary>
        /// Ontient la liste fictives des sociétés
        /// </summary>
        public List<RapportLigneEnt> RapportLignesStub => this.GetFakeDbSet().ToList();

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de RapportLigneEnt</returns>
        public FakeDbSet<RapportLigneEnt> GetFakeDbSet()
        {
            return new FakeDbSet<RapportLigneEnt>
            {
                new RapportLigneEnt{
                    CiId = 1,
                    RapportId = 1,
                    Ci = new Entities.CI.CIEnt()
                    {
                      Societe = new Entities.Societe.SocieteEnt()
                      {
                        Groupe = new Entities.Groupe.GroupeEnt()
                        { Code = Fred.Entities.Constantes.CodeGroupeFES }
                      }
                    }
                },
                new RapportLigneEnt{
                    CiId = 2,
                    RapportId = 2
                }
            };
        }
    }
}
