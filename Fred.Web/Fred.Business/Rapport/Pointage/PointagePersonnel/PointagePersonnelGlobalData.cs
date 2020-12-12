using System;
using System.Collections.Generic;
using Fred.Entities.Affectation;
using Fred.Entities.DatesClotureComptable;
using Fred.Entities.Rapport;

namespace Fred.Business.Rapport.Pointage.PointagePersonnel
{
    /// <summary>
    /// Model qui contient les informations necessaires pour l'affichage de pointage pour un personnel et un mois donné.
    /// </summary>
    public class PointagePersonnelGlobalData
    {
        /// <summary>
        /// RapportsLignesWithPrimes
        /// </summary>
        public IEnumerable<RapportLigneEnt> RapportsLignesWithPrimes { get; internal set; }
        /// <summary>
        /// CurrentUserCiIds
        /// </summary>
        public List<int> CurrentUserCiIds { get; internal set; }
        /// <summary>
        /// CiIdsOfPointages
        /// </summary>
        public List<int> CiIdsOfRapportLignes { get; internal set; }
        /// <summary>
        /// DatesPointagesOfPointages
        /// </summary>
        public List<DateTime> DatesPointagesOfRapportsLignes { get; internal set; }
        /// <summary>
        /// DatesClotureComptablesForCisOfPointages
        /// </summary>
        public IEnumerable<DatesClotureComptableEnt> DatesClotureComptablesForCisOfPointages { get; internal set; }
        /// <summary>
        /// Astreintes
        /// </summary>
        public IEnumerable<AstreinteEnt> AstreintesOfPersonnelOnCisOnDates { get; internal set; }
        /// <summary>
        /// RapportsTaches
        /// </summary>
        public IEnumerable<RapportTacheEnt> RapportsTachesOfRapportsOfRapportsLignes { get; internal set; }
        /// <summary>
        /// RapportsLignesOnAllRapports
        /// </summary>
        public IEnumerable<RapportLigneEnt> RapportsLignesOnAllRapports { get; internal set; }
        /// <summary>
        /// CurrentUser
        /// </summary>
        public int CurrentUserId { get; internal set; }
        /// <summary>
        /// PersonnelId
        /// </summary>
        public int PersonnelId { get; internal set; }
        /// <summary>
        /// CurrentUserIsInGroupeGFES
        /// </summary>
        public bool CurrentUserIsInGroupeGFES { get; internal set; }

        /// <summary>
        /// Liste de ci ID dont l'utilisateur courant a un role paie.
        /// </summary>
        public List<int> CiIdsOfPointagesWithRolePaieForCurrentUser { get; internal set; }
    }
}
