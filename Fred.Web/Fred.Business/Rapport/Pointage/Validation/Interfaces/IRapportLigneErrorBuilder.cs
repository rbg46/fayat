using System.Collections.Generic;
using Fred.Entities.Rapport;

namespace Fred.Business.Rapport.Pointage.Validation.Interfaces
{
    /// <summary>
    /// Service qui rajoute les erreurs dans chaque rapportLignes
    /// </summary>
    public interface IRapportLigneErrorBuilder
    {

        /// <summary>
        /// Rajoutes les message d'erreurs a un rapportLigne. UTILISER L AUTRE API SI PLUSIEURS RAPPORT LIGNES !!!!!!!! 
        /// </summary>
        /// <param name="validationGlobalData">validationGlobalData</param>
        /// <param name="rapportLigne">rapportLigne</param>
        void IncludeErrorsMessages(GlobalDataForValidationPointage validationGlobalData, RapportLigneEnt rapportLigne);


        /// <summary>
        /// Rajoutes les message d'erreurs a des rapportLignes
        /// </summary>
        /// <param name="validationGlobalData">validationGlobalData</param>
        /// <param name="rapportLignes">rapportLignes</param>
        void IncludeErrorsMessages(GlobalDataForValidationPointage validationGlobalData, IEnumerable<RapportLigneEnt> rapportLignes);

        /// <summary>
        /// Rajoutes les message d'erreurs a des rapportLignes pour le materiel
        /// </summary>
        /// <param name="rapportLignes">rapportLignes</param>
        void IncludeErrorsMessagesMaterielOnly(IEnumerable<RapportLigneEnt> rapportLignes);
    }
}
