using Fred.Business.RepriseDonnees.Common.Selector;
using Fred.Entites.RepriseDonnees.Commande;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace Fred.Business.Tests.RepriseDonnees.Common.Selector
{
    [TestClass]
    public class CommonFieldSelectorTests
    {
        private MockRepository mockRepository;



        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.mockRepository.VerifyAll();
        }

        private CommonFieldSelector CreateCommonFieldSelector()
        {
            return new CommonFieldSelector();
        }

        //[TestMethod]
        //public void GetCiOfDatabase_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var unitUnderTest = this.CreateCommonFieldSelector();
        //    OrganisationTree organisationTree = TODO;
        //    int groupeId = TODO;
        //    string codeSociete = TODO;
        //    string codeCi = TODO;
        //    List<CIEnt> cis = TODO;

        //    // Act
        //    var result = unitUnderTest.GetCiOfDatabase(
        //        organisationTree,
        //        groupeId,
        //        codeSociete,
        //        codeCi,
        //        cis);

        //    // Assert
        //    Assert.Fail();
        //}

        //[TestMethod]
        //public void GetDate_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var unitUnderTest = this.CreateCommonFieldSelector();
        //    string date = TODO;

        //    // Act
        //    var result = unitUnderTest.GetDate(
        //        date);

        //    // Assert
        //    Assert.Fail();
        //}

        private GetT3ByCodesOrDefaultResponse GetTacheParDefault()
        {
            var getT3ByCodesOrDefaultResponse_default = new GetT3ByCodesOrDefaultResponse
            {
                CiId = 1587,
                Code = "",
                Tache = new Entities.Referential.TacheEnt
                {
                    Active = true,
                    CI = null,
                    CiId = 1587,
                    Code = "000000",
                    Libelle = "TACHE PAR DEFAUT",
                    Niveau = 3,
                    Parent = null,
                    ParentId = 4798,
                    TacheId = 8775,
                    TacheParDefaut = true,
                    TacheType = 0,

                }
            };
            return getT3ByCodesOrDefaultResponse_default;
        }
        private GetT3ByCodesOrDefaultResponse GetTacheT1()
        {
            var getT3ByCodesOrDefaultResponse_T1 = new GetT3ByCodesOrDefaultResponse
            {
                CiId = 1587,
                Code = "T1",
                Tache = new Entities.Referential.TacheEnt
                {
                    Active = true,
                    CI = null,
                    CiId = 1587,
                    Code = "T1",
                    Libelle = "TACHE T1",
                    Niveau = 3,
                    Parent = null,
                    ParentId = 4798,
                    TacheId = 15,
                    TacheParDefaut = false,
                    TacheType = 0,

                }
            };
            return getT3ByCodesOrDefaultResponse_T1;
        }


        private List<GetT3ByCodesOrDefaultResponse> GetTachesResponses()
        {

            List<GetT3ByCodesOrDefaultResponse> tachesResponseUsedInExcel = new List<GetT3ByCodesOrDefaultResponse>
            {
                GetTacheParDefault(),
                GetTacheT1()
            };

            return tachesResponseUsedInExcel;
        }

        [TestMethod]
        public void GetTache_une_tache_devrait_etre_trouvee_pour_le_ci_avec_le_un_code_non_vide()
        {
            // Arrange
            var unitUnderTest = this.CreateCommonFieldSelector();
            int? ciId = 1587;
            string codeTache = "T1";

            List<GetT3ByCodesOrDefaultResponse> tachesResponseUsedInExcel = GetTachesResponses();

            // Act
            var result = unitUnderTest.GetTache(ciId, codeTache, tachesResponseUsedInExcel);

            // Assert
            Assert.AreEqual(GetTacheT1().Tache.TacheId, result.TacheId, "Une tache devrait etre trouvee pour le ci avec le un code NON vide.");

        }

        [TestMethod]
        public void GetTache_une_tache_devrait_etre_trouvee_pour_le_ci_avec_le_un_code_vide()
        {
            // Arrange
            var unitUnderTest = this.CreateCommonFieldSelector();
            int? ciId = 1587;
            string codeTache = "";

            List<GetT3ByCodesOrDefaultResponse> tachesResponseUsedInExcel = GetTachesResponses();


            // Act
            var result = unitUnderTest.GetTache(ciId, codeTache, tachesResponseUsedInExcel);

            // Assert
            Assert.AreEqual(GetTacheParDefault().Tache.TacheId, result.TacheId, "Une tache devrait etre trouvee pour le ci avec le un code vide.");

        }

        [TestMethod]
        public void GetDecimal_devrait_retourner_1200()
        {
            // Arrange
            var unitUnderTest = this.CreateCommonFieldSelector();
            string value = "1200,0";

            // Act
            var result = unitUnderTest.GetDecimal(value);

            // Assert
            Assert.AreEqual(result, (decimal)1200, "Devrait retourner 1200.");

        }

        [TestMethod]
        public void GetDecimal_devrait_retourner_1200_virgule_1()
        {
            // Arrange
            var unitUnderTest = this.CreateCommonFieldSelector();
            string value = "1200,1";

            // Act
            var result = unitUnderTest.GetDecimal(value);

            // Assert
            Assert.AreEqual(result, (decimal)1200.1, "Devrait retourner 1200.1.");

        }
    }
}
