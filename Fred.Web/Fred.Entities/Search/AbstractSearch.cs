using System;

namespace Fred.Entities.Search
{
    /// <summary>
    ///   Classe abstraite utilis� pour les recherches
    /// </summary>
    [Serializable]
    public abstract class AbstractSearch
    {
        /// <summary>
        ///   Obtient ou d�finit la valeur recherch�e
        /// </summary>
        public string ValueText { get; set; }

        /// <summary>
        /// Indique s'il faut chercher la valeur exacte
        /// </summary>
        public virtual bool SearchExactly { get; set; }
    }
}