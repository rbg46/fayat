namespace Fred.Business.RepriseDonnees.Ci.Validators.Results
{
    /// <summary>
    /// ype de regle à verifiée
    /// </summary>
    public enum ImportRuleType
    {
        /// <summary>
        /// Verifie si le ci est dan la base de donnée
        /// </summary>
        CiIsInDatabase,
        /// <summary>
        /// Verification de la regle la societe est dans le groupe
        /// </summary>
        CiSocieteIsInGroupe,
        /// <summary>
        /// Verification de la regle le ci appartient a la societe
        /// </summary>
        CiCiIsInSociete,
        /// <summary>
        /// Verification de la regle 'le code pays est connu'
        /// </summary>
        CiCodePaysUnknow,
        /// <summary>
        /// Verification de la regle 'le code pays de livraison est connu'
        /// </summary>
        CiCodePaysLivraisonUnknow,
        /// <summary>
        /// Verification de la regle 'le code pays de facturation est connu'
        /// </summary>
        CiCodePaysFacturationUnknow,
        /// <summary>
        /// Verification de la regle 'le responsable chantier est connu'
        /// </summary>
        CiResponsableChantierUnknow,
        /// <summary>
        /// Verification de la regle 'le responsable administratif est connu'
        /// </summary>
        CiResponsableAdministratifUnknow,
        /// <summary>
        /// Verification de la regle 'la zone est vrai , faux ou null'
        /// </summary>
        CiZoneModifiableUnknow,
        /// <summary>
        /// Format de la dated'ouverure incorrect
        /// </summary>
        CiDateOuvertureFormatIncorrect,
        /// <summary>
        /// Format de la longitude incorrect
        /// </summary>
        LongitudeFormatIncorrect,
        /// <summary>
        /// Format de la lalitude incorrect
        /// </summary>
        LatitudeFormatIncorrect,
        /// <summary>
        /// Format de la lalitude incorrect
        /// </summary>
        CiLongitudeOrLatitudeMissing,

    }
}
