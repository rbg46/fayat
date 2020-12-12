using System.Diagnostics;

namespace Fred.Business.DatesClotureComptable.Reception.Models
{
    /// <summary>
    /// Object qui donne une reponse  a une requette pour savoir si le ci est bloqué pour une annee et un mois donnée
    /// </summary>
    [DebuggerDisplay("Resquest = {Resquest} IsBlockedInReception = {IsBlockedInReception}")]
    public class ReceptionBlockedResponse
    {
        /// <summary>
        /// Object qui permet de faire un requette pour savoir qu'elle est la prochaine date disponible en reception
        /// </summary>
        public ReceptionBlockedResquest Resquest { get; set; }

        /// <summary>
        /// La reponse a la requette pour savoir si une reception es bloqué en reception
        /// </summary>
        public bool IsBlockedInReception { get; set; }
    }
}
