using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Business.OperationDiverse.ImportODfromExcel.Validators.Results
{
    /// <summary>
    /// type de regle à verifiée
    /// </summary>
    public enum ImportODRuleType
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
        UniteInvalid,
        /// <summary>
        /// Verification de la regle 'le responsable chantier est connu'
        /// </summary>
        DeviseInvalid,
        /// <summary>
        /// Verification de la regle 'le responsable administratif est connu'
        /// </summary>
        CodeFamilleInvalid,
        /// <summary>
        /// Verification de la regle 'le responsable administratif est connu'
        /// </summary>
        CodeRessourceInvalid,
        /// <summary>
        /// Verification de la regle 'la zone est vrai , faux ou null'
        /// </summary>
        CiUnknow,
        /// <summary>
        /// Verification de la regle 'la zone est vrai , faux ou null'
        /// </summary>
        QuantiteFormatIncorrect,
        /// <summary>
        /// Format de la date d'ouverure incorrect
        /// </summary>
        DateComptableFormatIncorrect,
        /// <summary>
        /// Format de la dated'ouverure incorrect
        /// </summary>
        PuHTFormatIncorrect,
        /// <summary>
        /// Format de la dated'ouverure incorrect
        /// </summary>
        TacheNotFound,
        /// <summary>
        /// champ obligatoire
        /// </summary>
        RequiredField,
        /// <summary>
        /// periode clotorée pour le CI
        /// </summary>
        PeriodClosed

    }
}
