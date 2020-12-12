using System;
using System.Text;

namespace Fred.Framework.Extensions
{
    /// <summary>
    /// Class d'extension pour les exceptions.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Permet d'avoir la première d'exception.
        /// </summary>
        /// <param name="exception">L'exception.</param>
        /// <returns>La première exception.</returns>
        public static Exception FirstInnerException(this Exception exception)
        {
            if (exception.InnerException != null)
            {
                exception = exception.InnerException.FirstInnerException();
            }

            return exception;
        }

        /// <summary>
        /// Retourne le message de l'exception ainsi que ceux des exceptions interne.
        /// Le séparateur utilisé entre les messages est Environment.NewLine.
        /// </summary>
        /// <param name="exception">L'exception concernée.</param>
        /// <returns>Le message de l'exception ainsi que ceux des exceptions interne.</returns>
        public static string GetMessages(this Exception exception)
        {
            return exception.GetMessages(Environment.NewLine);
        }

        /// <summary>
        /// Retourne le message de l'exception ainsi que ceux des exceptions interne.
        /// </summary>
        /// <param name="exception">L'exception concernée.</param>
        /// <param name="separator">Le séparateur utilisé entre les messages.</param>
        /// <returns>Le message de l'exception ainsi que ceux des exceptions interne.</returns>
        public static string GetMessages(this Exception exception, string separator)
        {
            var ret = new StringBuilder();
            for (var i = 10; i-- > 0;)
            {
                // Limité à 10 inner exceptions
                if (exception == null)
                {
                    break;
                }

                if (ret.Length > 0)
                {
                    ret.Append(separator);
                }
                ret.Append(exception.Message);
                exception = exception.InnerException;
            }
            return ret.ToString();
        }
    }
}
