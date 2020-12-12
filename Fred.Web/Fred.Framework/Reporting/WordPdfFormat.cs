using System;
using System.Collections;
using System.IO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocToPDFConverter;
using Syncfusion.Pdf;

namespace Fred.Framework.Reporting
{
    /// <summary>
    ///   Classe de génération de documents word et PDF
    /// </summary>
    public class WordPdfFormat
    {
        /// <summary>
        ///   Méthode de génération d'un fichier PDF après conversion d'un word généré
        /// </summary>
        /// <param name="pathName">Chemin d'accès vers le template</param>
        /// <param name="pathCGA">Chemin des CGA</param>
        /// <param name="groupName">Nom du type d'objets manipulés</param>
        /// <param name="list">Liste d'objets à injecter dans le document généré</param>
        /// <returns>Le fichier en flux de mémoire</returns>
        public MemoryStream GeneratePdf(string pathName, string pathCGA, string groupName, IEnumerable list)
        {
            //Loads an existing Word document
            var wordDocument = WordDocumentExtension.FromFile(pathName);

            //Inject data
            wordDocument.Inject(groupName, list);

            if (!string.IsNullOrEmpty(pathCGA))
            {
                var cgvDocument = WordDocumentExtension.FromFile(pathCGA);
                wordDocument.ImportContent(cgvDocument,ImportOptions.KeepSourceFormatting);
            }

            //Generate PDF
            var stream = GeneratePdf(wordDocument);
            wordDocument.Close();
            return stream;
        }

        /// <summary>
        ///   Méthode de génération d'un fichier PDF après conversion d'un word généré
        /// </summary>
        /// <param name="wordDocument">Le document Word concerné</param>
        /// <returns>Le fichier en flux de mémoire</returns>
        public MemoryStream GeneratePdf(WordDocument wordDocument)
        {
            //Creates an instance of the DocToPDFConverter
            DocToPDFConverter converter = new DocToPDFConverter();

            //Sets the jpeg image quality to reduce the Pdf file size
            converter.Settings.ImageQuality = 100;

            //Sets the image resolution
            converter.Settings.ImageResolution = 640;

            //Sets true to optimize the memory usage for identical images
            converter.Settings.OptimizeIdenticalImages = true;

            //Converts Word document into PDF document
            PdfDocument pdfDocument = converter.ConvertToPDF(wordDocument);

            //Creates an instance of memory stream
            MemoryStream stream = new MemoryStream();

            //Save the document stream
            pdfDocument.Save(stream);

            pdfDocument.Close(true);

            return stream;
        }
    }
}
