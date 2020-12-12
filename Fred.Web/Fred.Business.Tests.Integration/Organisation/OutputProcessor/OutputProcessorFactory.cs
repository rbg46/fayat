using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Business.Tests.Integration.Organisation.OutputProcessor
{
    public class OutputProcessorFactory
    {
        private IOutputProcessor myProcessor;

        /// <summary>
        /// Initialise le processor pour de l'affichage l'output debug
        /// </summary>
        public IOutputProcessor UseDebugOutputProcessor()
        {
            myProcessor = new DebugOutputProcessor();
            return myProcessor;
        }

        /// <summary>
        /// Initialise le processor pour de l'affichage dans un fichier
        /// </summary>
        public IOutputProcessor UseFileOutputProcessor()
        {
            myProcessor = new FileOutputProcessor();
            return myProcessor;
        }
    }
}
