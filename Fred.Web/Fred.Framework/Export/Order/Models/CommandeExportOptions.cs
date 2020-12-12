namespace Fred.Framework.Export.Order.Models
{
    public struct CommandeExportOptions
    {
        public bool WithCGV { get; set; }

        public string WaterMark { get; set; }

        public float WaterMarkFontSize { get; set; }
    }
}
