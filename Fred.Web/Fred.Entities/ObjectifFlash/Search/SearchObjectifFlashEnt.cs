using System;
using System.Collections.Generic;
using Fred.Entities.CI;

namespace Fred.Entities.ObjectifFlash.Search
{
    /// <summary>
    ///   Représente une recherche d'Objectif Flash
    /// </summary>
    public class SearchObjectifFlashEnt
    {
        /// <summary>
        ///   Obtient ou définit le CI
        /// </summary>
        public CILightEnt CI { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des identifiants de CI de l'utilisateur
        /// </summary>
        public List<int> UserCiIds { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de début de recherche
        /// </summary>
        public DateTime? DateDebut { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de fin de recherche
        /// </summary>
        public DateTime? DateFin { get; set; }

        /// <summary>
        ///   Obtient ou définit le flag de filtre sur les objectif flash cloturés
        /// </summary>
        public bool DisplayClosed { get; set; }
    }
}
