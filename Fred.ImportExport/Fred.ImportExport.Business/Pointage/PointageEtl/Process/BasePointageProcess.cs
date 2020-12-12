using Fred.Entities.Rapport;


using Fred.ImportExport.Business.Pointage.PointageEtl.Input;
using Fred.ImportExport.Business.Pointage.PointageEtl.Output;
using Fred.ImportExport.Business.Pointage.PointageEtl.Result;
using Fred.ImportExport.Business.Pointage.PointageEtl.Transform;
using Fred.ImportExport.Framework.Etl.Engine.Builder;

namespace Fred.ImportExport.Business.Pointage.PointageEtl.Process
{
    /// <summary>
    /// ETL des pointage personnel vers SAP
    /// </summary>
    public class BasePointageProcess : AbstractBasePointageProcess<RapportLigneEnt, ExportRapportLigneModel>
    {

        public override void Build()
        {
            var builder = new EtlBuilder<RapportLigneEnt, ExportRapportLigneModel>(Config);
            builder
              .Input(new BaseFredPointageInput(this.Parameter, this.EtlExecutionHelper))
              .Transform(new BasePointageTransform(this.Parameter))
              .Result(new BasePointageResult())
              .Output(new BasePointageOutput(this.Parameter, this.EtlExecutionHelper));
        }
    }
}
