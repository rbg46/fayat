namespace Fred.Business.RepriseDonnees.Personnel.Validators.Results
{
    /// <summary>
    /// Type de regle à verifier
    /// </summary>
    public enum PersonnelImportRuleType
    {
        /// <summary>
        /// Vérification des champs obligatoires 
        /// </summary>
        RequiredField = 0,

        /// <summary>
        /// Vérification de la validité du Type de Personnel
        /// </summary>
        TypePersonnelInvalide = 1,

        /// <summary>
        /// Vérification de la validité du code société
        /// </summary>
        SocieteNotInGroupe = 2,

        /// <summary>
        /// Vérification de la validité de la date d'entrée
        /// </summary>
        DateEntreeInvalide = 3,

        /// <summary>
        /// Vérification de la validité de la date de sortie
        /// </summary>
        DateSortieInvalide = 4,

        /// <summary>
        /// Vérification de la validité du code pays
        /// </summary>
        CodePaysInvalide = 5,

        /// <summary>
        /// Vérification de la validité du code ressource
        /// </summary>
        CodeRessourceInvalide = 6,

        /// <summary>
        /// Vérification de l'unicité du Matricule par Société
        /// </summary>
        MatriculeInvalide = 7,

        /// <summary>
        /// Vérification de le la validité de l'Email
        /// </summary>
        EmailInvalide = 8
    }
}
