using Fred.ImportExport.Framework.Etl.Transform;

namespace Fred.ImportExport.Framework.Etl.Engine.Builder
{


  /// <summary>
  /// Permet de construire l'object IEtlConfig de façon fluent
  /// </summary>
  /// <typeparam name="TI">Input Template</typeparam>
  /// <typeparam name="TR">Result Template</typeparam>
  public sealed class EtlBuilderInput<TI, TR>
  {
    public EtlBuilderInput(EtlBuilder<TI, TR> etlBuilder)
    {
      EtlBuilder = etlBuilder;
    }

    private EtlBuilder<TI, TR> EtlBuilder { get; }



    /// <summary>
    /// Ajoute l'objet IEtlTransform à la config
    /// </summary>
    /// <param name="transform">IEtlTransform</param>
    /// <returns>La suite de la construction fluent</returns>
    public EtlBuilderTransform<TI, TR> Transform(IEtlTransform<TI, TR> transform)
    {
      EtlBuilder.Config.Transforms.Items.Add(transform);
      return new EtlBuilderTransform<TI, TR>(EtlBuilder);
    }
  }
}