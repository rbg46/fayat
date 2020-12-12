using Fred.Entities.FonctionnaliteDesactive;
using System.Collections.Generic;

namespace Fred.Common.Tests.Data.Fonctionnalite.Mock
{
  public static class FonctionnaliteDesactiveDataMocks
  {

    public static IEnumerable<FonctionnaliteDesactiveEnt> GetInactifFonctionnalitesForSocieteId()
    {
      return new List<FonctionnaliteDesactiveEnt>
                            {
                              new FonctionnaliteDesactiveEnt
                              {
                               FonctionnaliteDesactiveId = 1,
                               FonctionnaliteId = 1,
                               SocieteId = 1
                              },

                            };
    }

    public static IEnumerable<FonctionnaliteDesactiveEnt> GetAllInactivesFonctionnalites()
    {
      return new List<FonctionnaliteDesactiveEnt>
                            {
                              new FonctionnaliteDesactiveEnt
                              {
                               FonctionnaliteDesactiveId = 1,
                               FonctionnaliteId = 1,
                               SocieteId = 1
                              },
                               new FonctionnaliteDesactiveEnt
                              {
                               FonctionnaliteDesactiveId = 2,
                               FonctionnaliteId = 2,
                               SocieteId = 1
                              },
                                new FonctionnaliteDesactiveEnt
                              {
                               FonctionnaliteDesactiveId = 3,
                               FonctionnaliteId = 3,
                               SocieteId = 1
                              },
                                 new FonctionnaliteDesactiveEnt
                              {
                               FonctionnaliteDesactiveId = 4,
                               FonctionnaliteId = 4,
                               SocieteId = 1
                              },

                            };
    }
  }
}
