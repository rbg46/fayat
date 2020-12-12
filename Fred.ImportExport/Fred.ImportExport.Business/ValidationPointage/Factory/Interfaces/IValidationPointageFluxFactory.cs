namespace Fred.ImportExport.Business.ValidationPointage.Factory.Interfaces
{
    /// <summary>
    /// Factory qui selection le flux manager pour la validation pointage
    /// </summary>
    public interface IValidationPointageFluxFactory
    {
        /// <summary>
        ///  Permet de resoudre le gestionnaire métier du flux des lots de pointage pour la validation pointage
        /// </summary>
        /// <param name="utilisateurId">utilisateurId</param>
        /// <returns>IValidationPointageFluxManager</returns>
        IValidationPointageFluxManager GetFluxManager(int utilisateurId);
    }
}
