using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Fred.Framework.Export.Order.Models;
using Fred.Framework.Images;
using Fred.Framework.Reporting;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;

namespace Fred.Framework.Export.Order
{
    public static class OrderExportUtils
    {
        private const string GroupName = "CommandeExportModel";

        public static MemoryStream GeneratePdfForOrderWithAmendment(OrderWithAmendmentExportPdfModel model)
        {
            model.Order.ConvertLogoAndSignaturesToImages();

            MemoryStream memoryStream;
            var lignes = model.Order.Lignes;
            var docs = new List<WordDocument>();

            // Charge le document principal qui contientdra la commande, les avenants et les conditions
            // Traitement de la partie commande
            var mainDoc = WordDocumentExtension.FromFile(model.FilePath);
            model.Order.Lignes = lignes.Where(l => l.AvenantNumero == null).ToList();
            // Le formatage se fait ici, pas moyen de le faire fonctionner correctement en le mettant dans le document (\# "...")
            model.Order.MontantBlocHT = model.Order.Lignes.Sum(l => l.MontantHTValue).ToString(model.HtFormatAmount);
            mainDoc.Inject(GroupName, new List<CommandeExportModel> { model.Order });
            FindTagInHeaderAndInsertElementByPath(mainDoc,"#Logo", model.Order.LogoPath);
            docs.Add(mainDoc);

            var avenants = lignes
              .Where(l => l.AvenantNumero != null)
              .OrderBy(l => l.AvenantNumero)
              .GroupBy(l => l.AvenantNumero)
              .ToList();

            // Traitement des avenants
            DocsAmendments(model, lignes, docs, mainDoc, avenants);

            // Les conditions
            if (model.ExportOptions.WithCGV && !string.IsNullOrEmpty(model.PathCGA))
            {
                var conditionsDoc = WordDocumentExtension.FromFile(model.PathCGA);
                docs.Add(conditionsDoc);
                mainDoc.ImportContent(conditionsDoc, ImportOptions.KeepSourceFormatting);
            }

            var pdfFormat = new WordPdfFormat();

            AddWatermark(mainDoc, model.ExportOptions);
            memoryStream = pdfFormat.GeneratePdf(mainDoc);

            // Fermeture de tous les documents ouvert
            // Doit se faire après la génération du PDF
            foreach (var doc in docs)
            {
                doc.Close();
            }

            return memoryStream;
        }

        public static MemoryStream GeneratePdfForOrderWithoutAmendment(OrderExportPdfModel model)
        {
            model.Order.ConvertLogoAndSignaturesToImages();

            MemoryStream memoryStream;

            //Loads an existing Word document
            var wordDocument = WordDocumentExtension.FromFile(model.FilePath);

            //Inject data
            wordDocument.Inject(GroupName, new List<CommandeExportModel> { model.Order });

            FindTagInHeaderAndInsertElementByPath(wordDocument,"#Logo", model.Order.LogoPath);

            WordDocument cgvDocument = null;
            if (model.ExportOptions.WithCGV && !string.IsNullOrEmpty(model.PathCGA))
            {
                cgvDocument = WordDocumentExtension.FromFile(model.PathCGA);
                wordDocument.ImportContent(cgvDocument, ImportOptions.KeepSourceFormatting);
            }

            var pdfFormat = new WordPdfFormat();

            AddWatermark(wordDocument, model.ExportOptions);
            memoryStream = pdfFormat.GeneratePdf(wordDocument);

            wordDocument.Close();
            cgvDocument?.Close();

            return memoryStream;
        }

        public static CommandeExportModel ConvertLogoAndSignaturesToImages(this CommandeExportModel commande)
        {
            if (commande.SignatureByteArray != null)
            {
                commande.Signature = (Image)ImageHelpers.ConvertByteArrayToImage(commande.SignatureByteArray);
            }

            if (commande.LogoPath != null)
            {
                commande.Logo = Image.FromFile(commande.LogoPath);
            }

            return commande;
        }

        private static void AddWatermark(WordDocument wordDocument, CommandeExportOptions options)
        {
            if (!string.IsNullOrEmpty(options.WaterMark))
            {
                //Creates a new text watermark
                TextWatermark textWatermark = new TextWatermark();

                textWatermark.Size = 36;
                if (options.WaterMarkFontSize > 0)
                {
                    textWatermark.Size = options.WaterMarkFontSize;
                }
                textWatermark.FontName = "Arial";
                textWatermark.Layout = WatermarkLayout.Diagonal;
                textWatermark.Semitransparent = true;
                textWatermark.Color = System.Drawing.Color.LightGray;
                textWatermark.Text = options.WaterMark;

                //Sets the created watermark to the document
                wordDocument.Watermark = textWatermark;
            }
        }

