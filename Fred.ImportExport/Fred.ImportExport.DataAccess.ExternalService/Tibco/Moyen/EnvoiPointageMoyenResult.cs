namespace Fred.ImportExport.DataAccess.ExternalService.Tibco.Moyen
{
    /// <summary>
    /// Cette classe représente le retour de TIBCO
    /// </summary>
    public class EnvoiPointageMoyenResult
    {
        /// <summary>
        /// Le code erreur en string
        /// </summary>
        private readonly string codeErrorString;

        public EnvoiPointageMoyenResult(string code, string message)
        {
            codeErrorString = code;
            Message = message;
        }

        /// <summary>
        /// Code retourné par TIBCO = > '0' succés / '-1' erreur
        /// </summary>
        public ExportPointageErrorCode Code => codeErrorString == Constantes.TibcoRetourErrorCode ?  ExportPointageErrorCode.Error : ExportPointageErrorCode.Success;

        /// <summary>
        /// Message d'erreur
        /// </summary>
        public string Message { get; set; }
    }
}
