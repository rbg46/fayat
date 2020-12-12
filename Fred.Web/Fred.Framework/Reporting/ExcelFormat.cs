using Fred.Framework.Models.Reporting;
using Syncfusion.ExcelToPdfConverter;
using Syncfusion.Pdf;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

// ReSharper disable PossibleMultipleEnumeration

namespace Fred.Framework.Reporting
{
    /// <summary>
    /// Classe de génération de documents excel 
    /// </summary>
    public class ExcelFormat : IDisposable
    {
        private ITemplateMarkersProcessor markerSheet;
        private readonly string totalHT = "Total H.T";

        /// <summary>
        /// Moteur Excel de SyncFusion
        /// </summary>
        public ExcelEngine ExcelEngine { get; } = new ExcelEngine();

        /// <summary>
        /// Génère un fichier Excel à partir d'un template et le sauvegarde dans le cache du serveur pour une durée de 30 secondes.
        /// </summary>
        /// <typeparam name="T">Type des donnée a sauvegarde(represente un element sur une ligne)</typeparam>
        /// <param name="templatePath">le path du template</param>
        /// <param name="data">les elements a sauvegardé sur la feuille excel de type T</param>
        /// <param name="workbookCustomAction">une action qui permet du customisé le fichier excel</param>
        /// <returns>l'id du cache</returns>
        public string GenerateExcelAndSaveOnServer<T>(string templatePath, IEnumerable<T> data, Action<IWorkbook> workbookCustomAction)
        {
            string pathName = HttpContext.Current.Server.MapPath(templatePath);

            var cacheId = Guid.NewGuid().ToString();

            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(30), Priority = CacheItemPriority.NotRemovable };

            byte[] excelBytes = GenerateExcel(pathName, data, workbookCustomAction);

            MemoryCache.Default.Add("excelBytes_" + cacheId, excelBytes, policy);

            return cacheId;
        }

        /// <summary>
        /// Génère un fichier Excel à partir d'un template
        /// et rempli le fichier avec la liste d'object modelList
        /// </summary>
        /// <typeparam name="T">Le template</typeparam>
        /// <param name="templateFilePath">Fichier template Excel</param>
        /// <param name="modelList">liste d'objet à écrire dans le fichier</param>
        /// <param name="customTranformWorbook">Action qui permet de modifier le wookbook</param>
        /// <param name="buildHeaderModel">Model du header</param>
        /// <returns>renvoie le fichier sous forme de bytes pour pouvoir le mettre dans le cache et l'envoyer au navigateur</returns>
        public byte[] GenerateExcel<T>(string templateFilePath, IEnumerable<T> modelList, Action<IWorkbook> customTranformWorbook = null, BuildHeaderExcelModel buildHeaderModel = null, bool wrapText = false)
        {
            // Open Template
            IWorkbook workbook = OpenTemplateWorksheet(templateFilePath);

            // Fill Template
            PopulateTemplate(workbook, typeof(T).Name, modelList);

            //apply transform on excel file
            customTranformWorbook?.Invoke(workbook);

            if (buildHeaderModel != null)
            {
                foreach (var ws in workbook.Worksheets)
                {
                    BuildHeader(ws, buildHeaderModel);
                }
            }

            if (wrapText)
            {
                foreach (var sheet in workbook.Worksheets)
                {
                    foreach (var row in sheet.Rows)
                    {
                        row.WrapText = true;
                    }
                }
            }

            // retourne le fichier sous forme de bytes
            var data = ConvertToByte(workbook);

            workbook.Close();
            return data;
        }

