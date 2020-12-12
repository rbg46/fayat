using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Rapport.RapportHebdo
{
    /// <summary>
    /// Model de résultat de validation d'un rapport hebdomadaire
    /// </summary>
    public class RapportHebdoValidationReponseViewModel : RapportHebdoSaveResultModel
    {
        /// <summary>
        /// Boolean indiquant si le rapport a été validé
        /// </summary>
        public bool IsValidated { get; set; }

        /// <summary>
        /// La liste des erreurs des pointages des personnels
        /// </summary>
        public Dictionary<int, List<string>> PersonnelErrorList { get; set; }

        /// <summary>
        /// La liste des avertissements sur les pointages des personnels
        /// </summary>
        public Dictionary<int, List<string>> PersonnelWarningList { get; set; }

        /// <summary>
        /// La liste des erreurs sur les rapports journaliers
        /// </summary>
        public Dictionary<int, List<string>> DailyRapportErrorList { get; set; }
    }
}
