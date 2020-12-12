using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Business.Tests.Integration.Organisation.OutputProcessor
{
    public class FileOutputProcessor : IOutputProcessor
    {
        public void Process(StringBuilder messagesStrBuilder)
        {
            var fileName = ConfigurationManager.AppSettings["fileErrorsOutput"].ToString();

            using (var sw = new StreamWriter(fileName, false))
            {
                sw.Write(messagesStrBuilder.ToString());
            }
        }
    }
}
