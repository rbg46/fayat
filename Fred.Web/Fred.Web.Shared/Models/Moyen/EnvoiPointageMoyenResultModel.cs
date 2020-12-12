using Fred.Web.Shared.Enum;

namespace Fred.Web.Shared.Models.Moyen
{
    /// <summary>
    /// Classe qui représente la réponse de l'appel TIBCO
    /// </summary>
    public class EnvoiPointageMoyenResultModel
    {
        /// <summary>
        /// Code retourné par TIBCO = > '0' succés / '1' erreur
        /// </summary>
        public ExportPointageErrorCode Code { get; set; }

        /// <summary>
        /// Message d'erreur
        /// </summary>
        public string Message { get; set; }
    }
}
