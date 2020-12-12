using System.Diagnostics;

namespace Fred.Business.RepriseDonnees.IndemniteDeplacement.Validators.Results
{
    /// <summary>
    /// Résultat d'une validation de règle
    /// </summary>
    [DebuggerDisplay("ImportRuleType = {ImportRuleType.ToString()} IsValid = {IsValid} ErrorMessage = {ErrorMessage}")]
    public class IndemniteDeplacementImportRuleResult
    {
        /// <summary>
        /// NumeroDeLigne
        /// </summary>
        public int NumeroDeLigne { get; private set; }

        /// <summary>
        /// Permet de savoir si la regle est respectée
        /// </summary>
        public bool IsValid { get; internal set; }

        /// <summary>
        /// Message d'erreur
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Type de regle verifiée
        /// </summary>
        public IndemniteDeplacementImportRuleType ImportRuleType { get; set; }

        /// <summary>
        /// Ajout du numero de ligne
        /// </summary>
        /// <param name="numeroDeLigne">numeroDeLigne</param>
        public void SetNumeroDeLigne(string numeroDeLigne)
        {
            int number = 0;

            // Je ne veux pas que la mise a jour du numero declenche une erreur, d'ou le tryparse
            bool success = int.TryParse(numeroDeLigne, out number);
            if (success)
            {
                NumeroDeLigne = int.Parse(numeroDeLigne);
            }
            else
            {
                NumeroDeLigne = 0;
            }
        }

        /// <summary>
        /// Ajout du numero de ligne
        /// </summary>
        /// <param name="numeroDeLigne">numeroDeLigne</param>
        public void SetNumeroDeLigne(int numeroDeLigne)
        {
            NumeroDeLigne = numeroDeLigne;
        }
    }
}
