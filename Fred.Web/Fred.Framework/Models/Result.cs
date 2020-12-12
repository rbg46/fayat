using System.Collections.Generic;
using System.Diagnostics;

namespace Fred.Framework.Models
{
    /// <summary>
    /// Classe qui permet d'encapsuler une resultat et une erreur
    /// </summary>
    /// <typeparam name="T">type de resultat</typeparam>
    public class Result<T>
    {
        private Result() { }
        /// <summary>
        /// Indique si le resultat est ok.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Retourne l'erreur.
        /// </summary>
        public string Error { get; private set; }


        /// <summary>
        /// Retourne l'erreur.
        /// </summary>
        public List<string> Errors { get; private set; }

        /// <summary>
        /// valeur du resultat ok.
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Créer un resultat ok
        /// </summary>
        /// <param name="value">valeur du resultat</param>
        /// <returns>Result</returns>
        public static Result<T> CreateSuccess(T value)
        {
            return new Result<T> { Success = true, Value = value };
        }

        /// <summary>
        /// Créer un resultat ko
        /// </summary>
        /// <param name="error">erreur</param>
        /// <returns>Result</returns>
        public static Result<T> CreateFailure(string error)
        {
            return new Result<T> { Error = error };
        }

        /// <summary>
        /// Créer un resultat ko avec une donnée.
        /// </summary>
        /// <param name="error">erreur</param>
        /// <param name="data">donnée</param>
        /// <returns>Result</returns>
        public static Result<T> CreateFailureWithData(string error, T data)
        {
            return new Result<T> { Error = error, Value = data };
        }

        /// <summary>
        /// Créer un resultat ko avec une donnée.
        /// </summary>
        /// <param name="errors">Liste d'erreurs</param>
        /// <param name="data">donnée</param>
        /// <returns>Result</returns>
        public static Result<T> CreateFailureWithData(List<string> errors, T data)
        {
            foreach (string error in errors)
            {
                Debug.WriteLine(error);
            }
            return new Result<T> { Errors = errors, Value = data };
        }
    }
}
