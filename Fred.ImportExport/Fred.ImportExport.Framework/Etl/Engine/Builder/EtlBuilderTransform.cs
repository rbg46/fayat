using Fred.ImportExport.Framework.Etl.Output;
using Fred.ImportExport.Framework.Etl.Result;
using Fred.ImportExport.Framework.Etl.Transform;

namespace Fred.ImportExport.Framework.Etl.Engine.Builder
{


  /// <summary>
  /// Permet de construire l'object IEtlConfig de façon fluent
  /// </summary>
  /// <typeparam name="TI">Input Template</typeparam>
  /// <typeparam name="TR">Result Template</typeparam>
  public sealed class EtlBuilderTransform<TI, TR>
  {
    public EtlBuilderTransform(EtlBuilder<TI, TR> etlBuilder)
    {
      EtlBuilder = etlBuilder;
    }

    private EtlBuilder<TI, TR> EtlBuilder { get; }


    /// <summary>
    /// Ajoute un object IEtlTransform à la config
    /// </summary>
    /// <param name="transform">IEtlTransform</param>
    /// <returns>La suite de la construction fluent</returns>
    public EtlBuilderTransform<TI, TR> Transform(IEtlTransform<TI, TR> transform)
    {
      EtlBuilder.Config.Transforms.Items.Add(transform);
      return this;
    }


    /// <summary>
    /// Ajoute un object IEtlResult à la config
    /// Un seul possible
    /// </summary>
    /// <param name="result">IEtlResult</param>
    /// <returns>La suite de la construction fluent</returns>
    public EtlBuilderResult<TI, TR> Result(IEtlResult<TR> result)
    {
      EtlBuilder.Config.Result = result;
      return new EtlBuilderResult<TI, TR>(EtlBuilder);
    }

    /// <summary>
    /// Ajoute un object IEtlOutput à la config
    /// </summary>
    /// <param name="output">IEtlOutput</param>
    /// <returns>La suite de la construction fluent</returns>
    public EtlBuilderOutput<TI, TR> Output(IEtlOutput<TR> output)
    {
      EtlBuilder.Config.OutPuts.Items.Add(output);
      return new EtlBuilderOutput<TI, TR>(EtlBuilder);
    }
  }
}