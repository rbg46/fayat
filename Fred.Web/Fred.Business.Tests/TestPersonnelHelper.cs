using Fred.Entities.Personnel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Personnel.Tests
{
    [TestClass()]
    public class TestPersonnelHelper
    {
        [TestMethod()]
        public void WhenAPersonnelIsActive()
        {
            var now = new System.DateTime(2017, 09, 13);

            PersonnelEnt personnelEnt = new PersonnelEnt();

            personnelEnt.DateEntree = new System.DateTime(2017, 09, 01);

            personnelEnt.DateSortie = new System.DateTime(2017, 09, 30);

            personnelEnt.DateSuppression = null;

            var isActive = PersonnelHelper.GetPersonnelIsActive(personnelEnt, now);

            Assert.IsTrue(isActive);
        }

        [TestMethod()]
        public void WhenAPersonnelHasNoDateSortieAndNoDateSuppressionHeIsActive()
        {
            var now = new System.DateTime(2017, 09, 13);

            PersonnelEnt personnelEnt = new PersonnelEnt();

            personnelEnt.DateEntree = new System.DateTime(2017, 09, 01);

            personnelEnt.DateSortie = null;

            personnelEnt.DateSuppression = null;

            var isActive = PersonnelHelper.GetPersonnelIsActive(personnelEnt, now);

            Assert.IsTrue(isActive, "un personnel est actif quand il n a pas de date de sortie et de date de supression");
        }

        [TestMethod()]
        public void WhenAPersonnelHasDateSortieIsInferiorTodayPersonnelIsInactive()
        {
            var now = new System.DateTime(2017, 09, 13);

            PersonnelEnt personnelEnt = new PersonnelEnt();

            personnelEnt.DateEntree = new System.DateTime(2017, 09, 01);

            personnelEnt.DateSortie = new System.DateTime(2017, 09, 02);

            personnelEnt.DateSuppression = null;

            var isActive = PersonnelHelper.GetPersonnelIsActive(personnelEnt, now);

            Assert.IsFalse(isActive, "Un personnel est incatif quand la date de sortie est inferieur a aujourd hui.");
        }

        [TestMethod()]
        public void WhenAPersonnelHasDateSuppressionEqualToTodayPersonnelIsInactive()
        {
            var now = new System.DateTime(2017, 09, 13);

            PersonnelEnt personnelEnt = new PersonnelEnt();

            personnelEnt.DateEntree = new System.DateTime(2017, 09, 01);

            personnelEnt.DateSortie = null;

            personnelEnt.DateSuppression = new System.DateTime(2017, 09, 13);

            var isActive = PersonnelHelper.GetPersonnelIsActive(personnelEnt, now);

            Assert.IsFalse(isActive, "Un personnel est incatif quand la date de supression est egale a aujourd hui.");
        }

        [TestMethod()]
        public void WhenAPersonnelHasDateSuppressionInferiorToTodayPersonnelIsInactive()
        {
            var now = new System.DateTime(2017, 09, 13);

            PersonnelEnt personnelEnt = new PersonnelEnt();

            personnelEnt.DateEntree = new System.DateTime(2017, 09, 01);

            personnelEnt.DateSortie = null;

            personnelEnt.DateSuppression = new System.DateTime(2017, 09, 12);

            var isActive = PersonnelHelper.GetPersonnelIsActive(personnelEnt, now);

            Assert.IsFalse(isActive, "Un personnel est incatif quand la date de supression est inferieure a aujourd hui.");
        }

        [TestMethod()]
        public void WhenAPersonnelHasDateSuppressionAndDateSortieInferiorToTodayPersonnelIsInactive()
        {
            var now = new System.DateTime(2017, 09, 13);

            PersonnelEnt personnelEnt = new PersonnelEnt();

            personnelEnt.DateEntree = new System.DateTime(2017, 09, 01);

            personnelEnt.DateSortie = new System.DateTime(2017, 09, 12);

            personnelEnt.DateSuppression = new System.DateTime(2017, 09, 12);

            var isActive = PersonnelHelper.GetPersonnelIsActive(personnelEnt, now);

            Assert.IsFalse(isActive, "Un personnel est incatif quand la date de supression et la date de sortie sont inferieure a aujourd hui.");
        }

        [TestMethod()]
        public void WhenAPersonnelHasDateEntreSuperiorToTodayPersonnelIsInactive()
        {
            var now = new System.DateTime(2017, 09, 13);

            PersonnelEnt personnelEnt = new PersonnelEnt();

            personnelEnt.DateEntree = new System.DateTime(2017, 09, 14);

            personnelEnt.DateSortie = new System.DateTime(2017, 09, 25);

            personnelEnt.DateSuppression = null;

            var isActive = PersonnelHelper.GetPersonnelIsActive(personnelEnt, now);

            Assert.IsFalse(isActive, "Un personnel est incatif quand la date de d'entree est superieur a aujourd hui.");
        }


    }
}
