using System;

namespace Fred.ImportExport.Business.ValidationPointage.Fon.Logs
{
    /// <summary>
    /// Interface pour le logger de Fon pour la validation pointage
    /// </summary>
    public interface IValidationPointageFonLogger
    {
        string ErrorInExecuteControleVracWithAnael(Exception e, string query);
        string ErrorInRetrieveAnaelControleVracInfo(Exception e, string query);
        string ErrorInInsertPointagesAndPrimes(Exception e, string currentQuery);
        string ErrorInExecuteRemonteeVracWithAnael(Exception e, string query);
        string ErrorInRetrieveAnaelRemonteeVracErrors(Exception e, string query);
        void ErrorPersonnelNotFoundForAddRemonteeVracErreur(string matricule);
    }
}
