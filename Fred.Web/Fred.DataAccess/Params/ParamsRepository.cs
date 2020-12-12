using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Params;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Fred.EntityFramework;

namespace Fred.DataAccess.Params
{
    /// <summary>
    ///   Représente un référentiel de données pour les paraméteres.
    /// </summary>
    public class ParamsRepository : FredRepository<ParamValueEnt>, IParamsRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="ParamsRepository" />.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        public ParamsRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        /// Retourne un paramètre.
        /// </summary>
        /// <param name="organisationId">Identifiant d'organisation.</param>
        /// <param name="key">La clé du paramétere</param>
        /// <returns>le paramètre ou null s'il n'existe pas.</returns>
        public ParamValueEnt GetParamValue(int organisationId, string key)
        {
            return Context.ParamValues.FirstOrDefault(p => p.Organisation.OrganisationId == organisationId && p.ParamKey.Libelle == key);
        }

        /// <summary>
        /// return une liste de paramètres 
        /// </summary>
        /// <param name="organisationId">Identifiant d'organisation.</param>
        /// <param name="key">La clé du paramétere</param>
        /// <returns>liste de paramètres ou null s'il n'existe pas.</returns>
        public List<ParamValueEnt> GetParamValues(int organisationId, string key)
        {
            return Context.ParamValues.Where(p => p.Organisation.OrganisationId == organisationId && p.ParamKey.Libelle == key)?.ToList();
        }

        /// <summary>
        /// return la liste des paramètres liés à une organisation
        /// </summary>
        /// <param name="organisationId">Identifiant d'organisation.</param>
        /// <returns>liste de paramètres ou null s'il n'existe pas.</returns>
        public List<ParamValueEnt> GetOrganisationParamValues(int organisationId)
        {
            return Context.ParamValues
                .Include(p => p.ParamKey)
                .Where(p => p.Organisation.OrganisationId == organisationId)?.ToList();
        }
        /// <summary>
        /// Add or update une valeur de paramètre unique pour une organisation
        /// </summary>
        /// <param name="paramValuesDictionnary">dictionnaire de paramvalues</param>
        /// <param name="userId">userId de modification</param>
        /// <param name="organisationId">identifiant de l'organisation</param>
        public void AddOrUpdateParamValues(Dictionary<string, string> paramValuesDictionnary, int userId, int organisationId)
        {
            // récupération des clefs
            var paramKeyList = paramValuesDictionnary.Keys.ToList();

            // update des valeurs existantes
            var paramValues = Context.ParamValues
                                .Include(x => x.ParamKey)
                                .Where(x => x.OrganisationId == organisationId
                                    && paramKeyList.Contains(x.ParamKey.Libelle));
            foreach (var paramValue in paramValues)
            {
                if (paramValue.Valeur != paramValuesDictionnary[paramValue.ParamKey.Libelle])
                {
                    paramValue.Valeur = paramValuesDictionnary[paramValue.ParamKey.Libelle];
                    paramValue.AuteurModificationId = userId;
                    paramValue.DateModification = DateTime.UtcNow;
                }
            }

            var existingParamKeys = paramValues.Select(x => x.ParamKey.Libelle).ToList();
            var newParamValuesDictionnary = paramValuesDictionnary
                                            .Where(x => !existingParamKeys.Contains(x.Key));
            // ajout des nouvelles valeurs
            if (newParamValuesDictionnary.Any())
            {
                var newParamValues = new List<ParamValueEnt>();
                var paramKeyLibelles = newParamValuesDictionnary.Select(x => x.Key);
                var paramKeys = Context.ParamKeys.Where(p => paramKeyLibelles.Contains(p.Libelle)).ToList();
                foreach (var keyValue in newParamValuesDictionnary)
                {
                    var paramKey = paramKeys.SingleOrDefault(x => x.Libelle == keyValue.Key);

                    if (paramKey != null)
                    {
                        newParamValues.Add(new ParamValueEnt
                        {
                            OrganisationId = organisationId,
                            ParamKeyId = paramKey.ParamKeyId,
                            Valeur = keyValue.Value,
                            AuteurModificationId = userId,
                            DateModification = DateTime.UtcNow,
                            AuteurCreationId = userId,
                            DateCreation = DateTime.UtcNow
                        });
                    }
                }
                Context.ParamValues.AddRange(newParamValues);
            }
            this.Context.SaveChanges();
        }

        /// <summary>
        /// Recupére l'identifiant de la table ParamsKey
        /// </summary>
        /// <param name="libelle">libelle</param>
        /// <returns>Identifiant unique</returns>
        public int GetParamKeyIdByLibelle(string libelle)
        {
            return Context.ParamKeys.FirstOrDefault(x => x.Libelle == libelle).ParamKeyId; 
        }
          

        /// <summary>
        /// Récupere la valeur de la table ParamsValuee
        /// </summary>
        /// <param name="organisationId">organisationId</param>
        /// <param name="keyId">identifiant unique</param>
        /// <returns>value</returns>
        public string GetParamsValueByOrganisationIdAndParamsKeyId(int organisationId, int keyId)
        { 
            return Context.ParamValues.FirstOrDefault(x => x.OrganisationId == organisationId && x.ParamKeyId == keyId).Valeur;
        }
           
    }
}
