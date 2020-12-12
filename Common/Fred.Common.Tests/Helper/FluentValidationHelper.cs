using System.Text;
using FluentValidation.Results;

namespace Fred.Common.Tests.Helper
{
    /// <summary>
    /// Fournit des fonctions helper pour FluentValidation
    /// </summary>
    public static class FluentValidationHelper
      {

  

        /// <summary>
        /// Methode d'extention pour ValidationResult, convertie la liste d'erreur en chaine de caractères 
        /// </summary>
        /// <param name="result">Objet courant de type ValidationResult</param>
        /// <returns>liste d'erreurs en chaine de caractères</returns>
        public static string ErrorsToString(this ValidationResult result)
        {
          StringBuilder errors = new StringBuilder();
          foreach (ValidationFailure validationFailure in result.Errors)
          {
            errors.AppendLine("["+validationFailure.PropertyName + "] : " +   validationFailure.ErrorMessage);
          }

          return errors.ToString();
        }
      }
}
