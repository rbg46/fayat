using System.Collections.Generic;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities;
using Fred.Entities.Organisation;
using Fred.Entities.Organisation.Tree;
using Fred.Framework.Extensions;

namespace Fred.Common.Tests.Data.Organisation.Mock
{
    public static class OrganisationBaseMocks
    {
        public static List<OrganisationBase> GetOrganisationBases()
        {
            return new List<OrganisationBase>
            {
                new OrganisationBase ()
                 {
                    OrganisationId =1,
                    PereId = null,
                    Id = 1,
                    TypeOrganisationId = OrganisationType.Holding.ToIntValue(),
                },
                new OrganisationBase()
                {
                    OrganisationId = 2,
                    PereId = 1,
                    TypeOrganisationId = OrganisationType.Pole.ToIntValue(),
                },
                new OrganisationBase()
                {
                    OrganisationId = 3,
                    PereId = 2,

                    Id = 1,
                    TypeOrganisationId = OrganisationType.Groupe.ToIntValue(),
                },
                new OrganisationBase()
                {
                    OrganisationId = 4,
                    PereId = 3,

                    Id = 1,
                    TypeOrganisationId = OrganisationType.Societe.ToIntValue(),
                    Affectations =   new List<AffectationBase>(){
                        new AffectationBase
                        {
                            OrganisationId =4,
                            AffectationId =1,
                            RoleId = 1,
                            UtilisateurId = 1,

                        },
                        new AffectationBase
                        {
                            OrganisationId =4,
                            AffectationId =2,
                            RoleId = 2,
                            UtilisateurId = 2,

                        }
                    }
                },
                new OrganisationBase()
                {
                    OrganisationId = 5,
                    PereId = 4,

                    Id = 1,
                    TypeOrganisationId = OrganisationType.Ci.ToIntValue(),
                    Affectations = new List<AffectationBase>(){
                        new AffectationBase
                        {
                            OrganisationId =5,
                            AffectationId =3,
                            RoleId =3,
                            UtilisateurId = 3,

                        },
                        new AffectationBase
                        {
                            OrganisationId =5,
                            AffectationId =4,
                            RoleId =4,
                            UtilisateurId = 4,

                        }
                    }
                },
                new OrganisationBase()
                {
                    OrganisationId = 6,
                    PereId = 4,

                    Id = 2,
                    TypeOrganisationId = OrganisationType.Ci.ToIntValue(),
                    Affectations = new List<AffectationBase>(){
                        new AffectationBase
                        {
                            OrganisationId = 6,
                            AffectationId = 5,
                            RoleId = 3,
                            UtilisateurId = 1,

                        },
                        new AffectationBase
                        {
                            OrganisationId =6,
                            AffectationId =6,
                            RoleId =4,
                            UtilisateurId = 2,

                        }
                    }
                },
                new OrganisationBase()
                {
                    OrganisationId = 7,
                    PereId = 3,

                    Id = 3,
                    TypeOrganisationId = OrganisationType.Ci.ToIntValue(),
                    Affectations = new List<AffectationBase>(){
                        new AffectationBase
                        {
                            OrganisationId = 7,
                            AffectationId = 7,
                            RoleId = 1,
                            UtilisateurId = 1,

                        }
                    }
                }
            };

        }

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de type</returns>
        public static FakeDbSet<TypeOrganisationEnt> GetFakeDbSetTypeOrganisation()
        {
            return new FakeDbSet<TypeOrganisationEnt>
            {
                new TypeOrganisationEnt
                {
                    TypeOrganisationId = 1,
                    Code = Constantes.OrganisationType.CodeHolding,
                    Libelle = "Holding"
                },
                new TypeOrganisationEnt
                {
                    TypeOrganisationId = 2,
                    Code = Constantes.OrganisationType.CodePole,
                    Libelle = "Pôle"
                },
                new TypeOrganisationEnt
                {
                    TypeOrganisationId = 3,
                    Code = Constantes.OrganisationType.CodeGroupe,
                    Libelle = "Groupe"
                },
                new TypeOrganisationEnt
                {
                    TypeOrganisationId = 4,
                    Code = Constantes.OrganisationType.CodeSociete,
                    Libelle = "Société"
                },
                new TypeOrganisationEnt
                {
                    TypeOrganisationId = 5,
                    Code = Constantes.OrganisationType.CodePuo,
                    Libelle = "Périmètre UO"
                },
                new TypeOrganisationEnt
                {
                    TypeOrganisationId = 6,
                    Code = Constantes.OrganisationType.CodeUo,
                    Libelle = "Unité Opérationnelle"
                },
                new TypeOrganisationEnt
                {
                    TypeOrganisationId = 7,
                    Code = Constantes.OrganisationType.CodeEtablissement,
                    Libelle = "Etablissement"
                },
                new TypeOrganisationEnt
                {
                    TypeOrganisationId = 8,
                    Code = Constantes.OrganisationType.CodeCi,
                    Libelle = "Centre d'imputation"
                },
                new TypeOrganisationEnt
                {
                    TypeOrganisationId = 9,
                    Code = Constantes.OrganisationType.CodeSousCi,
                    Libelle = "Sous centre d'imputation"
                }
            };
        }
    };
}
