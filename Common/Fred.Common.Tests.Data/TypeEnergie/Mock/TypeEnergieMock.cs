using System.Collections.Generic;
using Fred.Entities.Organisation;
using Fred.Entities.Referential;

namespace Fred.Common.Tests.Data.TypeEnergie.Mock
{
    public static class TypeEnergieMock
    {
        public static List<TypeEnergieEnt> GetAll()
        {
            return new List<TypeEnergieEnt>()
            {
                new TypeEnergieEnt
                    {
                        TypeEnergieId = 1,
                        Code ="P",
                        Libelle ="Personnels"
                    },
                    new TypeEnergieEnt
                    {
                        TypeEnergieId = 2,
                        Code ="M",
                        Libelle ="Matériels"
                    },
                    new TypeEnergieEnt
                    {
                        TypeEnergieId = 3,
                        Code ="I",
                        Libelle ="Intérimaires"
                    },
                    new TypeEnergieEnt
                    {
                        TypeEnergieId = 4,
                        Code ="D",
                        Libelle ="Divers"
                    },
            };
        }
    }
}
