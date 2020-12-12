using System.Collections.Generic;
using System.Linq;
using Fred.Common.Tests.Data.Fournisseur.Builder;
using Fred.Common.Tests.Data.Groupe.Builder;
using Fred.Common.Tests.Data.Referential.Etablissement.Builder;
using Fred.Common.Tests.Data.ReferentielFixe.Builder;
using Fred.Common.Tests.Data.Societe.Builder;
using Fred.Common.Tests.EntityFramework;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Moyen;
using Moq;

namespace Fred.Common.Tests.Data.Moyen.Mock
{
    /// <summary>
    /// Elements fictifs de Moyen
    /// </summary>
    public class AffectationMoyenTypeMocks
    {
        private readonly SocieteBuilder SocietyBuilder = new SocieteBuilder();
        private readonly GroupeBuilder GroupeBuilder = new GroupeBuilder();
        private readonly EtablissementComptableBuilder EtabBuilder = new EtablissementComptableBuilder();
        private readonly FournisseurBuilder FournisseurBuilder = new FournisseurBuilder();
        private readonly RessourceBuilder RessourceBuilder = new RessourceBuilder();

        public AffectationMoyenTypeMocks()
        {
        }

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de AffectationMoyenTypeEnt</returns>
        public FakeDbSet<AffectationMoyenTypeEnt> GetFakeDbSet()
        {
            FakeDbSet<AffectationMoyenTypeEnt> fakeDbSet = new FakeDbSet<AffectationMoyenTypeEnt>();
            foreach (var item in GetFakeList())
            {
                fakeDbSet.Add(item);
            }
            return fakeDbSet;
        }

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de AffectationMoyenTypeEnt</returns>
        public List<AffectationMoyenTypeEnt> GetFakeList()
        {
            return new List<AffectationMoyenTypeEnt>
            {
                new AffectationMoyenTypeEnt
                {
                    AffectationMoyenTypeId =2,
                    Code = "FAM4682",
                    Libelle = "AffectationMoyenType test3",
                    CiCode = "215487"
                },
                new AffectationMoyenTypeEnt
                {
                    AffectationMoyenTypeId =12,
                    Code = "FAM4682",
                    Libelle = "AffectationMoyenType test3",
                    CiCode = "215487"
                },
                new AffectationMoyenTypeEnt
                {
                    AffectationMoyenTypeId =20,
                    Code = "FAM4682",
                    Libelle = "AffectationMoyenType test3",
                    CiCode = "215487"
                }
            };
        }

        /// <summary>
        /// Obtient un repository fictif
        /// </summary>
        /// <returns></returns>
        public Mock<IAffectationMoyenTypeRepository> GetFakeRepository()
        {
            var fakeRepository = new Mock<IAffectationMoyenTypeRepository>();

            //setup
            fakeRepository.Setup(r => r.Get()).Returns(GetFakeList().ToList().AsQueryable());
            fakeRepository.Setup(r => r.Query()).Returns(new RepositoryQuery<AffectationMoyenTypeEnt>(new DbRepository<AffectationMoyenTypeEnt>(null)));

            return fakeRepository;
        }
    }
}
