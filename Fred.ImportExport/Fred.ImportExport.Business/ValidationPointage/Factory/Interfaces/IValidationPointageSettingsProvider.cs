namespace Fred.ImportExport.Business.ValidationPointage.Factory.Interfaces
{

    /// <summary>
    /// Provider qui fournie les settings pour le choix du manager du flux de pointage
    /// </summary>
    public interface IValidationPointageSettingsProvider
    {
        /// <summary>
        /// Retourne les setting necessaire au choix du flux manager en fonction de l'utilisateur qui fait le controle vrac
        /// </summary>
        /// <param name="utilisateurId">utilisateurId</param>
        /// <returns>ValidationPointageFactorySetting</returns>
        ValidationPointageFactorySetting GetFactorySetting(int utilisateurId);
    }
}
