using Fred.Entities.DatesCalendrierPaie;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
namespace Fred.Business.Tests.Integration
{
  [TestClass]
  public class TestDatesCalendrierPaie : FredBaseTest
  {
    /// <summary>
    ///   Test de récupération d'un calendrier existant
    /// </summary>
    [TestMethod]
    [TestCategory("DBDepend")]
    public void GetSocieteDatesCalendrierPaieByIdAndYearAndMonthIsNotNull()
    {

      int year = 1900;
      int month = 1;
      //Création du calendrier
      DatesCalendrierPaieEnt dcp = new DatesCalendrierPaieEnt
      {
        SocieteId = 1,
        DateFinPointages = new DateTime(year, month, 25),
        DateTransfertPointages = new DateTime(year, month, 27)
      };

      DatesCalendrierPaieMgr.AddOrUpdateDatesCalendrierPaie(dcp);

      //On récupère le dernier calendrier créé
      DatesCalendrierPaieEnt dcpNew = DatesCalendrierPaieMgr.GetSocieteDatesCalendrierPaieByIdAndYearAndMonth(dcp.SocieteId, year, month);

      //On nettoie la table
      DatesCalendrierPaieMgr.DeleteSocieteDatesCalendrierPaieByIdAndYear(dcp.SocieteId, year);

      Assert.IsTrue(dcpNew.SocieteId == dcp.SocieteId);
      Assert.IsTrue(dcpNew.DateFinPointages == dcp.DateFinPointages);
      Assert.IsTrue(dcpNew.DateTransfertPointages == dcp.DateTransfertPointages);
    }

    /// <summary>
    ///   Test de récupération d'un calendrier vide
    /// </summary>
    [TestMethod]
    [TestCategory("DBDepend")]
    public void GetSocieteDatesCalendrierPaieByIdAndYearAndMonthIsNull()
    {


      //On récupère un calendrier qui n'existe pas
      DatesCalendrierPaieEnt dcpNew = DatesCalendrierPaieMgr.GetSocieteDatesCalendrierPaieByIdAndYearAndMonth(-1, 1800, 1);

      Assert.IsTrue(dcpNew.DateFinPointages == null);
      Assert.IsTrue(dcpNew.DateTransfertPointages == null);
    }

    /// <summary>
    ///   Test de l'ajout d'un calendrier pour une société, une année et un mois
    /// </summary>
    [TestMethod]
    [TestCategory("DBDepend")]
    public void AddDatesCalendrierPaie()
    {


      int year = 1900;
      int month = 1;

      //Création du calendrier
      DatesCalendrierPaieEnt dcp = new DatesCalendrierPaieEnt
      {
        SocieteId = 1,
        DateFinPointages = new DateTime(year, month, 25),
        DateTransfertPointages = new DateTime(year, month, 27)
      };

      DatesCalendrierPaieMgr.AddOrUpdateDatesCalendrierPaie(dcp);

      //On récupère le dernier calendrier créé
      DatesCalendrierPaieEnt dcpNew = DatesCalendrierPaieMgr.GetSocieteDatesCalendrierPaieByIdAndYearAndMonth(dcp.SocieteId, year, month);

      //On nettoie la table
      DatesCalendrierPaieMgr.DeleteSocieteDatesCalendrierPaieByIdAndYear(dcp.SocieteId, year);

      //Test
      Assert.IsTrue(dcpNew.SocieteId == dcp.SocieteId);
      Assert.IsTrue(dcpNew.DateFinPointages == dcp.DateFinPointages);
      Assert.IsTrue(dcpNew.DateTransfertPointages == dcp.DateTransfertPointages);
    }

    /// <summary>
    ///   Test de l'ajout d'un calendrier pour une société, une année et un mois
    /// </summary>
    [TestMethod]
    [TestCategory("DBDepend")]
    public void UpdateDatesCalendrierPaie()
    {


      int year = 1900;
      int month = 1;

      //Création du calendrier
      DatesCalendrierPaieEnt dcpToCreateAndUpdate = new DatesCalendrierPaieEnt
      {
        SocieteId = 1,
        DateFinPointages = new DateTime(year, month, 25),
        DateTransfertPointages = new DateTime(year, month, 27)
      };

      //On duplique le calendrier avant modification
      DatesCalendrierPaieEnt dcpOld = new DatesCalendrierPaieEnt
      {
        SocieteId = 1,
        DateFinPointages = new DateTime(year, month, 25),
        DateTransfertPointages = new DateTime(year, month, 27)
      };

      DatesCalendrierPaieMgr.AddDatesCalendrierPaie(dcpToCreateAndUpdate);

      //On met à jour le calendrier
      dcpToCreateAndUpdate.DateFinPointages = new DateTime(year, month, 20);
      dcpToCreateAndUpdate.DateTransfertPointages = new DateTime(year, month, 22);

      //On ajoute le calendrier
      DatesCalendrierPaieMgr.UpdateDatesCalendrierPaie(dcpToCreateAndUpdate);

      //On récupère le dernier calendrier créé
      DatesCalendrierPaieEnt dcpNew = DatesCalendrierPaieMgr.GetSocieteDatesCalendrierPaieByIdAndYearAndMonth(dcpToCreateAndUpdate.SocieteId, year, month);

      //On nettoie la table
      DatesCalendrierPaieMgr.DeleteSocieteDatesCalendrierPaieByIdAndYear(dcpToCreateAndUpdate.SocieteId, year);

      //Test
      Assert.IsTrue(dcpNew.DateFinPointages != dcpOld.DateFinPointages);
      Assert.IsTrue(dcpNew.DateTransfertPointages != dcpOld.DateTransfertPointages);
    }
  }
}