        private static void DocsAmendments(OrderWithAmendmentExportPdfModel model,
            List<CommandeLigneExportModel> lignes,
            List<WordDocument> docs,
            WordDocument mainDoc,
            List<IGrouping<int?, CommandeLigneExportModel>> avenants)
        {
            for (var i = 0; i < avenants.Count; i++)
            {
                var templatePath = i < avenants.Count - 1 ? model.AmendmentPrestationPath : model.LastAmendmentPrestationPath;
                var avenant = avenants[i];
                var avenantDoc = WordDocumentExtension.FromFile(AppDomain.CurrentDomain.BaseDirectory + templatePath);
                FindTagInHeaderAndInsertElementByPath(avenantDoc, "#Logo", model.Order.LogoPath);
                docs.Add(avenantDoc);

                model.Order.AvenantNumero = avenant.Key.ToString();
                model.Order.Lignes = lignes.Where(l => l.AvenantNumero == avenant.Key).ToList();
                model.Order.MontantBlocHT = model.Order.Lignes.Sum(l => l.MontantHTValue).ToString(model.HtFormatAmount);
                if (avenant.Any(x => x.AvenantIsDiminution))
                {
                    model.Order.EntetePrestat = string.Format(model.DecreasedAmendmentHeader, model.Order.LibelleCommande.ToUpper());
                }
                else
                {
                    model.Order.EntetePrestat = string.Format(model.AmendmentHeader, model.Order.LibelleCommande.ToUpper());
                }
                avenantDoc.Inject(GroupName, new List<CommandeExportModel> { model.Order });
                mainDoc.ImportContent(avenantDoc, ImportOptions.KeepSourceFormatting);
            }
        }

        private static void FindTagInHeaderAndInsertElementByPath(WordDocument mainDoc, string searchTag, string elementPath)
        {
            var header = mainDoc.Sections[0].HeadersFooters.Header;
            if (header != null)
            {
                InsertElementInTextBody(header, searchTag, elementPath);
            }
        }

        private static void InsertElementInTextBody(WTextBody textBody,string searchTag, string elementPath)
        {
            //Iterates through each of the child items of WTextBody
            for (int i = 0; i < textBody.ChildEntities.Count; i++)
            {
                //IEntity is the basic unit in DocIO DOM. 
                //Accesses the body items (should be either paragraph or table) as IEntity
                IEntity bodyItemEntity = textBody.ChildEntities[i];
                //find Logo in Tables
                if (bodyItemEntity.EntityType.Equals(EntityType.Table))
                {
                    if (InsertElementInTable(bodyItemEntity as WTable, searchTag, elementPath))
                    {
                        return;
                    }
                }
            }
        }
        private static bool InsertElementInTable(WTable table,string searchTag, string elementPath)
        {
            //Iterates the row collection in a table
            foreach (WTableRow row in table.Rows)
            {
                float height = row.Height;
                //Iterates the cell collection in a table row
                foreach (WTableCell cell in row.Cells)
                {
                    //Table cell is derived from (also a) TextBody
                    foreach (WParagraph paragraph in cell.Paragraphs)
                    {
                        //Find searchTag
                        if(InsertElementInTextRange(paragraph, searchTag, elementPath, height, cell.Width))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private static bool InsertElementInTextRange(WParagraph paragraph, string searchTag, string elementPath, float height = 0, float width = 0)
        {
            if (paragraph.ChildEntities != null)
            {
                for (int i = 0; i < paragraph.ChildEntities.Count; i++)
                {
                    Entity entity = paragraph.ChildEntities[i];
                    //A paragraph can have child elements such as text, image, hyperlink, symbols, etc.,
                    //Decides the element type by using EntityType
                    switch (entity.EntityType)
                    {
                        case EntityType.TextRange:
                            {
                                WTextRange textRange = entity as WTextRange;
                                if (textRange.Text == searchTag)
                                {
                                    textRange.Text = string.Empty;
                                    //insert Picture
                                    WPicture picture = (WPicture)paragraph.AppendPicture(Image.FromFile(elementPath));
                                    picture.Width = width;
                                    picture.Height = height;
                                    return true;
                                }
                                break;
                            }
                        case EntityType.Table:
                            {
                                InsertElementInTable(entity as WTable, searchTag, elementPath);
                                break;
                            }
                    }
                }
                return false;
            }
            return false;
        }
    }
}
