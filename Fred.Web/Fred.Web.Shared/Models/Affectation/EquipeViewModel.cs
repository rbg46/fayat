using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Affectation
{
    /// <summary>
    /// Equipe view model class
    /// </summary>
    public class EquipeViewModel
    {
        /// <summary>
        /// List des identifiers des ouvriers ajouter
        /// </summary>
        public List<int> OuvrierListIdToAdd { get; set; }

        /// <summary>
        /// List des identifier des ouvriers a supprimer
        /// </summary>
        public List<int> OuvrierListIdToDelete { get; set; }
    }
}
