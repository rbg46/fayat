using System.Linq;
using Fred.Common.Tests.Data.Referential.SeuilValidation;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Role;

namespace Fred.Common.Tests.Data.Role.Builder
{
    public class RoleBuilder : ModelDataTestBuilder<RoleEnt>
    {
        private readonly SeuilValidationBuilder BuilderSeuilValidation = new SeuilValidationBuilder();
        public RoleEnt RoleChefChantier()
        {
            return new RoleEnt
            {
                RoleId = 1,
                Code = "CDC",
                Libelle = "CHEF DE CHANTIER",
                CommandeSeuilDefaut = null,
                ModeLecture = false,
                Actif = true,
                NiveauPaie = 1,
                NiveauCompta = 1,
                Description = null,
                SocieteId = 1,
                CodeNomFamilier = "02",
                RoleSpecification = null,
                SeuilsValidation = BuilderSeuilValidation.BuildNObjects(1, BuilderSeuilValidation.SeuilMinimum()).ToList()
            };
        }
    }
}
