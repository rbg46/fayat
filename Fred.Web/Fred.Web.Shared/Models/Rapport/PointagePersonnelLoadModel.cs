using System.Collections.Generic;
using Fred.Web.Models.CI;
using Fred.Web.Models.Rapport;

namespace Fred.Web.Shared.Models.Rapport
{
    /// <summary>
    /// Représente le modèle de chargement d'un pointage personnel.
    /// </summary>
    public class PointagePersonnelLoadModel<CI> where CI : CIModel
    {
        /// <summary>
        /// La liste des pointages du personnel concerné.
        /// </summary>
        public List<PointagePersonnelModel<CI>> Pointages { get; set; }

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
