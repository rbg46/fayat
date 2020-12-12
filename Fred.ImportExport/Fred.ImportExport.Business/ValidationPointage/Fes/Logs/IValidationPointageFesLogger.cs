using System;

namespace Fred.ImportExport.Business.ValidationPointage.Fes.Logs
{
    /// <summary>
    /// Interface pour le logger de fes pour la validation pointage
    /// </summary>
    public interface IValidationPointageFesLogger
    {
        string ErrorInExecuteControleVracWithAnael(Exception e, string query);
        string ErrorInRetrieveAnaelControleVracInfo(Exception e, string query);
        string ErrorInExecuteRemonteeVracWithAnael(Exception e, string query);
        string ErrorInRetrieveAnaelRemonteeVracErrors(Exception e, string query);
        void ErrorPersonnelNotFoundForAddRemonteeVracErreur(string matricule);
    }
}