        /// <summary>
        /// Génère un fichier Excel à partir d'un template
        /// et rempli le fichier avec la liste d'object modelList
        /// </summary>
        /// <typeparam name="T">Le template</typeparam>
        /// <param name="templateFilePath">Fichier template Excel</param>
        /// <param name="modelList">liste d'objet à écrire dans le fichier</param>
        /// <param name="customTranformWorbook">Action qui permet de modifier le wookbook</param>
        /// <param name="repeatedRowHeader">Ligne a répéter sur chaque page</param>
        /// <param name="buildHeaderModel">Model du header</param>
        /// <returns>renvoie le fichier sous forme de bytes pour pouvoir le mettre dans le cache et l'envoyer au navigateur</returns>
        public byte[] GeneratePdfFromExcel<T>(string templateFilePath, IEnumerable<T> modelList, Action<IWorkbook> customTranformWorbook = null, string repeatedRowHeader = null, BuildHeaderExcelModel buildHeaderModel = null)
        {
            // Open Template
            IWorkbook workbook = OpenTemplateWorksheet(templateFilePath);

            // Fill Template
            PopulateTemplate(workbook, typeof(T).Name, modelList);

            //apply transform on excel file
            customTranformWorbook?.Invoke(workbook);

            if (buildHeaderModel != null)
            {
                foreach (var ws in workbook.Worksheets)
                {
                    BuildHeader(ws, buildHeaderModel);
                }
            }

            // Layout
            workbook.ActiveSheet.PageSetup.Orientation = ExcelPageOrientation.Landscape;
            workbook.ActiveSheet.PageSetup.PrintTitleRows = repeatedRowHeader;

            //// Convert excel to PDF
            var converter = new ExcelToPdfConverter(workbook);
            var converterSettings = new ExcelToPdfConverterSettings();
            converterSettings.LayoutOptions = LayoutOptions.FitAllColumnsOnOnePage;
            var pdfDocument = converter.Convert(converterSettings);
            converter.Dispose();

            var stream = new MemoryStream();

            //// Save the document stream
            pdfDocument.Save(stream);

            workbook.Close();

            return stream.ToArray();
        }

        /// <summary>
        /// Génère un fichier Excel mais applique la fonction de transformation AVANT de peupler l'excel
        /// avec les données du model
        /// </summary>
        /// <typeparam name="T">Le template</typeparam>
        /// <param name="templateFilePath">Fichier template Excel</param>
        /// <param name="modelList">liste d'objet à écrire dans le fichier</param>
        /// <param name="customTranformWorbook">Action qui permet de modifier le wookbook</param>
        /// <returns>renvoie le fichier sous forme de bytes pour pouvoir le mettre dans le cache et l'envoyer au navigateur</returns>
        public byte[] GenerateExcelCustomTransformBeforePopulate<T>(string templateFilePath, IEnumerable<T> modelList, Action<IWorkbook> customTranformWorbook = null)
        {
            // Open Template
            IWorkbook workbook = OpenTemplateWorksheet(templateFilePath);

            //apply transform on excel file
            customTranformWorbook?.Invoke(workbook);

            // Fill Template
            PopulateTemplate(workbook, typeof(T).Name, modelList);

            // retourne le fichier sous forme de bytes
            var data = ConvertToByte(workbook);

            workbook.Close();
            return data;
        }

        /// <summary>
        /// Génère un fichier Excel à partir d'un template
        /// et rempli le fichier avec la liste d'object modelList
        /// </summary>
        /// <typeparam name="T">Le template</typeparam>
        /// <param name="templateFilePath">Fichier template Excel</param>
        /// <param name="modelList">liste d'objet à écrire dans le fichier</param>
        /// <param name="destinationPath">Destination du fichier</param>
        /// <returns>renvoie le fichier sous forme de bytes pour pouvoir le mettre dans le cache et l'envoyer au navigateur</returns>
        public byte[] GenerateAndSaveExcel<T>(string templateFilePath, IEnumerable<T> modelList, string destinationPath)
        {
            // Open Template
            IWorkbook workbook = OpenTemplateWorksheet(templateFilePath);

            // Fill Template
            PopulateTemplate(workbook, typeof(T).Name, modelList);

            // Sauvegarde du document 
            workbook.SaveAs(destinationPath);

            // retourne le fichier sous forme de bytes
            var data = ConvertToByte(workbook);

            workbook.Close();

            return data;
        }

        /// <summary>
        /// Initialise un workbook en vue de l'ajout de variables
        /// </summary>
        /// <param name="workbook">worbook à initialisé</param>
        public void InitVariables(IWorkbook workbook)
        {
            markerSheet = workbook.ActiveSheet.CreateTemplateMarkersProcessor();
        }

