using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Business.Tests.Integration.Organisation.OutputProcessor
{
    public class DebugOutputProcessor : IOutputProcessor
    {
        public void Process(StringBuilder messagesStrBuilder)
        {
            Debug.WriteLine(messagesStrBuilder.ToString());
        }
    }
}
