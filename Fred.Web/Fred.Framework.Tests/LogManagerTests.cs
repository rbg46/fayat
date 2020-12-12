using System;
using System.Diagnostics;
using System.IO;
using Fred.Framework.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Framework.Tests
{
    [TestClass]
    public class LogManagerTests
    {
        private static string logFilePath;
        private readonly object thisLock = new object();
        private ILogManager logMgr;

        /// <summary>
        ///   Retourne la taille du fichier de log
        /// </summary>
        /// <returns>taille du fichier de log</returns>
        private long GetCurrentFileLogLength()
        {
            return new FileInfo(logFilePath).Length;
        }


        /// <summary>
        ///   Initialise l'ensemble des tests de la classe.
        /// </summary>
        /// <param name="context">Le contexte de tests.</param>
        [ClassInitialize]
        public static void InitAllTests(TestContext context)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            logFilePath = $@"{path}\testlogdata.log";
            File.CreateText(logFilePath).Close();

            Trace.Listeners.Add(new TextWriterTraceListener(logFilePath));
            Trace.AutoFlush = true;
        }

        /// <summary>
        ///   Initialise un test, cette méthode s'exécute avant chaque test
        ///   ///
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            lock (this.thisLock)
            {
                this.logMgr = new LogManager();
            }
        }

        /// <summary>
        ///   Test si le fichier a été modifié après avoir loggué une exception
        /// </summary>
        [TestMethod]
        public void TraceExceptionInFile()
        {
            lock (this.thisLock)
            {
                long oldFileLength = GetCurrentFileLogLength();

                try
                {
                    throw new Exception();
                }
                catch (Exception exception)
                {
                    this.logMgr.TraceException("Test Exception standard", exception);
                }

                long newFileLength = GetCurrentFileLogLength();
                Assert.IsTrue(oldFileLength < newFileLength);
            }
        }


        /// <summary>
        ///   Test si le fichier a été modifié après avoir loggué une exception personalisée de type FredException
        /// </summary>
        [TestMethod]
        public void TraceFredBusinessExceptionInFile()
        {
            lock (this.thisLock)
            {
                long oldFileLength = GetCurrentFileLogLength();

                try
                {
                    NullReferenceException inner = new NullReferenceException("Message de Inner exception");
                    FredBusinessException ex = new FredBusinessException("Message de FredBusinessException", AppDomain.CurrentDomain, inner);
                    ex.Service = "Service qui a levé l'exception";
                    ex.UserLogin = "Login qui a levé l'exception";

                    throw ex;
                }
                catch (FredException exception)
                {
                    this.logMgr.TraceException("Test FredBusinessException", exception);
                }

                long newFileLength = GetCurrentFileLogLength();
                Assert.IsTrue(oldFileLength < newFileLength);
            }
        }


        /// <summary>
        ///   Test si le fichier a été modifié après avoir loggué une exception personalisée de type FredException
        /// </summary>
        [TestMethod]
        public void TraceFredTechnicalExceptionInFile()
        {
            lock (this.thisLock)
            {
                long oldFileLength = GetCurrentFileLogLength();

                try
                {
                    NullReferenceException inner = new NullReferenceException("Message de Inner exception");
                    FredTechnicalException ex = new FredTechnicalException("Message de FredTechnicalException", AppDomain.CurrentDomain, inner);
                    ex.Service = "Service qui a levé l'exception";
                    ex.UserLogin = "Login qui a levé l'exception";

                    throw ex;
                }
                catch (FredException exception)
                {
                    this.logMgr.TraceException("Test FredTechnicalException", exception);
                }

                long newFileLength = GetCurrentFileLogLength();
                Assert.IsTrue(oldFileLength < newFileLength);
            }
        }


        /// <summary>
        ///   Test si le fichier a été modifié après avoir loggué un warning
        /// </summary>
        [TestMethod]
        public void TraceWarningInFile()
        {
            lock (this.thisLock)
            {
                long oldFileLength = GetCurrentFileLogLength();

                this.logMgr.TraceWarning("Test Warning");

                long newFileLength = GetCurrentFileLogLength();
                Assert.IsTrue(oldFileLength < newFileLength);
            }
        }

        /// <summary>
        ///   Test si le fichier a été modifié après avoir loggué une information
        /// </summary>
        [TestMethod]
        public void TraceInformationInFile()
        {
            lock (this.thisLock)
            {
                long oldFileLength = GetCurrentFileLogLength();

                this.logMgr.TraceInformation("Test Information");

                long newFileLength = GetCurrentFileLogLength();
                Assert.IsTrue(oldFileLength < newFileLength);
            }
        }

        /// <summary>
        ///   Test si le fichier a été modifié après avoir loggué une information de débogage
        /// </summary>
        [TestMethod]
        [Ignore]
        public void TraceDebugInFile()
        {
            lock (this.thisLock)
            {
                long oldFileLength = GetCurrentFileLogLength();

                this.logMgr.TraceDebug("Test Debug");

                long newFileLength = GetCurrentFileLogLength();
                Assert.IsTrue(oldFileLength < newFileLength);
            }
        }
    }
}
