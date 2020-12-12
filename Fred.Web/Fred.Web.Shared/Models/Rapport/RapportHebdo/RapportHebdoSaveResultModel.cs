using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Rapport.RapportHebdo
{
    /// <summary>
    /// Représente le résultat de l'enregistrement d'un rapport hebdo.
    /// </summary>
    public class RapportHebdoSaveResultModel
    {
        /// <summary>
        /// Les messages d'alerte.
        /// </summary>
        public List<string> Warnings { get; set; } = new List<string>();

        /// <summary>
        /// Les messages d'erreur.
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();

    }
}
