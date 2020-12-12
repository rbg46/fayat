using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Params;

namespace Fred.Business.Params
{
    /// <summary>
    /// Gestionnaire des paramétres.
    /// </summary>
    public class ParamsManager : Manager<ParamValueEnt, IParamsRepository>, IParamsManager
    {
        public ParamsManager(IUnitOfWork uow, IParamsRepository paramsRepository)
          : base(uow, paramsRepository)
        {
        }

        /// <summary>
        /// Retourne un paramètre.
        /// </summary>
        /// <param name="organisationId">Identifiant d'organisation.</param>
        /// <param name="key">La clé du paramétere</param>
        /// <returns>le paramètre ou null s'il n'existe pas.</returns>
        public string GetParamValue(int organisationId, string key)
        {
            var param = this.Repository.GetParamValue(organisationId, key);
            return param?.Valeur;
        }

        /// <summary>
        /// return une liste de paramètres 
        /// </summary>
        /// <param name="organisationId">Identifiant d'organisation.</param>
        /// <param name="key">La clé du paramétere</param>
        /// <returns>liste de paramètres ou null s'il n'existe pas.</returns>
        public List<string> GetParamValues(int organisationId, string key)
        {
            var listParam = this.Repository.GetParamValues(organisationId, key);
            if (listParam != null)
            {
                return listParam.Select(o => o.Valeur).ToList();
            }
            return new List<string>();
        }

        /// <summary>
        /// return la liste des paramètres liés à une organisation
        /// </summary>
        /// <param name="organisationId">Identifiant d'organisation.</param>
        /// <returns>liste de paramètres ou null s'il n'existe pas.</returns>
        public List<ParamValueEnt> GetOrganisationParamValues(int organisationId)
        {
            return this.Repository.GetOrganisationParamValues(organisationId);
        }

        /// <summary>
        /// Add or update une liste de paramValues pour une organisation
        /// </summary>
        /// <param name="organisationId">Identifiant d'organisation.</param>
        /// <param name="paramValues">Liste des paramvalues à mettre à jour</param>
        public void AddOrUpdateOrganisationParamValues(int organisationId, Dictionary<string, string> paramValues)
        {
            int currentUserId = Managers.Utilisateur.GetContextUtilisateurId();
            this.Repository.AddOrUpdateParamValues(paramValues, currentUserId, organisationId);
        }

        /// <summary>
        /// Récupere la valeur de la table ParamsValuee
        /// </summary>
        /// <param name="organisationId">organisationId</param>
        /// <param name="keyId">identifiant unique</param>
        /// <returns>value</returns>
        public string GetParamsValueByOrganisationIdAndParamsKeyId(int organisationId, int keyId)
        {
            return this.Repository.GetParamsValueByOrganisationIdAndParamsKeyId(organisationId, keyId);
        }


        /// <summary>
        /// Recupére l'identifiant de la table ParamsKey
        /// </summary>
        /// <param name="libelle">libelle</param>
        /// <returns>Identifiant unique</returns>
        public int GetParamKeyIdByLibelle(string libelle)
        {
            return this.Repository.GetParamKeyIdByLibelle(libelle);
        }

    }
}
