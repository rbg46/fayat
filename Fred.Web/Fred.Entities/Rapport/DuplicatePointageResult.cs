using System.Collections.Generic;

namespace Fred.Entities.Rapport
{
    /// <summary>
    /// Resultat d'une duplication de pointage.
    /// </summary>
    public class DuplicatePointageResult
    {

        /// <summary>
        /// Resultat d'une duplication d'un pointage
        /// </summary>
        public DuplicatePointageResult()
        {
            DuplicatedRapportLignes = new List<RapportLigneEnt>();
        }

        /// <summary>
        /// Le pointage qui est dupliqué
        /// </summary>
        public RapportLigneEnt PointageToDuplicate { get; set; }

        /// <summary>
        /// Permet de savoir si la duplication est dans un mois cloturer
        /// </summary>
        public bool HasDatesInClosedMonth { get; set; }

        /// <summary>
        /// Les rapport ligne dupliquées.
        /// </summary>
        public List<RapportLigneEnt> DuplicatedRapportLignes { get; set; }

        /// <summary>
        /// Etat de la duplication apres le filtrage sur les contrat interimaire
        /// </summary>
        public InterimaireDuplicationState InterimaireDuplicationState { get; set; }


        /// <summary>
        /// Permet de savoir si la duplication demander ne se fait que sur un weekend
        /// </summary>
        public bool DuplicationOnlyOnWeekend{ get; set; }

        /// <summary>
        /// Permet de savoir si la duplication n'est que sur un personnel inactif sur la periode demandée
        /// </summary>
        public bool PersonnelIsInactiveInPeriode { get; set; }

        /// <summary>
        /// Permet de savoir si la duplication est sur une période contenant une zone identique et une zone différente de la période dupliquée
        /// </summary>
        public bool HasPartialDuplicationInDifferentZoneDeTravail { get; set; }

        /// <summary>
        /// Permet de savoir si la duplication ne contenient aucune zone de travail commune
        /// </summary>
        public bool HasAllDuplicationInDifferentZoneDeTravail { get; set; }

        /// <summary>
        /// Indique si le resultat de la duplication contien des erreurs
        /// </summary>
        /// <returns>true si le resultat de la duplication contien des erreurs</returns>
        public bool HasError()
        {
            return HasDatesInClosedMonth
                || PointageToDuplicate?.ListErreurs?.Count > 0
                || InterimaireDuplicationState == InterimaireDuplicationState.NothingDayDuplicate
                || DuplicationOnlyOnWeekend
                || PersonnelIsInactiveInPeriode;


        }

    }
}
