using Fred.Business.Personnel;
using Fred.Entities.Personnel.Import;
using Fred.Entities.Personnel.Imports;
using Fred.ImportExport.Business.Personnel.Etl.Input;
using Fred.ImportExport.Business.Personnel.Etl.Output;
using Fred.ImportExport.Business.Personnel.Etl.Result;
using Fred.ImportExport.Business.Personnel.Etl.Transform.Custom;
using Fred.ImportExport.Business.Personnel.EtlFactory;
using Fred.ImportExport.Framework.Etl.Engine.Builder;

namespace Fred.ImportExport.Business.Personnel.Etl.Process.Custom
{
    public class PersonnelProcessFon : AbstractBasePersonnelProcess<PersonnelModel, PersonnelAffectationResult>, IPersonnelEtl
    {

        private readonly IImportPersonnelManager importPersonnelFesManager;
        private readonly IPersonnelManager personnelManager;
        private readonly PersonnelEtlParameter parameter;

        public PersonnelProcessFon(IImportPersonnelManager importPersonnelFesManager, IPersonnelManager personnelManager, PersonnelEtlParameter parameter)
        {
            this.importPersonnelFesManager = importPersonnelFesManager;
            this.personnelManager = personnelManager;
            this.parameter = parameter;
        }

        public override void Build()
        {
            var builder = new EtlBuilder<PersonnelModel, PersonnelAffectationResult>(Config);
            builder
              .Input(new PersonnelInputFon(parameter))
              .Transform(new PersonnelTransformFon(parameter, importPersonnelFesManager))
              .Result(new PersonnelResult())
              .Output(new PersonnelFredOutput(personnelManager));
        }
    }
}
