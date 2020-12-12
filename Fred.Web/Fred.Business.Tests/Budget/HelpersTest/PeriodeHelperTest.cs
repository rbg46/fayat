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
    public class PeriodeHelperTest
    {
        [TestMethod]
        public void GetPeriodeTest()
        {
            var november2018 = new DateTime(2018, 11, 15);
            var january2016 = new DateTime(2016, 01, 01);
            var december2000 = new DateTime(2000, 12, 31);

            var november2018Periode = PeriodeHelper.GetPeriode(november2018);
            var january2016Periode = PeriodeHelper.GetPeriode(january2016);
            var december2000Periode = PeriodeHelper.GetPeriode(december2000);

            Assert.AreEqual(201811, november2018Periode);
            Assert.AreEqual(201601, january2016Periode);
            Assert.AreEqual(200012, december2000Periode);
        }

        [TestMethod]
        public void GetNextPeriode()
        {
            var august2018Periode = 201808;
            var december2018Periode = 201812;

            var september2018Periode = PeriodeHelper.GetNextPeriod(august2018Periode);
            var january2019Periode = PeriodeHelper.GetNextPeriod(december2018Periode);

            Assert.AreEqual(201809, september2018Periode);
            Assert.AreEqual(201901, january2019Periode);
        }

        [TestMethod]
        public void GetPreviousPeriod()
        {
            var january2018 = 201801;
            var september2018 = 201809;

            var december2017Periode = PeriodeHelper.GetPreviousPeriod(january2018);
            var august2018Periode = PeriodeHelper.GetPreviousPeriod(september2018);

            Assert.AreEqual(201712, december2017Periode);
            Assert.AreEqual(201808, august2018Periode);
        }

        [TestMethod]
        public void ToFirstOfMonth()
        {
            var september2018Periode = 201809;
            var september2018 = PeriodeHelper.ToFirstDayOfMonthDateTime(september2018Periode).Value;

            Assert.AreEqual(1, september2018.Day);
            Assert.AreEqual(09, september2018.Month);
            Assert.AreEqual(2018, september2018.Year);
        }

        [TestMethod]
        public void ToLastOfMonth()
        {
            //Année bisextile il y a un 29 février
            var february2016Periode = 201602;
            var september2018Periode = 201809;
            var august2018Periode = 201808;

            var february2016 = PeriodeHelper.ToLastDayOfMonthDateTime(february2016Periode).Value;
            var september2018 = PeriodeHelper.ToLastDayOfMonthDateTime(september2018Periode).Value;
            var august2018 = PeriodeHelper.ToLastDayOfMonthDateTime(august2018Periode).Value;

            Assert.AreEqual(29, february2016.Day);
            Assert.AreEqual(02, february2016.Month);
            Assert.AreEqual(2016, february2016.Year);

            Assert.AreEqual(30, september2018.Day);
            Assert.AreEqual(09, september2018.Month);
            Assert.AreEqual(2018, september2018.Year);

            Assert.AreEqual(31, august2018.Day);
            Assert.AreEqual(08, august2018.Month);
            Assert.AreEqual(2018, august2018.Year);
        }

        public void GetDateTime()
        {

        }
    }
}
