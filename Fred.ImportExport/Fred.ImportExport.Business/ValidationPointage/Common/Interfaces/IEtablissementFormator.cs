using System.Collections.Generic;

namespace Fred.ImportExport.Business.ValidationPointage.Common
{
    /// <summary>
    /// Permet de formater correctement les etablissement pour la requete anael
    /// </summary>
    public interface IEtablissementFormator
    {
        /// <summary>
        ///   Concaténation des codes établissements de paie
        /// </summary>
        /// <param name="etablissementPaieIdList">Liste d'identifiant des établissements de paie</param>
        /// <returns>Concaténation des codes sous forme de chaine de caractères</returns>
        string ConcatEtablissementPaieCode(IEnumerable<int> etablissementPaieIdList);
    }
}
