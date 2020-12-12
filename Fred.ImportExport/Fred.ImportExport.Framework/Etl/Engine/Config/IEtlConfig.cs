using Fred.ImportExport.Framework.Etl.Input;
using Fred.ImportExport.Framework.Etl.Output;
using Fred.ImportExport.Framework.Etl.Result;
using Fred.ImportExport.Framework.Etl.Transform;

namespace Fred.ImportExport.Framework.Etl.Engine.Config
{

  /// <summary>
  /// Contient la liste des étapes du workflow de l'ETL
  /// </summary>
  /// <typeparam name="TI">Input Template</typeparam>
  /// <typeparam name="TR">Result Template</typeparam>
  public interface IEtlConfig<TI, TR>
  {
    /// <summary>
    /// Il n'y a qu'une seule entrée possible dans l'Etl
    /// L'object est transporté étape après étape, dans chaque fonction Execute
    /// (si vous en voulez plusieurs: 
    /// soit faire évoluer l'etl, 
    /// soit mettre les entrées dans l'objet Input)
    /// </summary>
    /// <remarks>Obligatoire</remarks>
    IEtlInput<TI> Input { get; set; }

    /// <summary>
    /// Liste des transformations a effectuer
    /// </summary>
    /// <remarks>Obligatoire</remarks>
    IEtlTransforms<TI, TR> Transforms { get; }

    /// <summary>
    /// Contient le resultat des transformations
    /// L'object est transporté étape après étape, dans chaque fonction Execute
    /// </summary>
    /// <remarks>Optionnel</remarks>
    IEtlResult<TR> Result { get; set; }

    /// <summary>
    /// Liste des sorties a effectuer
    /// </summary>
    /// <remarks>Obligatoire</remarks>
    IEtlOutputs<TR> OutPuts { get; }


    /// <summary>
    /// Vérifie si les éléments de la config sont ok pour une bonne exécution du workflow
    /// </summary>
    void ValidateConfig();
  }
}