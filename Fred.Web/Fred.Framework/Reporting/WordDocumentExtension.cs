using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System.Collections;

namespace Fred.Framework.Reporting
{
  /// <summary>
  /// Méthodes d'extension pour <see cref="WordDocument"/>
  /// </summary>
  public static class WordDocumentExtension
  {
    /// <summary>
    /// Charge un fichier Word.
    /// </summary>
    /// <param name="pathName">Chemin du fichier.</param>
    /// <returns>Le document Word correspondant.</returns>
    public static WordDocument FromFile(string pathName)
    {
      return new WordDocument(pathName, FormatType.Docx);
    }

    /// <summary>
    /// Injecte des données dans un document Word.
    /// </summary>
    /// <param name="document">Le document Word concerné.</param>
    /// <param name="groupName">Nom du type d'objets manipulés.</param>
    /// <param name="list">Liste d'objets à injecter dans le document généré</param>
    public static void Inject(this WordDocument document, string groupName, IEnumerable list)
    {
      //Creates an instance of “MailMergeDataTable” by specifying mail merge group name and “IEnumerable” collection.
      MailMergeDataTable dataTable = new MailMergeDataTable(groupName, list);

      ////Performs Mail merge
      document.MailMerge.ExecuteNestedGroup(dataTable);
    }

    /// <summary>
    /// Ajoute un document Word à la suite.
    /// </summary>
    /// <param name="document">Le document Word source.</param>
    /// <param name="other">Le document Word à ajouter.</param>
    public static void Append(this WordDocument document, WordDocument other)
    {
      // Merging Word documents : https://help.syncfusion.com/file-formats/docio/working-with-word-document#merging-word-documents
      document.ImportContent(other, ImportOptions.UseDestinationStyles);
    }
  }
}
