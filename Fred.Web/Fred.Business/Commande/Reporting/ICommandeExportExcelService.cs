using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;

namespace Fred.Business.Commande
{
  /// <summary>
  ///  Service pour l'export excel des commandes
  /// </summary>
  public interface ICommandeExportExcelService
  {
    /// <summary>
    ///   Customize le fichier excel contenant la liste des commandes
    /// </summary>
    /// <typeparam name="T">Commande</typeparam>  
    /// <param name="modelList">Liste de commandes</param>
    /// <returns>action de customisation d'un workbook</returns>
    Action<IWorkbook> CustomizeExcelFileForExport<T>(IEnumerable<T> modelList);
  }
}