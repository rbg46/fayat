using System;

namespace Fred.ImportExport.DataAccess.ExternalService.Tibco.Common
{
    /// <summary>
    /// Cette classe représente le retour de TIBCO
    /// </summary>
    public class TibcoReturnResult
    {
        /// <summary>
        /// Le code erreur en string
        /// </summary>
        private readonly string codeErrorString;

        public TibcoReturnResult(string code, string message, Exception exception)
        {
            codeErrorString = code;
            Message = message;
            Exception = exception;
        }

        /// <summary>
        /// Code retourné par TIBCO = > '0' succés / '-1' erreur
        /// </summary>
        public TibcoReturnCode Code => codeErrorString == Constantes.TibcoRetourErrorCode ? TibcoReturnCode.Error : TibcoReturnCode.Success;

        /// <summary>
        /// Message d'erreur
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Exception
        /// </summary>
        public Exception Exception { get; set; }

    }
}
