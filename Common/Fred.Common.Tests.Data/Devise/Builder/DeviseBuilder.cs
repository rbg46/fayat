using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Referential;

namespace Fred.Common.Tests.Data.Devise.Builder
{
    public class DeviseBuilder : ModelDataTestBuilder<DeviseEnt>
    {
        public override DeviseEnt New()
        {
            return new DeviseEnt()
            {
                DeviseId = 48,
                IsoCode = "EUR",
                IsoNombre = "978",
                Symbole = "€",
                Libelle = "Euro",
                CodePaysIso = ""
            };
        }

        public DeviseBuilder Code(string code)
        {
            Model.IsoCode = code;
            return this;
        }
    }
}
