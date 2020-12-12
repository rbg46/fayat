using System.Collections.Generic;
using System.Diagnostics;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Fournisseur.Input
{
    /// <summary>
    /// Parametre d'entree pour l'import par liste de ci ids
    /// </summary>
    [DebuggerDisplay("FournisseurIds = {FournisseurIds.Count}")]
    public class ImportFournisseurByIdsListInputs
    {
        /// <summary>
        /// Liste de ci a inclure dans le flux
        /// </summary>
        public List<int> FournisseurIds { get; set; } = new List<int>();

    }
}
