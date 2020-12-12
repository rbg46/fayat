using System;
using System.Collections.Generic;
using FluentAssertions;
using Fred.Business.CommandeEnergies;
using Fred.Common.Tests;
using Fred.Common.Tests.Data.ContratInterimaire.Builder;
using Fred.Common.Tests.Data.Personnel.Mock;
using Fred.Common.Tests.Data.Rapport.Builder;
using Fred.Common.Tests.Data.Referential.Tache.Builder;
using Fred.Common.Tests.Data.ReferentielFixe.Mock;
using Fred.Common.Tests.Data.Societe.Mock;
using Fred.Common.Tests.Data.TypeEnergie.Builder;
using Fred.Common.Tests.Data.Unite.Mock;
using Fred.Common.Tests.EntityFramework;
using Fred.DataAccess.Commande;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using Fred.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fred.Business.Tests.CommandeEnergie.Service
{
    /// <summary>
    /// Classe de test du service <see cref="CommandeEnergieLigneService"/>
    /// </summary>
    [TestClass]
    public class CommandeEnergieLigneServiceTest : BaseTu<CommandeEnergieLigneService>
    {
        private TypeEnergieEnt energie;
        private TacheEnt tache;
        private List<int> socParticipantes;
        private List<ContratInterimaireEnt> listContrat = new List<ContratInterimaireEnt>();

        private FakeDbSet<PersonnelEnt> personnelMock;
        private FakeDbSet<RessourceEnt> ressourceMock;
        private FakeDbSet<UniteEnt> uniteMock;
        private FakeDbSet<SocieteEnt> societeMock;

        [TestInitialize]
        public void TestInitialize()
        {
            //Build
            energie = new TypeEnergieBuilder().Code(Constantes.TypeEnergie.Interimaire).Build();
            tache = new TacheBuilder().Build();

            var rapportbuilder = new RapportLigneBuilder();
            var Contratbuilder = new ContratInterimaireBuilder();
            societeMock = new SocieteMocks().GetFakeDbSet();
            personnelMock = new PersonnelMocks().GetFakeDbSet(false);
            ressourceMock = new RessourceMocks().GetFakeDbSet();
            uniteMock = new UniteMocks().GetFakeDbSet();


            listContrat.Add(Contratbuilder.ContratInterimaireId(387).DateDebut(It.IsAny<DateTime>()).DateFin(It.IsAny<DateTime>()).NumContrat("54")
                .InterimaireId(1).Interimaire(personnelMock.Find(1))
                .RessourceId(1).Ressource(ressourceMock.Find(1))
                .SocieteId(1).Societe(societeMock.Find(1))
                .UniteId(4).Unite(uniteMock.Find(4))
                .Build());
            listContrat.Add(Contratbuilder.ContratInterimaireId(409).DateDebut(It.IsAny<DateTime>()).DateFin(It.IsAny<DateTime>()).NumContrat("test_contrat")
                .InterimaireId(1).Interimaire(personnelMock.Find(1))
                .RessourceId(1).Ressource(ressourceMock.Find(1))
                .SocieteId(2).Societe(societeMock.Find(2))
                .UniteId(4).Unite(uniteMock.Find(4))
                .Build());
            listContrat.Add(Contratbuilder.ContratInterimaireId(417).DateDebut(It.IsAny<DateTime>()).DateFin(It.IsAny<DateTime>()).NumContrat("1234567")
                .InterimaireId(1).Interimaire(personnelMock.Find(1))
                .RessourceId(1).Ressource(ressourceMock.Find(1))
                .SocieteId(1).Societe(societeMock.Find(1))
                .UniteId(4).Unite(uniteMock.Find(4))
                .Build());
            listContrat.Add(Contratbuilder.ContratInterimaireId(99999).DateDebut(It.IsAny<DateTime>()).DateFin(It.IsAny<DateTime>()).NumContrat("neverlignerapport")
               .InterimaireId(1).Interimaire(personnelMock.Find(1))
               .RessourceId(1).Ressource(ressourceMock.Find(1))
               .SocieteId(1).Societe(societeMock.Find(1))
               .UniteId(4).Unite(uniteMock.Find(4))
               .Build());

            var listrapport = new List<RapportLigneEnt>();
            listrapport.Add(rapportbuilder.CiId(8080).HeureNormal(5).DatePointage(It.IsAny<DateTime>()).ContratId(387)
                .Contrat(listContrat.Find(x => x.ContratInterimaireId == 387)).Build());
            listrapport.Add(rapportbuilder.CiId(8080).HeureNormal(5).DatePointage(It.IsAny<DateTime>()).ContratId(387)
                .Contrat(listContrat.Find(x => x.ContratInterimaireId == 387)).Build());
            listrapport.Add(rapportbuilder.CiId(8080).HeureNormal(7).DatePointage(It.IsAny<DateTime>()).ContratId(409)
                 .Contrat(listContrat.Find(x => x.ContratInterimaireId == 409)).Build());
            listrapport.Add(rapportbuilder.CiId(8080).HeureNormal(6).DatePointage(It.IsAny<DateTime>()).ContratId(409)
                 .Contrat(listContrat.Find(x => x.ContratInterimaireId == 409)).Build());
            listrapport.Add(rapportbuilder.CiId(8080).HeureNormal(3).DatePointage(It.IsAny<DateTime>()).ContratId(417)
                 .Contrat(listContrat.Find(x => x.ContratInterimaireId == 417)).Build());
            listrapport.Add(rapportbuilder.CiId(8080).HeureNormal(6).DatePointage(It.IsAny<DateTime>()).ContratId(417)
                 .Contrat(listContrat.Find(x => x.ContratInterimaireId == 417)).Build());
            listrapport.Add(rapportbuilder.CiId(8080).HeureNormal(10000).DatePointage(It.IsAny<DateTime>()).ContratId(417).DateSuppression(It.IsAny<DateTime>())
                 .Contrat(listContrat.Find(x => x.ContratInterimaireId == 417)).Build());

            var context = GetMocked<FredDbContext>();
            context.Object.RapportLignes = rapportbuilder.BuildFakeDbSet(listrapport);

            var commandeEnergieRepository = new CommandeEnergieRepository(context.Object);

            SubstituteConstructorArgument<ICommandeEnergieRepository>(commandeEnergieRepository);
        }

        [TestMethod]
        [TestCategory("CommandeEnergieLigneService")]
        public void GetGeneratedCommandeEnergieLignes_WhenTypeEnergieInterimaire_Returns_NotEmptyList()
        {
            const int ci = 8080;
            socParticipantes = new List<int>() { 1, 2, 3 };
            List<CommandeEnergieLigne> result = Actual.GetGeneratedCommandeEnergieLignes(energie, ci, It.IsAny<DateTime>(), tache, uniteMock.Find(4), socParticipantes);
            result.Should().NotBeEmpty();
        }

        //RG_6472_015_01
        //L’ID Société du Contrat(cf.champ[SocieteId] de la table [FRED_CONTRAT_INTERIMAIRE]) fait partie de la liste des ID des Sociétés Participantes identifiés dans la RG précédente.

        [TestMethod]
        [TestCategory("CommandeEnergieLigneService")]
        public void GetGeneratedCommandeEnergieLignes_WhenTypeEnergieInterimaire_ContainsSocieteParticipantes()
        {
            const int ci = 8080;
            socParticipantes = new List<int>() { 1, 2, 3 }; //les societés particpantes
            List<CommandeEnergieLigne> result = Actual.GetGeneratedCommandeEnergieLignes(energie, ci, It.IsAny<DateTime>(), tache, uniteMock.Find(4), socParticipantes);
            result.Should().HaveCount(listContrat.Count - 1);
        }

        //RG_6472_015_01
        //la société ne fait pas partie
        [TestMethod]
        [TestCategory("CommandeEnergieLigneService")]
        public void GetGeneratedCommandeEnergieLignes_WhenTypeEnergieInterimaire_Societenotparticipante()
        {
            const int ci = 8080;
            socParticipantes = new List<int>() { 99 };// la société 99 n'exste pas dans la liste de contrat

            List<CommandeEnergieLigne> result = Actual.GetGeneratedCommandeEnergieLignes(energie, ci, It.IsAny<DateTime>(), tache, uniteMock.Find(4), socParticipantes);
            result.Should().BeNullOrEmpty();
        }

        //RG_6472_015_02
        //-	Le Contrat est lié à au moins une ligne de pointage avec des heures travaillées > 0 
        [TestMethod]
        [TestCategory("CommandeEnergieLigneService")]
        public void GetGeneratedCommandeEnergieLignes_WhenTypeEnergieInterimaireHaveR_AnyRapportLigne_()
        {
            const int ci = 8080;
            socParticipantes = new List<int>() { 3 };// la societe 3 n'a pas de pointage 
            List<CommandeEnergieLigne> result = Actual.GetGeneratedCommandeEnergieLignes(energie, ci, It.IsAny<DateTime>(), tache, uniteMock.Find(4), socParticipantes);
            result.Should().BeNullOrEmpty();
        }

        //RG_6472_015_02
        //et non supprimée logiquement        
        [TestMethod]
        [TestCategory("CommandeEnergieLigneService")]
        public void GetGeneratedCommandeEnergieLignes_WhenTypeEnergieInterimaire_Rapportligne_nosupprimer()
        {
            const int ci = 8080;
            socParticipantes = new List<int>() { 1, 2, 3 };

            List<CommandeEnergieLigne> result = Actual.GetGeneratedCommandeEnergieLignes(energie, ci, It.IsAny<DateTime>(), tache, uniteMock.Find(4), socParticipantes);
            result.Should().NotContain(x => x.Quantite == 10000);
        }
    }
}
