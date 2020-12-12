using System.Collections.Generic;
using Fred.Entities.Params;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les paraméteres.
    /// </summary>
    public interface IParamsRepository : IRepository<ParamValueEnt>
    {
        /// <summary>
        /// Retourne un paramètre.
        /// </summary>
        /// <param name="organisationId">Identifiant d'organisation.</param>
        /// <param name="key">La clé du paramétere</param>
        /// <returns>le paramètre ou null s'il n'existe pas.</returns>
        ParamValueEnt GetParamValue(int organisationId, string key);

        /// <summary>
        /// return une liste de paramètres 
        /// </summary>
        /// <param name="organisationId">Identifiant d'organisation.</param>
        /// <param name="key">La clé du paramétere</param>
        /// <returns>liste des paramètres ou null s'il n'existe pas.</returns>
        List<ParamValueEnt> GetParamValues(int organisationId, string key);

        /// <summary>
        /// return la liste des paramètres liés à une organisation
        /// </summary>
        /// <param name="organisationId">Identifiant d'organisation.</param>
        /// <returns>liste de paramètres ou null s'il n'existe pas.</returns>
        List<ParamValueEnt> GetOrganisationParamValues(int organisationId);

        /// <summary>
        /// Add or update une valeur de paramètre unique pour une organisation
        /// </summary>
        /// <param name="paramValuesDictionnary">dictionnaire de paramvalues</param>
        /// <param name="userId">userId de modification</param>
        /// <param name="organisationId">identifiant de l'organisation</param>
        void AddOrUpdateParamValues(Dictionary<string, string> paramValuesDictionnary, int userId, int organisationId);

        /// <summary>
        /// Recupére l'identifiant de la table ParamsKey
        /// </summary>
        /// <param name="libelle">libelle</param>
        /// <returns>Identifiant unique</returns>
        int GetParamKeyIdByLibelle(string libelle);

        /// <summary>
        /// Récupere la valeur de la table ParamsValuee
        /// </summary>
        /// <param name="organisationId">organisationId</param>
        /// <param name="keyId">identifiant unique</param>
        /// <returns>value</returns>
        string GetParamsValueByOrganisationIdAndParamsKeyId(int organisationId, int keyId);
    }
}
