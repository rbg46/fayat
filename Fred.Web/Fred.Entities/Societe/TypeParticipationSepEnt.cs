using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities
{
    /// <summary>
    ///   Représente un type de participation SEP (Gérant, Mandataire, Associé)
    /// </summary>
    public class TypeParticipationSepEnt
    {
        /// <summary>
        ///   Identifiant unique de l'entité
        /// </summary>
        public int TypeParticipationSepId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé
        /// </summary>
        public string Libelle { get; set; }
    }
}
