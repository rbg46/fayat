using System.Collections.Generic;

namespace Fred.Web.Shared.Models.PointagePersonnel
{
    /// <summary>
    /// Représente le résultat de l'enregistrement d'un pointage personnel
    /// </summary>
    public class PointagePersonnelSaveResultModel
    {
        /// <summary>
        /// Les messages d'erreur.
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();
    }
}
