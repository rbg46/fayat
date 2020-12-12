using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Fred.Framework.Exceptions
{
    [Serializable]
    public class DeserializableFredException : Exception
    {
        public string ExceptionMessage { get; set; }
        public override string Message { get; }
        public DeserializableFredException InnerException { get; set; }

        public DeserializableFredException()
        {

        }

        public DeserializableFredException(SerializationInfo info, StreamingContext context)
        {
            if (info != null)
            {
                ExceptionMessage = (string)GetPropertyValue(() => info.GetString(nameof(ExceptionMessage)));
                InnerException = (DeserializableFredException)GetPropertyValue(() => info.GetValue(nameof(InnerException), typeof(DeserializableFredException)));

                var concatErrorMessage = new List<string>()
                {
                    (string) GetPropertyValue(() => info.GetString(nameof(Message))),
                    ExceptionMessage,
                    InnerException?.Message
                };

                Message = string.Join(" - ", concatErrorMessage.Where(m => !string.IsNullOrWhiteSpace(m)));
            }
        }

        private object GetPropertyValue(Func<object> func)
        {
            try
            {
                return func();
            }
            catch (SerializationException)
            {
                return null;
            }
        }
    }
}

