using FluentAssertions;
using Fred.Business.Referential.Service;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Fournisseur.Builder;
using Fred.Entities.Referential;
using Fred.ImportExport.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.ImportExport.Business.Tests.Fournisseur
{
    [TestClass]
    public class FournisseurFluxManagerTest : BaseTu<FournisseurFluxManager>
    {
        internal enum ResultImportFournisseurByApi
        {
            NOTHING = 0,
            FOURNISSEUR_UPDATE = 1,
            AGENCE_UPDATE = 2
        }

        private ResultImportFournisseurByApi resultAfterOperation = ResultImportFournisseurByApi.NOTHING;
        private ImportFournisseurModelBuilder fournisseurBuilder;

        [TestInitialize]
        public void Initialize()
        {
            fournisseurBuilder = new ImportFournisseurModelBuilder();

            //Par defaut avant tout traitement.
            resultAfterOperation = ResultImportFournisseurByApi.NOTHING;

            var fournisseursImportServiceFake = GetMocked<IFournisseursImportService>();
            fournisseursImportServiceFake.Setup(c => c.AddOrUpdateFournisseurs(It.IsAny<FournisseurEnt>(), It.IsAny<int>()))
                .Callback(() => resultAfterOperation = ResultImportFournisseurByApi.FOURNISSEUR_UPDATE);
            fournisseursImportServiceFake.Setup(c => c.UpdateAgence(It.IsAny<AgenceEnt>(), It.IsAny<int>()))
                .Callback(() => resultAfterOperation = ResultImportFournisseurByApi.AGENCE_UPDATE);
            SubstituteProperty<IFournisseursImportService>(fournisseursImportServiceFake.Object);
        }

        [TestMethod]
        public void AddOrUpdateFournisseurAgence_WithRolePartenaireToAC_DoesProcessAgence()
        {
            WithArrange(fournisseurBuilder.Role.Agence().Build())
            .Invoking(() => Actual.AddOrUpdateFournisseur(ArrangedObject as ImportFournisseurModel));

            //Assert le resultat de la methode
            resultAfterOperation.Should().Equals(ResultImportFournisseurByApi.AGENCE_UPDATE);
        }

        [TestMethod]
        public void AddOrUpdateFournisseurAgence_WithRolePartenaireToFO_DoesProcessFournisseur()
        {
            WithArrange(fournisseurBuilder.Role.Fournisseur().Build())
            .Invoking(() => Actual.AddOrUpdateFournisseur(ArrangedObject as ImportFournisseurModel));

            //Assert le resultat de la methode
            resultAfterOperation.Should().Equals(ResultImportFournisseurByApi.FOURNISSEUR_UPDATE);
        }

        [TestMethod]
        public void AddOrUpdateFournisseurAgence_WithRolePartenaireNull_DoesProcessFournisseur()
        {
            WithArrange(fournisseurBuilder.Build())
            .Invoking(() => Actual.AddOrUpdateFournisseur(ArrangedObject as ImportFournisseurModel));

            //Assert le resultat de la methode
            resultAfterOperation.Should().Equals(ResultImportFournisseurByApi.FOURNISSEUR_UPDATE);
        }
    }
}
