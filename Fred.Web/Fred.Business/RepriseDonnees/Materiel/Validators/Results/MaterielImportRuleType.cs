namespace Fred.Business.RepriseDonnees.Materiel.Validators.Results
{
    /// <summary>
    /// Type de regle à verifier
    /// </summary>
    public enum MaterielImportRuleType
    {
        /// <summary>
        /// Vérification des champs obligatoires 
        /// </summary>
        RequiredField = 0,

        /// <summary>
        /// Vérification de la validité du code société
        /// </summary>
        SocieteNotInGroupe = 1,

        /// <summary>
        /// Vérification de la validité du code ressource
        /// </summary>
        CodeRessourceInvalide = 2,

        /// <summary>
        /// Vérification de l'unicité du Code Matériel par Société
        /// </summary>
        CodeMaterielInvalide = 3
    }
}
