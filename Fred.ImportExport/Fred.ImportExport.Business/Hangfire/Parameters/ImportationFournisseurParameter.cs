namespace Fred.ImportExport.Business.Hangfire.Parameters
{
    public class ImportationFournisseurParameter
    {
        public bool BypassDate { get; set; }
        public string CodeSocieteComptable { get; set; }
        public string TypeSequences { get; set; }
        public string RegleGestions { get; set; }
        public bool IsStormOutputDesactivated { get; set; }
    }
}