        /// <summary>
        /// Initialise un workbook en vue de l'ajout de variables
        /// </summary>
        /// <param name="worksheet">worksheet à initialiser</param>
        public void InitVariables(IWorksheet worksheet)
        {
            markerSheet = worksheet.CreateTemplateMarkersProcessor();
        }

        /// <summary>
        /// Ajoute une variable au template
        /// </summary>
        /// <param name="objectName">Nom de la variable passé au template</param>s
        /// <param name="obj">variable passé au template</param>
        public void AddVariable(string objectName, object obj)
        {
            markerSheet.AddVariable(objectName, obj, VariableTypeAction.DetectNumberFormat);
        }

        /// <summary>
        /// Applique les variables au template et charge avec données 
        /// </summary>
        public void ApplyVariables()
        {
            markerSheet.ApplyMarkers();
        }

        /// <summary>
        /// Insère une ligne dans le classeur
        /// </summary>
        /// <param name="workbook">classeur</param>
        /// <param name="indexRow">Index de la ligne à insérer</param>
        public void InsertRowFormatAsBefore(IWorkbook workbook, int indexRow)
        {
            workbook.ActiveSheet.InsertRow(indexRow, 1, ExcelInsertOptions.FormatAsBefore);
        }

        /// <summary>
        /// Affecte une valeur à une cellule d'un workbook
        /// </summary>
        /// <param name="workbook">worbook concerné par la modification de cellule</param>
        /// <param name="indexRow">Index de la ligne de la cellule</param>
        /// <param name="indexColumn">Index de la colonne de la cellule</param>
        /// <param name="value">Valeur à affecter dans la cellule</param>
        public void SetCellValue(IWorkbook workbook, int indexRow, int indexColumn, string value)
        {
            workbook.ActiveSheet.SetValue(indexRow, indexColumn, value);
        }

        /// <summary>
        /// Affecte une formule à une cellule d'un workbook
        /// </summary>
        /// <param name="workbook">worbook concerné par la modification de cellule</param>
        /// <param name="indexRow">Index de la ligne de la cellule</param>
        /// <param name="indexColumn">Index de la colonne de la cellule</param>
        /// <param name="value">Valeur à affecter dans la cellule</param>
        public void SetFormula(IWorkbook workbook, int indexRow, int indexColumn, string value)
        {
            workbook.ActiveSheet.SetFormula(indexRow, indexColumn, value);
        }

        /// <summary>
        /// PrintExcelToPdf
        /// </summary>
        /// <param name="workbook">workbook</param>
        /// <returns>PdfDocument</returns>
        public PdfDocument PrintExcelToPdf(IWorkbook workbook)
        {
            var converter = new ExcelToPdfConverter(workbook);
            var pdfDocument = converter.Convert();
            converter.Dispose();

            return pdfDocument;
        }

        /// <summary>
        /// PrintExcelToPdf avec ajustement sur une page
        /// </summary>
        /// <param name="workbook">workbook</param>
        /// <returns>PdfDocument</returns>
        public PdfDocument PrintExcelToPdfAutoFit(IWorkbook workbook)
        {
            foreach (var sheet in workbook.Worksheets)
            {
                for (int i = 1; i < sheet.UsedRange.Rows.Count(); i++)
                {
                    sheet.UsedRange.Rows[i].AutofitRows();
                }
            }

            var converter = new ExcelToPdfConverter(workbook);
            var converterSettings = new ExcelToPdfConverterSettings();
            converterSettings.LayoutOptions = LayoutOptions.FitAllColumnsOnOnePage;
            PdfDocument pdfDocument = converter.Convert(converterSettings);
            converter.Dispose();
            return pdfDocument;
        }

