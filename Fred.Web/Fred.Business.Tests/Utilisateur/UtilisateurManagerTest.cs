using System.Linq;
using FluentAssertions;
using Fred.Business.Utilisateur;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.Utilisateur.Builder;
using Fred.Common.Tests.EntityFramework;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Utilisateur;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.Utilisateur
{
    /// <summary>
    /// Classe de test pour <see cref="UtilisateurManager"/>
    /// </summary>
    [TestClass]
    public class UtilisateurManagerTest : BaseTu<UtilisateurManager>
    {
        [TestMethod]
        public void GetRoleOrganisationAffectationsWithUserIdValidReturnsNotEmptyRoleList()
        {
            var builder = new AffectationSeuilUtilisateurBuilder();
            FakeDbSet<AffectationSeuilUtilisateurEnt> utilOrgaRoleDevises = builder.BuildFakeDbSet(builder.Prototype());

            var affectationSeuilUtilisateurRepositoryMock = new Mock<IAffectationSeuilUtilisateurRepository>();
            affectationSeuilUtilisateurRepositoryMock.Setup(asurm => asurm.GetAffectationSeuilUtilisateursForDetailPersonnel(1)).Returns(utilOrgaRoleDevises.ToList());
            SubstituteConstructorArgument(affectationSeuilUtilisateurRepositoryMock.Object);

            Actual.GetRoleOrganisationAffectations(1).Should().NotBeEmpty();
        }
    }
}
