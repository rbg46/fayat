using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fred.Framework.Debugger
{
    /// <summary>
    /// Permet de comparer 2 objects
    /// </summary>
    public class DebugComparator
    {

        /// <summary>
        ///  Permet de comparer 2 objects
        /// </summary>
        /// <param name="sourceObject">source</param>
        /// <param name="targetObject">target</param>
        /// <returns>Les diffs</returns>
        public static Dictionary<string, JProperty> Compare(object sourceObject, object targetObject)
        {
            var result = new Dictionary<string, JProperty>();
#if DEBUG

            var setting = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string sourceObjectSerialized = JsonConvert.SerializeObject(sourceObject, Formatting.Indented, setting);
            string targetObjectSerialized = JsonConvert.SerializeObject(targetObject, Formatting.Indented, setting);

            JObject sourceJObject = JsonConvert.DeserializeObject<JObject>(sourceObjectSerialized);
            JObject targetJObject = JsonConvert.DeserializeObject<JObject>(targetObjectSerialized);

            result.Add("Source", new JProperty("Source", sourceObjectSerialized));
            result.Add("Target", new JProperty("Target", targetObjectSerialized));

            if (!JToken.DeepEquals(sourceJObject, targetJObject))
            {
                foreach (KeyValuePair<string, JToken> sourceProperty in sourceJObject)
                {
                    JProperty sourceProp = sourceJObject.Property(sourceProperty.Key);
                    JProperty targetProp = targetJObject.Property(sourceProperty.Key);

                    if (!JToken.DeepEquals(sourceProperty.Value, targetProp.Value))
                    {
                        result.Add(sourceProperty.Key + "_Source", sourceProp);
                        result.Add(sourceProperty.Key + "_Target", targetProp);
                    }

                }
            }
#endif
            return result;

        }

        /// <summary>
        /// Compare le 2 object en les serialisant
        /// </summary>
        /// <param name="sourceObject">la source </param>
        /// <param name="targetObject">la cible </param>
        /// <returns>le resultat de la comparaison</returns>
        public CompareResult CompareSerialized(object sourceObject, object targetObject)
        {
            string sourceObjectSerialized = JsonConvert.SerializeObject(sourceObject, Formatting.Indented).Trim();

            string targetObjectSerialized = JsonConvert.SerializeObject(targetObject, Formatting.Indented).Trim();

            if (sourceObjectSerialized != targetObjectSerialized)
            {
                return new CompareResult()
                {
                    //Source = sourceObjectSerialized,
                    //Target = targetObjectSerialized,
                    IsEqual = false
                };
            }
            else
            {
                return new CompareResult()
                {
                    IsEqual = true
                };
            }
        }

        /// <summary>
        /// Log le temps d'execution d'une fonction
        /// </summary>
        /// <typeparam name="T">Le type de retour de la fonction a monitorer</typeparam>
        /// <param name="methodeName">Le nom de la methode</param>
        /// <param name="newFunctionToWatch"> la nouvelle fonction a monitorer</param>
        /// <param name="oldfunctionToWatch"> l'ancienne fonction a monitorer</param>
        /// <param name="logAction">La fonction de log</param>
        /// <returns>Le retour de la fonction</returns>
        public T WatchMany<T>(string methodeName, Func<T> newFunctionToWatch, Func<T> oldfunctionToWatch, Action<string> logAction)
        {
            var newResult = Watch(methodeName,
                                                    "NEW",
                                                    () => newFunctionToWatch(),
                                                    (message) => logAction(message)
                                                    );

            var oldResult = Watch(methodeName,
                                                     "OLD",
                                                     () => oldfunctionToWatch(),
                                                      (message) => logAction(message)
                                                      );

            var isSame = CompareSerialized(newResult, oldResult);

            if (!isSame.IsEqual)
            {
                System.Diagnostics.Debugger.Break();
            }
            return newResult;
        }

        /// <summary>
        /// Log le temps d'execution d'une fonction
        /// </summary>
        /// <typeparam name="T">Le type de retour de la fonction a monitorer</typeparam>
        /// <param name="methodeName">Le nom de la methode</param>
        /// <param name="prefixResult">info dans log</param>
        /// <param name="functionToWatch"> la fonction a monitorer</param>
        /// <param name="logAction">La fonction de log</param>
        /// <returns>Le retour de la fonction</returns>
        public T Watch<T>(string methodeName, string prefixResult, Func<T> functionToWatch, Action<string> logAction)
        {
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            // Do something.
            var result = functionToWatch();

            // Stop timing.
            stopwatch.Stop();

            var message = $"[{methodeName}][{stopwatch.Elapsed}][{prefixResult}_RESULT])";

            Debug.WriteLine(message);

            logAction(message);

            return result;
        }


        /// <summary>
        /// Helper pour concater une liste en chaione de caractere
        /// </summary>
        /// <typeparam name="T">type T</typeparam>
        /// <param name="source">La liste</param>
        /// <param name="delimiter">le delimiter</param>
        /// <returns>La liste en chaine de caractere</returns>
        public string Concatenate<T>(List<T> source, string delimiter)
        {
            var s = new StringBuilder();
            bool first = true;
            foreach (var t in source)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    s.Append(delimiter);
                }
                s.Append(t);
            }
            return s.ToString();
        }



    }

}
