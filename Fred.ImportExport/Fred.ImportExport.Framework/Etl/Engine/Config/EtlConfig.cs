using Fred.ImportExport.Framework.Etl.Input;
using Fred.ImportExport.Framework.Etl.Output;
using Fred.ImportExport.Framework.Etl.Result;
using Fred.ImportExport.Framework.Etl.Transform;
using Fred.ImportExport.Framework.Exceptions;

namespace Fred.ImportExport.Framework.Etl.Engine.Config
{

  /// <inheritdoc/>
  public sealed class EtlConfig<TI, TR> : IEtlConfig<TI, TR>
  {
    public IEtlInput<TI> Input { get; set; }
    public IEtlTransforms<TI, TR> Transforms { get; set; } = new EtlTransforms<TI, TR>();
    public IEtlResult<TR> Result { get; set; }
    public IEtlOutputs<TR> OutPuts { get; set; } = new EtlOutputs<TR>();


    public void ValidateConfig()
    {
      if (Input == null)
      {
        throw new FredIeEtlConfigException("Vous devez définir obligatoirement un objet IEtlInput.");
      }

      if (Transforms.Items.Count == 0)
      {
        throw new FredIeEtlConfigException("Vous devez définir obligatoirement un objet IEtlOutput.");
      }

      if (OutPuts.Items.Count == 0)
      {
        throw new FredIeEtlConfigException("Vous devez définir obligatoirement un objet IEtlOutput.");
      }

      // Note :
      // IEtlResult n'est pas obligatoire dans la config, il peut être instancié pendant l'exécution du workflow
    }
  }
}