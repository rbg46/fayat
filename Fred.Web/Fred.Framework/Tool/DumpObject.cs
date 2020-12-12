using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Fred.Framework.Tool
{
    /// <summary>
    ///   This static class is used to assist tracing within the solution.
    ///   Credit :
    ///   http://blogs.msdn.com/b/mcsuksoldev/archive/2010/05/04/using-reflection-and-attributes-for-better-tracing-and-logging-data-rendering.aspx?Redirected=true
    /// </summary>
    public static class DumpObject
    {
        /// <summary>
        ///   Null display value.
        /// </summary>
        private const string TracingResourcesNullValue = @"null";

        /// <summary>
        ///   Quote for displaying.
        /// </summary>
        private const string TracingResourcesQuote = "\"";

        /// <summary>
        ///   Seperator for displaying.
        /// </summary>
        private const string TracingResourcesParamSeparator = @" | ";

        /// <summary>
        ///   Unknown Property String Value.
        /// </summary>
        private const string TracingResourcesUnknownValue = "Unknown Property Value";

        /// <summary>
        ///   Add details of a collection of parameters to the supplied log entry.
        /// </summary>
        /// <param name="parameters">Parameters to be described in the log entry.</param>
        /// <returns>String Value.</returns>
        public static string ParameterCollectionToString(object[] parameters)
        {
            // Make sure we have a parameter array which is safe to pass to Array.ConvertAll
            if (parameters == null)
            {
                parameters = new object[] { null };
            }

            // Get a string representation of each parameter that we have been passed
            var paramStrings = Array.ConvertAll(parameters, ParameterObjectToString);

            // Add details of each parameter to log entry
            string allParamStrings = string.Join(TracingResourcesParamSeparator, paramStrings);

            return allParamStrings;
        }

        /// <summary>
        ///   Convert a parameter object to a string for display in the trace.
        /// </summary>
        /// <param name="parameter">Parameter object to convert.</param>
        /// <returns>A string describing the parameter object.</returns>
        public static string ParameterObjectToString(object parameter)
        {
            string paramDesc;

            if (parameter == null)
            {
                paramDesc = TracingResourcesNullValue;
            }
            else
            {
                // Surround string values with quotes
                string s = parameter as string;
                paramDesc = s != null ? string.Concat(TracingResourcesQuote, s, TracingResourcesQuote) : GetObjectString(parameter);
            }

            return paramDesc;
        }

        /// <summary>
        ///   Gets a string representation of an object and items values.
        /// </summary>
        /// <param name="item">Object Item.</param>
        /// <returns>String Value.</returns>
        public static string GetObjectString(object item)
        {
            if (item == null)
            {
                return string.Empty;
            }

            StringBuilder valueString = new StringBuilder();

            // first call ToString and if returns type call GetValues
            valueString.AppendLine(item.ToString());

            // Ajout du type
            Type objectType = item.GetType();
            valueString.Append("Object type = ").AppendLine(objectType.ToString());

            if (!objectType.IsValueType)
            {
                valueString.AppendLine(GetObjectValues(item, true));
            }

            return valueString.ToString();
        }

        /// <summary>
        ///   Formate une chaine de caractère contenant différentes informations sur une exception
        /// </summary>
        /// <param name="exception">L'exception levée</param>
        /// <returns>Une string value</returns>
        public static string SerializeException(Exception exception)
        {
            return SerializeException(exception, string.Empty);
        }

        /// <summary>
        ///   Gets a string representation of an object and items values.
        /// </summary>
        /// <param name="item">Object Item.</param>
        /// <param name="multipleLine">Indicates is the output should be on a multiple lines.</param>
        /// <returns>Une string Value.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Tracing must not thrown an exception.")]
        private static string GetObjectValues(object item, bool multipleLine)
        {
            // check object not null
            if (item == null)
            {
                return string.Empty;
            }

            // get the configuration type
            Type objectType = item.GetType();

            StringBuilder printValue = new StringBuilder();
            AddToStringBuilder(printValue, objectType.Name, multipleLine);

            // look through all the properties for strings (public only) - ensure that the property has a GetProperty
            ForEachPublicStringProperties(item, multipleLine, objectType, printValue);

            // look through all the fields for strings (public only)
            ForEachPublicStringFields(item, multipleLine, objectType, printValue);

            return printValue.ToString();
        }

        private static void ForEachPublicStringFields(object item, bool multipleLine, Type objectType, StringBuilder printValue)
        {
            foreach (FieldInfo fieldInfo in objectType.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                SensitiveInformationAttribute attribute = GetPiiAttribute(fieldInfo);
                if (attribute != null && !attribute.Hidden)
                {
                    string value = GetPiiString(attribute.DisplayOverride, fieldInfo.Name);
                    AddToStringBuilder(printValue, value, multipleLine);
                }
                else
                {
                    string value;
                    try
                    {
                        value = ProcessProperty(fieldInfo.FieldType, fieldInfo.Name, fieldInfo.GetValue(item));
                    }
                    catch (Exception)
                    {
                        value = TracingResourcesUnknownValue;
                    }

                    AddToStringBuilder(printValue, value, multipleLine);
                }
            }
        }

        private static void ForEachPublicStringProperties(object item, bool multipleLine, Type objectType, StringBuilder printValue)
        {
            foreach (PropertyInfo propertyInfo in objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                SensitiveInformationAttribute attribute = GetPiiAttribute(propertyInfo);
                if (attribute != null && !attribute.Hidden)
                {
                    string value = GetPiiString(attribute.DisplayOverride, propertyInfo.Name);
                    AddToStringBuilder(printValue, value, multipleLine);
                }
                else if (propertyInfo.CanRead)
                {
                    // If the property has a get method, write the property value.
                    string value;
                    try
                    {
                        value = ProcessProperty(propertyInfo.PropertyType, propertyInfo.Name, propertyInfo.GetValue(item, null));
                    }
                    catch (Exception)
                    {
                        value = TracingResourcesUnknownValue;
                    }

                    AddToStringBuilder(printValue, value, multipleLine);

                }
            }
        }

        /// <summary>
        ///   Gets the PersonallyIdentifiableInformation Attribute.
        /// </summary>
        /// <param name="member">Member Information.</param>
        /// <returns>Personally Identifiable Information Attribute.</returns>
        private static SensitiveInformationAttribute GetPiiAttribute(MemberInfo member)
        {
            SensitiveInformationAttribute piiAttribute = null;

            foreach (Attribute attribute in member.GetCustomAttributes(typeof(SensitiveInformationAttribute), false))
            {
                piiAttribute = attribute as SensitiveInformationAttribute;
                if (piiAttribute != null)
                {
                    break;
                }
            }

            return piiAttribute;
        }

        /// <summary>
        ///   Gets the string from the Attribute name.
        /// </summary>
        /// <param name="displayOverride">The display override.</param>
        /// <param name="memberName">The member name.</param>
        /// <returns>String value to display.</returns>
        private static string GetPiiString(string displayOverride, string memberName)
        {
            return string.Format(CultureInfo.CurrentCulture, "{0} = {1}", memberName, displayOverride ?? string.Empty);
        }

        /// <summary>
        ///   Appends the given string to the given builder.
        /// </summary>
        /// <param name="builder">String Builder.</param>
        /// <param name="value">String Value.</param>
        /// <param name="multipleLine">Multiple line indicator.</param>
        private static void AddToStringBuilder(StringBuilder builder, string value, bool multipleLine)
        {
            if (value != null)
            {
                if (multipleLine)
                {
                    builder.AppendLine(value);
                }
                else
                {
                    builder.Append(string.Concat(value, "; "));
                }
            }
        }

        /// <summary>
        ///   Returns a string from an object Property/Field.
        /// </summary>
        /// <param name="propertyType">Property/Field type.</param>
        /// <param name="propertyName">Property/Field name.</param>
        /// <param name="propertyValue">Property/Field value.</param>
        /// <returns>String of the Property/Field.</returns>
        private static string ProcessProperty(Type propertyType, string propertyName, object propertyValue)
        {
            string value = null;

            if (propertyValue == null)
            {
                value = string.Format(CultureInfo.CurrentCulture, "{0} = {1}", propertyName, TracingResourcesNullValue);
            }
            else
            {
                if (propertyType == typeof(string))
                {
                    // see if underlying type is a string and persist the value
                    // get the value and ensure not null
                    string objectValue = propertyValue as string;
                    if (!string.IsNullOrEmpty(objectValue))
                    {
                        value = string.Format(CultureInfo.CurrentCulture, "{0} = {2}{1}{2}", propertyName, objectValue, TracingResourcesQuote);
                    }
                }
                else if (propertyType.IsEnum)
                {
                    // look for enum types and persist the value
                    value = string.Format(CultureInfo.CurrentCulture, "Enum {0} = {1}", propertyName, Enum.GetName(propertyType, propertyValue));
                }
                else if (propertyType.IsValueType)
                {
                    // look for other value type
                    value = string.Format(CultureInfo.CurrentCulture, "{0} = {1}", propertyName, propertyValue.ToString());
                }
                else
                {
                    // reference type so return the type name
                    value = string.Format(CultureInfo.CurrentCulture, "{0} Type = {1}", propertyName, propertyType.Name);
                }
            }

            return value;
        }

        /// <summary>
        ///   Fonction récursive qui formate le message d'une exception, son StackTrace et les InnerException éventuelles
        /// </summary>
        /// <param name="e">L'exception levée</param>
        /// <param name="exceptionMessage">Le message de l'exception levée</param>
        /// <returns>Un string</returns>
        private static string SerializeException(Exception e, string exceptionMessage)
        {
            if (e == null)
            {
                return string.Empty;
            }

            StringBuilder txt = new StringBuilder();

            // reprise du texte précédent (récursif)
            if (!string.IsNullOrEmpty(exceptionMessage))
            {
                txt.AppendLine(exceptionMessage);
            }

            if (string.IsNullOrEmpty(exceptionMessage))
            {
                txt.AppendLine("== Exception == ");
            }
            else
            {
                txt.AppendLine("== Inner Exception ==");
            }

            txt.Append("Message     = ").AppendLine(e.Message);
            txt.Append("Type        = ").AppendLine(e.GetType().FullName);
            txt.Append("Source      = ").AppendLine(e.Source);
            txt.Append("Target      = ").AppendLine(e.TargetSite?.Name);
            txt.Append("Help link   = ").AppendLine(e.HelpLink);

            if (!string.IsNullOrEmpty(e.StackTrace))
            {
                txt.AppendLine("Stack Trace = ").AppendLine(e.StackTrace);
            }

            exceptionMessage = txt.ToString();

            if (e.InnerException != null)
            {
                exceptionMessage = SerializeException(e.InnerException, exceptionMessage);
            }

            return exceptionMessage;
        }
    }
}
