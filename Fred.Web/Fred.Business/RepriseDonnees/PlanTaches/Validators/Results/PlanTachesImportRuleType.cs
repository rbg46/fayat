namespace Fred.Business.RepriseDonnees.PlanTaches.Validators.Results
{
    /// <summary>
    /// Type de regle à verifier
    /// </summary>
    public enum PlanTachesImportRuleType
    {
        /// <summary>
        /// Vérification des champs obligatoires 
        /// </summary>
        RequiredField = 0,

        /// <summary>
        /// Vérification de la validité du niveau de tâche
        /// </summary>
        NiveauTacheInvalide = 1,

        /// <summary>
        /// Vérification de la présence du code societé en BDD puis du code CI en BDD (avec le groupe et la société)
        /// </summary>
        CodeCiInvalide = 2,

        /// <summary>
        /// Vérification de la présence ou non Code Tache dans FRED_TACHE pour le CiId
        /// </summary>
        CodeTacheInvalide = 3,

        /// <summary>
        /// Vérification de la cohérence du code tache parent
        /// </summary>
        CodeTacheParentInvalide = 4
    }
}
