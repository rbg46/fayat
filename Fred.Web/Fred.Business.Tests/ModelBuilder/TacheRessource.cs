using System.Collections.Generic;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.ReferentielFixe;
using static Fred.Business.Tests.ModelBuilder.CiOrganisationSociete;

namespace Fred.Business.Tests.ModelBuilder
{
    internal class TacheRessource
    {
        public static ChapitreEnt GetFakeChapitre()
        {
            return new ChapitreEnt
            {
                ChapitreId = 1,
                Libelle = "Fake Chapitre",
                Code = "FakeChap"
            };
        }

        public static SousChapitreEnt GetFakeSousChapitre()
        {
            var fakeChapitre = GetFakeChapitre();

            return new SousChapitreEnt
            {
                Chapitre = fakeChapitre,
                ChapitreId = fakeChapitre.ChapitreId,
                SousChapitreId = 1,
                Libelle = "Fake Sous Chapitre",
                Code = "FakeSChap"
            };

        }


        public static RessourceEnt GetFakeRessource()
        {
            var fakeSociete = GetFakeSociete();
            var fakeUnite = GetFakeUnite();
            var fakeSousChapitre = GetFakeSousChapitre();

            return new RessourceEnt
            {
                ReferentielEtendus = new List<ReferentielEtenduEnt>()
                        {
                            new ReferentielEtenduEnt
                            {
                                SocieteId = fakeSociete.SocieteId,
                                ParametrageReferentielEtendus = new List<ParametrageReferentielEtenduEnt>()
                                {
                                    new ParametrageReferentielEtenduEnt
                                    {
                                        Unite = fakeUnite,
                                        UniteId = fakeUnite.UniteId
                                    }
                                }
                            }
                        },
                RessourceId = 1,
                SousChapitre = fakeSousChapitre,
                SousChapitreId = fakeSousChapitre.SousChapitreId
            };


        }

        public static TacheEnt GetFakeTache1()
        {
            var fakeCi = GetFakeCi();
            return new TacheEnt
            {
                Niveau = 1,
                TacheId = 1,
                CI = fakeCi,
                CiId = fakeCi.CiId,
                Code = "Fake T1",
                Libelle = "Fake Tache 1"
            };

        }

        public static TacheEnt GetFakeTache2()
        {
            var fakeT1 = GetFakeTache1();
            return new TacheEnt
            {
                Niveau = 2,
                TacheId = 2,
                CI = fakeT1.CI,
                CiId = fakeT1.CiId,
                Code = "Fake T2",
                Libelle = "Fake Tache 2",
                ParentId = fakeT1.TacheId,
                Parent = fakeT1
            };


        }

        public static TacheEnt GetFakeTache3()
        {
            var fakeT2 = GetFakeTache2();
            return new TacheEnt
            {
                Niveau = 3,
                TacheId = 3,
                CI = fakeT2.CI,
                CiId = fakeT2.CiId,
                Code = "Fake T3",
                Libelle = "Fake Tache 3",
                ParentId = fakeT2.TacheId,
                Parent = fakeT2
            };

        }

        public static TacheEnt GetFakeTache4()
        {
            var fakeTache3 = GetFakeTache3();
            return new TacheEnt
            {
                Niveau = 4,
                TacheId = 4,
                CI = fakeTache3.CI,
                CiId = fakeTache3.CiId,
                Code = "Fake T4",
                Libelle = "Fake Tache 4",
                ParentId = fakeTache3.TacheId,
                Parent = fakeTache3
            };


        }

    }

}
