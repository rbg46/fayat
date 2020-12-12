using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Business.Budget.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.Budget.HelpersTest
{
    [TestClass]
    public class VersionHelperTest
    {
        [TestMethod]
        public void TestIncrementVesionMineur()
        {
            var version4 = "4.0";
            var version19 = "1.9";

            var version41 = VersionHelper.IncrementVersionMineur(version4);
            var version110 = VersionHelper.IncrementVersionMineur(version19);

            Assert.AreEqual(version41, "4.1");
            Assert.AreEqual(version110, "1.10");
        }

        [TestMethod]
        public void TestIncrementVersionMajeure()
        {
            var version4 = "4.0";
            var version19 = "1.9";

            var version5 = VersionHelper.IncrementVersionMajeure(version4);
            var version2 = VersionHelper.IncrementVersionMajeure(version19);

            Assert.AreEqual(version5, "5.0");
            Assert.AreEqual(version2, "2.0");
        }
    }
}
