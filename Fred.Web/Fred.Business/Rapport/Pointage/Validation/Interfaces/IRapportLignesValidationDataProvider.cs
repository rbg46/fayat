using System.Collections.Generic;
using Fred.Entities.Rapport;

namespace Fred.Business.Rapport.Pointage.Validation.Interfaces
{
    /// <summary>
    ///  Service qui fournie les données necessaires pour faire la validation d'une liste de pointages et pour un personnel. 
    /// </summary>
    public interface IRapportLignesValidationDataProvider
    {
        /// <summary>
        /// Recupere les données necessaires pour faire la validation d'une liste de pointages. 
        /// </summary>
        /// <param name="rapportLignes">Les lignes de rapports Lignes</param>        
        /// <returns>Les données necessaires pour deterrminer les message d'erreurs</returns>
        GlobalDataForValidationPointage GetDataForValidateRapportLignes(IEnumerable<RapportLigneEnt> rapportLignes);


        /// <summary>
        /// Recupere les données necessaires pour faire la validation d'un pointage. 
        /// </summary>
        /// <param name="rapportLigne">Le rapportsLigne</param>        
        /// <returns>Les données necessaires pour deterrminer les message d'erreurs</returns>
        GlobalDataForValidationPointage GetDataForValidateRapportLignes(RapportLigneEnt rapportLigne);

    }
}
