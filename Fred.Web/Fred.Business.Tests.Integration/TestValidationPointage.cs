using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Entities;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.Integration
{

    [TestClass]

    public class TestValidationPointage : FredBaseTest
    {
        private const int CurrentUserId = 1;

        #region TU LotPointageEnt

        /// <summary>
        ///   Test l'ajout d'un lot de pointage
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void AddLotPointage()
        {
            var lp = new LotPointageEnt
            {
                AuteurCreationId = CurrentUserId,
                DateCreation = DateTime.UtcNow,
                Periode = DateTime.UtcNow,
            };

            LotPointageMgr.AddLotPointage(lp, CurrentUserId);

            Assert.IsTrue(lp.LotPointageId > 0);
        }

        /// <summary>
        ///   Test la mise à jour d'un lot de pointage
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void UpdateLotPointage()
        {
            var lp = new LotPointageEnt
            {
                AuteurCreationId = CurrentUserId,
                DateCreation = DateTime.UtcNow,
                Periode = DateTime.UtcNow,
            };

            LotPointageMgr.AddLotPointage(lp, CurrentUserId);

            lp.DateVisa = DateTime.UtcNow;
            lp.AuteurVisaId = CurrentUserId;

            LotPointageMgr.UpdateLotPointage(lp, CurrentUserId);

            var lpUpdated = LotPointageMgr.Get(lp.LotPointageId);

            Assert.IsTrue(lpUpdated.DateModification.HasValue);
            Assert.IsTrue(lpUpdated.DateVisa.HasValue);
            Assert.IsTrue(lpUpdated.AuteurVisaId.HasValue);
        }

        /// <summary>
        ///   Test la suppression d'un lot de pointage
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void DeleteLotPointage()
        {
            var lp = new LotPointageEnt
            {
                AuteurCreationId = CurrentUserId,
                DateCreation = DateTime.UtcNow,
                Periode = DateTime.UtcNow,
            };

            LotPointageMgr.AddLotPointage(lp, CurrentUserId);

            LotPointageMgr.DeleteLotPointage(lp.LotPointageId);

            var lpDeleted = LotPointageMgr.Get(lp.LotPointageId);

            Assert.IsNull(lpDeleted);
        }

        /// <summary>
        ///   Test la signature d'un lot de pointage
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        
        [Ignore]
        public void SignLotPointage()
        {
            var lp = new LotPointageEnt
            {
                AuteurCreationId = CurrentUserId,
                DateCreation = DateTime.UtcNow,
                Periode = DateTime.UtcNow,
            };

            LotPointageMgr.AddLotPointage(lp, CurrentUserId);

            LotPointageMgr.SignLotPointage(lp.LotPointageId);

            var lpSigned = LotPointageMgr.Get(lp.LotPointageId);

            Assert.IsTrue(lpSigned.DateModification.HasValue);
            Assert.IsTrue(lpSigned.DateVisa.HasValue);
            Assert.IsTrue(lpSigned.AuteurVisaId.HasValue);
        }

        #endregion

        #region TU ControlePointageEnt
        /// <summary>
        ///   Test l'ajout d'un contrôle pointage
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void AddControlePointage()
        {
            var lp = new LotPointageEnt
            {
                AuteurCreationId = CurrentUserId,
                DateCreation = DateTime.UtcNow,
                Periode = DateTime.UtcNow,
            };

            LotPointageMgr.AddLotPointage(lp, CurrentUserId);

            var cp = new ControlePointageEnt
            {
                AuteurCreationId = CurrentUserId,
                DateDebut = DateTime.UtcNow,
                LotPointageId = lp.LotPointageId,
                Statut = FluxStatus.InProgress.ToIntValue(),
                TypeControle = TypeControlePointage.ControleChantier.ToIntValue()
            };

            ControlePointageMgr.AddControlePointage(cp);

            Assert.IsTrue(cp.ControlePointageId > 0);
        }

        /// <summary>
        ///   Test la mise à jour d'un contrôle pointage
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void UpdateControlePointage()
        {
            var lp = new LotPointageEnt
            {
                AuteurCreationId = CurrentUserId,
                DateCreation = DateTime.UtcNow,
                Periode = DateTime.UtcNow,
            };

            LotPointageMgr.AddLotPointage(lp, CurrentUserId);

            var cp = new ControlePointageEnt
            {
                AuteurCreationId = CurrentUserId,
                DateDebut = DateTime.UtcNow,
                LotPointageId = lp.LotPointageId,
                Statut = FluxStatus.InProgress.ToIntValue(),
                TypeControle = TypeControlePointage.ControleChantier.ToIntValue()
            };

            ControlePointageMgr.AddControlePointage(cp);

            cp.Statut = FluxStatus.Done.ToIntValue();
            cp.DateFin = DateTime.UtcNow;

            ControlePointageMgr.UpdateControlePointage(cp);

            var cpUpdated = ControlePointageMgr.Get(cp.ControlePointageId);

            Assert.AreEqual(cpUpdated.Statut, FluxStatus.Done.ToIntValue());
            Assert.IsTrue(cpUpdated.DateFin.HasValue);
        }

        /// <summary>
        ///   Test la suppression d'un contrôle pointage
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void DeleteControlePointage()
        {
            var lp = new LotPointageEnt
            {
                AuteurCreationId = CurrentUserId,
                DateCreation = DateTime.UtcNow,
                Periode = DateTime.UtcNow,
            };

            LotPointageMgr.AddLotPointage(lp, CurrentUserId);

            var cp = new ControlePointageEnt
            {
                AuteurCreationId = CurrentUserId,
                DateDebut = DateTime.UtcNow,
                LotPointageId = lp.LotPointageId,
                Statut = FluxStatus.InProgress.ToIntValue(),
                TypeControle = TypeControlePointage.ControleChantier.ToIntValue()
            };

            ControlePointageMgr.AddControlePointage(cp);

            ControlePointageMgr.DeleteControlePointage(cp.ControlePointageId);

            var cpDeleted = ControlePointageMgr.Get(cp.ControlePointageId);

            Assert.IsNull(cpDeleted);
        }

        /// <summary>
        ///   Test la récupération du plus récent contrôle pointage
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]

        public void GetLatestControlePointage()
        {
            var lp = new LotPointageEnt
            {
                AuteurCreationId = CurrentUserId,
                DateCreation = DateTime.UtcNow,
                Periode = DateTime.UtcNow,
            };

            LotPointageMgr.AddLotPointage(lp, CurrentUserId);

            var cp1 = new ControlePointageEnt
            {
                AuteurCreationId = CurrentUserId,
                DateDebut = DateTime.UtcNow,
                LotPointageId = lp.LotPointageId,
                Statut = FluxStatus.Done.ToIntValue(),
                TypeControle = TypeControlePointage.ControleChantier.ToIntValue()
            };

            var cp2 = new ControlePointageEnt
            {
                AuteurCreationId = CurrentUserId,
                DateDebut = DateTime.UtcNow.AddMinutes(1),
                LotPointageId = lp.LotPointageId,
                Statut = FluxStatus.Failed.ToIntValue(),
                TypeControle = TypeControlePointage.ControleChantier.ToIntValue()
            };

            var cp3 = new ControlePointageEnt
            {
                AuteurCreationId = CurrentUserId,
                DateDebut = DateTime.UtcNow.AddMinutes(5),
                LotPointageId = lp.LotPointageId,
                Statut = FluxStatus.InProgress.ToIntValue(),
                TypeControle = TypeControlePointage.ControleChantier.ToIntValue()
            };

            ControlePointageMgr.AddControlePointage(cp1);
            ControlePointageMgr.AddControlePointage(cp2);
            ControlePointageMgr.AddControlePointage(cp3);

            var latestCp = ControlePointageMgr.GetLatest(lp.LotPointageId, TypeControlePointage.ControleChantier.ToIntValue());

            Assert.IsNotNull(latestCp);
            Assert.AreEqual(latestCp.ControlePointageId, cp3.ControlePointageId);
        }

        /// <summary>
        ///   Test la récupération des controles pointage les plus récents (pour chaque type contrôle)
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]

        public void GetLatestListControlePointage()
        {
            var lp = new LotPointageEnt
            {
                AuteurCreationId = CurrentUserId,
                DateCreation = DateTime.UtcNow,
                Periode = DateTime.UtcNow,
            };

            List<int> ListPointageId = new List<int>();
            LotPointageMgr.AddLotPointage(lp, CurrentUserId);

            // --------------------------------- CONTROLE CHANTIER ---------------------------------
            var cpChantier1 = new ControlePointageEnt
            {
                AuteurCreationId = CurrentUserId,
                DateDebut = DateTime.UtcNow,
                LotPointageId = lp.LotPointageId,
                Statut = FluxStatus.Done.ToIntValue(),
                TypeControle = TypeControlePointage.ControleChantier.ToIntValue()
            };

            var cpChantier2 = new ControlePointageEnt
            {
                AuteurCreationId = CurrentUserId,
                DateDebut = DateTime.UtcNow.AddMinutes(1),
                LotPointageId = lp.LotPointageId,
                Statut = FluxStatus.Failed.ToIntValue(),
                TypeControle = TypeControlePointage.ControleChantier.ToIntValue()
            };

            var cpChantier3 = new ControlePointageEnt
            {
                AuteurCreationId = CurrentUserId,
                DateDebut = DateTime.UtcNow.AddMinutes(5),
                LotPointageId = lp.LotPointageId,
                Statut = FluxStatus.InProgress.ToIntValue(),
                TypeControle = TypeControlePointage.ControleChantier.ToIntValue()
            };

            // ---------------------------------- CONTROLE VRAC ---------------------------------
            var cpVrac1 = new ControlePointageEnt
            {
                AuteurCreationId = CurrentUserId,
                DateDebut = DateTime.UtcNow,
                LotPointageId = lp.LotPointageId,
                Statut = FluxStatus.Done.ToIntValue(),
                TypeControle = TypeControlePointage.ControleVrac.ToIntValue()
            };

            var cpVrac2 = new ControlePointageEnt
            {
                AuteurCreationId = CurrentUserId,
                DateDebut = DateTime.UtcNow.AddMinutes(1),
                LotPointageId = lp.LotPointageId,
                Statut = FluxStatus.Failed.ToIntValue(),
                TypeControle = TypeControlePointage.ControleVrac.ToIntValue()
            };

            var cpVrac3 = new ControlePointageEnt
            {
                AuteurCreationId = CurrentUserId,
                DateDebut = DateTime.UtcNow.AddMinutes(5),
                LotPointageId = lp.LotPointageId,
                Statut = FluxStatus.InProgress.ToIntValue(),
                TypeControle = TypeControlePointage.ControleVrac.ToIntValue()
            };

            // --------------------------------- REMONTEE VRAC ---------------------------------
            var remonteeVrac1 = new RemonteeVracEnt
            {
                AuteurCreationId = CurrentUserId,
                DateDebut = DateTime.UtcNow,
                Statut = FluxStatus.Done.ToIntValue(),
                Periode = DateTime.UtcNow
            };

            var remonteeVrac2 = new RemonteeVracEnt
            {
                AuteurCreationId = CurrentUserId,
                DateDebut = DateTime.UtcNow.AddMinutes(1),
                Statut = FluxStatus.Failed.ToIntValue(),
                Periode = DateTime.UtcNow
            };

            var remonteeVrac3 = new RemonteeVracEnt
            {
                AuteurCreationId = CurrentUserId,
                DateDebut = DateTime.UtcNow.AddMinutes(5),
                Statut = FluxStatus.InProgress.ToIntValue(),
                Periode = DateTime.UtcNow
            };

            ControlePointageMgr.AddControlePointage(cpChantier1);
            ControlePointageMgr.AddControlePointage(cpChantier2);
            ControlePointageMgr.AddControlePointage(cpChantier3);
            ControlePointageMgr.AddControlePointage(cpVrac1);
            ControlePointageMgr.AddControlePointage(cpVrac2);
            ControlePointageMgr.AddControlePointage(cpVrac3);
            RemonteeVracMgr.AddRemonteeVrac(remonteeVrac1);
            RemonteeVracMgr.AddRemonteeVrac(remonteeVrac2);
            RemonteeVracMgr.AddRemonteeVrac(remonteeVrac3);

            ListPointageId.Add(lp.LotPointageId);
            var latestList = ControlePointageMgr.GetLatestList(ListPointageId).ToList();
            var latestCpChantier = latestList.FirstOrDefault(x => x.TypeControle == TypeControlePointage.ControleChantier.ToIntValue());
            var latestCpVrac = latestList.FirstOrDefault(x => x.TypeControle == TypeControlePointage.ControleVrac.ToIntValue());
            var latestRemonteeVrac = RemonteeVracMgr.GetLatest(CurrentUserId, DateTime.UtcNow);

            Assert.IsNotNull(latestList);
            Assert.AreEqual(latestList.Count, 2);
            Assert.IsNotNull(latestCpChantier);
            Assert.IsNotNull(latestCpVrac);
            Assert.IsNotNull(latestRemonteeVrac);
            Assert.AreEqual(latestCpChantier.ControlePointageId, cpChantier3.ControlePointageId);
            Assert.AreEqual(latestCpVrac.ControlePointageId, cpVrac3.ControlePointageId);
            Assert.AreEqual(latestRemonteeVrac.RemonteeVracId, remonteeVrac3.RemonteeVracId);
        }
        #endregion

        #region TU ControlePointageErreurEnt
        /// <summary>
        ///   Test l'ajout d'un contrôle pointage erreur
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void AddControlePointageErreur()
        {
            var lp = new LotPointageEnt
            {
                AuteurCreationId = CurrentUserId,
                DateCreation = DateTime.UtcNow,
                Periode = DateTime.UtcNow,
            };

            LotPointageMgr.AddLotPointage(lp, CurrentUserId);

            var cp = new ControlePointageEnt
            {
                AuteurCreationId = CurrentUserId,
                DateDebut = DateTime.UtcNow,
                LotPointageId = lp.LotPointageId,
                Statut = FluxStatus.InProgress.ToIntValue(),
                TypeControle = TypeControlePointage.ControleChantier.ToIntValue()
            };

            ControlePointageMgr.AddControlePointage(cp);

            var cpe = new ControlePointageErreurEnt
            {
                ControlePointageId = cp.ControlePointageId,
                DateRapport = DateTime.UtcNow,
                Message = "Erreur " + GenerateString(2),
                PersonnelId = 1
            };

            ControlePointageMgr.AddControlePointageErreur(cpe);

            Assert.IsTrue(cpe.ControlePointageErreurId > 0);
        }

        #endregion
    }


}
