using System.Collections.Generic;
using System.Linq;
using Fred.Common.Tests.Data.Devise.Mock;
using Fred.Common.Tests.Data.FeatureFlipping.Mock;
using Fred.Common.Tests.Data.Fournisseur.Mock;
using Fred.Common.Tests.EntityFramework;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Fred.EntityFramework;
using Fred.Framework.Security;
using Moq;

namespace Fred.Common.Tests.Data.Commande.Mock
{
    /// <summary>
    /// Elements fictifs de SocieteClassification
    /// </summary>
    public class CommandeMocks
    {
        /// <summary>
        /// éléments fictifes des commande statut
        /// </summary>
        private CommandeStatutMocks commandeStatutMock;

        /// <summary>
        /// éléments fictifes des commande type
        /// </summary>
        private CommandeTypeMocks commandeTypeMock;

        /// <summary>
        /// éléments fictifes des devise
        /// </summary>
        private DeviseMocks deviseMock;

        /// <summary>
        /// éléments fictifes des devise
        /// </summary>
        private FournisseurMocks fournisseurMock;

        /// <summary>
        /// éléments fictifes des devise
        /// </summary>
        private FeatureFlippingMocks featureFlippingMocks;

        private List<CommandeEnt> commandeEntStub;
        /// <summary>
        /// obtient ou définit la liste d'entites des classifications société
        /// </summary>
        public List<CommandeEnt> CommandeEntStub
        {
            get { return commandeEntStub ?? (commandeEntStub = GetFakeDbSet().ToList()); }
            set { commandeEntStub = value; }
        }
        
        /// <summary>
        /// ctor
        /// </summary>
        public CommandeMocks()
        {
            commandeStatutMock = new CommandeStatutMocks();
            commandeTypeMock = new CommandeTypeMocks();
            deviseMock = new DeviseMocks();
            fournisseurMock = new FournisseurMocks();
            featureFlippingMocks = new FeatureFlippingMocks();
        }

        /// <summary>
        /// Obtient un unit of work fictif
        /// </summary>
        /// <returns></returns>
        public Mock<IUnitOfWork> GetFakeUow()
        {
            var unitOfWork = new Mock<IUnitOfWork>();

            return unitOfWork;
        }

        public IUnitOfWork GetRealUowWithFakeDbSet(Mock<FredDbContext> fakeContext, Mock<ISecurityManager> securityManager)
        {
            fakeContext.Object.StatutsCommande = commandeStatutMock.GetFakeDbSet();
            fakeContext.Object.Devise = deviseMock.GetFakeDbSet();
            fakeContext.Object.Fournisseurs = fournisseurMock.GetFakeDbSet();
            fakeContext.Object.Commandes = GetFakeDbSet();
            fakeContext.Object.CommandeTypes = commandeTypeMock.GetFakeDbSet();

            var unitOfWork = new UnitOfWork(fakeContext.Object, securityManager.Object);

            return unitOfWork;
        }

        /// <summary>
        /// Obtient un dbset fictif
        /// </summary>
        /// <returns>DbSet de SocieteClassificationEnt</returns>
        public FakeDbSet<CommandeEnt> GetFakeDbSet()
        {
            return new FakeDbSet<CommandeEnt>
            {
                new CommandeEnt()
                {
                    CiId = 1,
                    TypeId = 1,
                    Libelle = "Commande de test",
                    FournisseurId = 1,
                    Numero = "2200001",
                    NumeroCommandeExterne = "Commande1",
                    AuteurCreationId = 1,
                    FraisAmortissement = false,
                    FraisAssurance = false,
                    Carburant = false,
                    Lubrifiant = false,
                    EntretienJournalier = false,
                    EntretienMecanique = false,
                    MOConduite = false,
                    CommandeManuelle = false,
                    ContactId = 1,
                    SuiviId = 1,
                    LivraisonAdresse = "blabla",
                    LivraisonVille = "blabla",
                    LivraisonCPostale = "34000",
                    Contact = new Entities.Personnel.PersonnelEnt() { },
                    FacturationAdresse = "blabla",
                    FacturationVille = "blabla",
                    FacturationCPostale = "34000",
                    Lignes = new List<CommandeLigneEnt>(),
                    DeviseId = 48 // €
                }
            };
        }

        /// <summary>
        /// Permet d'inialiser les éléments fictifs
        /// </summary>
        public void InitialiseMocks()
        {
        }
    }
}

