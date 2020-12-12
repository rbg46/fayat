using System.Collections.Generic;
using Fred.Entities.Organisation;

namespace Fred.Common.Tests.Data.TypeOrganisation.Mock
{
    public static class TypeOrganisationMock
    {
        public static List<TypeOrganisationEnt> GetAll()
        {
            return new List<TypeOrganisationEnt>()
            {
                new TypeOrganisationEnt
                    {
                        TypeOrganisationId = 1,
                        Code ="HOLDING",
                        Libelle ="Holding"
                    },
                    new TypeOrganisationEnt
                    {
                        TypeOrganisationId = 2,
                        Code ="POLE",
                        Libelle ="Pôle"
                    },
                    new TypeOrganisationEnt
                    {
                        TypeOrganisationId = 3,
                        Code ="GROUPE",
                        Libelle ="Groupe"
                    },
                    new TypeOrganisationEnt
                    {
                        TypeOrganisationId = 4,
                        Code ="SOCIETE",
                        Libelle ="Société"
                    },
                    new TypeOrganisationEnt
                    {
                        TypeOrganisationId = 5,
                        Code ="PUO",
                        Libelle ="Périmètre UO"
                    },
                        new TypeOrganisationEnt
                    {
                        TypeOrganisationId = 6,
                        Code ="UO",
                        Libelle ="Unité Opérationnelle"
                    },
                        new TypeOrganisationEnt
                    {
                        TypeOrganisationId = 7,
                        Code ="ETABLISSEMENT",
                        Libelle ="Etablissement"
                    },
                    GetCiType(),
            };
        }

        public static TypeOrganisationEnt GetCiType()
        {
            return new TypeOrganisationEnt
            {
                TypeOrganisationId = 8,
                Code = "CI",
                Libelle = "Centre d'imputation"
            };
        }

    }
}