        /// <summary>
        /// Génère un fichier Excel à partir d'un template
        /// et rempli le fichier avec la liste d'object modelList
        /// </summary>
        /// <typeparam name="T">Le template</typeparam>
        /// <param name="templateFilePath">Fichier template Excel</param>
        /// <param name="modelList">liste d'objet à écrire dans le fichier</param>
        /// <returns>renvoie le fichier sous forme de bytes pour pouvoir le mettre dans le cache et l'envoyer au navigateur</returns>
        public byte[] GenerateExcelDepenses<T>(string templateFilePath, IEnumerable<T> modelList)
        {
            // Open Template
            IWorkbook workbook = OpenTemplateWorksheet(templateFilePath);

            // Fill Template
            PopulateTemplate(workbook, typeof(T).Name, modelList);

            // Insertion de la ligne contenant les totaux
            int numDerLigne = modelList.Count() + 2;
            workbook.Worksheets[0].InsertRow(numDerLigne, 1, ExcelInsertOptions.FormatAsBefore);
            workbook.Worksheets[0].Range["P" + numDerLigne].Text = totalHT;
            workbook.Worksheets[0].Range["Q" + numDerLigne].Formula = "=SUM(L2,L" + (numDerLigne - 1) + ")";

            // retourne le fichier sous forme de bytes
            var data = ConvertToByte(workbook);

            workbook.Close();
            return data;
        }

        /// <summary>
        /// Récupère un fichier Excel Template
        /// </summary>
        /// <returns>Workbook excel</returns>
        public IWorkbook GetNewWorbook()
        {
            IWorkbook workbook = ExcelEngine.Excel.Workbooks.Create();
            workbook.Worksheets.Remove(2);
            workbook.Worksheets.Remove(1);
            workbook.Version = ExcelVersion.Excel2010;
            return workbook;
        }

        /// <summary>
        /// Ouvre un fichier Excel Template
        /// </summary>
        /// <param name="pathName">Le chemin d'accés</param>
        /// <returns>Ouvre un workbook excel</returns>
        public IWorkbook OpenTemplateWorksheet(string pathName)
        {
            IWorkbook workbook = ExcelEngine.Excel.Workbooks.Open(pathName);
            workbook.Version = ExcelVersion.Excel2010;
            workbook.SetSeparators(';', ';');

            return workbook;
        }

