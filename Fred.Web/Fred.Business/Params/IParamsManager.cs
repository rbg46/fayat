using System.Collections.Generic;
using Fred.Entities.Params;

namespace Fred.Business.Params
{
    /// <summary>
    /// Gestionnaire des paramétres.
    /// </summary>
    public interface IParamsManager : IManager<ParamValueEnt>
    {
        /// <summary>
        /// Retourne un paramètre.
        /// </summary>
        /// <param name="organisationId">Identifiant d'organisation.</param>
        /// <param name="key">La clé du paramétere</param>
        /// <returns>le paramètre ou null s'il n'existe pas.</returns>
        string GetParamValue(int organisationId, string key);

        /// <summary>
        /// return une liste de paramètres 
        /// </summary>
        /// <param name="organisationId">Identifiant d'organisation.</param>
        /// <param name="key">La clé du paramétere</param>
        /// <returns>liste de paramètres ou null s'il n'existe pas.</returns>
        List<string> GetParamValues(int organisationId, string key);

        /// <summary>
        /// return la liste des paramètres liés à une organisation
        /// </summary>
        /// <param name="organisationId">Identifiant d'organisation.</param>
        /// <returns>liste de paramètres ou null s'il n'existe pas.</returns>
        List<ParamValueEnt> GetOrganisationParamValues(int organisationId);


        /// <summary>
        /// Add or update une liste de paramValues pour une organisation
        /// </summary>
        /// <param name="organisationId">Identifiant d'organisation.</param>
        /// <param name="paramValues">Liste des paramvalues à mettre à jour</param>
        void AddOrUpdateOrganisationParamValues(int organisationId, Dictionary<string, string> paramValues);

        /// <summary>
        /// Récupere la valeur de la table ParamsValuee
        /// </summary>
        /// <param name="organisationId">organisationId</param>
        /// <param name="keyId">identifiant unique</param>
        /// <returns>value</returns>
        string GetParamsValueByOrganisationIdAndParamsKeyId(int organisationId, int keyId);

        /// <summary>
        /// Recupére l'identifiant de la table ParamsKey
        /// </summary>
        /// <param name="libelle">libelle</param>
        /// <returns>Identifiant unique</returns>
        int GetParamKeyIdByLibelle(string libelle);
    }
}
