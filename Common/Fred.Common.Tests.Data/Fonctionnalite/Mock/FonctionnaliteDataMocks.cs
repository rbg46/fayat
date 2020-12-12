using System.Collections.Generic;
using Fred.Entities.Fonctionnalite;

namespace Fred.Common.Tests.Data.Fonctionnalite.Mock
{
    public static class FonctionnaliteDataMocks
    {

        public static IEnumerable<FonctionnaliteEnt> GetFeatureListByModuleId()
        {

            var fonctionnalites = new List<FonctionnaliteEnt>
                            {
                              new FonctionnaliteEnt
                              {
                                Code = "0400",
                                DateSuppression = null,
                                FonctionnaliteId = 1,
                                HorsOrga = false,
                                Libelle = "Affichage des menus 'Pointage'",
                                Module = null,
                                ModuleId = 1
                              },
                               new FonctionnaliteEnt
                              {
                                Code = "0401",
                                DateSuppression = null,
                                FonctionnaliteId = 2,
                                HorsOrga = false,
                                Libelle = "Affichage des menus 'Achat'",
                                Module = null,
                                ModuleId = 1
                              },
                                new FonctionnaliteEnt
                              {
                                Code = "0402",
                                DateSuppression = null,
                                FonctionnaliteId = 3,
                                HorsOrga = false,
                                Libelle = "Affichage des menus 'Commande'",
                                Module = null,
                                ModuleId = 1
                              }
                            };

            return fonctionnalites;
        }

        public static FonctionnaliteEnt GetCentreImputationFonctionnalite()
        {
            return new FonctionnaliteEnt
            {
                Code = "1100",
                DateSuppression = null,
                FonctionnaliteId = 51,
                HorsOrga = false,
                Libelle = "Centre d'imputation'",
                Module = null,
                ModuleId = 18,
                Description = ""
            };
        }

    }
}
