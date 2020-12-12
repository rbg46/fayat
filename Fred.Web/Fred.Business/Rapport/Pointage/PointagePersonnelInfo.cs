using System.Collections.Generic;
using Fred.Entities.Rapport;

namespace Fred.Business.Rapport.Pointage
{
    /// <summary>
    /// Représente les informations d'un pointage personnel.
    /// </summary>
    public class PointagePersonnelInfo
    {
        /// <summary>
        /// La liste des pointages du personnel concerné.
        /// </summary>
        public List<RapportLigneEnt> Pointages { get; set; }

        /// <summary>
        /// Indique si le code de déplacement est en lecture seule.
        /// </summary>
        public bool CodeDeplacementReadonly { get; set; }

        /// <summary>
        /// Indique si la zone déplacement doit être gérée.
        /// </summary>
        public bool ShowDeplacement { get; set; }

        /// <summary>
        /// Indique si le flag qui informe de la saisie manuelle doit être géré.
        /// </summary>
        public bool ShowSaisieManuelle { get; set; }
    }
}
