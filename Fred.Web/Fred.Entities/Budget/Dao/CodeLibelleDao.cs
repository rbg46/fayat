using System.Diagnostics;

namespace Fred.Entities.Budget.Dao
{
    /// <summary>
    /// Représente un élément composé d'un code et d'un libellé.
    /// </summary>
    [DebuggerDisplay("{Code} - {Libelle}")]
    public class CodeLibelleDao
    {
        /// <summary>
        /// Le code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Le libellé.
        /// </summary>
        public string Libelle { get; set; }
    }
}
