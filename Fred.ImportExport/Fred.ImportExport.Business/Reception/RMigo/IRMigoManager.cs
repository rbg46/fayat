using System.Collections.Generic;
using Fred.ImportExport.Models.Depense;

namespace Fred.ImportExport.Business.Reception.RMigo
{

    /// <summary>
    /// Gestionnaire de la RMIGO
    /// </summary>   
    public interface IRMigoManager
    {
        /// <summary>
        /// Réponse de STORM après export des réceptions (Retour MIGO de SAP)
        /// </summary>
        /// <param name="receptions">Liste des réceptions SAP</param>
        /// <returns>Vrai si l'opération se termine</returns>
        bool ImportRetourReceptionsFromSap(List<ReceptionSapModel> receptions);
    }
}
