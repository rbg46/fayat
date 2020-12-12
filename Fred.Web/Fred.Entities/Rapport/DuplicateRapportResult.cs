using System.Collections.Generic;

namespace Fred.Entities.Rapport
{
    /// <summary>
    /// Resultat d'une duplication de rapport.
    /// </summary>
    public class DuplicateRapportResult
    {
        /// <summary>
        /// ctor
        /// </summary>
        public DuplicateRapportResult()
        {
            this.Rapports = new List<RapportEnt>();
        }
        /// <summary>
        /// Permet de savoir si la duplication est dans un mois cloturer
        /// </summary>
        public bool HasDatesInClosedMonth { get; set; }

        /// <summary>
        /// Les Rapports dupliqués
        /// </summary>
        public List<RapportEnt> Rapports { get; set; }

        /// <summary>
        /// Le rapport contients un interimaire sans contrat
        /// </summary>
        public bool HasInterimaireWithoutContrat { get; set; }
        /// <summary>
        /// Permet de savoir si la duplication demander ne se fait que sur un weekend
        /// </summary>
        public bool DuplicationOnlyOnWeekend { get; set; }
        /// <summary>
        /// Permet de savoir si la duplication n'est que sur un personnel inactif sur la periode demandée
        /// </summary>
        public bool HasPersonnelsInactivesOnPeriode { get; set; }

        /// <summary>
        /// Permet de savoir si la duplication est sur une période contenant une zone identique et une zone différente de la période dupliquée
        /// </summary>
        public bool HasPartialDuplicationInDifferentZoneDeTravail { get; set; }

        /// <summary>
        /// Permet de savoir si la duplication ne contenient aucune zone de travail commune
        /// </summary>
        public bool HasAllDuplicationInDifferentZoneDeTravail { get; set; }
    }
}
