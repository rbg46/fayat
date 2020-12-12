using Fred.ImportExport.Framework.Etl.Output;

namespace Fred.ImportExport.Framework.Etl.Engine.Builder
{

  /// <summary>
  /// Permet de construire l'object IEtlConfig de façon fluent
  /// </summary>
  /// <typeparam name="TI">Input Template</typeparam>
  /// <typeparam name="TR">Result Template</typeparam>
  public sealed class EtlBuilderOutput<TI, TR>
  {
    public EtlBuilderOutput(EtlBuilder<TI, TR> etlBuilder)
    {
      EtlBuilder = etlBuilder;
    }

    private EtlBuilder<TI, TR> EtlBuilder { get; }



    /// <summary>
    /// Ajoute un object IEtlOutput à la config
    /// </summary>
    /// <param name="output">IEtlOutput</param>
    /// <returns>La suite de la construction fluent</returns>
    public EtlBuilderOutput<TI, TR> Output(IEtlOutput<TR> output)
    {
      EtlBuilder.Config.OutPuts.Items.Add(output);
      return this;
    }


  }
}