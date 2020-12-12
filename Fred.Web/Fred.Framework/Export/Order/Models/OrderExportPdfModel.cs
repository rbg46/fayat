namespace Fred.Framework.Export.Order.Models
{
    public class OrderExportPdfModel
    {
        public CommandeExportModel Order { get; set; }
        public CommandeExportOptions ExportOptions { get; set; }
        public string PathCGA { get; set; }
        public string FilePath { get; set; }
    }
}