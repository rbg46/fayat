using Fred.Entities.CI;
using Fred.Entities.Search;
using System;

namespace Fred.Entities.Referential
{
    /// <summary>
    ///   Représente une recherche de plan de tache
    /// </summary>
    [Serializable]
    public class SearchTacheEnt : AbstractSearch
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un ci
        /// </summary>
        public CILightEnt CI { get; set; }
    }
}
