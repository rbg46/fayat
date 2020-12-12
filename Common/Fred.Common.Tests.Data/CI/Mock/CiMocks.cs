using System.Collections.Generic;
using System.Linq;
using Fred.Common.Tests.Data.Affectation.Builder;
using Fred.Common.Tests.Data.Groupe.Builder;
using Fred.Common.Tests.Data.Organisation.Builder;
using Fred.Common.Tests.Data.Referential.Etablissement.Builder;
using Fred.Common.Tests.Data.Societe.Builder;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.CI;

namespace Fred.Common.Tests.Data.CI.Mock
{
    /// <summary>
    /// Elements fictifs de Societe
    /// </summary>
    public class CiMocks
    {
        private readonly OrganisationBuilder OrgBuilder = new OrganisationBuilder();
        private readonly AffectationBuilder AffectBuilder = new AffectationBuilder();
        private readonly SocieteBuilder SocietyBuilder = new SocieteBuilder();
        private readonly GroupeBuilder GroupeBuilder = new GroupeBuilder();
        private readonly EtablissementComptableBuilder EtabBuilder = new EtablissementComptableBuilder();

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de SocieteClassificationEnt</returns>
        public FakeDbSet<CIEnt> GetFakeDbSet()
        {
            FakeDbSet<CIEnt> fakeDbSet = new FakeDbSet<CIEnt>();
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
        public List<CIEnt> GetFakeList()
        {
            return new List<CIEnt>
            {
                new CIEnt{
                    CiId = 189,
                    EtablissementComptableId = 3,
                    Code = "435141",
                    Libelle = "ECO PARK FABREGUES",
                    Organisation = OrgBuilder.Prototype(),
                    Societe = SocietyBuilder.Prototype(),
                    SocieteId = SocietyBuilder.Prototype().SocieteId,
                    Affectations = AffectBuilder.PersonnelId(1).BuildNObjects(1, true).ToList(),
                    EtablissementComptable = EtabBuilder.Prototype()
                },
                new CIEnt{
                    CiId = 210,
                    EtablissementComptableId = 10,
                    Code = "302135",
                    Libelle = "TARAVO (50%)",
                    Organisation = OrgBuilder.Prototype(),
                    Societe = SocietyBuilder.Prototype(),
                    SocieteId = SocietyBuilder.Prototype().SocieteId,
                    Affectations = AffectBuilder.PersonnelId(1).BuildNObjects(1, true).ToList(),
                    EtablissementComptable = EtabBuilder.Prototype()
                },
                new CIEnt{
                    CiId = 657,
                    EtablissementComptableId = 5,
                    Code = "314311",
                    Libelle = "HOPITAL MELUN (50%)",
                    Organisation = OrgBuilder.Prototype(),
                    Societe = SocietyBuilder.Prototype(),
                    SocieteId = SocietyBuilder.Prototype().SocieteId,
                    Affectations = AffectBuilder.PersonnelId(1).BuildNObjects(1, true).ToList(),
                    EtablissementComptable = EtabBuilder.Prototype()
                },
                new CIEnt{
                    CiId = 2589,
                    EtablissementComptableId = 101,
                    Code = "0143.3700.1",
                    Libelle = "piste 1 a paver",
                    Organisation = OrgBuilder.Prototype(),
                    Societe = SocietyBuilder.Prototype(),
                    SocieteId = SocietyBuilder.Prototype().SocieteId,
                    Affectations = AffectBuilder.PersonnelId(1).BuildNObjects(1, true).ToList(),
                    EtablissementComptable = EtabBuilder.Prototype()
                },
                new CIEnt{
                    CiId = 7200,
                    EtablissementComptableId = 101,
                    Code = "720000",
                    Libelle = "Dans 2 societes differents",
                    Organisation = OrgBuilder.Prototype(),
                    Societe = SocietyBuilder.Prototype(),
                    SocieteId = SocietyBuilder.Prototype().SocieteId,
                    Affectations = AffectBuilder.PersonnelId(1).BuildNObjects(1, true).ToList(),
                    EtablissementComptable = EtabBuilder.Prototype()
                },
                new CIEnt{
                    CiId = 7201,
                    EtablissementComptableId = 101,
                    Code = "720000",
                    Libelle = "Dans 2 societes differents",
                    Organisation = OrgBuilder.OrganisationId(1).Build(),
                    Societe = SocietyBuilder.SocieteId(20).Groupe(GroupeBuilder.Prototype()).Build(),
                    SocieteId = 20,
                    Affectations = AffectBuilder.PersonnelId(1).BuildNObjects(1, true).ToList(),
                    EtablissementComptable = EtabBuilder.Prototype()
                }
            };
        }
    }
}
