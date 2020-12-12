using System;
using System.Collections.Generic;
using FluentAssertions;
using Fred.Common.Tests.Data.TypeOrganisation.Mock;
using Fred.Entities.CI;
using Fred.Entities.Organisation;
using Fred.ImportExport.Business.CI.WebApi.Fred;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.ImportExport.Business.Tests.CI.WebApi.Fred
{
    [TestClass]
    public class CiFinderTests
    {
        private const string CodeCi1 = "CI1";
        private const int EtablissementComptableId1 = 1;
        private const int SocieteId1 = 1;
        private const int CI1ID = 1;

        private CiFinderInWebApiSystemService CreateCiFinder()
        {
            return new CiFinderInWebApiSystemService();
        }

        [TestMethod]
        public void GeGetCiIn_si_un_ci_a_le_meme_code_etabliessement_et_le_meme_societe_alors_la_recherche_devrait_reussir()
        {
            // Arrange
            var ciFinder = this.CreateCiFinder();

            var existingCIs = GetCIs();

            CIEnt webApiCi = new CIEnt()
            {
                Code = CodeCi1,
                EtablissementComptableId = EtablissementComptableId1,
                SocieteId = SocieteId1,
            };

            // Act
            var result = ciFinder.GetCiIn(existingCIs, webApiCi);

            // Assert
            result.Should().NotBeNull();

            result.CiId.Should().Be(CI1ID);
        }


        private List<CIEnt> GetCIs()
        {
            return new List<CIEnt>()
             {
                CreateCI1(),
                CreateCI2(),
             };
        }

        private CIEnt CreateCI1()
        {
            return new CIEnt()
            {
                CiId = CI1ID,
                Code = CodeCi1,
                EtablissementComptableId = EtablissementComptableId1,
                SocieteId = SocieteId1,
                Organisation = new OrganisationEnt
                {
                    TypeOrganisationId = TypeOrganisationMock.GetCiType().TypeOrganisationId
                }
            };
        }

        private OrganisationEnt OrganisationEnt()
        {
            throw new NotImplementedException();
        }

        private CIEnt CreateCI2()
        {
            return new CIEnt()
            {
                CiId = 2,
                Code = CodeCi1,
                EtablissementComptableId = 2,
                SocieteId = 2,
                Organisation = new OrganisationEnt
                {
                    TypeOrganisationId = TypeOrganisationMock.GetCiType().TypeOrganisationId
                }
            };
        }

    }
}
