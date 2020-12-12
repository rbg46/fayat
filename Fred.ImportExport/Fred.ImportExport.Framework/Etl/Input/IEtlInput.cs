using System.Collections.Generic;

namespace Fred.ImportExport.Framework.Etl.Input
{

  /// <summary>
  /// Représente une classe qui contient les données d'entrée de l'ETL
  /// </summary>
  /// <typeparam name="TI">Type des données</typeparam>
  public interface IEtlInput<TI>
  {

    /// <summary>
    /// Type de données contenant le résultat des transformations
    /// </summary>
    IList<TI> Items { get; set; }


    /// <summary>
    /// Appelé par l'ETL
    /// Si besoin, permet de faire un traitement avant d'appeler les transformations
    /// (Ex : Aller chercher les datas dans une base )
    /// </summary>
    void Execute();
  }
}
