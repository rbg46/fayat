namespace Fred.Entities.Rapport
{
    /// <summary>
    /// Etat de la duplication apres le filtrage sur les contrats interimaires
    /// </summary>
    public enum InterimaireDuplicationState
    {
        /// <summary>
        /// Tous les jours demandés sont dupliqués
        /// </summary>
        AllDaysDuplicate = 0,

        /// <summary>
        /// Une partie des jours  sont dupliqués
        /// </summary>
        PartialDuplicate = 1,

        /// <summary>
        /// Aucun jours demandés n'est dupliquer
        /// </summary>
        NothingDayDuplicate = 2,

        /// <summary>
        /// Calcul non applicable
        /// </summary>
        NoApplicable = 3,

        /// <summary>
        /// Tous les pointages ne sont pas dans une période contenant un contrat ayant la même zone de travail
        /// </summary>
        AllDuplicationInDifferentZoneDeTravail = 4,

        /// <summary>
        /// Une partie des pointages ne sont pas dans une période contenant un contrat ayant la même zone de travail
        /// </summary>
        PartialDuplicationInDifferentZoneDeTravail = 5

    }
}
