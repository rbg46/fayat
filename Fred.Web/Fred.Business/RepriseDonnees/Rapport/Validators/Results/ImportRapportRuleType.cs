namespace Fred.Business.RepriseDonnees.Rapport.Validators.Results
{
    /// <summary>
    /// type de regle à verifiée
    /// </summary>
    public enum ImportRapportRuleType
    {
        /// <summary>
        /// Verification de la regle la societe est dans le groupe
        /// </summary>
        CiSocieteIsInGroupe,
        /// <summary>
        /// Verification de la regle le ci appartient a la societe
        /// </summary>
        CiCiIsInSociete,
        /// <summary>
        /// Verification de la regle 'le responsable chantier est connu'
        /// </summary>
        CodeDeplacementUnknow,
        /// <summary>
        /// Verification de la regle 'le responsable chantier est connu'
        /// </summary>
        CodeZoneDeplacementUnknow,
        /// <summary>
        /// Verification de la regle 'le responsable administratif est connu'
        /// </summary>
        PersonnelUnknow,
        /// <summary>
        /// Verification de la regle 'la zone est vrai , faux ou null'
        /// </summary>
        CiUnknow,
        /// <summary>
        /// Verification de la regle 'la zone est vrai , faux ou null'
        /// </summary>
        IVDUnknow,
        /// <summary>
        /// Format de la dated'ouverure incorrect
        /// </summary>
        DateChantierFormatIncorrect,
        /// <summary>
        /// Format de la dated'ouverure incorrect
        /// </summary>
        HeuresTotalFormatIncorrect,
        /// <summary>
        /// Format de la dated'ouverure incorrect
        /// </summary>
        TacheNotFound,
        /// <summary>
        /// champ obligatoire
        /// </summary>
        RequiredField

    }
}
