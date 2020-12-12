namespace Fred.ImportExport.Business.ValidationPointage.Common
{
    /// <summary>
    /// Provider des données contextuelles
    /// </summary>
    public interface IValidationPointageContextDataProvider
    {
        /// <summary>
        /// Recupere les données contextuelles pour un controle et une remontée vrac
        /// </summary>
        /// <param name="utilisateurId">utilisateurId</param>
        /// <param name="societeId">societeId</param>
        /// <param name="jobId">jobId</param>
        /// <returns>ControleVracContextData</returns>
        ValidationPointageContextData GetGlobalData(int utilisateurId, int societeId, string jobId);
    }
}
