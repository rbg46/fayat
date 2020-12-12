using System;

namespace Fred.Entities.Search
{
    /// <summary>
    ///   Classe abstraite utilisé pour les recherches
    /// </summary>
    [Serializable]
    public abstract class AbstractSearch
    {
        /// <summary>
        ///   Obtient ou définit la valeur recherchée
        /// </summary>
        public string ValueText { get; set; }

        /// <summary>
        /// Indique s'il faut chercher la valeur exacte
        /// </summary>
        public virtual bool SearchExactly { get; set; }
    }
}