using Fred.ImportExport.Framework.Etl.Engine.Config;
using Fred.ImportExport.Framework.Etl.Input;

namespace Fred.ImportExport.Framework.Etl.Engine.Builder
{

  /// <summary>
  /// Permet de construire l'object IEtlConfig de façon fluent
  /// </summary>
  /// <typeparam name="TI">Input Template</typeparam>
  /// <typeparam name="TR">Result Template</typeparam>
  public sealed class EtlBuilder<TI, TR>
  {
    public EtlBuilder(IEtlConfig<TI, TR> config)
    {
      Config = config;
    }

    public IEtlConfig<TI, TR> Config { get; }


    /// <summary>
    /// Ajoute l'objet IEtlInput à la config
    /// </summary>
    /// <param name="input">IEtlInput</param>
    /// <returns>La suite de la construction fluent</returns>
    public EtlBuilderInput<TI, TR> Input(IEtlInput<TI> input)
    {
      Config.Input = input;
      return new EtlBuilderInput<TI, TR>(this);
    }
  }
}