namespace Fred.Business.RepriseDonnees.IndemniteDeplacement.Validators.Results
{
    /// <summary>
    /// Type de regle à verifier
    /// </summary>
    public enum IndemniteDeplacementImportRuleType
    {
        /// <summary>
        /// Vérification des champs obligatoires 
        /// </summary>
        RequiredField = 0,

        /// <summary>
        /// Vérification format de la date dernier calcul
        /// </summary>
        DateDernierCalculInvalide = 1,

        /// <summary>
        /// Vérification du format du IVD
        /// </summary>
        IVDInvalide = 2,

        /// <summary>
        /// Vérification du format du champs Saisie Manuelle
        /// </summary>
        SaisieManuelleInvalide = 3,

        /// <summary>
        /// Vérification du Code Déplacement
        /// </summary>
        CodeDeplacementInvalide = 4,

        /// <summary>
        /// Vérification des Societé des Codes Dep
        /// </summary>
        SocieteDesCodesDepInvalide = 5,

        /// <summary>
        /// Vérification du Code Zone Déplacement
        /// </summary>
        CodeZoneDeplacementInvalide = 6,

        /// <summary>
        /// Vérification de la Société du Personnel
        /// </summary>
        SocieteDuPersonnelInvalide = 7,

        /// <summary>
        /// Vérification du Matricule du Personnel
        /// </summary>
        MatriculeDuPersonnelInvalide = 8,

        /// <summary>
        /// Vérification de la Société du CI
        /// </summary>
        SocieteDuCIInvalide = 9,

        /// <summary>
        /// Vérification du Code CI
        /// </summary>
        CodeCIInvalide = 10,

        /// <summary>
        /// Vérification de l'unicité du couple Personnel (via le matricule) + Ci (via son code)
        /// </summary>
        PersonnelAndCINonUnique = 11
    }
}
