using System;
using System.Collections.Generic;
using Fred.Business.Moyen;
using Fred.Common.Tests.Data.Fournisseur.Builder;
using Fred.Common.Tests.Data.Referential.Etablissement.Builder;
using Fred.Common.Tests.Data.ReferentielFixe.Builder;
using Fred.Common.Tests.Data.Societe.Builder;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Referential;
using Moq;

namespace Fred.Common.Tests.Data.Moyen.Mock
{
    /// <summary>
    /// Elements fictifs de Moyen
    /// </summary>
    public class MoyenMocks
    {
        private readonly SocieteBuilder SocietyBuilder = new SocieteBuilder();
        private readonly EtablissementComptableBuilder EtabBuilder = new EtablissementComptableBuilder();
        private readonly FournisseurBuilder FournisseurBuilder = new FournisseurBuilder();
        private readonly RessourceBuilder RessourceBuilder = new RessourceBuilder();

        public MoyenMocks()
        {
        }

        /// <summary>
        /// Obtient un moyen
        /// </summary>
        /// <returns></returns>
        public Mock<IMoyenManager> GetFakeManager()
        {
            Mock<IMoyenManager> fakeManager = new Mock<IMoyenManager>();
            return fakeManager;
        }

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de SocieteClassificationEnt</returns>
        public FakeDbSet<MaterielEnt> GetFakeDbSet()
        {
            FakeDbSet<MaterielEnt> fakeDbSet = new FakeDbSet<MaterielEnt>();
            foreach (var item in GetFakeList())
            {
                fakeDbSet.Add(item);
            }
            return fakeDbSet;
        }

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de SocieteClassificationEnt</returns>
        public List<MaterielEnt> GetFakeList()
        {
            return new List<MaterielEnt>
            {
                new MaterielEnt{
                    MaterielId = 1,
                    Actif = true,
                    Code = "FAM4682",
                    Libelle = "Moyen test1",
                    SocieteId = SocietyBuilder.Prototype().SocieteId,
                    Societe = SocietyBuilder.Prototype(),
                    EtablissementComptable = EtabBuilder.Prototype(),
                    Fournisseur = FournisseurBuilder.New(),
                    Ressource = RessourceBuilder.New()
                },
                new MaterielEnt{
                    MaterielId = 2,
                    Actif = true,
                    Code = "NBESEP1",
                    Libelle = "Moyen test2",
                    SocieteId = SocietyBuilder.Prototype().SocieteId,
                    Societe = SocietyBuilder.Prototype(),
                    EtablissementComptable = EtabBuilder.Prototype(),
                    Fournisseur = FournisseurBuilder.New(),
                    Ressource = RessourceBuilder.New()
                },
                new MaterielEnt{
                    MaterielId = 3,
                    Actif = true,
                    Code = "P225JS1",
                    Libelle = "Moyen test3",
                    SocieteId = SocietyBuilder.Prototype().SocieteId,
                    Societe = SocietyBuilder.Prototype(),
                    EtablissementComptable = EtabBuilder.Prototype(),
                    Fournisseur = FournisseurBuilder.New(),
                    Ressource = RessourceBuilder.New()
                },
                new MaterielEnt{
                    MaterielId = 5,
                    Actif = true,
                    Code = "P225JS1",
                    Libelle = "Moyen test doublon",
                    SocieteId = SocietyBuilder.Prototype().SocieteId,
                    Societe = SocietyBuilder.Prototype(),
                    EtablissementComptable = EtabBuilder.Prototype(),
                    Fournisseur = FournisseurBuilder.New(),
                    Ressource = RessourceBuilder.New()
                },
                new MaterielEnt
                {
                    MaterielId = 10,
                    Actif = true,
                    Code = "FAM4682",
                    Libelle = "Moyen test3",
                    DateCreation = new DateTime(2019, 1, 28),
                    AuteurCreationId = 1
                }
            };
        }
    }
}
