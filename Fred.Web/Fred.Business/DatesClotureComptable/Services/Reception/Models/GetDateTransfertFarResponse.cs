using System;

namespace Fred.Business.DatesClotureComptable.Reception.Models
{
    /// <summary>
    /// Permet de faire une reponse a une requetes pour obtenir la date de transfert FAR
    /// </summary>
    public class GetDateTransfertFarResponse
    {
        /// <summary>
        /// La requete
        /// </summary>
        public GetDateTransfertFarResquest Resquest { get; internal set; }
        /// <summary>
        /// La date obtenue
        /// </summary>
        public DateTime? DateTransfertFar { get; internal set; }
    }
}
