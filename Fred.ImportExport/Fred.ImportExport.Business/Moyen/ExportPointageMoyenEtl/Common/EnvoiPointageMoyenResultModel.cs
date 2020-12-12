namespace Fred.ImportExport.Business.Moyen.ExportPointageMoyenEtl.Common
{
    /// <summary>
    /// Classe qui représente la réponse de l'appel TIBCO
    /// </summary>
    public class EnvoiPointageMoyenResultModel
    {
        /// <summary>
        /// Code retourné par TIBCO = > '0' succés / '-1' erreur
        /// </summary>
        public ExportPointageErrorCode Code { get; set; }

        /// <summary>
        /// Message d'erreur
        /// </summary>
        public string Message { get; set; }
    }
}
