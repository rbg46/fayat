using System;
using Fred.ImportExport.Business.Stair.ImportSphinxEtl.Input;
using Fred.ImportExport.Business.Stair.ImportSphinxEtl.Output;
using Fred.ImportExport.Business.Stair.ImportSphinxEtl.Transform;
using Fred.ImportExport.Framework.Etl.Engine;
using Fred.ImportExport.Framework.Etl.Engine.Builder;
using Fred.ImportExport.Models.Stair.Sphinx;

namespace Fred.ImportExport.Business.Stair.ImportSphinxEtl.Process
{
    public class SphinxEtlProcess : EtlProcessBase<SphinxApiModel, SphinxFormulaireModel>
  {
    public override void Build()
    {
      var builder = new EtlBuilder<SphinxApiModel, SphinxFormulaireModel>(Config);
      builder
        .Input(new SphinxEtlInput())
        .Transform(new SphinxEtlTransform())
        .Output(new SphinxEtlOutput());
    }

    protected override void OnError(Exception ex)
    {
      throw new NotImplementedException(ex.Message);
    }

    protected override void OnSuccess()
    {

    }
  }
}
