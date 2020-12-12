using System.Collections.Generic;
using Fred.Entities.Depense;

namespace Fred.ImportExport.Business.Reception.Migo
{
    /// <summary>
    /// Classe contenant une liste de receptions qui seront envoyés à SAP, 
    /// et une liste de réceptions qu'on n'enverra pas 
    /// </summary>
    public class ReceptionFilterForSap
    {
        /// <summary>
        /// Liste des réceptions valides pour l'envoi à SAP
        /// </summary>
        public List<DepenseAchatEnt> ValidReceptions { get; set; }

        /// <summary>
        /// Liste des réceptions qui ne seront pas envoyées à SAP
        /// </summary>
        public List<DepenseAchatEnt> FilteredReceptions { get; set; }
    }
}
