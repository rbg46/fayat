using System;
using System.Runtime.Serialization;

namespace Fred.Business.ExplorateurDepense
{
    [Serializable]
    public class ReceptionCounterBalancedException : Exception
    {
        public ReceptionCounterBalancedException()
        {
        }

        public ReceptionCounterBalancedException(string message)
            : base(message)
        {
        }

        public ReceptionCounterBalancedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ReceptionCounterBalancedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}