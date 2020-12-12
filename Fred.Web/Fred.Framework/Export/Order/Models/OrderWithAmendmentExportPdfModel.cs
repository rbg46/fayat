namespace Fred.Framework.Export.Order.Models
{
    public class OrderWithAmendmentExportPdfModel : OrderExportPdfModel
    {
        public string HtFormatAmount { get; set; }
        public string AmendmentPrestationPath { get; set; }
        public string LastAmendmentPrestationPath { get; set; }
        public string DecreasedAmendmentHeader { get; set; }
        public string AmendmentHeader { get; set; }
    }
}
