using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Business.Tests.Integration.Organisation.OutputProcessor
{
    public interface IOutputProcessor
    {
        void Process(StringBuilder messagesStrBuilder);
    }
}
