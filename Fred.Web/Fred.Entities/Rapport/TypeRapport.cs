namespace Fred.Entities.Rapport
{
    /// <summary>
    ///   ENumération type de rapport
    /// </summary>
    public enum TypeRapport
    {
        /// <summary>
        ///   La journée
        /// </summary>
        Journee = 0,

        /// <summary>
        ///   Le matin
        /// </summary>
        Matin = 1,

        /// <summary>
        ///   L'après midi
        /// </summary>
        ApresMidi = 2
    }

    /// <summary>
    /// Enumération de type de rapport par statut
    /// </summary>
    public enum TypeStatutRapport
    {
        /// <summary>
        /// Pour n'importe quelle autre societe appart Fes
        /// </summary>
        Default = 0,

        /// <summary>
        /// Type Ouvrier
        /// </summary>
        Ouvrier = 1,

        /// <summary>
        /// Type Etam
        /// </summary>
        Etam = 2,

        /// <summary>
        /// Type Iac
        /// </summary>
        Iac = 3
    }
}