        /// <summary>
        /// Ajout multi-feuille
        /// </summary>
        /// <typeparam name="T">Le template</typeparam>
        /// <param name="workbook">Le workbook</param>
        /// <param name="worksheetTemplate">Le worksheetTemplate</param>
        /// <param name="list">La liste</param>
        /// <param name="workSheetName">Le nom de la feuille</param>
        /// <param name="objectName">Le nom de l'objectName</param>
        /// <param name="markerSheets">Le dictionnaire de marker</param>
        /// <returns>Renvoie un IWorkbook</returns>
        public IWorkbook AddWorkSheetTemplate<T>(IWorkbook workbook,
                                                 IWorksheet worksheetTemplate,
                                                 IEnumerable<T> list,
                                                 string workSheetName,
                                                 string objectName,
                                                 Dictionary<string, string> markerSheets = null)
        {
            IWorksheet worksheet = workbook.Worksheets.AddCopy(worksheetTemplate);
            worksheet.View = SheetView.PageLayout;
            worksheet.Name = workSheetName;
            ITemplateMarkersProcessor markerProc = worksheet.CreateTemplateMarkersProcessor();

            if (list != null)
            {
                //Binding the business object with the marker.
                markerProc.AddVariable(objectName, list, VariableTypeAction.DetectNumberFormat);
            }

            if (markerSheets != null)
            {
                foreach (string key in markerSheets.Keys)
                {
                    markerProc.AddVariable(key, markerSheets[key] ?? string.Empty);
                }
            }

            try
            {
                markerProc.ApplyMarkers();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            return workbook;
        }

        /// <summary>
        /// Supprime tous les onglets sauf ceux de la liste passée en paramètre
        /// </summary>
        /// <param name="listWorkSheet">liste passée en paramètre</param>
        /// <param name="workbook">classeur excel</param>
        public void RemoveWorksheetsExcept(List<string> listWorkSheet, IWorkbook workbook)
        {
            bool removing = true;
            while (removing)
            {
                removing = false;
                for (int i = 0; i < workbook.Worksheets.Count; i++)
                {
                    IWorksheet worksheet = workbook.Worksheets[i];
                    if (!listWorkSheet.Contains(worksheet.Name))
                    {
                        workbook.Worksheets.Remove(i);
                        removing = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Méthode de test, pour charger un fichier en mémoire
        /// </summary>
        /// <param name="pathname">chemin du fichier à charger</param>
        /// <returns>objet contenant le fichier chargé en mémoire</returns>
        public MemoryStream ChargerFichier(string pathname)
        {
            MemoryStream ms = new MemoryStream();
            using (FileStream file = new FileStream(pathname, FileMode.Open, FileAccess.Read))
            {
                var byts = new byte[file.Length];
                file.Read(byts, 0, (int)file.Length);
                ms.Write(byts, 0, (int)file.Length);
            }
            return ms;
        }

        /// <summary>
        /// Désallocation du excel après vérification
        /// </summary>
        /// <param name="disposing">Connaitre si le fichier est déjà libéré</param>
        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "<Excel>k__BackingField", Justification = "membre non public")]
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ExcelEngine?.Dispose();
            }
            //// free native resources if there are any.
        }

        /// <summary>
        /// Rempli une page Excel avec une liste d'object en fonction du Template utilisé
        /// </summary>
        /// <typeparam name="T">Le template</typeparam>
        /// <param name="workbook">Le workbook</param>
        /// <param name="modelName">Le nom du modèle</param>
        /// <param name="modelList">La liste de modèles</param>
        private void PopulateTemplate<T>(IWorkbook workbook, string modelName, IEnumerable<T> modelList)
        {
            // Create Template Marker Processor
            ITemplateMarkersProcessor marker = workbook.CreateTemplateMarkersProcessor();

            // Detects number format in DateTable values.
            marker.AddVariable(modelName, modelList, VariableTypeAction.DetectNumberFormat);

            // Process the markers and detect the number format along with the data type in the template.
            marker.ApplyMarkers();

            //Formula calculation is enabled for the sheet.
            workbook.Worksheets[0].EnableSheetCalculations();
        }

        /// <summary>
        /// Converti un workbook en byte
        /// En effet, il ne faut pas utiliser directement GetBuffer()
        /// car il n'y a pas de notion de taille dans GetBuffer, donc quand on flush la mémoire
        /// sur un fichier, si tu as des zeros en mémoire après ton buffer, et bin ton fichier va contenir aussi des zeros
        /// à la fin...
        /// Bref, ne jamais travailler sur un buffer sans en connaître sa taille !
        /// Corrige la WI 1912, c-a-d Fichier excel corrompu.
        /// </summary>
        /// <param name="workbook">Le workbook</param>
        /// <returns>un tableau de byte sans dépassement de mémoire...</returns>
        public byte[] ConvertToByte(IWorkbook workbook)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                workbook.SaveAs(stream);

                stream.Seek(0, SeekOrigin.Begin);
                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
                return bytes;
            }
        }

        /// <summary>
        /// Obtient l'adresse d'une cellule
        /// </summary>
        /// <param name="workbook">Le workbook</param>
        /// <param name="indexRow">Index de la ligne de la cellule</param>
        /// <param name="indexCol">Index de la colonne de la cellule</param>
        /// <returns>L'adresse de la cellule </returns>
        public string GetCellAdress(IWorkbook workbook, int indexRow, int indexCol)
        {
            return workbook.ActiveSheet.Range[indexRow, indexCol].AddressLocal;
        }

        /// <summary>
        /// Sauvegarde sous forme de bytes 
        /// </summary>
        /// <param name="workbook">Nom du fichier excel</param>
        /// <param name="memoryStream">Stream de fichiers</param>
        public void SaveExcelToMemoryStream(IWorkbook workbook, MemoryStream memoryStream)
        {
            workbook.SaveAs(memoryStream);

            workbook.Close();
            ExcelEngine.Dispose();
        }

        /// <summary>
        /// Applique un style aux bordures des cellules d'un workbook
        /// </summary>
        /// <param name="workbook">Le workbook</param>
        /// <param name="range">la plage de cellules</param>
        /// <param name="border">Bordure de la cellule</param>
        /// <param name="style">Style à appliquer</param>
        public void ChangeBorderStyle(IWorkbook workbook, string range, ExcelBordersIndex border, ExcelLineStyle style)
        {
            workbook.ActiveSheet.Range[range].Borders[border].LineStyle = style;
        }

        /// <summary>
        /// Applique une couleur aux cellules d'un workbook
        /// </summary>
        /// <param name="workbook">Le workbook</param>
        /// <param name="range">la plage de cellules</param>
        /// <param name="color">couleur à appliquer aux cellules</param>
        public void ChangeColor(IWorkbook workbook, string range, Color color)
        {
            workbook.ActiveSheet.Range[range].CellStyle.Color = color;
        }

        /// <summary>
        /// Désallocation du excel
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Generate Pdf Or Excel
        /// </summary>
        /// <param name="isPdf">is pdf</param>
        /// <param name="workbook">workbook</param>
        /// <returns>MemoryStream</returns>
        public MemoryStream GeneratePdfOrExcel(bool isPdf, IWorkbook workbook)
        {
            MemoryStream stream = new MemoryStream();
            if (isPdf)
            {
                PdfDocument pdfDoc = this.PrintExcelToPdfAutoFit(workbook);
                string cacheId = Guid.NewGuid().ToString();
                string tempFilename = Path.Combine(Path.GetTempPath(), cacheId + ".pdf");
                pdfDoc.Save(tempFilename);
                stream = this.ChargerFichier(tempFilename);
                File.Delete(tempFilename);
                pdfDoc.Close();
            }
            else
            {
                foreach (var sheet in workbook.Worksheets)
                {
                    sheet.PageSetup.IsFitToPage = true;
                }
                workbook.SaveAs(stream);
            }

            workbook.Close();

            return stream;
        }

        /// <summary>
        /// Ajout multi-feuille
        /// </summary>
        /// <typeparam name="T">Le template</typeparam>
        /// <param name="list">La liste</param>
        /// <param name="objectName">Le nom de l'objectName</param>
        /// <param name="markerProc">Process creation Macker</param>
        /// <param name="markerSheets">Le dictionnaire de marker</param>
        public void AddVariableIntemplate<T>(
                                                 IEnumerable<T> list,
                                                 string objectName,
                                                 ITemplateMarkersProcessor markerProc,
                                                 Dictionary<string, string> markerSheets = null
                                                 )
        {
            if (list != null)
            {
                //Binding the business object with the marker.
                markerProc.AddVariable(objectName, list, VariableTypeAction.DetectNumberFormat);
            }

            if (markerSheets != null)
            {
                foreach (string key in markerSheets.Keys)
                {
                    markerProc.AddVariable(key, markerSheets[key] ?? string.Empty);
                }
            }
        }

        /// <summary>
        /// Ajustement de la hauteur des lignes de détail.
        /// Bug syncfusion lors de la conversion pdf, il est nécessaire de définir les hauteurs de lignes une par une.
        /// </summary>
        /// <param name="workSheet">worksheet</param>
        /// <param name="startIndex">index de départ</param>
        /// <param name="endIndex">index de fin</param>
        public void AutoFitRows(IWorksheet workSheet, int startIndex, int endIndex)
        {
            // Ajustement de la hauteur des lignes de détail
            workSheet.Range[$"A{startIndex}:A{endIndex}"].AutofitRows();
            // applique les hauteurs pour chaque ligne (bug syncfusion)
            for (int index = startIndex; index < endIndex; index++)
            {
                workSheet.SetRowHeight(index, workSheet.Range[$"A{index}"].RowHeight);
            }
        }

        /// <summary>
        /// Construit l'entête d'un export excel
        /// </summary>
        /// <param name="ws">Worksheet à modifier</param>
        /// <param name="buildHeaderModel">Modèle contenant les informations de l'entête</param>
        /// <param name="insertRow">Indique si le header est inséré avant ou placé sur la première ligne</param>
        /// <param name="isPrime">Si concernce les rapports primes</param>
        public void BuildHeader(IWorksheet ws, BuildHeaderExcelModel buildHeaderModel, bool insertRow = true, bool isPrime = false)
        {
            var bestIndexModel = buildHeaderModel.IndexHeaderModel as BestIndexHeaderExcelModel;
            if (bestIndexModel != null)
            {
                ComputeHeaderBestIndexes(ws, bestIndexModel);
            }

            // Insertion de la ligne contenant l'entête de l'édition
            if (insertRow)
            {
                ws.InsertRow(1);
            }
            ws.SetRowHeight(1, 69.44);
            ws.Range[1, 1, 1, buildHeaderModel.IndexHeaderModel.IndexDerniereColonne].CellStyle.Color = Color.FromArgb(0, 86, 160);

            // Fusion de cellules et application de la police
            var titreCells = ws.Range[1, buildHeaderModel.IndexHeaderModel.IndexColonneTitre, 1, buildHeaderModel.IndexHeaderModel.IndexColonneInfos - 1];
            var infosCells = ws.Range[1, buildHeaderModel.IndexHeaderModel.IndexColonneInfos, 1, buildHeaderModel.IndexHeaderModel.IndexColonneLogoFred - 1];

            // Cellules pour le titre
            titreCells.Merge();
            titreCells.CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            titreCells.CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
            titreCells.CellStyle.Font.FontName = "Calibri";
            titreCells.CellStyle.Font.Color = ExcelKnownColors.White;
            titreCells.CellStyle.Font.Size = 14;
            titreCells.CellStyle.Font.Bold = true;

            // Cellules pour les informations
            infosCells.Merge();
            infosCells.CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
            infosCells.CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
            infosCells.CellStyle.Font.FontName = "Calibri";
            infosCells.CellStyle.Font.Color = ExcelKnownColors.White;
            infosCells.CellStyle.Font.Size = 8;

            if (!string.IsNullOrEmpty(buildHeaderModel.PathLogo))
            {
                AddLogoSociete(buildHeaderModel.PathLogo, ws, isPrime);
            }

            // Injection du logo FRED
            string pathLogoFred = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "medias/images/logo/FRED_LOGO_EXPORT_BLUE.png");
            IPictureShape shapeLogoFred = ws.Pictures.AddPicture(1, buildHeaderModel.IndexHeaderModel.IndexColonneLogoFred, pathLogoFred);
            int fixedheightLogoFred = 30;
            decimal ratioLogoFred = (decimal)fixedheightLogoFred / shapeLogoFred.Height;
            shapeLogoFred.Height = fixedheightLogoFred;
            shapeLogoFred.Width = (int)Math.Truncate(shapeLogoFred.Width * ratioLogoFred);
            shapeLogoFred.Top = 30;
            if (bestIndexModel != null)
            {
                var logoFredCells = ws.Range[1, buildHeaderModel.IndexHeaderModel.IndexColonneLogoFred, 1, buildHeaderModel.IndexHeaderModel.IndexDerniereColonne];
                logoFredCells.Merge();
                shapeLogoFred.Left = shapeLogoFred.Left + ws.ColumnWidthToPixels(logoFredCells.Columns.Sum(c => c.ColumnWidth)) - shapeLogoFred.Width - 20;
            }

            // Injection du titre

            if (!string.IsNullOrEmpty(buildHeaderModel.Titre))
            {
                var headerTitle = buildHeaderModel.Titre;
                if (!string.IsNullOrEmpty(buildHeaderModel.SousTitre))
                {
                    headerTitle = headerTitle + "\n" + buildHeaderModel.SousTitre;
                }
                ws.Range[1, buildHeaderModel.IndexHeaderModel.IndexColonneTitre].Text = headerTitle;
            }

            // Injection des informations
            var detailEdition = buildHeaderModel.DateEdition + "\n" + buildHeaderModel.EditePar;
            if (!string.IsNullOrEmpty(buildHeaderModel.InfoSupplementaire))
            {
                detailEdition = detailEdition + "\n" + buildHeaderModel.InfoSupplementaire;
            }
            ws.Range[1, buildHeaderModel.IndexHeaderModel.IndexColonneInfos].Text = detailEdition;

        }

        /// <summary>
        /// Ajoute le logo de la société courante à la feuille de calcule
        /// </summary>
        /// <param name="path">Chemin d'accès au logo</param>
        /// <param name="worksheet">Feuille de calcule</param>
        /// <param name="isPrime">Si concernce les rapports primes</param>
        public void AddLogoSociete(string path, IWorksheet worksheet, bool isPrime = false)
        {
            IPictureShape shapeLogoSociete = worksheet.Pictures.AddPicture(1, 1, path);
            int fixedheight = 60;
            decimal ratio = (decimal)fixedheight / shapeLogoSociete.Height;
            shapeLogoSociete.Height = fixedheight;
            shapeLogoSociete.Width = (int)Math.Truncate(shapeLogoSociete.Width * ratio);

            // Si concerne les rapports primes fixer le width du logo : correction [Bug 12032]
            if (isPrime)
            {
                shapeLogoSociete.Width = worksheet.GetColumnWidthInPixels(1);
            }

            shapeLogoSociete.Left = 5;
            shapeLogoSociete.Top = 15;
        }

        /// <summary>
        /// Calcule les meilleurs index à utiliser pour les champs du header.
        /// </summary>
        /// <param name="worksheet">La page concernée.</param>
        /// <param name="model">Le model qui contient les tailles minimums des champs.</param>
        private void ComputeHeaderBestIndexes(IWorksheet worksheet, BestIndexHeaderExcelModel model)
        {
            model.IndexColonneTitre = GetHeaderBestLastIndex(worksheet, 1, model.LogoSocieteMinWidth) + 1;
            model.IndexColonneInfos = GetHeaderBestLastIndex(worksheet, model.IndexColonneTitre, model.TitreMinWidth) + 1;
            model.IndexColonneLogoFred = GetHeaderBestLastIndex(worksheet, model.IndexColonneInfos, model.InfosMinWidth) + 1;
            model.IndexDerniereColonne = GetHeaderBestLastIndex(worksheet, model.IndexColonneLogoFred, model.LogoFredMinWidth);
            if (model.IndexDerniereColonne < model.MinLastIndex)
            {
                model.IndexDerniereColonne = model.MinLastIndex;
            }
        }

        /// <summary>
        /// Retourne l'index de la colonne qui permet à partir du paramètre start d'avoir au moins minWidth.
        /// </summary>
        /// <param name="worksheet">La page concernée.</param>
        /// <param name="start">L'index de début.</param>
        /// <param name="minWidth">La largeur minimum de la colonne.</param>
        /// <returns>L'index de la colonne.</returns>
        private int GetHeaderBestLastIndex(IWorksheet worksheet, int start, double minWidth)
        {
            double width = 0;
            int index = start;
            while (width < minWidth)
            {
                width += worksheet.Range[1, index++].ColumnWidth;
            }
            return index - 1;
        }

        /// <summary>
        /// recherhce le tag et le remplacer avec le logo
        /// </summary>
        /// <param name="pathCGA">path du fichier logo</param>
        /// <param name="searchTag">recherche du tag</param>
        /// <param name="worksheet">work Sheet</param>
        public void FindLogoAndAddPictureForExcel(string pathCGA, string searchTag, IWorksheet worksheet)
        {
            // Find all with string and specified find option.
            IRange[] targetRangeList = worksheet.FindAll(searchTag, ExcelFindType.Text, ExcelFindOptions.MatchCase);
            if (targetRangeList != null)
            {
                foreach (var targetRange in targetRangeList)
                {
                    int columsWidth = 0;
                    int rowsHeight = 0;
                    //Adding a Picture

                    IPictureShape shapeLogo = worksheet.Pictures.AddPicture(targetRange.Row, targetRange.Column, pathCGA);
                    targetRange.Text = string.Empty;
                    var clonedRange = targetRange;

                    ///test Cellule fusionner
                    if (targetRange.MergeArea != null)
                    {
                        clonedRange = targetRange.MergeArea;
                    }

                    foreach (var row in clonedRange.Rows)
                    {
                        rowsHeight += worksheet.GetRowHeightInPixels(row.Row);
                    }

                    foreach (var col in clonedRange.Columns)
                    {
                        columsWidth += worksheet.GetColumnWidthInPixels(col.Column);
                    }
                    shapeLogo.Width = columsWidth;
                    shapeLogo.Height = rowsHeight;
                    shapeLogo.IsMoveWithCell = false;
                    shapeLogo.IsSizeWithCell = true;
                }
            }
        }
    }
}
