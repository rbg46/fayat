using System.Collections.Generic;
using Fred.Common.Tests.Data.Groupe.Builder;
using Fred.Common.Tests.Data.Organisation.Builder;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities;
using Fred.Entities.Groupe;
using Fred.Entities.Societe;

namespace Fred.Common.Tests.Data.Societe.Mock
{
    /// <summary>
    /// Elements fictifs de Societe
    /// </summary>
    public class SocieteMocks
    {
        /// <summary>
        /// Types de société
        /// </summary>
        public enum SocieteType
        {
            Interne = 1,
            Partenaire = 2,
            Sep = 3
        };

        public enum OrgaType
        {
            RZB = 5,
            MBTP = 11
        }

        /// <summary>
        /// Constante du Groupe Razel-Bec
        /// </summary>
        public const int GroupeIdGRZB = 1;
        private readonly GroupeBuilder GroupeBuilder = new GroupeBuilder();
        private readonly OrganisationBuilder OrganisationBuilder = new OrganisationBuilder();

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de SocieteClassificationEnt</returns>
        public FakeDbSet<SocieteEnt> GetFakeDbSet()
        {
            GroupeEnt groupeRZB = GroupeBuilder.RazelBec().Build();
            return new FakeDbSet<SocieteEnt>
            {
                new SocieteEnt{
                    SocieteId = 1,
                    Code = "RB",
                    Libelle = "Razel-Bec",
                    TypeSocieteId = (int)SocieteType.Interne,
                    GroupeId = GroupeIdGRZB,
                    Groupe = groupeRZB,
                    SocieteClassificationId = 1,
                    CodeSocietePaye = "Code 0012A",
                    Active = true,
                    Organisation = OrganisationBuilder.OrganisationId((int)OrgaType.RZB).TypeOrganisationSociete().Build(),
                    AssocieSeps = new List<AssocieSepEnt>()
                },
                new SocieteEnt{
                    SocieteId = 2,
                    Code = "LACH",
                    Libelle = "Lachaux",
                    TypeSocieteId = (int)SocieteType.Interne,
                    GroupeId = GroupeIdGRZB,
                    Groupe = groupeRZB,
                    SocieteClassificationId = 1,
                    CodeSocietePaye = "Code 0012A",
                    Active = true,
                    Organisation = OrganisationBuilder.OrganisationId(6).TypeOrganisationSociete().Build(),
                    AssocieSeps = new List<AssocieSepEnt>()
                },
                new SocieteEnt{
                    SocieteId = 3,
                    Code = "BYCN",
                    Libelle = "Bouygues Construction",
                    TypeSocieteId = (int)SocieteType.Partenaire,
                    GroupeId = GroupeIdGRZB,
                    Groupe = groupeRZB,
                    SocieteClassificationId = 2,
                    CodeSocietePaye = "Code 0012A",
                    Active = true,
                    Organisation = OrganisationBuilder.OrganisationId(7).TypeOrganisationSociete().Build(),
                    AssocieSeps = new List<AssocieSepEnt>()
                },
                new SocieteEnt{
                    SocieteId = 4,
                    Code = "BYFO",
                    Libelle = "Bouygues Fondations",
                    TypeSocieteId = (int)SocieteType.Partenaire,
                    GroupeId = GroupeIdGRZB,
                    Groupe = groupeRZB,
                    SocieteClassificationId = 2,
                    CodeSocietePaye = "Code 0012A",
                    Active = true,
                    Organisation = OrganisationBuilder.OrganisationId(8).TypeOrganisationSociete().Build(),
                    AssocieSeps = new List<AssocieSepEnt>()
                },
                new SocieteEnt{
                    SocieteId = 5,
                    Code = "EFCN",
                    Libelle = "Eiffage Construction",
                    TypeSocieteId = (int)SocieteType.Partenaire,
                    GroupeId = GroupeIdGRZB,
                    Groupe = groupeRZB,
                    SocieteClassificationId = 3,
                    CodeSocietePaye = "Code 0012A",
                    Active = true,
                    Organisation = OrganisationBuilder.OrganisationId(9).TypeOrganisationSociete().Build(),
                    AssocieSeps = new List<AssocieSepEnt>()
                },
                new SocieteEnt{
                    SocieteId = 6,
                    Code = "50338",
                    Libelle = "SEP A",
                    TypeSocieteId = (int)SocieteType.Sep,
                    GroupeId = GroupeIdGRZB,
                    Groupe = groupeRZB,
                    SocieteClassificationId = null,
                    CodeSocietePaye = "Code 0012A",
                    Active = true,
                    Organisation = OrganisationBuilder.OrganisationId(10).TypeOrganisationSociete().Build(),
                    AssocieSeps = new List<AssocieSepEnt>()
                },
                new SocieteEnt{
                    SocieteId = 7,
                    Code = "MBTP",
                    Libelle = "MOULIN BTP",
                    TypeSocieteId = (int)SocieteType.Interne,
                    GroupeId = GroupeIdGRZB,
                    Groupe = groupeRZB,
                    SocieteClassificationId = null,
                    CodeSocietePaye = "Code 0012A",
                    Active = true,
                    Organisation = OrganisationBuilder.OrganisationId((int)OrgaType.MBTP).TypeOrganisationSociete().Build(),
                    AssocieSeps = new List<AssocieSepEnt>()
                }
            };
        }
    }
}
