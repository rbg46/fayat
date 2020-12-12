using System;

namespace Fred.ImportExport.Business.Log
{
    public abstract class BaseFredIELogger
    {
        private readonly string fluxDirection; // exemple : "IMPORT" "EXPORT" "IMPORT-EXPORT"
        private readonly string codeFlux; // exemple : "PERSONNEL_FES"

        protected BaseFredIELogger(string fluxDirection, string codeFlux)
        {
            if (string.IsNullOrEmpty(fluxDirection))
            {
                throw new ArgumentNullException(nameof(fluxDirection));
            }

            if (string.IsNullOrEmpty(codeFlux))
            {
                throw new ArgumentNullException(nameof(codeFlux));
            }

            this.fluxDirection = fluxDirection;
            this.codeFlux = codeFlux;
        }

        public void Log(string codeLog, string message)
        {
            var warnMessage = $"{CreateMessage(TypeLog.Info, codeLog)} {message}";
            InternalLog(warnMessage);
        }

        public void Log(string codeLog, TypeLog type, string message)
        {
            var warnMessage = $"{CreateMessage(type, codeLog)} {message}";
            InternalLog(warnMessage);
        }

        public void Warn(string codeLog, string message)
        {
            var warnMessage = $"{CreateMessage(TypeLog.Warn, codeLog)} {message}";
            InternalWarn(warnMessage);
        }

        public void Warn(string codeLog, TypeLog type, string message)
        {
            var warnMessage = $"{CreateMessage(type, codeLog)} {message}";
            InternalWarn(warnMessage);
        }

        public string Error(string codeLog, string message)
        {
            var errorMessage = $"{CreateMessage(TypeLog.Warn, codeLog)} {message}";
            InternalError(errorMessage);
            return errorMessage;
        }

        public string Error(string codeLog, TypeLog type, string message)
        {
            var errorMessage = $"{CreateMessage(type, codeLog)} {message}";
            InternalError(errorMessage);
            return errorMessage;
        }

        public string Error(string codeLog, TypeLog type, string message, Exception exception)
        {
            var errorMessage = $"{CreateMessage(type, codeLog)} {message}";
            InternalError(errorMessage, exception);
            return errorMessage;
        }


        public string CreateMessage(TypeLog type, string codeLog)
        {
            return $"[{fluxDirection}][{codeFlux}]{Get(type)}{codeLog}";
        }

        public string GetTechnicalLogErrorMessage(string codeLog)
        {
            return $"[{fluxDirection}][{codeFlux}]{Get(TypeLog.TechnicalError)}{codeLog}";
        }

        public void NotThrowErrorInLog(Action logAction)
        {
            try
            {
                logAction();
            }
            catch
            {
                var error = $"{CreateMessage(TypeLog.TechnicalError, "[TECH-GLOB-001]")} L'erreur n'a pas pu etre loguée.";
                NLog.LogManager.GetCurrentClassLogger().Warn(error);
            }
        }

        public string NotThrowErrorInLog(Func<string> logFunction)
        {
            try
            {
                return logFunction();
            }
            catch
            {
                var error = $"{GetTechnicalLogErrorMessage("[TECH-GLOB-001]")} L'erreur n'a pas pu etre logée.";
                NLog.LogManager.GetCurrentClassLogger().Warn(error);
                return error;
            }
        }

        public string Get(TypeLog typeError)
        {
            var result = string.Empty;
            switch (typeError)
            {
                case TypeLog.Info:
                    result = "[INFO]";
                    break;
                case TypeLog.Critique:
                    result = "[CRITIQUE]";
                    break;
                case TypeLog.Warn:
                    result = "[WARN]";
                    break;
                case TypeLog.Error:
                    result = "[ERROR]";
                    break;
                case TypeLog.BadConfig:
                    result = "[BAD_CONFIG]";
                    break;
                case TypeLog.TechnicalError:
                    result = "[TECKNICAL_LOG_ERROR]";
                    break;
                default:
                    break;
            }
            return result;
        }

        private void InternalLog(string log)
        {
            NLog.LogManager.GetCurrentClassLogger().Info(log);
        }

        private void InternalWarn(string warn)
        {
            NLog.LogManager.GetCurrentClassLogger().Warn(warn);
        }

        private void InternalError(string error)
        {
            NLog.LogManager.GetCurrentClassLogger().Error(error);
        }

        private void InternalError(string error, Exception exception)
        {
            NLog.LogManager.GetCurrentClassLogger().Error(exception, error);
        }
    }
}
